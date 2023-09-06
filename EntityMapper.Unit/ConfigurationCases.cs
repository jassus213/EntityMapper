using Entity.Unit.Entities.GoodCases;
using EntityMapper;
using EntityMapper.ServiceCollection.Extension;

namespace Entity.Unit;

public class ConfigurationCases
{
    [Test]
    public void Good_Configuration()
    {
        var entityMapper = new EntityMapper.EntityMapper();
        entityMapper.AddConfiguration<UserDto, User>((user) => new UserDto()
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
    public void Disposable_Configuration()
    {
        var entityMapper = new EntityMapper.EntityMapper();
        entityMapper.AddDisposableConfiguration<UserDto, User>((user) => new UserDto()
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