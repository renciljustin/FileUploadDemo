using System.ComponentModel.DataAnnotations;

namespace UploadFileDemo.Core.Models
{
    public class Photo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FileName { get; set; }
    }
}