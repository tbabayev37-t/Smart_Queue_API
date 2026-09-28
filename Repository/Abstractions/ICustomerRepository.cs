using Smart_Queue_API.Entities;

namespace Smart_Queue_API.Repository.Abstractions
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> GetAllInQueueAsync();
        Task<Customer?> GetByIdAsync(Guid id);
        Task AddCustomerAsync(Customer customer);
        void UpdateCustomer(Customer customer);
        void DeleteCustomer(Customer customer);
        Task SaveAsync();
        Task<Customer?> GetNextWaitingCustomerAsync();
        Task<int> GetCustomerPositionAsync(Guid id);
    }
}
