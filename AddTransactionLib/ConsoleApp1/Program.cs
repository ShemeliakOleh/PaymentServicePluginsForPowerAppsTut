using AddTransactionLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var content = "{\"Amount\":234,\"Completed\":false,\"CreaditCardId\":\"962db026-30d8-ec11-a7b5-0022489d7b40\",\"Currency\":\"42479c58-e29b-eb11-b1ac-000d3ab1aa35\",\"Id\":\"ABAE0C40-C5D9-EC11-BB3D-000D3AD7BB50\",\"TerminalId\":\"2b38276a-85b5-ec11-983f-0022489bc08f\",\"TransactionType\":\"1c2ae79b-f3b4-ec11-983f-0022489bc08f\"}";

            //var endpoint = "https://localhost:7080/api/Transaction/Create";
            var endpoint = "https://paymentservice20220518104558.azurewebsites.net/api/Transaction/Create";


            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(endpoint);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.KeepAlive = false;

            using (var writer = new StreamWriter(request.GetRequestStream()))
            {
                writer.Write(content);
            }

            try
            {
                var response = (HttpWebResponse)request.GetResponse();
            }

            catch (WebException ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
