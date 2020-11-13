using System.Windows;

namespace TPA_Desktop.Views.UserControls
{
    public partial class Logout
    {
        public Logout()
        {
            InitializeComponent();
        }

        private void HandleLogout(object sender, RoutedEventArgs e)
        {
            new LoginWindow().Show();
            Window.GetWindow(this)?.Close();
        }
    }
}