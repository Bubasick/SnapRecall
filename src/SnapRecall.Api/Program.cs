using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore;
using SnapRecall.Application.SnapRecallBotHandlers;
using SnapRecall.Infrastructure.Data;
using Telegram.BotAPI;

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
            builder.Services.AddTransient<ITelegramBot, SnapRecallBot>();
            builder.Services.AddHttpClient();
            builder.Services.AddSingleton<ITelegramBotClient, TelegramBotClient>((serviceProvider) =>
            {
                var httpClient = serviceProvider.GetRequiredService<IHttpClientFactory>().CreateClient();

                return new TelegramBotClient("7744751662:AAE1wcsbF_RGEln5MpMd582oxiKgVtqwxvc", httpClient);
            });

            builder.Services.AddSingleton(x => new BlobServiceClient(builder.Configuration.GetConnectionString("StorageAccount")));


            builder.Services.AddDbContext<SnapRecallDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

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

            app.UseTelegramWebhook();
            app.Run();
        }
    }
}
