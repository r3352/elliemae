// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Configuration.SpecialFeatureCodeDefinitionExtensions
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Configuration
{
  public static class SpecialFeatureCodeDefinitionExtensions
  {
    private static StringComparison comparison = StringComparison.InvariantCultureIgnoreCase;

    public static bool IsOtherActiveCodeSource(
      this IList<SpecialFeatureCodeDefinition> list,
      SpecialFeatureCodeDefinition thisSfc)
    {
      return list != null && list.Any<SpecialFeatureCodeDefinition>((Func<SpecialFeatureCodeDefinition, bool>) (sfc => sfc.Code.Equals(thisSfc.Code, SpecialFeatureCodeDefinitionExtensions.comparison) && sfc.Source.Equals(thisSfc.Source, SpecialFeatureCodeDefinitionExtensions.comparison) && sfc.ID != thisSfc.ID && sfc.IsActive));
    }
  }
}
