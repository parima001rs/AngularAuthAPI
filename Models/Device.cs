using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace UserAuth.Models
{
    public class Device
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; private set; }

        [Required]
        public Guid DeviceId { get; set; }

        [Required]
        public Guid ApplicationId { get; set; }

        [Required]
        [ForeignKey("Customer")]
        public string CustId { get; set; }

        [JsonIgnore]
        public Customer Customer { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Required]
        public bool IsPlanActive {  get; set; } //status of plan whether active or not 

        public bool IsActive { get; set; } //deletion status false means deleted by default it's true

        /*
        public Device()
        {
            IsActive = true;
            StartDate = DateTime.UtcNow;
            EndDate = DateTime.UtcNow.AddDays(30);
        }
        */
        
    }
}
