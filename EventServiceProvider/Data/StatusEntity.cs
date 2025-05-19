using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EventServiceProvider.Data;

public class StatusEntity
{
    [Key]
    public string StatusId { get; set; } = null!;
    public string StatusName { get; set; } = null!;
}
