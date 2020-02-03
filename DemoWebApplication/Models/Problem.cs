namespace DemoWebApplication.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    class Problem
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Header { get; set; }

        [Required]
        [MaxLength(100)]
        public string Description { get; set; }

        public DateTime CreatedOn { get; set; }

        public bool IsSolved { get; set; }

        public int CreatorId { get; set; }

        public virtual User Creator { get; set; }
    }
}