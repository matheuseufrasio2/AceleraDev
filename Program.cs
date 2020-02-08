using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft;
using Newtonsoft.Json;

namespace AceleraDev
{
    class Program
    {
        static string url = "https://api.codenation.dev/v1/challenge/dev-ps/generate-data?token=2f495d80465d9040c4a2037999b6a321746e7fa4";
        static string urlFinal = "https://api.codenation.dev/v1/challenge/dev-ps/submit-solution?token=2f495d80465d9040c4a2037999b6a321746e7fa4";
        static string path = @"C:\Users\Matheus Eufrásio\source\repos\AceleraDev\ArquivosGerados/answer.json";
        static string fraseJson;
        static HttpClient httpClient = new HttpClient();
        static void Main(string[] args)
        {
            fraseJson =  GetJson(url);
            GerarArquivoJson(path, fraseJson);
            CriptografiaJson criptografiaJson = JsonConvert.DeserializeObject<CriptografiaJson>(fraseJson);
            criptografiaJson.GeraDecifradoJson(criptografiaJson.numero_casas, criptografiaJson.cifrado);
            criptografiaJson.GeraResumoCriptografico(criptografiaJson.decifrado);
            criptografiaJson.AtualizaJson(path, criptografiaJson.decifrado, criptografiaJson.resumo_criptografico);
            //criptografiaJson.EnviarArquivoParaAPI();


            var content = new MultipartFormDataContent();
            httpClient.BaseAddress = new Uri(urlFinal);

            var fileContent1 = new ByteArrayContent(File.ReadAllBytes(path));

            fileContent1.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { 
                FileName = "answer"
            };

            content.Add(fileContent1);

            var result = httpClient.PostAsync("file/upload", content).Result;

            Console.WriteLine("Status: " + result.StatusCode);


            Console.ReadLine();
        }



        static string GetJson(string url)
        {
            try
            {
                using (var client = new WebClient())
                {
                    string response = client.DownloadString(url);
                    if (!string.IsNullOrEmpty(response))
                    {
                        Console.WriteLine("Começando o Processo.");
                    }
                    Console.WriteLine("Request feito a API");
                    return response;
                }
            }
            catch (Exception x)
            {
                throw new Exception(x.Message);
            }
            


        }

        private static void GerarArquivoJson(string path, string fraseJson)
        {
            try
            {
                if (!File.Exists(path))
                {
                    File.WriteAllText(path, fraseJson);
                    Console.WriteLine("Aquivo criado com sucesso.");
                }
                else
                {
                    Console.WriteLine("Já existe um arquivo no diretório.");
                }
            }
            catch (Exception x)
            {
                throw new Exception(x.Message);
            }
            
        }
    }
}


//{
//    "numero_casas":10,
//    "token":"2f495d80465d9040c4a2037999b6a321746e7fa4",
//    "cifrado":"nyx xyd wkuo dro ecob zbyfsno sxpybwkdsyx drkd dro cicdow kvbokni uxygc. bsmu vowyxc",
//    "decifrado":"",
//    "resumo_criptografico":""
//}
