using System.ComponentModel.DataAnnotations;

namespace MathslideLearning.Models.AccountsDtos
{
    public class UpdateUserRequestDto
    {
        [MinLength(2, ErrorMessage = "Name must be at least 2 characters long.")]
        public string? Name { get; set; }
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string? Email { get; set; }
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string? Password { get; set; }
    }
}

