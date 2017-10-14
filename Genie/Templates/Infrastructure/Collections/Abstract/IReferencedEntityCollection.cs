using Genie.Core.Base.Generating.Concrete;

namespace Genie.Core.Templates.Infrastructure.Collections.Abstract
{
    internal class IReferencedEntityCollectionTemplate : GenieTemplate
    {
        public IReferencedEntityCollectionTemplate(string path) : base(path)
        {
        }

        public override string Generate()
        {
            L($@"

using System.Collections.Generic;
using {GenerationContext.BaseNamespace}.Infrastructure.Models.Concrete;

namespace {GenerationContext.BaseNamespace}.Infrastructure.Collections.Abstract
{{
    public interface IReferencedEntityCollection<T> : IEnumerable<T> where T: BaseModel
	{{
		void Add(T entityToAdd);
	}}
}}

");

            return E();
        }
    }
}