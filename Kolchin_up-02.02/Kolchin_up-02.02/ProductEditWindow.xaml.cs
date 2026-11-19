
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using Kolchin_up_02._02.Models;

namespace Kolchin_up_02._02
{
    public partial class ProductEditWindow : Window
    {
        // Если _editingArticle == null, окно работает в режиме добавления.
        // Если _editingArticle != null, окно работает в режиме редактирования.
        private readonly string? _editingArticle;

        // Имя старого файла фотографии из БД.
        private string? _oldPhotoFileName;

        // Полный путь к новому выбранному файлу.
        private string? _selectedNewPhotoSourcePath;

        // Конструктор окна.
        public ProductEditWindow(string? article = null)
        {
            InitializeComponent();

            _editingArticle = article;

            LoadDictionaries();

            if (_editingArticle == null)
            {
                // Режим добавления.
                Title = "ООО «ЧитайГород» — Добавление товара";
                WindowHeaderTextBlock.Text = "Добавление нового товара";

                LoadPreviewImage(null);
            }
            else
            {
                // Режим редактирования.
                Title = "ООО «ЧитайГород» — Редактирование товара";
                WindowHeaderTextBlock.Text = "Редактирование товара";

                // Артикул нельзя изменять, так как это первичный ключ.
                ArticleTextBox.IsReadOnly = true;
                ArticleTextBox.Background =
                    System.Windows.Media.Brushes.LightGray;

                LoadProductData(_editingArticle);
            }
        }

        // Загрузка справочников из базы данных.
        private void LoadDictionaries()
        {
            try
            {
                using var db = new DBContext();

                CategoryComboBox.ItemsSource = db.Categories
                    .OrderBy(c => c.CategoryName)
                    .ToList();

                ManufacturerComboBox.ItemsSource = db.Manufacturers
                    .OrderBy(m => m.ManufacturerName)
                    .ToList();

                SupplierComboBox.ItemsSource = db.Suppliers
                    .OrderBy(s => s.SupplierName)
                    .ToList();

                UnitComboBox.ItemsSource = db.Units
                    .OrderBy(u => u.UnitName)
                    .ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Не удалось загрузить справочники:\n" + ex.Message,
                    "Ошибка БД",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        // Загрузка данных товара для редактирования.
        private void LoadProductData(string article)
        {
            try
            {
                using var db = new DBContext();

                var product = db.Products
                    .FirstOrDefault(p => p.Article == article);

                if (product == null)
                {
                    MessageBox.Show(
                        "Товар не найден в базе данных.",
                        "Ошибка",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);

                    Close();
                    return;
                }

                ArticleTextBox.Text = product.Article;
                NameTextBox.Text = product.ProductName;

                CategoryComboBox.SelectedValue = product.CategoryId;
                ManufacturerComboBox.SelectedValue = product.ManufacturerId;
                SupplierComboBox.SelectedValue = product.SupplierId;
                UnitComboBox.SelectedValue = product.UnitId;

                PriceTextBox.Text = product.Price.ToString(
                    "0.##",
                    CultureInfo.CurrentCulture);

                StockTextBox.Text = product.StockQuantity.ToString();

                DiscountTextBox.Text = product.Discount.ToString(
                    "0.##",
                    CultureInfo.CurrentCulture);

                DescriptionTextBox.Text =
                    product.Description ?? string.Empty;

                _oldPhotoFileName = product.Photo;

                PhotoFileNameTextBlock.Text =
                    string.IsNullOrWhiteSpace(_oldPhotoFileName)
                    ? "Используется заглушка"
                    : $"Текущий файл: {_oldPhotoFileName}";

                LoadPreviewImage(_oldPhotoFileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Ошибка загрузки данных товара:\n" + ex.Message,
                    "Ошибка БД",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        // Отображение фотографии или изображения-заглушки.
        private void LoadPreviewImage(string? fileNameOrFullPath)
        {
            try
            {
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string? targetPath = null;

                if (!string.IsNullOrWhiteSpace(fileNameOrFullPath))
                {
                    if (File.Exists(fileNameOrFullPath))
                    {
                        targetPath = fileNameOrFullPath;
                    }
                    else
                    {
                        string inImages = Path.Combine(
                            baseDir,
                            "Images",
                            Path.GetFileName(fileNameOrFullPath));

                        if (File.Exists(inImages))
                            targetPath = inImages;
                    }
                }

                if (targetPath == null)
                {
                    string placeholder = Path.Combine(
                        baseDir,
                        "Resources",
                        "picture.png");

                    if (File.Exists(placeholder))
                        targetPath = placeholder;
                }

                if (targetPath != null)
                {
                    var bmp = new BitmapImage();

                    bmp.BeginInit();
                    bmp.CacheOption = BitmapCacheOption.OnLoad;
                    bmp.UriSource = new Uri(
                        targetPath,
                        UriKind.Absolute);
                    bmp.EndInit();
                    bmp.Freeze();

                    ProductPreviewImage.Source = bmp;
                }
            }
            catch
            {
                // Ошибку предпросмотра пропускаем,
                // если файл изображения повреждён.
            }
        }

        // Выбор новой фотографии товара.
        private void SelectPhotoButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Title = "Выберите изображение товара (не более 300×200 пикселей)",
                Filter = "Изображения (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png"
            };

            if (dialog.ShowDialog() != true)
                return;

            try
            {
                var checkBmp = new BitmapImage();

                checkBmp.BeginInit();
                checkBmp.CacheOption = BitmapCacheOption.OnLoad;
                checkBmp.UriSource = new Uri(
                    dialog.FileName,
                    UriKind.Absolute);
                checkBmp.EndInit();

                bool isHorizontalValid = checkBmp.PixelWidth <= 300 && checkBmp.PixelHeight <= 200;
                bool isVerticalValid = checkBmp.PixelWidth <= 200 && checkBmp.PixelHeight <= 300;

                if (!isHorizontalValid && !isVerticalValid)
                {
                    MessageBox.Show(
                        $"Размер выбранного изображения ({checkBmp.PixelWidth}×{checkBmp.PixelHeight} пикс.) превышает допустимый предел 300×200 (или 200×300 для вертикальных обложек).\nВыберите другое изображение.",
                        "Ограничение размера фото",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    return;
                }

                _selectedNewPhotoSourcePath = dialog.FileName;

                PhotoFileNameTextBlock.Text =
                    $"Выбран файл: {Path.GetFileName(_selectedNewPhotoSourcePath)}";

                ProductPreviewImage.Source = checkBmp;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Не удалось прочитать файл изображения:\n" + ex.Message,
                    "Ошибка файла",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        // Сохранение нового или отредактированного товара.
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // 1. Проверка обязательных полей.
            string article = ArticleTextBox.Text.Trim();
            string name = NameTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(article) ||
                string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show(
                    "Заполните обязательные поля: Артикул и Наименование товара.",
                    "Проверка данных",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            if (CategoryComboBox.SelectedValue == null ||
                ManufacturerComboBox.SelectedValue == null ||
                SupplierComboBox.SelectedValue == null ||
                UnitComboBox.SelectedValue == null)
            {
                MessageBox.Show(
                    "Выберите Категорию, Производителя, Поставщика и Единицу измерения из списков.",
                    "Проверка данных",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            // 2. Проверка цены.
            string priceRaw = PriceTextBox.Text.Trim().Replace(',', '.');

            if (!decimal.TryParse(
                    priceRaw,
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out decimal price) ||
                price < 0)
            {
                MessageBox.Show(
                    "Цена должна быть неотрицательным числом (например, 450.50).",
                    "Ошибка ввода цены",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            // Проверка количества на складе.
            if (!int.TryParse(
                    StockTextBox.Text.Trim(),
                    out int stock) ||
                stock < 0)
            {
                MessageBox.Show(
                    "Количество на складе должно быть целым неотрицательным числом.",
                    "Ошибка ввода количества",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            // Проверка скидки.
            string discountRaw =
                DiscountTextBox.Text.Trim().Replace(',', '.');

            if (!decimal.TryParse(
                    discountRaw,
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out decimal discount) ||
                discount < 0 ||
                discount > 100)
            {
                MessageBox.Show(
                    "Действующая скидка должна быть числом от 0 до 100.",
                    "Ошибка ввода скидки",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            try
            {
                using var db = new DBContext();

                Product? product;

                if (_editingArticle == null)
                {
                    // Проверка уникальности артикула.
                    if (db.Products.Any(p => p.Article == article))
                    {
                        MessageBox.Show(
                            $"Товар с артикулом «{article}» уже существует в базе данных.",
                            "Дубликат артикула",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning);

                        return;
                    }

                    product = new Product
                    {
                        Article = article
                    };

                    db.Products.Add(product);
                }
                else
                {
                    product = db.Products.FirstOrDefault(
                        p => p.Article == _editingArticle);

                    if (product == null)
                    {
                        MessageBox.Show(
                            "Редактируемый товар не найден в базе.",
                            "Ошибка",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);

                        return;
                    }
                }

                // 3. Сохранение новой фотографии.
                if (!string.IsNullOrWhiteSpace(
                        _selectedNewPhotoSourcePath))
                {
                    string imagesFolder = Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory,
                        "Images");

                    Directory.CreateDirectory(imagesFolder);

                    // Создаём уникальное имя файла.
                    string extension =
                        Path.GetExtension(_selectedNewPhotoSourcePath);

                    string newFileName =
                        $"{article}_{DateTime.Now:yyyyMMddHHmmss}{extension}";

                    string destPath = Path.Combine(
                        imagesFolder,
                        newFileName);

                    File.Copy(
                        _selectedNewPhotoSourcePath,
                        destPath,
                        true);

                    // Удаляем старую фотографию, если она была.
                    if (!string.IsNullOrWhiteSpace(_oldPhotoFileName) &&
                        !_oldPhotoFileName.Equals(
                            "picture.png",
                            StringComparison.OrdinalIgnoreCase))
                    {
                        string oldFullPath = Path.Combine(
                            imagesFolder,
                            Path.GetFileName(_oldPhotoFileName));

                        if (File.Exists(oldFullPath))
                        {
                            try
                            {
                                File.Delete(oldFullPath);
                            }
                            catch
                            {
                                // Если файл занят, пропускаем удаление.
                            }
                        }
                    }

                    product.Photo = newFileName;
                }

                // 4. Запись значений в сущность Product.
                product.ProductName = name;
                product.CategoryId = (int)CategoryComboBox.SelectedValue;
                product.ManufacturerId = (int)ManufacturerComboBox.SelectedValue;
                product.SupplierId = (int)SupplierComboBox.SelectedValue;
                product.UnitId = (int)UnitComboBox.SelectedValue;

                product.Price = Math.Round(price, 2);
                product.StockQuantity = stock;
                product.Discount = Math.Round(discount, 2);

                product.Description =
                    string.IsNullOrWhiteSpace(DescriptionTextBox.Text)
                    ? null
                    : DescriptionTextBox.Text.Trim();

                db.SaveChanges();

                MessageBox.Show(
                    "Данные товара успешно сохранены!",
                    "Сохранение",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Ошибка при сохранении товара в базу данных:\n" +
                    ex.Message,
                    "Ошибка БД",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        // Отмена операции.
        private void CancelButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            Close();
        }
    }
}
