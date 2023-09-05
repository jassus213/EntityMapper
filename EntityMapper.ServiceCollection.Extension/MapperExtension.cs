using EntityMapper.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace EntityMapper.ServiceCollection.Extension;

public static class MapperExtension
{
    public static AutoMapper UseMapper(this IServiceCollection serviceCollection)
    {
        var mapper = new AutoMapper();
        serviceCollection.AddSingleton(mapper);
        serviceCollection.AddSingleton<IMapper>(x => x.GetRequiredService<AutoMapper>());
        return mapper;
    }

    public static void AddMapperConfiguration<T, TDto>(this AutoMapper mapper, Func<T, TDto> conf, bool isMapBack = false) =>
        mapper.AddConfiguration(conf);
}