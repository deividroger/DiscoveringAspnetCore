using System.ComponentModel.DataAnnotations;

namespace DiscoveringAsp.netCore.Dtos
{
    public class CustomerUpdateDto
    {
        [Required(ErrorMessage = "Please give the FirstName")]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please give the LastName")]
        [StringLength(100)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please give the Age")]
        [Range(0, 100)]
        public int Age { get; set; }

    }
}
