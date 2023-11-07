using System;
using AnropaDB.Data;
using AnropaDB.Models;
using Microsoft.EntityFrameworkCore;

namespace AnropaDB
{
    internal class Program
    {
        static string GenerateRandomLetters(Random random, int length)
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            char[] result = new char[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = chars[random.Next(chars.Length)];
            }
            return new string(result);
        }
        static void Main(string[] args)
        {
            string input = "";
            while (input != "E")
            {
                Console.WriteLine("Greetings! Make a choice in the menu:");
                Console.WriteLine("F. Fetch all customers.");
                Console.WriteLine("C. Choose a customer in list.");
                Console.WriteLine("A. Add a customer.");
                Console.WriteLine("Press E to exit.");

                input = Console.ReadLine().ToUpper();
                using (NorthContext context = new NorthContext()) { 
                    switch (input)
                {
                    case "F":

                        {
                            FetchCustomer(context);
                        }
                        break;

                    case "C":
                        {
                            ChooseCustomer(context);
                        }
                        break;

                    case "A":
                        {
                            AddCustomer(context);
                        }
                        break;

                    default:
                        if (input != "E") { 
                        Console.WriteLine("Wrong input. Try again.");
                    }
                        break;
                }
            }
        }
        }
        static Customer AddCustomer(NorthContext context)
        {
            Console.Clear();
            //Det står att alla fält är string i databasmodellen, så jag kör på det med
            Console.WriteLine("Type in the needed information to add new customer:");
            Console.Write("Enter Company Name: ");
            string companyName = Console.ReadLine();
            Console.Write("Enter Contact Name: ");
            string contactName = Console.ReadLine();
            Console.Write("Enter Contact Title: ");
            string contactTitle = Console.ReadLine();
            Console.Write("Enter Address: ");
            string address = Console.ReadLine();
            Console.Write("Enter City: ");
            string city = Console.ReadLine();
            Console.Write("Enter Region: ");
            string region = Console.ReadLine();
            Console.Write("Enter Postal Code: ");
            string postalCode = Console.ReadLine();
            Console.Write("Enter Country: ");
            string country = Console.ReadLine();
            Console.Write("Enter Phone: ");
            string phone = Console.ReadLine();
            Console.Write("Enter Fax: ");
            string fax = Console.ReadLine();
            Random random = new Random();
            string randomLetters = GenerateRandomLetters(random, 5);
            string customerId = randomLetters;

            Customer customer = new Customer()
            {
                CustomerId = customerId,
                CompanyName = string.IsNullOrWhiteSpace(companyName) ? null : companyName,
                ContactName = string.IsNullOrWhiteSpace(contactName) ? null : contactName,
                ContactTitle = string.IsNullOrWhiteSpace(contactTitle) ? null : contactTitle,
                Address = string.IsNullOrWhiteSpace(address) ? null : address,
                City = string.IsNullOrWhiteSpace(city) ? null : city,
                Region = string.IsNullOrWhiteSpace(region) ? null : region,
                PostalCode = string.IsNullOrWhiteSpace(postalCode) ? null : postalCode,
                Country = string.IsNullOrWhiteSpace(country) ? null : country,
                Phone = string.IsNullOrWhiteSpace(phone) ? null : phone,
                Fax = string.IsNullOrWhiteSpace(fax) ? null : fax,
            };
            context.Customers.Add(customer);
            context.SaveChanges();
            return customer;
        }

        static void ChooseCustomer(NorthContext context)
        {
            Console.Clear();
            Console.WriteLine("Enter the company name of the customer you want to look up: ");
            string customerChoice = Console.ReadLine().ToUpper();
            var selectedCustomer = context.Customers
            .Where(c => c.CompanyName.ToUpper() == customerChoice.ToUpper())
            .Select(c => new { c.CompanyName, c.ContactName, c.ContactTitle, c.Address, c.City, c.Region, c.PostalCode, c.Country, c.Phone, c.Fax })
            .FirstOrDefault();

            Console.WriteLine("Company information:");
            if (selectedCustomer != null)
            {
                foreach (var c in new[] { selectedCustomer })
                {
                    Console.WriteLine($"Contact Name: {c.ContactName}, \nContact Title: {c.ContactTitle}, \nAddress: {c.Address}, \nCity: {c.City}, \nRegion: {c.Region}, \nPostal Code: {c.PostalCode}, \nCountry: {c.Country}, \nPhone: {c.Phone}, \nFax: {c.Fax} ");
                }
                var selectedCustomersOrders = context.Customers
                .Include(c => c.Orders)
                .FirstOrDefault(c => c.CompanyName.ToUpper() == customerChoice.ToUpper());

                if (selectedCustomersOrders.Orders.Any())
                {
                    Console.WriteLine("Orders:");
                    foreach (var order in selectedCustomersOrders.Orders)
                    {
                        Console.WriteLine($"Customer ID: {order.CustomerId}, Order date: {order.OrderDate}");
                    }
                }

                else
                {
                    Console.WriteLine("We cannot find any orders connected to this customer.");
                }
            }
            else
            {
                Console.WriteLine("Customer not found");
            }


        }
        static void FetchCustomer(NorthContext context)
        {
            Console.Clear();
            var customers = context.Customers
            .Include(c => c.Orders)
            .Select(c => new { c.CompanyName, c.Country, c.Region, c.Phone, TotalOrders = c.Orders.Count })
            .OrderBy(c => c.CompanyName)
            .ToList();

            Console.WriteLine("Sorting by Company Name. Choose between (A)scending or (D)escending.");
            string sortingDirection = Console.ReadLine().ToUpper();

            if (sortingDirection == "A")
            {
                customers = customers.OrderBy(c => c.CompanyName).ToList();
            }
            else if (sortingDirection == "D")
            {
                customers = customers.OrderByDescending(c => c.CompanyName).ToList();
            }
            else
            {
                Console.WriteLine("Invalid input. Default sorting:");
                customers = customers.OrderByDescending(c => c.CompanyName).ToList();
            }

            foreach (var c in customers)
            {
                Console.WriteLine($"CompanyName: {c.CompanyName} -- Country: {c.Country} -- Region: {c.Region} -- Phone: {c.Phone} -- Total Orders: {c.TotalOrders} ");
            }

        }

    }
}
