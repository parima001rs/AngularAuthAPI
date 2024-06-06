﻿using System.ComponentModel.DataAnnotations.Schema;
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
        public string ApplicationId { get; set; }

        [Required]
        [ForeignKey("Customer")]
        public string CustId { get; set; }

        [JsonIgnore]
        public Customer Customer { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        private bool _isPlanActive;

        [Required]
        public bool IsPlanActive 
        { 
            get { return EndDate <= DateTime.UtcNow; }
            set { _isPlanActive = value; }
        }
        
    }
}
