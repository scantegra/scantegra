using Microsoft.Xrm.Sdk;
using Scantegra.Core.Entities;
using Scantegra.Core.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scantegra.Xrm.Plugins.CommunicationCircuit
{
    public class CommunicationCircuitPreCreate : BasePlugin
    {
        public override void Execute(ILocalPluginContext localContext)
        {
            var serviceContext = new ScantegraServiceContext(localContext.OrganizationService);
            throw new InvalidPluginExecutionException("Plugin Step Not Implemented Yet");


        }

    }
}
