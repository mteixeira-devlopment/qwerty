var script = document.createElement('script');
script.type = 'text/javascript';

var random = Math.random() * 1000000;
var randomToken = parseInt(random);

script.src = `https://sandbox.gerencianet.com.br/v1/cdn/a9a414ab43db61b628dc4a91e8bf5a58/${randomToken}`;
script.async = false;

script.id = 'a9a414ab43db61b628dc4a91e8bf5a58';

if(!document.getElementById('a9a414ab43db61b628dc4a91e8bf5a58'))
  document.getElementsByTagName('head')[0].appendChild(script);

var paymentProvider = {
  pay: (paymentInformations, callback) => {
    $gn = {
      validForm: true,
      processed: false,
      done: {},
      ready: function(fn) {
        $gn.done = fn;
      }
    };

    $gn.ready(function(checkout) {
      checkout.getPaymentToken(paymentInformations, (error, response) => {
        callback(response);
      });

      checkout.getInstallments(50000,'visa', function(error, response) {
        if(error) console.log(error);
        else console.log('sucesso!');
      });
    });
  }
}

// $gn.ready(function(checkout) {

//   var callback = function(error, response) {
//     if(error) {
//       console.error(error);
//     } else {
//       console.log('callback response => ', response);
//     }
//   };

//   checkout.getPaymentToken({
//     brand: 'visa', // bandeira do cartão
//     number: '4012001038443335', // número do cartão
//     cvv: '123', // código de segurança
//     expiration_month: '05', // mês de vencimento
//     expiration_year: '2018' // ano de vencimento
//   }, callback);

//   checkout.getInstallments(50000,'visa', function(error, response) {
//     if(error) console.log(error);
//     else console.log('sucesso!');
//   });
// });

module.exports = {
  paymentProvider
}
