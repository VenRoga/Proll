using Proll.Api.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Proll.Api.Models.BaseModels
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string Name { get; set; }
        [Required, MinLength(200)]
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        [Required]
        public string Unit { get; set; }

        public static Product[] GetSeedData()
        {
            const string BaseImageUrl = "";

            Product[] products = 
            [
                new () {Id = 1, Name = "Avo", ImageUrl = $"avo.png", Unit = "each", Price = 1.99m, },
                new () {Id = 2, Name = "Bra", ImageUrl = $"Bra.png", Unit = "kg", Price = 2.00m, },
                new () {Id = 3, Name = "Cav", ImageUrl = $"Cav.png", Unit = "each", Price = 13.99m, }
            ];

            return products;
                
        }
    }
}
