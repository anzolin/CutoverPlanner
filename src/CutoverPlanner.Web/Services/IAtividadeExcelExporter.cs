using CutoverPlanner.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CutoverPlanner.Web.Services
{
    /// <summary>
    /// Produces Excel documents for a collection of activities.
    /// </summary>
    public interface IAtividadeExcelExporter
    {
        /// <summary>
        /// Generates the binary contents of a workbook and a suggested file name.
        /// </summary>
        Task<(byte[] content, string fileName)> ExportAsync(IEnumerable<Atividade> atividades);
    }
}