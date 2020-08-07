using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using CMS.MacroEngine;

namespace UCommerce.Kentico.Macros.Fields
{
    /// <summary>
    /// Generic class, responsible for registering all the properties of T as custom macro fields in Kentico's macro engine.
    /// </summary>
    /// <typeparam name="T">The type to register properties for.</typeparam>
    public abstract class GenericFieldContainer<T> : MacroFieldContainer where T : class
    {
        /// <summary>
        /// Runs through all the public properties of T and registers them ny name.
        /// </summary>
        /// <remarks>
        /// This method is being called from Kentico, when the application starts up.
        /// This is the "entry point" for the integration with Kentico's macro engine, regarding custom macro fields.
        /// </remarks>
        protected override void RegisterFields()
        {
            base.RegisterFields();

            var type = typeof(T);

            // Using reflection to get a list of all the names of the public properties.
            foreach (var propertyName in type.GetProperties().Where(x => x.GetIndexParameters().Length == 0).Select(x => x.Name))
            {
                RegisterFieldByName(propertyName);
            }
        }

        /// <summary>
        /// Calls a Kentico method, and registers a field evaluator on the property name.
        /// </summary>
        /// <param name="name">The name of the property to register as a field.</param>
        protected virtual void RegisterFieldByName(string name)
        {
            RegisterField(new MacroField(name, context => FieldEvaluator(context, name)));
        }

        /// <summary>
        /// Field evaluator called by the Kentico macro engine to get the value of a macro.
        /// </summary>
        /// <param name="evaluationContext">The Kentico macro evaluation context.</param>
        /// <param name="name">The name of the property to return a value for.</param>
        /// <returns>The property value of the source object in the current evaluation context.</returns>
        protected virtual object FieldEvaluator(EvaluationContext evaluationContext, string name)
        {
            T source = GetSource(evaluationContext);
            if (source == null) return string.Empty;

            // Use reflection, to find the property with the given name, on the type of the source.
            var propertyInfo = source.GetType().GetProperty(name);
            if (propertyInfo == null) return string.Empty;

            // Use reflection to get the value of the Property on the Source object.
            return propertyInfo.GetValue(source);
        }

        /// <summary>
        /// Get the source object from the Kentico macro <see cref="EvaluationContext"/>.
        /// </summary>
        /// <param name="context">The Kentico macro evaluation context.</param>
        /// <returns>The source object if found, null otherwise.</returns>
        protected virtual T GetSource(EvaluationContext context)
        {
            var resolver = context.Resolver;

            // Source object
            T target = resolver.SourceObject as T;
            if (target != null) return target;

            // Anonymous targets.
            target = resolver.GetAnonymousSources().OfType<T>().FirstOrDefault();
            if (target != null) return target;

            // DynamicParameters
            Hashtable dynamicParameters = ExtractDynamicParameters(resolver);
            target = dynamicParameters.Values.OfType<T>().FirstOrDefault();

            return target;
        }

        protected virtual Hashtable ExtractDynamicParameters(MacroResolver resolver)
        {
            var type = resolver.GetType();
            var propertyInfo = type.GetProperty("DynamicParameters", BindingFlags.NonPublic | BindingFlags.Instance);
            if (propertyInfo == null) throw new Exception("Could not extract the DynamicParameters property from the resolver.");

            var value = propertyInfo.GetValue(resolver);

            return value as Hashtable;
        }
    }
}
