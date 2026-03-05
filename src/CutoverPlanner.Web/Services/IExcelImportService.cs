using System.IO;
using System.Threading.Tasks;

namespace CutoverPlanner.Web.Services
{
    public interface IExcelImportService
    {
        Task<(int atividades, int deps, int areas, int endpoints)> ImportAsync(Stream xlsx);
    }
}