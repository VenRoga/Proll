using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Proll.Api.Models.BaseModels
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(20)]
        public string Name { get; set; }
        [Required, MaxLength(150)]
        public string Email { get; set; }
        [MaxLength(20)]
        public string? Mobile { get; set; }
        [Required]
        public string PaswdHash { get; set; }
        public ICollection<UserAddress> UserAddress { get; set; }
    }
}
