using HangOut.Domain.Entities.Common;

namespace HangOut.Domain.Entities;

public class Event : EntityAuditBase<Guid>
{
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime DueDate { get; set; }
    public string Location { get; set; }
    public bool Active { get; set; }
    public string? Description { get; set; }
    public string Latitude { get; set; }
    public string Longitude { get; set; }
    public Guid BusinessId { get; set; }
    
    public virtual Business Business { get; set; }
    public ICollection<Image> Images { get; set; } = new List<Image>();
    
}