using Microsoft.EntityFrameworkCore;
using Smart_Queue_API.Entities;
using System.Reflection;

namespace Smart_Queue_API.Contexts
{
    public sealed class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options) 
        {
             
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
        public DbSet<Customer> Customers { get; set; }
    }
}
