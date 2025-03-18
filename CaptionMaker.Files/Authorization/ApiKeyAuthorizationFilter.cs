using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace CaptionMaker.Files.Authorization
{
    public class ApiKeyAuthorizationFilter : IAuthorizationFilter
    {
        private readonly IOptions<CaptionMakerFilesOptions> _options;

        public ApiKeyAuthorizationFilter(IOptions<CaptionMakerFilesOptions> options)
        {
            this._options = options;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            string authorizationHeader = context.HttpContext.Request.Headers.Authorization.FirstOrDefault();

            if (String.IsNullOrEmpty(authorizationHeader))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            string[] authorizationHeaderValues = authorizationHeader.Split(' ');

            if (authorizationHeaderValues.Length != 2)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            if (authorizationHeaderValues[0] != "Bearer" || authorizationHeaderValues[1] != this._options.Value.ApiKey)
                context.Result = new UnauthorizedResult();
        }
    }
}
