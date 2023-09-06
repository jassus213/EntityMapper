using EntityMapper.Interfaces;

namespace EntityMapper;

public class AutoMapper : IMapper
{
    private readonly Dictionary<ValueTuple<Type, Type>, Delegate> _configurations =
        new Dictionary<ValueTuple<Type, Type>, Delegate>();

    public TDto Map<T, TDto>(T entity) where T : new() 
        where TDto : new()
    {
        var kvp = new ValueTuple<Type, Type>(typeof(T), typeof(TDto));
        if (_configurations.TryGetValue(kvp, out var deleg))
        {
            var func = (Func<T, TDto>)deleg;
            return func(entity);
        }

        throw new Exception($"Missing Configuration For {typeof(T)} and {typeof(TDto)}");
    }

    public void AddConfiguration<TDto, T>(Func<T, TDto> configuration) =>
        _configurations.Add(new ValueTuple<Type, Type>(typeof(T), typeof(TDto)), configuration);
}