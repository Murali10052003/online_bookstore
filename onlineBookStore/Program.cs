using onlineBookStore.Entity;
using onlineBookStore.Util;
using onlineBookStore.DAO.Implementations;
using onlineBookStore.Exceptions;
using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        var connectionString = DbPropertyUtil.GetConnectionString();
        var dbUtil = new DbConnectionUtil(connectionString);

        var userDAO = new UserImpl(dbUtil);
        var genreDAO = new GenreDAOImpl(dbUtil);
        var bookDAO = new BookDAOImpl(dbUtil);
        var cartDAO = new CartDAOImpl(dbUtil);
        var orderDAO = new OrderImpl(dbUtil);
        var reviewDAO = new ReviewImpl(dbUtil);

        User currentUser = null;
        bool appRunning = true;

        while (appRunning)
        {
            Console.WriteLine("\n Welcome to Online Bookstore ");
            Console.WriteLine("1. Register");
            Console.WriteLine("2. Login");
            Console.WriteLine("3. Exit");
            Console.Write("Enter choice: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Write("Enter Username: ");
                    string uname = Console.ReadLine();

                    Console.Write("Enter Email: ");
                    string email = Console.ReadLine();

                    Console.Write("Enter Password: ");
                    string pwd = Console.ReadLine();

                    var user = new User { Username = uname, Email = email, PasswordHash = pwd };
                    bool registered = userDAO.Register(user);
                    Console.WriteLine(registered ? "Registration successful!" : "Failed to register.");
                    break;

                case "2":
                    try
                    {
                        Console.Write("Enter Email: ");
                        string logEmail = Console.ReadLine();

                        Console.Write("Enter Password: ");
                        string logPassword = Console.ReadLine();

                        currentUser = userDAO.Login(logEmail, logPassword);

                        if (currentUser == null)
                        {
                            throw new InvalidCredentialsException("Invalid email or password.");
                        }

                        Console.WriteLine($"\nWelcome, {currentUser.Username}!");
                        bool userLoggedIn = true;

                        while (userLoggedIn)
                        {
                            Console.WriteLine("\n===== Main Menu =====");
                            Console.WriteLine("1. Browse Books");
                            Console.WriteLine("2. View Cart");
                            Console.WriteLine("3. My Orders");
                            Console.WriteLine("4. Logout");
                            Console.WriteLine("5. Review");
                            Console.Write("Select an option: ");
                            string userOption = Console.ReadLine();

                            switch (userOption)
                            {
                                case "1":
                                    var genres = genreDAO.GetAllGenres();
                                    Console.WriteLine("\nAvailable Genres:");
                                    foreach (var g in genres)
                                    {
                                        Console.WriteLine($"{g.GenreID}. {g.GenreName}");
                                    }

                                    Console.Write("Enter Genre ID: ");
                                    int selectedGenre = int.Parse(Console.ReadLine());

                                    var books = bookDAO.GetBooksByGenre(selectedGenre);
                                    Console.WriteLine("\nBooks in selected genre:");
                                    foreach (var b in books)
                                    {
                                        Console.WriteLine($"{b.BookID}. {b.Title} by {b.Author} ");
                                    }

                                    Console.Write("Enter Book ID to view details: ");
                                    int bookId = int.Parse(Console.ReadLine());

                                    var selectedBook = bookDAO.GetBookById(bookId);
                                    Console.WriteLine($"\n{selectedBook.Title} by {selectedBook.Author}");
                                    Console.WriteLine($"Price: {selectedBook.Price} | Stock: {selectedBook.Stock}");

                                    Console.WriteLine("\n1. Add to Cart\n2. Go Back");
                                    Console.Write("Choose: ");
                                    string bookChoice = Console.ReadLine();

                                    if (bookChoice == "1")
                                    {
                                        Console.Write("Enter Quantity: ");
                                        int qty = int.Parse(Console.ReadLine());

                                        Cart cartItem = new Cart
                                        {
                                            UserID = currentUser.UserID,
                                            BookID = bookId,
                                            Quantity = qty
                                        };

                                        bool added = cartDAO.AddToCart(cartItem);
                                        Console.WriteLine(added ? "Book added to cart." : "Failed to add.");
                                    }
                                    break;

                                case "2":
                                    var cartItems = cartDAO.GetCartItemsByUser(currentUser.UserID);
                                    decimal total = 0;
                                    Console.WriteLine("\nYour Cart:");
                                    Console.WriteLine("Book | Price | Qty | Subtotal");
                                    Console.WriteLine("--------------------------------");
                                    foreach (var item in cartItems)
                                    {
                                        decimal sub = item.Bookprice * item.Quantity;
                                        total += sub;
                                        Console.WriteLine($"{item.BookTitle} | ₹{item.Bookprice} | {item.Quantity} | ₹{sub}");
                                    }
                                    Console.WriteLine("--------------------------------");
                                    Console.WriteLine($"Total: ₹{total}");

                                    Console.WriteLine("\n1. Checkout\n2. Remove Item\n3. Back");
                                    Console.Write("Choose: ");
                                    string cartOption = Console.ReadLine();

                                    if (cartOption == "1")
                                    {
                                        Order newOrder = new Order
                                        {
                                            UserID = currentUser.UserID,
                                            OrderDate = DateTime.Now,
                                            Status = "Processing"
                                        };

                                        List<OrderItem> orderItems = new List<OrderItem>();
                                        foreach (var item in cartItems)
                                        {
                                            orderItems.Add(new OrderItem
                                            {
                                                BookID = item.BookID,
                                                Quantity = item.Quantity,
                                                Price = item.Bookprice
                                            });
                                        }

                                        int newOrderId = orderDAO.PlaceOrder(newOrder, orderItems);
                                        if (newOrderId > 0)
                                        {
                                            cartDAO.ClearCart(currentUser.UserID);
                                            Console.WriteLine($"Order #{newOrderId} placed successfully!");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Order failed.");
                                        }
                                    }
                                    else if (cartOption == "2")
                                    {
                                        Console.Write("Enter Book ID to remove: ");
                                        int removeId = int.Parse(Console.ReadLine());
                                        bool removed = cartDAO.RemoveFromCart(currentUser.UserID, removeId);
                                        Console.WriteLine(removed ? "Removed from cart." : "Failed to remove.");
                                    }
                                    break;

                                case "3":
                                    var orders = orderDAO.GetOrdersByUser(currentUser.UserID);
                                    Console.WriteLine("\n📦 Your Orders:");
                                    foreach (var o in orders)
                                    {
                                        Console.WriteLine($"Order #{o.OrderID} | Date: {o.OrderDate.ToShortDateString()} | Status: {o.Status}");
                                    }
                                    break;

                                case "4":
                                    userLoggedIn = false;
                                    currentUser = null;
                                    Console.WriteLine("Logged out.");
                                    break;

                                case "5":
                                    Console.WriteLine("\n===== Review Section =====");
                                    Console.WriteLine("1. Add Review");
                                    Console.WriteLine("2. View Reviews by Book");
                                    Console.WriteLine("3. Back");
                                    Console.Write("Choose an option: ");
                                    string reviewOption = Console.ReadLine();

                                    switch (reviewOption)
                                    {
                                        case "1":
                                            var userOrders = orderDAO.GetOrdersByUser(currentUser.UserID);
                                            Console.WriteLine("\nPurchased Books:");
                                            foreach (var ord in userOrders)
                                            {
                                                var items = orderDAO.GetOrderItems(ord.OrderID);
                                                foreach (var i in items)
                                                {
                                                    Console.WriteLine($"{i.BookID} - {i.BookTitle}");
                                                }
                                            }

                                            Console.Write("Enter Book ID to review: ");
                                            int revBookId = int.Parse(Console.ReadLine());

                                            Console.Write("Enter rating (1-5): ");
                                            int rating = int.Parse(Console.ReadLine());

                                            Console.Write("Enter your review: ");
                                            string comment = Console.ReadLine();

                                            Review newReview = new Review
                                            {
                                                UserID = currentUser.UserID,
                                                BookID = revBookId,
                                                Rating = rating,
                                                ReviewText = comment,
                                                CreatedAt = DateTime.Now,

                                            };

                                            bool success = reviewDAO.AddReview(newReview);
                                            Console.WriteLine(success ? "Review submitted!" : "Failed to submit review.");
                                            break;

                                        case "2":
                                            Console.Write("Enter Book ID to view reviews: ");
                                            int bookToView = int.Parse(Console.ReadLine());

                                            var reviews = reviewDAO.GetReviewsByBookId(bookToView);
                                            Console.WriteLine($"\nReviews for Book ID {bookToView}:");
                                            foreach (var r in reviews)
                                            {
                                                Console.WriteLine($"Rating: {r.Rating}/5 | {r.ReviewText} (By User name: {r.Username} on {r.CreatedAt.ToShortDateString()})");
                                            }
                                            break;

                                        case "3":
                                            break;

                                        default:
                                            Console.WriteLine("Invalid option in Reviews menu.");
                                            break;
                                    }
                                    break;

                                default:
                                    Console.WriteLine("Invalid option.");
                                    break;
                            }
                        }
                    }
                    catch (InvalidCredentialsException ex)
                    {
                        Console.WriteLine($"[Error] {ex.Message}");
                    }
                    break;

                case "3":
                    appRunning = false;
                    Console.WriteLine("Exiting application.");
                    break;

                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }
    }
}