namespace Taufiqurrahman_Test_Qoin.Services
{
    public static class ExceptionHandler
    {
        public static Exception GetMessageException(this Exception ex)
        {
            if (ex.InnerException == null) return ex;

            return ex.InnerException.GetMessageException();
        }
    }
}
