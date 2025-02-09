// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.InitialDisclosureAlertPanel
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.DocEngine;
using EllieMae.EMLite.eFolder;
using EllieMae.EMLite.InputEngine;
using EllieMae.EMLite.LoanUtils.Workflow;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Log
{
  public class InitialDisclosureAlertPanel : AlertPanelBase
  {
    private TextBox txtDisclosures;
    private Label lblDisclosures;
    private TextBox txtChannel;
    private Label label1;
    private Button sendDisclosuresButton;
    private LoanDataMgr loanDataMgr;
    private Sessions.Session session;

    public InitialDisclosureAlertPanel(PipelineInfo.Alert alert)
    {
      this.InitializeComponent();
      this.SetAlert(alert);
      this.AlertDateCaption = "Application Date";
      this.SetAlertDate(Utils.ParseDate((object) Session.LoanData.GetField("3142")));
      this.ShowAlternateDate("Initial Disclosure Due Date", alert.Date);
      this.loanDataMgr = Session.LoanDataMgr;
      this.session = Session.DefaultInstance;
      this.txtChannel.Text = EncompassFields.GetField("2626").ToDisplayValue(Session.LoanData.GetField("2626"));
      List<string> stringList = new List<string>();
      DisclosureTrackingLog.DisclosureTrackingType requiredDisclosures = Session.LoanData.GetRequiredDisclosures();
      if (Utils.CheckIf2015RespaTila(Session.LoanData.GetField("3969")))
      {
        stringList.Add("LE");
        if (Session.LoanData.ISLESectionCPopulated())
          stringList.Add("SSPL");
      }
      else
      {
        if ((requiredDisclosures & DisclosureTrackingLog.DisclosureTrackingType.GFE) != DisclosureTrackingLog.DisclosureTrackingType.None)
          stringList.Add("GFE");
        if ((requiredDisclosures & DisclosureTrackingLog.DisclosureTrackingType.TIL) != DisclosureTrackingLog.DisclosureTrackingType.None)
          stringList.Add("TIL");
      }
      this.txtDisclosures.Text = string.Join(", ", stringList.ToArray());
      this.CheckIfHide736();
      this.AdjustControlPositions(0);
    }

    private void updateBusinessRule()
    {
      try
      {
        if (this.loanDataMgr.LoanData == null || this.loanDataMgr.LoanData.IsTemplate)
          return;
        Session.Application.GetService<ILoanEditor>().ApplyOnDemandBusinessRules();
      }
      catch (Exception ex)
      {
      }
    }

    private void btnSendDisclosures_Click(object sender, EventArgs e)
    {
      if (this.loanDataMgr.SessionObjects.StartupInfo.EnableSendDisclosureSmartClient && this.loanDataMgr.IsPlatformLoan(setCCSiteId: true))
        Session.Application.GetService<IEFolder>().LaunchEDisclosures(Session.LoanDataMgr, Convert.ToInt32((double) this.Height * 0.9), Convert.ToInt32((double) this.Width * 0.9));
      else if (!string.IsNullOrEmpty(this.loanDataMgr.WCNotAllowedMessage))
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, this.loanDataMgr.WCNotAllowedMessage, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        if (!Modules.IsModuleAvailableForUser(EncompassModule.EDM, true))
          return;
        if ((bool) this.session.StartupInfo.PolicySettings[(object) "Policies.VALIDATESYSRULEPRIORORDERINGDOC"] && ((IEnumerable<BizRuleInfo>) ((BpmManager) this.session.BPM.GetBpmManager(BpmCategory.FieldRules)).GetAllActiveRules()).Any<BizRuleInfo>((Func<BizRuleInfo, bool>) (b => ((FieldRuleInfo) b).RequiredFields.ContainsKey((object) "BUTTON_EDISCLOSURES"))) | ((IEnumerable<BizRuleInfo>) ((BpmManager) this.session.BPM.GetBpmManager(BpmCategory.FieldAccess)).GetAllActiveRules()).SelectMany<BizRuleInfo, FieldAccessRights>((Func<BizRuleInfo, IEnumerable<FieldAccessRights>>) (far => (IEnumerable<FieldAccessRights>) ((FieldAccessRuleInfo) far).FieldAccessRights)).Any<FieldAccessRights>((Func<FieldAccessRights, bool>) (b => b.FieldID == "BUTTON_EDISCLOSURES")) && this.loanDataMgr.LoanData.Dirty)
        {
          if (Utils.Dialog((IWin32Window) this, "You must save the loan before you can add a Disclosure Tracking entry. Would you like to save the loan now?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
          {
            WaitDialog waitDialog = new WaitDialog("Encompass is currently checking validation rules to determine if there are known discrepancies before Documents can be drawn", (WaitCallback) null, (object) null);
            waitDialog.Show((IWin32Window) this);
            waitDialog.Top = this.Top + this.Height / 2 - waitDialog.Height / 2;
            waitDialog.Left = this.Left + this.Width / 2 - waitDialog.Width / 2;
            if (!Session.LoanDataMgr.SaveLoan(true, (ILoanMilestoneTemplateOrchestrator) null, false))
            {
              waitDialog.Close();
              return;
            }
            this.updateBusinessRule();
            waitDialog.Close();
          }
          this.setEDisclosureButtonAccess();
          if (this.sendDisclosuresButton != null && (!this.sendDisclosuresButton.Visible || !this.sendDisclosuresButton.Enabled))
          {
            int num2 = (int) Utils.Dialog((IWin32Window) this, "This action cannot be performed at this time due to system rules. Please contact your administrator.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return;
          }
        }
        if (new BusinessRuleCheck().HasPrerequiredFields(Session.LoanData, "BUTTON_EDISCLOSURES", true, (Hashtable) null))
          return;
        BorrowerPair currentBorrowerPair = Session.LoanData.CurrentBorrowerPair;
        ILoanServices service1 = Session.Application.GetService<ILoanServices>();
        IEPass service2 = Session.Application.GetService<IEPass>();
        if (Session.LoanData.GetField("Docs.Engine") == "New_Encompass_Docs_Solution" && service1.IsEncompassDocServiceAvailable(DocumentOrderType.Opening))
          service2.ProcessURL("_EPASS_SIGNATURE;ENCOMPASSDOCS;EDISCLOSURES2", false);
        else
          service2.ProcessURL("_EPASS_SIGNATURE;ENCOMPASSDISCLOSURES;2;PROCESS2", false);
        if (!(currentBorrowerPair.Id != Session.LoanData.PairId))
          return;
        Session.LoanData.SetBorrowerPair(currentBorrowerPair);
      }
    }

    public void CheckIfHide736()
    {
      if ((Session.LoanData.GetField("1172") == "FHA" || Session.LoanData.GetField("1172") == "FarmersHomeAdministration") && Session.LoanData.GetField("MORNET.X40") != "")
      {
        GVItem gvItem1 = (GVItem) null;
        foreach (GVItem gvItem2 in (IEnumerable<GVItem>) this.gvFields.Items)
        {
          if (gvItem2.SubItems[0].Text == "736")
          {
            gvItem1 = gvItem2;
            break;
          }
        }
        if (gvItem1 == null)
          return;
        this.gvFields.Items.Remove(gvItem1);
      }
      else
      {
        if (!(Session.LoanData.GetField("1172") != "FHA") && !(Session.LoanData.GetField("1172") != "FarmersHomeAdministration") || !(Session.LoanData.GetField("736") != ""))
          return;
        GVItem gvItem3 = (GVItem) null;
        foreach (GVItem gvItem4 in (IEnumerable<GVItem>) this.gvFields.Items)
        {
          if (gvItem4.SubItems[0].Text == "MORNET.X40")
          {
            gvItem3 = gvItem4;
            break;
          }
        }
        if (gvItem3 == null)
          return;
        this.gvFields.Items.Remove(gvItem3);
      }
    }

    protected override void AdjustControlPositions(int additionalWidth = 0)
    {
      base.AdjustControlPositions();
      if (this.txtChannel == null)
        return;
      this.txtChannel.Left = this.txtAlertDate.Left;
      this.lblDisclosures.Left = this.pnlAltDate.Left + this.lblAltDate.Left;
      this.txtDisclosures.Left = this.pnlAltDate.Left + this.txtAltDate.Left;
    }

    protected override void OnLoad(EventArgs e)
    {
      base.OnLoad(e);
      if (!new eFolderAccessRights(Session.LoanDataMgr).CanSendDisclosures)
        return;
      this.setEDisclosureButtonAccess();
    }

    private void setEDisclosureButtonAccess()
    {
      switch (Session.LoanDataMgr.GetFieldAccessRights("BUTTON_EDISCLOSURES"))
      {
        case BizRule.FieldAccessRight.Hide:
        case BizRule.FieldAccessRight.ViewOnly:
          if (this.sendDisclosuresButton == null)
            break;
          this.sendDisclosuresButton.Visible = false;
          break;
        default:
          if (this.sendDisclosuresButton == null)
          {
            this.sendDisclosuresButton = new Button();
            this.sendDisclosuresButton.Text = "Send eDisclosures";
            this.sendDisclosuresButton.AutoSize = true;
            this.sendDisclosuresButton.Margin = new Padding(3, 0, 2, 0);
            this.sendDisclosuresButton.Click += new EventHandler(this.btnSendDisclosures_Click);
            this.AddHeaderControl((Control) this.sendDisclosuresButton);
            int width = this.sendDisclosuresButton.Width;
            this.sendDisclosuresButton.AutoSize = false;
            this.sendDisclosuresButton.Height = 22;
            this.sendDisclosuresButton.Width = width;
            break;
          }
          this.sendDisclosuresButton.Visible = true;
          break;
      }
    }

    private void InitializeComponent()
    {
      this.label1 = new Label();
      this.txtChannel = new TextBox();
      this.txtDisclosures = new TextBox();
      this.lblDisclosures = new Label();
      this.grpFields.SuspendLayout();
      this.pnlAltDate.SuspendLayout();
      this.grpDetails.SuspendLayout();
      this.pnlFields.SuspendLayout();
      this.SuspendLayout();
      this.grpFields.Size = new Size(972, 314);
      this.gvFields.Size = new Size(970, 287);
      this.pnlAltDate.Location = new Point(218, 96);
      this.pnlAltDate.Size = new Size(199, 21);
      this.grpDetails.Controls.Add((Control) this.txtDisclosures);
      this.grpDetails.Controls.Add((Control) this.lblDisclosures);
      this.grpDetails.Controls.Add((Control) this.txtChannel);
      this.grpDetails.Controls.Add((Control) this.label1);
      this.grpDetails.Size = new Size(972, 148);
      this.grpDetails.Controls.SetChildIndex((Control) this.lblAlertName, 0);
      this.grpDetails.Controls.SetChildIndex((Control) this.txtAlertName, 0);
      this.grpDetails.Controls.SetChildIndex((Control) this.lblDescription, 0);
      this.grpDetails.Controls.SetChildIndex((Control) this.txtMessage, 0);
      this.grpDetails.Controls.SetChildIndex((Control) this.label1, 0);
      this.grpDetails.Controls.SetChildIndex((Control) this.txtChannel, 0);
      this.grpDetails.Controls.SetChildIndex((Control) this.lblAlertDate, 0);
      this.grpDetails.Controls.SetChildIndex((Control) this.txtAlertDate, 0);
      this.grpDetails.Controls.SetChildIndex((Control) this.pnlAltDate, 0);
      this.grpDetails.Controls.SetChildIndex((Control) this.lblDisclosures, 0);
      this.grpDetails.Controls.SetChildIndex((Control) this.txtDisclosures, 0);
      this.txtAlertDate.Location = new Point(71, 96);
      this.txtAltDate.Location = new Point(62, 0);
      this.txtMessage.Location = new Point(71, 58);
      this.txtMessage.Size = new Size(892, 36);
      this.txtAlertName.Location = new Point(71, 36);
      this.txtAlertName.Size = new Size(892, 20);
      this.pnlFields.Location = new Point(0, 148);
      this.pnlFields.Size = new Size(972, 314);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(7, 122);
      this.label1.Name = "label1";
      this.label1.Size = new Size(46, 14);
      this.label1.TabIndex = 8;
      this.label1.Text = "Channel";
      this.txtChannel.Location = new Point(74, 118);
      this.txtChannel.Name = "txtChannel";
      this.txtChannel.ReadOnly = true;
      this.txtChannel.Size = new Size(137, 20);
      this.txtChannel.TabIndex = 9;
      this.txtDisclosures.Location = new Point(352, 118);
      this.txtDisclosures.Name = "txtDisclosures";
      this.txtDisclosures.ReadOnly = true;
      this.txtDisclosures.Size = new Size(137, 20);
      this.txtDisclosures.TabIndex = 11;
      this.lblDisclosures.AutoSize = true;
      this.lblDisclosures.Location = new Point(241, 122);
      this.lblDisclosures.Name = "lblDisclosures";
      this.lblDisclosures.Size = new Size(110, 14);
      this.lblDisclosures.TabIndex = 10;
      this.lblDisclosures.Text = "Required Disclosures";
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.Name = nameof (InitialDisclosureAlertPanel);
      this.Size = new Size(972, 462);
      this.grpFields.ResumeLayout(false);
      this.pnlAltDate.ResumeLayout(false);
      this.pnlAltDate.PerformLayout();
      this.grpDetails.ResumeLayout(false);
      this.grpDetails.PerformLayout();
      this.pnlFields.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
