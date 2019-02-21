using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Identity.API.Configurations
{
    public class SigningConfigurations
    {
        private const string SecretKey = "qw3rty#@integration";

        /// <summary>
        /// Armazena a chave de criptografia utilizada na criação dos tokens
        /// </summary>
        public SymmetricSecurityKey Key { get; }

        /// <summary>
        /// Contém a chave de criptografia e o algoritimo de segurança utiliziado
        /// na geração da assinatura do token  
        /// </summary>
        public SigningCredentials SigningCredentials { get; }

        public SigningConfigurations()
        {
            Key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));
            SigningCredentials = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);
        }
    }
}