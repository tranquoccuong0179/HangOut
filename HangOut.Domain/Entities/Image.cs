using System.ComponentModel.DataAnnotations.Schema;
using HangOut.Domain.Entities.Common;
using HangOut.Domain.Enums;

namespace HangOut.Domain.Entities;

public class Image : EntityAuditBase<Guid>
{
    public string Url { get; set; }
    public EImageType ImageType { get; set; }
    public Guid ObjectId { get; set; }
    public EntityTypeEnum EntityType { get; set; }
    [NotMapped]
    public virtual Business? Business { get; set; }
    [NotMapped]
    public virtual Event? Event { get; set; }
}