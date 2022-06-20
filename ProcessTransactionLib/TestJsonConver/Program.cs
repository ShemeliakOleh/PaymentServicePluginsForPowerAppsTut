using ProcessTransactionLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestJsonConver
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //var streamObject = "{\"name\":\"285fa26f-c673-4e82-9225-22b5972730f3\",\"transaction\":{\"id\":\"E6D430DC-CBD9-EC11-BB3D-000D3AD7BB50\",\"creaditCardId\":\"962db026-30d8-ec11-a7b5-0022489d7b40\",\"amount\":2423,\"terminalId\":\"2b38276a-85b5-ec11-983f-0022489bc08f\",\"transactionType\":\"1c2ae79b-f3b4-ec11-983f-0022489bc08f\",\"currency\":\"42479c58-e29b-eb11-b1ac-000d3ab1aa35\",\"completed\":true},\"result\":\"200\",\"message\":\"Ok\"}";
            var streamObject = "{\"name\":\"285fa26f-c673-4e82-9225-22b5972730f3\",\"result\":\"200\",\"message\":\"Ok\"}";

            var serialized = JsonHelper.SerializeJson(new TransactionHistory() { Name = "name" });

            var result = JsonHelper.DeserializeJson<TransactionHistory>(serialized);
            var s = result;

            //var query = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
            //                    <entity name='tm1_contact'>
            //                    <attribute name='tm1_contactid' />
            //                    <attribute name='tm1_name' />
            //                    <attribute name='tm1_lastname' />
            //                    <attribute name='tm1_email' />
            //                    <attribute name='tm1_externalcustomerid' />
            //                    <order attribute='tm1_name' descending='false' />
            //                    <filter type='and'>
            //                      <condition attribute='tm1_contactid' operator='eq' value='{result.Transaction.CreaditCardId}' />
            //                    </filter>
            //                  </entity>
            //                </fetch>";
        }
    }
}
