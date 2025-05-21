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
    }
}