
namespace Library.Application.Models
{
    public enum ResultErrorCode
    {
        NONE,
        NOT_FOUND = 404,
        BAD_REQUEST = 400,
        INTERNAL_ERROR = 500,
        UNAUTHORIZED = 401
    }

    public class Result
    {
        private Result(bool isSuccess, ResultErrorCode code, IEnumerable<IError> errors)
        {
            if (isSuccess && errors.Any() ||
                !isSuccess && !errors.Any())
            {
                throw new ArgumentException("Invalid Error");
            }
            IsSuccess = isSuccess;
            Errors = errors;
            Code = code;
        }
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public ResultErrorCode Code { get; }
        public IEnumerable<IError> Errors { get; }

        public static Result Success() => new(true, ResultErrorCode.NONE, []);

        public static Result Failure(ResultErrorCode code, List<IError> errors) => new(false, code, errors);
    }
}
