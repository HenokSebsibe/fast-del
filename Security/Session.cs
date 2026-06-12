namespace FoodyExpress.Security
{
    public static class Session
    {
        public static int? CurrentUserId { get; set; }
        public static string? CurrentUserName { get; set; }
        public static string? CurrentUserRole { get; set; }
        public static bool IsAuthenticated => CurrentUserId.HasValue;

        public static void Clear()
        {
            CurrentUserId = null;
            CurrentUserName = null;
            CurrentUserRole = null;
        }
    }
}
