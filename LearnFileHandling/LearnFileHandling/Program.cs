using LearnFileHandling.Database_Context;
using LearnFileHandling.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var constr = builder.Configuration.GetConnectionString("myconnection");
builder.Services.AddDbContext<ApplicationDbContext>(options=>options.UseSqlServer(constr));
builder.Services.AddScoped<IUserRepo,UserRepo>();
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAuthorization();

app.MapControllers();

app.Run();
