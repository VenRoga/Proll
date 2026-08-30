using Proll.Api.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Proll.Api.Models.BaseModels
{
    public class OrderItem
    {
        [Key]
        public long Id { get; set; }

        public int OrderId { get; set; }
        public virtual Order Order { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public int Quantity { get; set; }
        public string Unit { get; set; }
        public string ProductImageUrl { get; set; }
    }
}
