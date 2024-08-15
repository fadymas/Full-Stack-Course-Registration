namespace WithAuthintication.Models
{
    public class ClientCourse
    {
        public string ClientId { get; set; }
        public Client Client { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}
