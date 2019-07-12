using System;
using Newtonsoft.Json;

namespace Deposit.API.Domain.DTOs
{
    public class PaymentCreditCardBodyTransferObject
    {
        [JsonProperty("payment")]
        public Payment PaymentObject { get; set; }

        public PaymentCreditCardBodyTransferObject(string paymentToken)
        {
            PaymentObject = new Payment(paymentToken);
        }

        public class Payment
        {
            [JsonProperty("credit_card")]
            public CreditCard Card { get; private set; }

            public Payment(string paymentToken)
                => Card = new CreditCard(paymentToken);

            public class CreditCard
            {
                [JsonProperty("installments")]
                public int Installments => 1;

                [JsonProperty("payment_token")]
                public string PaymentToken { get; private set; }

                [JsonProperty("billing_address")]
                public BillingAddress BillingAddressObject { get; private set; }

                [JsonProperty("customer")]
                public Customer CustomerObject { get; private set; }

                public CreditCard(string paymentToken) => PaymentToken = paymentToken;

                public void AddBillingAddress(string street, int number, string neighborhood, string zipCode, string city, string state)
                    => BillingAddressObject = new BillingAddress(street, number, neighborhood, zipCode, city, state);

                public void AddCustomer(string name, string email, string cpf, DateTime birthDate, string phoneNumber)
                {
                    var birth = birthDate.ToString("yyyy-MM-dd");
                    var customer = new Customer(name, email, cpf, birth, phoneNumber);

                    CustomerObject = customer;
                }

                public class BillingAddress
                {
                    [JsonProperty("street")]
                    public string Street { get; private set; }

                    [JsonProperty("number")]
                    public int Number { get; private set; }

                    [JsonProperty("neighborhood")]
                    public string Neighborhood { get; private set; }

                    [JsonProperty("zipcode")]
                    public string ZipCode { get; private set; }

                    [JsonProperty("city")]
                    public string City { get; private set; }

                    [JsonProperty("state")]
                    public string State { get; private set; }

                    public BillingAddress(string street, int number, string neighborhood, string zipCode, string city, string state)
                    {
                        Street = street;
                        Number = number;
                        Neighborhood = neighborhood;
                        ZipCode = zipCode;
                        City = city;
                        State = state;
                    }
                }

                public class Customer
                {
                    [JsonProperty("name")]
                    public string Name { get; private set; }

                    [JsonProperty("email")]
                    public string Email { get; private set; }

                    [JsonProperty("cpf")]
                    public string Cpf { get; private set; }

                    [JsonProperty("birth")]
                    public string Birth { get; private set; }

                    [JsonProperty("phone_number")]
                    public string PhoneNumber { get; set; }

                    public Customer(string name, string email, string cpf, string birth, string phoneNumber)
                    {
                        Name = name;
                        Email = email;
                        Cpf = cpf;
                        Birth = birth;
                        PhoneNumber = phoneNumber;
                    }
                }
            }
        }
    }
}