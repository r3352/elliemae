// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.TradeView
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Serialization;
using System;
using System.Collections;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class TradeView : BinaryConvertible<TradeView>, ITemplateSetting
  {
    private string name;
    private FieldFilterList filter;
    private TableLayout layout;
    private bool viewedStatus = true;
    public static string StandardViewName = "Standard View";

    public TradeView()
    {
    }

    public TradeView(string name) => this.name = name;

    public TradeView(XmlSerializationInfo info)
    {
      this.name = info.GetString(nameof (name));
      this.filter = (FieldFilterList) info.GetValue(nameof (filter), typeof (FieldFilterList));
      this.layout = (TableLayout) info.GetValue(nameof (layout), typeof (TableLayout));
      this.viewedStatus = info.GetBoolean("viewedstatus", true);
    }

    public string Name
    {
      get => this.name;
      set => this.name = value;
    }

    public FieldFilterList Filter
    {
      get => this.filter;
      set => this.filter = value;
    }

    public TableLayout Layout
    {
      get => this.layout;
      set => this.layout = value;
    }

    public bool ViewActive
    {
      get => this.viewedStatus;
      set => this.viewedStatus = value;
    }

    public bool Default => this.Name == TradeView.StandardViewName;

    public override string ToString() => this.Name;

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("name", (object) this.name);
      info.AddValue("viewedstatus", (object) this.viewedStatus);
      info.AddValue("filter", (object) this.filter);
      info.AddValue("layout", (object) this.layout);
    }

    public static explicit operator TradeView(BinaryObject binaryObject)
    {
      return BinaryConvertible<TradeView>.Parse(binaryObject);
    }

    public static TradeView CreateDefaultTradeView()
    {
      TradeView defaultTradeView = new TradeView(TradeView.StandardViewName);
      TableLayout tableLayout = new TableLayout();
      tableLayout.AddColumn(new TableLayout.Column("TradeLoanTradeSummary.PendingLoanCount", "Pending Loan", HorizontalAlignment.Left, 120));
      tableLayout.AddColumn(new TableLayout.Column("LoanTradeDetails.Locked", "Locked", HorizontalAlignment.Left, 70));
      tableLayout.AddColumn(new TableLayout.Column("LoanTradeDetails.Name", "Trade ID", HorizontalAlignment.Left, 120));
      tableLayout.AddColumn(new TableLayout.Column("LoanTradeDetails.InvestorName", "Investor", HorizontalAlignment.Left, 100));
      tableLayout.AddColumn(new TableLayout.Column("LoanTradeDetails.Status", "Status", HorizontalAlignment.Right, 110));
      tableLayout.AddColumn(new TableLayout.Column("LoanTradeDetails.TargetDeliveryDate", "Target Delivery Date", HorizontalAlignment.Left, 110));
      tableLayout.AddColumn(new TableLayout.Column("LoanTradeDetails.TradeAmount", "Trade Amount", HorizontalAlignment.Right, 90));
      tableLayout.AddColumn(new TableLayout.Column("LoanTradeDetails.MinAmount", "Minimum Amount", HorizontalAlignment.Right, 100));
      tableLayout.AddColumn(new TableLayout.Column("LoanTradeDetails.MaxAmount", "Maximum Amount", HorizontalAlignment.Right, 100));
      tableLayout.AddColumn(new TableLayout.Column("TradeLoanTradeSummary.TotalAmount", "Assigned Amount", HorizontalAlignment.Right, 100));
      tableLayout.AddColumn(new TableLayout.Column("TradeLoanTradeSummary.CompletionPercent", "Completion Percent", HorizontalAlignment.Left, 110));
      tableLayout.AddColumn(new TableLayout.Column("TradesMasterContract.ContractNumber", "Master #", HorizontalAlignment.Left, 110));
      defaultTradeView.Layout = tableLayout;
      return defaultTradeView;
    }

    public static TradeView CreateDefaultMasterContractView()
    {
      TradeView masterContractView = new TradeView(TradeView.StandardViewName);
      TableLayout tableLayout = new TableLayout();
      tableLayout.AddColumn(new TableLayout.Column("MasterContracts.ContractNumber", "Master Contract #", HorizontalAlignment.Left, 120));
      tableLayout.AddColumn(new TableLayout.Column("MasterContracts.InvestorName", "Investor Name", HorizontalAlignment.Left, 100));
      tableLayout.AddColumn(new TableLayout.Column("MasterContracts.ContractAmount", "Contract Amount", HorizontalAlignment.Right, 110));
      tableLayout.AddColumn(new TableLayout.Column("MasterContracts.EndDate", "End Date", HorizontalAlignment.Left, 90));
      tableLayout.AddColumn(new TableLayout.Column("ContractTrades.TradeCount", "# Trades/Pools", HorizontalAlignment.Right, 100));
      tableLayout.AddColumn(new TableLayout.Column("ContractTrades.AssignedAmount", "Total Assigned Amount", HorizontalAlignment.Right, 150));
      tableLayout.AddColumn(new TableLayout.Column("ContractTrades.CompletionPercent", "Completion Percent", HorizontalAlignment.Left, 110));
      masterContractView.Layout = tableLayout;
      return masterContractView;
    }

    string ITemplateSetting.TemplateName
    {
      get => this.name;
      set => this.name = value;
    }

    public string Description
    {
      get => "";
      set
      {
      }
    }

    public Hashtable GetProperties()
    {
      return new Hashtable((IEqualityComparer) StringComparer.CurrentCultureIgnoreCase);
    }

    public ITemplateSetting Duplicate() => (ITemplateSetting) this.Clone();
  }
}
