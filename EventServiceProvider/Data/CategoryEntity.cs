using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventServiceProvider.Data;

public class CategoryEntity
{
    [Key]
    public string CategoryId { get; set; } = null!;
    public string CategoryName { get; set; } = null!;


}
