using Microsoft.AspNetCore.Mvc;

namespace CaptionMaker.Files.Authorization
{
    public class ApiKeyAttribute : ServiceFilterAttribute
    {
        public ApiKeyAttribute() : base(typeof(ApiKeyAuthorizationFilter))
        {

        }
    }
}
