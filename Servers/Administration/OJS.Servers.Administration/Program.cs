namespace OJS.Servers.Administration
{
    using Microsoft.AspNetCore.Builder;
    using OJS.Servers.Administration.Infrastructure.Extensions;

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.ConfigureServices<Program>();

            builder
                .Build()
                .Configure()
                .Run();
        }
    }
}