namespace TemplateDotNet.Application.Api.Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        public bool IsAuthenticated => throw new NotImplementedException();

        public string Email => throw new NotImplementedException();
    }
}

