using InvoicingSystem.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

///builder.Services.AddControllers();
builder.Services.AddControllersWithViews();
// Add services to the container.
builder.Services.AddRazorPages(); // Ensure Razor Pages services are added

// Register the repository interface and implementation
builder.Services.AddScoped<IProductRepository, ProductCsvRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

builder.Services.AddScoped<IInvoiceRepository, CsvInvoiceRepository>();
builder.Services.AddScoped<IPurchasedProductRepository, CsvInvoiceRepository>();

// Register services
builder.Services.AddScoped<InvoiceService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

var app = builder.Build();




// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    //app.UseSwaggerUI();
}

app.UseHttpsRedirection();


// Enable static file serving from wwwroot
app.UseStaticFiles();



app.UseRouting();
app.UseAuthorization();
// Set default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Product}/{action=View}");

app.MapRazorPages(); // Make sure Razor Pages are also mapped
app.MapControllers();

app.Run();
