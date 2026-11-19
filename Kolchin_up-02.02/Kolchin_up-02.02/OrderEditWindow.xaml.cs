using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using Kolchin_up_02._02.Models;

namespace Kolchin_up_02._02
{
    // Вспомогательный класс для отображения строк в таблице OrderItemsDataGrid
    public class OrderItemRow
    {
        public string ProductArticle { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }

    public partial class OrderEditWindow : Window
    {
        private readonly int? _editingOrderId;
        private readonly ObservableCollection<OrderItemRow> _orderItems = new();

        public OrderEditWindow(int? orderId = null)
        {
            InitializeComponent();
            _editingOrderId = orderId;

            // Привязываем коллекцию позиций заказа к DataGrid из твоего XAML
            OrderItemsDataGrid.ItemsSource = _orderItems;

            // Загружаем справочники (статусы, пункты выдачи, товары)
            LoadDictionaries();

            if (_editingOrderId.HasValue)
            {
                Title = "ООО «ЧитайГород» — Редактирование заказа";
                WindowHeaderTextBlock.Text = $"Редактирование заказа №{_editingOrderId.Value}";
                LoadOrderData(_editingOrderId.Value);
            }
            else
            {
                Title = "ООО «ЧитайГород» — Добавление заказа";
                WindowHeaderTextBlock.Text = "Добавление нового заказа";
                OrderDatePicker.SelectedDate = DateTime.Today;
                DeliveryDatePicker.SelectedDate = DateTime.Today.AddDays(3);
            }
        }

        // Загрузка списков статусов, пунктов выдачи и книг из БД
        private void LoadDictionaries()
        {
            try
            {
                using var db = new DBContext();

                StatusComboBox.ItemsSource = db.OrderStatuses
                    .OrderBy(s => s.StatusId)
                    .ToList();

                PickupPointComboBox.ItemsSource = db.PickupPoints
                    .OrderBy(p => p.PickupPointId)
                    .ToList();

                var products = db.Products
                    .OrderBy(p => p.ProductName)
                    .ToList();

                ProductComboBox.ItemsSource = products;
                if (products.Count > 0)
                    ProductComboBox.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Не удалось загрузить справочники для заказа:\n" + ex.Message,
                    "Ошибка БД",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        // Загрузка существующего заказа при редактировании
        private void LoadOrderData(int orderId)
        {
            try
            {
                using var db = new DBContext();

                var order = db.Orders
                    .Include(o => o.OrderItems)
                        .ThenInclude(oi => oi.ProductArticleNavigation)
                    .FirstOrDefault(o => o.OrderId == orderId);

                if (order == null)
                {
                    MessageBox.Show("Заказ не найден в базе данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    Close();
                    return;
                }

                OrderDatePicker.SelectedDate = order.OrderDate.ToDateTime(TimeOnly.MinValue);

                if (order.DeliveryDate.HasValue)
                    DeliveryDatePicker.SelectedDate = order.DeliveryDate.Value.ToDateTime(TimeOnly.MinValue);

                StatusComboBox.SelectedValue = order.StatusId;
                PickupPointComboBox.SelectedValue = order.PickupPointId;

                _orderItems.Clear();
                foreach (var item in order.OrderItems)
                {
                    _orderItems.Add(new OrderItemRow
                    {
                        ProductArticle = item.ProductArticle,
                        ProductName = item.ProductArticleNavigation?.ProductName ?? item.ProductArticle,
                        Quantity = item.Quantity
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Ошибка при загрузке данных заказа:\n" + ex.Message,
                    "Ошибка БД",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        // Кнопка "Добавить" (добавляет выбранную книгу в таблицу состава заказа)
        private void AddItemButton_Click(object sender, RoutedEventArgs e)
        {
            if (ProductComboBox.SelectedItem is not Product selectedProduct)
            {
                MessageBox.Show("Выберите товар из списка.", "Проверка данных", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(QuantityTextBox.Text.Trim(), out int quantity) || quantity <= 0)
            {
                MessageBox.Show("Количество товара должно быть целым положительным числом.", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using var db = new DBContext();

                // Берём актуальный остаток на складе из БД
                var dbProduct = db.Products.FirstOrDefault(p => p.Article == selectedProduct.Article);
                int availableStock = dbProduct?.StockQuantity ?? selectedProduct.StockQuantity;

                // Если мы редактируем уже существующий заказ со статусом "Новый", прибавляем к остатку его текущее количество в БД
                if (_editingOrderId.HasValue)
                {
                    var oldOrder = db.Orders
                        .Include(o => o.Status)
                        .Include(o => o.OrderItems)
                        .FirstOrDefault(o => o.OrderId == _editingOrderId.Value);

                    if (oldOrder != null && oldOrder.Status.StatusName.Equals("Новый", StringComparison.OrdinalIgnoreCase))
                    {
                        int oldQtyInThisOrder = oldOrder.OrderItems
                            .Where(oi => oi.ProductArticle == selectedProduct.Article)
                            .Sum(oi => oi.Quantity);

                        availableStock += oldQtyInThisOrder;
                    }
                }

                var existingRow = _orderItems.FirstOrDefault(r => r.ProductArticle == selectedProduct.Article);
                int alreadyInTable = existingRow?.Quantity ?? 0;
                int totalRequested = alreadyInTable + quantity;

                if (totalRequested > availableStock)
                {
                    MessageBox.Show(
                        $"Недостаточно товара «{selectedProduct.ProductName}» на складе.\n" +
                        $"Доступно на складе: {availableStock} шт., запрошено в заказ: {totalRequested} шт.",
                        "Превышение доступного остатка",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    return;
                }

                if (existingRow != null)
                {
                    existingRow.Quantity = totalRequested;
                    OrderItemsDataGrid.Items.Refresh();
                }
                else
                {
                    _orderItems.Add(new OrderItemRow
                    {
                        ProductArticle = selectedProduct.Article,
                        ProductName = selectedProduct.ProductName,
                        Quantity = quantity
                    });
                }

                QuantityTextBox.Text = "1";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при проверке остатка товара:\n" + ex.Message, "Ошибка БД", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Кнопка "Удалить выбранный товар" из таблицы состава заказа
        private void RemoveItemButton_Click(object sender, RoutedEventArgs e)
        {
            if (OrderItemsDataGrid.SelectedItem is not OrderItemRow selectedRow)
            {
                MessageBox.Show("Выберите строку товара в таблице для удаления.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            _orderItems.Remove(selectedRow);
        }

        // Кнопка "Сохранить" (записывает заказ и его состав в PostgreSQL)
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (OrderDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Укажите дату заказа.", "Проверка данных", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (DeliveryDatePicker.SelectedDate != null &&
                DeliveryDatePicker.SelectedDate.Value.Date < OrderDatePicker.SelectedDate.Value.Date)
            {
                MessageBox.Show("Дата доставки не может быть раньше даты заказа.", "Ошибка дат", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (StatusComboBox.SelectedValue == null || PickupPointComboBox.SelectedValue == null)
            {
                MessageBox.Show("Выберите статус заказа и пункт выдачи.", "Проверка данных", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_orderItems.Count == 0)
            {
                MessageBox.Show("Добавьте хотя бы один товар в состав заказа.", "Проверка данных", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using var db = new DBContext();
                Order? order;

                if (_editingOrderId.HasValue)
                {
                    order = db.Orders
                        .Include(o => o.OrderItems)
                        .FirstOrDefault(o => o.OrderId == _editingOrderId.Value);

                    if (order == null)
                    {
                        MessageBox.Show("Заказ не найден в базе данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    // 1. Возвращаем на склад товары из старого состава редактируемого заказа
                    foreach (var oldItem in order.OrderItems)
                    {
                        var prod = db.Products.FirstOrDefault(p => p.Article == oldItem.ProductArticle);
                        if (prod != null)
                        {
                            prod.StockQuantity += oldItem.Quantity;
                        }
                    }

                    db.OrderItems.RemoveRange(order.OrderItems);
                }
                else
                {
                    int nextPickupCode = (db.Orders.Max(o => (int?)o.PickupCode) ?? 900) + 1;

                    order = new Order
                    {
                        PickupCode = nextPickupCode
                    };
                    db.Orders.Add(order);
                }

                order.OrderDate = DateOnly.FromDateTime(OrderDatePicker.SelectedDate.Value);
                order.DeliveryDate = DeliveryDatePicker.SelectedDate.HasValue
                    ? DateOnly.FromDateTime(DeliveryDatePicker.SelectedDate.Value)
                    : null;
                order.StatusId = (int)StatusComboBox.SelectedValue;
                order.PickupPointId = (int)PickupPointComboBox.SelectedValue;

                db.SaveChanges();

                // 2. Записываем новые позиции заказа и вычитаем их количество со склада
                foreach (var row in _orderItems)
                {
                    var prod = db.Products.FirstOrDefault(p => p.Article == row.ProductArticle);
                    if (prod != null)
                    {
                        if (prod.StockQuantity < row.Quantity)
                        {
                            MessageBox.Show($"Недостаточно товара «{prod.ProductName}» на складе (доступно: {prod.StockQuantity} шт.).", "Ошибка остатка", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }

                        prod.StockQuantity -= row.Quantity;
                    }

                    db.OrderItems.Add(new OrderItem
                    {
                        OrderId = order.OrderId,
                        ProductArticle = row.ProductArticle,
                        Quantity = row.Quantity
                    });
                }

                db.SaveChanges();

                MessageBox.Show("Заказ успешно сохранён, остатки на складе обновлены!", "Сохранение", MessageBoxButton.OK, MessageBoxImage.Information);
                try { DialogResult = true; } catch { }
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении заказа в базу данных:\n" + ex.Message, "Ошибка БД", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        // Кнопка "Отмена"
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            try { DialogResult = false; } catch { }
            Close();
        }
    }
}