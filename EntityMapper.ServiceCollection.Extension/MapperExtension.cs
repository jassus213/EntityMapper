using System.Linq.Expressions;
using EntityMapper.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace EntityMapper.ServiceCollection.Extension;

public static class MapperExtension
{
    public static IMapperConfigurator UseMapper(this IServiceCollection serviceCollection)
    {
        var mapper = new EntityMapper();
        serviceCollection.AddSingleton(mapper);
        serviceCollection.AddSingleton<IMapper>(x => x.GetRequiredService<EntityMapper>());
        return mapper;
    }

    public static void AddMapperConfiguration<T, TDto>(this IMapperConfigurator entityMapper, Func<T, TDto> configuration) =>
        entityMapper.AddConfiguration(configuration);

    public static void AddDisposableMapperConfiguration<T, TDto>(this IMapperConfigurator entityMapper,
        Func<T, TDto> configurationFunc) => entityMapper.AddDisposableConfiguration(configurationFunc);

    public static void AddAsyncMapperConfiguration<TSource, TDestination>(this IMapperConfigurator entityMapper,
        Func<TSource, Task<TDestination>> configuration) =>
        entityMapper.AddAsyncConfiguration(configuration);
    
    public static void AddABidirectionalMapperConfiguration<TSource, TDestination>(this IMapperConfigurator entityMapper,
        Expression<Func<TSource, TDestination>> configuration) =>
        entityMapper.AddBidirectionalMapping(configuration);
}