// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Bpm.FoundSearchFieldRules
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer.Bpm;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.Bpm
{
  public class FoundSearchFieldRules
  {
    public FoundSearchFieldRules() => this.Rules = new List<FieldSearchRule>();

    public FoundSearchFieldRules(List<FieldSearchRule> rules, int totalCount)
    {
      this.Rules = rules;
      this.TotalCount = totalCount;
    }

    public List<FieldSearchRule> Rules { get; set; }

    public int TotalCount { get; set; }
  }
}
