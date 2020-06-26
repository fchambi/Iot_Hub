using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace IotHub
{
    public class TempHumIot
    {
         

        [JsonProperty("messageId")]
        public string messageId { get; set; }
        [JsonProperty("deviceId")]
        public string deviceId { get; set; }
        [JsonProperty("temperature")]
        public string temperature { get; set; }
        [JsonProperty("humidity")]
        public string humidity { get; set; }
        }
    }


