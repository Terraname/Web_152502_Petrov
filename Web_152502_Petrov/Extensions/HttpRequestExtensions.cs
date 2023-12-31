namespace Web_152502_Petrov.Extensions
{

    public static class HttpRequestExtensions
    {
        public static bool isAjaxRequest(this HttpRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }
    }
}
