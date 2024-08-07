﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace UserAuth.Models
{
    public class Device
    {
        [Key]
        public int Id { get; set; }

        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public Guid ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }

        [Required]
        public string DeviceId { get; set; }

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


        //status of plan whether active or not 
        [Required]
        public bool IsPlanActive 
        {
            get
            {
                return EndDate >= DateTime.UtcNow;
            }
            set
            {
                _isPlanActive = value;
            }
        }
        

        public bool IsActive { get; set; } //deletion status false means deleted by default it's true

       
        
    }
}
