using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Logging;

namespace Rasputin.TM {
    public class ServiceService {
        public async Task<Service> InsertService(ILogger log, CloudTable tblService, string name)
        {
            Service Service = new Service(name);
            TableOperation operation = TableOperation.Insert(Service);
            await tblService.ExecuteAsync(operation);
            return Service;
        }

        public async Task<Service> FindService(ILogger log, CloudTable tblService, Guid ServiceID)
        {
            string pk = "p1";
            string rk = ServiceID.ToString();
            log.LogInformation($"FindService: {pk},{rk}");
            TableOperation operation = TableOperation.Retrieve(pk, rk);
            try {
                return (Service)await tblService.ExecuteAsync(operation);
            } catch(Exception ex) {
                log.LogWarning(ex, "FindService", ServiceID);
                return null;
            }
        }

        public async Task<Service[]> All(ILogger log, CloudTable tblService)
        {
            log.LogInformation($"All");
            List<Service> result = new List<Service>();
            TableQuery<Service> query = new TableQuery<Service>();
            TableContinuationToken continuationToken = null;
            try {
                do {
                var page = await tblService.ExecuteQuerySegmentedAsync(query, continuationToken);
                continuationToken = page.ContinuationToken;
                result.AddRange(page.Results);
                } while(continuationToken != null);
                return result.ToArray();
            } catch(Exception ex) {
                log.LogWarning(ex, "All");
                return null;
            }
        }

        public async Task DeleteService(ILogger log, CloudTable tblService, Service Service) 
        {
            log.LogInformation($"DeleteService: {Service.ServiceID}");
            TableOperation operation = TableOperation.Delete(Service);
            await tblService.ExecuteAsync(operation);
        }
    }
}