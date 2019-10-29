using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace clientApp
{
    class sendRequest2
    {
        public void go()
        {
            X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly);
            X509Certificate2 cert = new X509Certificate2();
            foreach (X509Certificate2 certificate in store.Certificates)
            {
                Console.WriteLine(certificate.Subject);
                if(certificate.Subject == "CN = NetworkingDemoClientCert2")
                {
                    Console.WriteLine("Found");
                    Console.WriteLine(certificate.Subject);
                    cert = certificate;
          
                }
            }


            using (var cert2 = new X509Certificate2())
            {
                
                var _clientHandler = new HttpClientHandler();
                _clientHandler.ClientCertificates.Add(cert2);
                _clientHandler.ClientCertificateOptions = ClientCertificateOption.Manual;
                //var myModel = new Dictionary<string, string>
                //   {
                //       { "property1","value" },
                //       { "property2","value" },
                //   };
                //using (var content = new FormUrlEncodedContent(myModel))
                using (var _client = new HttpClient(_clientHandler))
                using (HttpResponseMessage response = _client.GetAsync("https://v-daizqucertauth.azurewebsites.net/api/TodoItems").Result)
                {
                    response.EnsureSuccessStatusCode();
                    string jsonString = response.Content.ReadAsStringAsync().Result;
                    //var myClass = JsonConvert.DeserializeObject<MyClass>(jsonString);
                }
            }

        }

    }
}
