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

                EntityReference accessProfileRef = (EntityReference)accessRequest.GetAttributeValue<EntityReference>("cat_accessprofile");

                tracingService.Trace($"Get Access Profile ({accessProfileRef.Id})");
                Entity accessProfile = currentUserService.Retrieve("cat_accessprofile", accessProfileRef.Id, new Microsoft.Xrm.Sdk.Query.ColumnSet(true));

                tracingService.Trace($"access profile state {accessProfile.GetAttributeValue<OptionSetValue>("statecode").Value} status {accessProfile.GetAttributeValue<OptionSetValue>("statuscode").Value}");

                if(accessProfile.GetAttributeValue<OptionSetValue>("statecode").Value != 0 || accessProfile.GetAttributeValue<OptionSetValue>("statuscode").Value != 1)
                {
                    throw new Exception($"Access Request created for Access Profile ({accessProfile.Id}) that is not Active");
                }

                tracingService.Trace("Ensure Access Request match Access Profile");
                accessRequest.Attributes["cat_environmentid"] = accessProfile.GetAttributeValue<String>("cat_environmentid");
                accessRequest.Attributes["cat_environmentname"] = accessProfile.GetAttributeValue<String>("cat_environmentname");
                accessRequest.Attributes["cat_environmentuniquename"] = accessProfile.GetAttributeValue<String>("cat_environmentuniquename");
                accessRequest.Attributes["cat_environmenturl"] = accessProfile.GetAttributeValue<String>("cat_environmenturl");
                accessRequest.Attributes["cat_duration"] = accessProfile.GetAttributeValue<Int32>("cat_duration");
                accessRequest.Attributes["cat_approvalrequired"] = accessProfile.GetAttributeValue<bool>("cat_approvalrequired");
                accessRequest.Attributes["cat_securityrole"] = accessProfile.GetAttributeValue<String>("cat_securityrole");

                if(accessProfile.GetAttributeValue<bool>("cat_justificationrequired"))
                {
                    if(String.IsNullOrEmpty(accessRequest.GetAttributeValue<String>("cat_justification")))
                    {
                        throw new Exception($"Access Request has no Justification. Selected Access Profile ({accessProfile.Id}) requires Justification");
                    }
                }

                // Set Status Reason to "Request" (1)
                if (accessRequest.GetAttributeValue<OptionSetValue>("statuscode") != new OptionSetValue(1))
                {
                    tracingService.Trace($"Access Request created with status code {accessRequest.GetAttributeValue<OptionSetValue>("statuscode").Value}");
                    tracingService.Trace($"Setting Access Request status code to 1 (Requested)");
                    accessRequest.Attributes["statuscode"] = 1;
                }
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
