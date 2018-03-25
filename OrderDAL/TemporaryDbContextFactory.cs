using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderDAL
{
    class TemporaryDbContextFactory : IDesignTimeDbContextFactory<ModelDbContext>
    {
         public ModelDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<ModelDbContext>();
            builder.UseSqlServer("Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=OrderManagement;Data Source=DESKTOP-19RK5VV\\SQLEXPRESS");
            return new ModelDbContext(builder.Options);
        }
    }
}
