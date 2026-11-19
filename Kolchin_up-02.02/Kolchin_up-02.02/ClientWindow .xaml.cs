using System.Windows;

namespace Kolchin_up_02._02
{
    public partial class ClientWindow : Window
    {
        public ClientWindow(string fullName)
        {
            InitializeComponent();
            UserFullNameTextBlock.Text = fullName; //подпись клиента
            DataContext = new MainViewModel();//вывод карточек
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e) //кнопка выхода
        {
            var avtoriz = new Avtoriz();
            avtoriz.Show();
            Close();
        }
    }
}
