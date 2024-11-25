using CustomerManagement_with_DB.Views;
using MySql.Data.MySqlClient;
using System.Data;
using System.Windows;

namespace CustomerManagement_with_DB
{
    public partial class MainWindow : Window
    {
        DBConnector connection = new DBConnector();

        public MainWindow()
        {
            InitializeComponent();
            LoadCustomers();
        }

        private void LoadCustomers()
        {
            try
            {
                using (var conn = connection.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT * FROM Kunden";
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);

                    DataTable customerTable = new DataTable();
                    adapter.Fill(customerTable);

                    CustomerDataGrid.ItemsSource = customerTable.DefaultView;
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show($"Datenbankfehler: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddCustomer_Click(object sender, RoutedEventArgs e)
        {
            AddCustomersWindow addCustomersWindow = new AddCustomersWindow();
            addCustomersWindow.ShowDialog();
            LoadCustomers();
        }

        private void EditCustomer_Click(object sender, RoutedEventArgs e)
        {
            // Sicherstellen, dass ein Kunde ausgewählt wurde
            if (CustomerDataGrid.SelectedItem is DataRowView selectedRow)
            {
                // Extrahieren der KundenID
                int customerId = Convert.ToInt32(selectedRow["KundenID"]);

                // Erstellen eines Customer-Objekts mit den Daten aus der DataRowView
                Customer selectedCustomer = new Customer
                {
                    KundenID = customerId,  // KundenID aus der DataRowView setzen
                    Vorname = (string)selectedRow["Vorname"],
                    Nachname = (string)selectedRow["Nachname"],
                    Adresse = (string)selectedRow["Adresse"],
                    PLZ = (string)selectedRow["PLZ"],
                    Wohnort = (string)selectedRow["Wohnort"],
                    Email = (string)selectedRow["Email"]
                };

                // Neues Fenster für die Bearbeitung öffnen
                EditCustomerWindow editWindow = new EditCustomerWindow(selectedCustomer);

                // Zeige das Fenster als modales Fenster
                if (editWindow.ShowDialog() == true)
                {
                    // Nachdem der Benutzer die Änderungen bestätigt hat, aktualisiere die Werte
                    selectedCustomer.Vorname = editWindow.VornameTextBox.Text;
                    selectedCustomer.Nachname = editWindow.NachnameTextBox.Text;
                    selectedCustomer.Adresse = editWindow.AdresseTextBox.Text;
                    selectedCustomer.PLZ = editWindow.PLZTextBox.Text;
                    selectedCustomer.Wohnort = editWindow.WohnortTextBox.Text;
                    selectedCustomer.Email = editWindow.EmailTextBox.Text;

                    // Update-Methode aufrufen, um Änderungen in der Datenbank zu speichern
                    UpdateCustomerInDatabase(selectedCustomer);

                    // Nach der Bearbeitung die Liste der Kunden erneut laden
                    LoadCustomers();
                }
            }
            else
            {
                MessageBox.Show("Bitte wählen Sie einen Kunden aus.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void UpdateCustomerInDatabase(Customer customer)
        {
            try
            {
                using (var connection = new MySqlConnection("Server=localhost;Database=Customermanagement;User ID=root;Password=;"))
                {
                    connection.Open();
                    string query = "UPDATE Kunden SET Vorname = @Vorname, Nachname = @Nachname, Adresse = @Adresse, " +
                                   "PLZ = @PLZ, Wohnort = @Wohnort, Email = @Email WHERE KundenID = @KundenID";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@KundenID", customer.KundenID);
                        command.Parameters.AddWithValue("@Vorname", customer.Vorname);
                        command.Parameters.AddWithValue("@Nachname", customer.Nachname);
                        command.Parameters.AddWithValue("@Adresse", customer.Adresse);
                        command.Parameters.AddWithValue("@PLZ", customer.PLZ);
                        command.Parameters.AddWithValue("@Wohnort", customer.Wohnort);
                        command.Parameters.AddWithValue("@Email", customer.Email);

                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Daten erfolgreich aktualisiert.", "Erfolg", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler bei der Datenaktualisierung: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteCustomer_Click(object sender, RoutedEventArgs e)
        {
            // Überprüfen, ob ein Kunde im DataGrid ausgewählt ist
            if (CustomerDataGrid.SelectedItem is DataRowView selectedRow)
            {
                // Extrahieren der KundenID
                int customerId = Convert.ToInt32(selectedRow["KundenID"]);

                // Bestätigungsdialog anzeigen
                MessageBoxResult result = MessageBox.Show($"Möchten Sie den Kunden {selectedRow["Vorname"]} {selectedRow["Nachname"]} wirklich löschen?",
                                                          "Kunde löschen",
                                                          MessageBoxButton.YesNo,
                                                          MessageBoxImage.Warning);

                // Wenn der Benutzer "Ja" auswählt, wird der Löschvorgang fortgesetzt
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        // Löschen des Kunden aus der Datenbank
                        DeleteCustomerFromDatabase(customerId);

                        // Entfernen des Kunden aus der Ansicht (DataGrid)
                        var customers = CustomerDataGrid.ItemsSource as DataView;
                        if (customers != null)
                        {
                            // Sucht die Zeile, die der KundenID entspricht
                            var rowToDelete = customers.Cast<DataRowView>()
                                .FirstOrDefault(row => Convert.ToInt32(row["KundenID"]) == customerId);  // "KundenID" hier verwenden

                            if (rowToDelete != null)
                            {
                                // Entfernt die Zeile aus der DataView
                                customers.Table.Rows.Remove(rowToDelete.Row);  // Entferne die Zeile aus der zugrunde liegenden DataTable
                            }
                        }

                        // Bestätigung der erfolgreichen Löschung
                        MessageBox.Show("Der Kunde wurde erfolgreich gelöscht.", "Erfolg", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        // Fehlerbehandlung
                        MessageBox.Show($"Fehler beim Löschen des Kunden: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                // Wenn kein Kunde ausgewählt wurde
                MessageBox.Show("Bitte wählen Sie einen Kunden aus, den Sie löschen möchten.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DeleteCustomerFromDatabase(int customerId)
        {
            try
            {
                // Verbindung zur Datenbank herstellen
                using (var connection = new MySqlConnection("Server=localhost;Database=Customermanagement;User ID=root;Password=;"))
                {
                    connection.Open();
                    string query = "DELETE FROM Kunden WHERE KundenID = @KundenID";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@KundenID", customerId);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Fehler beim Löschen des Kunden aus der Datenbank: {ex.Message}");
            }
        }

        private void EndProgram_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
