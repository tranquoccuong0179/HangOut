using System.Text.Json.Serialization;
using HangOut.API.Common.Extensions;
using HangOut.API.Common.Middlewares;
using HangOut.Domain;
using HangOut.Domain.Configurations;
using HangOut.Domain.Constants;
using HangOut.Domain.Persistence;
using HangOut.Repository;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog(SerilogConfig.Configure);
Log.Information("Starting HangOut API up");
try
{
    builder.Services.AddCors(options =>
    {
        options.AddPolicy(name: CorsConstant.PolicyName,
            policy =>
            {
                policy.WithOrigins("*")
                    .AllowAnyHeader().AllowAnyMethod();
            });
    });
    builder.Services.AddControllers().AddJsonOptions(x =>
    {
        x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        // x.JsonSerializerOptions.Converters.Add(new TimeOnlyJsonConverter());
    });
    builder.Services.AddDomainServices(builder.Configuration);
    builder.Services.AddRepositoryServices();
    builder.Services.AddServices();
    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddJwtValidation(builder.Configuration);
    builder.Services.AddConfigSwagger();
    builder.Services.AddHttpContextAccessor();

    var app = builder.Build();

    if (app.Environment.IsDevelopment() || app.Environment.IsProduction() || app.Environment.IsStaging())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "HangOut API V1");
            c.InjectStylesheet("/assets/css/kkk.css");
        });
    }
    using (var scope = app.Services.CreateScope())
    {
        try
        {
            var hangOutContextSeed = scope.ServiceProvider.GetRequiredService<HangOutContextSeed>();
            await hangOutContextSeed.InitializeAsync();
        }
        catch (Exception e)
        {
            Log.Error(e, "An error occurred while seeding the database.");
            throw; 
        }
    }
    app.UseMiddleware<GlobalException>();
    app.UseCors(CorsConstant.PolicyName);
    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseDefaultFiles();
    app.UseStaticFiles();
    app.MapControllers();
    
    app.Run();
}
catch (Exception ex)
{
    string type = ex.GetType().Name;
    Log.Fatal(ex, $"Unhandled: {ex.Message}");
    if (type.Equals("StopTheHostException", StringComparison.Ordinal))
    {
        throw;
    }
}
finally
{
    Log.Information("Shut down HangOut API complete");
    Log.CloseAndFlush();
}
