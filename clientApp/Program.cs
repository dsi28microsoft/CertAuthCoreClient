using System;
using System.IO;
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

            // Find cert and access with password
            string certPath = "C:\\temp\\cert.pfx";
            string certPass = "Welcome1";
            var cert2 = new X509Certificate2(Path.Combine(certPath), certPass);
            var _clientHandler = new HttpClientHandler();
            _clientHandler.ClientCertificates.Add(cert2);
            _clientHandler.ClientCertificateOptions = ClientCertificateOption.Manual;

            using (var _client = new HttpClient(_clientHandler))
            {
                try
                {
                    HttpResponseMessage response = await _client.GetAsync("https://v-daizqucertauth.azurewebsites.net/api/TodoItems");
                   //HttpResponseMessage response = await _client.GetAsync("https://localhost:44373/api");
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
