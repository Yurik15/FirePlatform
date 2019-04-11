using System;
namespace CuttingSystem3mkMobile.RestAPI
{
    public class ServiceStatusMessage : BaseMessage
    {
        public string ResponseMessage { get; set; }

        public string StatusCode { get; set; }

        public string Content { get; set; }

        public override string ToString()
        {
            return $"{StatusCode}{Environment.NewLine}{ResponseMessage}";
        }
    }

    public class ServiceStatusMessage<TEntity> : ServiceStatusMessage //where TEntity : class
    {
        public TEntity Entity { get; set; }

        public bool HasEntity => Entity != null;
    }

    public class ServiceStatusOperationSuccessMessage : ServiceStatusMessage
    {
        public bool Entity { get; set; }
    }
}
