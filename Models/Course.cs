using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WithAuthintication.Models;

namespace WithAuthintication;

public class Course
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CourseId { get; set; }

    public string Name { get; set; }
    public string Description { get; set; }

    [ForeignKey("Department")]
    public int DepartmentId {get; set;}
    public Department Department { get; set; }
    public ICollection<ClientCourse> ClientCourses { get; set; }

}
