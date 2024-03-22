namespace TemplateDotNet.Application.Api.Common.Interfaces
{
    public interface ICurrentUserService
	{
		bool IsAuthenticated { get; }
		string Email { get; }
	}
}

