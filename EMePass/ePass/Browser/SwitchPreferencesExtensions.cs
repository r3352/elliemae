// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ePass.Browser.SwitchPreferencesExtensions
// Assembly: EMePass, Version=3.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A610697F-A1EC-4CC3-A30A-403E37B2B276
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMePass.dll

using System.ComponentModel;

#nullable disable
namespace EllieMae.EMLite.ePass.Browser
{
  public static class SwitchPreferencesExtensions
  {
    public static string ToDescriptionString(this SwitchPreferences val)
    {
      DescriptionAttribute[] customAttributes = (DescriptionAttribute[]) val.GetType().GetField(val.ToString()).GetCustomAttributes(typeof (DescriptionAttribute), false);
      return customAttributes.Length == 0 ? string.Empty : customAttributes[0].Description;
    }
  }
}
