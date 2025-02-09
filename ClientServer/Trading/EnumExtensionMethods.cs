// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.EnumExtensionMethods
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;
using System.ComponentModel;
using System.Reflection;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  public static class EnumExtensionMethods
  {
    public static string ToDescription(this Enum en)
    {
      MemberInfo[] member = en.GetType().GetMember(en.ToString());
      if (member != null && member.Length != 0)
      {
        object[] customAttributes = member[0].GetCustomAttributes(typeof (DescriptionAttribute), false);
        if (customAttributes != null && customAttributes.Length != 0)
          return ((DescriptionAttribute) customAttributes[0]).Description;
      }
      return en.ToString();
    }
  }
}
