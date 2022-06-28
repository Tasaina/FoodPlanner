using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FoodPlanner.DataLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;

namespace FoodPlanner.Services
{
    public class FoodServiceProvider
    {

        private readonly string _connectionString = ""; 
            //@"Data Source=localhost; Initial Catalog=FoodPlanner;MultipleActiveResultSets=True;Integrated Security=True;User Id=username;Password=password;";
            
        public static FoodServiceProvider Instance { get; private set; }
        private ServiceProvider Services { get; set; }

        public T GetServiceCore<T>()
        {
            return Services.GetService<T>();
        }

        public static T GetService<T>()
        {
            return Instance.GetServiceCore<T>();
        }

        private FoodServiceProvider()
        {
            RegisterServices();
        }


        public static void Initialize()
        {
            Instance = new FoodServiceProvider();
            Log(LogEventLevel.Information, $"RegisterServices completed successfully");
        }

        private void RegisterServices()
        {
            var services = new ServiceCollection();

            if (string.IsNullOrEmpty(_connectionString))
                services.AddDbContext<FoodContext>(o => o.UseSqlite().UseLazyLoadingProxies());
            else
                services.AddDbContext<FoodContext>(o=>o.UseSqlServer(_connectionString).UseLazyLoadingProxies());
            
            services.AddScoped<IFoodService, FoodService>();
            services.AddScoped<ITrackerService, TrackerService>();
            services.AddScoped<ITagService, TagService>();
            services.AddSingleton<FoodLogger>();
            Services = services.BuildServiceProvider();
            var con = Services.GetService<FoodContext>();
            con.Database.EnsureCreated();
        }

        public static void Log(LogEventLevel level, string message)
        {
            GetService<FoodLogger>().Log(level, message);
        }
    }
}