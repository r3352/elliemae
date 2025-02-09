// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.TradeManagement.GSECommitmentService
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

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
  /// <summary>GSECommitmentService class</summary>
  [Guid("9a3b50c5-2dd0-4de3-b6d3-7e904806bfc3")]
  public class GSECommitmentService : SessionBoundObject, IGSECommitmentService
  {
    internal GSECommitmentService(Session session)
      : base(session)
    {
    }

    /// <summary>
    /// Get a list of <see cref="T:EllieMae.Encompass.BusinessObjects.TradeManagement.GSECommitment" /> by trade status
    /// </summary>
    /// <param name="statusList"><see cref="T:EllieMae.Encompass.BusinessObjects.TradeManagement.GSECommitmentStatus" />List</param>
    /// <returns>A list of <see cref="T:EllieMae.Encompass.BusinessObjects.TradeManagement.GSECommitment" /></returns>
    /// <remarks>
    /// Example 1: Get all current GSE Commitments
    ///                  GSECommitmentStatus[] statusList = new GSECommitmentStatus[2] { GSECommitmentStatus.Open, GSECommitmentStatus.Committed};
    ///                   EllieMae.Encompass.Client.Session session = Session["EncompassSession"] as EllieMae.Encompass.Client.Session;
    ///                  var ActiveGSECommitments = session.GSECommitmentService.GetGSECommitments(statusList);
    /// Example 2: Get all Archived GSE Commitments
    ///                  GSECommitmentStatus[] statusList = new GSECommitmentStatus[1] { GSECommitmentStatus.Archived };
    ///                   EllieMae.Encompass.Client.Session session = Session["EncompassSession"] as EllieMae.Encompass.Client.Session;
    ///                  var ArchivedGSECommitments = session.GSECommitmentService.GetGSECommitments(statusList);
    /// Example 3: Get all GSE Commitments
    ///                  GSECommitmentStatus[] statusList = new GSECommitmentStatus[1] { GSECommitmentStatus.None };
    ///                   EllieMae.Encompass.Client.Session session = Session["EncompassSession"] as EllieMae.Encompass.Client.Session;
    ///                  var AllGSECommitments = session.GSECommitmentService.GetGSECommitments(statusList);
    /// </remarks>
    public List<GSECommitment> GetGSECommitments(GSECommitmentStatus[] statusList)
    {
      List<GSECommitment> gseCommitments = new List<GSECommitment>();
      IGseCommitmentManager commitmentManager = (IGseCommitmentManager) this.Session.GetObject("GseCommitmentManager");
      QueryCriterion queryCriterion = (QueryCriterion) null;
      if (!((IEnumerable<GSECommitmentStatus>) statusList).Any<GSECommitmentStatus>((Func<GSECommitmentStatus, bool>) (a => a == GSECommitmentStatus.None)))
      {
        int[] valueList = new int[statusList.Length];
        for (int index = 0; index < statusList.Length; ++index)
          valueList[index] = (int) statusList[index];
        queryCriterion = (QueryCriterion) new ListValueCriterion("GseCommitmentDetails.Status", (Array) valueList, true);
      }
      ICursor cursor = commitmentManager.OpenTradeCursor(new QueryCriterion[1]
      {
        queryCriterion
      }, (SortField[]) null, (string[]) null, true, false);
      if (cursor == null || cursor.GetItemCount() <= 0)
        return gseCommitments;
      foreach (GSECommitmentViewModel info in (GSECommitmentViewModel[]) cursor.GetItems(0, cursor.GetItemCount()))
        gseCommitments.Add(new GSECommitment(info));
      return gseCommitments;
    }
  }
}
