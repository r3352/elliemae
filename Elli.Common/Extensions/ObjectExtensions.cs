// Decompiled with JetBrains decompiler
// Type: Elli.Common.Extensions.ObjectExtensions
// Assembly: Elli.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 5A516607-8D77-4351-85BB-54751A6A69B4
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Common.dll

using System;
using System.Collections.Generic;
using System.Reflection;

#nullable disable
namespace Elli.Common.Extensions
{
  public static class ObjectExtensions
  {
    public static bool IsNullableType(this Type type)
    {
      return type.IsGenericType && type.GetGenericTypeDefinition() == typeof (Nullable<>);
    }

    public static Type GetNullableValueType(this Type type)
    {
      return !type.IsNullableType() ? (Type) null : type.GetGenericArguments()[0];
    }

    public static Type GetUnderlyingValueTypeIfNullable(this Type type)
    {
      return !type.IsNullableType() ? type : type.GetGenericArguments()[0];
    }

    public static bool IsGenericEnumerable(this Type type)
    {
      return type.IsGenericType && typeof (IEnumerable<>).IsAssignableFrom(type.GetGenericTypeDefinition());
    }

    public static T Cast<T>(this object o) => (T) o;

    public static object Cast(this object o, Type type)
    {
      return typeof (ObjectExtensions).GetMethod(nameof (Cast), BindingFlags.Static | BindingFlags.Public).MakeGenericMethod(type).Invoke(o, (object[]) null);
    }
  }
}
