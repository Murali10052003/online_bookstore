using OnlineBookstore.Entity;
using OnlineBookstore.Util;
using OnlineBookstore.DAO.Implementations;

class Program
{
    static void Main(string[] args)
    {
        // Use your server/DB info
        string connectionString = "Data Source=YOUR_SERVER;Initial Catalog=OnlineBookStore;Integrated Security=True;";
        var dbUtil = new DbConnectionUtil(connectionString);
        var userDAO = new UserDAOImpl(dbUtil);

        Console.WriteLine("===== Register New User =====");
        Console.Write("Enter Username: ");
        string username = Console.ReadLine();

        Console.Write("Enter Email: ");
        string email = Console.ReadLine();

        Console.Write("Enter Password: ");
        string password = Console.ReadLine();

        User newUser = new User
        {
            Username = username,
            Email = email,
            PasswordHash = password
        };

        bool success = userDAO.Register(newUser);

        Console.WriteLine(success ? "✅ Registration successful!" : "❌ Registration failed.");
    }
}
