using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using shop.Helpers;
using shop.Models;
using shop.Data;

namespace shop.Services
{
    public interface ICustomerService
    {
        Customer Authenticate(string username, string password);
        IEnumerable<Customer> GetAll();
        Customer GetById(int id);
        Customer Create(Customer customer, string password);
        void Update(Customer customer, string currentPassword, string password, string confirmPassword);
        void Delete(int id);
    }

    public class CustomerService : ICustomerService
    {
        private Context _context;

        public CustomerService(Context context)
        {
            _context = context;
        }

        public Customer Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return null;
            }

            var customer = _context.Customer.FirstOrDefault(x => x.username == username) ?? null;

            // check if username exists
            if (customer == null)
            {
                return null;
            }

            // Granting access if the hashed password in the database matches with the password(hashed in computeHash method) entered by customer.
            if(computeHash(password) != customer.passwordhash)
            {
                return null;
            }
            return customer;        
        }

        public IEnumerable<Customer> GetAll()
        {
            return _context.Customer;
        }

        public Customer GetById(int id)
        {
            return _context.Customer.Find(id);
        }

        public Customer Create(Customer customer, string password)
        {
            // validation
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new AppException("Password is required");
            }

            if (_context.Customer.Any(x => x.username == customer.username))
            {
                throw new AppException("username \"" + customer.username + "\" is already taken");
            }

            //Saving hashed password into Database table
            customer.passwordhash = computeHash(password);  

            _context.Customer.Add(customer);
            _context.SaveChanges();

            return customer;
        }

        public void Update(Customer customerParam, string currentPassword = null, string password = null, string confirmPassword = null)
        {
            //Find the customer by Id
            var customer = _context.Customer.Find(customerParam.id);

            if (customer == null) 
            {
                throw new AppException("Customer not found");
            }
            // update customer properties if provided
            if (!string.IsNullOrWhiteSpace(customerParam.username) && customerParam.username != customer.username)
            {
                // throw error if the new username is already taken
                if (_context.Customer.Any(x => x.username == customerParam.username))
                {
                    throw new AppException("Username " + customerParam.username + " is already taken");
                }
                else
                {
                    customer.username = customerParam.username;
                }
            }
            if (!string.IsNullOrWhiteSpace(customerParam.firstname))
            {
                customer.firstname = customerParam.firstname;
            }
            if (!string.IsNullOrWhiteSpace(customerParam.lastname))
            {
                customer.lastname = customerParam.lastname;
            }
            if (!string.IsNullOrWhiteSpace(currentPassword))
            {   
                if(computeHash(currentPassword) != customer.passwordhash)
                {
                    throw new AppException("Invalid Current password!");
                }

                if(currentPassword == password)
                {
                    throw new AppException("Please choose another password!");
                }

                if(password != confirmPassword)
                {
                    throw new AppException("Password doesn't match!");
                }
    
                //Updating hashed password into Database table
                customer.passwordhash = computeHash(password); 
            }
            
            _context.Customer.Update(customer);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var customer = _context.Customer.Find(id);
            if (customer != null)
            {
                _context.Customer.Remove(customer);
                _context.SaveChanges();
            }
        }

        private static string computeHash(string Password)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            var input = md5.ComputeHash(Encoding.UTF8.GetBytes(Password));
            var hashstring = "";
            foreach(var hashbyte in input)
            {
                hashstring += hashbyte.ToString("x2"); 
            } 
            return hashstring;
        }
    }
}