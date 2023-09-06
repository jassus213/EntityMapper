using EntityMapper.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace EntityMapper.ServiceCollection.Extension;

public static class MapperExtension
{
    public static EntityMapper UseMapper(this IServiceCollection serviceCollection)
    {
        var mapper = new EntityMapper();
        serviceCollection.AddSingleton(mapper);
        serviceCollection.AddSingleton<IMapper>(x => x.GetRequiredService<EntityMapper>());
        return mapper;
    }

    public static void AddMapperConfiguration<T, TDto>(this EntityMapper entityMapper, Func<T, TDto> configuration) =>
        entityMapper.AddConfiguration(configuration);

    public static void AddDisposableMapperConfiguration<T, TDto>(this EntityMapper entityMapper,
        Func<T, TDto> configurationFunc) => entityMapper.AddDisposableConfiguration(configurationFunc);
}