using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LibraryNetCoreAPI.Tests.Mocks
{
    public class AuthorizationServiceMock : IAuthorizationService
    {
        public AuthorizationResult authorizationResult { get; set; }
        public Task<AuthorizationResult> AuthorizeAsync(ClaimsPrincipal user, object resource, IEnumerable<IAuthorizationRequirement> requirements)
        {
            //throw new NotImplementedException();
            return Task.FromResult(authorizationResult); //devuelve success o failed según la necesidad de la prueba
        }

        public Task<AuthorizationResult> AuthorizeAsync(ClaimsPrincipal user, object resource, string policyName)
        {
            //throw new NotImplementedException();
            return Task.FromResult(authorizationResult); //devuelve success o failed según la necesidad de la prueba
        }    
    }
}
