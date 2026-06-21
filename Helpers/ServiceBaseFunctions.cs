using LogisticsERP.API.Models;

namespace LogisticsERP.API.Helpers
{
    public class ServiceBaseFunctions
    {
        protected ApiResponse<T> Ok<T> (T data, string message) =>
            new() { Success = true, Message = message , Data = data};

        protected ApiResponse<T> Fail<T>(string message) =>
            new() { Success = false, Message = message, Data = default };
    }
}
