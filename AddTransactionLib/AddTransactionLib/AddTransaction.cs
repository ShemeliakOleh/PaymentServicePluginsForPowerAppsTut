using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AddTransactionLib
{
    public class AddTransaction : IPlugin
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

            var transaction = new Transaction();

            transaction.Id = context.InputParameters["Id"].ToString();
            transaction.CreaditCardId = context.InputParameters["CreaditCardId"].ToString();
            transaction.Amount = int.Parse(context.InputParameters["Amount"].ToString());
            transaction.TerminalId = context.InputParameters["TeraminalId"].ToString();
            transaction.Currency = context.InputParameters["Currency"].ToString();
            transaction.TransactionType = context.InputParameters["TransactionType"].ToString();
            transaction.Completed = bool.Parse(context.InputParameters["Completed"].ToString());

            var content = JsonHelper.SerializeJson(transaction);

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
                throw new InvalidPluginExecutionException(ex.Message);
            }



        }
    }
}
