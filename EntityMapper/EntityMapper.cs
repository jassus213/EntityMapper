using EntityMapper.Interfaces;

namespace EntityMapper;

/// <summary>
/// Call the Map method if you need to map from one object to another
/// </summary>
public class EntityMapper : IMapper
{
    private readonly Dictionary<ValueTuple<Type, Type>, Configuration.Configuration> _configurations =
        new Dictionary<(Type, Type), Configuration.Configuration>();

    public TDto Map<T, TDto>(T entity) where T : new()
        where TDto : new()
    {
        try
        {
            var kvp = new ValueTuple<Type, Type>(typeof(T), typeof(TDto));
            if (_configurations.TryGetValue(kvp, out var configuration))
            {
                var func = (Func<T, TDto>)configuration.ConfigurationFunc;
                if (configuration.IsDisposable)
                    _configurations.Remove(kvp);

                return func(entity);
            }

            throw new Exception($"Missing Configuration For {typeof(T)} and {typeof(TDto)}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Something went wrong with mapping {typeof(T)} and {typeof(TDto)}");
            throw;
        }
    }

    public void AddConfiguration<TDto, T>(Func<T, TDto> configuration) =>
        _configurations.Add(new ValueTuple<Type, Type>(typeof(T), typeof(TDto)),
            new Configuration.Configuration(configuration));

    public void AddDisposableConfiguration<TDto, T>(Func<T, TDto> disposableConfiguration) => _configurations.Add(
        new ValueTuple<Type, Type>(typeof(T), typeof(TDto)),
        new Configuration.Configuration(disposableConfiguration, true));
}