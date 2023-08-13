namespace API.Helpers
{
    public class UserParams : PaginationParams
    {   
        public string CurrentUserName { get; set; }
        public string Gender { get; set; }

        public int MaxAge { get; set; } = 80 ;
        public int MinAge { get; set;} = 18 ;

        public string OrderBy { get; set; } = "lastActive";

    }
}