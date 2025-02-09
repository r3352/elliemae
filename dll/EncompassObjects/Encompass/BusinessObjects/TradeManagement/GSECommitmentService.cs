// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.TradeManagement.GSECommitmentService
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.Trading;
using EllieMae.Encompass.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.TradeManagement
{
  [Guid("9a3b50c5-2dd0-4de3-b6d3-7e904806bfc3")]
  public class GSECommitmentService : SessionBoundObject, IGSECommitmentService
  {
    internal GSECommitmentService(Session session)
      : base(session)
    {
    }

    public List<GSECommitment> GetGSECommitments(GSECommitmentStatus[] statusList)
    {
      List<GSECommitment> gseCommitments = new List<GSECommitment>();
      IGseCommitmentManager commitmentManager = (IGseCommitmentManager) this.Session.GetObject("GseCommitmentManager");
      QueryCriterion queryCriterion = (QueryCriterion) null;
      if (!((IEnumerable<GSECommitmentStatus>) statusList).Any<GSECommitmentStatus>((Func<GSECommitmentStatus, bool>) (a => a == GSECommitmentStatus.None)))
      {
        int[] numArray = new int[statusList.Length];
        for (int index = 0; index < statusList.Length; ++index)
          numArray[index] = (int) statusList[index];
        queryCriterion = (QueryCriterion) new ListValueCriterion("GseCommitmentDetails.Status", (Array) numArray, true);
      }
      ICursor icursor = commitmentManager.OpenTradeCursor(new QueryCriterion[1]
      {
        queryCriterion
      }, (SortField[]) null, (string[]) null, true, false);
      if (icursor == null || icursor.GetItemCount() <= 0)
        return gseCommitments;
      foreach (GSECommitmentViewModel info in (GSECommitmentViewModel[]) icursor.GetItems(0, icursor.GetItemCount()))
        gseCommitments.Add(new GSECommitment(info));
      return gseCommitments;
    }
  }
}
