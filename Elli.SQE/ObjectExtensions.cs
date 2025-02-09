// Decompiled with JetBrains decompiler
// Type: Elli.SQE.ObjectExtensions
// Assembly: Elli.SQE, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 49809150-F4C8-4F09-B50E-E491713B0B30
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.SQE.dll

using System;

#nullable disable
namespace Elli.SQE
{
  public static class ObjectExtensions
  {
    public static T CastTo<T>(this object value) where T : class => (T) value;

    public static T CastAs<T>(this object value) where T : class => value as T;

    public static bool IsNull<T>(this T obj) where T : class => (object) obj == null;

    public static bool IsNotNull<T>(this T obj) where T : class => (object) obj != null;

    public static bool IsDate(this object value)
    {
      if (value == null)
        return false;
      Type type = value.GetType();
      return type == typeof (DateTime) || type == typeof (DateTime?);
    }
  }
}
