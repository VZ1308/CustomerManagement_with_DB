using MySql.Data.MySqlClient;
using System;

public class DBConnector
{
    private readonly string connectionString = "Server=localhost;Database=Customermanagement;User ID=root;Password=;"; // appsettings.json

    // Verbindung erstellen
    public MySqlConnection GetConnection()
    {
        return new MySqlConnection(connectionString);
    }

    // Verbindung testen
    public bool TestConnection()
    {
        try
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                return true; // Verbindung erfolgreich
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Datenbankverbindung fehlgeschlagen: {ex.Message}");
            return false; // Verbindung fehlgeschlagen
        }
    }

    // Login-Daten überprüfen
    public bool CheckLogin(string username, string password)
    {
        try
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM Benutzer WHERE Benutzername = @username AND Passwort = @password";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password);

                    int result = Convert.ToInt32(cmd.ExecuteScalar()); // schnellste Methode, um einen einzelnen Wert aus der Datenbank zu erhalten
                    return result > 0; // Es wurde mindestens ein Benutzer mit passendem Namen und Passwort gefunden
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fehler bei der Login-Überprüfung: {ex.Message}");
            return false;
        }
    }
}
