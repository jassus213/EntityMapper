# EntityMapper Documentation
## Introducion
EntityMapper is a powerful and flexible object-to-object mapping library for .NET applications. It simplifies the process of mapping one object's properties to another, allowing you to focus on writing clean and maintainable code.
This documentation provides a detailed guide on how to use EntityMapper in your .NET projects. It covers the basic setup, configuration, mapping, and integration with the `ServiceCollection`.

## Installation 
## <a id="installcli"/> Using .NET CLI
To install EntityMapper using the .NET CLI, open your command-line interface and navigate to your project's directory.
Run the following command to add the EntityMapper package to your project:
```c#
dotnet add package Entity.Mapper.Asp --version 1.0.0
```
## <a id="installnugetpackage"/> Using NuGet Package Manager Console
If you prefer to use Visual Studio and its integrated development environment, you can install EntityMapper using the NuGet Package Manager Console.
Open Visual Studio.
Go To `"Tools" > "NuGet Package Manager" > "Package Manager Console."`
In the Package Manager Console, run the following command to install EntityMapper:
```c#
NuGet\Install-Package Entity.Mapper.Asp -Version 1.0.0
```
This command will install the latest version of EntityMapper into your project.
## <a id="installmsbuild"/> Using PackageReference (MSBuild)
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
var EntityMapper = new EntityMapper();
EntityMapper.AddConfiguration<UserDto, User>((user) => new UserDto()
{
    Id = user.Id,
    Name = user.Name
});
```
This code sets up a mapping from User objects to UserDto objects. It specifies how properties should be mapped from the source (User) to the destination (`UserDto`).

## Mapping Objects
Once you have configured EntityMapper, you can easily map objects using the Map method.
```c#
var userDto = EntityMapper.Map<User, UserDto>(user);
```
In this example, we map a User object to a UserDto object, and EntityMapper takes care of copying the properties according to the configuration.

## ServiceCollection Integration
EntityMapper can be integrated with the ServiceCollection in ASP.NET Core applications to enable dependency injection of mappers. Here's how to set it up:
```c#
var serviceCollection = new ServiceCollection();
var mapper = serviceCollection.UseMapper();
mapper.AddMapperConfiguration<User, UserDto>((user) => new UserDto()
{
    Id = user.Id,
    Name = user.Name
});
```
This code configures EntityMapper to work with the ServiceCollection. It allows you to inject the mapper into your services, making it easy to use EntityMapper within your application's services and controllers.
## Conclusion
EntityMapper is a powerful tool for simplifying object-to-object mapping in your .NET applications. By following this documentation, you can quickly set up and use EntityMapper to streamline your code and improve maintainability. It's worth noting that EntityMapper is extensively tested, with a test suite that achieves 
`100% code coverage` to ensure its reliability and correctness in various scenarios. This comprehensive testing ensures that EntityMapper works seamlessly in your projects, providing confidence in its behavior and robustness.
