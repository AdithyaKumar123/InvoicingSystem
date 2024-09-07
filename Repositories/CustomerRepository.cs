using CsvHelper;
using InvoicingSystem.Models;
using System.Globalization;

namespace InvoicingSystem.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly string _filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "customers.csv");

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            try
            {
                return await Task.Run(() =>
                {
                    if (!File.Exists(_filePath))
                        return new List<Customer>();

                    using var reader = new StreamReader(_filePath);
                    using var csv = new CsvReader(reader, new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture));
                    return csv.GetRecords<Customer>().ToList();
                });
            }catch (Exception ex)
            {
                throw new InvalidOperationException("GetAllCustomersAsync method error:", ex);
            }
        }

        public async Task<Customer> GetCustomerByIdAsync(Guid customerId)
        {
            try
            {
                var customers = await GetAllCustomersAsync();
                return customers.FirstOrDefault(c => c.Id == customerId);
            }catch (Exception ex) 
            {
                throw new InvalidOperationException("GetCustomerByIdAsync method error:", ex);
            }
        }

        public async Task AddCustomerAsync(Customer customer)
        {
            try
            {
                var customers = (await GetAllCustomersAsync()).ToList();
                customer.Id = Guid.NewGuid(); // Generate a new ID for the customer
                customers.Add(customer);
                await SaveCustomersAsync(customers);
            }catch(Exception ex)
            {
                throw new InvalidOperationException("AddCustomerAsync method error:", ex);
            }
        }

        public async Task UpdateCustomerAsync(Customer customer)
        {
            try
            {
                var customers = (await GetAllCustomersAsync()).ToList();
                var existingCustomer = customers.FirstOrDefault(c => c.Id == customer.Id);

                if (existingCustomer == null)
                {
                    throw new Exception("Customer not found.");
                }

                existingCustomer.Name = customer.Name;
                existingCustomer.Email = customer.Email;
                existingCustomer.Address = customer.Address;
                existingCustomer.ContactNumber = customer.ContactNumber;

                await SaveCustomersAsync(customers);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("UpdateCustomerAsync method error:", ex);
            }
        }

        public async Task DeleteCustomerAsync(Guid customerId)
        {
            try
            {
                var customers = (await GetAllCustomersAsync()).ToList();
                var customerToRemove = customers.FirstOrDefault(c => c.Id == customerId);

                if (customerToRemove == null)
                {
                    throw new Exception("Customer not found.");
                }

                customers.Remove(customerToRemove);
                await SaveCustomersAsync(customers);
            }catch (Exception ex)
            {
                throw new InvalidOperationException("DeleteCustomerAsync method error:", ex);
            }
        }

        private async Task SaveCustomersAsync(IEnumerable<Customer> customers)
        {
            try
            {
                using var writer = new StreamWriter(_filePath);
                using var csv = new CsvWriter(writer, new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture));
                await csv.WriteRecordsAsync(customers);
            }catch(Exception ex)
            {
                throw new InvalidOperationException("SaveCustomersAsync method error:", ex);
            }
        }
    }
}
