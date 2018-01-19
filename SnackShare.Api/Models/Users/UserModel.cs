using System.ComponentModel.DataAnnotations;

namespace SnackShare.Api.Models.Users
{
    public class UserModel
    {
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string FirstName { get; set; }
        [Required, MaxLength(50)]
        public string LastName { get; set; }

        [Required, MaxLength(100)]
        public string EmailAddress { get; set; }
    }
}
