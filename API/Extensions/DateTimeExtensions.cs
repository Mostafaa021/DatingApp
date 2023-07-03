namespace API.Extensions
{
    public static class DateTimeExtensions
    {
        public static int CalculateAge(this DateOnly BirthDate){
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var age  = today.Year  - BirthDate.Year;
            if(BirthDate > today.AddYears(-age))  age--;
            return age;
            
        }
    }
}