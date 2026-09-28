using Microsoft.EntityFrameworkCore;
using Smart_Queue_API.Contexts;
using Smart_Queue_API.Entities;
using Smart_Queue_API.Repository.Abstractions;

namespace Smart_Queue_API.Repository.Implementations
{
    public class CustomerRepository: ICustomerRepository
    {
        protected readonly ApplicationDbContext _contex;
        public CustomerRepository(ApplicationDbContext contex)
        {
            _contex = contex;
        }
        public async Task AddCustomerAsync(Customer customer)
        {
            await _contex.Customers.AddAsync(customer);
        }

        public void DeleteCustomer(Customer customer)
        {
            _contex.Customers.Remove(customer);
        }

        public async Task<IEnumerable<Customer>> GetAllInQueueAsync()
        {
            return await _contex.Customers.ToListAsync();
        }

        public async Task<Customer?> GetByIdAsync(Guid id)
        {
            return await _contex.Customers.FindAsync(id);
        }

        public void UpdateCustomer(Customer customer)
        {
            _contex.Customers.Update(customer);
        }
        public async Task SaveAsync()
        {
            await _contex.SaveChangesAsync();
        }
        public async Task<Customer?> GetNextWaitingCustomerAsync()
        {
            return await _contex.Customers
                .Where(c => c.Status == QueueStatus.Waiting)
                .OrderBy(c => c.CreatedAt)
                .FirstOrDefaultAsync();
        }

        public async Task<int> GetCustomerPositionAsync(Guid id)
        {
            var customer = await GetByIdAsync(id);
            if (customer == null || customer.Status != QueueStatus.Waiting)
                return 0; 

            int position = await _contex.Customers
                .Where(c => c.Status == QueueStatus.Waiting && c.CreatedAt < customer.CreatedAt)
                .CountAsync();

            return position + 1;
        }
    }
}
