using Business.Interface;
using Tools.Interface;
using Business.Process;
using FreeDataExports;
using Moq;
using System.Data;

namespace TestProject
{
    public class ExcelExportTest
    {
        CsvManager excelManager;
       
        private IDataWorkbook workbook;
        private IConfigurationParameters _configurationParameters;

        [Fact]
        public void ExcelExport_ReturnsException_WhenConfigurationParametersAreNull()
        {
            var _logger = new Mock<ILogger>();
            excelManager = new CsvManager(_logger.Object, null);
            DataTable dataTable = new DataTable();
            Assert.ThrowsAsync<NullReferenceException>(() => excelManager.ExportToExcel(dataTable,""));
        }

        [Fact]
        public void ExcelExport_ReturnsException_WhenDataIsNull()
        {
            var _logger = new Mock<ILogger>();
            var Configuration = new Mock<IConfigurationParameters>();
            Configuration.Setup(c=>c.HeadersTitle).Returns("one;two");
            Configuration.Setup(c => c.TimeInterval).Returns(50000);
            Configuration.Setup(c => c.OutputSavePath).Returns("c:\\");
            Configuration.Setup(c => c.DateFormat).Returns("ddMMYYYY");
            Configuration.Setup(c => c.OutputFileNameDateFormat).Returns("ddMMYYYY");
            Configuration.Setup(c => c.OutputFileNameTimeFormat).Returns("HHmm");
            Configuration.Setup(c => c.OutputFileName).Returns("Test");

            excelManager = new CsvManager(_logger.Object, Configuration.Object);
            DataTable dataTable = new DataTable();
            Assert.ThrowsAsync<NullReferenceException>(() => excelManager.ExportToExcel(null,""));
        }
    }
}