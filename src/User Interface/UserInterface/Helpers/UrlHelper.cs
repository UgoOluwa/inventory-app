using Microsoft.AspNetCore.Http;

namespace UserInterface.Helpers
{
    public class UrlHelper
    {
        public static string GetPaginatationQueryString(HttpRequest httpRequest, decimal pageNumber)
        {
            return !httpRequest.QueryString.HasValue ? QueryString.Create("pageNumber", $"{pageNumber}").ToUriComponent() :
                !httpRequest.Query.ContainsKey("pageNumber") ? httpRequest.QueryString.Add("pageNumber", $"{pageNumber}").ToUriComponent() : httpRequest.QueryString.Value.Replace($"pageNumber={httpRequest.Query["pageNumber"]}", $"pageNumber={pageNumber}");

        }
    }
}