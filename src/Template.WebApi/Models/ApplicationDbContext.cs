using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;
using OpenIddict.Core;
using OpenIddict.Models;

namespace Template.WebApi.Models
{
    //public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    //{
    //    public DbSet<TempApplication> Applications { get; set; }
    //    protected override void OnModelCreating(ModelBuilder builder)
    //    {
    //        base.OnModelCreating(builder);
    //        // Customize the ASP.NET Identity model and override the defaults if needed.
    //        // For example, you can rename the ASP.NET Identity table names and more.
    //        // Add your customizations after calling base.OnModelCreating(builder);
    //    }
    //}
    public class ApplicationDbContext : OpenIddictContext<ApplicationUser, Application, IdentityRole, string>
    {
        //public DbSet<TempApplication> Applications { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
