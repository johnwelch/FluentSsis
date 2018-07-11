namespace FluentSsis.Emitter
{
    using System;
    using Microsoft.SqlServer.Dts.Runtime;

    /// <summary>
    /// Extends <see cref="IDTSName"/>.
    /// </summary>
    public static class DtsNameExtensions
    {
        /// <summary>
        /// Set the name on objects that implement IDTSName.
        /// </summary>
        /// <typeparam name="T">The type of object that implements IDTSName.</typeparam>
        /// <param name="namedObject">The object to be named.</param>
        /// <param name="name">The name to assign to the object.</param>
        /// <returns>The object that was named.</returns>
        public static T Named<T>(this T namedObject, string name)
            where T : IDTSName
        {
            if (namedObject == null)
            {
                throw new ArgumentNullException(nameof(namedObject));
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            namedObject.Name = name;
            return namedObject;
        }

        /// <summary>
        /// Set the description on classes that implement IDTSName.
        /// </summary>
        /// <typeparam name="T">The type of object that implements IDTSName.</typeparam>
        /// <param name="namedObject">The object to set the description on.</param>
        /// <param name="description">The description to assign to the object.</param>
        /// <returns>The object that was described.</returns>
        public static T WithDescription<T>(this T namedObject, string description)
            where T : IDTSName
        {
            if (namedObject == null)
            {
                throw new ArgumentNullException(nameof(namedObject));
            }

            if (string.IsNullOrWhiteSpace(description))
            {
                throw new ArgumentNullException(nameof(description));
            }

            namedObject.Description = description;
            return namedObject;
        }

        // TODO: Add naming / description support for precedence constraints
        // These methods should work, but I think we need the chained actions.
        public static Action<T> Named<T>(this Action<T> namedObject, string name)
            where T : IDTSName
        {
            return updatedObject =>
            {
                updatedObject.Named(name);
            };
        }

        public static Action<T> WithDescription<T>(this Action<T> namedObject, string description)
            where T : IDTSName
        {
            return updatedObject =>
            {
                updatedObject.WithDescription(description);
            };
        }
    }
}