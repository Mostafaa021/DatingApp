using Microsoft.EntityFrameworkCore;

namespace API.Helpers
{
    public class PagedList<T> : List<T>
    {
        public PagedList(IEnumerable<T> items,int pageNumber,  int pageSize, int count)
        {
            CurrentPage = pageNumber;
            PagesCount =(int) Math.Ceiling(count/(double)pageSize);
            PageSize = pageSize;
            RecordsCount = count;
            AddRange(items);
        }
        public int CurrentPage { get; set; }
        public int PagesCount { get; set; }
        public int PageSize { get; set; }
        public int RecordsCount { get; set; }

        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source , int pageSize , int pageNumber)
        {
            int count = await source.CountAsync(); 
            var items = await source.Skip((pageNumber-1) * pageSize).Take(pageSize).ToListAsync();
            return new PagedList<T>(items,pageNumber,pageSize,count);
        }
    }
}