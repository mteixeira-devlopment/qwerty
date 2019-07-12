using Newtonsoft.Json;

namespace Deposit.API.Domain.DTOs
{
    public class ChargeBodyTransferObject
    {
        [JsonProperty("items")]
        public Product[] Items { get; private set; }

        [JsonProperty("shippings")]
        public Shipping[] Shippings { get; private set; }

        public ChargeBodyTransferObject(decimal value)
        {
            var product = new Product(value);
            Items = new[] { product };

            var shipping = new Shipping();
            Shippings = new[] { shipping };
        }

        public class Product
        {
            [JsonProperty("name")]
            public string Name => "Credit Bet";

            [JsonProperty("value")]
            public decimal Value { get; private set; }

            [JsonProperty("amount")]
            public int Amount => 1;

            public Product(decimal value) => Value = value;
        }

        public class Shipping
        {
            [JsonProperty("name")]
            public string Name => "None Shipping";

            [JsonProperty("value")]
            public decimal Value => 0M;
        }
    }
}