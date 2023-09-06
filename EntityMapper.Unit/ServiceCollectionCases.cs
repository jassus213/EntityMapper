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
        var mapper = serviceCollection.UseMapper();
        mapper.AddMapperConfiguration<User, UserDto>((user) => new UserDto()
        {
            Id = user.Id,
            Name = user.Name
        });
        
        var user = new User()
        {
            Id = 25,
            Name = "This is Test Name"
        };
        
        var userDto = mapper.Map<User, UserDto>(user);
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
        var mapper = serviceCollection.UseMapper();
        mapper.AddMapperConfiguration<User, UserDto>((user) => new UserDto()
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
            Assert.That(interfaceMapper, Is.EqualTo(mapper));
            Assert.That(userDto, Is.Not.Null);
            Assert.That(userDto.Id, Is.EqualTo(user.Id));
            Assert.That(userDto.Name, Is.EqualTo(user.Name));
        });
    }

    
}