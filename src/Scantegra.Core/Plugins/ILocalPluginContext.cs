using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Scantegra.Core.Plugins
{
    public interface ILocalPluginContext
    {
        IOrganizationService OrganizationService { get; }
        IPluginExecutionContext PluginExecutionContext { get; }
        ITracingService TracingService { get; }
        void Trace(string message, params object[] o);
        void Trace(FaultException<OrganizationServiceFault> exception);
    }
}
