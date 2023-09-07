namespace EntityMapper.Interfaces;

public interface IMapper
{
    TDestination Map<TSource, TDestination>(TSource entity) where TSource : new() where TDestination : new();
    Task<TDestination> MapAsync<TSource, TDestination>(TSource source) where TSource : new();
}