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
    public static class GetService
    {
        [FunctionName("GetService")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
                                                    [Table("tblServices")] CloudTable tblService,
                                                    ILogger log)
        {
            log.LogInformation("GetService called");
            string responseMessage = null;
            string serviceIdString = req.Query["ServiceID"].ToString();
            if (serviceIdString == null || serviceIdString.Equals("")) {
                Service[] services = await new ServiceService().All(log, tblService);
                responseMessage = JsonConvert.SerializeObject(services);
            } else {
                Guid ServiceID = Guid.Parse(serviceIdString);            
                Service service = await new ServiceService().FindService(log, tblService, ServiceID);
                if (service == null) {
                    return new NotFoundResult();
                }
                responseMessage = JsonConvert.SerializeObject(service);
            }

            return new OkObjectResult(responseMessage);
        }
    }
}
