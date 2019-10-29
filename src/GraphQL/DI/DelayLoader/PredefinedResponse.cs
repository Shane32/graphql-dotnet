using System.Threading.Tasks;

namespace GraphQL.DI.DelayLoader
{
    public sealed class PredefinedResponse<TOut> : IDelayLoadedResult<TOut>
    {
        public PredefinedResponse(TOut result)
        {
            _result = result;
        }

        private TOut _result;

        public Task<TOut> GetResultAsync() => Task.FromResult(_result);

        Task<object> IDelayLoadedResult.GetResultAsync() => Task.FromResult((object)_result);
    }

}