namespace Login_AM.Models
{
    public class Comments
    {
        public int id { get; set; }
        public int userId { get; set; }
        public int workItemId { get; set; }
        public string comment { get; set; }
    }
}