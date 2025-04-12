using OnlineBookstore.DAO.Implementations;
using OnlineBookstore.Entity;
using OnlineBookstore.Util;
using System;

class Program
{
    static void Main(string[] args)
    {
        string connectionString = "Data Source=YOUR_SERVER;Initial Catalog=OnlineBookStore;Integrated Security=True;";
        var dbUtil = new DbConnectionUtil(connectionString);
        var userDAO = new UserDAOImpl(dbUtil);

        bool exit = false;

        while (!exit)
        {
            Console.WriteLine("\n📚 Welcome to Online Bookstore 📚");
            Console.WriteLine("1. Login");
            Console.WriteLine("2. Register");
            Console.WriteLine("3. Exit");
            Console.Write("Enter your choice: ");
            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    Console.WriteLine("\n🔐 Login:");
                    Console.Write("Enter Email: ");
                    string email = Console.ReadLine();

                    Console.Write("Enter Password: ");
                    string password = Console.ReadLine();

                    User loggedInUser = userDAO.Login(email, password);

                    if (loggedInUser != null)
                    {
                        Console.WriteLine($"\n✅ Welcome back, {loggedInUser.Username}!");
                        ShowUserMenu(loggedInUser); // ← continue to user menu
                    }
                    else
                    {
                        Console.WriteLine("❌ Invalid credentials. Please try again.");
                    }
                    break;

                case "2":
                    Console.WriteLine("\n📝 Register New User:");
                    Console.Write("Enter Username: ");
                    string username = Console.ReadLine();

                    Console.Write("Enter Email: ");
                    string regEmail = Console.ReadLine();

                    Console.Write("Enter Password: ");
                    string regPassword = Console.ReadLine();

                    User newUser = new User
                    {
                        Username = username,
                        Email = regEmail,
                        PasswordHash = regPassword
                    };

                    bool success = userDAO.Register(newUser);
                    Console.WriteLine(success ? "✅ Registration successful!" : "❌ Registration failed.");
                    break;

                case "3":
                    Console.WriteLine("👋 Thank you for visiting! Goodbye.");
                    exit = true;
                    break;

                default:
                    Console.WriteLine("❌ Invalid option. Please enter 1, 2, or 3.");
                    break;
            }
        }
    }

    static void ShowUserMenu(User user)
    {
        Console.WriteLine("\n🔽 User Menu will appear here...");
        // You can add Browse Books / Cart / Orders / Wishlist here later
    }
}
