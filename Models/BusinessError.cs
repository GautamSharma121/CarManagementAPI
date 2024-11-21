namespace CarModelManagementAPI.Models
{
    public class BusinessError
    {
        private string errorCode;
        private string errorDesc;
        private Exception exception;

        public BusinessError()
        {
            this.exception = null;
        }
        public BusinessError(string desc)
        {
            this.errorDesc = desc;
           // this.ErrorCode = code.ToString();
        }
        public string ErrorCode
        {
            get { return errorCode; }
            set { errorCode = value; }
        }

        public string ErrorDesc
        {
            get { return errorDesc; }
            set { errorDesc = value; }
        }

        public Exception Exception
        {
            get { return exception; }
            set { exception = value; }
        }
    }
}
