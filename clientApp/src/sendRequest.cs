using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;


namespace client
{
    class Program
    {
        static void Main(string[] args)
        {

            var p = new Program();
            p.RunAsync().Wait();
        }

        private async Task RunAsync()
        {
            var handler = new WebRequestHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            var cert = GetClientCert();
            handler.ClientCertificates.Add(cert);

            handler.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors error)
            {
                //Ignore errors
                return true;
            };
            handler.UseProxy = false;

            using (var client = new HttpClient(handler))
            {
                client.BaseAddress = new Uri("https://certdemo.azurewebsites.net/");
                                                    //client.BaseAddress = new Uri("https://localhost:44315/");

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application / json"));

                var response = await client.GetAsync("api / values");
                if (response.IsSuccessStatusCode)
                {
                    var str = await response.Content.ReadAsStringAsync();

                    Console.WriteLine(str);
                }
                else
                {
                    Console.WriteLine("FAIL:" +response.StatusCode);
                }

            }

        }

        private static X509Certificate GetClientCert()
        {
            X509Store store = null;
            try
            {
                store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
                store.Open(OpenFlags.OpenExistingOnly | OpenFlags.ReadOnly);

                var certs = store.Certificates.Find(X509FindType.FindBySubjectName, "NetworkingDemoClientCert2", true);

                if (certs.Count == 1)
                {
                    var cert = certs[0];
                    return cert;
                }
            }
            finally
            {
                if (store != null)
                    store.Close();
            }

            return null;
        }
    }
}