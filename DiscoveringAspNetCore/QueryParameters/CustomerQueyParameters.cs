using System.Linq;

namespace DiscoveringAsp.netCore.QueryParameters
{
    public class CustomerQueyParameters
    {
        private const int maxPageCount = 100;

        private int _pageCount = 100;

        public int Page { get; set; } = 1;

        public int PageCount { get { return _pageCount; } set { _pageCount = (value > maxPageCount) ? maxPageCount : value; } }

        public bool HasQuery { get { return !string.IsNullOrEmpty(Query); } }

        public string Query { get; set; }

        public string OrderBy { get; set; } = "FirstName";

        public bool Descending
        {
            get
            {
                if (!string.IsNullOrEmpty(OrderBy))
                {
                    return OrderBy.Split(' ').Last().ToLowerInvariant().StartsWith("desc");
                }

                return false;
            }
        }



    }
}
