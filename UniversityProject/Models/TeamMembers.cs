using System.ComponentModel.DataAnnotations;

namespace UniversityProject.Models
{
    public class TeamMembers
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Ad mutleq doldurumalidir"), MinLength(3, ErrorMessage = "Adin uzunluqu minimum 3 olmalidir")]
        public string Name { get; set; }
        public int StudentCardId { get; set; }
        public string Profession { get; set; }
        public DateTime CreateDate { get; set; }
         public DateTime DeadLine { get; set; }
        public string BookName { get; set; }
    }
}
