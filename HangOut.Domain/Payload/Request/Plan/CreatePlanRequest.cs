namespace HangOut.Domain.Payload.Request.Plan;

public class CreatePlanRequest
{
    public string Name { get; set; }
    public string Location { get; set; }
    public List<CreatePlanItemRequest>? PlanItems { get; set; }
}

public class CreatePlanItemRequest
{
    public TimeOnly Time { get; set; }
    public Guid BusinessId { get; set; }
}