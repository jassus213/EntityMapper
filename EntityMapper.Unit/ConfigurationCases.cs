using Entity.Unit.Entities.GoodCases;
using EntityMapper;

namespace Entity.Unit;

public class ConfigurationCases
{
    [Test]
    public void Good_Configuration()
    {
        var autoMapper = new AutoMapper();
        autoMapper.AddConfiguration<UserDto, User>((user) => new UserDto()
        {
            Id = user.Id,
            Name = user.Name
        });

        var user = new User()
        {
            Id = 10,
            Name = "Nikita"
        };

        var userDto = autoMapper.Map<User, UserDto>(user);
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
        var autoMapper = new AutoMapper();
        var user = new User()
        {
            Id = 10,
            Name = "Nikita"
        };
        
        var exception = Assert.Catch<Exception>(() => autoMapper.Map<User, UserDto>(user));
        Assert.Multiple(() =>
        {
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.EqualTo("Missing Configuration For Entity.Unit.Entities.GoodCases.User and Entity.Unit.Entities.GoodCases.UserDto"));
        });
    }
}