using IdentityLearningSecondTry.Data;
using IdentityLearningSecondTry.EmailConfigurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<IdentityUser,IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true).AddDefaultTokenProviders()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddSingleton<IEmailSender,EmailSender>();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddAuthentication()
   .AddGoogle(options =>
   {
       IConfigurationSection googleAuthNSection = builder.Configuration.GetSection("Authentication:Google");
       options.ClientId = googleAuthNSection["ClientId"];
       options.ClientSecret = googleAuthNSection["ClientSecret"];
   });
   //.AddFacebook(options =>
   //{
   //    IConfigurationSection FBAuthNSection =
   //    config.GetSection("Authentication:FB");
   //    options.ClientId = FBAuthNSection["ClientId"];
   //    options.ClientSecret = FBAuthNSection["ClientSecret"];
   //})
   //.AddMicrosoftAccount(microsoftOptions =>
   //{
   //    microsoftOptions.ClientId = config["Authentication:Microsoft:ClientId"];
   //    microsoftOptions.ClientSecret = config["Authentication:Microsoft:ClientSecret"];
   //})
   //.AddTwitter(twitterOptions =>
   //{
   //    twitterOptions.ConsumerKey = config["Authentication:Twitter:ConsumerAPIKey"];
   //    twitterOptions.ConsumerSecret = config["Authentication:Twitter:ConsumerSecret"];
   //    twitterOptions.RetrieveUserDetails = true;
   //});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
