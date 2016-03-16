using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Template.Domain.Entities;
using Template.OpenIdConnect.EntityFramework;

namespace Template.WebApi.Models
{
    //public static class ApplicationTypes
    //{
    //    public const string Confidential = "confidential";
    //    public const string Public = "public";
    //}
    //public static class Destinations
    //{
    //    //public const string AccessToken = "access_token";
    //    public const string IdentityToken = "id_token";
    //    //TODO rename in rc2
    //    public const string AccessToken = "token";
    //}
    //public static class Claims
    //{
    //    public const string Roles = "roles";
    //}

    //public static class Scopes
    //{
    //    public const string Roles = "roles";
    //}
    //public static class Properties
    //{
    //    public const string Audiences = "audiences";
    //    public const string Confidential = "confidential";
    //    public const string Destinations = "destinations";
    //    public const string Destination = "destination";
    //    public const string Nonce = "nonce";
    //    public const string Presenters = "presenters";
    //    public const string RedirectUri = "redirect_uri";
    //    public const string Resources = "resources";
    //    public const string Scopes = "scopes";
    //    public const string Usage = "usage";
    //}
    /// <summary>
    /// Native repository
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    /// <typeparam name="TApplication"></typeparam>
    /// <typeparam name="TRole"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    //public class ApplicationDbContext<TUser, TApplication, TRole, TKey> : IdentityDbContext<TUser, TRole, TKey>
    //    where TUser : IdentityUser<TKey>
    //    where TApplication : Application
    //    where TRole : IdentityRole<TKey>
    //    where TKey : IEquatable<TKey>
    //{
    //    public ApplicationDbContext() { }

    //    public ApplicationDbContext(DbContextOptions options)
    //        : base(options)
    //    { }

    //    public ApplicationDbContext(IServiceProvider services)
    //        : base(services)
    //    { }

    //    public ApplicationDbContext(IServiceProvider services, DbContextOptions options)
    //        : base(services, options)
    //    { }

    //    //public DbSet<TApplication> Applications { get; set; }
    //    //TODO remove testData
    //    public DbSet<SampleEntity<string>> SampleEntities { get; set; }
    //}
    public class ApplicationDbContext<TUser, TApplication, TRole, TKey> : OpenIdConnectDbContext<TUser, TApplication, TRole, TKey>
       where TUser : IdentityUser<TKey>
       where TApplication : Application
       where TRole : IdentityRole<TKey>
       where TKey : IEquatable<TKey>
    {
        public ApplicationDbContext() { }

        public ApplicationDbContext(DbContextOptions options)
            : base(options)
        { }

        public ApplicationDbContext(IServiceProvider services)
            : base(services)
        { }

        public ApplicationDbContext(IServiceProvider services, DbContextOptions options)
            : base(services, options)
        { }

        //public DbSet<TApplication> Applications { get; set; }
        //TODO remove testData
        public DbSet<SampleEntity<string>> SampleEntities { get; set; }
    }
}
