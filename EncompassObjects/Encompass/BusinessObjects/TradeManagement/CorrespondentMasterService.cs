// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.TradeManagement.CorrespondentMasterService
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

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.TradeManagement
{
  /// <summary>CorrespondentMasterService Class</summary>
  public class CorrespondentMasterService : SessionBoundObject, ICorrespondentMasterService
  {
    private ICorrespondentMasterManager cmmgr;

    /// <summary>CorrespondentMasterService Constructor</summary>
    /// <param name="session"></param>
    public CorrespondentMasterService(Session session)
      : base(session)
    {
      this.cmmgr = (ICorrespondentMasterManager) this.Session.GetObject("CorrespondentMasterManager");
    }

    /// <summary>
    /// Gets Correspondent Master by Corrsespondent Master Number.
    /// </summary>
    /// <param name="number">Correspondent Master Number</param>
    /// <remarks>Returns single CorrespondentMaster identified by Master Number</remarks>
    public CorrespondentMaster GetCorrespondentMasterByNumber(string number)
    {
      CorrespondentMasterInfo byContractNumber = this.cmmgr.GetCorrespondentMastersByContractNumber(number);
      if (byContractNumber == null)
        return (CorrespondentMaster) null;
      List<CorrespondentMaster> sdkObject = this.MapModelToSDKObject(new CorrespondentMasterInfo[1]
      {
        byContractNumber
      });
      return sdkObject.Count > 0 ? sdkObject[0] : (CorrespondentMaster) null;
    }

    /// <summary>Gets Correspondent Masters by TPO Id.</summary>
    /// <param name="tpoId">TPO Id</param>
    /// <remarks>Returns list of Correspondent Masters objects associated to TPO by TPO Id</remarks>
    public CorrespondentMaster[] GetCorrespondentMastersByTPOId(string tpoId)
    {
      CorrespondentMasterInfo[] correspondentMastersByTpoid = this.cmmgr.GetCorrespondentMastersByTPOID(tpoId);
      if (correspondentMastersByTpoid == null)
        return (CorrespondentMaster[]) null;
      List<CorrespondentMaster> sdkObject = this.MapModelToSDKObject(correspondentMastersByTpoid);
      return sdkObject.Count > 0 ? sdkObject.ToArray() : (CorrespondentMaster[]) null;
    }

    /// <summary>Gets Correspondent Masters by Organization Id.</summary>
    /// <param name="orgId">TPO Organization Id</param>
    /// <remarks>Returns list of Correspondent Masters objects associated to TPO by Organization Id</remarks>
    public CorrespondentMaster[] GetCorrespondentMastersByOrganizationId(string orgId)
    {
      CorrespondentMasterInfo[] byOrganizationId = this.cmmgr.GetCorrespondentMastersByOrganizationID(orgId);
      if (byOrganizationId == null)
        return (CorrespondentMaster[]) null;
      List<CorrespondentMaster> sdkObject = this.MapModelToSDKObject(byOrganizationId);
      return sdkObject.Count > 0 ? sdkObject.ToArray() : (CorrespondentMaster[]) null;
    }

    private List<CorrespondentMaster> MapModelToSDKObject(CorrespondentMasterInfo[] infos)
    {
      return ((IEnumerable<CorrespondentMasterInfo>) infos).Select<CorrespondentMasterInfo, CorrespondentMaster>((Func<CorrespondentMasterInfo, CorrespondentMaster>) (cmInfo => new CorrespondentMaster()
      {
        AvailableAmount = cmInfo.AvailableAmount,
        commitmentAmount = cmInfo.CommitmentAmount,
        CommitmentType = (MasterCommitmentType) cmInfo.CommitmentType,
        CompanyName = cmInfo.CompanyName,
        TPOId = cmInfo.TpoId,
        OrganizationId = cmInfo.OrganizationId,
        EffectiveDate = cmInfo.MasterEffectiveDateTime,
        expireDate = cmInfo.MasterExpirationDateTime,
        MasterCommitmentNumber = cmInfo.Name,
        PriceGroup = cmInfo.RateSheet,
        Status = (MasterCommitmentStatus) cmInfo.Status,
        DeliveryTypes = cmInfo.DeliveryInfos.Select<MasterCommitmentDeliveryInfo, CorrespondentMasterDelivery>((Func<MasterCommitmentDeliveryInfo, CorrespondentMasterDelivery>) (d => new CorrespondentMasterDelivery()
        {
          DeliveryDays = d.DeliveryDays,
          DeliveryType = (CorrespondentMasterDeliveryType) d.Type,
          EffectiveDate = d.EffectiveDateTime,
          ExpireDate = d.ExpireDateTime,
          Tolerance = d.Tolerance
        })).ToList<CorrespondentMasterDelivery>()
      })).ToList<CorrespondentMaster>();
    }

    private List<CorrespondentMaster> MapModelToSDKObject(CorrespondentMasterViewModel[] infos)
    {
      IEnumerable<CorrespondentMaster> correspondentMasters = ((IEnumerable<CorrespondentMasterViewModel>) infos).Select<CorrespondentMasterViewModel, CorrespondentMaster>((Func<CorrespondentMasterViewModel, CorrespondentMaster>) (cmInfo => new CorrespondentMaster()
      {
        AvailableAmount = cmInfo.AvailableAmount,
        commitmentAmount = cmInfo.CommitmentAmount,
        CommitmentType = (MasterCommitmentType) Enum.Parse(typeof (MasterCommitmentType), cmInfo.CommitmentType, true),
        CompanyName = cmInfo.CompanyName,
        TPOId = cmInfo.TpoId,
        OrganizationId = cmInfo.OrganizationId,
        EffectiveDate = cmInfo.MasterEffectiveDate,
        expireDate = cmInfo.MasterExpirationDate,
        MasterCommitmentNumber = cmInfo.ContractNumber,
        PriceGroup = cmInfo.RateSheet,
        Status = (MasterCommitmentStatus) cmInfo.Status
      }));
      List<CorrespondentMaster> source = new List<CorrespondentMaster>();
      foreach (CorrespondentMaster correspondentMaster in correspondentMasters)
      {
        CorrespondentMaster cmc = correspondentMaster;
        if (!source.Any<CorrespondentMaster>((Func<CorrespondentMaster, bool>) (c => c.MasterCommitmentNumber == cmc.MasterCommitmentNumber)))
          source.Add(cmc);
      }
      foreach (CorrespondentMaster correspondentMaster in source)
      {
        CorrespondentMaster cmc = correspondentMaster;
        cmc.DeliveryTypes = ((IEnumerable<CorrespondentMasterViewModel>) infos).Where<CorrespondentMasterViewModel>((Func<CorrespondentMasterViewModel, bool>) (c => c.ContractNumber == cmc.MasterCommitmentNumber)).Select<CorrespondentMasterViewModel, CorrespondentMasterDelivery>((Func<CorrespondentMasterViewModel, CorrespondentMasterDelivery>) (c => new CorrespondentMasterDelivery()
        {
          DeliveryDays = c.DeliveryDays,
          DeliveryType = (CorrespondentMasterDeliveryType) Enum.Parse(typeof (CorrespondentMasterDeliveryType), c.DeliveryType, true),
          EffectiveDate = c.EffectiveDate,
          ExpireDate = c.ExpirationDate,
          Tolerance = c.Tolerance
        })).ToList<CorrespondentMasterDelivery>();
      }
      return source;
    }

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
    public List<CorrespondentMaster> GetCorrespondentMasters(MasterCommitmentStatus[] tradeStatus)
    {
      List<CorrespondentMaster> correspondentMasterList = new List<CorrespondentMaster>();
      ICorrespondentMasterManager correspondentMasterManager = (ICorrespondentMasterManager) this.Session.GetObject("CorrespondentMasterManager");
      int[] valueList = new int[tradeStatus.Length];
      for (int index = 0; index < tradeStatus.Length; ++index)
        valueList[index] = (int) tradeStatus[index];
      QueryCriterion queryCriterion = (QueryCriterion) new ListValueCriterion("CorrespondentMaster.Status", (Array) valueList, true);
      ICursor cursor = correspondentMasterManager.OpenCorrespondentMasterCursor(new QueryCriterion[1]
      {
        queryCriterion
      }, (SortField[]) null, false);
      return cursor != null && cursor.GetItemCount() > 0 ? this.MapModelToSDKObject((CorrespondentMasterViewModel[]) cursor.GetItems(0, cursor.GetItemCount())) : correspondentMasterList;
    }
  }
}
