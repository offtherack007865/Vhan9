using log4net;
using log4net.Config;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Vhan9.data;
using Vhan9.data.Models;
using Scalar.AspNetCore;


ILog log = LogManager.GetLogger(typeof(Program));


// configure logging via log4net

string log4netConfigFullFilename =
    Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "log4net.config");

var fileInfo = new FileInfo(log4netConfigFullFilename);
if (fileInfo.Exists)
    log4net.Config.XmlConfigurator.Configure(fileInfo);
else
    throw new InvalidOperationException("No log config file found");

log.Info($"Started TennCareWith2PassAuth9.WebApiLand Program");

var builder = WebApplication.CreateBuilder(args);

// Add service and create Policy with options
builder.Services.AddCors(options =>
    options.AddPolicy("MyPolicy", builder =>
    {
        builder.WithOrigins("http://webservices:4042", "http://localhost:4200", "http://localhost:5003")
            .AllowAnyMethod()
            .AllowCredentials()
            .AllowAnyHeader();
    }));

builder.Services.Configure<IISServerOptions>(options =>
{
    options.AutomaticAuthentication = false;
});

string projectPath = AppDomain.CurrentDomain.BaseDirectory;
IConfigurationRoot configuration = new ConfigurationBuilder()
    .SetBasePath(projectPath)
    .AddJsonFile(MyConstants.AppSettingsFile)
    .Build();

//Data Source=sqldata; Initial Catalog=Production Monitoring; Integrated Security=False; User ID=dwproduction;Password=dwproduction
var withEfConnection = configuration.GetConnectionString(MyConstants.ConnectionString);
builder.Services.AddDbContextPool<SimplifyVbcAdt8Context>(options => options.UseSqlServer(
    withEfConnection,
    sqlServerOptions => sqlServerOptions.CommandTimeout(3600))
);

builder.Services.AddControllers()
    .AddNewtonsoftJson();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}
app.UseRouting();

app.UseCors("MyPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();