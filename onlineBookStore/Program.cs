using onlineBookStore.DAO.Implementations;
using onlineBookStore.Entity;
using onlineBookStore.Util;
using System;

namespace onlineBookStore
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = DbPropertyUtil.GetConnectionString();
            var dbUtil = new DbConnectionUtil(connectionString);
            var userDAO = new UserImpl(dbUtil);

            Console.WriteLine("1. Register");
            Console.WriteLine("2. Login");
            int option = Convert.ToInt32(Console.ReadLine());

            switch (option)
            {
                case 1:
                    Console.WriteLine("===== Register New User =====");
                    Console.Write("Enter Username: ");
                    string username = Console.ReadLine();

                    Console.Write("Enter Email: ");
                    string email = Console.ReadLine();

                    Console.Write("Enter Password: ");
                    string password = Console.ReadLine();

                    // Hash the password before storing it
                    string passwordHash = HashPassword(password);

                    User newUser = new User
                    {
                        Username = username,
                        Email = email,
                        PasswordHash = passwordHash
                    };

                    try
                    {
                        bool success = userDAO.Register(newUser);
                        Console.WriteLine(success ? "Registration successful!" : "Registration failed.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"An error occurred: {ex.Message}");
                    }

                    break; // Add break statement to prevent fall-through

                case 2:
                    
                        Console.WriteLine("===== Register New User =====");


                        Console.Write("Enter Email: ");
                        string _email = Console.ReadLine();

                        Console.Write("Enter Password: ");
                        string _password = Console.ReadLine();
                    User logUser = new User
                    {
                        
                        Email = _email,
                        PasswordHash = _password
                    };
                    try
                    {
                        User success = userDAO.Login(_email,_password);
                        
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"An error occurred: {ex.Message}");
                    }


                    // Add case for Login if needed
            }
            }

            static string HashPassword(string password)
            {
                // Implement your password hashing logic here
                return password; // Placeholder, replace with actual hashing
            }
        }
    }

