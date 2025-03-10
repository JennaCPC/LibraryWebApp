using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Shared.Pagination
{
    public class MembersPaginationParameters : PaginationParameters
    {
        public string SearchTerm { get; set; } = string.Empty;
        public bool? IsActive { get; set; } = null;

        public string OrderBy { get; set; } = string.Empty;
       
        public void Deconstruct(out string searchTerm, out bool? isActive, out string orderBy)
        {
            searchTerm = SearchTerm; 
            isActive = IsActive;
            orderBy = OrderBy;
        }
    }
}
