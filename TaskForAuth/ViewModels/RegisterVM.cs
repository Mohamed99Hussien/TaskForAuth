using System.ComponentModel.DataAnnotations;

namespace TaskForAuth.ViewModels
{
    public class RegisterVM
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string SchoolName { get; set; }

    }
}
