using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ContactApiValidation
{
    public class CheckExternalID : IPlugin
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



            var contactId = context.InputParameters["ContactId"].ToString();

            var firstName = "";
            var lastName = "";
            var externalClientId = "";


            try
            {
                var query = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                <entity name='tm1_contact'>
                                <attribute name='tm1_contactid' />
                                <attribute name='tm1_name' />
                                <attribute name='tm1_lastname' />
                                <attribute name='tm1_email' />
                                <attribute name='tm1_externalcustomerid' />
                                <order attribute='tm1_name' descending='false' />
                                <filter type='and'>
                                  <condition attribute='tm1_contactid' operator='eq' value='{contactId}' />
                                </filter>
                              </entity>
                            </fetch>";
                var fetchXML = new FetchExpression(query);
                var response =  service.RetrieveMultiple(fetchXML);
                var attributes =  response.Entities[0].Attributes;

                firstName = attributes["tm1_name"].ToString();
                lastName = attributes["tm1_lastname"].ToString();
                if(attributes.ContainsKey("tm1_externalcustomerid")) externalClientId = attributes["tm1_externalcustomerid"].ToString();

            }
            catch (Exception ex)
            {

                throw new InvalidPluginExecutionException(ex.Message);
            }

            if(externalClientId == "")
            {
                var customer = new Customer
                {
                    Id = "",
                    Name = firstName,
                    LastName = lastName
                };

                var content = JsonHelper.SerializeJson(customer);

                var endpoint = "https://paymentservice20220518104558.azurewebsites.net/api/Customer/Create";

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

                    using(var streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        var streamObject = streamReader.ReadToEnd();
                        var result = JsonHelper.DeserializeJson<string>(streamObject);

                        var contact = new Entity("tm1_contact", Guid.Parse(contactId));

                        contact["tm1_externalcustomerid"] = result;

                        service.Update(contact);
                    }

                    


                }

                catch (WebException ex)
                {
                    throw new InvalidPluginExecutionException(ex.Message);
                }
            }

           
        }
    }
}
