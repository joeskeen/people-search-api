using Microsoft.EntityFrameworkCore;

namespace assessment
{
    public class PeopleContext : DbContext
    {
        public PeopleContext(DbContextOptions<PeopleContext> options)
          : base(options)
        {
        }

        public DbSet<Person> People { get; set; }
    }
}