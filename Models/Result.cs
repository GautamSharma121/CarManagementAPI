namespace CarModelManagementAPI.Models
{
    public class Result<T>
    {
        public Result()
        {
            errorList = new List<BusinessError>();
            isSuccess = true;
        }
        #region private properties

        private bool isSuccess;
        private List<BusinessError> errorList;
        private T data;
        private int totalCount;

        #endregion

        #region Public Properties

        public T Data
        {
            get { return data; }
            set { data = value; }
        }

        public bool IsSuccess
        {
            get { return isSuccess; }
            set { isSuccess = value; }
        }

        public List<BusinessError> ErrorList
        {
            get { return errorList; }
            set { errorList = value; }
        }
        /// <summary>
        /// contains the total number of records for a result. This is not same the count of the records in data.
        /// </summary>
        public int TotalRecordCount
        {
            get { return totalCount; }
            set { totalCount = value; }
        }
        #endregion
    }
}