using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SearchAPI.Models
{
    public class Products: BaseEntity
    {
        [Required]
        public string Category { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        public long EAN { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Rating { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Vendor { get; set; }
    }
}
