using System.Linq.Expressions;
using System.Reflection;
using EntityMapper.Interfaces;

namespace EntityMapper;

/// <summary>
/// Call the Map method if you need to map from one object to another
/// </summary>
public class EntityMapper : IMapper
{
    private readonly Dictionary<ValueTuple<Type, Type>, Configuration.Configuration> _configurations =
        new Dictionary<(Type, Type), Configuration.Configuration>();

    public TDestination Map<TSource, TDestination>(TSource entity) where TSource : new()
        where TDestination : new()
    {
        try
        {
            var kvp = new ValueTuple<Type, Type>(typeof(TSource), typeof(TDestination));
            if (_configurations.TryGetValue(kvp, out var configuration))
            {
                var func = (Func<TSource, TDestination>)configuration.ConfigurationFunc;
                if (configuration.IsDisposable)
                    _configurations.Remove(kvp);

                return func(entity);
            }

            throw new Exception($"Missing Configuration For {typeof(TSource)} and {typeof(TDestination)}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Something went wrong with mapping {typeof(TSource)} and {typeof(TDestination)}");
            throw;
        }
    }

    public async Task<TDestination> MapAsync<TSource, TDestination>(TSource source) where TSource : new()
    {
        var kvp = new ValueTuple<Type, Type>(typeof(TSource), typeof(TDestination));
        if (_configurations.TryGetValue(kvp, out var configuration))
        {
            var configurationFunc = configuration.ConfigurationFunc as Func<TSource, Task<TDestination>>;
            if (configurationFunc == null)
                throw new Exception("Is Not Async Configuration");

            // TODO Async Disposable Configuration

            return await configurationFunc(source);
        }

        throw new Exception($"Missing Configuration For {typeof(TSource)} and {typeof(TDestination)}");
    }

    public void AddConfiguration<TSource, TDestination>(Func<TSource, TDestination> configuration) =>
        _configurations.Add(new ValueTuple<Type, Type>(typeof(TSource), typeof(TDestination)),
            new Configuration.Configuration(configuration));

    public void AddDisposableConfiguration<TSource, TDestination>(
        Func<TSource, TDestination> disposableConfiguration) =>
        _configurations.Add(
            new ValueTuple<Type, Type>(typeof(TSource), typeof(TDestination)),
            new Configuration.Configuration(disposableConfiguration, true));

    public void AddAsyncConfiguration<TSource, TDestination>(Func<TSource, Task<TDestination>> configuration) =>
        _configurations.Add(new ValueTuple<Type, Type>(typeof(TSource), typeof(TDestination)),
            new Configuration.Configuration(configuration));

    public void AddBidirectionalMapping<TSource, TDestination>(Expression<Func<TSource, TDestination>> expression)
    {
        var body = expression.Body.ReduceExtensions();

        var sourceParam = Expression.Parameter(typeof(TSource), typeof(TSource).Name);
        var destinationParam = Expression.Parameter(typeof(TDestination), typeof(TDestination).Name);

        var memberInitExpression = (MemberInitExpression)body;

        var bindings = memberInitExpression.Bindings;
        var reversedBindings = new List<MemberBinding>(memberInitExpression.Bindings.Count);
        foreach (var binding in bindings)
        {
            if (binding is MemberAssignment memberAssignment)
            {
                var memberInfo = memberAssignment.Member;
                
                var correspondingProperty = typeof(TSource).GetProperty(memberInfo.Name);
                if (correspondingProperty != null)
                {
                    var propertyAccess = Expression.Property(sourceParam, correspondingProperty);
                    reversedBindings.Add(Expression.Bind(propertyAccess.Member,
                        Expression.Property(destinationParam, (PropertyInfo)memberInfo)));
                }
            }
        }

        var reversedMemberInit = Expression.MemberInit(Expression.New(typeof(TSource)), reversedBindings);

        var reversedLambda = Expression.Lambda<Func<TDestination, TSource>>(reversedMemberInit, destinationParam);
        var reversedConfiguration = reversedLambda.Compile();
        

        _configurations.Add(new ValueTuple<Type, Type>(typeof(TSource), typeof(TDestination)),
            new Configuration.Configuration(expression.Compile()));

        _configurations.Add(new ValueTuple<Type, Type>(typeof(TDestination), typeof(TSource)),
            new Configuration.Configuration(reversedConfiguration));
    }
}