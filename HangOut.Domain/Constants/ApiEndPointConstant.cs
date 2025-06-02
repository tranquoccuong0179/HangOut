namespace HangOut.Domain.Constants;

public static class ApiEndPointConstant
{
    public const string RootEndPoint = "/api";
    public const string ApiVersion = "/v1";
    public const string ApiEndpoint = RootEndPoint + ApiVersion;
    
    public static class Authentication
    {
        public const string AuthenticationEndpoint = ApiEndpoint + "/auth";
        public const string Login = AuthenticationEndpoint + "/login";
        public const string Otp = AuthenticationEndpoint + "/otp";
        public const string ForgetPassword = AuthenticationEndpoint + "/forget-password";
    }

    public static class Account
    {
        public const string AccountEndpoint = ApiEndpoint + "/accounts";
        public const string Register = AccountEndpoint + "/register";
    }

    public static class User
    {
        public const string UserEndpoint = ApiEndpoint + "/users";
        public const string Profile = UserEndpoint + "/profile";
        public const string UserById = UserEndpoint + "/{id}";
        public const string UserByIdRemove = UserEndpoint + "/{id}/remove";
    }

    public static class Plan
    {
        public const string PlanEndpoint = ApiEndpoint + "/plans";
        public const string PlanWithId = PlanEndpoint + "/{id}";
        public const string PlanItemsByPlanId = PlanEndpoint + "/{id}/plan-items";
    }

    public static class PlanItem
    {
        public const string PlanItemEndpoint = ApiEndpoint + "/plan-items";
        public const string PlanItemWithId = PlanItemEndpoint + "/{id}";
    }
}