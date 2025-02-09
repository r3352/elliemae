// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.LockSnapshotCompareForm
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.ClientServer.EPC2;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.ProductPricing;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Log
{
  public class LockSnapshotCompareForm : Form
  {
    private LoanDataMgr loanMgr;
    private bool forConfirm;
    private Hashtable currentLockTable;
    private Hashtable diffTable = CollectionsUtil.CreateCaseInsensitiveHashtable();
    private bool excludeInterestRateOnCopyToLockRequest;
    private IContainer components;
    private Label labelCompare;
    private Button btnClose;
    private Button btnCopy;
    private Panel panelLoan;
    private Panel panelLock;
    private GroupContainer groupContainer1;
    private GridView gridLoan;
    private GroupContainer groupContainer2;
    private GridView gridLock;
    private Button btnConfirm;
    private CheckBox chkExcludeInterestRate;

    public LockSnapshotCompareForm(
      LoanDataMgr loanMgr,
      Hashtable currentLockTable,
      bool forConfirm,
      bool forceConfirm,
      bool isNewLock)
    {
      this.loanMgr = loanMgr;
      this.currentLockTable = currentLockTable;
      this.forConfirm = forConfirm;
      this.InitializeComponent();
      bool flag = false;
      if (!isNewLock)
        flag = ProductPricingUtils.IsProviderICEPPE(this.loanMgr.LoanData.GetProviderId(openedLockRequestSnapshot: currentLockTable));
      if (((LockUtils.IfShipDark(Session.SessionObjects, "EPPS_EPC2_SHIP_DARK_SR") || Session.StartupInfo == null || Session.StartupInfo.ProductPricingPartner == null || Session.StartupInfo.ProductPricingPartner.VendorPlatform != VendorPlatform.EPC2 ? 0 : (Session.StartupInfo.ProductPricingPartner.IsEPPS ? 1 : 0)) | (flag ? 1 : 0)) != 0)
      {
        this.labelCompare.Visible = false;
        this.chkExcludeInterestRate.Visible = false;
        this.btnCopy.Visible = false;
      }
      if (forceConfirm & forConfirm)
        this.btnClose.Enabled = false;
      if (this.forConfirm)
      {
        this.labelCompare.Text = "The table below displays current loan and lock request data that does not match. If you click the Confirm button, the lock will be confirmed, and the lock data will overwrite the current loan data.";
        this.btnClose.Text = "Cancel";
        this.AcceptButton = (IButtonControl) this.btnConfirm;
      }
      else
        this.btnConfirm.Visible = false;
      this.initForm();
      this.resizeListView();
      this.gridLoan.ScrollChange += new ScrollEventHandler(this.gridLoan_ScrollChange);
      this.gridLock.ScrollChange += new ScrollEventHandler(this.gridLock_ScrollChange);
    }

    private void gridLock_ScrollChange(object sender, ScrollEventArgs e)
    {
      this.gridLoan.SetHScroll(this.gridLock.ScrollHPosition);
      this.gridLoan.SetVScroll(this.gridLock.ScrollVPosition);
    }

    private void gridLoan_ScrollChange(object sender, ScrollEventArgs e)
    {
      this.gridLock.SetHScroll(this.gridLoan.ScrollHPosition);
      this.gridLock.SetVScroll(this.gridLoan.ScrollVPosition);
    }

    public bool LoanValueIsChanged => this.gridLoan.Items.Count > 0;

    private void initForm()
    {
      this.gridLoan.BeginUpdate();
      this.gridLock.BeginUpdate();
      for (int index = 0; index < LockRequestLog.RequestFieldMap.Count; ++index)
      {
        KeyValuePair<string, string> requestField = LockRequestLog.RequestFieldMap[index];
        if (!(requestField.Key == "4463") || this.currentLockTable[(object) "4463"] != null && !string.IsNullOrEmpty(this.currentLockTable[(object) "4463"].ToString()))
        {
          requestField = LockRequestLog.RequestFieldMap[index];
          if (!(requestField.Key == "4463") || this.currentLockTable[(object) "4463"] != null && !string.IsNullOrEmpty(this.currentLockTable[(object) "4463"].ToString()))
          {
            requestField = LockRequestLog.RequestFieldMap[index];
            string loanID = requestField.Value;
            requestField = LockRequestLog.RequestFieldMap[index];
            string key = requestField.Key;
            this.buildListView(loanID, key);
          }
        }
      }
      string[] fields = this.loanMgr.LoanData.Settings.FieldSettings.LockRequestAdditionalFields.GetFields(true);
      if (fields != null)
      {
        for (int index = 0; index < fields.Length; ++index)
        {
          string customFieldId = LockRequestCustomField.GenerateCustomFieldID(fields[index]);
          this.buildListView(fields[index], customFieldId);
        }
      }
      this.buildListView("NEWHUD.X1720", "3873");
      this.buildListView("NEWHUD.X1721", "3875");
      this.gridLoan.EndUpdate();
      this.gridLock.EndUpdate();
    }

    private void buildListView(string loanID, string lockID)
    {
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      string empty3 = string.Empty;
      string empty4 = string.Empty;
      string strA = this.loanMgr.LoanData.GetField(loanID);
      string strB = this.currentLockTable.ContainsKey((object) lockID) ? this.currentLockTable[(object) lockID].ToString() : string.Empty;
      FieldDefinition field = EncompassFields.GetField(loanID);
      if (field != null)
      {
        strA = field.FormatValue(strA);
        strB = field.FormatValue(strB);
      }
      if (string.Compare(lockID, "3875", true) == 0 && !string.IsNullOrWhiteSpace(strB) && !string.IsNullOrWhiteSpace(strA) && !this.GetZeroBasedParPricingSetting())
      {
        if (Utils.ParseDecimal((object) strB) == 100M - Utils.ParseDecimal((object) strA))
          return;
      }
      else if (string.Compare(strA, strB, true) == 0)
        return;
      if (!this.diffTable.ContainsKey((object) lockID))
        this.diffTable.Add((object) lockID, (object) strA);
      string str1 = field != null ? field.Description : string.Empty;
      this.gridLoan.Items.Add(new GVItem(loanID)
      {
        SubItems = {
          (object) str1,
          (object) strA
        }
      });
      string str2 = field != null ? field.Description : string.Empty;
      this.gridLock.Items.Add(new GVItem(lockID)
      {
        SubItems = {
          (object) str2,
          (object) strB
        }
      });
    }

    private bool GetZeroBasedParPricingSetting()
    {
      bool parPricingSetting = false;
      string field = this.loanMgr.LoanData.GetField("2626");
      if (string.Equals(field, "Banked - Retail", StringComparison.InvariantCultureIgnoreCase))
        parPricingSetting = Utils.ParseBoolean(Session.SessionObjects.StartupInfo.PolicySettings[(object) "Policies.EnableZeroParPricingRetail"]);
      else if (string.Equals(field, "Banked - Wholesale", StringComparison.InvariantCultureIgnoreCase))
        parPricingSetting = Utils.ParseBoolean(Session.SessionObjects.StartupInfo.PolicySettings[(object) "Policies.EnableZeroParPricingWholesale"]);
      return parPricingSetting;
    }

    private void btnCopy_Click(object sender, EventArgs e)
    {
      if (Utils.Dialog((IWin32Window) this, "Are you sure you want to copy the current loan data? If you click the Yes button, the lock request snapshot will be updated with the current loan data.", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        return;
      this.updateLockData();
      this.DialogResult = DialogResult.OK;
    }

    private void LockSnapshotCompareForm_Resize(object sender, EventArgs e)
    {
      this.resizeListView();
    }

    private void resizeListView()
    {
      this.panelLoan.Width = (this.Width - this.panelLoan.Left * 2 - 4) / 2;
      this.panelLock.Left = this.panelLoan.Left + this.panelLoan.Width - 1;
      this.panelLock.Width = this.panelLoan.Width;
      this.panelLoan.Height = this.chkExcludeInterestRate.Top - this.panelLoan.Top - 10;
      this.panelLock.Height = this.panelLoan.Height;
    }

    private void btnConfirm_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Yes;
    }

    private void updateLockData()
    {
      foreach (DictionaryEntry d in this.diffTable)
      {
        if (!(d.Key.ToString() == "2092") || !this.excludeInterestRateOnCopyToLockRequest)
        {
          if (d.Key.ToString() == "4632")
            this.setBuydownFields(d);
          else if (!this.currentLockTable.ContainsKey((object) d.Key.ToString()))
            this.currentLockTable.Add((object) d.Key.ToString(), (object) d.Value.ToString());
          else
            this.currentLockTable[(object) d.Key.ToString()] = (object) d.Value.ToString();
          if (d.Key.ToString() == "3875" && !string.IsNullOrWhiteSpace(d.Value.ToString()) && !this.GetZeroBasedParPricingSetting())
          {
            Decimal num = Utils.ParseDecimal(d.Value, 0M);
            this.currentLockTable[(object) d.Key.ToString()] = (object) (100M - num).ToString();
          }
          if (d.Key.ToString().StartsWith("LR.", StringComparison.InvariantCultureIgnoreCase))
          {
            string key = d.Key.ToString().Substring(3);
            if (!this.currentLockTable.ContainsKey((object) key))
              this.currentLockTable.Add((object) key, (object) d.Value.ToString());
            else
              this.currentLockTable[(object) key] = (object) d.Value.ToString();
          }
        }
      }
    }

    private void setBuydownFields(DictionaryEntry d)
    {
      if (!this.currentLockTable.ContainsKey((object) d.Key.ToString()))
        this.currentLockTable.Add((object) d.Key.ToString(), (object) d.Value.ToString());
      else
        this.currentLockTable[(object) d.Key.ToString()] = (object) d.Value.ToString();
      if (this.currentLockTable.ContainsKey((object) "4631") && this.currentLockTable[(object) "4631"].ToString() == "Borrower")
      {
        this.setSnapshotField(4633, 1269);
        this.setSnapshotField(4634, 1270);
        this.setSnapshotField(4635, 1271);
        this.setSnapshotField(4636, 1272);
        this.setSnapshotField(4637, 1273);
        this.setSnapshotField(4638, 1274);
        this.setSnapshotField(4639, 1613);
        this.setSnapshotField(4640, 1614);
        this.setSnapshotField(4641, 1615);
        this.setSnapshotField(4642, 1616);
        this.setSnapshotField(4643, 1617);
        this.setSnapshotField(4644, 1618);
      }
      else
      {
        int lockId = 4633;
        int loanId = 4535;
        while (lockId < 4645)
        {
          this.setSnapshotField(lockId, loanId);
          ++lockId;
          ++loanId;
        }
      }
    }

    private void setSnapshotField(int lockId, int loanId)
    {
      if (!this.currentLockTable.ContainsKey((object) lockId.ToString()))
        this.currentLockTable.Add((object) lockId.ToString(), (object) this.loanMgr.LoanData.GetField(loanId.ToString()));
      else
        this.currentLockTable[(object) lockId.ToString()] = (object) this.loanMgr.LoanData.GetField(loanId.ToString());
    }

    private void chkExcludeInterestRate_CheckedChanged(object sender, EventArgs e)
    {
      this.excludeInterestRateOnCopyToLockRequest = this.chkExcludeInterestRate.Checked;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      GVColumn gvColumn4 = new GVColumn();
      GVColumn gvColumn5 = new GVColumn();
      GVColumn gvColumn6 = new GVColumn();
      this.labelCompare = new Label();
      this.btnClose = new Button();
      this.btnCopy = new Button();
      this.panelLoan = new Panel();
      this.groupContainer1 = new GroupContainer();
      this.gridLoan = new GridView();
      this.panelLock = new Panel();
      this.groupContainer2 = new GroupContainer();
      this.gridLock = new GridView();
      this.btnConfirm = new Button();
      this.chkExcludeInterestRate = new CheckBox();
      this.panelLoan.SuspendLayout();
      this.groupContainer1.SuspendLayout();
      this.panelLock.SuspendLayout();
      this.groupContainer2.SuspendLayout();
      this.SuspendLayout();
      this.labelCompare.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.labelCompare.Location = new Point(7, 13);
      this.labelCompare.Name = "labelCompare";
      this.labelCompare.Size = new Size(752, 34);
      this.labelCompare.TabIndex = 0;
      this.labelCompare.Text = "The table below displays current loan and lock request data that does not match. To overwrite the lock request data with the loan data, click the Copy Loan Data to Lock Request button.";
      this.btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnClose.DialogResult = DialogResult.Cancel;
      this.btnClose.Location = new Point(690, 389);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new Size(75, 23);
      this.btnClose.TabIndex = 1;
      this.btnClose.Text = "Close";
      this.btnClose.UseVisualStyleBackColor = true;
      this.btnCopy.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.btnCopy.Location = new Point(10, 389);
      this.btnCopy.Name = "btnCopy";
      this.btnCopy.Size = new Size(209, 23);
      this.btnCopy.TabIndex = 2;
      this.btnCopy.Text = "Copy Loan Data to Lock Request";
      this.btnCopy.UseVisualStyleBackColor = true;
      this.btnCopy.Click += new EventHandler(this.btnCopy_Click);
      this.panelLoan.Controls.Add((Control) this.groupContainer1);
      this.panelLoan.Location = new Point(10, 50);
      this.panelLoan.Name = "panelLoan";
      this.panelLoan.Size = new Size(378, 303);
      this.panelLoan.TabIndex = 4;
      this.groupContainer1.Controls.Add((Control) this.gridLoan);
      this.groupContainer1.Dock = DockStyle.Fill;
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(0, 0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(378, 303);
      this.groupContainer1.TabIndex = 0;
      this.groupContainer1.Text = "Current Loan Data";
      this.gridLoan.AllowMultiselect = false;
      this.gridLoan.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Field ID";
      gvColumn1.Width = 100;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Field Description";
      gvColumn2.Width = 150;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Text = "Current Loan Value";
      gvColumn3.Width = 126;
      this.gridLoan.Columns.AddRange(new GVColumn[3]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3
      });
      this.gridLoan.Dock = DockStyle.Fill;
      this.gridLoan.Location = new Point(1, 26);
      this.gridLoan.Name = "gridLoan";
      this.gridLoan.Size = new Size(376, 276);
      this.gridLoan.TabIndex = 1;
      this.panelLock.Controls.Add((Control) this.groupContainer2);
      this.panelLock.Location = new Point(388, 50);
      this.panelLock.Name = "panelLock";
      this.panelLock.Size = new Size(378, 303);
      this.panelLock.TabIndex = 5;
      this.groupContainer2.Controls.Add((Control) this.gridLock);
      this.groupContainer2.Dock = DockStyle.Fill;
      this.groupContainer2.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer2.Location = new Point(0, 0);
      this.groupContainer2.Name = "groupContainer2";
      this.groupContainer2.Size = new Size(378, 303);
      this.groupContainer2.TabIndex = 0;
      this.groupContainer2.Text = "Lock Request Data";
      this.gridLock.AllowMultiselect = false;
      this.gridLock.BorderStyle = BorderStyle.None;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column1";
      gvColumn4.Text = "Field ID";
      gvColumn4.Width = 100;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column2";
      gvColumn5.Text = "Field Description";
      gvColumn5.Width = 150;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "Column3";
      gvColumn6.Text = "Lock Value";
      gvColumn6.Width = 126;
      this.gridLock.Columns.AddRange(new GVColumn[3]
      {
        gvColumn4,
        gvColumn5,
        gvColumn6
      });
      this.gridLock.Dock = DockStyle.Fill;
      this.gridLock.Location = new Point(1, 26);
      this.gridLock.Name = "gridLock";
      this.gridLock.Size = new Size(376, 276);
      this.gridLock.TabIndex = 0;
      this.btnConfirm.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnConfirm.DialogResult = DialogResult.Cancel;
      this.btnConfirm.Location = new Point(609, 389);
      this.btnConfirm.Name = "btnConfirm";
      this.btnConfirm.Size = new Size(75, 23);
      this.btnConfirm.TabIndex = 6;
      this.btnConfirm.Text = "&Confirm";
      this.btnConfirm.UseVisualStyleBackColor = true;
      this.btnConfirm.Click += new EventHandler(this.btnConfirm_Click);
      this.chkExcludeInterestRate.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.chkExcludeInterestRate.AutoSize = true;
      this.chkExcludeInterestRate.Location = new Point(22, 358);
      this.chkExcludeInterestRate.Name = "chkExcludeInterestRate";
      this.chkExcludeInterestRate.Size = new Size(338, 17);
      this.chkExcludeInterestRate.TabIndex = 7;
      this.chkExcludeInterestRate.Text = "Exclude Interest Rate if applicable when copying to Lock Request";
      this.chkExcludeInterestRate.UseVisualStyleBackColor = true;
      this.chkExcludeInterestRate.CheckedChanged += new EventHandler(this.chkExcludeInterestRate_CheckedChanged);
      this.AcceptButton = (IButtonControl) this.btnClose;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(777, 424);
      this.Controls.Add((Control) this.chkExcludeInterestRate);
      this.Controls.Add((Control) this.btnConfirm);
      this.Controls.Add((Control) this.panelLoan);
      this.Controls.Add((Control) this.btnCopy);
      this.Controls.Add((Control) this.btnClose);
      this.Controls.Add((Control) this.labelCompare);
      this.Controls.Add((Control) this.panelLock);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MinimizeBox = false;
      this.Name = nameof (LockSnapshotCompareForm);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Compare with Current Loan Data";
      this.Resize += new EventHandler(this.LockSnapshotCompareForm_Resize);
      this.panelLoan.ResumeLayout(false);
      this.groupContainer1.ResumeLayout(false);
      this.panelLock.ResumeLayout(false);
      this.groupContainer2.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
