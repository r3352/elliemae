// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.TradeManagement.ICorrespondentMasterService
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Collections.Generic;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.TradeManagement
{
  /// <summary>ICorrespondentMasterService Interface</summary>
  public interface ICorrespondentMasterService
  {
    /// <summary>
    /// Gets Correspondent Master by Corrsespondent Master Number.
    /// </summary>
    /// <param name="number">Correspondent Master Number</param>
    /// <remarks>Returns single CorrespondentMaster identified by Master Number</remarks>
    CorrespondentMaster GetCorrespondentMasterByNumber(string number);

    /// <summary>Gets Correspondent Masters by TPO Id.</summary>
    /// <param name="tpoId">TPO Id</param>
    /// <remarks>Returns list of Correspondent Masters objects associated to TPO by TPO Id</remarks>
    CorrespondentMaster[] GetCorrespondentMastersByTPOId(string tpoId);

    /// <summary>Gets Correspondent Masters by Organization Id.</summary>
    /// <param name="orgId">TPO Organization Id</param>
    /// <remarks>Returns list of Correspondent Masters objects associated to TPO by Organization Id</remarks>
    CorrespondentMaster[] GetCorrespondentMastersByOrganizationId(string orgId);

    /// <summary>
    /// Get a list of <see cref="T:EllieMae.Encompass.BusinessObjects.TradeManagement.CorrespondentMaster" /> by trade status
    /// </summary>
    /// <param name="tradeStatus"><see cref="T:EllieMae.Encompass.BusinessObjects.TradeManagement.MasterCommitmentStatus" />List</param>
    /// <returns>A list of <see cref="T:EllieMae.Encompass.BusinessObjects.TradeManagement.CorrespondentMaster" /></returns>
    /// <remarks>
    /// Example 1: Get all active Correspondent Masters
    ///                  MasterCommitmentStatus[] statusList = new MasterCommitmentStatus[1] { MasterCommitmentStatus.Active };
    ///                   EllieMae.Encompass.Client.Session session = Session["EncompassSession"] as EllieMae.Encompass.Client.Session;
    ///                  var ActiveCorrespondentMasters = session.CorrespondentMasterService.GetCorrespondentMasters(statusList);
    /// Example 2: Get all archived Correspondent Masters
    ///                  TradeStatus[] statusList = new MasterCommitmentStatus[1] { MasterCommitmentStatus.Archived };
    ///                   EllieMae.Encompass.Client.Session session = Session["EncompassSession"] as EllieMae.Encompass.Client.Session;
    ///                  var ArchivedCorrespondentMasters= session.CorrespondentMasterService.GetCorrespondentMasters(statusList);
    /// Example 3: Get all Correspondent Masters
    ///                  TradeStatus[] statusList = new TradeStatus[2] { MasterCommitmentStatus.Active, MasterCommitmentStatus.Archived };
    ///                   EllieMae.Encompass.Client.Session session = Session["EncompassSession"] as EllieMae.Encompass.Client.Session;
    ///                  var AllCorrespondentMasters = session.CorrespondentMasterService.GetCorrespondentMasters(statusList);
    /// </remarks>
    List<CorrespondentMaster> GetCorrespondentMasters(MasterCommitmentStatus[] tradeStatus);
  }
}
