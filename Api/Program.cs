using Api.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServicesConfiguration(builder.Configuration);
builder.Services.AddIdentityConfiguration();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Minha API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseSecurity();
app.UseIdentityConfiguration();

app.MapControllers();
app.Run();
