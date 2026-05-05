using RazorpayRouteDemo.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register Razorpay service
builder.Services.AddScoped<RazorpayService>();

var app = builder.Build();

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();   // 🔥 MUST BE HERE

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.UseStaticFiles();

app.Run();