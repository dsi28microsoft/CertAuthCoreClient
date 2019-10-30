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

            //// find cert in store
            //X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            //store.Open(OpenFlags.ReadOnly);
            //X509Certificate2 cert = new X509Certificate2();
            //foreach (X509Certificate2 certificate in store.Certificates)
            //{
            //    Console.WriteLine(certificate.Thumbprint);

            //    if (certificate.Thumbprint.ToLower() == "f7a3de6bae2558ee82e0a1496b9abc8ae9a4c683")
            //    {
            //        Console.WriteLine("Found");
            //        Console.WriteLine(certificate.Thumbprint);
            //        cert = new X509Certificate2(certificate);

            //    }
            //}
            //Console.WriteLine("Out");
            //_clientHandler.ClientCertificates.Add(cert);



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
