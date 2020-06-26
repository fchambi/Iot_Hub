

namespace IotHub
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using IotHub.Helpers;
    using System.Collections.Generic;
    public static class ConsultarIot
    {
        [FunctionName("ConsultarIot")]
        public static async Task<IActionResult> Run(
               [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
               [CosmosDB(
                databaseName: Constantes.COSMOS_DB_DATABASE_NAME,
                collectionName: Constantes.COSMO_DB_CONTAINER_NAME,
                ConnectionStringSetting = "StrCosmos",
                SqlQuery ="SELECT * FROM c order by c._ts desc")] IEnumerable<TempHumIot> datos,
               ILogger log)
        {
            if (datos == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(datos);
        }
    }
}
