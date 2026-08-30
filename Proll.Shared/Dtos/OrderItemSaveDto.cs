using System.ComponentModel.DataAnnotations;

public class OrderItemSaveDto
{
    [Required]
    public int Id { get; set; }
    [Required, Range(1, int.MaxValue)]
    public int Quantity { get; set; }
}
