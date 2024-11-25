using System;
using System.Text.RegularExpressions;
using System.Windows;

namespace CustomerManagement_with_DB.Controllers
{
    public class Validation
    {
        public static bool ValidateCustomerData(string vorname, string nachname, string adresse, string plz, string wohnort, string email)
        {
            if (string.IsNullOrWhiteSpace(vorname) ||
                string.IsNullOrWhiteSpace(nachname) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(adresse) ||
                string.IsNullOrWhiteSpace(wohnort) ||
                string.IsNullOrWhiteSpace(plz))
            {
                MessageBox.Show("Bitte alle Felder ausfüllen!", "Fehler", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            if (!Regex.IsMatch(email, emailPattern))
            {
                MessageBox.Show("Die eingegebene E-Mail-Adresse ist ungültig!", "Fehler", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true; // Alles ist gültig
        }
        public static bool ValidateLogin(string username, string password)
        {
            // Überprüfen, ob die Felder leer sind
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Bitte füllen Sie alle Felder aus.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true; // Felder sind valide
        }

        public static bool ValidateRegistration(string username, string password, string confirmPassword)
        {
            // Überprüfen, ob alle Felder ausgefüllt sind
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(confirmPassword))
            {
                MessageBox.Show("Bitte füllen Sie alle Felder aus.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            // Überprüfen, ob die Passwörter übereinstimmen
            if (password != confirmPassword)
            {
                MessageBox.Show("Passwörter stimmen nicht überein.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true; // Alle Prüfungen bestanden
        }
    }
}


