namespace Project1.Models
{
    public class UsersEachLocation
    {
        public int OfficeLocation { get; set; }
        public List<int> UserIds { get; set; }

        public UsersEachLocation(int officeLocation, List<int> userIds)
        {
            OfficeLocation = officeLocation;
            UserIds = userIds;
        }
    }
}
