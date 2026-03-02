using System.Text.Json;
using AspNetCoreBlazorInMvc.Components;
using AspNetCoreBlazorInMvc.Data;
using AspNetCoreBlazorInMvc.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents(); // Interactive Serverサービスを追加

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("LocationDb"));

var app = builder.Build();

// Seed database from JSON
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var jsonPath = Path.Combine(app.Environment.WebRootPath, "data", "locations.json");
    if (File.Exists(jsonPath) && !context.Prefectures.Any())
    {
        var jsonContent = File.ReadAllText(jsonPath);
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var prefectures = JsonSerializer.Deserialize<List<Prefecture>>(jsonContent, options);
        if (prefectures != null)
        {
            foreach (var pref in prefectures)
            {
                if (!context.Prefectures.Any(p => p.Id == pref.Id))
                {
                    context.Prefectures.Add(pref);
                }
            }
            context.SaveChanges();
        }
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

// .NET 8 以降の Blazor Interactive Server では UseAntiforgery が必須です。
// SignalR 通信時の CSRF 対策としてアンチフォージェリトークンの検証が行われます。
app.UseAntiforgery();

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
