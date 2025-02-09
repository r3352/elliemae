// Decompiled with JetBrains decompiler
// Type: Elli.SQE.DotNetCoreTypeExtensions
// Assembly: Elli.SQE, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 49809150-F4C8-4F09-B50E-E491713B0B30
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.SQE.dll

using System;

#nullable disable
namespace Elli.SQE
{
  public static class DotNetCoreTypeExtensions
  {
    public static TypeCode GetTypeCode(this Type type) => Type.GetTypeCode(type);

    public static bool IsEnumType(this Type type) => type.IsEnum;

    public static bool IsValue(this Type type) => type.IsValueType;

    public static bool IsNullable(this Type type)
    {
      return Nullable.GetUnderlyingType(type) != (Type) null;
    }
  }
}
