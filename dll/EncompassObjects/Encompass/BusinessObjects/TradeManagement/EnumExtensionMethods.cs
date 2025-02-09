// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.TradeManagement.EnumExtensionMethods
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System;
using System.ComponentModel;
using System.Reflection;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.TradeManagement
{
  internal static class EnumExtensionMethods
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
