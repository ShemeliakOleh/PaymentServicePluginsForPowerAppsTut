﻿using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddTransactionPlugin
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



            var contactId = context.InputParameters["ContactId"].ToString();
        }
    }
}




namespace ValidateTerminalUsersRelationships
{
    public class Validate : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext context = (IPluginExecutionContext)
           serviceProvider.GetService(typeof(IPluginExecutionContext));
            Entity entity = (Entity)context.InputParameters["Target"];
            IOrganizationServiceFactory serviceFactory =
                (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

            try
            {
                var userId = entity.GetAttributeValue<EntityReference>("tm1_user").Id;
                var query = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                 <entity name='tm1_terminaluser_team1'>
                                 <attribute name='tm1_terminaluser_team1id' />
                                <filter type='and'>
                                 <condition attribute='tm1_user' operator='eq' value='{userId}' />
                                 </filter>
                                 </entity>
                                 </fetch>";

                var fetchXml = new FetchExpression(query);
                var response = service.RetrieveMultiple(fetchXml);
                if (response != null && response.Entities.Any())
                {
                    throw new InvalidPluginExecutionException("An error occurred in FollowUpPlugin");
                }

            }
            catch (FaultException<OrganizationServiceFault> ex)
            {
                throw new InvalidPluginExecutionException("An error occurred in FollowUpPlugin.", ex);
            }
        }
    }
}