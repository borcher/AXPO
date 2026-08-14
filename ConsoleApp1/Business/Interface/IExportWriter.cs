using System.Data;
using System.Threading.Tasks;

namespace Business.Interface
{
    public interface IExportWriter
    {
        Task ExportAsync(DataTable data, string userPath);
    }
}
