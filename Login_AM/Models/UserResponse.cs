using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login_AM.Models
{
    internal class UserResponse
    {
        public int id { get; set; }
        public string firstName { get; set; }
        public string lastName1 { get; set; }
        public string lastName2 { get; set; }
        public string email { get; set; }
        public string token { get; set; }
    }
}
