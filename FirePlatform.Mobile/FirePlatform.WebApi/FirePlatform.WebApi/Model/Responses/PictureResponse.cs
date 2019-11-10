using FirePlatform.WebApi.Model.Template;

namespace FirePlatform.WebApi.Model.Responses
{
    public class PictureResponse
    {
        public int NumID { get; set; }
        public int GroupID { get; set; }
        public Picture Picture { get; set; }
    }
}
