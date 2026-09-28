using Microsoft.AspNetCore.Http.HttpResults;
using Smart_Queue_API.DTOs;
using Smart_Queue_API.Entities;
using Smart_Queue_API.Repository.Abstractions;
using Smart_Queue_API.Services.Abstractions;

namespace Smart_Queue_API.Services.Implementations
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }
        public async Task<ResultDto<CustomerDto>> AddCustomerAsync(CustomerCreateDto dto)
        {
            Customer newCustommer = new()
            {
                Name = dto.Name,
                CreatedAt = DateTime.UtcNow,
                Status = QueueStatus.Waiting
                
            };
            await _customerRepository.AddCustomerAsync(newCustommer);
            await _customerRepository.SaveAsync();

            var customerDto = new CustomerDto(
                newCustommer.Id,
                newCustommer.Name,
                newCustommer.CreatedAt,
                newCustommer.Status
                );
            return ResultDto<CustomerDto>.Success(customerDto, "Customer added successfully!");
        }

        public async Task<ResultDto<bool>> DeleteCustomerAsync(Guid id)
        {
            var deletedCustomer = await _customerRepository.GetByIdAsync(id);
            if (deletedCustomer == null) throw new KeyNotFoundException("Customer not found");
            _customerRepository.DeleteCustomer(deletedCustomer);
            await _customerRepository.SaveAsync();
            return ResultDto<bool>.Success(true, "Customer deleted in queue");
        }

        public async Task<ResultDto<IEnumerable<CustomerDto>>> GetAllinQueue()
        {
            var allCustomers = await _customerRepository.GetAllInQueueAsync();
            var customerDtos = allCustomers.Select(c => new CustomerDto(
                c.Id,
                c.Name,
                c.CreatedAt, c.Status
                ));
            return ResultDto<IEnumerable<CustomerDto>>.Success(customerDtos);
        }

        public async Task<ResultDto<CustomerDto>> GetById(Guid id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);

            if (customer == null)
            {
                return ResultDto<CustomerDto>.Failure("Customer not found.");
            }

            var customerDto = new CustomerDto(
                customer.Id,
                customer.Name,
                customer.CreatedAt,
                customer.Status
            );

            return ResultDto<CustomerDto>.Success(customerDto);
        }

        public async Task<ResultDto<bool>> UpdateCustomerAsync(CustomerUpdateeDto dto)
        {
            var customer = await _customerRepository.GetByIdAsync(dto.Id);

            if (customer == null)
            {
                return ResultDto<bool>.Failure("Customer not found.");
            }
            customer.Name = dto.Name;
            customer.Status = dto.Status;

            _customerRepository.UpdateCustomer(customer);
            await _customerRepository.SaveAsync();

            return ResultDto<bool>.Success(true, "Customer updated successfully!");

        }
        public async Task<ResultDto<CustomerDto>> ServeNextCustomerAsync()
        {
            var nextCustomer = await _customerRepository.GetNextWaitingCustomerAsync();

            if (nextCustomer == null)
            {
                return ResultDto<CustomerDto>.Failure("There are no customers waiting in line!");
            }

            nextCustomer.Status = QueueStatus.Serving;
            _customerRepository.UpdateCustomer(nextCustomer);
            await _customerRepository.SaveAsync();

            var customerDto = new CustomerDto(
                nextCustomer.Id,
                nextCustomer.Name,
                nextCustomer.CreatedAt,
                nextCustomer.Status
            );

            return ResultDto<CustomerDto>.Success(customerDto, "The next customer was called!");
        }

        public async Task<ResultDto<int>> GetCustomerPositionAsync(Guid id)
        {
            var position = await _customerRepository.GetCustomerPositionAsync(id);

            if (position == 0)
            {
                return ResultDto<int>.Failure("Customer was not found in the queue or has already been served!");
            }

            return ResultDto<int>.Success(position, $"The customer is at position {position} in the waiting queue.");
        }
    }
}
