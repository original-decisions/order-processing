﻿using System;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Microsoft.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Processing.Tests
{
    public class Tester<TDbContext> 
        where TDbContext : DbContext
    {
        protected Tester()
        {
        }
        [OneTimeSetUp]
        public virtual void Init()
        {
            //var optionsBuilder = new DbContextOptionsBuilder<TDbContext>();

            //optionsBuilder.UseInMemoryDatabase();

            Options= CreateNewContextOptions();
        }

        public DbContextOptions<TDbContext> Options { get; set; }

        public static DbContextOptions<TDbContext> CreateNewContextOptions()
        {
            // Create a fresh service provider, and therefore a fresh 
            // InMemory database instance.
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            // Create a new options instance telling the context to use an
            // InMemory database and the new service provider.
            var builder = new DbContextOptionsBuilder<TDbContext>();
            builder.UseInMemoryDatabase(Guid.NewGuid().ToString())
                   .UseInternalServiceProvider(serviceProvider);
            return builder.Options;
        }

    }
}
