using Project1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Threading.Tasks;



namespace Project1.Models
{
    public class ApplicationUser 
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public int OfficeLocation { get; set; }
        public bool CheckedIn { get; set; }
        public List<WorkPattern> WorkPatterns { get; set; }
    }
}



