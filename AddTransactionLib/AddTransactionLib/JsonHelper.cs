using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace AddTransactionLib
{
    public class JsonHelper
    {

        public static string SerializeJson<T>(T response)
        {
            if (response == null) throw new InvalidPluginExecutionException("Response is null;");
            using (var memoryStream = new MemoryStream())
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
                serializer.WriteObject(memoryStream, response);
                byte[] json = memoryStream.ToArray();
                return Encoding.UTF8.GetString(json, 0, json.Length);
            }
        }


        public static T DeserializeJson<T>(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) throw new InvalidPluginExecutionException("json is null;");

            T obj;
            using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                DataContractJsonSerializer deSerializer = new DataContractJsonSerializer(typeof(T));
                try
                {
                    obj = (T)deSerializer.ReadObject(memoryStream);
                }
                catch
                {
                    obj = default;
                }
            }

            return obj;

        }



    }
}
