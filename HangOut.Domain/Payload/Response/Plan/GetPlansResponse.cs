namespace HangOut.Domain.Payload.Response.Plan;

public class GetPlansResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Location { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? LastModifiedDate { get; set; }
    public List<GetPlanItemsResponse> PlanItems { get; set; } = new();
}