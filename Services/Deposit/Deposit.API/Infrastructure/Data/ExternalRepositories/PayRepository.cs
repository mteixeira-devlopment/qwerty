using System;
using System.Threading.Tasks;
using Deposit.API.Domain;
using Deposit.API.Domain.DataTransferObjects;
using Gerencianet.SDK;
using Newtonsoft.Json;

namespace Deposit.API.Infrastructure.Data.ExternalRepositories
{
    public class ExternalResponseContentNullException : Exception
    {

    }

    public class ExternalResponse<TResponseModel>
    {
        public bool Success { get; }

        public TResponseModel Content { get; private set; }
        public string Error { get; private set; }

        public ExternalResponse(bool success) => Success = success;

        public ExternalResponse<TResponseModel> ReplySuccessful(TResponseModel content)
        {
            Content = content;
            return this;
        }

        public ExternalResponse<TResponseModel> ReplyFail(string error)
        {
            Error = error;
            return this;
        }
    }

    public class ExternalRepository
    {
        public async Task<ExternalResponse<TResponseModel>> Call<TResponseModel>(Func<object> caller)
        {
            try
            {
                var response = caller.Invoke();
                var castedResponse = await CastExternalResponse<TResponseModel>(response);

                return new ExternalResponse<TResponseModel>(true).ReplySuccessful(castedResponse);
            }
            catch (Exception exception)
            {
                return new ExternalResponse<TResponseModel>(false).ReplyFail(exception.Message);
            }
        }

        private async Task<TResponseModel> CastExternalResponse<TResponseModel>(object callResponse)
        {
            var serializedResponse = JsonConvert.SerializeObject(callResponse);
            var deserializedResponse = JsonConvert.DeserializeObject<TResponseModel>(serializedResponse);

            return await Task.FromResult(deserializedResponse);
        }
    }

    public class PayExternalRepository : ExternalRepository, IPayExternalRepository
    {
        public async Task<ExternalResponse<ChargeTransferObject>> CreateCharge(ChargeBodyTransferObject chargeBody)
        {
            dynamic endpoint = new Endpoints(
                "Client_Id_74b485376ceef56724b1c454b1a17379dc786fb5", "Client_Secret_b9366d778d887f5cfade0c1fda64de2b8fdda2b4", true);

            var createChargeResponse = await Call<ChargeTransferObject>(() => endpoint.CreateCharge(null, chargeBody));
            return createChargeResponse;
        }

        public async Task<ExternalResponse<PaymentTransferObject>> PayCreditCard(int chargeId, PaymentCreditCardBodyTransferObject paymentBody)
        {
            dynamic endpoints = new Endpoints(
                "Client_Id_74b485376ceef56724b1c454b1a17379dc786fb5", "Client_Secret_b9366d778d887f5cfade0c1fda64de2b8fdda2b4", true);

            var payChargeResponse = await Call<PaymentTransferObject>(() => endpoints.PayCharge(new {id = chargeId}, paymentBody));
            return payChargeResponse;
        }
    }
}
