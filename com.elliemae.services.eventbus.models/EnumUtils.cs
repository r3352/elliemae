// Decompiled with JetBrains decompiler
// Type: com.elliemae.services.eventbus.models.EnumUtils
// Assembly: com.elliemae.services.eventbus.models, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 9B148EFB-427E-4DF5-8EA2-5C9491D22624
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\com.elliemae.services.eventbus.models.dll

using System;
using System.ComponentModel;

#nullable disable
namespace com.elliemae.services.eventbus.models
{
  public class EnumUtils
  {
    public static string StringValueOf(Enum value)
    {
      DescriptionAttribute[] customAttributes = (DescriptionAttribute[]) value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof (DescriptionAttribute), false);
      return customAttributes.Length != 0 ? customAttributes[0].Description : value.ToString();
    }

    public static object EnumValueOf(string value, System.Type enumType)
    {
      foreach (string name in Enum.GetNames(enumType))
      {
        if (EnumUtils.StringValueOf((Enum) Enum.Parse(enumType, name)).Equals(value))
          return Enum.Parse(enumType, name);
      }
      throw new ArgumentException("The string is not a description or value of the specified enum.");
    }
  }
}
