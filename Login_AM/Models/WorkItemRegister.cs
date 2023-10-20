using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login_AM.Models
{
    public class WorkItemRegister
    {
       

        

        public int id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public bool isAThought { get; set; }
        public int userId { get; set; }
        public WorkItemRegister(int id, string titulo, string descripcion, bool isThought, int userid)
        {
            this.id = id;
            this.title = titulo;
            this.description = descripcion;
            this.isAThought = isThought;
            this.userId = userid;
        }
    }
}
