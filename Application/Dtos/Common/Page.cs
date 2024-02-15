namespace Application.Dtos.Common
{
    public abstract class Page
    {
        const int maxPageSize = 50;
        private int _pageSize = 0;

        public int PageNumber { get; set; } = 1;
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }
    }
}
