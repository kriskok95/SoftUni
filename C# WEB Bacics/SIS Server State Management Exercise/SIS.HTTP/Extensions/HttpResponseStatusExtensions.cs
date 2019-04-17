using SIS.HTTP.Enums;

namespace SIS.HTTP.Extensions
{
    public static class HttpResponseStatusExtensions
    {
        public static string GetResponseLine(this HttpResponseStatusCode statusCode)
        {
            string statusExtension = string.Empty;

            switch (statusCode)
            {
                case HttpResponseStatusCode.Ok:
                    statusExtension = "200 OK";
                    break;
                case HttpResponseStatusCode.Created:
                    statusExtension = "201 Created";
                    break;
                case HttpResponseStatusCode.Found:
                    statusExtension = "302 Found";
                    break;
                case HttpResponseStatusCode.SeeOther:
                    statusExtension = "303 See Other";
                    break;
                case HttpResponseStatusCode.BadRequest:
                    statusExtension = "400 BadRequest";
                    break;
                case HttpResponseStatusCode.Unauthorized:
                    statusExtension = "401 Unauthorized";
                    break;
                case HttpResponseStatusCode.Forbidden:
                    statusExtension = "403 Forbidden";
                    break;
                case HttpResponseStatusCode.NotFound:
                    statusExtension = "404 Not Found";
                    break;
                case HttpResponseStatusCode.InternalServerError:
                    statusExtension = "500 Internal Server Error";
                    break;
            }

            return statusExtension;
        }
    }
}
