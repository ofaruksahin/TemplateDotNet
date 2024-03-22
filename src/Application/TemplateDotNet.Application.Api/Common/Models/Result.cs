namespace TemplateDotNet.Application.Api.Common.Models
{
	public class Result
	{
		public object Data { get; set; }
		public bool Succeeded { get; set; }
		public string[] Errors { get; set; }

		public Result(object data,bool succeded, string[] errors)
		{
			Data = data;
			Succeeded = succeded;
			Errors = errors;
		}

		public static Result Success(object data)
		{
			return new Result(data, true, Array.Empty<string>());
		}

		public static Result Failure()
		{
			return new Result(new NoContent(), false, Array.Empty<string>());
		}

		public static Result Failure(string[] errors)
		{
			return new Result(new NoContent(), false, errors);
		}
	}
}

