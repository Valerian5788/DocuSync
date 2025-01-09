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
    public class AdminAuthorizationHandler : AuthorizationHandler<AdminRequirement>
    {
        private readonly ICurrentUser _currentUser;

        public AdminAuthorizationHandler(ICurrentUser currentUser)
        {
            _currentUser = currentUser;
        }

        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            AdminRequirement requirement)
        {
            if (_currentUser.IsAdmin)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
