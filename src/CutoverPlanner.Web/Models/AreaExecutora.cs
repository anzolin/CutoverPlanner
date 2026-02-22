
using System.ComponentModel.DataAnnotations;

namespace CutoverPlanner.Web.Models
{
    public class AreaExecutora
    {
        public int Id { get; set; }
        [MaxLength(150)] public string? Gerencia { get; set; }
        [MaxLength(200)] public string? NomeAreaExecutora { get; set; }
        [MaxLength(150)] public string? Torre { get; set; }
        [MaxLength(200)] public string? Responsavel { get; set; }
        [MaxLength(200)] public string? Executor { get; set; }
    }
}
