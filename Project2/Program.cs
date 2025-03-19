using Microsoft.EntityFrameworkCore;
using Project2.Hubs;
using Project2.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TrialContext>(options =>
     options.UseSqlServer(builder.Configuration.GetConnectionString("Trialj")));

builder.Services.AddRazorPages();
builder.Services.AddSignalR();

var app = builder.Build();

app.UseStaticFiles();
app.MapRazorPages();
app.MapHub<MovieHub>("/movieHub");

// Mapping: redirect từ "/" đến "/Movies/Director_Movie"
app.MapGet("/", context =>
{
    context.Response.Redirect("/Movies/Director_Movie");
    return Task.CompletedTask;
});

app.Run();
