using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Server.Kestrel.Core;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(new WebApplicationOptions
        {
            Args = args,
            ApplicationName = typeof(Program).Assembly.FullName,
            ContentRootPath = Directory.GetCurrentDirectory()
        });

        builder.WebHost.ConfigureKestrel(options =>
        {
            options.ListenLocalhost(2910, ListenOptions =>
            {
                ListenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
            });
        });

        builder.Services.AddGrpc();

        builder.Services.AddDbContext<LogDbContext>(options =>
        {
            options.UseSqlite("Data Source=data/logs.db");
        });

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<LogDbContext>();
            db.Database.EnsureCreated();
        }

        app.MapGrpcService<LogService>();

        app.Run();
    }

}