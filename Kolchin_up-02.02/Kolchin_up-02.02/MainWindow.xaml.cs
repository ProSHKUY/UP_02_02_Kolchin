using System.Windows;

namespace Kolchin_up_02._02
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel(); //вывод карточек
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e) //кнопка выхода
        {
            Avtoriz avtoriz = new Avtoriz();
            avtoriz.Show();
            Close();
        }
    }
}
