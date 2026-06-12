using System.Windows;
using FoodyExpress.Views.Pages;

namespace FoodyExpress.Views
{
    public partial class MainWindow : Window
    {
        private int _currentUserId;
        private string _currentUserRole;

        public MainWindow(int userId, string role)
        {
            InitializeComponent();
            _currentUserId = userId;
            _currentUserRole = role;
            
            txtUserInfo.Text = $"User ID: {userId} | Role: {role}";
            
            // Adjust menu based on role
            if (role == "Admin")
            {
                btnCustomerDashboard.Visibility = Visibility.Collapsed;
                btnCustomerMenu.Visibility = Visibility.Collapsed;
                btnOrders.Visibility = Visibility.Collapsed;
                
                MainContent.Content = new AdminDashboard();
            }
            else
            {
                btnAdminDashboard.Visibility = Visibility.Collapsed;
                btnUserManagement.Visibility = Visibility.Collapsed;
                btnRestaurantManagement.Visibility = Visibility.Collapsed;
                btnMenuItemManagement.Visibility = Visibility.Collapsed;
                btnOrderManagement.Visibility = Visibility.Collapsed;

                if (role == "Delivery")
                {
                    btnCustomerMenu.Visibility = Visibility.Collapsed;
                }
                
                MainContent.Content = new DashboardControl();
            }
        }

        private void NavButton_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as System.Windows.Controls.Button;
            if (btn == btnAdminDashboard)
                MainContent.Content = new AdminDashboard();
            else if (btn == btnUserManagement)
                MainContent.Content = new UserManagement();
            else if (btn == btnRestaurantManagement)
                MainContent.Content = new RestaurantManagement();
            else if (btn == btnMenuItemManagement)
                MainContent.Content = new MenuItemManagement();
            else if (btn == btnOrderManagement)
                MainContent.Content = new OrderManagement();
            else if (btn == btnCustomerDashboard)
                MainContent.Content = new DashboardControl();
            else if (btn == btnCustomerMenu)
                MainContent.Content = new MenuControl();
            else if (btn == btnOrders)
                MainContent.Content = new OrdersControl(_currentUserRole, _currentUserId);
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow login = new LoginWindow();
            login.Show();
            this.Close();
        }
    }
}
