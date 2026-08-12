using System.ComponentModel.DataAnnotations;

namespace Proll.Api.Models.BaseModels
{
    [Key]
    public int Id {  get; set; }
    public string Name { get; set; }
    public string Email {  get; set; }
}
