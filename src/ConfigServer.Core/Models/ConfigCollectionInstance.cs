﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace ConfigServer.Core
{

    /// <summary>
    /// Represents a collection of configuration with meta data such as client id
    /// </summary>
    public abstract class ConfigCollectionInstance : ConfigInstance
    {
        /// <summary>
        /// Initializes ConfigCollectionInstance with empty configuration
        /// </summary>
        /// <param name="type">Config Type</param>
        protected ConfigCollectionInstance(Type type) : base(type, true)
        {
        }

        /// <summary>
        /// Initializes ConfigCollectionInstance with empty configuration
        /// </summary>
        /// <param name="type">Config Type</param>
        /// <param name="clientId">Client Id</param>
        protected ConfigCollectionInstance(Type type, string clientId) : base(type, true, clientId)
        {
        }

        /// <summary>
        /// Create CollectionBuilder For Instance
        /// </summary>
        /// <returns>CollectionBuilder For Instance</returns>
        public abstract CollectionBuilder CreateCollectionBuilder();
    }

    /// <summary>
    /// Represents a collection of configuration with meta data such as client id
    /// </summary>
    /// <typeparam name="TConfig">Configuration type</typeparam>
    public class ConfigCollectionInstance<TConfig> : ConfigCollectionInstance where TConfig : class, new()
    {

        /// <summary>
        /// Initializes ConfigCollectionInstance with empty configuration
        /// </summary>
        public ConfigCollectionInstance() : base(typeof(TConfig))
        {
            Configuration = new TConfig[0];
        }

        /// <summary>
        /// Initializes ConfigCollectionInstance with empty configuration
        /// </summary>
        /// <param name="clientId">Client Id</param>
        public ConfigCollectionInstance(string clientId) : base(typeof(TConfig), clientId)
        {
            Configuration = new TConfig[0];
        }

        /// <summary>
        /// Initializes ConfigCollectionInstance with supplied configuration
        /// </summary>
        /// <param name="config">configuration</param>
        /// <param name="clientId">Client Id</param>
        public ConfigCollectionInstance(IEnumerable<TConfig> config, string clientId) : base(typeof(TConfig), clientId)
        {
            Configuration = config.ToArray();
        }

        /// <summary>
        /// Configuration object for ConfigInstance
        /// </summary>
        public ICollection<TConfig> Configuration { get; set; }

        /// <summary>
        /// Constructs a new instance of the configuration
        /// </summary>
        /// <returns>New instance of the configuration </returns>
        public override object ConstructNewConfiguration() => new TConfig[0];

        /// <summary>
        /// Gets configuration as object
        /// </summary>
        /// <returns>configuration as object</returns>
        public override object GetConfiguration() => Configuration;

        /// <summary>
        /// Sets configuration
        /// </summary>
        /// <param name="value">value of configuration</param>
        /// <exception cref="InvalidCastException">When object is not of the same type as generic type parameter.</exception>
        public override void SetConfiguration(object value) => Configuration = ((IEnumerable<TConfig>)value).ToArray();

        /// <summary>
        /// Create CollectionBuilder For Instance
        /// </summary>
        /// <returns>CollectionBuilder For Instance</returns>
        public override CollectionBuilder CreateCollectionBuilder()
        {
            return new CollectionBuilder<TConfig>(typeof(List<TConfig>));
        }
    }
}
