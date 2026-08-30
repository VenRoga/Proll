using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices.Marshalling;
using System.Text;

public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ImageUrl { get; set; }
    public decimal Price { get; set; }
    public string Unit { get; set; }
}


public class AddressDto
{
    public int Id { get; set; }
    [Required]
    public string Address { get; set; }
    [Required]
    public string Name { get; set; }
    public bool IsDefault { get; set; }
}

