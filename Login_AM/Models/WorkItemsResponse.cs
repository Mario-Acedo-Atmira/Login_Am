using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login_AM.Models
{
    public class WorkItemsResponse
    {
        public int id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public Boolean isAThought { get; set; }
        public int userId { get; set; }

        public List<Comments> comments { get; set; }
        public List<Valorations> valorations { get; set; }
        public List<Files> files { get; set; }
    }
}
