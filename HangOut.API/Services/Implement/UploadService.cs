using HangOut.API.Services.Interface;

namespace HangOut.API.Services.Implement;

public class UploadService : IUploadService
{
    private readonly Supabase.Client _client;
    private readonly ILogger _logger;
    public UploadService(Supabase.Client client, ILogger logger)
    {
        _client = client;
        _logger = logger;
    }
    
    public async Task<string> UploadImageAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            throw new BadHttpRequestException("Không tìm thấy file");
        }

        var allowedExtensions = new[] { ".jpeg", ".png", ".jpg", ".gif", ".bmp", ".webp" };
        var extension = Path.GetExtension(file.FileName).ToLower();

        if (!allowedExtensions.Contains(extension))
            throw new InvalidOperationException(
                "Chỉ các định dạng tệp .jpeg, .png, .jpg, .gif, .bmp, và .webp được phép tải lên.");
        try
        {
            var imageGuid = Guid.NewGuid();
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            await _client.Storage.From("hangout").Upload(
                memoryStream.ToArray(),
                $"hangout-{imageGuid}.{extension}");
            var url = _client.Storage.From("hangout").GetPublicUrl($"hangout-{imageGuid}.{extension}");
            return url;
        }
        catch (Exception e)
        {
            _logger.Error("Lỗi khi tải lên tệp lên Supabase Storage");
            throw new BadHttpRequestException("Lỗi khi tải lên tệp lên Supabase Storage");
        }
    }
}