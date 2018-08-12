using Microsoft.Xrm.Sdk;
using Scantegra.Core.Entities;
using Scantegra.Core.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scantegra.Xrm.Plugins.ScantegraSettings
{
    public class ScantegraSettingsPreCreate : BasePlugin
    {
        public override void Execute(ILocalPluginContext localContext)
        {
            var serviceContext = new ScantegraServiceContext(localContext.OrganizationService);
            var settings = serviceContext.scan_scantegrasettingsSet.ToList();
            if (settings.Count() > 0)
            {
                throw new InvalidPluginExecutionException("Only one finance settings entity allowed");

            }
        }

    }
}
