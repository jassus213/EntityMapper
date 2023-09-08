using System.Linq.Expressions;

namespace EntityMapper.Interfaces;

public interface IMapperConfigurator
{
    public void AddBidirectionalMapping<TSource, TDestination>(Expression<Func<TSource, TDestination>> expression);
    public void AddAsyncConfiguration<TSource, TDestination>(Func<TSource, Task<TDestination>> configuration);
    public void AddConfiguration<TSource, TDestination>(Func<TSource, TDestination> configuration);
    public void AddDisposableConfiguration<TSource, TDestination>(Func<TSource, TDestination> disposableConfiguration);
}