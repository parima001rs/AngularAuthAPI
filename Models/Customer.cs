﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UserAuth.Models
{
    public class Customer
    {
        
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; private set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string CustomerId { get; private set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        public Customer()
        {
            Id = Guid.NewGuid();
            CustomerId = GenerateUniqueId();
        }

        private string GenerateUniqueId()
        {
            return "CUST" + Guid.NewGuid().ToString("N");
        }

    }
}