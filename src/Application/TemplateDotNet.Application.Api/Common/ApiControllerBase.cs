namespace TemplateDotNet.Application.Api.Common
{
	[ApiController]
	[Route("/api/[controller]")]
    public class ApiControllerBase : ControllerBase
	{
		protected readonly IMediator _mediator;

		public ApiControllerBase(IMediator mediator)
		{
			_mediator = mediator;
		}
	}
}

