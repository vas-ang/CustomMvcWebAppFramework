namespace DemoWebApplication.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class User
    {
        public string Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public virtual ICollection<Problem> Problems { get; set; } = new HashSet<Problem>();
    }
}
