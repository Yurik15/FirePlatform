namespace FirePlatform.WebApi.Model.Requests
{
    public class PreselectionRequest
    {
        public int UserId { get; set; }
        public bool PreselectionEnabled { get; set; }
        public bool IsRightTemplate { get; set; }
    }
}
