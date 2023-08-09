namespace WebsiteStatus
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        public HttpClient? _client;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _client = new HttpClient();
            _logger.LogInformation("Started Sample Service");
            return base.StartAsync(cancellationToken);
        }
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopped Sample Service");
            if (_client != null) _client.Dispose();
   
            return base.StopAsync(cancellationToken);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var result = await _client.GetAsync("https://www.iamtimcorey.com/");
                if(result.IsSuccessStatusCode) {
                    _logger.LogInformation("The Website is up. Status code is {statuscode}", result.StatusCode);

                }
                else{
                    _logger.LogError("The Website is down. Status code is {statuscode}", result.StatusCode);
                }
                //_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}

//create service 
// Go to windows pewershell -> Run as administrator -> give below command
//sc.exe create SampleStatusService binpath= C:\temp\WebsiteStatus.exe start= auto

//delete service
//sc.exe delete SampleStatusService

