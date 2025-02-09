// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.MasterContractSummaryInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  [Serializable]
  public class MasterContractSummaryInfo : IPropertyDictionary
  {
    private Hashtable data;

    public MasterContractSummaryInfo() => this.data = new Hashtable();

    public MasterContractSummaryInfo(
      int contractId,
      string contractNumber,
      int status,
      string investorName,
      string investorContractNumber,
      MasterContractTerm term,
      Decimal contractAmount,
      Decimal tolerance,
      DateTime startDate,
      DateTime endDate,
      int tradeCount,
      Decimal assignedAmount,
      Decimal totalProfit,
      Decimal completionPercent)
    {
      this.data = new Hashtable();
      this.ContractID = contractId;
      this.ContractNumber = contractNumber;
      this.Status = status;
      this.InvestorName = investorName;
      this.InvestorContractNumber = investorContractNumber;
      this.ContractAmount = contractAmount;
      this.Tolerance = tolerance;
      this.StartDate = startDate;
      this.EndDate = endDate;
      this.TradeCount = tradeCount;
      this.AssignedAmount = assignedAmount;
      this.TotalProfit = totalProfit;
      this.Term = term;
      this.CompletionPercent = completionPercent;
    }

    protected MasterContractSummaryInfo(MasterContractInfo source)
    {
      this.data = new Hashtable();
      this.InvestorName = source.InvestorName;
      this.ContractAmount = source.ContractAmount;
      this.Tolerance = source.Tolerance;
      this.Term = source.Term;
    }

    public MasterContractSummaryInfo(Hashtable data) => this.data = data;

    public int ContractID
    {
      get
      {
        return this.getField("MasterContracts.ContractID") == null ? -1 : (int) this.getField("MasterContracts.ContractID");
      }
      set => this.setField("MasterContracts.ContractID", (object) value);
    }

    public string ContractNumber
    {
      get
      {
        return this.getField("MasterContracts.ContractNumber") == null ? "" : (string) this.getField("MasterContracts.ContractNumber");
      }
      set => this.setField("MasterContracts.ContractNumber", (object) value);
    }

    public int Status
    {
      get
      {
        return this.getField("MasterContracts.Status") == null ? 0 : (int) this.getField("MasterContracts.Status");
      }
      set => this.setField("MasterContracts.Status", (object) value);
    }

    public MasterContractStatus MasterContractStatus => (MasterContractStatus) this.Status;

    public virtual string InvestorName
    {
      get
      {
        return this.getField("MasterContracts.InvestorName") == null ? "" : (string) this.getField("MasterContracts.InvestorName");
      }
      set => this.setField("MasterContracts.InvestorName", (object) value);
    }

    public string InvestorContractNumber
    {
      get
      {
        return this.getField("MasterContracts.InvestorContractNum") == null ? "" : (string) this.getField("MasterContracts.InvestorContractNum");
      }
      set => this.setField("MasterContracts.InvestorContractNum", (object) value);
    }

    public Decimal ContractAmount
    {
      get
      {
        return this.getField("MasterContracts.ContractAmount") == null ? 0M : (Decimal) this.getField("MasterContracts.ContractAmount");
      }
      set => this.setField("MasterContracts.ContractAmount", (object) value);
    }

    public Decimal Tolerance
    {
      get
      {
        return this.getField("MasterContracts.Tolerance") == null ? 0M : (Decimal) this.getField("MasterContracts.Tolerance");
      }
      set => this.setField("MasterContracts.Tolerance", (object) value);
    }

    public DateTime StartDate
    {
      get
      {
        return this.getField("MasterContracts.StartDate") == null ? DateTime.MinValue : (DateTime) this.getField("MasterContracts.StartDate");
      }
      set => this.setField("MasterContracts.StartDate", (object) value);
    }

    public DateTime EndDate
    {
      get
      {
        return this.getField("MasterContracts.EndDate") == null ? DateTime.MinValue : (DateTime) this.getField("MasterContracts.EndDate");
      }
      set => this.setField("MasterContracts.EndDate", (object) value);
    }

    public MasterContractTerm Term
    {
      get
      {
        return this.getField("MasterContracts.Term") == null ? MasterContractTerm.None : (MasterContractTerm) this.getField("MasterContracts.Term");
      }
      set => this.setField("MasterContracts.Term", (object) value);
    }

    public int TradeCount
    {
      get
      {
        return this.getField("ContractTrades.TradeCount") == null ? 0 : (int) this.getField("ContractTrades.TradeCount");
      }
      set => this.setField("ContractTrades.TradeCount", (object) value);
    }

    public Decimal AssignedAmount
    {
      get
      {
        return this.getField("ContractTrades.AssignedAmount") == null ? 0M : (Decimal) this.getField("ContractTrades.AssignedAmount");
      }
      set => this.setField("ContractTrades.AssignedAmount", (object) value);
    }

    public Decimal TotalProfit
    {
      get
      {
        return this.getField("ContractTrades.TotalProfit") == null ? 0M : (Decimal) this.getField("ContractTrades.TotalProfit");
      }
      set => this.setField("ContractTrades.TotalProfit", (object) value);
    }

    public Decimal CompletionPercent
    {
      get
      {
        return this.getField("ContractTrades.CompletionPercent") == null ? 0M : (Decimal) this.getField("ContractTrades.CompletionPercent");
      }
      set => this.setField("ContractTrades.CompletionPercent", (object) value);
    }

    public override string ToString() => this.ContractNumber;

    private object getField(string fieldName)
    {
      if (this.data.ContainsKey((object) fieldName))
        return this.data[(object) fieldName];
      if (this.data.ContainsKey((object) fieldName.Replace(" ", "")))
        return this.data[(object) fieldName.Replace(" ", "")];
      if (this.data.ContainsKey((object) ("MasterContracts." + fieldName)))
        return this.data[(object) ("MasterContracts." + fieldName)];
      return this.data.ContainsKey((object) ("ContractTrades." + fieldName)) ? this.data[(object) ("ContractTrades." + fieldName)] : (object) null;
    }

    private void setField(string fieldName, object value)
    {
      if (this.data.ContainsKey((object) fieldName))
        this.data[(object) fieldName] = value;
      else if (this.data.ContainsKey((object) ("MasterContracts." + fieldName)))
        this.data[(object) ("MasterContracts." + fieldName)] = value;
      else if (this.data.ContainsKey((object) ("ContractTrades." + fieldName)))
        this.data[(object) ("ContractTrades." + fieldName)] = value;
      else
        this.data.Add((object) fieldName, value);
    }

    public object this[string propertyName]
    {
      get => this.getField(propertyName);
      set => throw new NotImplementedException();
    }
  }
}
