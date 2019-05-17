using Newtonsoft.Json;

namespace Deposit.API.Domain.DataTransferObjects
{
    public class PaymentTransferObject
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("data")]
        public Data DataObject { get; set; }

        public class Data
        {
            [JsonProperty("installments")]
            public int Installments { get; set; }

            [JsonProperty("installment_value")]
            public int InstallmentValue { get; set; }

            [JsonProperty("charge_id")]
            public int ChargeId { get; set; }

            [JsonProperty("status")]
            public string Status { get; set; }

            [JsonProperty("total")]
            public int Total { get; set; }

            [JsonProperty("payment")]
            public string PaymentMethod { get; set; }
        }
    }
}