using FakeXrmEasy;
using Microsoft.Xrm.Sdk;
using Scantegra.Core.Entities;
using Scantegra.Xrm.Plugins.ScantegraSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Scantegra.Xrm.Plugins.Test.ScantegraSettings
{
    public class ScantegraSettingsTests
    {
        [Fact]
        public void AllowOnlyOneEntity()
        {
            // Arrange
            var context = new XrmFakedContext()
            {
                ProxyTypesAssembly = Assembly.GetAssembly(typeof(scan_scantegrasettings))
            };
            var executionContext = context.GetDefaultPluginContext();

            // Create a stub entity
            var existing = new scan_scantegrasettings
            {
                Id = Guid.NewGuid()
            };

            context.Initialize(new List<scan_scantegrasettings>()
            {
                existing
            });

            var target = new scan_scantegrasettings();

            // Act
            Exception ex = Assert.Throws<InvalidPluginExecutionException>(() => context.ExecutePluginWithTarget<ScantegraSettingsPreCreate>(target));

            // Assert
            Assert.Equal("Only one finance settings entity allowed", ex.Message);
        }


    }
}
