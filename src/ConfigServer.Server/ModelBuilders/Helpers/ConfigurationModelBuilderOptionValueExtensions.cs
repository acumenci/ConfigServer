﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;

namespace ConfigServer.Server
{
    /// <summary>
    /// Property Option Value Builders For Configurations Set
    /// </summary>
    public static class ConfigurationModelBuilderOptionValueExtensions
    {
        /// <summary>
        /// Gets ConfigurationPropertyModelBuilder for property with option
        /// Overides existing configuration from property
        /// </summary>
        /// <typeparam name="TModel">Source model type</typeparam>
        /// <typeparam name="TOption">Option type</typeparam>
        /// <typeparam name="TConfigurationSet">ConfigurationSet to provide available options</typeparam>
        /// <typeparam name="TValue">Property Type</typeparam>
        /// <param name="source">model with property</param>
        /// <param name="expression">property selector</param>
        /// <param name="optionProvider">Options Selector</param>
        /// <param name="keySelector">Value Selector</param>
        /// <returns>ConfigurationPropertyWithOptionBuilder for selected property</returns>
        public static ConfigurationPropertyWithOptionValueBuilder PropertyWithOptionValue<TModel, TValue, TOption, TConfigurationSet>(this IModelWithProperties<TModel> source, Expression<Func<TModel, TValue>> expression, Expression<Func<TConfigurationSet, OptionSet<TOption>>> optionProvider, Expression<Func<TOption, TValue>> keySelector) where TConfigurationSet : ConfigurationSet
        {
            var propertyName = ExpressionHelper.GetPropertyNameFromExpression(expression);
            var optionName = ExpressionHelper.GetPropertyNameFromExpression(optionProvider);
            var model = new ConfigurationPropertyWithOptionValueModelDefinition<TConfigurationSet, TOption, TValue>(optionProvider.Compile(), keySelector.Compile(), optionName, propertyName, typeof(TModel));
            source.ConfigurationProperties[propertyName] = model;
            return new ConfigurationPropertyWithOptionValueBuilder(model);
        }


        /// <summary>
        /// Gets ConfigurationPropertyModelBuilder for property with multiple option
        /// Overides existing configuration from property
        /// </summary>
        /// <typeparam name = "TModel" > Source model type</typeparam>
        /// <typeparam name = "TOption" > Option type</typeparam>
        /// <typeparam name = "TValue" > Option value type</typeparam>
        /// <typeparam name = "TValueCollection" > Value Collection type</typeparam>
        /// <typeparam name = "TConfigurationSet" > ConfigurationSet to provide available options</typeparam>
        /// <param name = "source" > model with property</param>
        /// <param name = "expression" > property selector</param>
        /// <param name = "optionProvider" > Options Selector</param>
        /// <param name = "keySelector" > Option value selector</param>
        /// <returns>ConfigurationPropertyWithOptionBuilder for selected property</returns>
        public static ConfigurationPropertyWithOptionValueBuilder PropertyWithMultipleOptionValues<TModel, TOption, TValue, TValueCollection, TConfigurationSet>(this IModelWithProperties<TModel> source, Expression<Func<TModel, TValueCollection>> expression, Expression<Func<TConfigurationSet, OptionSet<TOption>>> optionProvider, Expression<Func<TOption, TValue>> keySelector) where TConfigurationSet : ConfigurationSet where TOption : new() where TValueCollection : ICollection<TValue>
        {
            var propertyName = ExpressionHelper.GetPropertyNameFromExpression(expression);
            var optionName = ExpressionHelper.GetPropertyNameFromExpression(optionProvider);
            var model = new ConfigurationPropertyWithMultipleOptionValuesModelDefinition<TConfigurationSet, TOption, TValue, TValueCollection>(optionProvider.Compile(), keySelector.Compile(), optionName, propertyName, typeof(TModel));
            source.ConfigurationProperties[propertyName] = model;
            return new ConfigurationPropertyWithOptionValueBuilder(model);
        }
    }
}
