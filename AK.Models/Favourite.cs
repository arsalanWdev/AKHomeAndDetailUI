using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AK.Models
{
    public class Favourite
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } 

        [Required]
        public int GalleryId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; } 

        [ForeignKey("GalleryId")]
        public virtual Gallery Gallery
        {
            get; set;
        }
    }
}