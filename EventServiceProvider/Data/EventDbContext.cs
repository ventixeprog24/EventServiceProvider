using Microsoft.EntityFrameworkCore;

namespace EventServiceProvider.Data
{
    public class EventDbContext(DbContextOptions<EventDbContext> options) : DbContext(options)
    {
        public DbSet<EventEntity> Events { get; set; }
    }

}
