using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace UserAuth.Models
{
    [Index(nameof(Customer.CustomerId), IsUnique = true)]
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        public Guid CreatedBy {  get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public Guid ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string CustomerId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }
        [Required]
        public int AllowedResources { get; set; }

        public bool IsActive { get; set; }
        
        [JsonIgnore]
        public ICollection<Device> Devices { get; set; }


    }
}
