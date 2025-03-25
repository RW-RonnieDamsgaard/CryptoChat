using CryptoChat.Data;
using CryptoChat.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace CryptoChat
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor();
            builder.Services.AddSingleton<WeatherForecastService>();

            // Read the key from the configuration file
            var cryptoKey = builder.Configuration["CryptoSettings:Key"];
            if (string.IsNullOrEmpty(cryptoKey))
            {
                throw new InvalidOperationException("CryptoSettings:Key configuration is not set.");
            }
            builder.Services.AddScoped<CryptoService>(_ => new CryptoService(cryptoKey));

            builder.Services.AddSignalR();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.MapHub<ChatHub>("/chathub");

            app.MapBlazorHub();
            app.MapFallbackToPage("/_Host");

            app.Run();
        }
    }
}
