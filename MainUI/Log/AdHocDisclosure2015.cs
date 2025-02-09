// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.AdHocDisclosure2015
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.JedScriptEngine;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Log
{
  public class AdHocDisclosure2015 : Form
  {
    private DisclosureTracking2015Log newLog;
    private DisclosureTracking2015Log newLog_ls;
    private bool leAccessFlag = true;
    private bool cdAccessFlag = true;
    private IContainer components;
    private Button btnNo;
    private Button btnYes;
    private Label label1;
    private Panel panel2;
    private CheckBox chkDisclosure;
    private Label label3;
    private CheckBox chkSafeHarbor;
    private CheckBox chkSettlementServicesProvider;
    private RadioButton rdbCD;
    private RadioButton rdbLE;
    private CheckBox chkSettlementServicesProviderNoFee;

    public AdHocDisclosure2015()
    {
      if (Session.StartupInfo.UserAclFeatureRights.Contains((object) AclFeature.ToolsTab_DT_LESSPLSafeHarborNode))
        this.leAccessFlag = (bool) Session.StartupInfo.UserAclFeatureRights[(object) AclFeature.ToolsTab_DT_LESSPLSafeHarborNode];
      if (Session.StartupInfo.UserAclFeatureRights.Contains((object) AclFeature.ToolsTab_DT_CD))
        this.cdAccessFlag = (bool) Session.StartupInfo.UserAclFeatureRights[(object) AclFeature.ToolsTab_DT_CD];
      this.InitializeComponent();
      this.chkDisclosure_CheckedChanged((object) null, (EventArgs) null);
      this.enableButtons(false);
      if (this.leAccessFlag)
        return;
      this.chkSettlementServicesProviderNoFee.Enabled = false;
      this.chkSettlementServicesProvider.Enabled = false;
      this.chkSafeHarbor.Enabled = false;
    }

    private void btnYes_Click(object sender, EventArgs e)
    {
      if (!this.chkDisclosure.Checked && !this.chkSafeHarbor.Checked && !this.chkSettlementServicesProvider.Checked && !this.chkSettlementServicesProviderNoFee.Checked)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "One of the options need to be selected", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (this.chkDisclosure.Checked && !this.rdbLE.Checked && !this.rdbCD.Checked)
      {
        int num2 = (int) MessageBox.Show((IWin32Window) this, "Please indicate if the Loan Estimate (LE) or Closing Disclosure (CD) was disclosed for this record.", "Error - Indicate Disclosure Type", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        Cursor.Current = Cursors.WaitCursor;
        bool flag = false;
        Session.LoanData.Calculator.CalcOnDemand(CalcOnDemandEnum.PaymentSchedule);
        if (this.chkDisclosure.Checked && this.rdbCD.Checked)
          Session.LoanData.Calculator.FormCalculation("CLOSINGDISCLOSUREPAGE3", (string) null, (string) null);
        try
        {
          bool skipLockRequestSync = Session.LoanData.Calculator.SkipLockRequestSync;
          Session.LoanData.Calculator.SkipLockRequestSync = true;
          Session.LoanData.Calculator.CalculateAll(false);
          Session.LoanData.Calculator.SkipLockRequestSync = skipLockRequestSync;
        }
        catch (Exception ex)
        {
        }
        if (this.chkDisclosure.Checked && !this.checkLEorCDDate(this.rdbLE.Checked))
        {
          this.DialogResult = DialogResult.Cancel;
        }
        else
        {
          if (Session.LoanData.LinkedData != null && (Session.LoanData.LinkSyncType == LinkSyncType.ConstructionPrimary || Session.LoanData.LinkSyncType == LinkSyncType.ConstructionLinked))
          {
            flag = true;
            string str = !this.IsBorrowerPairSame(Session.LoanData.CurrentBorrowerPair, Session.LoanData.LinkedData.CurrentBorrowerPair) ? this.GetLinkedLoanCorrespondingBorrowerPairID(Session.LoanData.CurrentBorrowerPair) : Session.LoanData.LinkedData.CurrentBorrowerPair.Id;
          }
          this.newLog = new DisclosureTracking2015Log(DateTime.Now, Session.LoanData, this.chkDisclosure.Checked && this.rdbLE.Checked, this.chkDisclosure.Checked && this.rdbCD.Checked, true, this.chkSafeHarbor.Checked, this.chkSettlementServicesProvider.Checked, this.chkSettlementServicesProviderNoFee.Checked);
          this.newLog.DisclosedBy = Session.UserInfo.Userid;
          this.newLog.DisclosedByFullName = Session.UserInfo.FullName;
          this.newLog.DisclosureMethod = DisclosureTrackingBase.DisclosedMethod.ByMail;
          this.newLog.BorrowerPairID = Session.LoanData.CurrentBorrowerPair.Id;
          foreach (KeyValuePair<string, INonBorrowerOwnerItem> allnboItem in this.newLog.GetAllnboItems())
            this.newLog.SetnboAttributeValue(allnboItem.Key, (object) DisclosureTrackingBase.DisclosedMethod.ByMail, DisclosureTracking2015Log.NBOUpdatableFields.DisclosedMethod);
          this.newLog_ls = (DisclosureTracking2015Log) null;
          if (flag)
          {
            this.newLog_ls = new DisclosureTracking2015Log(DateTime.Now, Session.LoanData.LinkedData, this.chkDisclosure.Checked && this.rdbLE.Checked, this.chkDisclosure.Checked && this.rdbCD.Checked, true, this.chkSafeHarbor.Checked, this.chkSettlementServicesProvider.Checked, this.chkSettlementServicesProviderNoFee.Checked);
            this.newLog_ls.DisclosedBy = Session.UserInfo.Userid;
            this.newLog_ls.DisclosedByFullName = Session.UserInfo.FullName;
            this.newLog_ls.DisclosureMethod = DisclosureTrackingBase.DisclosedMethod.ByMail;
            this.newLog_ls.BorrowerPairID = !this.IsBorrowerPairSame(Session.LoanData.CurrentBorrowerPair, Session.LoanData.LinkedData.CurrentBorrowerPair) ? this.GetLinkedLoanCorrespondingBorrowerPairID(Session.LoanData.CurrentBorrowerPair) : Session.LoanData.LinkedData.CurrentBorrowerPair.Id;
            this.newLog_ls.LinkedGuid = this.newLog.Guid;
            this.newLog_ls.OriginalDisclosedDate = this.newLog.OriginalDisclosedDate;
            this.newLog_ls.IsLocked = this.newLog.IsLocked;
            this.newLog_ls.LockedDisclosedDateField = this.newLog.LockedDisclosedDateField;
            foreach (KeyValuePair<string, INonBorrowerOwnerItem> allnboItem in this.newLog_ls.GetAllnboItems())
              this.newLog_ls.SetnboAttributeValue(allnboItem.Key, (object) DisclosureTrackingBase.DisclosedMethod.ByMail, DisclosureTracking2015Log.NBOUpdatableFields.DisclosedMethod);
          }
          if (this.chkDisclosure.Checked && this.rdbCD.Checked)
          {
            this.newLog.AddDisclosedFormItem("Closing Disclosure", DisclosureTrackingFormItem.FormType.StandardForm);
            this.newLog.AddDisclosedFormItem("Closing Disclosure (Alternate)", DisclosureTrackingFormItem.FormType.StandardForm);
            this.newLog.AddDisclosedFormItem("Closing Disclosure (Seller)", DisclosureTrackingFormItem.FormType.StandardForm);
            if (flag)
            {
              this.newLog_ls.AddDisclosedFormItem("Closing Disclosure", DisclosureTrackingFormItem.FormType.StandardForm);
              this.newLog_ls.AddDisclosedFormItem("Closing Disclosure (Alternate)", DisclosureTrackingFormItem.FormType.StandardForm);
              this.newLog_ls.AddDisclosedFormItem("Closing Disclosure (Seller)", DisclosureTrackingFormItem.FormType.StandardForm);
            }
          }
          if (this.chkDisclosure.Checked && this.rdbLE.Checked)
          {
            this.newLog.AddDisclosedFormItem("Loan Estimate", DisclosureTrackingFormItem.FormType.StandardForm);
            this.newLog.AddDisclosedFormItem("Loan Estimate (Alternate)", DisclosureTrackingFormItem.FormType.StandardForm);
            if (flag)
            {
              this.newLog_ls.AddDisclosedFormItem("Loan Estimate", DisclosureTrackingFormItem.FormType.StandardForm);
              this.newLog_ls.AddDisclosedFormItem("Loan Estimate (Alternate)", DisclosureTrackingFormItem.FormType.StandardForm);
            }
          }
          if (this.chkSafeHarbor.Checked)
          {
            this.newLog.AddDisclosedFormItem("2015 Anti-Steering Safe Harbor", DisclosureTrackingFormItem.FormType.StandardForm);
            if (flag)
              this.newLog_ls.AddDisclosedFormItem("2015 Anti-Steering Safe Harbor", DisclosureTrackingFormItem.FormType.StandardForm);
          }
          if (this.chkSettlementServicesProvider.Checked)
          {
            this.newLog.AddDisclosedFormItem("2015 Settlement Service Provider List", DisclosureTrackingFormItem.FormType.StandardForm);
            if (flag)
              this.newLog_ls.AddDisclosedFormItem("2015 Settlement Service Provider List", DisclosureTrackingFormItem.FormType.StandardForm);
          }
          if (this.chkSettlementServicesProviderNoFee.Checked)
          {
            this.newLog.AddDisclosedFormItem("2015 Settlement Service Provider List - No Fees", DisclosureTrackingFormItem.FormType.StandardForm);
            if (flag)
              this.newLog_ls.AddDisclosedFormItem("2015 Settlement Service Provider List - No Fees", DisclosureTrackingFormItem.FormType.StandardForm);
          }
          Cursor.Current = Cursors.Default;
          this.DialogResult = DialogResult.Yes;
        }
      }
    }

    private bool IsBorrowerPairSame(BorrowerPair bp1, BorrowerPair bp2)
    {
      return Session.LoanData.CurrentBorrowerPair.Borrower.FirstName == Session.LoanData.LinkedData.CurrentBorrowerPair.Borrower.FirstName && Session.LoanData.CurrentBorrowerPair.Borrower.LastName == Session.LoanData.LinkedData.CurrentBorrowerPair.Borrower.LastName && Session.LoanData.CurrentBorrowerPair.CoBorrower.FirstName == Session.LoanData.LinkedData.CurrentBorrowerPair.CoBorrower.FirstName && Session.LoanData.CurrentBorrowerPair.CoBorrower.LastName == Session.LoanData.LinkedData.CurrentBorrowerPair.CoBorrower.LastName;
    }

    private string GetLinkedLoanCorrespondingBorrowerPairID(BorrowerPair bp)
    {
      if (Session.LoanData.LinkedData != null && Session.LoanData.LinkSyncType == LinkSyncType.ConstructionPrimary)
      {
        foreach (BorrowerPair borrowerPair in Session.LoanData.LinkedData.GetBorrowerPairs())
        {
          if (string.Compare(borrowerPair.Borrower.FirstName, bp.Borrower.FirstName, true) == 0 && string.Compare(borrowerPair.Borrower.LastName, bp.Borrower.LastName, true) == 0 && string.Compare(borrowerPair.CoBorrower.FirstName, bp.CoBorrower.FirstName, true) == 0 && string.Compare(borrowerPair.CoBorrower.LastName, bp.CoBorrower.LastName, true) == 0)
          {
            Session.LoanData.LinkedData.SetBorrowerPair(borrowerPair);
            return borrowerPair.Id;
          }
        }
      }
      return Session.LoanData.LinkedData.CurrentBorrowerPair.Id;
    }

    public DisclosureTracking2015Log Log => this.newLog;

    public DisclosureTracking2015Log Log_ls => this.newLog_ls;

    private void chkDisclosure_CheckedChanged(object sender, EventArgs e)
    {
      if (this.leAccessFlag)
        this.rdbLE.Enabled = this.chkDisclosure.Checked;
      else
        this.rdbLE.Enabled = false;
      if (this.cdAccessFlag)
        this.rdbCD.Enabled = this.chkDisclosure.Checked;
      else
        this.rdbCD.Enabled = false;
      this.fieldChanged(sender, e);
    }

    private void enableButtons(bool value) => this.btnYes.Enabled = value;

    private void fieldChanged(object sender, EventArgs e)
    {
      this.enableSSPL();
      if (this.chkDisclosure.Checked && (this.rdbCD.Checked || this.rdbLE.Checked))
        this.enableButtons(true);
      else if (this.chkSafeHarbor.Checked)
        this.enableButtons(true);
      else if (this.chkSettlementServicesProvider.Checked)
        this.enableButtons(true);
      else if (this.chkSettlementServicesProviderNoFee.Checked)
        this.enableButtons(true);
      else
        this.enableButtons(false);
    }

    private void enableSSPL()
    {
      if (!this.leAccessFlag)
        return;
      if (this.chkSettlementServicesProvider.Checked)
        this.chkSettlementServicesProviderNoFee.Enabled = false;
      else if (this.chkSettlementServicesProviderNoFee.Checked)
      {
        this.chkSettlementServicesProvider.Enabled = false;
      }
      else
      {
        this.chkSettlementServicesProvider.Enabled = true;
        this.chkSettlementServicesProviderNoFee.Enabled = true;
      }
    }

    private bool checkLEorCDDate(bool isLE)
    {
      string primaryLoanNumber = (string) null;
      string linkedLoanNumber = (string) null;
      bool flag1 = false;
      if (Session.LoanData.LinkedData != null && (Session.LoanData.LinkSyncType == LinkSyncType.ConstructionPrimary || Session.LoanData.LinkSyncType == LinkSyncType.ConstructionLinked))
      {
        primaryLoanNumber = Session.LoanData.GetField("364");
        linkedLoanNumber = Session.LoanData.LinkedData.GetField("364");
        flag1 = true;
      }
      bool flag2 = Session.LoanData.ContentAccess != 0;
      if (flag1 & flag2)
        flag2 = Session.LoanData.LinkedData.ContentAccess != 0;
      System.TimeZoneInfo timeZoneInfo = Utils.GetTimeZoneInfo(Session.LoanData.GetField("LE1.XG9") == "" ? Session.LoanData.GetField("LE1.X9") : Session.LoanData.GetField("LE1.XG9"));
      if (isLE)
      {
        if (flag2)
        {
          using (UpdateLECDDateIssued updateLecdDateIssued = new UpdateLECDDateIssued(true, Utils.ParseDate((object) Session.LoanData.GetField("LE1.X1")), timeZoneInfo, primaryLoanNumber, linkedLoanNumber))
          {
            if (!updateLecdDateIssued.SameIssueDate)
            {
              if (updateLecdDateIssued.ShowDialog() != DialogResult.OK)
                return false;
              if (updateLecdDateIssued.UpdateWithCurrentDate)
              {
                Session.LoanData.SetField("LE1.X1", DateTime.Today.ToString("MM/dd/yyyy"));
                if (flag1)
                  Session.LoanData.LinkedData.SetField("LE1.X1", DateTime.Today.ToString("MM/dd/yyyy"));
              }
            }
          }
        }
        else
        {
          Utils.Dialog((IWin32Window) this, "You do not have the required access rights to update this field. Please contact your system administrator for more information.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          return false;
        }
      }
      else if (flag2)
      {
        using (UpdateLECDDateIssued updateLecdDateIssued = new UpdateLECDDateIssued(false, Utils.ParseDate((object) Session.LoanData.GetField("CD1.X1")), timeZoneInfo, primaryLoanNumber, linkedLoanNumber))
        {
          if (!updateLecdDateIssued.SameIssueDate)
          {
            if (updateLecdDateIssued.ShowDialog() != DialogResult.OK)
              return false;
            if (updateLecdDateIssued.UpdateWithCurrentDate)
            {
              Session.LoanData.SetField("CD1.X1", DateTime.Today.ToString("MM/dd/yyyy"));
              if (flag1)
                Session.LoanData.LinkedData.SetField("CD1.X1", DateTime.Today.ToString("MM/dd/yyyy"));
            }
          }
        }
        if (!Session.LoanData.IsFieldReadOnly("748"))
        {
          if (Session.LoanData.GetField("748") == "//")
          {
            ActivatedPrintFormRule allRequiredFields = ((PrintFormsBpmManager) Session.BPM.GetBpmManager(BpmCategory.PrintForms)).GetAllRequiredFields(new LoanBusinessRuleInfo(Session.LoanData).CurrentLoanForBusinessRule(), Session.LoanData);
            Hashtable reqFields = new Hashtable();
            reqFields.Add((object) "748", (object) "");
            allRequiredFields.AddFormRequiredFields("Closing Disclosure", reqFields);
            allRequiredFields.AddFormRequiredFields("Closing Disclosure (Seller)", reqFields);
            allRequiredFields.AddFormRequiredFields("Closing Disclosure (Alternate)", reqFields);
            FormItemInfo formItemInfo = new FormItemInfo("General Forms", "Closing Disclosure", OutputFormType.PdfForms);
            allRequiredFields.CheckRequiredFields(new FormItemInfo[1]
            {
              formItemInfo
            }, Session.LoanData, false);
            using (PrintRuleCheckStatusForm ruleCheckStatusForm = new PrintRuleCheckStatusForm(Session.DefaultInstance, new FormItemInfo[1]
            {
              formItemInfo
            }, PdfFormPrintOptions.WithData, Session.LoanData, allRequiredFields))
            {
              ruleCheckStatusForm.RefreshPrintFormStatus();
              return ruleCheckStatusForm.ShowDialog((IWin32Window) Session.MainForm) == DialogResult.OK && ruleCheckStatusForm.PrintableForms.Length != 0;
            }
          }
        }
        else if (Session.LoanData.GetField("748") == "//")
        {
          Utils.Dialog((IWin32Window) this, "The CD Closing Date is blank and you do not have the permission to modify this date. Hence, this CD can not be printed / disclosed.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          return false;
        }
      }
      else
      {
        Utils.Dialog((IWin32Window) this, "you do not have the permission to modify this date. Hence, this CD can not be printed / disclosed.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return false;
      }
      return true;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.btnNo = new Button();
      this.btnYes = new Button();
      this.label1 = new Label();
      this.panel2 = new Panel();
      this.chkSettlementServicesProviderNoFee = new CheckBox();
      this.rdbCD = new RadioButton();
      this.rdbLE = new RadioButton();
      this.chkSettlementServicesProvider = new CheckBox();
      this.chkSafeHarbor = new CheckBox();
      this.chkDisclosure = new CheckBox();
      this.label3 = new Label();
      this.panel2.SuspendLayout();
      this.SuspendLayout();
      this.btnNo.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnNo.DialogResult = DialogResult.No;
      this.btnNo.Location = new Point(333, 140);
      this.btnNo.Name = "btnNo";
      this.btnNo.Size = new Size(75, 25);
      this.btnNo.TabIndex = 5;
      this.btnNo.Text = "Cancel";
      this.btnNo.UseVisualStyleBackColor = true;
      this.btnYes.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnYes.Location = new Point(252, 140);
      this.btnYes.Name = "btnYes";
      this.btnYes.Size = new Size(75, 25);
      this.btnYes.TabIndex = 4;
      this.btnYes.Text = "OK";
      this.btnYes.UseVisualStyleBackColor = true;
      this.btnYes.Click += new EventHandler(this.btnYes_Click);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(16, 16);
      this.label1.MaximumSize = new Size(400, 0);
      this.label1.Name = "label1";
      this.label1.Size = new Size(328, 14);
      this.label1.TabIndex = 9;
      this.label1.Text = "Do you want to record a disclosure record in Disclosure Tracking?";
      this.panel2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.panel2.Controls.Add((Control) this.chkSettlementServicesProviderNoFee);
      this.panel2.Controls.Add((Control) this.rdbCD);
      this.panel2.Controls.Add((Control) this.rdbLE);
      this.panel2.Controls.Add((Control) this.chkSettlementServicesProvider);
      this.panel2.Controls.Add((Control) this.chkSafeHarbor);
      this.panel2.Controls.Add((Control) this.chkDisclosure);
      this.panel2.Controls.Add((Control) this.label3);
      this.panel2.Location = new Point(19, 41);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(395, 93);
      this.panel2.TabIndex = 13;
      this.chkSettlementServicesProviderNoFee.AutoSize = true;
      this.chkSettlementServicesProviderNoFee.Location = new Point(108, 53);
      this.chkSettlementServicesProviderNoFee.Name = "chkSettlementServicesProviderNoFee";
      this.chkSettlementServicesProviderNoFee.Size = new Size(223, 18);
      this.chkSettlementServicesProviderNoFee.TabIndex = 7;
      this.chkSettlementServicesProviderNoFee.Text = "Settlement Service Provider List - No Fees";
      this.chkSettlementServicesProviderNoFee.UseVisualStyleBackColor = true;
      this.chkSettlementServicesProviderNoFee.CheckedChanged += new EventHandler(this.fieldChanged);
      this.rdbCD.AutoSize = true;
      this.rdbCD.Location = new Point(234, 7);
      this.rdbCD.Name = "rdbCD";
      this.rdbCD.Size = new Size(39, 18);
      this.rdbCD.TabIndex = 6;
      this.rdbCD.TabStop = true;
      this.rdbCD.Text = "CD";
      this.rdbCD.UseVisualStyleBackColor = true;
      this.rdbCD.CheckedChanged += new EventHandler(this.fieldChanged);
      this.rdbLE.AutoSize = true;
      this.rdbLE.Location = new Point(191, 6);
      this.rdbLE.Name = "rdbLE";
      this.rdbLE.Size = new Size(37, 18);
      this.rdbLE.TabIndex = 5;
      this.rdbLE.TabStop = true;
      this.rdbLE.Text = "LE";
      this.rdbLE.UseVisualStyleBackColor = true;
      this.rdbLE.CheckedChanged += new EventHandler(this.fieldChanged);
      this.chkSettlementServicesProvider.AutoSize = true;
      this.chkSettlementServicesProvider.Location = new Point(108, 30);
      this.chkSettlementServicesProvider.Name = "chkSettlementServicesProvider";
      this.chkSettlementServicesProvider.Size = new Size(179, 18);
      this.chkSettlementServicesProvider.TabIndex = 4;
      this.chkSettlementServicesProvider.Text = "Settlement Service Provider List";
      this.chkSettlementServicesProvider.UseVisualStyleBackColor = true;
      this.chkSettlementServicesProvider.CheckedChanged += new EventHandler(this.fieldChanged);
      this.chkSafeHarbor.AutoSize = true;
      this.chkSafeHarbor.Location = new Point(108, 76);
      this.chkSafeHarbor.Name = "chkSafeHarbor";
      this.chkSafeHarbor.Size = new Size(85, 18);
      this.chkSafeHarbor.TabIndex = 3;
      this.chkSafeHarbor.Text = "Safe Harbor";
      this.chkSafeHarbor.UseVisualStyleBackColor = true;
      this.chkSafeHarbor.CheckedChanged += new EventHandler(this.fieldChanged);
      this.chkDisclosure.AutoSize = true;
      this.chkDisclosure.Location = new Point(108, 7);
      this.chkDisclosure.Name = "chkDisclosure";
      this.chkDisclosure.Size = new Size(77, 18);
      this.chkDisclosure.TabIndex = 1;
      this.chkDisclosure.Text = "Disclosure";
      this.chkDisclosure.UseVisualStyleBackColor = true;
      this.chkDisclosure.CheckedChanged += new EventHandler(this.chkDisclosure_CheckedChanged);
      this.label3.AutoSize = true;
      this.label3.Location = new Point(1, 9);
      this.label3.Name = "label3";
      this.label3.Size = new Size(101, 14);
      this.label3.TabIndex = 0;
      this.label3.Text = "Disclosure includes";
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.ClientSize = new Size(426, 179);
      this.Controls.Add((Control) this.panel2);
      this.Controls.Add((Control) this.btnNo);
      this.Controls.Add((Control) this.btnYes);
      this.Controls.Add((Control) this.label1);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AdHocDisclosure2015);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "New Disclosure Record";
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
