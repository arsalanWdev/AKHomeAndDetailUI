using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AK.Models
{
    public class Feedback
    {
        [Key]
        public int FeedbackId { get; set; }
        [Required]
        public string CustomerId { get; set; } // Customer who gives feedback
        public ApplicationUser Customer { get; set; } // Navigation property to customer

        [Required]
        public string DesignerId { get; set; } // Designer receiving feedback
        public ApplicationUser Designer { get; set; } // Navigation property to designer

        [Required]
        [StringLength(500, ErrorMessage = "Feedback can't exceed 500 characters.")]
        public string Comment { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
