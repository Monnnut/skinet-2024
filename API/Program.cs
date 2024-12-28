using Core.Interfaces;
using infrastructure.Data;
using Microsoft.EntityFrameworkCore;

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

var app = builder.Build();

// Configure the HTTP request pipeline.

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
