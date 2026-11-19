using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Kolchin_up_02._02
{
    public partial class ManagerWindow : Window
    {
        private readonly MainViewModel _viewModel; //привязка
        private readonly ICollectionView _productsView;//сортировка

        public ManagerWindow(string fullName)
        {
            InitializeComponent();
            UserFullNameTextBlock.Text = fullName; //вывод имени

            _viewModel = new MainViewModel();//вывод карточек
            DataContext = _viewModel;

            _productsView = CollectionViewSource.GetDefaultView(_viewModel.Products);//сортировка поиск
            _productsView.Filter = FilterProduct;//фильтрация
        }

        private bool FilterProduct(object item) //метод фильтрации
        {
            if (item is not ProductCard product)
                return false; //убеждаемся что точно есть такой продукт

            string searchText = SearchTextBox.Text.Trim(); //берем то что ввели в строку поиска с вычитом проблелов по бокам

            //по каким методам сортируем
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

            return matchesSearch && matchesDiscount; //выполнение обоих дествий сразу только выведет
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)  //каждый раз при измненении обновляем лист
        {
            _productsView?.Refresh();
        }

        private void Filter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_productsView == null || SortComboBox.SelectedItem is not ComboBoxItem sortItem)//защита пока данных нет
                return;

            _productsView.SortDescriptions.Clear();//очистка на всякий случай
            string sortOption = sortItem.Content?.ToString() ?? "";//читаем выбранный вариант сортировки

            switch (sortOption)//методы сортировки
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

            _productsView.Refresh();
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e) 
        {
            var avtoriz = new Avtoriz();
            avtoriz.Show();
            Close();
        }

        private void OrdersButton_Click(object sender, RoutedEventArgs e)
        {
            var ordersWindow = new OrdersWindow(isAdmin: false)
            {
                Owner = this
            };
            ordersWindow.ShowDialog();
        }

    }
}
