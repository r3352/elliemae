// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.SessionStartupInfoETagsProvider
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.Cache;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public class SessionStartupInfoETagsProvider : MultiPropertiesETagsProvider<SessionStartupInfo>
  {
    public SessionStartupInfoETagsProvider()
    {
      this.Add<BizRuleInfo[]>("ActiveRules", (Func<SessionStartupInfo, BizRuleInfo[]>) (info => info.ActiveRules), (Action<SessionStartupInfo, BizRuleInfo[]>) ((info, value) => info.ActiveRules = value));
    }
  }
}
