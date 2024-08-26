using Restaurants.Common.Enum;

namespace Restaurants.Common
{
    public class OperationResult
    {
        public OperationResult(string error, FailureReason failureReason)
        {
            Error = error;
            FailureReason = failureReason;
        }

        protected OperationResult()
        { }

        public string Error { get; }
        public FailureReason? FailureReason { get; }
        public bool IsSuccess
            => !FailureReason.HasValue;

        public static OperationResult Success
            => new OperationResult();
    }

    public class OperationResult<T> : OperationResult
    {
        public OperationResult(T result)
        {
            Result = result;
        }

        public OperationResult(string error, FailureReason failureReason)
            : base(error, failureReason)
        { }

        public T Result { get; }
    }
}
