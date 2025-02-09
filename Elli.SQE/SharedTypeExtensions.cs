// Decompiled with JetBrains decompiler
// Type: Elli.SQE.SharedTypeExtensions
// Assembly: Elli.SQE, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 49809150-F4C8-4F09-B50E-E491713B0B30
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.SQE.dll

using System;
using System.Diagnostics;
using System.Reflection;

#nullable disable
namespace Elli.SQE
{
  [DebuggerStepThrough]
  public static class SharedTypeExtensions
  {
    public static Type UnwrapNullableType(this Type type)
    {
      Type underlyingType = Nullable.GetUnderlyingType(type);
      return (object) underlyingType != null ? underlyingType : type;
    }

    public static bool IsNullableType(this Type type)
    {
      TypeInfo typeInfo = type.GetTypeInfo();
      if (!typeInfo.IsValueType)
        return true;
      return typeInfo.IsGenericType && typeInfo.GetGenericTypeDefinition() == typeof (Nullable<>);
    }

    public static Type MakeNullable(this Type type)
    {
      if (type.IsNullableType())
        return type;
      return typeof (Nullable<>).MakeGenericType(type);
    }

    public static Type UnwrapEnumType(this Type type)
    {
      return !type.GetTypeInfo().IsEnum ? type : Enum.GetUnderlyingType(type);
    }
  }
}
