// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.Configuration.InstanceConfigurationExtensions
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.Common.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#nullable disable
namespace EllieMae.EMLite.Server.Configuration
{
  public static class InstanceConfigurationExtensions
  {
    public static Dictionary<string, bool> CompareWith(
      this IInstanceConfiguration first,
      IInstanceConfiguration second,
      string[] ignores = null)
    {
      return ((IEnumerable<PropertyInfo>) typeof (IInstanceConfiguration).GetProperties()).Where<PropertyInfo>((Func<PropertyInfo, bool>) (prop => ignores == null || !((IEnumerable<string>) ignores).Any<string>((Func<string, bool>) (ig => ig.Equals(prop.Name, StringComparison.CurrentCultureIgnoreCase))))).ToDictionary<PropertyInfo, string, bool>((Func<PropertyInfo, string>) (prop => prop.Name), (Func<PropertyInfo, bool>) (prop => object.Equals(prop.GetValue((object) first), prop.GetValue((object) second))));
    }

    public static void CopyTo(
      this IInstanceConfiguration first,
      IInstanceConfiguration second,
      string[] ignores = null)
    {
      foreach (PropertyInfo propertyInfo in ((IEnumerable<PropertyInfo>) typeof (IInstanceConfiguration).GetProperties()).Where<PropertyInfo>((Func<PropertyInfo, bool>) (prop => ignores == null || !((IEnumerable<string>) ignores).Any<string>((Func<string, bool>) (ig => ig.Equals(prop.Name, StringComparison.CurrentCultureIgnoreCase))))).ToList<PropertyInfo>())
        propertyInfo.SetValue((object) second, propertyInfo.GetValue((object) first));
    }
  }
}
