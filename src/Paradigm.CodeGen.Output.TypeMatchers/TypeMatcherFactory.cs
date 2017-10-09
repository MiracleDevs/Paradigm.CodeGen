using System;
using System.Reflection;

namespace  Paradigm.CodeGen.Output.TypeMatchers
{
    public static class TypeMatcherFactory
    {
        public static ITypeMatcher Create(string name)
        {
            var factoryType = typeof(TypeMatcherFactory);
            var typeName = $"{factoryType.Namespace}.{name}TypeMatcher";
            var type = factoryType.GetTypeInfo().Assembly.GetType(typeName);

            if (type == null)
                throw new Exception($"The type matcher {name} does not exist.");

            var instance = Activator.CreateInstance(type) as ITypeMatcher;

            if (instance == null)
                throw new Exception($"The type matcher {name} couldn't be instantiated.");

            return instance;
        }
    }
}