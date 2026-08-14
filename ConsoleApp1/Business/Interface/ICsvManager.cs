using Axpo;
using Tools.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interface
{
    public interface ICsvManager
    {
        Task ExportToExcel(DataTable data, string userPath);
    }
}
