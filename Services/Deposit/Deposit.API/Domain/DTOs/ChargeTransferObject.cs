using System;
using Newtonsoft.Json;

namespace Deposit.API.Domain.DTOs
{
    public class ChargeTransferObject
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("data")]
        public Data DataObject { get; set; }

        public class Data
        {
            [JsonProperty("charge_id")]
            public int ChargeId { get; set; }

            public string Status { get; set; }
            public int Total { get; set; }

            [JsonProperty("custom_id")]
            public int? CustomId { get; set; }

            [JsonProperty("created_at")]
            public DateTime CreatedAt { get; set; }
        }
    }
}
