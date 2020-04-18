using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AceleraDev
{
    class CriptografiaJson
    {
        public int numero_casas { get; set; }
        public string token { get; set; }
        public string cifrado { get; set; }
        public string decifrado { get; set; }
        public string resumo_criptografico { get; set; }

        public void GeraDecifradoJson(int numerodeCasas, string codificado)
        {
            try
            {
                char[] temp = codificado.ToCharArray();

                for (int i = 0; i < temp.Length; i++)
                {
                    int letra = (int)temp[i];
                    if (letra >= 'a' && letra <= 'z')
                    {
                        if (letra <= 'j')
                        {
                            letra += 16;
                        }
                        else
                        {
                            letra -= 10;
                        }
                    }

                    temp[i] = (char)letra;
                }
                decifrado = new string(temp);
                Console.WriteLine($"Json completamente decifrado: {decifrado}");
            }
            catch (Exception x)
            {
                throw new Exception(x.Message);
            }
            
        }


        public void GeraResumoCriptografico(string decodificado)
        {
            try
            {
                byte[] buffer = Encoding.Default.GetBytes(decodificado);
                SHA1CryptoServiceProvider cryptoTransformSHA1 = new SHA1CryptoServiceProvider();
                string hash = BitConverter.ToString(cryptoTransformSHA1.ComputeHash(buffer)).Replace("-", "");
                resumo_criptografico = hash.ToLower();
                Console.WriteLine($"Resumo da Criptografia feito: {resumo_criptografico}");
            }
            catch (Exception x)
            {
                throw new Exception(x.Message);
            }
        }

        public void AtualizaJson(string path, string decifrado, string resumo)
        {
            try
            {
                string text = File.ReadAllText(path);
                text = text.Replace("\"decifrado\":\"\"", $"\"decifrado\":\"{decifrado}\"");
                text = text.Replace("\"resumo_criptografico\":\"\"", $"\"resumo_criptografico\":\"{resumo}\"");
                File.WriteAllText(path, text);
                Console.WriteLine("Arquivo atualizado com sucesso.");
            }
            catch (Exception x)
            {
                throw new Exception(x.Message);
            }
            
        }

        //public HttpResponseMessage EnviarArquivoParaAPI(string arquivo, string urlApi)
        //{
        //    try
        //    {
        //        byte[] fileBytes = File.ReadAllBytes(arquivo);

        //        HttpContent bytesContent = new ByteArrayContent(fileBytes);

        //        // Submit the form using HttpClient and 
        //        // create form data as Multipart (enctype="multipart/form-data")

        //        using (var client = new HttpClient())
        //        using (var formData = new MultipartFormDataContent())
        //        {
        //            formData.Add(bytesContent, "answer", Path.GetFileName(arquivo));
        //            HttpResponseMessage response = client.PostAsync(urlApi, formData).Result;

        //            // equivalent of pressing the submit button on the form
        //            if (!response.IsSuccessStatusCode)
        //            {
        //                return null;
        //            }
        //            return response.Content.ReadAsStreamAsync().Result;
        //        }
        //    }
        //    catch (Exception x)
        //    {
        //        throw new Exception(x.Message);
        //    }
        //}

    }
}
