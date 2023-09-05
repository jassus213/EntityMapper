namespace EntityMapper.Interfaces;

public interface IMapper
{
    TDto Map<T, TDto>(T dto) where T : new() where TDto : new();
}