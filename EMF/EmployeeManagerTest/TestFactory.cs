using CustomerManager.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeManagerTest
{
    public class TestFactory
    {
        public static DbContextOptions<AppDbContext> CreateDbContextOptions(string database)
        {
            return new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: database)
                .Options;
        }
    }
}
