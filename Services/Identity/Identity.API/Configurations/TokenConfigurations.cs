namespace Identity.API.Configurations
{
    public class TokenConfigurations
    {
        /// <summary>
        ///´Define quem poderá utilizar os tokens gerados. Restringe ou permite o acesso de determinados grupos em conjuntos com o issuer
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// Define ou nomeia o gerador do token
        /// </summary>
        public string Issuer { get; set; }

        public int Seconds { get; set; }
    }
}