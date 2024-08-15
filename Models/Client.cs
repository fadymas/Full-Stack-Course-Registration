using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WithAuthintication.Data;
using WithAuthintication.Models;

namespace WithAuthintication
{
    public class Client
    {
        [Key]
        public string UserId { get; set; }
        [DataType(DataType.Date)]
        public DateOnly data_of_birth {get; set;}

       

        public virtual ApplicationUser User { get; set; }
        
        [ForeignKey("Department")]
        public int DepartmentId { get; set; }
        public  Department Department { get; set; }
        public ICollection<ClientCourse> ClientCourses { get; set; }

    }
}   
