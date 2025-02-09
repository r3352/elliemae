// Decompiled with JetBrains decompiler
// Type: Elli.Common.Extensions.TypeHelperExtensionMethods
// Assembly: Elli.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 5A516607-8D77-4351-85BB-54751A6A69B4
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Common.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Elli.Common.Extensions
{
  public static class TypeHelperExtensionMethods
  {
    public static void ForEach<T>(this IEnumerable<T> query, Action<T> method)
    {
      foreach (T obj in query)
        method(obj);
    }

    public static bool IsEnumerableOfT(this Type type)
    {
      if (!type.IsGenericType)
        return false;
      Type genericTypeDefinition = type.GetGenericTypeDefinition();
      return genericTypeDefinition == typeof (IEnumerable<>) || genericTypeDefinition.DeclaringType == typeof (Enumerable);
    }

    public static Type GetUnderlyingTypeOfEnumerableOfT(this Type type)
    {
      return type.GetGenericArguments()[0];
    }

    public static Type GetUnderlyingTypeOfGenericOfT(this Type type)
    {
      return type.GetGenericArguments()[0];
    }

    public static bool IsNullable(this Type type)
    {
      return type.IsGenericType && type.GetGenericTypeDefinition() == typeof (Nullable<>);
    }

    public static bool IsNullableOrReference(this Type type)
    {
      return !type.IsValueType || type.IsNullable();
    }

    public static Type NullableOf(this Type type) => type.GetGenericArguments()[0];

    public static bool IsPrimitive(this Type type)
    {
      return type.IsValueType || type.IsNullable() || type == typeof (string);
    }

    public static bool IsNonPrimitive(this Type type) => !type.IsPrimitive();

    public static T As<T>(this object source) => (T) source;

    public static IEnumerable<Type> GetInterfaces(this Type type, bool includeInherited)
    {
      return includeInherited || type.BaseType == (Type) null ? (IEnumerable<Type>) type.GetInterfaces() : ((IEnumerable<Type>) type.GetInterfaces()).Except<Type>((IEnumerable<Type>) type.BaseType.GetInterfaces());
    }
  }
}
