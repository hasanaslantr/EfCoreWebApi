using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.RequestFeatures
{
    public class PagedList<T> : List<T>
    {
        public MetaData MetaData { get; set; }
        public PagedList(List<T> item, int count, int pageNumber, int pageSize)
        {
            MetaData = new MetaData()
            {
                TotalCount = count,
                PageSize = pageSize,
                CurrentPage = pageNumber,
                TotalPage = (int)Math.Ceiling(count / (double)pageSize)
            };
            AddRange(item);
        }
        public static PagedList<T> ToPagedList(IEnumerable<T> source, int pageNumber, int PageSize)
        {
            var count = source.Count();
            var items = source
                .Skip((pageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToList();
            return new PagedList<T>(items, count, pageNumber, PageSize);
        }
    }
}
