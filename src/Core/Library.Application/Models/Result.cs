
namespace Library.Application.Models
{
    public class Result
    {
        private Result(bool isSuccess, ErrorStatus status, IEnumerable<Error> errors)
        {
            if (isSuccess && errors.Any() ||
                !isSuccess && !errors.Any())
            {
                throw new ArgumentException("Invalid Error");
            }
            IsSuccess = isSuccess;
            Errors = errors;
            Status = status;
        }
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public ErrorStatus Status { get; }
        public IEnumerable<Error> Errors { get; }

        public static Result Success() => new(true, ErrorStatus.NONE, []);

        public static Result Failure(ErrorStatus status, List<Error> errors) => new(false, status, errors);
    }
}
