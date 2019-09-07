using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using AngleSharp;
using AngleSharp.Html.Parser;
using FunApp.Data;
using FunApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Sandbox
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine($"{typeof(Program).Namespace} ({string.Join(" ", args)}) starts working...");
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider(true);

            // Seed data on application startup
            //using (var serviceScope = serviceProvider.CreateScope())
            //{
            //    var dbContext = serviceScope.ServiceProvider.GetRequiredService<FunAppContext>();
            //    dbContext.Database.Migrate();
            //    new ApplicationDbContextSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();
            //}

            using (var serviceScope = serviceProvider.CreateScope())
            {
                serviceProvider = serviceScope.ServiceProvider;
                SandboxCode(serviceProvider);
                //return Parser.Default.ParseArguments<SandboxOptions>(args).MapResult(
                //    opts => SandboxCode(opts, serviceProvider),
                //    _ => 255);
            }
        }

        private static void SandboxCode(IServiceProvider serviceProvider)
        {
            var dbContext = serviceProvider.GetService<FunAppContext>();
            //Console.WriteLine(db.Users.CountAsync().GetAwaiter().GetResult());
            
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var config = Configuration.Default.WithDefaultLoader();
            var parser = new HtmlParser();
            //var context = BrowsingContext.New(config);
            var webClient = new WebClient() { Encoding = Encoding.GetEncoding("windows-1251") };

            var address = "http://fun.dir.bg/vic_open.php?id=";

            for (int i = 3000; i < 3050; i++)
            {
                string html = null;
                for (int j = 0; j < 10; j++)
                {
                    try
                    {
                        html = webClient.DownloadString($"{address}{i}");
                        break;
                    }
                    catch(Exception e)
                    {
                        Thread.Sleep(10000);
                    }
                }

                if (string.IsNullOrWhiteSpace(html))
                {
                    continue;
                }

                var document = parser.ParseDocument(html);
                var jokeContent = document.QuerySelector("#newsbody")?.TextContent?.Trim();
                var categoryName = document.QuerySelector(".tag-links-left a")?.TextContent?.Trim();

                if (!string.IsNullOrWhiteSpace(jokeContent) && !string.IsNullOrWhiteSpace(categoryName))
                {
                    var category = dbContext.Categories.FirstOrDefault(x => x.Name == categoryName);

                    if (category == null)
                    {
                        category = new Category() { Name = categoryName };
                    }

                    var joke = new Joke()
                    {
                        Content = jokeContent,
                        Category = category,
                    };

                    dbContext.Jokes.Add(joke);
                    dbContext.SaveChanges();
                }

                Console.WriteLine($"{i} => {categoryName}");
            }
            
            
            //var cellSelector = "tr.vevent td:nth-child(3)";
            //var cells = document.QuerySelector("");
            //var titles = cells.Select(m => m.TextContent);
        }

        private static void ConfigureServices(ServiceCollection serviceCollection)
        {
            var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .AddEnvironmentVariables()
                .Build();

            serviceCollection.AddDbContext<FunAppContext>(options =>
            options.UseSqlServer(
            configuration.GetConnectionString("DefaultConnection")));
        }

        //private static int SandboxCode(SandboxOptions options, IServiceProvider serviceProvider)
        //{
        //    var sw = Stopwatch.StartNew();
        //    var settingsService = serviceProvider.GetService<ISettingsService>();
        //    Console.WriteLine($"Count of settings: {settingsService.GetCount()}");
        //    Console.WriteLine(sw.Elapsed);
        //    return 0;
        //}

        //private static void ConfigureServices(ServiceCollection services)
        //{
        //    var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
        //        .AddJsonFile("appsettings.json", false, true)
        //        .AddEnvironmentVariables()
        //        .Build();

        //    services.AddSingleton<IConfiguration>(configuration);
        //    services.AddDbContext<FunAppContext>(
        //        options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
        //            .UseLoggerFactory(new LoggerFactory()));

        //    services
        //        .AddIdentity<FunAppUser, ApplicationRole>(options =>
        //        {
        //            options.Password.RequireDigit = false;
        //            options.Password.RequireLowercase = false;
        //            options.Password.RequireUppercase = false;
        //            options.Password.RequireNonAlphanumeric = false;
        //            options.Password.RequiredLength = 6;
        //        })
        //        .AddEntityFrameworkStores<ApplicationDbContext>()
        //        .AddUserStore<ApplicationUserStore>()
        //        .AddRoleStore<ApplicationRoleStore>()
        //        .AddDefaultTokenProviders();

        //    services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
        //    services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
        //    services.AddScoped<IDbQueryRunner, DbQueryRunner>();

        //    // Application services
        //    services.AddTransient<IEmailSender, NullMessageSender>();
        //    services.AddTransient<ISmsSender, NullMessageSender>();
        //    services.AddTransient<ISettingsService, SettingsService>();
        //}
    }
}
