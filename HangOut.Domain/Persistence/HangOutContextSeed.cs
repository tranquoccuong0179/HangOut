using System.Security.Cryptography;
using System.Text;
using HangOut.Domain.Entities;
using HangOut.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace HangOut.Domain.Persistence;

public class HangOutContextSeed
{
    private readonly HangOutContext _context;
    private readonly ILogger _logger;
    public HangOutContextSeed(HangOutContext context, ILogger logger)
    {
        _context = context;
        _logger = logger;
    }
    
    public async Task InitializeAsync()
    {
        try
        {
            if (_context.Database.IsSqlServer())
            {
                await _context.Database.MigrateAsync();
            }
        }
        catch (Exception e)
        {
            _logger.Error(e, "An error occurred while migrating the database");
            throw;
        }
    }public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            _logger.Error(e, "An error occurred while seeding the database");
            throw;
        }
    }
    public async Task TrySeedAsync()
    {
        if (!_context.Account.Any())
        {
            await _context.Account.AddRangeAsync(
                new Account()
                {
                    Id = Guid.Parse("01972b33-9943-77fb-8867-e1be705ec1b9"),
                    Phone = "0123456789",
                    Email = "admin@hangout.com",
                    Role = ERoleEnum.Admin,
                    CreatedDate = DateTime.UtcNow,
                    Active = true,
                    Password = HashPassword("123456"),
                }
            );
        }
    }
    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }
}