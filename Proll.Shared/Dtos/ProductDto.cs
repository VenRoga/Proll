using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Text.Json.Serialization;

public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ImageUrl { get; set; }
    public decimal Price { get; set; }
    public string Unit { get; set; }

    [JsonIgnore] //for UI
    public int Quantity { get; set;  }
}

