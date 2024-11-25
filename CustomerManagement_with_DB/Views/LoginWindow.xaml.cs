using System;
using System.Windows;
using CustomerManagement_with_DB;
using CustomerManagement_with_DB.Controllers;
using MySql.Data.MySqlClient;

namespace Invoice_Management
{
    public partial class LoginWindow : Window
    {
        private DBConnector dbConnector = new DBConnector();

        public LoginWindow()
        {
            InitializeComponent();
            if (!dbConnector.TestConnection())
            {
                MessageBox.Show("Datenbankverbindung fehlgeschlagen. Bitte prüfen Sie Ihre Einstellungen.", "Verbindungsfehler");
                Application.Current.Shutdown(); // Programm beenden
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordTextBox.Password;

            // Aufruf der Validierungslogik
            if (!Validation.ValidateLogin(username, password))
            {
                return; // Validierung fehlgeschlagen, daher beenden wir die Methode
            }

            if (dbConnector.CheckLogin(username, password))
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Passwort oder Benutzername ungültig. Bitte versuchen Sie es erneut.", "Login fehlgeschlagen");
            }
        }

        private void ShowPasswordCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            PasswordTextBoxVisible.Text = PasswordTextBox.Password;
            PasswordTextBoxVisible.Visibility = Visibility.Visible;
            PasswordTextBox.Visibility = Visibility.Collapsed;
        }

        private void ShowPasswordCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            PasswordTextBox.Password = PasswordTextBoxVisible.Text;
            PasswordTextBox.Visibility = Visibility.Visible;
            PasswordTextBoxVisible.Visibility = Visibility.Collapsed;
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow registerWindow = new RegisterWindow();
            registerWindow.Show();
            this.Close();
        }
    }
}
