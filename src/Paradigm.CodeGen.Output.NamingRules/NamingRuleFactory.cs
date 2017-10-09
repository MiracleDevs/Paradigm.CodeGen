using System;
using System.Reflection;

namespace Paradigm.CodeGen.Output.NamingRules
{
    public static class NamingRuleFactory
    {
        public static INamingRule Create(string name)
        {
            var factoryType = typeof(NamingRuleFactory);
            var typeName = $"{factoryType.Namespace}.{name}NamingRule";
            var type = factoryType.GetTypeInfo().Assembly.GetType(typeName);

            if (type == null)
                throw new Exception($"The naming rule {name} does not exist.");

            var instance = Activator.CreateInstance(type) as INamingRule;

            if (instance == null)
                throw new Exception($"The naming rule {name} couldn't be instantiated.");

            return instance;
        }
    }
}