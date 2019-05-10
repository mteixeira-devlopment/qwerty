using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Deposit.API.Domain;
using Gerencianet.SDK;
using Newtonsoft.Json;
using Depos = Deposit.API.Domain.Deposit;

namespace Deposit.API.Infrastructure.Data.ExternalRepositories
{
    public class PayRepository : IPayRepository
    {
        private async Task<int> CreateCharge(decimal value)
        {
            dynamic endpoint = new Endpoints(
                "Client_Id_74b485376ceef56724b1c454b1a17379dc786fb5", "Client_Secret_b9366d778d887f5cfade0c1fda64de2b8fdda2b4", true);

            var body = new
            {
                items = new[]
                {
                    new
                    {
                        name = "Bet Credit",
                        value,
                        amount = 1
                    }
                },
                shippings = new[]
                {
                    new
                    {
                        name = "None Shipping",
                        value = 0
                    }
                }
            };

            dynamic chargeId = endpoint.CreateCharge(null, body);

            System.IO.File.WriteAllText("teste.txt", JsonConvert.SerializeObject(chargeId));

            return await Task.FromResult(chargeId);
        }

        public async Task PayCreditCard(Depos deposit, string creditCardMask, string paymentToken)
        {
            dynamic endpoints = new Endpoints(
                "Client_Id_74b485376ceef56724b1c454b1a17379dc786fb5", "Client_Secret_b9366d778d887f5cfade0c1fda64de2b8fdda2b4", true);

            var param = new
            {
                id = await CreateCharge(deposit.Value)
            };

            var body = new
            {
                payment = new
                {
                    credit_card = new
                    {
                        installments = 1,
                        payment_token = paymentToken,
                        billing_address = new
                        {
                            street = "Av. JK",
                            number = 909,
                            neighborhood = "Bauxita",
                            zipcode = "35400000",
                            city = "Ouro Preto",
                            state = "MG"
                        },
                        customer = new
                        {
                            name = "Gorbadoc Oldbuck",
                            email = "oldbuck@gerencianet.com.br",
                            cpf = "94271564656",
                            birth = "1977-01-15",
                            phone_number = "5144916523"
                        }
                    }
                }
            };

            var response = endpoints.PayCharge(param, body);
        }
    }
}
