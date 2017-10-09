using System;
using Microsoft.Extensions.DependencyInjection;
using Paradigm.CodeGen.Input.Json.Providers.Interfaces;

namespace Paradigm.CodeGen.Input.Json.Providers.Implementations
{
    public abstract class DefinitionProviderBase<TInput, TOutput> : IDefinitionProvider<TInput, TOutput>
    {
        private IServiceProvider ServiceProvider { get; }

        protected DefinitionProviderBase(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
        }

        public abstract TOutput Create(TInput input);

        public virtual TOutput Process(TOutput output, TInput input, IInputService service)
        {
            return output;
        }

        protected T Resolve<T>()
        {
            return this.ServiceProvider.GetService<T>();
        }
    }
}