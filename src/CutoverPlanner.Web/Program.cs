using CutoverPlanner.Web.Data;
using CutoverPlanner.Web.Services;
using CutoverPlanner.Web.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(opts =>
    opts.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IExcelImportService, CutoverPlanner.Web.Services.ExcelImportService>();
builder.Services.AddScoped<ICriticalPathService, CutoverPlanner.Web.Services.CriticalPathService>();

// repository and exporter abstractions used by controllers
builder.Services.AddScoped<CutoverPlanner.Web.Repositories.IAtividadeRepository, CutoverPlanner.Web.Repositories.AtividadeRepository>();
builder.Services.AddScoped<CutoverPlanner.Web.Services.IAtividadeExcelExporter, CutoverPlanner.Web.Services.AtividadeExcelExporter>();

var app = builder.Build();

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
