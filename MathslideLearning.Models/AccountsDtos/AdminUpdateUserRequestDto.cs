using System.ComponentModel.DataAnnotations;

namespace MathslideLearning.Models.AccountsDtos
{
    public class AdminUpdateUserRequestDto
    {
        [Required]
        [RegularExpression("^(Admin|Teacher|Student)$", ErrorMessage = "Role must be 'Admin', 'Teacher', or 'Student'.")]
        public string Role { get; set; }

        [Range(1, 12, ErrorMessage = "Grade must be between 1 and 12.")]
        public int? Grade { get; set; }
    }
}
