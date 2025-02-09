// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.VerificationTool
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class VerificationTool : UserControl, IOnlineHelpTarget, IRefreshContents
  {
    private Sessions.Session session;
    private LoanData loan;
    private VerificationDetailsControl timelineEmploymentControl;
    private VerificationDetailsControl timelineIncomeControl;
    private VerificationDetailsControl timelineAssetControl;
    private VerificationDetailsControl timelineMonthlyControl;
    private IContainer components;
    private Panel panelAll;
    private Panel panelTimelineIncome;
    private Panel panelDivider1;
    private Panel panelTimelineEmployment;
    private Panel panelDivider2;
    private Panel panelTimelineAsset;
    private Panel panelTimelineObligation;
    private Panel panelDivider3;

    public VerificationTool(Sessions.Session session, LoanData loan)
    {
      this.session = session;
      this.loan = loan;
      this.InitializeComponent();
      this.Dock = DockStyle.Fill;
      this.initForm();
    }

    private void initForm()
    {
      this.session.LoanDataMgr.LockLoanWithExclusiveA();
      this.timelineEmploymentControl = new VerificationDetailsControl(this.session, this.loan, VerificationTimelineType.Employment);
      this.panelTimelineEmployment.Controls.Add((Control) this.timelineEmploymentControl);
      this.timelineIncomeControl = new VerificationDetailsControl(this.session, this.loan, VerificationTimelineType.Income);
      this.panelTimelineIncome.Controls.Add((Control) this.timelineIncomeControl);
      this.timelineAssetControl = new VerificationDetailsControl(this.session, this.loan, VerificationTimelineType.Asset);
      this.panelTimelineAsset.Controls.Add((Control) this.timelineAssetControl);
      this.timelineMonthlyControl = new VerificationDetailsControl(this.session, this.loan, VerificationTimelineType.Obligation);
      this.panelTimelineObligation.Controls.Add((Control) this.timelineMonthlyControl);
    }

    public string GetHelpTargetName() => "Verification Tracking";

    public void RefreshContents()
    {
      this.timelineEmploymentControl.ReloadList();
      this.timelineIncomeControl.ReloadList();
      this.timelineAssetControl.ReloadList();
      this.timelineMonthlyControl.ReloadList();
    }

    public void RefreshLoanContents() => this.RefreshContents();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.panelAll = new Panel();
      this.panelTimelineIncome = new Panel();
      this.panelDivider1 = new Panel();
      this.panelTimelineEmployment = new Panel();
      this.panelDivider2 = new Panel();
      this.panelTimelineAsset = new Panel();
      this.panelDivider3 = new Panel();
      this.panelTimelineObligation = new Panel();
      this.panelAll.SuspendLayout();
      this.SuspendLayout();
      this.panelAll.Controls.Add((Control) this.panelTimelineObligation);
      this.panelAll.Controls.Add((Control) this.panelDivider3);
      this.panelAll.Controls.Add((Control) this.panelTimelineAsset);
      this.panelAll.Controls.Add((Control) this.panelDivider2);
      this.panelAll.Controls.Add((Control) this.panelTimelineIncome);
      this.panelAll.Controls.Add((Control) this.panelDivider1);
      this.panelAll.Controls.Add((Control) this.panelTimelineEmployment);
      this.panelAll.Dock = DockStyle.Top;
      this.panelAll.Location = new Point(0, 0);
      this.panelAll.Name = "panelAll";
      this.panelAll.Size = new Size(1191, 1030);
      this.panelAll.TabIndex = 0;
      this.panelTimelineIncome.Dock = DockStyle.Top;
      this.panelTimelineIncome.Location = new Point(0, 260);
      this.panelTimelineIncome.Name = "panelTimelineIncome";
      this.panelTimelineIncome.Size = new Size(1191, 250);
      this.panelTimelineIncome.TabIndex = 4;
      this.panelDivider1.Dock = DockStyle.Top;
      this.panelDivider1.Location = new Point(0, 250);
      this.panelDivider1.Name = "panelDivider1";
      this.panelDivider1.Size = new Size(1191, 10);
      this.panelDivider1.TabIndex = 2;
      this.panelTimelineEmployment.Dock = DockStyle.Top;
      this.panelTimelineEmployment.Location = new Point(0, 0);
      this.panelTimelineEmployment.Name = "panelTimelineEmployment";
      this.panelTimelineEmployment.Size = new Size(1191, 250);
      this.panelTimelineEmployment.TabIndex = 0;
      this.panelDivider2.Dock = DockStyle.Top;
      this.panelDivider2.Location = new Point(0, 510);
      this.panelDivider2.Name = "panelDivider2";
      this.panelDivider2.Size = new Size(1191, 10);
      this.panelDivider2.TabIndex = 6;
      this.panelTimelineAsset.Dock = DockStyle.Top;
      this.panelTimelineAsset.Location = new Point(0, 520);
      this.panelTimelineAsset.Name = "panelTimelineAsset";
      this.panelTimelineAsset.Size = new Size(1191, 250);
      this.panelTimelineAsset.TabIndex = 7;
      this.panelDivider3.Dock = DockStyle.Top;
      this.panelDivider3.Location = new Point(0, 770);
      this.panelDivider3.Name = "panelDivider3";
      this.panelDivider3.Size = new Size(1191, 10);
      this.panelDivider3.TabIndex = 8;
      this.panelTimelineObligation.Dock = DockStyle.Top;
      this.panelTimelineObligation.Location = new Point(0, 780);
      this.panelTimelineObligation.Name = "panelTimelineObligation";
      this.panelTimelineObligation.Size = new Size(1191, 250);
      this.panelTimelineObligation.TabIndex = 9;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.AutoScroll = true;
      this.Controls.Add((Control) this.panelAll);
      this.Name = nameof (VerificationTool);
      this.Size = new Size(1191, 1030);
      this.panelAll.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
