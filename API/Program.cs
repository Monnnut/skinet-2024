using API.Middleware;
using Core.Interfaces;
using infrastructure.Data;
using infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
//establish DBcontext service
//need to confirm connection string in app.setting
builder.Services.AddDbContext<StoreContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
}
);
//AddScoped to http request
builder.Services.AddScoped<IProductRepository,ProductRepository>();
builder.Services.AddScoped(typeof(IGenericRepository<>),typeof(GenericRepository<>));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//services use to transfer data to different host
builder.Services.AddCors();
//one instance and up for lifetime
//IConnectionMultiplexer used to interect with Redis
//main entry point for estab and manage connections to redis instances
builder.Services.AddSingleton<IConnectionMultiplexer>(config =>
{
    var connString = builder.Configuration.GetConnectionString("Redis") ?? throw new Exception("Cannot get redis connection string");
    var configuration = ConfigurationOptions.Parse(connString, true);
    return ConnectionMultiplexer.Connect(configuration);
}
);

builder.Services.AddSingleton<ICartService, CartService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();
//cors need to be between middleware and mapcontroller
//can send request to api server as long as its from the origin
//browser sercurity feature
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod()
.WithOrigins("http://localhost:4200","https://localhost:4200"));

app.MapControllers();

try
{
    //creates scope(a temperory container for services)from dependency injection
    //scope are used to manage service lifetimes, ensuring that services are disposed when not in use
    using var scope = app.Services.CreateScope();
    //retrieve service provider from scope, give access to service
    var services = scope.ServiceProvider;
    //get StoreContext from service proveder
    var context = services.GetRequiredService<StoreContext>();
    //Applies any pending migration
    await context.Database.MigrateAsync();
    //populate default database after migration applied
    await StoreContextSeed.SeedAsync(context);
}
catch (Exception ex)
{
    Console.WriteLine(ex);
    throw;
}

app.Run();
