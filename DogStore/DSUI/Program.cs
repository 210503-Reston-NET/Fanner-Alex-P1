using DSBL;
using DSDL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Serilog;
using System.IO;

namespace DSUI
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //Setup for DB and Serilog
            var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            string connectionString = configuration.GetConnectionString("FannerDogsDB");
            DbContextOptions<FannerDogsDBContext> options = new DbContextOptionsBuilder<FannerDogsDBContext>()
            .UseNpgsql(connectionString).Options;
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File("logs/DogStore.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
            var context = new FannerDogsDBContext(options);
            IMenu menu = new GeneralMenu(new StoreLocationBL(context), new BuyerBL(context), new OrderBL(context), new ManagerBL(context));
            menu.OnStart();
        }
    }
}