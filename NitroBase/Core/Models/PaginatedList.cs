using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class PaginatedList<T>
    {
        const int MaxPageSize = 500;
        private int _pageSize;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

        public int PageNumber { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public ICollection<T> Items { get; set; }

        public PaginatedList()
        {
            
        }
    }
    
}
