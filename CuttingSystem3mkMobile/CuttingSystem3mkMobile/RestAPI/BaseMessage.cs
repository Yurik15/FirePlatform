using System;
namespace CuttingSystem3mkMobile.RestAPI
{
    public abstract class BaseMessage
    {
        protected BaseMessage()
        {
        }

        protected BaseMessage(bool didSucceed, Exception exception)
        {
            this.DidSucceed = didSucceed;
            this.RaisedException = exception;
        }

        public bool DidSucceed { get; set; }

        public Exception RaisedException { get; set; }

        public bool HasException
        {
            get { return RaisedException != null; }
        }
    }
}
