namespace CollegeApp.Models
{
    public static class CollegeRepository
    {
        //in memory class so static
        public static List<Student> Students { get; set; } =
            new List<Student>() {
            new Student{Id=1,Name="Aum",Email="aghadiyal@yahoo.com",Address="Vadodara"},
            new Student{Id=2,Name="Rash",Email="rashmikap2003@gmail.com",Address="Hyd"}
            };


    }
}
