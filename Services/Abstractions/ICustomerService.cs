using Smart_Queue_API.DTOs;

namespace Smart_Queue_API.Services.Abstractions
{
    public interface ICustomerService
    {
        Task<ResultDto<CustomerDto>> AddCustomerAsync(CustomerCreateDto dto);
        Task<ResultDto<IEnumerable<CustomerDto>>>GetAllinQueue();
        Task<ResultDto<CustomerDto>> GetById(Guid id);
        Task<ResultDto<bool>> UpdateCustomerAsync(CustomerUpdateeDto dto);
        Task<ResultDto<bool>> DeleteCustomerAsync(Guid id);

        Task<ResultDto<CustomerDto>> ServeNextCustomerAsync();
        Task<ResultDto<int>> GetCustomerPositionAsync(Guid id);
    }
}
