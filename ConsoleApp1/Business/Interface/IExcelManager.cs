using Axpo;
using Business.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interface
{
    public interface IExcelManager
    {
        void ExportToExcel(DataTable data);
    }
}
