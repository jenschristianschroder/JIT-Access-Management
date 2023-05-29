using Microsoft.Xrm.Sdk;
using System;

namespace AccessManagementPlugins
{
    /// <summary>
    /// Plugin development guide: https://docs.microsoft.com/powerapps/developer/common-data-service/plug-ins
    /// Best practices and guidance: https://docs.microsoft.com/powerapps/developer/common-data-service/best-practices/business-logic/
    /// </summary>
    public class PreOperationcat_accessrequestCreate : PluginBase
    {
        public PreOperationcat_accessrequestCreate(string unsecureConfiguration, string secureConfiguration)
            : base(typeof(PreOperationcat_accessrequestCreate))
        {
            // TODO: Implement your custom configuration handling
            // https://docs.microsoft.com/powerapps/developer/common-data-service/register-plug-in#set-configuration-data
        }

        // Entry point for custom business logic execution
        protected override void ExecuteDataversePlugin(ILocalPluginContext localPluginContext)
        {
            if (localPluginContext == null)
            {
                throw new ArgumentNullException(nameof(localPluginContext));
            }

            var context = localPluginContext.PluginExecutionContext;

            // Obtain the tracing service
            ITracingService tracingService = localPluginContext.TracingService;

            IOrganizationService currentUserService = localPluginContext.PluginUserService;

            try
            {
                Entity accessRequest = (Entity)context.InputParameters["Target"];

                // Set Status Reason to "Request" (1)
                accessRequest.Attributes["statuscode"] = 1;
            }
            // Only throw an InvalidPluginExecutionException. Please Refer https://go.microsoft.com/fwlink/?linkid=2153829.
            catch (Exception ex)
            {
                tracingService?.Trace("An error occurred executing Plugin AccessManagementPlugins.CreateAccessRequest.PreOperationcat_accessrequestCreate : {0}", ex.ToString());
                throw new InvalidPluginExecutionException("An error occurred executing Plugin AccessManagementPlugins.CreateAccessRequest.PreOperationcat_accessrequestCreate .", ex);
            }
        }
    }
}
