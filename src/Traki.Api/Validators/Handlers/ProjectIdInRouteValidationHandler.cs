using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Traki.Api.Validators.Requirements;
using Traki.Domain.Providers;
using Traki.Domain.Repositories;

namespace Traki.Api.Validators.Handlers
{
    public class ProjectIdInRouteValidationHandler : AuthorizationHandler<ProjectIdInRouteValidation>, IAuthorizationHandler
    {
        private readonly IProjectsRepository _projectsRepository;
        private readonly IClaimsProvider _claimsProvider;
        private readonly IActionContextAccessor _actionContextAccessor;
        public ProjectIdInRouteValidationHandler(IProjectsRepository theatersHandler, IClaimsProvider claimsProvider, IActionContextAccessor actionContextAccessor)
        {
            _projectsRepository = theatersHandler;
            _claimsProvider = claimsProvider;
            _actionContextAccessor = actionContextAccessor;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ProjectIdInRouteValidation requirement)
        {
            if (!_claimsProvider.TryGetUserId(out int userId))
            {
                return;
            }

            if (context.Resource is HttpContext httpContext)
            {
                int projectId = int.Parse(httpContext.GetRouteValue("projectId").ToString());
                var project = await _projectsRepository.GetProject(projectId);
                if (project == null)
                {
                    return;
                }

                if (project.AuthorId != userId)
                {
                    context.Fail();
                    return;
                }
            }

            context.Succeed(requirement);
            return;
        }
    }
}
