namespace BlazorApp1.Model
{
    public class ApplicationUser
    {
        public int Id { get; set; }
        public bool ScheduleWorkToday { get; set; }

        public bool Checked_In { get; set; }
        public string TodayShift { get; set; }

    }
}
