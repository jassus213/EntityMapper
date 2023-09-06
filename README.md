# EntityMapper Documentation
## Introducion
AutoMapper is a powerful and flexible object-to-object mapping library for .NET applications. It simplifies the process of mapping one object's properties to another, allowing you to focus on writing clean and maintainable code.
This documentation provides a detailed guide on how to use AutoMapper in your .NET projects. It covers the basic setup, configuration, mapping, and integration with the `ServiceCollection`.

## Basic Configuration
AutoMapper provides a straightforward way to configure object mapping. You can create a mapping configuration using the AutoMapper class and the AddConfiguration method.
```c#
var autoMapper = new AutoMapper();
autoMapper.AddConfiguration<UserDto, User>((user) => new UserDto()
{
    Id = user.Id,
    Name = user.Name
});
```
This code sets up a mapping from User objects to UserDto objects. It specifies how properties should be mapped from the source (User) to the destination (`UserDto`).

## Mapping Objects
Once you have configured AutoMapper, you can easily map objects using the Map method.
```c#
var userDto = autoMapper.Map<User, UserDto>(user);
```
In this example, we map a User object to a UserDto object, and AutoMapper takes care of copying the properties according to the configuration.

## ServiceCollection Integration
AutoMapper can be integrated with the ServiceCollection in ASP.NET Core applications to enable dependency injection of mappers. Here's how to set it up:
```c#
var serviceCollection = new ServiceCollection();
var mapper = serviceCollection.UseMapper();
mapper.AddMapperConfiguration<User, UserDto>((user) => new UserDto()
{
    Id = user.Id,
    Name = user.Name
});
```
This code configures AutoMapper to work with the ServiceCollection. It allows you to inject the mapper into your services, making it easy to use AutoMapper within your application's services and controllers.
## Conclusion
AutoMapper is a powerful tool for simplifying object-to-object mapping in your .NET applications. By following this documentation, you can quickly set up and use AutoMapper to streamline your code and improve maintainability. It's worth noting that AutoMapper is extensively tested, with a test suite that achieves 
`100% code coverage` to ensure its reliability and correctness in various scenarios. This comprehensive testing ensures that AutoMapper works seamlessly in your projects, providing confidence in its behavior and robustness.
