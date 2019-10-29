using System;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace clientApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            go().Wait();
        }

        public static async Task go()
        {
            X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly);
            X509Certificate2 cert = new X509Certificate2();
            foreach (X509Certificate2 certificate in store.Certificates)
            {
                Console.WriteLine(certificate.Thumbprint);
                if (certificate.Thumbprint.ToLower() == "2f181017b83520044370bac773fd07d97f92eea9")
                {
                    Console.WriteLine("Found");
                    Console.WriteLine(certificate.Thumbprint);
                    cert = new X509Certificate2(certificate);

                }
            }
            Console.WriteLine("Out");

            var _clientHandler = new HttpClientHandler();
            _clientHandler.ClientCertificates.Add(cert);
            _clientHandler.ClientCertificateOptions = ClientCertificateOption.Manual;

            using (var _client = new HttpClient(_clientHandler))
            {
                try
                {
                    HttpResponseMessage response = await _client.GetAsync("https://v-daizqucertauth.azurewebsites.net/api/TodoItems");
                    response.EnsureSuccessStatusCode();
                    Console.WriteLine(response.Content);
                    string jsonString = response.Content.ReadAsStringAsync().Result;

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }

            }
         

        }
    }
}
