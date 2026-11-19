using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.EntityFrameworkCore;
using Kolchin_up_02._02.Models;

namespace Kolchin_up_02._02
{
    public class ProductCard //свойства карточки товара 
    {
        public string Article { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public string ManufactureName { get; set; } = string.Empty;
        public string SupplierName { get; set; } = string.Empty;
        public string UnitName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public decimal BasePrice { get; set; }
        public decimal DiscountPrice { get; set; }
        public decimal DiscountPercent { get; set; }
        public int StockQuantity { get; set; }

        public bool HasDiscount => DiscountPercent > 0;
        public Visibility OldPriceVisibility => HasDiscount ? Visibility.Visible : Visibility.Collapsed;

        //подсветка по цене
        public Brush CardBackground
        {
            get
            {
                if (StockQuantity == 0)
                    return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D3D3D3"));

                if (DiscountPercent > 25)
                    return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#23E1EF"));

                return Brushes.White;
            }
        }

        public BitmapImage? ImageSource { get; set; }
    }

    public class MainViewModel
    {
        public ObservableCollection<ProductCard> Products { get; set; } = new();  //для показа изменений

        public MainViewModel()
        {
            LoadProducts();
        }

        public void LoadProducts() //метод загрузки продуктов из бд
        {
            try
            {
                using var db = new DBContext();

                var products = db.Products
                    .Include(p => p.Category)
                    .Include(p => p.Manufacturer)
                    .Include(p => p.Supplier)
                    .Include(p => p.Unit)
                    .OrderBy(p => p.ProductName) //сортировка по имени
                    .ToList();

                var cards = new List<ProductCard>();

                //перебор каждой карточки на дисконт
                foreach (var p in products)
                {
                    decimal finalPrice = p.Discount > 0
                        ? p.Price * (100m - p.Discount) / 100m
                        : p.Price;

                    //заполнение списка карточками
                    cards.Add(new ProductCard
                    {
                        Article = p.Article,
                        Name = p.ProductName,
                        CategoryName = p.Category?.CategoryName ?? "Без категории",
                        ManufactureName = p.Manufacturer?.ManufacturerName ?? "Не указан",
                        SupplierName = p.Supplier?.SupplierName ?? "Не указан",
                        UnitName = p.Unit?.UnitName ?? "шт.",
                        Description = string.IsNullOrWhiteSpace(p.Description) ? "Описание отсутствует" : p.Description,
                        BasePrice = p.Price,
                        DiscountPrice = finalPrice,
                        DiscountPercent = p.Discount,
                        StockQuantity = p.StockQuantity,
                        ImageSource = LoadImage(p.Photo)
                    });
                }

                //для отображения в окне
                Products = new ObservableCollection<ProductCard>(cards);
            }
            catch (Exception ex) //на случай ошибки
            {
                MessageBox.Show(
                    "Не удалось загрузить каталог товаров.\n\n" + ex.Message,
                    "Ошибка базы данных",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private static BitmapImage? LoadImage(string? imagePath) //загрузка картинки
        {
            try
            {
                string? path = null;
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;//путь к директории проекта

                if (!string.IsNullOrWhiteSpace(imagePath)) //если не пустое имя файла
                {
                    if (File.Exists(imagePath))//путь существует тогда используем его
                    {
                        path = imagePath;
                    }
                    else //иначе берем его из папок проекта
                    {
                        string imageName = Path.GetFileName(imagePath);
                        string inImages = Path.Combine(baseDir, "Images", imageName);
                        string inResources = Path.Combine(baseDir, "Resources", imageName);

                        if (File.Exists(inImages))
                            path = inImages;
                        else if (File.Exists(inResources))
                            path = inResources;
                    }
                }

                //заглушка из ресурсов
                if (path == null)
                {
                    string placeholder = Path.Combine(baseDir, "Resources", "picture.png");
                    if (File.Exists(placeholder))
                        path = placeholder;
                }

                if (path == null) //если все равно нет, то просто выводим пустоту
                    return null;

                var bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.CacheOption = BitmapCacheOption.OnLoad;
                bmp.UriSource = new Uri(path, UriKind.Absolute);
                bmp.EndInit();
                bmp.Freeze();

                return bmp;
            }
            catch
            {
                return null;
            }
        }
    }
}
