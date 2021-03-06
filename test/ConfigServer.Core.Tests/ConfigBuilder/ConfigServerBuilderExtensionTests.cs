﻿using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using ConfigServer.InMemoryProvider;
using Xunit;
using System.Threading.Tasks;
using ConfigServer.Server;

namespace ConfigServer.Core.Tests
{
    public class ConfigServerBuilderExtensionTests
    {
        private readonly IServiceCollection serviceCollection;

        public ConfigServerBuilderExtensionTests()
        {
            serviceCollection = new ServiceCollection();
        }

        [Fact]
        public void CanAddConfigRegistry()
        {
            var config = new SimpleConfig { IntProperty = 23 };
            var applicationId = "3E37AC18-A00F-47A5-B84E-C79E0823F6D4";
            var builder = serviceCollection.AddConfigServer()
                .UseInMemoryProvider()
                .UseLocalConfigServerClient(applicationId)
                .WithConfig<SimpleConfig>();
            var regs = builder.ConfigurationRegistry.ToList();
            Assert.Equal(1, regs.Count);
            Assert.Equal(typeof(SimpleConfig).Name, regs[0].ConfigurationName);
        }

        [Fact]
        public void ConfigRegistry_IsAdded()
        {
            var config = new SimpleConfig { IntProperty = 23 };
            var applicationId = "3E37AC18-A00F-47A5-B84E-C79E0823F6D4";
            var builder = serviceCollection.AddConfigServer()
                .UseInMemoryProvider()
                .UseLocalConfigServerClient(applicationId)
                .WithConfig<SimpleConfig>();
            var serviceProvider = builder.ServiceCollection.BuildServiceProvider();
            var configRepo = serviceProvider.GetRequiredService<ConfigurationRegistry>();
            var regs = configRepo.ToList();
            Assert.Equal(1, regs.Count);
            Assert.Equal(typeof(SimpleConfig).Name, regs[0].ConfigurationName);
        }
    }
}
