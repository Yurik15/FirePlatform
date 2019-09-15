namespace FirePlatform.WebApi.Model.Requests
{
    public class ItemsRequest
    {
        public int GroupId { get; set; }
        public int ItemId { get; set; }
        public string Value { get; set; }
        public int UserId { get; set; }
    }
}
