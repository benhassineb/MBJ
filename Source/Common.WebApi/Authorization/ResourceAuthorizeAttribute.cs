using Microsoft.AspNetCore.Mvc;

namespace Common.WebApi.Authorization
{
    public class ResourceAuthorizeAttribute : TypeFilterAttribute
    {
        public ResourceAuthorizeAttribute(string resourceName, ResourceGrant requiredGrants) : base(typeof(ResourceAuthorizeFilter))
        {
            Arguments = new object[] {resourceName, requiredGrants};
        }
    }
}
