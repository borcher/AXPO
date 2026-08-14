using Axpo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools.Configuration;

namespace Business.Interface
{
    public interface IProcessManager {
        //void Start();
        Task<DataTable> StartProcessing();
    }
}
