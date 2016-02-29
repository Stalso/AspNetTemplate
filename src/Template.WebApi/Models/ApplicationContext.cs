using Microsoft.Data.Entity;

namespace Template.WebApi.Models {
    public class ApplicationContext : DbContext {
        public DbSet<Application> Applications { get; set; }

    }
}