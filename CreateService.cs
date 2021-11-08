using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Cosmos.Table;

namespace Rasputin.TM
{
    public static class CreateService
    {
        [FunctionName("CreateService")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
                                                    [Table("tblServices")] CloudTable tblService,
                                                    ILogger log)
        {
            log.LogInformation("CreateService called");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            string name = data?.name;

            Service Service = await new ServiceService().InsertService(log, tblService, name);

            string responseMessage = JsonConvert.SerializeObject(Service);
            return new OkObjectResult(responseMessage);
        }
    }
}
