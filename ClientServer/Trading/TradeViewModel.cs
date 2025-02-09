// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.TradeViewModel
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
  public class TradeViewModel : TradeBase, IPropertyDictionary
  {
    private Hashtable dataTable = new Hashtable();
    protected const Decimal DEFAULT_DECIMAL = 0M;
    protected const string DEFAULT_STRING = "�";
    protected const int DEFAULT_INT = -1;
    protected readonly DateTime DEFAULT_DATETIME;
    protected const bool DEFAULT_BOOL = false;

    public TradeViewModel() => this.DEFAULT_DATETIME = DateTime.MinValue;

    public TradeViewModel(int tradeID, string guid, string name, Decimal pairOffAmount)
    {
      this.TradeID = tradeID;
      this.Guid = guid;
      this.Name = name;
      this.PairOffAmount = pairOffAmount;
      this.DEFAULT_DATETIME = DateTime.MinValue;
    }

    protected virtual string dataTableName => "Trades";

    protected void setField(string fieldName, object value)
    {
      if (this.dataTable.ContainsKey((object) fieldName))
        this.dataTable[(object) fieldName] = value;
      else if (this.dataTable.ContainsKey((object) (this.dataTableName + "." + fieldName)))
        this.dataTable[(object) (this.dataTableName + "." + fieldName)] = value;
      else
        this.dataTable.Add((object) fieldName, value);
    }

    protected object getField(string fieldName)
    {
      if (this.dataTable.ContainsKey((object) fieldName))
        return this.dataTable[(object) fieldName];
      if (this.dataTable.ContainsKey((object) fieldName.Replace(" ", "")))
        return this.dataTable[(object) fieldName.Replace(" ", "")];
      return this.dataTable.ContainsKey((object) (this.dataTableName + "." + fieldName)) ? this.dataTable[(object) (this.dataTableName + "." + fieldName)] : (object) null;
    }

    protected object getField(string fieldName, object defaultValue)
    {
      return this.getField(fieldName) ?? defaultValue;
    }

    public override int TradeID
    {
      get
      {
        object field = this.getField(this.dataTableName + ".TradeID");
        return field != null ? (int) field : -1;
      }
      set => this.setField(this.dataTableName + ".TradeID", (object) value);
    }

    public override string Guid
    {
      get => (string) this.getField(this.dataTableName + ".Guid") ?? "";
      set => this.setField(this.dataTableName + ".Guid", (object) value);
    }

    public override string Name
    {
      get => (string) this.getField(this.dataTableName + ".Name") ?? "";
      set => this.setField(this.dataTableName + ".Name", (object) value);
    }

    public virtual Decimal PairOffAmount
    {
      get
      {
        return this.getField(this.dataTableName + ".PairOffAmount") == null ? 0M : (Decimal) this.getField(this.dataTableName + ".PairOffAmount");
      }
      set => this.setField(this.dataTableName + ".PairOffAmount", (object) value);
    }

    public virtual string InvestorName
    {
      get => this.getField(this.dataTableName + ".InvestorName", (object) "") as string;
      set => this.setField(this.dataTableName + ".InvestorName", (object) value);
    }

    public virtual TradeStatus Status
    {
      get => (TradeStatus) this.getField(this.dataTableName + ".Status", (object) TradeStatus.None);
      set => this.setField(this.dataTableName + ".Status", (object) value);
    }

    public virtual DateTime TargetDeliveryDate
    {
      get
      {
        return (DateTime) this.getField(this.dataTableName + ".TargetDeliveryDate", (object) this.DEFAULT_DATETIME);
      }
      set => this.setField(this.dataTableName + ".TargetDeliveryDate", (object) value);
    }

    public virtual Decimal TradeAmount
    {
      get => (Decimal) this.getField(this.dataTableName + ".TradeAmount", (object) 0M);
      set => this.setField(this.dataTableName + ".TradeAmount", (object) value);
    }

    public virtual Decimal AssignedAmount
    {
      get => (Decimal) this.getField(this.dataTableName + ".TotalAmount", (object) 0M);
      set => this.setField("TradeMbsPoolSummary.TotalAmount", (object) value);
    }

    public virtual Decimal CompletionPercent
    {
      get => (Decimal) this.getField(this.dataTableName + ".CompletionPercent", (object) 0M);
      set => this.setField("TradeMbsPoolSummary.CompletionPercent", (object) value);
    }

    public virtual int ContractID
    {
      get => (int) this.getField(this.dataTableName + ".ContractID", (object) -1);
      set => this.setField(this.dataTableName + ".ContractID", (object) value);
    }

    public virtual bool Locked
    {
      get
      {
        return this.Status == TradeStatus.Archived || (bool) this.getField(this.dataTableName + ".Locked", (object) false);
      }
      set => this.setField(this.dataTableName + ".Locked", (object) value);
    }

    public virtual object this[string propertyName]
    {
      get => this.getField(propertyName);
      set => throw new NotImplementedException();
    }
  }
}
