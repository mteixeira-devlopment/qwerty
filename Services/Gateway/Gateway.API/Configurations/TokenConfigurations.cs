using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Gateway.API.Configurations
{
    public class TokenConfigurations
    {
        /// <summary>
        ///´Define quem poderá utilizar os tokens gerados. Restringe ou permite o acesso de determinados grupos em conjuntos com o issuer
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// Define e nomeia o gerador do token
        /// </summary>
        public string Issuer { get; set; }

        private const string SigningCredentialsKey = "qw3rty#@integration";

        public SymmetricSecurityKey SigningCredentialsSymmetricKey { get; } =
            new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SigningCredentialsKey));
    }
}