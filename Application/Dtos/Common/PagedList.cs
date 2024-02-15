namespace Application.Dtos.Common
{
    public class PagedList<T> : List<T>
    {
		public int CurrentPage { get; private set; }
		public int TotalPages { get; private set; }
		public int PageSize { get; private set; }
		public int TotalCount { get; private set; }

		public bool HasPrevious => CurrentPage > 1;
		public bool HasNext => CurrentPage < TotalPages;

		public PagedList(List<T> items, int count, int pageNumber, int pageSize)
		{
			TotalCount = count;
			PageSize = pageSize > 0? pageSize : TotalCount;
			CurrentPage = pageNumber;
			TotalPages = (int)Math.Ceiling(count / (double)PageSize);

			AddRange(items);
		}

		public static async Task<PagedList<T>> ToPagedList(IQueryable<T> source, int pageNumber, int pageSize)
		{
			var count = source.Count();
			var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize > 0? pageSize : count).ToList();

			return await Task.Run(() => new PagedList<T>(items, count, pageNumber, pageSize));
		}
	}
}
