using System;
using AutoServiceHelper.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AutoServiceHelper.Test.Mock
{
    public class DataBaseMock
    {
        public static ApplicationDbContext Instance
        {
            get
            {
                var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options;

                return new ApplicationDbContext(dbContextOptions);
            }
        }
    }
}
