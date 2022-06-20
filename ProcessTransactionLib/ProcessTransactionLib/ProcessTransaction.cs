using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using ProcessTransactionLib.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ProcessTransactionLib
{
    public class ProcessTransaction : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {

            IPluginExecutionContext context = (IPluginExecutionContext)
           serviceProvider.GetService(typeof(IPluginExecutionContext));
            var tracingService = (ITracingService)
                 serviceProvider.GetService(typeof(ITracingService));
            IOrganizationServiceFactory serviceFactory =
                (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);


            var transactionId = context.InputParameters["transactionId"].ToString();

            PowerAppsDbManager powerAppsDbManager = new PowerAppsDbManager(service);

            var url = powerAppsDbManager.GetUrlByTerminalUser(context.UserId);

            var endpoint = url + "/Transaction/Process?transactionId=" + transactionId;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(endpoint);
            request.Method = "GET";
            request.ContentType = "application/json";
            request.KeepAlive = false;

            try
            {
                var response = (HttpWebResponse)request.GetResponse();

                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    var streamObject = streamReader.ReadToEnd();

                    var result = JsonHelper.DeserializeJson<TransactionHistory>(streamObject);


                    var query = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                <entity name='tm1_creditcard'>
                                <attribute name='tm1_customer' />
                                <order attribute='tm1_name' descending='false' />
                                <filter type='and'>
                                  <condition attribute='tm1_creditcardid' operator='eq' value='{result.Transaction.CreaditCardId}' />
                                </filter>
                              </entity>
                            </fetch>";

                    var fetchXML = new FetchExpression(query);
                    var responseFromDb = service.RetrieveMultiple(fetchXML);
                    var attributes = responseFromDb.Entities[0].Attributes;

                    var customerId = ((EntityReference)attributes["tm1_customer"]).Id;

                    var transactionHistory = new Entity("tm1_transactionhistory_team1");


                    transactionHistory["tm1_name"] = result.Name;
                    transactionHistory["tm1_transaction"] = new EntityReference("tm1_transaction_team1", Guid.Parse(result.Transaction.Id));
                    transactionHistory["tm1_transactiontype"] = new EntityReference("tm1_transactiontype", Guid.Parse(result.Transaction.TransactionType));
                    transactionHistory["tm1_result"] = result.Result;
                    transactionHistory["tm1_message"] = result.Message;
                    transactionHistory["tm1_resultjson"] = streamObject;
                    transactionHistory["tm1_terminalid"] = new EntityReference("tm1_terminal_team1", Guid.Parse(result.Transaction.TerminalId));
                    transactionHistory["tm1_amount"] = new Money((decimal)result.Transaction.Amount);
                    transactionHistory["transactioncurrencyid"] = new EntityReference("transactioncurrency", Guid.Parse(result.Transaction.Currency));
                    transactionHistory["tm1_customer"] = new EntityReference("tm1_contact", customerId);
                    transactionHistory["tm1_creditcard"] = new EntityReference("tm1_creditcard", Guid.Parse(result.Transaction.CreaditCardId));

                    
                    service.Create(transactionHistory);

                }

            }

            catch (WebException ex)
            {
                throw new InvalidPluginExecutionException(ex.Message);
            }
        }
    }
}
