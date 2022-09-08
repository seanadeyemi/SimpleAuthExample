using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SimpleAuthExample.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimpleAuthExample.Data
{
    public class SimpleAuthDbContext : IdentityDbContext<User, Role, int>
    {
        public SimpleAuthDbContext(DbContextOptions<SimpleAuthDbContext> dbContextOptions) : base(dbContextOptions)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            //Complete overwriting of default ASP.NET Core Identity table names
            var tableNameWithAspNet = builder.Model.GetEntityTypes().Where(e => e.GetTableName().StartsWith("AspNet")).ToList();
            tableNameWithAspNet.ForEach(x =>
            {
                x.SetTableName(x.GetTableName().Substring(6));
            });
        }
    }
}
