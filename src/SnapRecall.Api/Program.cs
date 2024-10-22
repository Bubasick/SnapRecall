using Microsoft.EntityFrameworkCore;
using SnapRecall.Infrastructure.Data;
using Telegram.BotAPI;
using Telegram.BotAPI.GettingUpdates;

namespace SnapRecall.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            var client = new TelegramBotClient();
            builder.Services.AddSingleton<ITelegramBotClient>(client);

            builder.Services.AddDbContext<SnapRecallDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            client.DeleteWebhook(true); // Delete old webhook
            client.SetWebhook("https://working-badger-model.ngrok-free.app/bot"); // Set new webhook
            var info = client.GetWebhookInfo();
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
