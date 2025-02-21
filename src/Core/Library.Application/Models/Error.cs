
namespace Library.Application.Models
{
    public enum ErrorStatus
    {
        NONE,
        NOT_FOUND = 404,
        BAD_REQUEST = 400,
        INTERNAL_ERROR = 500,
        UNAUTHORIZED = 401
    }

    public record Error(string Path, string Msg);
   
}
