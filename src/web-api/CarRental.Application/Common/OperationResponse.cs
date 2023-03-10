namespace CarRental.Application.Common
{
    public class OperationResponse
    {
        protected bool _focedFailedResponse;
        public OperationError? OperationError { get; set; }
        public bool CompletedWithSuccess => OperationError == null && !_focedFailedResponse;

        public OperationResponse SetAsFailureResponse(OperationError operationError)
        {
            OperationError = operationError;
            _focedFailedResponse = true;
            return this;
        }
    }

    public class OperationResponse<T> : OperationResponse
    {
        public T? Result { get; set; }

        public OperationResponse() { }

        public OperationResponse(T result)
        {
            Result = result;
        }
        public new OperationResponse<T> SetAsFailureResponse(OperationError operationError)
        {
            base.SetAsFailureResponse(operationError);
            return this;
        }
    }
}
