using API.Extensions;
using Application;
using Infrastructure;
using Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSwaggerGenWithAuth();
builder.Services.AddApplication()
                .AddInfrastructure(builder.Configuration);

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var initializer = services.GetRequiredService<DatabaseInitializer>();
        await initializer.Seed();
    }

    app.UseSwaggerWithUi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();



app.Run();
