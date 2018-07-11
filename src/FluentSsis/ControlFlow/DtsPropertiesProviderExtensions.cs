namespace FluentSsis.Emitter
{
    using System;
    using System.Linq;
    using Microsoft.SqlServer.Dts.Runtime;

    /// <summary>
    /// Extends <see cref="IDTSPropertiesProvider"/>.
    /// </summary>
    public static class DtsPropertiesProviderExtensions
    {
        /// <summary>
        /// Gets the property associated with the specified name.
        /// </summary>
        /// <param name="properties">The <see cref="DtsProperties"/> collection to check.</param>
        /// <param name="name">The name of the <see cref="DtsProperty"/> to find.</param>
        /// <param name="property">When this method returns, contains the <see cref="DtsProperty"/> associated with the specified name.
        /// The value will be null if the <see cref="DtsProperty"/> was not found. This parameter is passed uninitialized.</param>
        /// <returns><c>true</c> if the element was found, otherwise <c>false</c>.</returns>
        public static bool TryGetValue(this DtsProperties properties, string name, out DtsProperty property)
        {
            property = properties.OfType<DtsProperty>().FirstOrDefault(item => item.Name == name);
            return property != null;
        }

        /// <summary>
        /// Sets a property on objects that implement <see cref="IDTSPropertiesProvider"/>.
        /// </summary>
        /// <typeparam name="T">The type of object that implements <see cref="IDTSPropertiesProvider"/>.</typeparam>
        /// <param name="propertyProvider">The object to that the property should be set on.</param>
        /// <param name="name">The name to of the property to set.</param>
        /// <param name="value">The value to assign to the property.</param>
        /// <returns>The object that had it's property updated.</returns>
        public static T WithProperty<T>(this T propertyProvider, string name, object value)
            where T : IDTSPropertiesProvider
        {
            if (propertyProvider == null)
            {
                throw new ArgumentNullException(nameof(propertyProvider));
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (!propertyProvider.Properties.TryGetValue(name, out DtsProperty dtsProperty))
            {
                // TODO: Would be nice to know the name of the object for this.
                throw new InvalidOperationException($"Property {name} could not be found.");
            }

            // TODO: Add support for "magic" conversion of variable and connection references
            dtsProperty.SetValue(propertyProvider, value);
            return propertyProvider;
        }

        /// <summary>
        /// Sets a property on objects that implement <see cref="IDTSPropertiesProvider"/>.
        /// </summary>
        /// <typeparam name="TContainer">The type of container for the  object</typeparam>
        /// <typeparam name="TItem">The type of object that implements <see cref="IDTSPropertiesProvider"/>.</typeparam>
        /// <param name="function">The factory method.</param>
        /// <param name="name">The name to of the property to set.</param>
        /// <param name="value">The value to assign to the property.</param>
        /// <returns>The factory method for creating the object.</returns>
        public static Func<TContainer, TItem> WithProperty<TContainer, TItem>(this Func<TContainer, TItem> function, string name, object value)
                where TItem : IDTSPropertiesProvider
        {
            return factoryMethod =>
            {
                var propertyProvider = function(factoryMethod);
                propertyProvider.WithProperty(name, value);
                return propertyProvider;
            };
        }
    }
}
