// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.Cursors.MbsPoolLoanCursor
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.Server;
using EllieMae.EMLite.Trading;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Elli.Server.Remoting.Cursors
{
  public class MbsPoolLoanCursor : CursorBase
  {
    private const string className = "MbsPoolLoanCursor";
    private MbsPoolInfo[] trades;
    private string[] fields;
    private PipelineData dataToInclude;

    public MbsPoolLoanCursor Initialize(
      ISession session,
      int[] tradeIds,
      string[] fields,
      PipelineData dataToInclude,
      SortField[] sortOrder,
      bool isExternalOrganization,
      EligibleLoanFilterOption filterOption)
    {
      return this.Initialize(session, MbsPools.GetTrades(tradeIds), fields, dataToInclude, sortOrder, isExternalOrganization, filterOption);
    }

    public MbsPoolLoanCursor Initialize(
      ISession session,
      MbsPoolInfo[] trades,
      string[] fields,
      PipelineData dataToInclude,
      SortField[] sortOrder,
      bool isExternalOrganization,
      EligibleLoanFilterOption filterOption)
    {
      return this.Initialize(session, trades, fields, dataToInclude, sortOrder, (string[]) null, isExternalOrganization, filterOption);
    }

    public MbsPoolLoanCursor Initialize(
      ISession session,
      MbsPoolInfo[] trades,
      string[] fields,
      PipelineData dataToInclude,
      SortField[] sortOrder,
      string[] guidsToOmit,
      bool isExternalOrganization,
      EligibleLoanFilterOption filterOption)
    {
      this.InitializeInternal(session);
      this.trades = trades;
      this.fields = fields;
      this.dataToInclude = dataToInclude;
      PipelineInfo[] pinfos = MbsPools.GetEligibleLoans(session.GetUserInfo(), trades, new string[0], sortOrder, isExternalOrganization, filterOption);
      if (guidsToOmit != null && guidsToOmit.Length != 0)
        pinfos = this.removeItemsFromList(pinfos, guidsToOmit);
      for (int index = 0; index < pinfos.Length; ++index)
        this.Items.Add((object) pinfos[index].GUID);
      return this;
    }

    public override object[] GetItems(int startIndex, int count, bool isExternalOrganization)
    {
      TraceLog.WriteApi(nameof (MbsPoolLoanCursor), nameof (GetItems), (object) startIndex, (object) count);
      string[] array = (string[]) this.Items.GetRange(startIndex, count).ToArray(typeof (string));
      PipelineInfo[] source1 = Pipeline.Generate(this.Session.GetUserInfo(), array, this.fields, this.dataToInclude, isExternalOrganization, tradeType: TradeType.MbsPool);
      PipelineInfo[] source2 = new PipelineInfo[source1.Length];
      int index = 0;
      foreach (string str in array)
      {
        string guid = str;
        if (!((IEnumerable<PipelineInfo>) source2).Any<PipelineInfo>((Func<PipelineInfo, bool>) (p => p != null && string.Equals(p.Info[(object) "Guid"] as string, guid))) && ((IEnumerable<PipelineInfo>) source1).Any<PipelineInfo>((Func<PipelineInfo, bool>) (p => string.Equals(p.Info[(object) "Guid"] as string, guid))))
        {
          source2[index] = ((IEnumerable<PipelineInfo>) source1).Where<PipelineInfo>((Func<PipelineInfo, bool>) (p => string.Equals(p.Info[(object) "Guid"] as string, guid))).First<PipelineInfo>();
          ++index;
        }
      }
      foreach (PipelineInfo pinfo in source2)
        MbsPools.PopulateTradeProfitData(pinfo, this.trades);
      return (object[]) source2;
    }

    public override object GetItem(int index, bool isExternalOrganization)
    {
      return this.GetItems(index, 1, isExternalOrganization)[0];
    }

    private PipelineInfo[] removeItemsFromList(PipelineInfo[] pinfos, string[] guidsToOmit)
    {
      Dictionary<string, object> dictionary = new Dictionary<string, object>();
      foreach (string key in guidsToOmit)
        dictionary[key] = (object) null;
      List<PipelineInfo> pipelineInfoList = new List<PipelineInfo>((IEnumerable<PipelineInfo>) pinfos);
      for (int index = pipelineInfoList.Count - 1; index >= 0; --index)
      {
        if (dictionary.ContainsKey(pipelineInfoList[index].GUID))
          pipelineInfoList.RemoveAt(index);
      }
      return pipelineInfoList.ToArray();
    }

    private class TradeLoanCursorItemComparer : IComparer
    {
      private SortField[] sortFields;

      public TradeLoanCursorItemComparer(SortField[] sortOrder) => this.sortFields = sortOrder;

      public int Compare(object a, object b)
      {
        PipelineInfo pinfo1 = (PipelineInfo) a;
        PipelineInfo pinfo2 = (PipelineInfo) b;
        for (int index = 0; index < this.sortFields.Length; ++index)
        {
          object sortValue1 = this.getSortValue(pinfo1, this.sortFields[index]);
          object sortValue2 = this.getSortValue(pinfo2, this.sortFields[index]);
          if (!object.Equals(sortValue1, sortValue2))
          {
            int num;
            if (sortValue1 == null)
              num = -1;
            else if (sortValue2 == null)
            {
              num = 1;
            }
            else
            {
              try
              {
                num = ((IComparable) sortValue1).CompareTo(sortValue2);
              }
              catch
              {
                num = 0;
              }
            }
            if (num != 0)
              return this.sortFields[index].SortOrder != FieldSortOrder.Ascending ? -num : num;
          }
        }
        return 0;
      }

      private object getSortValue(PipelineInfo pinfo, SortField field)
      {
        string key = "SortKey." + field.Term.FieldName;
        if (pinfo.Info.ContainsKey((object) key))
          return pinfo.Info[(object) key];
        object sortValue = pinfo.Info[(object) field.Term.FieldName];
        pinfo.Info[(object) key] = sortValue;
        return sortValue;
      }
    }
  }
}
