using System.Windows;

namespace CustomerManagement_with_DB.Views
{
    public partial class EditCustomerWindow : Window
    {
        public Customer Customer { get; set; }

        // Hinzufügen einer Eigenschaft für KundenID, falls du sie separat verwenden möchtest
        public int KundenID => Customer.KundenID;

        public EditCustomerWindow(Customer customer)
        {
            InitializeComponent();
            Customer = customer;

            // Initialisieren der Textfelder mit den aktuellen Werten des Kunden
            VornameTextBox.Text = customer.Vorname;
            NachnameTextBox.Text = customer.Nachname;
            AdresseTextBox.Text = customer.Adresse;
            PLZTextBox.Text = customer.PLZ;
            WohnortTextBox.Text = customer.Wohnort;
            EmailTextBox.Text = customer.Email;
        }

        // Event-Handler für den Save-Button
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Speichern der Änderungen
            Customer.Vorname = VornameTextBox.Text;
            Customer.Nachname = NachnameTextBox.Text;
            Customer.Adresse = AdresseTextBox.Text;
            Customer.PLZ = PLZTextBox.Text;
            Customer.Wohnort = WohnortTextBox.Text;
            Customer.Email = EmailTextBox.Text;

            // Fenster schließen und als "DialogResult = true" markieren, um die Änderungen zu speichern
            this.DialogResult = true;
            this.Close();
        }

        // Event-Handler für den Cancel-Button
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Fenster schließen ohne Änderungen zu speichern
            this.DialogResult = false;
            this.Close();
        }
    }
}
