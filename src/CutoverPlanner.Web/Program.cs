using CutoverPlanner.Web.Data;
using CutoverPlanner.Web.Repositories;
using CutoverPlanner.Web.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(opts =>
    opts.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<ExcelImportService>();
builder.Services.AddScoped<IAreaService, AreaService>();
builder.Services.AddScoped<IExecutorService, ExecutorService>();
builder.Services.AddScoped<ISistemaService, SistemaService>();
builder.Services.AddScoped<IAtividadeService, AtividadeService>();
builder.Services.AddScoped<IPlanoService, PlanoService>();
builder.Services.AddScoped<IMarcoService, MarcoService>();

// repository and exporter abstractions used by controllers
builder.Services.AddScoped<IAreaRepository, AreaRepository>();
builder.Services.AddScoped<IAtividadeRepository, AtividadeRepository>();
builder.Services.AddScoped<IExecutorRepository, ExecutorRepository>();
builder.Services.AddScoped<IMarcoRepository, MarcoRepository>();
builder.Services.AddScoped<IPlanoRepository, PlanoRepository>();
builder.Services.AddScoped<ISistemaRepository, SistemaRepository>();

var app = builder.Build();

// Apply migrations automatically
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
