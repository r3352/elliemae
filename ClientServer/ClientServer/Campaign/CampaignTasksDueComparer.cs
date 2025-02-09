// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Campaign.CampaignTasksDueComparer
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Campaign
{
  public class CampaignTasksDueComparer : IComparer<CampaignTasksDueInfo>
  {
    public int Compare(CampaignTasksDueInfo x, CampaignTasksDueInfo y)
    {
      return new CaseInsensitiveComparer().Compare((object) y.TasksDueCount, (object) x.TasksDueCount);
    }
  }
}
