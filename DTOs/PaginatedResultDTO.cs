using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    public class PaginatedResultDTO<T>
    {
        public List<T> Posts { get; set; }  
        public int? PreviousPage { get; set; }
        public int CurrentPage { get; set; }
        public int? NextPage { get; set; }  
        public int TotalCount { get; set; }
        public string ? NextURL { get; set; }
    }
}
