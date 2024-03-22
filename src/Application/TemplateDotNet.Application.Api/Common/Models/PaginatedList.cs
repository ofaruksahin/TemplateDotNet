namespace TemplateDotNet.Application.Api.Common.Models
{
    public class PaginatedList<T>
		where T : class
	{
		public List<T> Items { get; set; }
		public int PageNumber { get; set; }
		public int TotalPages { get; set; }
		public int TotalCount { get; set; }

		public PaginatedList(List<T> items, int count,int pageNumber,int pageSize)
		{
			PageNumber = pageNumber;
			TotalPages = (int)Math.Ceiling(count / (double)pageSize);
			TotalCount = count;
			Items = items;
		}

		public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageNumber,int pageSize)
		{
			var count = await source.CountAsync();
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

			return new PaginatedList<T>(items, count, pageNumber, pageSize);
        } 
	}
}

