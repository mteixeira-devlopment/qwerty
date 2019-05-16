using System;
using System.Threading.Tasks;
using Deposit.API.Domain;
using Deposit.API.Domain.DataTransferObjects;
using Gerencianet.SDK;
using Newtonsoft.Json;

using Depos = Deposit.API.Domain.Deposit;

namespace Deposit.API.Infrastructure.Data.ExternalRepositories
{
    public class PayRepository : IPayRepository
    {
        public async Task<ChargeTransferObject.Data> CreateCharge(decimal value)
        {
            dynamic endpoint = new Endpoints(
                "Client_Id_74b485376ceef56724b1c454b1a17379dc786fb5", "Client_Secret_b9366d778d887f5cfade0c1fda64de2b8fdda2b4", true);

            var chargeBody = new ChargeBodyTransferObject(value);

            var response = JsonConvert.SerializeObject(endpoint.CreateCharge(null, chargeBody)) as string;
            var charge = JsonConvert.DeserializeObject<ChargeTransferObject>(response);

            return await Task.FromResult(charge.DataObject);
        }

        public async Task PayCreditCard(int chargeId, string paymentToken)
        {
            dynamic endpoints = new Endpoints(
                "Client_Id_74b485376ceef56724b1c454b1a17379dc786fb5", "Client_Secret_b9366d778d887f5cfade0c1fda64de2b8fdda2b4", true);

            var paymentBody = new PaymentCreditCardBodyTransferObject(paymentToken);
            var payment = paymentBody.PaymentObject;

            payment.Card.AddBillingAddress("Av Darcy Vargas", 713, "Ipiranga", "36031100", "Juiz de Fora", "MG");
            payment.Card.AddCustomer("Maycon Teixeira", "mteixeira.dev@outlook.com", "11709501677", DateTime.Now, "32991179841");

            var response = await endpoints.PayCharge(new { id = chargeId }, paymentBody);
        }
    }
}
