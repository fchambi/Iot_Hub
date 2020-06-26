using IoTHubTrigger = Microsoft.Azure.WebJobs.EventHubTriggerAttribute;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.EventHubs;
using System.Text;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Build.Utilities;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents.Client;
using System;
using Microsoft.AspNetCore.Http;
using IotHub.Helpers;

namespace IotHub
{
    public class Function1
    {
        private static HttpClient client = new HttpClient();

        [FunctionName("Function1")]
        public static async System.Threading.Tasks.Task RunAsync([IoTHubTrigger("messages/events", Connection = "ConnectionIOT")]EventData message, ILogger log)
        {
            log.LogInformation($"C# IoT Hub trigger function processed a message: {Encoding.UTF8.GetString(message.Body.Array)}");
            var data = JsonConvert.DeserializeObject<TempHumIot>(Encoding.UTF8.GetString(message.Body.Array));
            var datos = new TempHumIot
            {
                messageId = data.messageId,
                deviceId = data.deviceId,
                temperature = data.temperature,
                humidity = data.humidity
            };
            await Insertar(datos);
            
        }
        private static async Task<IActionResult> Insertar(TempHumIot tempe)
        {
            IActionResult returnValue = null;
            DocumentClient client;
            client = new DocumentClient(new Uri(Constantes.COSMOS_DB_URI), Constantes.COSMOS_DB_PRIMMARY_KEY);
            try
            {
                var collectionUri = UriFactory.CreateDocumentCollectionUri(Constantes.COSMOS_DB_DATABASE_NAME, Constantes.COSMO_DB_CONTAINER_NAME);
                var documentResponse = await client.CreateDocumentAsync(collectionUri, tempe);
                returnValue = new OkObjectResult(tempe);
            }
            catch (Exception ex)
            {
                returnValue = new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            return returnValue;
        }

    }
}