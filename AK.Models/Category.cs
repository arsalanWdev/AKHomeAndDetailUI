using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AK.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }


        [Required]
        [MaxLength(30)]
        [DisplayName("Category Name")]
        public string Name { get; set; }


        [DisplayName("Display Order")]
        [Range(1,100,ErrorMessage ="Invalid Value")]
        public int DisplayOrder { get; set; }



    }
}
