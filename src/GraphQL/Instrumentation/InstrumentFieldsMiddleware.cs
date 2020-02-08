using System.Collections.Generic;
using System.Threading.Tasks;
using GraphQL.Types;
using GraphQL.Utilities;

namespace GraphQL.Instrumentation
{
    public class InstrumentFieldsMiddleware
    {
        public async Task SetResultAsync(IResolveFieldContext context, FieldMiddlewareDelegate next)
        {
            var metadata = new Dictionary<string, object>
            {
                { "typeName", context.ParentType.Name },
                { "fieldName", context.FieldName },
                { "returnTypeName", SchemaPrinter.ResolveName(context.ReturnType) },
                { "path", context.Path },
            };

            using (context.Metrics.Subject("field", context.FieldName, metadata))
            {
                await next(context).ConfigureAwait(false);
            }
        }
    }
}
