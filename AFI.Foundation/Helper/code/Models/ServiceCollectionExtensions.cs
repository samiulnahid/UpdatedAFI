using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Web.Http;
using System.Web.Mvc;

namespace AFI.Foundation.Helper.Models
{
    public static class ServiceCollectionExtensions
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void AddMvcControllersInCurrentAssembly(this IServiceCollection serviceCollection) => serviceCollection.AddMvcControllers(Assembly.GetCallingAssembly());

        public static void AddMvcControllers(
          this IServiceCollection serviceCollection,
          params string[] assemblyFilters)
        {
            HashSet<string> assemblyNames = new HashSet<string>(((IEnumerable<string>)assemblyFilters).Where<string>((Func<string, bool>)(filter => !filter.Contains<char>('*'))));
            string[] wildcardNames = ((IEnumerable<string>)assemblyFilters).Where<string>((Func<string, bool>)(filter => filter.Contains<char>('*'))).ToArray<string>();
            Assembly[] array = ((IEnumerable<Assembly>)AppDomain.CurrentDomain.GetAssemblies()).Where<Assembly>((Func<Assembly, bool>)(assembly =>
            {
                string nameToMatch = assembly.GetName().Name;
                return assemblyNames.Contains(nameToMatch) || ((IEnumerable<string>)wildcardNames).Any<string>((Func<string, bool>)(wildcard => ServiceCollectionExtensions.IsWildcardMatch(nameToMatch, wildcard)));
            })).ToArray<Assembly>();
            serviceCollection.AddMvcControllers(array);
        }

        public static void AddMvcControllers(
          this IServiceCollection serviceCollection,
          params Assembly[] assemblies)
        {
            foreach (Type serviceType in ((IEnumerable<Type>)ServiceCollectionExtensions.GetTypesImplementing<IController>(assemblies)).Where<Type>((Func<Type, bool>)(controller => controller.Name.EndsWith("Controller", StringComparison.Ordinal))))
                serviceCollection.AddTransient(serviceType);
            foreach (Type serviceType in ((IEnumerable<Type>)ServiceCollectionExtensions.GetTypesImplementing<ApiController>(assemblies)).Where<Type>((Func<Type, bool>)(controller => controller.Name.EndsWith("Controller", StringComparison.Ordinal))))
                serviceCollection.AddTransient(serviceType);
        }

        public static Type[] GetTypesImplementing<T>(params Assembly[] assemblies)
        {
            if (assemblies == null || assemblies.Length == 0)
                return new Type[0];
            Type targetType = typeof(T);
            return ((IEnumerable<Assembly>)assemblies).Where<Assembly>((Func<Assembly, bool>)(assembly => !assembly.IsDynamic)).SelectMany<Assembly, Type>(new Func<Assembly, IEnumerable<Type>>(ServiceCollectionExtensions.GetExportedTypes)).Where<Type>((Func<Type, bool>)(type => !type.IsAbstract && !type.IsGenericTypeDefinition && targetType.IsAssignableFrom(type))).ToArray<Type>();
        }

        private static IEnumerable<Type> GetExportedTypes(Assembly assembly)
        {
            try
            {
                return (IEnumerable<Type>)assembly.GetExportedTypes();
            }
            catch (NotSupportedException ex)
            {
                return (IEnumerable<Type>)Type.EmptyTypes;
            }
            catch (ReflectionTypeLoadException ex)
            {
                return ((IEnumerable<Type>)ex.Types).Where<Type>((Func<Type, bool>)(type => type != (Type)null));
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(string.Format((IFormatProvider)CultureInfo.InvariantCulture, "Unable to load types from assembly {0}. {1}", (object)assembly.FullName, (object)ex.Message), ex);
            }
        }

        private static bool IsWildcardMatch(string input, string wildcards) => Regex.IsMatch(input, "^" + Regex.Escape(wildcards).Replace("\\*", ".*").Replace("\\?", ".") + "$", RegexOptions.IgnoreCase);
    }
}