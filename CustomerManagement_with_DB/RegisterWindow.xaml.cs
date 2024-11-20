using System;
using System.Windows;
using MySql.Data.MySqlClient;

namespace CustomerManagement_with_DB
{
    public partial class RegisterWindow : Window
    {
        private DBConnector dbConnector = new DBConnector();

        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;
            string confirmPassword = ConfirmPasswordBox.Password;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(confirmPassword))
            {
                MessageBox.Show("Bitte füllen Sie alle Felder aus.", "Fehler");
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Passwörter stimmen nicht überein.", "Fehler");
                return;
            }

            try
            {
                using (var conn = dbConnector.GetConnection())
                {
                    conn.Open();
                    string query = "INSERT INTO Benutzer (Benutzername, Passwort) VALUES (@username, @password)";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Registrierung erfolgreich!", "Erfolg");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler bei der Registrierung: {ex.Message}", "Fehler");
            }
        }
    }
}
