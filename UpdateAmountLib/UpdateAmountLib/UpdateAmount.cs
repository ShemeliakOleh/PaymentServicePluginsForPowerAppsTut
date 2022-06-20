using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace UpdateAmountLib
{
    public class UpdateAmount : IPlugin
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
            var amount = context.InputParameters["amount"].ToString();



            var endpoint = "https://paymentservice20220518104558.azurewebsites.net/api/Transaction/UpdateAmount?transactionId=" + transactionId + "&amount="+amount;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(endpoint);
            request.Method = "GET";
            request.ContentType = "application/json";
            request.KeepAlive = false;


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
