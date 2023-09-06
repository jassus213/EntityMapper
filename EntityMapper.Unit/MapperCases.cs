using Entity.Unit.Entities.GoodCases;
using Entity.Unit.Entities.StrangeCases;
using EntityMapper.Interfaces;
using EntityMapper.ServiceCollection.Extension;
using Microsoft.Extensions.DependencyInjection;

namespace Entity.Unit;

public class MapperCases
{
    [Test]
    public void DefaultCase_Good()
    {
        var serviceCollection = new Microsoft.Extensions.DependencyInjection.ServiceCollection();
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
            Id = 40,
            Name = "Test Name"
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
    public void Null_Case()
    {
        var serviceCollection = new ServiceCollection();
        var entityMapper = serviceCollection.UseMapper();
        entityMapper.AddMapperConfiguration<UserStrange, UserDtoStrange>((user) => new UserDtoStrange()
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
        
        var userDto = interfaceMapper.Map<UserStrange, UserDtoStrange>(user);
        Assert.Multiple(() =>
        {
            Assert.That(interfaceMapper, Is.EqualTo(entityMapper));
            Assert.That(userDto, Is.Not.Null);
            Assert.That(userDto.Id, Is.EqualTo(user.Id));
            Assert.That(userDto.FullName, Is.EqualTo($"{user.Name} {user.LastName}"));
        });
    }
    
    [Test]
    public void Not_Default_Case()
    {
        var serviceCollection = new ServiceCollection();
        var entityMapper = serviceCollection.UseMapper();
        entityMapper.AddMapperConfiguration<UserStrange, UserDtoStrange>((user) => new UserDtoStrange()
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
        
        var userDto = interfaceMapper.Map<UserStrange, UserDtoStrange>(user);
        Assert.Multiple(() =>
        {
            Assert.That(interfaceMapper, Is.EqualTo(entityMapper));
            Assert.That(userDto, Is.Not.Null);
            Assert.That(userDto.Id, Is.EqualTo(user.Id));
            Assert.That(userDto.FullName, Is.EqualTo($"{user.Name} {user.LastName}"));
        });
    }
}