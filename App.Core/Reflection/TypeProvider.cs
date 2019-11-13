using System;
using System.Linq;
using System.Reflection;

namespace App.Core.Reflection
{
    public static class TypeProvider
    {
        public static Type GetTypeFromAnyReferencingAssembly(string typeName)
        {
            var referencedAssemblies = Assembly.GetEntryAssembly()
                .GetReferencedAssemblies()
                .Select(a => a.FullName).ToList();

            return AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => referencedAssemblies.Contains(a.FullName))
                .SelectMany(a => a.GetTypes().Where(x => x.Name == typeName))
                .FirstOrDefault();
        }

        public static Type GetTypeFromTypeName(string typeName)
        {
            var referencedAssemblies = Assembly.GetEntryAssembly()
                .GetReferencedAssemblies()
                .Select(a => a.FullName).ToList();

            return AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => referencedAssemblies.Contains(a.FullName))
                .SelectMany(a => a.GetTypes().Where(x => x.ToString() == typeName))
                .FirstOrDefault();
        }
    }
}
