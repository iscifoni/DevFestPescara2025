using System.Net;

namespace MIB.Core.Domain.Query
{
    public class HandlerResult<T>
    {
        public T? Result { get; set; }

        public bool Success { get; set; }

        public Dictionary<string, string?>? Messages { get; set; }

        public HttpStatusCode StatusCode { get; set; }
    }

    public static class HandlerResult
    {
        /// <summary>
        /// Return a default success instance of <see cref="HandlerResult{T}"/>
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="entity">Entity value</param>
        /// <returns></returns>
        public static HandlerResult<T> Success<T>(T entity)
        {
            return new HandlerResult<T>()
            {
                Result = entity,
                Success = true,
                StatusCode = HttpStatusCode.OK
            };
        }

        public static HandlerResult<T> Error<T>(Dictionary<string, string?> messages)
        {
            return new HandlerResult<T>()
            {
                Result = default,
                Success = false,
                Messages = messages,
                StatusCode = HttpStatusCode.InternalServerError
            };
        }

        public static HandlerResult<T> Error<T>(Exception ex)
        {
            return new HandlerResult<T>()
            {
                Result = default,
                Success = false,
                Messages = new Dictionary<string, string?> { { "GEN001", ex.Message } },
                StatusCode = HttpStatusCode.InternalServerError
            };
        }

        public static HandlerResult<T> NotFound<T>()
        {
            return new HandlerResult<T>()
            {
                Result = default,
                Success = false,
                Messages = new Dictionary<string, string?> { { "GEN002", "Entity not found" } },
                StatusCode = HttpStatusCode.NotFound
            };
        }

        public static HandlerResult<T> BabRequest<T>(Dictionary<string, string?> messages)
        {
            return new HandlerResult<T>()
            {
                Result = default,
                Success = false,
                Messages = messages,
                StatusCode = HttpStatusCode.BadRequest
            };
        }
    }
}