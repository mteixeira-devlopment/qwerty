using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace Identity.API.Configurations
{
    public class SigningConfigurations
    {
        /// <summary>
        /// Armazena a chave de criptografia utilizada na criação dos tokens
        /// </summary>
        public SecurityKey Key { get; }

        /// <summary>
        /// Contém a chave de criptografia e o algoritimo de segurança utiliziado na geração da assinatura do token  
        /// </summary>
        public SigningCredentials SigningCredentials { get; }

        public SigningConfigurations()
        {
            using (var provider = new RSACryptoServiceProvider(2048))
                Key = new RsaSecurityKey(provider.ExportParameters(true));

            SigningCredentials = new SigningCredentials(Key, SecurityAlgorithms.RsaSha256Signature);
        }
    }
}