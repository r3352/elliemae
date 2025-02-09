// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.Cursors.TradeLoanCursor
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
  public class TradeLoanCursor : CursorBase
  {
    private const string className = "TradeLoanCursor";
    private LoanTradeInfo[] trades;
    private string[] fields;
    private PipelineData dataToInclude;

    public TradeLoanCursor Initialize(
      ISession session,
      int[] tradeIds,
      string[] fields,
      PipelineData dataToInclude,
      SortField[] sortOrder,
      bool isExternalOrganization,
      EligibleLoanFilterOption filterOption)
    {
      return this.Initialize(session, LoanTrades.GetTrades(tradeIds), fields, dataToInclude, sortOrder, isExternalOrganization, filterOption);
    }

    public TradeLoanCursor Initialize(
      ISession session,
      LoanTradeInfo[] trades,
      string[] fields,
      PipelineData dataToInclude,
      SortField[] sortOrder,
      bool isExternalOrganization,
      EligibleLoanFilterOption filterOption)
    {
      return this.Initialize(session, trades, fields, dataToInclude, sortOrder, (string[]) null, isExternalOrganization, filterOption);
    }

    public TradeLoanCursor Initialize(
      ISession session,
      LoanTradeInfo[] trades,
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
      PipelineInfo[] pinfos = LoanTrades.GetEligibleLoans(session.GetUserInfo(), trades, new string[0], sortOrder, isExternalOrganization, filterOption);
      if (guidsToOmit != null && guidsToOmit.Length != 0)
        pinfos = this.removeItemsFromList(pinfos, guidsToOmit);
      for (int index = 0; index < pinfos.Length; ++index)
        this.Items.Add((object) pinfos[index].GUID);
      return this;
    }

    public override object[] GetItems(int startIndex, int count, bool isExternalOrganization)
    {
      TraceLog.WriteApi(nameof (TradeLoanCursor), nameof (GetItems), (object) startIndex, (object) count);
      string[] array = (string[]) this.Items.GetRange(startIndex, count).ToArray(typeof (string));
      PipelineInfo[] source1 = Pipeline.Generate(this.Session.GetUserInfo(), array, this.fields, this.dataToInclude, isExternalOrganization, tradeType: TradeType.LoanTrade);
      PipelineInfo[] source2 = new PipelineInfo[source1.Length];
      int index1 = 0;
      foreach (string str in array)
      {
        string guid = str;
        if (!((IEnumerable<PipelineInfo>) source2).Any<PipelineInfo>((Func<PipelineInfo, bool>) (p => p != null && string.Equals(p.Info[(object) "Guid"] as string, guid))) && ((IEnumerable<PipelineInfo>) source1).Any<PipelineInfo>((Func<PipelineInfo, bool>) (p => string.Equals(p.Info[(object) "Guid"] as string, guid))))
        {
          source2[index1] = ((IEnumerable<PipelineInfo>) source1).Where<PipelineInfo>((Func<PipelineInfo, bool>) (p => string.Equals(p.Info[(object) "Guid"] as string, guid))).First<PipelineInfo>();
          ++index1;
        }
      }
      Dictionary<int, SecurityTradeInfo> secTrades = new Dictionary<int, SecurityTradeInfo>();
      for (int index2 = 0; index2 < this.trades.Length; ++index2)
      {
        if (this.trades[index2].SecurityTradeID > 0)
          secTrades.Add(this.trades[index2].TradeID, SecurityTrades.GetTrade(this.trades[index2].SecurityTradeID));
      }
      foreach (PipelineInfo pinfo in source2)
        LoanTrades.PopulateTradeProfitData(pinfo, this.trades, secTrades);
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
