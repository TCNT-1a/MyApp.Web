using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApp.Infrastructure.Data;
using MyApp.Web.Batch;
using MyApp.Web.Exceptions;
using MyApp.Web.Filter;
using Quartz;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

MakeBuilder(builder);

var app = builder.Build();

//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.UseAuthorization();

//
app.UseMiddleware<ExceptionMiddleware>();
app.UseRouting();
using (var scope = app.Services.CreateScope())
{
    var schedulerFactory = scope.ServiceProvider.GetRequiredService<ISchedulerFactory>();
    var scheduler = schedulerFactory.GetScheduler().Result;
    scheduler.Start().Wait();
}

//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.Run();

static void MakeBuilder(WebApplicationBuilder builder)
{
    var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
    IConfigurationRoot configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: false)
        .AddJsonFile($"appsettings.{env}.json", optional: true)
        .Build();

    builder.Services.AddSingleton<IConfigurationRoot>(configuration);

    string connectionString = configuration.GetConnectionString("DbConnection") ?? "";


    if (connectionString == null)
    {
        throw new InvalidOperationException("DbConnection connection string is missing");
    }
    builder.Services.Configure<ApiBehaviorOptions>(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    });
    builder.Services.AddControllers();
    builder.Services.AddScoped<ValidateModelAttribute>();

    //builder.Services.AddDbContext<RoosterLotteryContext>(options =>
    //    options.UseSqlServer(connectionString,
    //        sqlServerOptions => sqlServerOptions.EnableRetryOnFailure()));
    //
    builder.Services.AddDbContext<BloggingContext>((options) => options.UseSqlite(connectionString));
    builder.Services.AddQuartz(q =>
    {
        q.UseMicrosoftDependencyInjectionJobFactory();
        var CRON = new
        {
            //Repeat every at begin hour
            H = "0 0 * * * ? *",
            //Repeat every at begin minute
            M = "0 0/1 * 1/1 * ? *",
        };


        q.ScheduleJob<MyJob>(trigger => trigger
            .WithIdentity("my-job-trigger", "default")
            .WithCronSchedule(CRON.H)
        );
    });


    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    //string Host = configuration.GetValue("Host", "localhost");
    //string Port = configuration.GetValue("Port", "5000");
    //string baseAddress = $"http://{Host}:{Port}";
    //builder.WebHost.UseUrls(baseAddress);
}