using System.Reflection;
using AutoMapper;

namespace Payroll.Services.Mapping;

public class MappingProfile : Profile
{
  public MappingProfile() => ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());

  private void ApplyMappingsFromAssembly(Assembly assembly)
  {
    var types = assembly.GetExportedTypes()
      .Where(type => type.GetInterfaces()
        .Any(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IMapFrom<>)))
      .ToList();

    foreach (var type in types)
    {
      var instance = Activator.CreateInstance(type);
      var methodInfo = type.GetMethod("Mapping") ?? type.GetInterface("IMapFrom`1")!.GetMethod("Mapping");
      methodInfo?.Invoke(instance, new object[] {this});
    }
  }
}
