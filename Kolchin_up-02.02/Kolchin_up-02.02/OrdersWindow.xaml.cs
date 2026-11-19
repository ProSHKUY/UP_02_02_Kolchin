using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using Kolchin_up_02._02.Models;

namespace Kolchin_up_02._02
{
    // Модель отображения карточки заказа по макету ТЗ (стр. 8)
    public class OrderCard
    {
        public int OrderId { get; set; }
        public string ArticlesSummary { get; set; } = string.Empty;
        public string StatusName { get; set; } = string.Empty;
        public string PickupAddress { get; set; } = string.Empty;
        public string OrderDateText { get; set; } = string.Empty;
        public string DeliveryDateText { get; set; } = string.Empty;
        public Visibility DeleteButtonVisibility { get; set; } = Visibility.Collapsed;
    }

    public partial class OrdersWindow : Window
    {
        private readonly bool _isAdmin;
        private OrderEditWindow? _activeOrderEditWindow;

        public OrdersWindow(bool isAdmin)
        {
            InitializeComponent();
            _isAdmin = isAdmin;

            if (_isAdmin)
            {
                Title = "ООО «ЧитайГород» — Управление заказами (Администратор)";
                AddOrderButton.Visibility = Visibility.Visible;
            }
            else
            {
                Title = "ООО «ЧитайГород» — Просмотр заказов (Менеджер)";
            }

            LoadOrders();
        }

        public void LoadOrders()
        {
            try
            {
                using var db = new DBContext();

                var orders = db.Orders
                    .Include(o => o.Status)
                    .Include(o => o.PickupPoint)
                    .Include(o => o.OrderItems)
                    .OrderBy(o => o.OrderId)
                    .ToList();

                var cards = new List<OrderCard>();

                foreach (var o in orders)
                {
                    // Собираем строку состава заказа вида "А112Т4, 2, G843H5, 2"
                    string articles = o.OrderItems.Count > 0
                        ? string.Join(", ", o.OrderItems.Select(oi => $"{oi.ProductArticle}, {oi.Quantity}"))
                        : $"Заказ №{o.OrderId} (нет позиций)";

                    cards.Add(new OrderCard
                    {
                        OrderId = o.OrderId,
                        ArticlesSummary = articles,
                        StatusName = o.Status?.StatusName ?? "Не указан",
                        PickupAddress = o.PickupPoint?.Address ?? "Не указан",
                        OrderDateText = o.OrderDate.ToString("dd.MM.yyyy"),
                        DeliveryDateText = o.DeliveryDate.HasValue ? o.DeliveryDate.Value.ToString("dd.MM.yyyy") : "Не назначена",
                        DeleteButtonVisibility = _isAdmin ? Visibility.Visible : Visibility.Collapsed
                    });
                }

                OrdersItemsControl.ItemsSource = cards;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось загрузить список заказов:\n" + ex.Message, "Ошибка БД", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OpenOrderEditWindow(int? orderId)
        {
            // Менеджер может только просматривать заказы (редактирование запрещено по ТЗ)
            if (!_isAdmin)
                return;

            if (_activeOrderEditWindow != null)
            {
                _activeOrderEditWindow.Activate();
                MessageBox.Show("Окно редактирования или добавления заказа уже открыто.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            _activeOrderEditWindow = new OrderEditWindow(orderId)
            {
                Owner = this
            };

            bool? result = _activeOrderEditWindow.ShowDialog();
            _activeOrderEditWindow = null;

            if (result == true)
            {
                LoadOrders();
            }
        }

        private void AddOrderButton_Click(object sender, RoutedEventArgs e)
        {
            OpenOrderEditWindow(null);
        }

        private void OrderCard_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!_isAdmin)
                return;

            if (sender is FrameworkElement element && element.DataContext is OrderCard card)
            {
                OpenOrderEditWindow(card.OrderId);
            }
        }

        private void DeleteOrderButton_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;

            if (!_isAdmin || sender is not Button btn || btn.Tag is not OrderCard card)
                return;

            var confirm = MessageBox.Show(
                $"Вы действительно хотите удалить заказ №{card.OrderId} ({card.ArticlesSummary})?\nЭта операция необратима.",
                "Подтверждение удаления заказа",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (confirm != MessageBoxResult.Yes)
                return;

            try
            {
                using var db = new DBContext();

                var order = db.Orders
                    .Include(o => o.Status)
                    .Include(o => o.OrderItems)
                    .FirstOrDefault(o => o.OrderId == card.OrderId);

                if (order != null)
                {
                    // Возвращаем книги на склад ТОЛЬКО если статус заказа называется "Новый"
                    string statusText = order.Status?.StatusName?.Trim() ?? string.Empty;
                    bool isNewOrder = statusText.Equals("Новый", StringComparison.OrdinalIgnoreCase);

                    if (isNewOrder)
                    {
                        foreach (var item in order.OrderItems)
                        {
                            var prod = db.Products.FirstOrDefault(p => p.Article == item.ProductArticle);
                            if (prod != null)
                            {
                                prod.StockQuantity += item.Quantity;
                            }
                        }
                    }

                    db.OrderItems.RemoveRange(order.OrderItems);
                    db.Orders.Remove(order);
                    db.SaveChanges();

                    string extraInfo = isNewOrder
                        ? "\nТовары из заказа возвращены на склад."
                        : "\nЗаказ уже был завершён, остатки на складе не изменялись.";

                    MessageBox.Show("Заказ успешно удалён." + extraInfo, "Удаление", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadOrders();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при удалении заказа:\n" + ex.Message, "Ошибка БД", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
