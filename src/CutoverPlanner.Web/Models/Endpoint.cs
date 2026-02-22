
using System.ComponentModel.DataAnnotations;

namespace CutoverPlanner.Web.Models
{
    public class Endpoint
    {
        public int Id { get; set; }
        [MaxLength(150)] public string? Sistemas { get; set; }
        [MaxLength(150)] public string? TipoIntegracao { get; set; }
        [MaxLength(150)] public string? Barramento { get; set; }
        [MaxLength(150)] public string? Integracao { get; set; }
        [MaxLength(500)] public string? Url { get; set; }
    }
}
