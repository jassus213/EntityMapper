using Entity.Unit.Entities.GoodCases;
using Entity.Unit.Entities.StrangeCases;
using EntityMapper;
using EntityMapper.Interfaces;
using EntityMapper.ServiceCollection.Extension;
using Microsoft.Extensions.DependencyInjection;

namespace Entity.Unit;

public class ConfigurationCases
{
    [Test]
    public void Good_Configuration()
    {
        var entityMapper = new EntityMapper.EntityMapper();
        entityMapper.AddConfiguration<User, UserDto>((user) => new UserDto()
        {
            Id = user.Id,
            Name = user.Name
        });

        var user = new User()
        {
            Id = 10,
            Name = "Nikita"
        };

        var userDto = entityMapper.Map<User, UserDto>(user);
        Assert.Multiple(() =>
        {
            Assert.IsNotNull(userDto);
            Assert.That(userDto.Name, Is.EqualTo("Nikita"));
            Assert.That(user.Id, Is.EqualTo(10));
        });
    }

    [Test]
    public void Missing_Configuration()
    {
        var entityMapper = new EntityMapper.EntityMapper();
        var user = new User()
        {
            Id = 10,
            Name = "Nikita"
        };
        
        var exception = Assert.Catch<Exception>(() => entityMapper.Map<User, UserDto>(user));
        Assert.Multiple(() =>
        {
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.EqualTo("Missing Configuration For Entity.Unit.Entities.GoodCases.User and Entity.Unit.Entities.GoodCases.UserDto"));
        });
    }

    [Test]
    public void Disposable_Missing_Configuration()
    {
        var entityMapper = new EntityMapper.EntityMapper();
        entityMapper.AddDisposableConfiguration<User, UserDto>((user) => new UserDto()
        {
            Id = user.Id,
            Name = user.Name
        });
        
        var user = new User()
        {
            Id = 10,
            Name = "Nikita"
        };

        var userDto = entityMapper.Map<User, UserDto>(user);
        var exception = Assert.Catch<Exception>(() => entityMapper.Map<User, UserDto>(user));
        Assert.Multiple(() =>
        {
            Assert.That(userDto, Is.Not.Null);
            Assert.That(user.Id, Is.EqualTo(10));
            Assert.That(userDto.Name, Is.EqualTo("Nikita"));
            
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.EqualTo("Missing Configuration For Entity.Unit.Entities.GoodCases.User and Entity.Unit.Entities.GoodCases.UserDto"));
        });
    }

    [Test]
    public async Task Bad_AsyncConfiguration()
    {
        var serviceCollection = new ServiceCollection();
        var entityMapper = serviceCollection.UseMapper();
        entityMapper.AddConfiguration<UserStrange, UserDtoStrange>((user) => new UserDtoStrange()
        {
            Id = user.Id,
            FullName = $"{user.Name} {user.LastName}"
        });
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var interfaceMapper = serviceProvider.GetRequiredService<IMapper>();
        var user = new UserStrange()
        {
            Id = 25,
            Name = "Nikita",
            LastName = "Okhotnikov"
        };

        var exception = Assert.CatchAsync(async () => await interfaceMapper.MapAsync<UserStrange, UserDtoStrange>(user));
        Assert.Multiple(() =>
        {
            Assert.That(interfaceMapper, Is.EqualTo(entityMapper));
            Assert.That(exception.Message, Is.EqualTo("Is Not Async Configuration"));
        });
    }
    
    [Test]
    public async Task Good_AsyncConfiguration()
    {
        var serviceCollection = new ServiceCollection();
        var entityMapper = serviceCollection.UseMapper();
        entityMapper.AddAsyncMapperConfiguration<UserStrange, UserDtoStrange>(async (user) =>
        {
            await Task.Delay(500);
            return new UserDtoStrange()
            {
                Id = user.Id,
                FullName = $"{user.Name} {user.LastName}"
            };
        });
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var interfaceMapper = serviceProvider.GetRequiredService<IMapper>();
        var user = new UserStrange()
        {
            Id = 25,
            Name = "Nikita",
            LastName = "Okhotnikov"
        };

        var userDto = await interfaceMapper.MapAsync<UserStrange, UserDtoStrange>(user);
        Assert.Multiple(() =>
        {
            Assert.That(interfaceMapper, Is.EqualTo(entityMapper));
            
            Assert.That(userDto, Is.Not.Null);
            Assert.That(user.Id, Is.EqualTo(25));
            Assert.That(userDto.FullName, Is.EqualTo("Nikita Okhotnikov"));
        });
    }

    [Test]
    public async Task Async_Missing_Configuration()
    {
        var serviceCollection = new ServiceCollection();
        var entityMapper = serviceCollection.UseMapper();
        var user = new User()
        {
            Id = 25,
            Name = "Nikita",
        };
        
        var exception = Assert.CatchAsync<Exception>(async () => await entityMapper.MapAsync<User, UserDto>(user));
        Assert.Multiple(() =>
        {
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.EqualTo("Missing Configuration For Entity.Unit.Entities.GoodCases.User and Entity.Unit.Entities.GoodCases.UserDto"));
        });
    }
}