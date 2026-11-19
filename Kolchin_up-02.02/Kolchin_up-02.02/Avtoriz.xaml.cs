using Kolchin_up_02._02.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Kolchin_up_02._02
{
    public partial class Avtoriz : Window
    {
        public Avtoriz()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e) //кнопка авторизации
        {
            string login = LoginTextBox.Text.Trim(); //берем логин из поля ввода
            string password = PasswordBox.Password.Trim();//берем пароль из поля ввода

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password)) //если что то из этого пусто - ошибка
            {
                MessageBox.Show(
                    "Пожалуйста, введите логин и пароль",
                    "Авторизация",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            try //пробуем взять данные из базы
            {
                using var db = new DBContext();

                var employee = db.Employees //таблица с сотрудниками
                    .Include(emp => emp.Role) //берем роль
                    .FirstOrDefault(emp => emp.Login == login && emp.Password == password);// логин и пароль сразу с сравнением с textbox

                if (employee == null) //если пусто, тогда ошибка
                {
                    MessageBox.Show(
                        "Неверный логин или пароль",
                        "Ошибка авторизации",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return;
                }

                string fullName = $"{employee.LastName} {employee.FirstName} {employee.MiddleName}".Trim(); //вывод имени
                string roleName = employee.Role.RoleName; //вывод роли

                MessageBox.Show(
                    $"Добро пожаловать, {fullName}!\nВаша роль: {roleName}",
                    "Успешный вход",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information); //сообщение об успешном вводе

                Window nextWindow = roleName switch //выбор окна взависимости от роли пользователя
                {
                    "Администратор" => new AdminWindow(fullName),
                    "Менеджер" => new ManagerWindow(fullName),
                    _ => new ClientWindow(fullName)
                };

                nextWindow.Show();
                Close();
            }
            catch (Exception ex) //ошибка для того чтобы не упала программа
            {
                MessageBox.Show(
                    "Ошибка подключения к базе данных:\n\n" + ex.Message,
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void GuestButton_Click(object sender, RoutedEventArgs e) //гостевая кнопка
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }
    }
}
