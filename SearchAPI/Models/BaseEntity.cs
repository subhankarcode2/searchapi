using System.ComponentModel.DataAnnotations;

namespace SearchAPI.Models
{
    public class BaseEntity
    {
        [Key]
        public int Id
        {
            get;
            set;
        }
    }
}
