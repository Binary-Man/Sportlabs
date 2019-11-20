using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsLabs.Models
{
    public class Team
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        public int Id { get; set; }
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Name cannot be longer than 50 characters or less than 3 characters.")]
        [Required]
        public string Name { get; set; }
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Country cannot be longer than 50 characters or less than 3 characters.")]
        [Required]
        public string Country { get; set; }
        [Required]
        public bool Eliminated { get; set; }

        public IEnumerable<string> Countries { get; set; }

    }
}