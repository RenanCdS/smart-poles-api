using SmartPoles.IOC;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRepositories();
var env = builder.Environment;
builder.Configuration.AddEnvironmentVariables();
builder.Configuration
.SetBasePath(Directory.GetCurrentDirectory())
.AddJsonFile("appsettings.json")
.AddJsonFile($"appsettings.{env.EnvironmentName}.json")
.AddEnvironmentVariables().Build();

var prometheusUrl = builder.Configuration["PrometheusUrl"]; 

builder.Services.AddHttpClient("Prometheus", httpClient =>
{
    httpClient.BaseAddress = new Uri(prometheusUrl);
});


var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseAuthorization();

app.MapControllers();

app.Run();
