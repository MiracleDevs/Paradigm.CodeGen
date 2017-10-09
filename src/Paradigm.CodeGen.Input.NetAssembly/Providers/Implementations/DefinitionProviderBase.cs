using System;
using Microsoft.Extensions.DependencyInjection;
using Paradigm.CodeGen.Input.NetAssembly.Collections.Interfaces;
using Paradigm.CodeGen.Input.NetAssembly.Providers.Interfaces;

namespace Paradigm.CodeGen.Input.NetAssembly.Providers.Implementations
{
    public abstract class DefinitionProviderBase<TInput, TOutput, TKey>: IDefinitionProvider<TInput, TOutput, TKey>
    {
        private IServiceProvider ServiceProvider { get; }

        protected DefinitionProviderBase(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
        }

        public abstract TInput GetFromSource(TOutput parameter);

        public abstract TInput Process(TInput definition, IObjectDefinitions<TKey> objectDefinitions, TOutput parameter);

        protected T Resolve<T>()
        {
            return this.ServiceProvider.GetService<T>();
        }
    }
}