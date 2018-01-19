using System.ComponentModel.DataAnnotations;

namespace SnackShare.Api.Data.Entities
{
    public class User
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
