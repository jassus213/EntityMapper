# EntityMapper Documentation
![Static Badge](https://img.shields.io/badge/latest_version-2.2.2-blue) ![Static Badge](https://img.shields.io/badge/license-MIT-green)
## Introducion
EntityMapper is a powerful and flexible object-to-object mapping library for .NET applications. It simplifies the process of mapping one object's properties to another, allowing you to focus on writing clean and maintainable code.
This documentation provides a detailed guide on how to use EntityMapper in your .NET projects. It covers the basic setup, configuration, mapping, and integration with the `ServiceCollection`.

## Installation 
## Using .NET CLI
To install EntityMapper using the .NET CLI, open your command-line interface and navigate to your project's directory.
Run the following command to add the EntityMapper package to your project:
```c#
dotnet add package Entity.Mapper.Asp --version 1.0.0
```
## Using NuGet Package Manager Console
If you prefer to use Visual Studio and its integrated development environment, you can install EntityMapper using the NuGet Package Manager Console.
Open Visual Studio.
Go To `"Tools" > "NuGet Package Manager" > "Package Manager Console."`
In the Package Manager Console, run the following command to install EntityMapper:
```c#
NuGet\Install-Package Entity.Mapper.Asp -Version 1.0.0
```
This command will install the latest version of EntityMapper into your project.
## Using PackageReference (MSBuild)
In some cases, you may want to integrate EntityMapper into your project using MSBuild. This method is useful when you need more control over how EntityMapper is incorporated into your build process.
Open your project file (e.g., .csproj) in a text editor or Visual Studio or Rider By F4.
Add a PackageReference element for EntityMapper in the ItemGroup section:
```c#
<ItemGroup>
  <PackageReference Include="EntityMapper" Version="X.Y.Z" />
</ItemGroup>
```
Replace "X.Y.Z" with the desired version of EntityMapper.
Save the project file.
To restore the packages, you can use the following command in the command-line interface:
```c#
dotnet restore
```
This command will download and install EntityMapper and its dependencies into your project.
## Basic Configuration
EntityMapper provides a straightforward way to configure object mapping. You can create a mapping configuration using the EntityMapper class and the AddConfiguration method.
```c#
var entityMapper = new EntityMapper();
entityMapper.AddConfiguration<UserDto, User>((user) => new UserDto()
{
    Id = user.Id,
    Name = user.Name
});
```
This code sets up a mapping from User objects to UserDto objects. It specifies how properties should be mapped from the source (User) to the destination (`UserDto`).
## Disposable Mapper Configurations
EntityMapper traditionally uses a global configuration to define how objects of one type should be mapped to another. While this approach works well for most cases, it can lead to memory overhead when configuring mappings that are seldom used, especially in applications where memory efficiency is crucial.

To address this issue, EntityMapper introduced Disposable Mapper Configurations. This feature allows you to define and configure a mapping for a specific use case, and once that mapping is no longer needed, it is automatically disposed of. This can be particularly beneficial for scenarios where you want to minimize memory consumption, such as handling infrequent or specialized mappings.
```c#
entityMapper.AddDisposableConfiguration<User, UserDto>((user) => new UserDto()
{
    Id = user.Id,
    Name = user.Name
});
```
In this code snippet, we define a mapping from the User class to the UserDto class. The unique aspect is that this mapping is treated as disposable. Once the mapping has been used (typically after the first request), EntityMapper automatically disposes of it, freeing up any resources associated with it.
## Asynchronous Configuration
EntityMapper provides the capability to define asynchronous mapping functions, allowing you to perform asynchronous operations during the mapping process. This is achieved by using asynchronous delegates (lambda expressions) in the `AddAsyncMapperConfiguration` method. Let's look at an example of using asynchronous configuration:
```c#
entityMapper.AddAsyncConfiguration<User, UserDto>(async (user) =>
{
    // Perform an asynchronous operation (e.g., delay)
    await Task.Delay(500);

    // Create a UserDtoStrange object with asynchronously retrieved data
    return new UserDto()
    {
        Id = user.Id,
        FullName = $"{user.Name} {user.LastName}"
    };
});
```
In this example, we create an asynchronous mapping configuration between the UserStrange and UserDtoStrange types. Inside the lambda expression, we use the async keyword to define an asynchronous mapping function. Within the mapping function, we perform an asynchronous operation, such as Task.Delay, to introduce an artificial delay of 500 milliseconds. Then, we create a UserDtoStrange object with data retrieved asynchronously.

## Bidirectional Mapping Configuration
EntityMapper simplifies bidirectional mapping by allowing you to set up mappings in both directions using a single configuration. Let's examine an example of bidirectional mapping configuration:
```c#
var user = new User()
{
    Id = 20,
    Name = "Test Name"
};

entityMapper.AddBidirectionalMapping<User, UserDto>((user) => new UserDto()
{
    Id = user.Id,
    Name = user.Name
});

var dto = entityMapper.Map<User, UserDto>(user);
var userNew = entityMapper.Map<UserDto, User>(dto);
```
In this code snippet, we perform the following steps:
1. We create a User object with some initial data.
2. We use the AddABidirectionalMapperConfiguration method to configure bidirectional mapping between User and UserDto. This method accepts a mapping function that defines how User objects are mapped to UserDto objects.
3. We use the Map method to map a User object (user) to a UserDto object (dto).
4. Finally, we use the Map method again to map the UserDto object (dto) back to a User object (userNew).
The bidirectional mapping configuration allows you to effortlessly switch between User and UserDto objects while maintaining the integrity of your data.

## Mapping Objects
Once you have configured EntityMapper, you can easily map objects using the Map method.
```c#
var userDto = EntityMapper.Map<User, UserDto>(user);
```
In this example, we map a User object to a UserDto object, and EntityMapper takes care of copying the properties according to the configuration.

## Extending ServiceCollection with EntityMapper Configuration
In many .NET applications, configuring object mapping using a custom mapper, like entityMapper, is a common task. This custom mapper simplifies the process of mapping objects from one type to another, reducing boilerplate code. To make this process even more convenient and flexible, we can extend the ServiceCollection in ASP.NET Core applications with a custom extension method, UseMapper(). This method provides access to a configurator object that allows the addition of various mapping configurations. In this chapter, we'll explore how to use this extension and the four types of mapping configurations it supports.
### Using `UseMapper()` Extension
To streamline the configuration of entity mappings within an ASP.NET Core application, we can extend the ServiceCollection class with the UseMapper() extension method. This method returns an IMapperConfigurator object, which enables us to add different types of mapping configurations.

Here's how to use the UseMapper() method:
```c#
services.UseMapper();
```
This line of code adds the entity mapper to the DI container and returns the IMapperConfigurator interface, which we can use to configure mappings.

## Adding Mapping Configurations
### 1. Regular Mapping Configuration
The `AddMapperConfiguration<TSource, TDestination>()` method allows you to add a standard mapping configuration. This configuration is suitable for most mapping scenarios where you want to define how properties of one type map to another.
```c#
services.UseMapper()
    .AddMapperConfiguration<User, UserDto>((user) => new UserDto()
    {
        Id = user.Id,
        Name = user.Name
    });
```
### 2. Disposable Mapping Configuration
The `AddDisposableMapperConfiguration<TSource, TDestination>()` method adds a mapping configuration that is automatically removed from the list of configurations after its initial use. This is useful for scenarios where you want to optimize memory usage by disposing of configurations that are only needed temporarily.
```c#
services.UseMapper()
    .AddDisposableMapperConfiguration<User, UserDto>((user) => new UserDto()
    {
        Id = user.Id,
        Name = user.Name
    });
```
### 3. Async Mapping Configuration
For scenarios where asynchronous mapping is required, the `AddAsyncMapperConfiguration<TSource, TDestination>()` method allows you to specify an asynchronous mapping configuration.
```c#
services.UseMapper()
    .AddAsyncMapperConfiguration<User, UserDto>(async (user) =>
    {
        // Perform asynchronous mapping logic here
        return await SomeAsyncMethod(user);
    });
```
### 4. Bidirectional Mapping Configuration
The `AddABidirectionalMapperConfiguration<TSource, TDestination>()` method is a special method that adds both the specified configuration and its reverse (Map Back) configuration. This is useful when you need to map objects bidirectionally.
```c#
services.UseMapper()
    .AddABidirectionalMapperConfiguration<User, UserDto>((user) => new UserDto()
    {
        Id = user.Id,
        Name = user.Name
    });
```

## Conclusion
EntityMapper is a powerful tool for simplifying object-to-object mapping in your .NET applications. By following this documentation, you can quickly set up and use EntityMapper to streamline your code and improve maintainability. It's worth noting that EntityMapper is extensively tested, with a test suite that achieves 
`100% code coverage` to ensure its reliability and correctness in various scenarios. This comprehensive testing ensures that EntityMapper works seamlessly in your projects, providing confidence in its behavior and robustness.
