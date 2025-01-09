using DocuSync.Application.Authorization.Requirements;
using DocuSync.Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocuSync.Application.Authorization.Handlers
{
    public class ClientAccessAuthorizationHandler
    : AuthorizationHandler<ClientAccessRequirement>
    {
        private readonly ICurrentUser _currentUser;

        public ClientAccessAuthorizationHandler(ICurrentUser currentUser)
        {
            _currentUser = currentUser;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            ClientAccessRequirement requirement)
        {
            if (_currentUser.IsAdmin)
            {
                context.Succeed(requirement);
                return;
            }

            if (await _currentUser.CanAccessClientAsync(requirement.ClientId))
            {
                context.Succeed(requirement);
            }
        }
    }
}
