using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DataContext
{
    public class DesignTimeContext: IDesignTimeDbContextFactory<ApplicationContext>
    {
        public ApplicationContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>()
                .UseSqlServer("Server=localhost;Database=cerberus;User Id=sa;Password=Passw0rd;");

            return new ApplicationContext(optionsBuilder.Options);
        }
    }
}