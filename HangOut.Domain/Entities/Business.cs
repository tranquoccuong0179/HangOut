using HangOut.Domain.Entities.Common;

namespace HangOut.Domain.Entities;

public class Business : EntityAuditBase<Guid>
{
    public string Name { get; set; }
    public bool Active { get; set; }
    public string? Vibe { get; set; }
    public string Latitude { get; set; }
    public string Longitude { get; set; }
    public string Address { get; set; }
    public string Province { get; set; }
    public string? Description { get; set; }
    public string? MainImageUrl { get; set; }
    public Guid AccountId { get; set; }
    
    
    public virtual Account Account { get; set; }
    public virtual ICollection<Image> Images { get; set; } = new List<Image>();
    public virtual ICollection<PlanItem> PlanItems { get; set; } = new List<PlanItem>();
    public virtual ICollection<Voucher> Vouchers { get; set; } = new List<Voucher>();
    public virtual ICollection<Event> Events { get; set; } = new List<Event>();
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}