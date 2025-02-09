// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.FannieMaePEMbsPoolAssignmentListScreen
// Assembly: TradeManagement, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 30F9401A-5385-498E-9C5B-D147722AC94C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\TradeManagement.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common.UI;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  internal class FannieMaePEMbsPoolAssignmentListScreen : MbsPoolAssignmentListScreen
  {
    public FannieMaePEMbsPoolAssignmentListScreen(ITradeEditorBase tradeEditor)
      : base(tradeEditor)
    {
      this.init();
    }

    public FannieMaePEMbsPoolAssignmentListScreen(ITradeEditorBase tradeEditor, bool useByFlag)
      : base(tradeEditor, useByFlag)
    {
      this.init();
    }

    private void init()
    {
      this.PoolName = "Fannie Mae PE MBS Pool";
      this.TableLayoutFileName = "FannieMaePeMbsPoolListSmallScreenView";
      this.TradeType = "commitment";
    }

    protected override ReportFieldDefs getFieldDefs()
    {
      MbsPoolReportFieldDefs fieldDefs1 = MbsPoolReportFieldDefs.GetFieldDefs();
      MbsPoolReportFieldDefs fieldDefs2 = new MbsPoolReportFieldDefs();
      foreach (ReportFieldDef fieldDef in (ReportFieldDefContainer) fieldDefs1)
      {
        if (!fieldDef.FieldID.Equals("ContractNumber"))
          fieldDefs2.Add(fieldDef);
      }
      return (ReportFieldDefs) fieldDefs2;
    }

    protected override TableLayout getDemoTableLayout()
    {
      TableLayout demoTableLayout = new TableLayout();
      demoTableLayout.AddColumn(new TableLayout.Column("MbsPoolDetails.CommitmentDate", "Commitment Date", HorizontalAlignment.Left, 100));
      demoTableLayout.AddColumn(new TableLayout.Column("MbsPoolDetails.Name", "Pool ID", HorizontalAlignment.Left, 120));
      demoTableLayout.AddColumn(new TableLayout.Column("MbsPoolDetails.PoolNumber", "Pool Number", HorizontalAlignment.Left, 110));
      demoTableLayout.AddColumn(new TableLayout.Column("MbsPoolDetails.TradeAmount", "Pool Amount", HorizontalAlignment.Right, 100));
      demoTableLayout.AddColumn(new TableLayout.Column("MbsPoolDetails.AmortizationType", "Amortization Type", HorizontalAlignment.Left, 120));
      demoTableLayout.AddColumn(new TableLayout.Column("MbsPoolDetails.Term", "Term", HorizontalAlignment.Left, 90));
      demoTableLayout.AddColumn(new TableLayout.Column("TradeMbsPoolSummary.TotalAmount", "Assigned Amount", HorizontalAlignment.Right, 110));
      return demoTableLayout;
    }
  }
}
