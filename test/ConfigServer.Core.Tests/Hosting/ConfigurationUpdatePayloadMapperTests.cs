﻿using ConfigServer.Sample.Models;
using ConfigServer.Server;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using ConfigServer.Core.Tests.TestModels;
using Moq;

namespace ConfigServer.Core.Tests.Hosting
{
    public class ConfigurationUpdatePayloadMapperTests
    {
        private IConfigurationUpdatePayloadMapper target;
        private Mock<IConfigurationSetService> configurationSet;
        private ConfigurationSetModel definition;
        private SampleConfig sample;
        private JObject updatedObject;
        private SampleConfig updatedSample;

        private const string clientId = "7aa7d5f0-90fb-420b-a906-d482428a0c44";


        public ConfigurationUpdatePayloadMapperTests()
        {
            configurationSet = new Mock<IConfigurationSetService>();
            configurationSet.Setup(r => r.GetConfigurationSet(typeof(SampleConfigSet), It.IsAny<ConfigurationIdentity>()))
                .ReturnsAsync(() => new SampleConfigSet { Options = new OptionSet<Option>(OptionProvider.Options, o => o.Id.ToString(), o => o.Description) });

            target = new ConfigurationUpdatePayloadMapper(new TestOptionSetFactory(), new PropertyTypeProvider(), configurationSet.Object);
            definition = new SampleConfigSet().BuildConfigurationSetModel();
            sample = new SampleConfig
            {
                Choice = Choice.OptionThree,
                IsLlamaFarmer = true,
                Decimal = 0.23m,
                StartDate = new DateTime(2013, 10, 10),
                LlamaCapacity = 47,
                Name = "Name 1",
                Option = OptionProvider.OptionOne,
                MoarOptions = new List<Option> { OptionProvider.OptionOne, OptionProvider.OptionThree },
                ListOfConfigs = new List<ListConfig> { new ListConfig { Name = "One", Value = 23 } }
            };

            updatedSample = new SampleConfig
            {
                Choice = Choice.OptionTwo,
                IsLlamaFarmer = false,
                Decimal = 0.213m,
                StartDate = new DateTime(2013, 10, 11),
                LlamaCapacity = 147,
                Name = "Name 2",
                Option = OptionProvider.OptionTwo,
                MoarOptions = new List<Option> { OptionProvider.OptionTwo, OptionProvider.OptionThree },
                ListOfConfigs = new List<ListConfig> { new ListConfig { Name = "Two plus Two", Value = 5 } }
            };
            dynamic updatedValue = new ExpandoObject();
            updatedValue.Choice = updatedSample.Choice;
            updatedValue.IsLlamaFarmer = updatedSample.IsLlamaFarmer;
            updatedValue.Decimal = updatedSample.Decimal;
            updatedValue.StartDate = updatedSample.StartDate;
            updatedValue.LlamaCapacity = updatedSample.LlamaCapacity;
            updatedValue.Name = updatedSample.Name;
            updatedValue.Option = updatedSample.Option.Id;
            updatedValue.MoarOptions = updatedSample.MoarOptions.Select(s => s.Id).ToList();
            updatedValue.ListOfConfigs = updatedSample.ListOfConfigs;
            var serilaisationSetting = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            var json = JsonConvert.SerializeObject(updatedValue, serilaisationSetting);
            updatedObject = JObject.Parse(json);
        }

        [Fact]
        public async Task UpdatesRegularValues()
        {
            var response = await target.UpdateConfigurationInstance(new ConfigInstance<SampleConfig>(sample, clientId), updatedObject, definition);
            var result = (SampleConfig)response.GetConfiguration();
            Assert.Equal(updatedSample.Choice, result.Choice);
            Assert.Equal(updatedSample.IsLlamaFarmer, result.IsLlamaFarmer);
            Assert.Equal(updatedSample.Decimal, result.Decimal);
            Assert.Equal(updatedSample.LlamaCapacity, result.LlamaCapacity);
            Assert.Equal(updatedSample.Name, result.Name);
        }

        [Fact]
        public async Task UpdatesOptionValues()
        {
            var response = await target.UpdateConfigurationInstance(new ConfigInstance<SampleConfig>(sample, clientId), updatedObject, definition);
            var result = (SampleConfig)response.GetConfiguration();
            Assert.Equal(updatedSample.Option.Description, result.Option.Description);
        }

        [Fact]
        public async Task UpdatesOptionsValues()
        {
            var response = await target.UpdateConfigurationInstance(new ConfigInstance<SampleConfig>(sample, clientId), updatedObject, definition);
            var result = (SampleConfig)response.GetConfiguration();
            Assert.Equal(updatedSample.MoarOptions.Count, result.MoarOptions.Count);
            Assert.Equal(updatedSample.MoarOptions.Select(s => s.Description), result.MoarOptions.Select(s => s.Description));
        }
        [Fact]
        public async Task UpdatesListOfConfigsValues()
        {
            var response = await target.UpdateConfigurationInstance(new ConfigInstance<SampleConfig>(sample, clientId), updatedObject, definition);
            var result = (SampleConfig)response.GetConfiguration();
            Assert.Equal(1, result.ListOfConfigs.Count);
            Assert.Equal(updatedSample.ListOfConfigs[0].Name, result.ListOfConfigs[0].Name);
            Assert.Equal(updatedSample.ListOfConfigs[0].Value, result.ListOfConfigs[0].Value);

        }

        [Fact]
        public async Task ThrowsIfNonNullable()
        {
            updatedObject["llamaCapacity"].Replace(null);
            var response = await Assert.ThrowsAsync<ConfigModelParsingException>(() => target.UpdateConfigurationInstance(new ConfigInstance<SampleConfig>(sample, clientId), updatedObject, definition));
        }

    }

}
