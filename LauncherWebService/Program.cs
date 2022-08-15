using LauncherWebService.Services;
using Microsoft.AspNetCore.HttpLogging;

namespace LauncherWebService;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddW3CLogging(logging =>
        {
            // Log all W3C fields
            logging.LoggingFields = W3CLoggingFields.All;

            logging.FileSizeLimit = 5 * 1024 * 1024;
            logging.RetainedFileCountLimit = 2;
            logging.FileName = "LauncherWebServices.log";
            logging.LogDirectory = "logs/";
            logging.FlushInterval = TimeSpan.FromSeconds(2);
        });

        // Additional configuration is required to successfully run gRPC on macOS.
        // For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

        // Add services to the container.
        builder.Services.AddGrpc();

        var app = builder.Build();

        app.UseW3CLogging();

        // Configure the HTTP request pipeline.
        app.MapGrpcService<LoginService>();
        app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

        app.Run();
    }
}
