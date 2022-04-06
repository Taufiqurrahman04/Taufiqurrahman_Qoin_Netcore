namespace Taufiqurrahman_Test_Qoin.Services
{
    public class Test01Parameters
    {
       // const int _maxPageSize = 50;
        public int pageNumber { get; set; } = 1;
        private int _pageSize=20;
        public int pageSize
        {
            get { return _pageSize; }
            set { _pageSize = value; }
        }
    }
}
