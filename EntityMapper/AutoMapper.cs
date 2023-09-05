using EntityMapper.Interfaces;

namespace EntityMapper;

public class AutoMapper : IMapper
{
    private readonly Dictionary<KeyValuePair<Type, Type>, Delegate> _configurations =
        new Dictionary<KeyValuePair<Type, Type>, Delegate>();

    public TDto Map<T, TDto>(T dto) where T : new() 
        where TDto : new()
    {
        var kvp = new KeyValuePair<Type, Type>(typeof(T), typeof(TDto));
        if (_configurations.TryGetValue(kvp, out var deleg))
        {
            var func = (Func<T, TDto>)deleg;
            return func(dto);
        }

        throw new Exception("Missing Configuration");
    }

    public void AddConfiguration<TDto, T>(Func<T, TDto> configuration) =>
        _configurations.Add(new KeyValuePair<Type, Type>(typeof(T), typeof(TDto)), configuration);
}