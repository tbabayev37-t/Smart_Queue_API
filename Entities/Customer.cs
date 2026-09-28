namespace Smart_Queue_API.Entities
{
    public sealed class Customer
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public QueueStatus Status { get; set; }
    }
}
