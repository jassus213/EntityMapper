using Entity.Unit.Entities.GoodCases;
using EntityMapper.Interfaces;
using EntityMapper.ServiceCollection.Extension;
using Microsoft.Extensions.DependencyInjection;

namespace Entity.Unit;

public class ServiceCollectionCases
{
    [Test]
    public void Good_Registration()
    {
        var serviceCollection = new ServiceCollection();
        var entityMapper = serviceCollection.UseMapper();
        entityMapper.AddMapperConfiguration<User, UserDto>((user) => new UserDto()
        {
            Id = user.Id,
            Name = user.Name
        });
        
        var user = new User()
        {
            Id = 25,
            Name = "This is Test Name"
        };
        
        var userDto = entityMapper.Map<User, UserDto>(user);
        Assert.Multiple(() =>
        {
            Assert.That(userDto, Is.Not.Null);
            Assert.That(userDto.Id, Is.EqualTo(user.Id));
            Assert.That(userDto.Name, Is.EqualTo(user.Name));
        });
    }

    [Test]
    public void Good_Registration_ByInterface()
    {
        var serviceCollection = new ServiceCollection();
        var entityMapper = serviceCollection.UseMapper();
        entityMapper.AddMapperConfiguration<User, UserDto>((user) => new UserDto()
        {
            Id = user.Id,
            Name = user.Name
        });
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var interfaceMapper = serviceProvider.GetRequiredService<IMapper>();

        var user = new User()
        {
            Id = 25,
            Name = "This is Test Name"
        };
        
        var userDto = interfaceMapper.Map<User, UserDto>(user);
        Assert.Multiple(() =>
        {
            Assert.That(interfaceMapper, Is.EqualTo(entityMapper));
            Assert.That(userDto, Is.Not.Null);
            Assert.That(userDto.Id, Is.EqualTo(user.Id));
            Assert.That(userDto.Name, Is.EqualTo(user.Name));
        });
    }

    [Test]
    public void DisposableConfiguration_Good()
    {
        var serviceCollection = new ServiceCollection();
        var entityMapper = serviceCollection.UseMapper();
        entityMapper.AddDisposableMapperConfiguration<User, UserDto>((user) => new UserDto()
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

    
}