using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApp.Infrastructure.Data;
using MyApp.Web.Batch;
using MyApp.Web.Exceptions;
using MyApp.Web.Filter;
using Quartz;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
ConfigBuilder(ref builder);

var app = builder.Build();

AddSwaggerPage(app);
app.MapControllers();
app.UseAuthorization();
app.UseAuthentication();
app.UseSession();
app.UseMiddleware<ExceptionMiddleware>();
app.UseRouting();
using (var scope = app.Services.CreateScope())
{
    var schedulerFactory = scope.ServiceProvider.GetRequiredService<ISchedulerFactory>();
    var scheduler = schedulerFactory.GetScheduler().Result;
    scheduler.Start().Wait();
}
app.Run();

static void AddSwaggerPage(WebApplication app)
{
    //if (app.Environment.IsDevelopment())
    //{
    //    app.UseSwagger();
    //    app.UseSwaggerUI();
    //}
    app.UseSwagger();
    app.UseSwaggerUI();
}
static void AddSwagger(WebApplicationBuilder builder)
{
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
}
static void AddSessionCookie(WebApplicationBuilder builder)
{
    builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
    {
        options.LoginPath = "";
        options.AccessDeniedPath = "";
        options.ExpireTimeSpan = TimeSpan.FromDays(10);
        options.Cookie.Name = "Demo_ASPNet_Custom_Identity";
    });

    builder.Services.AddSession(options =>
    {
        options.IdleTimeout = TimeSpan.FromDays(20);
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;
    });
    builder.Services.AddDistributedMemoryCache();
}
static void AddConfigDbContext(WebApplicationBuilder builder)
{
    var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
    IConfigurationRoot configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: false)
        .AddJsonFile($"appsettings.{env}.json", optional: true)
        .Build();
    builder.Services.AddSingleton<IConfiguration>(configuration);
    string connectionString = configuration.GetConnectionString("DbConnection");
    builder.Services.AddDbContext<BloggingContext>((options) => options.UseSqlite(connectionString));
}
static void AddBatchSerice(WebApplicationBuilder builder)
{
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
}
static void ConfigBuilder(ref WebApplicationBuilder builder)
{
    AddSessionCookie(builder);
    AddSwagger(builder);
    AddConfigDbContext(builder);
    builder.Services.Configure<ApiBehaviorOptions>(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    });
    builder.Services.AddScoped<ValidateModelAttribute>();
    AddBatchSerice(builder);
    ConfigPort(builder);
}
static void ConfigPort(WebApplicationBuilder builder)
{
    var configuration = builder.Configuration;
    string Host = configuration.GetValue("Host", "localhost");
    string Port = configuration.GetValue("Port", "5000");
    string baseAddress = $"http://{Host}:{Port}";
    builder.WebHost.UseUrls(baseAddress);
}