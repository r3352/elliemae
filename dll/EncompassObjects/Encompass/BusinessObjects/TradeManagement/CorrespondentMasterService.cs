// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.TradeManagement.CorrespondentMasterService
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

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.TradeManagement
{
  public class CorrespondentMasterService : SessionBoundObject, ICorrespondentMasterService
  {
    private ICorrespondentMasterManager cmmgr;

    public CorrespondentMasterService(Session session)
      : base(session)
    {
      this.cmmgr = (ICorrespondentMasterManager) this.Session.GetObject("CorrespondentMasterManager");
    }

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

    public CorrespondentMaster[] GetCorrespondentMastersByTPOId(string tpoId)
    {
      CorrespondentMasterInfo[] correspondentMastersByTpoid = this.cmmgr.GetCorrespondentMastersByTPOID(tpoId);
      if (correspondentMastersByTpoid == null)
        return (CorrespondentMaster[]) null;
      List<CorrespondentMaster> sdkObject = this.MapModelToSDKObject(correspondentMastersByTpoid);
      return sdkObject.Count > 0 ? sdkObject.ToArray() : (CorrespondentMaster[]) null;
    }

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
        commitmentAmount = ((MasterCommitmentBase) cmInfo).CommitmentAmount,
        CommitmentType = (MasterCommitmentType) cmInfo.CommitmentType,
        CompanyName = cmInfo.CompanyName,
        TPOId = cmInfo.TpoId,
        OrganizationId = cmInfo.OrganizationId,
        EffectiveDate = cmInfo.MasterEffectiveDateTime,
        expireDate = cmInfo.MasterExpirationDateTime,
        MasterCommitmentNumber = ((TradeBase) cmInfo).Name,
        PriceGroup = cmInfo.RateSheet,
        Status = (MasterCommitmentStatus) ((MasterCommitmentBase) cmInfo).Status,
        DeliveryTypes = ((MasterCommitmentBase) cmInfo).DeliveryInfos.Select<MasterCommitmentDeliveryInfo, CorrespondentMasterDelivery>((Func<MasterCommitmentDeliveryInfo, CorrespondentMasterDelivery>) (d => new CorrespondentMasterDelivery()
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

    public List<CorrespondentMaster> GetCorrespondentMasters(MasterCommitmentStatus[] tradeStatus)
    {
      List<CorrespondentMaster> correspondentMasterList = new List<CorrespondentMaster>();
      ICorrespondentMasterManager icorrespondentMasterManager = (ICorrespondentMasterManager) this.Session.GetObject("CorrespondentMasterManager");
      int[] numArray = new int[tradeStatus.Length];
      for (int index = 0; index < tradeStatus.Length; ++index)
        numArray[index] = (int) tradeStatus[index];
      QueryCriterion queryCriterion = (QueryCriterion) new ListValueCriterion("CorrespondentMaster.Status", (Array) numArray, true);
      ICursor icursor = icorrespondentMasterManager.OpenCorrespondentMasterCursor(new QueryCriterion[1]
      {
        queryCriterion
      }, (SortField[]) null, false);
      return icursor != null && icursor.GetItemCount() > 0 ? this.MapModelToSDKObject((CorrespondentMasterViewModel[]) icursor.GetItems(0, icursor.GetItemCount())) : correspondentMasterList;
    }
  }
}
