using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RazorLight.Razor;

namespace Paradigm.CodeGen.Output.Razor
{
    public sealed class TemplateCacheRazorLightProject : RazorLightProject
    {
        public override Task<RazorLightProjectItem> GetItemAsync(string templateKey)
        {
            return Task.FromResult(new TemplateCacheRazorLightProjectItem(TemplateCache.Instance.Get(templateKey).Body, templateKey) as RazorLightProjectItem);
        }

        public override Task<IEnumerable<RazorLightProjectItem>> GetImportsAsync(string templateKey)
        {
            return Task.FromResult(Enumerable.Empty<RazorLightProjectItem>());
        }
    }
}