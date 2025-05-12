using System.Diagnostics;
using System.Text;

class RabbitMqServiceManager
{
    private Process? publisherProcess;
    private Process? consumerProcess;

    public void StartPublisher()
    {
        StartProcess("DataCaptureService.exe");
    }

    public void StartConsumer()
    {
        StartProcess("ImageTransformationServices.exe");
    }

    public void StopAll()
    {
        publisherProcess?.Kill(true);
        consumerProcess?.Kill(true);
        publisherProcess = null;
        consumerProcess = null;
    }


    private void StartProcess(string file)
    {
        Process process = new Process();
        process.StartInfo.FileName = file;
        process.StartInfo.Arguments = "-n";
        process.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
        process.Start();
        process.WaitForExit();
    }

    public async Task<object> CheckRabbitMqStatus(string queueName)
    {
        using var client = new HttpClient();
        var credentials = Encoding.ASCII.GetBytes("guest:guest");
        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(credentials));

        var uri = $"http://localhost:15672/api/queues/%2F/{queueName}";
        try
        {
            var res = await client.GetAsync(uri);
            res.EnsureSuccessStatusCode();
            var json = await res.Content.ReadAsStringAsync();
            dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(json)!;

            return new
            {
                queue = queueName,
                messages = (int)data.messages,
                consumers = (int)data.consumers,
                status = (int)data.messages > 100 ? "Possibly Stuck" : "Healthy"
            };
        }
        catch (Exception ex)
        {
            return new { error = ex.Message };
        }
    }
}