#region Usings



#endregion

using Genie.Core.Base.Generating;

namespace Genie.Core.Templates.Dapper
{
    internal class DynamicParameters_ParamInfoTemplate : GenieTemplate
    {
        public DynamicParameters_ParamInfoTemplate(string path) : base(path)
        {
        }

        public override string Generate()
        {
            L($@"

using System;
using System.Data;

namespace {GenerationContext.BaseNamespace}.Dapper 
{{
    partial class DynamicParameters
    {{
        sealed class ParamInfo
        {{
            public string Name {{ get; set; }}
            public object Value {{ get; set; }}
            public ParameterDirection ParameterDirection {{ get; set; }}
            public DbType? DbType {{ get; set; }}
            public int? Size {{ get; set; }}
            public IDbDataParameter AttachedParam {{ get; set; }}
            internal Action<object, DynamicParameters> OutputCallback {{ get; set; }}
            internal object OutputTarget {{ get; set; }}
            internal bool CameFromTemplate {{ get; set; }}

            public byte? Precision {{ get; set; }}
            public byte? Scale {{ get; set; }}
        }}
    }}
}}");

            return E();
        }
    }
}