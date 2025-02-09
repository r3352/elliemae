// Decompiled with JetBrains decompiler
// Type: Elli.Service.Common.WCF.KnownTypeProvider
// Assembly: Elli.Service.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: C15A70DC-1690-48C1-BCA7-AD8CA880845A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Service.Common.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#nullable disable
namespace Elli.Service.Common.WCF
{
  public static class KnownTypeProvider
  {
    private static List<Type> _knownTypes = new List<Type>();

    public static void ClearAllKnownTypes() => KnownTypeProvider._knownTypes = new List<Type>();

    public static void Register<T>() => KnownTypeProvider.Register(typeof (T));

    public static void Register(Type type) => KnownTypeProvider._knownTypes.Add(type);

    public static void RegisterDerivedTypesOf<T>(Assembly assembly)
    {
      KnownTypeProvider.RegisterDerivedTypesOf(typeof (T), (IEnumerable<Type>) assembly.GetTypes());
    }

    public static void RegisterDerivedTypesOf<T>(IEnumerable<Type> types)
    {
      KnownTypeProvider.RegisterDerivedTypesOf(typeof (T), types);
    }

    public static void RegisterDerivedTypesOf(Type type, Assembly assembly)
    {
      KnownTypeProvider.RegisterDerivedTypesOf(type, (IEnumerable<Type>) assembly.GetTypes());
    }

    public static void RegisterDerivedTypesOf(Type type, IEnumerable<Type> types)
    {
      List<Type> derivedTypesOf = KnownTypeProvider.GetDerivedTypesOf(type, types);
      KnownTypeProvider._knownTypes = KnownTypeProvider.Union<Type>((IEnumerable<Type>) KnownTypeProvider._knownTypes, (IEnumerable<Type>) derivedTypesOf);
    }

    public static IEnumerable<Type> GetKnownTypes(ICustomAttributeProvider provider)
    {
      return (IEnumerable<Type>) KnownTypeProvider._knownTypes;
    }

    private static List<Type> GetDerivedTypesOf(Type baseType, IEnumerable<Type> types)
    {
      return types.Where<Type>((Func<Type, bool>) (t => !t.IsAbstract && t.IsSubclassOf(baseType))).ToList<Type>();
    }

    private static List<T> Union<T>(IEnumerable<T> first, IEnumerable<T> second)
    {
      return first.Union<T>(second).ToList<T>();
    }
  }
}
