using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WithAuthintication;

public class Department
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int Fees { get; set; }

    public string Name { get; set; }

    public  ICollection<Course> Courses { get; set; }
    public ICollection<Client> Clients { get; set; }
}
