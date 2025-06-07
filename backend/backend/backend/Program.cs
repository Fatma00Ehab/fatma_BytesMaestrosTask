using backend.DataAccess;
using backend.DataAccess.IRepository;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Http.Features;
using System.Text.Json.Serialization;
using backend.Data.IRepository;
using backend.Data;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("defaultDbContext"));
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });


// Add services to the container.

builder.Services.AddControllers();


builder.Services.AddCors(options =>
{
    options.AddPolicy("MyPolicy",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200") 
                  .AllowAnyMethod() 
                  .AllowAnyHeader(); 
        });
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IDataRepository<Product>, ProductRepository>();

builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


 

app.UseHttpsRedirection();

app.UseAuthorization();



app.UseCors("MyPolicy");


app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Resources")),
    RequestPath = new PathString("/Resources")
});
 

app.MapControllers();

app.Run();
