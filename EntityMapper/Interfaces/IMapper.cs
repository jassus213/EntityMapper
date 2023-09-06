namespace EntityMapper.Interfaces;

public interface IMapper
{
    TDto Map<T, TDto>(T entity) where T : new() where TDto : new();
}