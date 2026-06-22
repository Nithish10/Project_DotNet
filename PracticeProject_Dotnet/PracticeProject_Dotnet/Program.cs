using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.RateLimiting;
using PracticeProject_Dotnet.CustomMiddlewares;
using PracticeProject_Dotnet.Exceptions;
using PracticeProject_Dotnet.Filters;
using PracticeProject_Dotnet.KeyedServiceExample;
using PracticeProject_Dotnet.Patterns;
using PracticeProject_Dotnet.Services;
using PracticeProject_Dotnet.Services.Interfaces;
using Scalar.AspNetCore;
using Serilog;

namespace PracticeProject_Dotnet
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration().WriteTo.Console().
            WriteTo.File(path:"logs/logs.txt", rollingInterval: RollingInterval.Minute, retainedFileCountLimit: 7)
            .CreateBootstrapLogger();

            try
            {
                Log.Information("Started");
                var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddSingleton<IProductService, ProductService>();
            // builder.Services.AddProblemDetails(options =>
            // {
            //     options.CustomizeProblemDetails = (cpd =>
            //     {
            //        cpd.ProblemDetails.Extensions["error-details"] = "not found you idiot"; 
            //     });
            // });

            builder.Services.AddSerilog((service, loggerConfiguration) =>
            {
                loggerConfiguration.ReadFrom.Configuration(builder.Configuration).
                ReadFrom.Services(service);
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.Configure<OptionsPatternExample>(builder.Configuration.GetSection("optionsPatternSection"));
            
            builder.Services.AddTransient<CorrelationBasedMiddleware>();
            builder.Services.AddScoped<AuthorizationFilter>();
            builder.Services.AddScoped<ResourceFilter>();
            builder.Services.AddScoped<IKeyedService,  PracticeProject_Dotnet.KeyedServiceExample.KeyedService>();
            ///for global handling exception - using IExceptionHandler
            //builder.Services.AddExceptionHandler<GlobalHandlingException>();
            builder.Services.AddKeyedScoped<INotificationProvider, EmailNotification>("email");

            builder.Services.AddKeyedScoped<INotificationProvider, SMSNotification>("sms");

            builder.Services.AddControllers();

            var app = builder.Build();

            app.UseSwagger(options =>
            {
                options.RouteTemplate = "openapi/{documentName}.json";
            });

            app.MapScalarApiReference();

            app.UseMiddleware<CustomExceptionHandler>();

            //for global handling exception - using IExceptionHandler
            //app.UseExceptionHandler();

            app.Use( async (context, next) =>
            {
               //Console.WriteLine("Middleware 1 started");
               await next();
               //Console.WriteLine("Middleware 1 ended"); 
            });

            app.UseMiddleware<ConventionBasedMiddleware>();

            app.UseMiddlewareExtensionMethod();
            app.UseMiddleware<CorrelationBasedMiddleware>();

            app.Use( async (context, next) =>
            {
               //Console.WriteLine("Middleware 2 started");
               await next();
               //Console.WriteLine("Middleware 2 ended"); 
            });

            app.Use(async (context, request) =>
            {
                context.Response.Headers["sample-header"]="sample response";
                await request(context);
            });

            // app.UseSwagger();
            // app.UseSwaggerUI();
            app.UseSerilogRequestLogging();
            //app.UseExceptionHandler();
            app.MapControllers();

        //    app.UseExceptionHandler(exception =>
        //    {
               
        //    });
 
            app.Run();

            }
            catch(Exception ex)
            {
                Log.Fatal("crashed");
            }
            finally
            {
                Log.Information("done");
                Log.CloseAndFlush();
            }


                    }
    }
}
