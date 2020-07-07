using System.Threading.Tasks;
using ChatBot.Admin.ReadStorage.Specifications.Abstractions;

namespace ChatBot.Admin.ReadStorage.Queries.Abstractions
{
    public interface IQuery<TResult>
        where TResult : class
    {
        TResult Ask();
    }

    public interface IQuery<in TSpecification, TResult>
        where TResult : class
        where TSpecification: ISpecification
    {
        TResult Ask(TSpecification specification);
    }
}
