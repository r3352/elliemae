// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.EnumUtil
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

#nullable disable
namespace EllieMae.EMLite.Common
{
  public static class EnumUtil
  {
    public static string GetEnumDescription(Enum value)
    {
      try
      {
        DescriptionAttribute[] customAttributes = (DescriptionAttribute[]) value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof (DescriptionAttribute), false);
        return customAttributes != null && customAttributes.Length != 0 ? customAttributes[0].Description : value.ToString();
      }
      catch
      {
        return string.Empty;
      }
    }

    public static string[] GetEnumDescriptions(Type a)
    {
      List<string> stringList = new List<string>();
      foreach (Enum @enum in Enum.GetValues(a))
        stringList.Add(EnumUtil.GetEnumDescription(@enum));
      return stringList.ToArray();
    }

    public static T GetEnumValueFromDescription<T>(string description)
    {
      Type type = typeof (T);
      if (!type.IsEnum)
        throw new InvalidOperationException();
      foreach (FieldInfo field in type.GetFields())
      {
        if (Attribute.GetCustomAttribute((MemberInfo) field, typeof (DescriptionAttribute)) is DescriptionAttribute customAttribute)
        {
          if (customAttribute.Description == description)
            return (T) field.GetValue((object) null);
        }
        else if (field.Name == description)
          return (T) field.GetValue((object) null);
      }
      throw new ArgumentException("Not found.", nameof (description));
    }

    public static object GetEnumValueFromDescription(string description, Type type)
    {
      if (!type.IsEnum)
        throw new InvalidOperationException();
      foreach (FieldInfo field in type.GetFields())
      {
        if (Attribute.GetCustomAttribute((MemberInfo) field, typeof (DescriptionAttribute)) is DescriptionAttribute customAttribute)
        {
          if (customAttribute.Description == description)
            return field.GetValue((object) null);
        }
        else if (field.Name == description)
          return field.GetValue((object) null);
      }
      throw new ArgumentException("Not found.", nameof (description));
    }

    public static T ToEnum<T>(this string value, bool ignoreCase = true)
    {
      return (T) Enum.Parse(typeof (T), value, ignoreCase);
    }

    public static TEnum ParseEnum<TEnum>(string value) where TEnum : struct
    {
      TEnum result;
      if (!Enum.TryParse<TEnum>(value, true, out result))
        result = new TEnum();
      return result;
    }
  }
}
