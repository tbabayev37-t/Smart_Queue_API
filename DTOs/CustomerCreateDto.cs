using Smart_Queue_API.Entities;

namespace Smart_Queue_API.DTOs
{
    public record CustomerCreateDto
    (
        string Name
     );
    public record CustomerUpdateeDto
    (
        Guid Id,
        string Name,
        QueueStatus Status
     );
    public record CustomerDto
    (
        Guid Id,
        string Name,
        DateTime CreatedAt,
        QueueStatus Status
    );
}
