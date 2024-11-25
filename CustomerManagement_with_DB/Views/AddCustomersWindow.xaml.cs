using System.Windows;
using CustomerManagement_with_DB.Controllers;
using MySql.Data.MySqlClient;

namespace CustomerManagement_with_DB
{
    public partial class AddCustomersWindow : Window
    {
        private readonly DBConnector connection;

        public AddCustomersWindow()
        {
            InitializeComponent();
            connection = new DBConnector(); // Instanz der Datenbankverbindung
        }

        private void SaveCustomer_Click(object sender, RoutedEventArgs e)
        {
            // Eingabewerte sammeln
            string vorname = VornameTextBox.Text;
            string nachname = NachnameTextBox.Text;
            string adresse = AdresseTextBox.Text;
            string plz = PLZTextBox.Text;
            string wohnort = WohnortTextBox.Text;
            string email = EmailTextBox.Text;

            if (!Validation.ValidateCustomerData(vorname, nachname, adresse, plz, wohnort, email))
            {
                return; 
            }

            try
            {
                using (var conn = connection.GetConnection())
                {
                    conn.Open();
                    string query = "INSERT INTO Kunden (Vorname, Nachname, Adresse, PLZ, Wohnort, Email) " +
                                   "VALUES (@Vorname, @Nachname, @Adresse, @PLZ, @Wohnort, @Email)";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Vorname", vorname);
                    cmd.Parameters.AddWithValue("@Nachname", nachname);
                    cmd.Parameters.AddWithValue("@Adresse", adresse);
                    cmd.Parameters.AddWithValue("@PLZ", plz);
                    cmd.Parameters.AddWithValue("@Wohnort", wohnort);
                    cmd.Parameters.AddWithValue("@Email", email);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show($"Kunde '{vorname} {nachname}' wurde erfolgreich hinzugefügt!",
                                    "Erfolg", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show($"Fehler beim Hinzufügen des Kunden: {ex.Message}", "Fehler",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); 
        }
    }
}
