using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.IO;
using System.Linq;
using Kolchin_up_02._02.Models;
using System.Windows.Input;


namespace Kolchin_up_02._02
{
    public partial class AdminWindow : Window
    {
        private  MainViewModel _viewModel; //привязка к основному методу карточек
        private  ICollectionView _productsView;

        private ProductEditWindow? _activeEditWindow;

        public AdminWindow(string fullName)
        {
            InitializeComponent();
            UserFullNameTextBlock.Text = fullName; //выводим фио пользователя

            _viewModel = new MainViewModel(); //привязки
            DataContext = _viewModel;

            
            _productsView = CollectionViewSource.GetDefaultView(_viewModel.Products);//для управления поиском и сортировкой
            _productsView.Filter = FilterProduct;//фильтрация
        }

        private bool FilterProduct(object item) //метод фильтрации
        {
            if (item is not ProductCard product)
                return false;

            string searchText = SearchTextBox.Text.Trim();//текст который ввел пользователь и убираем возможные пробелы по краям

            //проверка поиска
            bool matchesSearch = string.IsNullOrWhiteSpace(searchText)
                || product.Article.Contains(searchText, StringComparison.OrdinalIgnoreCase)
                || product.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase)
                || product.Description.Contains(searchText, StringComparison.OrdinalIgnoreCase)
                || product.CategoryName.Contains(searchText, StringComparison.OrdinalIgnoreCase)
                || product.ManufactureName.Contains(searchText, StringComparison.OrdinalIgnoreCase)
                || product.SupplierName.Contains(searchText, StringComparison.OrdinalIgnoreCase)
                || product.UnitName.Contains(searchText, StringComparison.OrdinalIgnoreCase);

            //из выпадающего списка фильтра по скидке
            string discountOption = DiscountFilterComboBox.SelectedItem is ComboBoxItem dItem
                ? dItem.Content?.ToString() ?? "Все диапазоны"
                : "Все диапазоны";

            // попадает ли процент в выбранный диапазон
            bool matchesDiscount = discountOption switch
            {
                "0-12,99%" => product.DiscountPercent >= 0m && product.DiscountPercent <= 12.99m,
                "13-16,99%" => product.DiscountPercent >= 13m && product.DiscountPercent <= 16.99m,
                "17% и более" => product.DiscountPercent >= 17m,
                _ => true
            };

            return matchesSearch && matchesDiscount; //выполнение обоих действий сразу только выведет
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e) //каждый раз при измненении обновляем лист
        {
            _productsView?.Refresh();
        }

        private void Filter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_productsView == null || SortComboBox.SelectedItem is not ComboBoxItem sortItem) //защита пока данных нет
                return;

            _productsView.SortDescriptions.Clear(); //очистка на всякий случай
            string sortOption = sortItem.Content?.ToString() ?? "";//читаем выбранный вариант сортировки

            switch (sortOption) //методы сортировки
            {
                case "Цена: по возрастанию":
                    _productsView.SortDescriptions.Add(new SortDescription(nameof(ProductCard.DiscountPrice), ListSortDirection.Ascending));
                    break;
                case "Цена: по убыванию":
                    _productsView.SortDescriptions.Add(new SortDescription(nameof(ProductCard.DiscountPrice), ListSortDirection.Descending));
                    break;
                case "Остаток: по возрастанию":
                    _productsView.SortDescriptions.Add(new SortDescription(nameof(ProductCard.StockQuantity), ListSortDirection.Ascending));
                    break;
                case "Остаток: по убыванию":
                    _productsView.SortDescriptions.Add(new SortDescription(nameof(ProductCard.StockQuantity), ListSortDirection.Descending));
                    break;
            }

            _productsView.Refresh();//обновляем
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e) //кнопка выхода
        {
            var avtoriz = new Avtoriz();
            avtoriz.Show();
            Close();
        }

        private void OpenEditWindow(string? article)
        {
            if (_activeEditWindow != null)
            {
                _activeEditWindow.Activate();
                MessageBox.Show("Окно добавления или редактирования товара уже открыто.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            _activeEditWindow = new ProductEditWindow(article)
            {
                Owner = this
            };

            bool? result = _activeEditWindow.ShowDialog();
            _activeEditWindow = null;

            if (result == true)
            {
                _viewModel = new MainViewModel();
                DataContext = _viewModel;
                _productsView = CollectionViewSource.GetDefaultView(_viewModel.Products);
                _productsView.Filter = FilterProduct;
                Filter_SelectionChanged(null, null);
            }
        }

        private void AddProductButton_Click(object sender, RoutedEventArgs e)
        {
            var add = new ProductEditWindow();
            if (add.ShowDialog() == true)
            {
                _viewModel = new MainViewModel();
                DataContext = _viewModel;
                _productsView = CollectionViewSource.GetDefaultView(_viewModel.Products);
                _productsView.Filter = FilterProduct;
            }
        }

        private void ProductCard_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement element && element.DataContext is ProductCard card)
            {
                OpenEditWindow(card.Article);
            }
        }


        private void DeleteProductButton_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;

            if (sender is not Button btn || btn.Tag is not ProductCard card)
                return;

            var confirm = MessageBox.Show(
                $"Вы действительно хотите удалить книгу «{card.Name}» (артикул: {card.Article})?\nЭта операция необратима.",
                "Подтверждение удаления",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (confirm != MessageBoxResult.Yes)
                return;

            try
            {
                using var db = new DBContext();

                // Товар, который присутствует в заказе, удалить нельзя (требование ТЗ)
                if (db.OrderItems.Any(oi => oi.ProductArticle == card.Article))
                {
                    MessageBox.Show(
                        $"Невозможно удалить товар «{card.Name}», так как он присутствует в оформленных заказах.",
                        "Удаление запрещено",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return;
                }

                var product = db.Products.FirstOrDefault(p => p.Article == card.Article);
                if (product == null)
                {
                    MessageBox.Show("Товар не найден в базе данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Удаляем файл фото товара с диска, если он был загружен
                if (!string.IsNullOrWhiteSpace(product.Photo) && !product.Photo.Equals("picture.png", StringComparison.OrdinalIgnoreCase))
                {
                    string photoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", Path.GetFileName(product.Photo));
                    if (File.Exists(photoPath))
                    {
                        try { File.Delete(photoPath); } catch { }
                    }
                }

                db.Products.Remove(product);
                db.SaveChanges();

                MessageBox.Show("Товар успешно удалён.", "Удаление выполнено", MessageBoxButton.OK, MessageBoxImage.Information);

                // Обновляем список товаров на экране
                _viewModel = new MainViewModel();
                DataContext = _viewModel;
                _productsView = CollectionViewSource.GetDefaultView(_viewModel.Products);
                _productsView.Filter = FilterProduct;
                Filter_SelectionChanged(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при удалении товара из базы данных:\n" + ex.Message, "Ошибка БД", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OrdersButton_Click(object sender, RoutedEventArgs e)
        {
            var ordersWindow = new OrdersWindow(isAdmin: true)
            {
                Owner = this
            };

            // ShowDialog() ждёт, пока ты не закроешь окно заказов кнопкой "Назад" или крестиком
            ordersWindow.ShowDialog();

            // Как только окно заказов закрылось — сразу обновляем каталог книг на экране
            _viewModel = new MainViewModel();
            DataContext = _viewModel;
            _productsView = CollectionViewSource.GetDefaultView(_viewModel.Products);
            _productsView.Filter = FilterProduct;
            Filter_SelectionChanged(null, null);
            _productsView.Refresh();
        }
    } 
} 