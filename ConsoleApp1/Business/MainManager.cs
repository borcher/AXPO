using Business.Interface;
using Business.Process;
using Microsoft.Extensions.Configuration;
using Tools.Interface;


namespace Business
{
    public class MainManager
    {
        private readonly IConfigurationBuilder _configurationBuilder;
        private readonly Tools.Interface.IConfiguration _configuration;
        private readonly ILogger _logger;
        private IConfigurationParameters _configurationParameters;
        private ICsvManager _csvManager;
        private IProcessManager _ProcessManager;
        private int _intervalMs;
        private string _path;

        public IConfigurationParameters ConfigurationParameters => _configurationParameters;

        public MainManager( ILogger logger,  Tools.Interface.IConfiguration configuration, IConfigurationBuilder configBuilder ) 
        {
            _logger = logger;
            _configurationBuilder = configBuilder; 
            _configuration = configuration;
            _configurationParameters = _configuration.LoadSettings();
        }

        public async Task DoProcess(IProcessManager processManager, ICsvManager csvManager,int intervalMs, string? userpath)
        {
            _path = userpath;
            _intervalMs = intervalMs;
            _csvManager = csvManager;
            _ProcessManager = processManager;
            ValidateParameters();
            await ExportToCsvTradeInformation().ConfigureAwait(false);
        }

        private void ValidateParameters()
        {
            if(string.IsNullOrEmpty(_path))
            {
                _path = _configurationParameters.OutputSavePath;
            }
            if (_intervalMs==0)
            {
                _intervalMs = _configurationParameters.TimeInterval;
            }
        }

        public async Task ExportToCsvTradeInformation()
            
        {
            try
            {
                _logger.Info("Ejecutando la primera extracción de datos...");

                var data = await _ProcessManager.StartProcessing().ConfigureAwait(false);
                if (data != null && data.Rows.Count > 0)
                {
                    await _csvManager.ExportToExcel(data, _path).ConfigureAwait(false);
                }

                _logger.Info($"Iniciando temporizador. Intervalo: {_intervalMs}ms");

                using (var timer = new PeriodicTimer(TimeSpan.FromMilliseconds(_intervalMs)))
                {
                    try
                    {
                        while (await timer.WaitForNextTickAsync().ConfigureAwait(false))
                        {
                            try
                            {
                                Console.Write($"Create new file at : {DateTime.Now.ToString()}" + Environment.NewLine);
                                if (data != null) data.Rows.Clear();
                                data = await _ProcessManager.StartProcessing().ConfigureAwait(false);
                                if (data != null && data.Rows.Count > 0)
                                {
                                    await _csvManager.ExportToExcel(data, _path).ConfigureAwait(false);
                                }
                                Console.Write($"End create new file at : {DateTime.Now.ToString() + Environment.NewLine}");
                            }
                            catch (Exception ex)
                            {
                                Console.Write($"Scheduled extract failed: {ex.Message}" + Environment.NewLine);
                            }
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        Console.Write("Scheduler stopped" + Environment.NewLine);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
        }
    }
}
