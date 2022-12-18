using Microsoft.EntityFrameworkCore;

namespace CustomerEstablishmentRequest.Models
{
    public class InputContext : DbContext
    {
        public InputContext(DbContextOptions<InputContext> options)
            : base(options)
        {
        }

        public DbSet<Input> InputItems { get; set; }

    }
}
