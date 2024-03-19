using Microsoft.EntityFrameworkCore;
using TajamarProyecto.Data;
using TajamarProyecto.Helpers;
using TajamarProyecto.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSession();
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<HelperMails>();
builder.Services.AddTransient<HelperPathProvider>();
string connectionString = builder.Configuration.GetConnectionString("SqlTajamar");
builder.Services.AddTransient<RepositoryUsuarios>();
builder.Services.AddTransient<RepositoryEmpresa>();
builder.Services.AddDbContext<UsuariosContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddHttpContextAccessor();
//NECESITAMOS HABILITAR EL SERVICIO DE MEMORIA CACHE
builder.Services.AddDistributedMemoryCache();
//SESSION PODEMOS CONFIGURARLO CON TIEMPO DE INACTIVIDAD
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseSession();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
