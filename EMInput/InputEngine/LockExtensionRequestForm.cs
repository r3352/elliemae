// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.LockExtensionRequestForm
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.ProductPricing;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class LockExtensionRequestForm : UserControl, IRefreshContents
  {
    private Sessions.Session session;
    private Hashtable currentLockRequestSnapshot;
    private LockExtensionUtils lockExtensionUtils;
    private LoanData loan;
    private LockRequestLog currentConfirmedLog;
    private Hashtable companySettings;
    private int extensionNumber;
    private IContainer components;
    private GroupContainer groupContainer1;
    private TextBox txtLockExpDate;
    private Label label3;
    private Label label2;
    private TextBox txtCurrentLockExpDate;
    private Label label1;
    private Label label5;
    private TextBox txtComments;
    private TextBox txtPriceAdjustment;
    private Label label4;
    private GroupContainer groupContainer2;
    private Label label14;
    private Label lblOriLockExpDate;
    private Label lblCurLockExpDate;
    private Label lblCurNewLockExpDate;
    private Label label10;
    private Label lblCurDaysToExtend;
    private Label lblCurPriceAdj;
    private Label label6;
    private TextBox txtCurPriceAdj;
    private TextBox txtCurNewLockExpDate;
    private TextBox txtCurDaysToExtend;
    private TextBox txtCurLockExpDate;
    private TextBox txtOriLockExpDate;
    private TextBox txtLockNumDays;
    private TextBox txtLockDate;
    private TextBox txtRateSheetID;
    private GroupContainer groupContainer3;
    private GroupContainer groupContainer4;
    private GridView gvBaseRate;
    private GridView gvBasePrice;
    private TextBox textBox11;
    private TextBox textBox12;
    private Label label16;
    private Label label17;
    private TextBox textBox10;
    private TextBox textBox9;
    private Label label15;
    private Label label7;
    private Label lblBaseRate;
    private Label lblBasePrice;
    private LockExtensionDropdownBox lockExtDDL;
    private Label label81;
    private Label label80;
    private TextBox textBoxCorpConcession;
    private TextBox textBoxBranchConcession;

    public LockExtensionRequestForm(
      Sessions.Session session,
      LoanData loan,
      LockExtensionUtils settings,
      int newExtNumber)
    {
      this.session = session;
      this.loan = loan;
      this.InitializeComponent();
      this.companySettings = Session.ConfigurationManager.GetCompanySettings("POLICIES");
      this.lockExtDDL.ResetSession(this.session);
      this.lockExtDDL.DDLTextChanged += new EventHandler(this.lockExtDDL_TextChanged);
      LockRequestLog lockRequestLog = this.loan.GetLogList().GetCurrentConfirmedLockRequest();
      if (lockRequestLog == null)
      {
        LockRequestLog currentLockRequest = this.loan.GetLogList().GetCurrentLockRequest();
        Hashtable hashtable = currentLockRequest != null ? currentLockRequest.GetLockRequestSnapshot() : throw new Exception("Cannot request a lock extension without a confirmed lock.");
        if (hashtable == null)
          throw new Exception("Cannot request a lock extension without a confirmed lock (A).");
        if (string.IsNullOrEmpty((string) hashtable[(object) "3839"]))
          throw new Exception("Cannot request a lock extension without a confirmed lock (B).");
        lockRequestLog = currentLockRequest;
      }
      this.currentConfirmedLog = lockRequestLog;
      this.currentLockRequestSnapshot = this.currentConfirmedLog.GetLockRequestSnapshot();
      this.lockExtensionUtils = settings;
      this.extensionNumber = newExtNumber;
      this.RefreshContents();
      this.enforceSettings();
    }

    private void lockExtDDL_TextChanged(object sender, EventArgs e)
    {
      if (!Utils.IsInt((object) this.lockExtDDL.Text.Trim()))
      {
        this.txtLockExpDate.Text = "";
        this.txtPriceAdjustment.Text = "";
      }
      else
      {
        this.txtPriceAdjustment.Text = this.getPriceAdjustment().ToString("N3");
        this.txtLockExpDate.Text = this.getExpirationDate().ToString("MM/dd/yyyy");
        this.dataChanged(sender, e);
      }
    }

    private DateTime getExpirationDate()
    {
      return this.lockExtensionUtils.GetExtensionExpirationDate(Utils.ParseDate((object) this.txtCurrentLockExpDate.Text), Utils.ParseInt((object) this.lockExtDDL.Text));
    }

    private Decimal getPriceAdjustment()
    {
      DateTime date = Utils.ParseDate((object) this.txtCurrentLockExpDate.Text);
      int daysToExtend = Utils.ParseInt((object) this.lockExtDDL.Text);
      if (this.lockExtensionUtils.IsCompanyControlled)
        return this.lockExtensionUtils.GetPriceAdjustment(date, daysToExtend);
      return this.lockExtensionUtils.IsCompanyControlledOccur ? this.lockExtensionUtils.GetPriceAdjustmentOccur(this.extensionNumber) : 0M;
    }

    private void enforceSettings()
    {
      if (this.lockExtensionUtils.IsCompanyControlledOccur)
      {
        this.txtPriceAdjustment.Enabled = false;
        this.txtPriceAdjustment.Text = this.getPriceAdjustment().ToString("N3");
        this.lockExtDDL.Text = this.lockExtensionUtils.GetExtensionDays(this.extensionNumber).ToString();
        this.txtLockExpDate.Text = this.getExpirationDate().ToString("MM/dd/yyyy");
      }
      else
      {
        if (this.lockExtensionUtils.IsCompanyControlled)
          return;
        this.txtPriceAdjustment.Enabled = true;
        this.txtPriceAdjustment.Text = "0.000";
      }
    }

    public void RefreshContents()
    {
      if (this.currentLockRequestSnapshot == null)
        return;
      if (this.currentConfirmedLog.IsLockExtension)
      {
        this.lblCurLockExpDate.Visible = this.txtCurLockExpDate.Visible = true;
        this.lblCurDaysToExtend.Visible = this.txtCurDaysToExtend.Visible = true;
        this.lblCurNewLockExpDate.Visible = this.txtCurNewLockExpDate.Visible = true;
        this.lblCurPriceAdj.Visible = this.txtCurPriceAdj.Visible = true;
        this.lblOriLockExpDate.Text = "Original Lock Expiration Date";
        this.txtOriLockExpDate.Tag = (object) "3358";
      }
      this.populateLoanSnapshot(this.Controls);
      this.populateGridView(this.gvBaseRate, 2153, 2157);
      this.populateGridView(this.gvBaseRate, 2448, 2480);
      this.populateGridView(this.gvBasePrice, 2162, 2200);
      LoanLockUtils.IncludeAdjustments(this.gvBasePrice, this.currentLockRequestSnapshot, 3474, "Extension #");
      LoanLockUtils.IncludeAdjustments(this.gvBasePrice, this.currentLockRequestSnapshot, 4276, "Re-Lock Fees #");
      LoanLockUtils.IncludeAdjustments(this.gvBasePrice, this.currentLockRequestSnapshot, 4356, "Custom Price Adjustments #");
    }

    private void populateGridView(GridView gv, int sf, int ef)
    {
      if (this.currentLockRequestSnapshot == null)
        return;
      for (int index = sf; index <= ef; index += 2)
      {
        int num = index + 1;
        if (this.currentLockRequestSnapshot.ContainsKey((object) index.ToString()) || this.currentLockRequestSnapshot.ContainsKey((object) num.ToString()))
        {
          string empty1 = string.Empty;
          if (this.currentLockRequestSnapshot.ContainsKey((object) index.ToString()))
            empty1 = this.currentLockRequestSnapshot[(object) index.ToString()].ToString();
          string empty2 = string.Empty;
          if (this.currentLockRequestSnapshot.ContainsKey((object) num.ToString()))
            empty2 = this.currentLockRequestSnapshot[(object) num.ToString()].ToString();
          gv.Items.Add(new GVItem(empty1)
          {
            SubItems = {
              (object) empty2
            }
          });
        }
      }
    }

    private void populateLoanSnapshot(Control.ControlCollection cs)
    {
      foreach (Control c in (ArrangedElementCollection) cs)
      {
        switch (c)
        {
          case TextBox _:
            TextBox textBox = (TextBox) c;
            if (textBox != null && textBox.Tag != null)
            {
              string str = textBox.Tag.ToString();
              if (!(str == string.Empty))
              {
                switch (str)
                {
                  case "2151":
                    if (c.Name == "txtCurrentLockExpDate" && this.currentConfirmedLog.IsLockExtension)
                    {
                      str = "3364";
                      break;
                    }
                    break;
                  case "3360":
                  case "3361":
                  case "3362":
                    continue;
                }
                if (this.currentLockRequestSnapshot.ContainsKey((object) str))
                {
                  textBox.Text = this.formatValue(str, this.currentLockRequestSnapshot[(object) str].ToString());
                  continue;
                }
                continue;
              }
              continue;
            }
            continue;
          case Label _:
            Label label = (Label) c;
            if (label != null && label.Tag != null)
            {
              string str = label.Tag.ToString();
              if (!(str == string.Empty) && this.currentLockRequestSnapshot.ContainsKey((object) str))
              {
                label.Text = this.formatValue(str, this.currentLockRequestSnapshot[(object) str].ToString());
                continue;
              }
              continue;
            }
            continue;
          default:
            this.populateLoanSnapshot(c.Controls);
            continue;
        }
      }
    }

    private string formatValue(string id, string val)
    {
      if (!(id == "2216"))
        return val;
      if (string.Compare(val, "Y", true) == 0)
        return "Yes";
      return string.Compare(val, "N", true) == 0 ? "No" : "";
    }

    private bool validateData()
    {
      if (this.txtCurrentLockExpDate.Text.Trim() == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "There is no Current Lock Expiration Date.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      if (this.lockExtDDL.Text.Trim() == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Days to Extend can not be empty.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      if (this.txtLockExpDate.Text.Trim() == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "There is no new lock expiration date.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      if (this.txtPriceAdjustment.Text.Trim() == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "There is no Price Adjustment.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      string extend = new LockUtils(this.session.SessionObjects).ValidateDaysToExtend(this.session.LoanData, Utils.ParseInt((object) this.lockExtDDL.Text, 0), Utils.ParseInt((object) this.txtLockNumDays.Text, 0), (Hashtable) null, 0);
      if (extend == string.Empty)
        return true;
      int num1 = (int) Utils.Dialog((IWin32Window) this, extend, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      return false;
    }

    public void RefreshLoanContents() => this.RefreshContents();

    public bool RequestLockExtension(bool interactive)
    {
      if (!this.validateData())
        return false;
      try
      {
        this.session.LoanDataMgr.CreateExtendedRateLockRequest(this.session.UserInfo, Utils.ParseInt((object) this.lockExtDDL.Text), Utils.ParseDate((object) this.txtLockExpDate.Text, DateTime.MinValue), Utils.ParseDecimal((object) this.txtPriceAdjustment.Text, 0M), this.txtComments.Text.Trim());
      }
      catch (Exception ex)
      {
        if (!interactive)
          throw ex;
        int num = (int) Utils.Dialog((IWin32Window) this, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      this.txtComments.Text = "";
      this.lockExtDDL.Text = "";
      this.loan.SetField(string.Concat(this.lockExtDDL.Tag), "");
      this.txtLockExpDate.Text = "";
      this.loan.SetField(string.Concat(this.txtLockExpDate.Tag), "");
      this.txtPriceAdjustment.Text = "";
      this.loan.SetField(string.Concat(this.txtPriceAdjustment.Tag), "");
      if (!this.session.Application.GetService<ILoanConsole>().SaveLoan())
      {
        if (!interactive)
          throw new Exception("Failed to save loan file.");
        int num = (int) Utils.Dialog((IWin32Window) this, "Failed to save loan file.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      if (interactive)
      {
        if (this.session.SessionObjects.StartupInfo.ProductPricingPartner != null && this.session.SessionObjects.StartupInfo.ProductPricingPartner.EnableAutoLockExtensionRequest)
        {
          int num1 = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "The lock extension has been successfully confirmed.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        else
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "Lock Extension Request has been submitted successfully.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
      }
      return true;
    }

    private void dataChanged(object sender, EventArgs e)
    {
      if (!(sender is LockExtensionDropdownBox))
        return;
      LockExtensionDropdownBox extensionDropdownBox = (LockExtensionDropdownBox) sender;
      this.loan.SetField(string.Concat(extensionDropdownBox.Tag), extensionDropdownBox.Text.Trim());
    }

    private void txtPriceAdjustment_Leave(object sender, EventArgs e)
    {
      if (!Utils.IsDecimal((object) this.txtPriceAdjustment.Text))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Invalid data for price adjustment.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.txtPriceAdjustment.Text = "";
      }
      else
      {
        this.txtPriceAdjustment.Text = Utils.ParseDecimal((object) this.txtPriceAdjustment.Text.Trim()).ToString("N3");
        this.loan.SetField(string.Concat(this.txtPriceAdjustment.Tag), this.txtPriceAdjustment.Text);
      }
    }

    private void lockExtDDL_DDLTextChanged(object sender, EventArgs e)
    {
      if (!Utils.IsInt((object) this.lockExtDDL.Text.Trim()))
      {
        this.txtLockExpDate.Text = "";
        this.txtPriceAdjustment.Text = "";
      }
      else
      {
        if (this.lockExtDDL.Text.Length > 1 && this.lockExtDDL.Text.Substring(0, 1) == "-")
          this.lockExtDDL.Text = "";
        if (this.lockExtDDL.Text.Length > 3)
          this.lockExtDDL.Text = this.lockExtDDL.Text.Substring(0, 3);
        int daysToExtend = Utils.ParseInt((object) this.lockExtDDL.Text);
        if (this.lockExtensionUtils.HasPriceAdjustment(daysToExtend))
          this.txtPriceAdjustment.Text = this.lockExtensionUtils.GetPriceAdjustment(daysToExtend).ToString("N3");
        else
          this.txtPriceAdjustment.Text = "0.000";
        this.txtLockExpDate.Text = Utils.ParseDate((object) this.txtCurrentLockExpDate.Text, DateTime.MinValue).AddDays((double) daysToExtend).ToString("MM/dd/yyyy");
        this.dataChanged(sender, e);
      }
    }

    private void txtComments_Leave(object sender, EventArgs e)
    {
      if (string.IsNullOrEmpty(this.txtComments.Text.Trim()) || this.txtComments.Text.Trim().Length <= 1000)
        return;
      this.txtComments.Text = this.txtComments.Text.Substring(0, 1000);
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
      this.groupContainer1 = new GroupContainer();
      this.lockExtDDL = new LockExtensionDropdownBox();
      this.label5 = new Label();
      this.txtComments = new TextBox();
      this.txtPriceAdjustment = new TextBox();
      this.label4 = new Label();
      this.txtLockExpDate = new TextBox();
      this.label3 = new Label();
      this.label2 = new Label();
      this.txtCurrentLockExpDate = new TextBox();
      this.label1 = new Label();
      this.groupContainer2 = new GroupContainer();
      this.label81 = new Label();
      this.label80 = new Label();
      this.textBoxCorpConcession = new TextBox();
      this.textBoxBranchConcession = new TextBox();
      this.textBox11 = new TextBox();
      this.textBox12 = new TextBox();
      this.label16 = new Label();
      this.label17 = new Label();
      this.textBox10 = new TextBox();
      this.textBox9 = new TextBox();
      this.label15 = new Label();
      this.label7 = new Label();
      this.groupContainer4 = new GroupContainer();
      this.lblBasePrice = new Label();
      this.gvBasePrice = new GridView();
      this.groupContainer3 = new GroupContainer();
      this.lblBaseRate = new Label();
      this.gvBaseRate = new GridView();
      this.txtCurPriceAdj = new TextBox();
      this.txtCurNewLockExpDate = new TextBox();
      this.txtCurDaysToExtend = new TextBox();
      this.txtCurLockExpDate = new TextBox();
      this.txtOriLockExpDate = new TextBox();
      this.txtLockNumDays = new TextBox();
      this.txtLockDate = new TextBox();
      this.txtRateSheetID = new TextBox();
      this.label14 = new Label();
      this.lblOriLockExpDate = new Label();
      this.lblCurLockExpDate = new Label();
      this.lblCurNewLockExpDate = new Label();
      this.label10 = new Label();
      this.lblCurDaysToExtend = new Label();
      this.lblCurPriceAdj = new Label();
      this.label6 = new Label();
      this.groupContainer1.SuspendLayout();
      this.groupContainer2.SuspendLayout();
      this.groupContainer4.SuspendLayout();
      this.groupContainer3.SuspendLayout();
      this.SuspendLayout();
      this.groupContainer1.Borders = AnchorStyles.None;
      this.groupContainer1.Controls.Add((Control) this.lockExtDDL);
      this.groupContainer1.Controls.Add((Control) this.label5);
      this.groupContainer1.Controls.Add((Control) this.txtComments);
      this.groupContainer1.Controls.Add((Control) this.txtPriceAdjustment);
      this.groupContainer1.Controls.Add((Control) this.label4);
      this.groupContainer1.Controls.Add((Control) this.txtLockExpDate);
      this.groupContainer1.Controls.Add((Control) this.label3);
      this.groupContainer1.Controls.Add((Control) this.label2);
      this.groupContainer1.Controls.Add((Control) this.txtCurrentLockExpDate);
      this.groupContainer1.Controls.Add((Control) this.label1);
      this.groupContainer1.Dock = DockStyle.Top;
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(0, 0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(726, 130);
      this.groupContainer1.TabIndex = 0;
      this.groupContainer1.Text = "Extension Information";
      this.lockExtDDL.BackColor = Color.Transparent;
      this.lockExtDDL.Location = new Point(157, 53);
      this.lockExtDDL.Name = "lockExtDDL";
      this.lockExtDDL.Size = new Size(171, 23);
      this.lockExtDDL.TabIndex = 10;
      this.lockExtDDL.Tag = (object) "3360";
      this.lockExtDDL.DDLTextChanged += new EventHandler(this.lockExtDDL_DDLTextChanged);
      this.label5.AutoSize = true;
      this.label5.Location = new Point(391, 33);
      this.label5.Name = "label5";
      this.label5.Size = new Size(56, 13);
      this.label5.TabIndex = 9;
      this.label5.Text = "Comments";
      this.txtComments.Location = new Point(395, 54);
      this.txtComments.Multiline = true;
      this.txtComments.Name = "txtComments";
      this.txtComments.Size = new Size(317, 61);
      this.txtComments.TabIndex = 8;
      this.txtComments.Tag = (object) "3370";
      this.txtComments.Leave += new EventHandler(this.txtComments_Leave);
      this.txtPriceAdjustment.Enabled = false;
      this.txtPriceAdjustment.Location = new Point(157, 98);
      this.txtPriceAdjustment.Name = "txtPriceAdjustment";
      this.txtPriceAdjustment.Size = new Size(171, 20);
      this.txtPriceAdjustment.TabIndex = 7;
      this.txtPriceAdjustment.Tag = (object) "3362";
      this.txtPriceAdjustment.TextChanged += new EventHandler(this.dataChanged);
      this.txtPriceAdjustment.Leave += new EventHandler(this.txtPriceAdjustment_Leave);
      this.label4.AutoSize = true;
      this.label4.Location = new Point(9, 101);
      this.label4.Name = "label4";
      this.label4.Size = new Size(86, 13);
      this.label4.TabIndex = 6;
      this.label4.Text = "Price Adjustment";
      this.txtLockExpDate.Enabled = false;
      this.txtLockExpDate.Location = new Point(157, 77);
      this.txtLockExpDate.Name = "txtLockExpDate";
      this.txtLockExpDate.Size = new Size(171, 20);
      this.txtLockExpDate.TabIndex = 5;
      this.txtLockExpDate.Tag = (object) "3361";
      this.txtLockExpDate.TextChanged += new EventHandler(this.dataChanged);
      this.label3.AutoSize = true;
      this.label3.Location = new Point(9, 79);
      this.label3.Name = "label3";
      this.label3.Size = new Size(131, 13);
      this.label3.TabIndex = 4;
      this.label3.Text = "New Lock Expiration Date";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(9, 56);
      this.label2.Name = "label2";
      this.label2.Size = new Size(79, 13);
      this.label2.TabIndex = 2;
      this.label2.Text = "Days to Extend";
      this.txtCurrentLockExpDate.Location = new Point(157, 31);
      this.txtCurrentLockExpDate.Name = "txtCurrentLockExpDate";
      this.txtCurrentLockExpDate.ReadOnly = true;
      this.txtCurrentLockExpDate.Size = new Size(171, 20);
      this.txtCurrentLockExpDate.TabIndex = 1;
      this.txtCurrentLockExpDate.Tag = (object) "2151";
      this.label1.AutoSize = true;
      this.label1.Location = new Point(9, 34);
      this.label1.Name = "label1";
      this.label1.Size = new Size(143, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Current Lock Expiration Date";
      this.groupContainer2.Borders = AnchorStyles.Top | AnchorStyles.Bottom;
      this.groupContainer2.Controls.Add((Control) this.label81);
      this.groupContainer2.Controls.Add((Control) this.label80);
      this.groupContainer2.Controls.Add((Control) this.textBoxCorpConcession);
      this.groupContainer2.Controls.Add((Control) this.textBoxBranchConcession);
      this.groupContainer2.Controls.Add((Control) this.textBox11);
      this.groupContainer2.Controls.Add((Control) this.textBox12);
      this.groupContainer2.Controls.Add((Control) this.label16);
      this.groupContainer2.Controls.Add((Control) this.label17);
      this.groupContainer2.Controls.Add((Control) this.textBox10);
      this.groupContainer2.Controls.Add((Control) this.textBox9);
      this.groupContainer2.Controls.Add((Control) this.label15);
      this.groupContainer2.Controls.Add((Control) this.label7);
      this.groupContainer2.Controls.Add((Control) this.groupContainer4);
      this.groupContainer2.Controls.Add((Control) this.groupContainer3);
      this.groupContainer2.Controls.Add((Control) this.txtCurPriceAdj);
      this.groupContainer2.Controls.Add((Control) this.txtCurNewLockExpDate);
      this.groupContainer2.Controls.Add((Control) this.txtCurDaysToExtend);
      this.groupContainer2.Controls.Add((Control) this.txtCurLockExpDate);
      this.groupContainer2.Controls.Add((Control) this.txtOriLockExpDate);
      this.groupContainer2.Controls.Add((Control) this.txtLockNumDays);
      this.groupContainer2.Controls.Add((Control) this.txtLockDate);
      this.groupContainer2.Controls.Add((Control) this.txtRateSheetID);
      this.groupContainer2.Controls.Add((Control) this.label14);
      this.groupContainer2.Controls.Add((Control) this.lblOriLockExpDate);
      this.groupContainer2.Controls.Add((Control) this.lblCurLockExpDate);
      this.groupContainer2.Controls.Add((Control) this.lblCurNewLockExpDate);
      this.groupContainer2.Controls.Add((Control) this.label10);
      this.groupContainer2.Controls.Add((Control) this.lblCurDaysToExtend);
      this.groupContainer2.Controls.Add((Control) this.lblCurPriceAdj);
      this.groupContainer2.Controls.Add((Control) this.label6);
      this.groupContainer2.Dock = DockStyle.Fill;
      this.groupContainer2.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer2.Location = new Point(0, 130);
      this.groupContainer2.Name = "groupContainer2";
      this.groupContainer2.Size = new Size(726, 433);
      this.groupContainer2.TabIndex = 1;
      this.groupContainer2.Text = "Current Lock Details";
      this.label81.AutoSize = true;
      this.label81.Font = new Font("Arial", 8.25f);
      this.label81.Location = new Point(391, 328);
      this.label81.Name = "label81";
      this.label81.Size = new Size(129, 14);
      this.label81.TabIndex = 160;
      this.label81.Text = "Branch Price Concession";
      this.label80.AutoSize = true;
      this.label80.Font = new Font("Arial", 8.25f);
      this.label80.Location = new Point(391, 303);
      this.label80.Name = "label80";
      this.label80.Size = new Size(142, 14);
      this.label80.TabIndex = 159;
      this.label80.Text = "Corporate Price Concession";
      this.textBoxCorpConcession.BackColor = Color.WhiteSmoke;
      this.textBoxCorpConcession.Font = new Font("Arial", 8.25f);
      this.textBoxCorpConcession.Location = new Point(538, 300);
      this.textBoxCorpConcession.Name = "textBoxCorpConcession";
      this.textBoxCorpConcession.ReadOnly = true;
      this.textBoxCorpConcession.Size = new Size(172, 20);
      this.textBoxCorpConcession.TabIndex = 158;
      this.textBoxCorpConcession.TabStop = false;
      this.textBoxCorpConcession.Tag = (object) "3371";
      this.textBoxCorpConcession.TextAlign = HorizontalAlignment.Right;
      this.textBoxBranchConcession.BackColor = Color.WhiteSmoke;
      this.textBoxBranchConcession.Font = new Font("Arial", 8.25f);
      this.textBoxBranchConcession.Location = new Point(538, 325);
      this.textBoxBranchConcession.Name = "textBoxBranchConcession";
      this.textBoxBranchConcession.ReadOnly = true;
      this.textBoxBranchConcession.Size = new Size(171, 20);
      this.textBoxBranchConcession.TabIndex = 157;
      this.textBoxBranchConcession.TabStop = false;
      this.textBoxBranchConcession.Tag = (object) "3375";
      this.textBoxBranchConcession.TextAlign = HorizontalAlignment.Right;
      this.textBox11.Location = new Point(538, 372);
      this.textBox11.Name = "textBox11";
      this.textBox11.ReadOnly = true;
      this.textBox11.Size = new Size(171, 20);
      this.textBox11.TabIndex = 26;
      this.textBox11.Tag = (object) "2203";
      this.textBox12.Location = new Point(538, 349);
      this.textBox12.Name = "textBox12";
      this.textBox12.ReadOnly = true;
      this.textBox12.Size = new Size(171, 20);
      this.textBox12.TabIndex = 25;
      this.textBox12.Tag = (object) "2202";
      this.label16.AutoSize = true;
      this.label16.Location = new Point(391, 375);
      this.label16.Name = "label16";
      this.label16.Size = new Size(113, 13);
      this.label16.TabIndex = 24;
      this.label16.Text = "Total Price Requested";
      this.label17.AutoSize = true;
      this.label17.Location = new Point(391, 352);
      this.label17.Name = "label17";
      this.label17.Size = new Size(118, 13);
      this.label17.TabIndex = 23;
      this.label17.Text = "Total Price Adjustments";
      this.textBox10.Location = new Point(156, 325);
      this.textBox10.Name = "textBox10";
      this.textBox10.ReadOnly = true;
      this.textBox10.Size = new Size(171, 20);
      this.textBox10.TabIndex = 22;
      this.textBox10.Tag = (object) "2160";
      this.textBox9.Location = new Point(156, 301);
      this.textBox9.Name = "textBox9";
      this.textBox9.ReadOnly = true;
      this.textBox9.Size = new Size(171, 20);
      this.textBox9.TabIndex = 21;
      this.textBox9.Tag = (object) "2159";
      this.label15.AutoSize = true;
      this.label15.Location = new Point(10, 328);
      this.label15.Name = "label15";
      this.label15.Size = new Size(112, 13);
      this.label15.TabIndex = 20;
      this.label15.Text = "Total Rate Requested";
      this.label7.AutoSize = true;
      this.label7.Location = new Point(9, 304);
      this.label7.Name = "label7";
      this.label7.Size = new Size(117, 13);
      this.label7.TabIndex = 19;
      this.label7.Text = "Total Rate Adjustments";
      this.groupContainer4.Controls.Add((Control) this.lblBasePrice);
      this.groupContainer4.Controls.Add((Control) this.gvBasePrice);
      this.groupContainer4.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer4.Location = new Point(395, 136);
      this.groupContainer4.Name = "groupContainer4";
      this.groupContainer4.Size = new Size(315, 153);
      this.groupContainer4.TabIndex = 18;
      this.groupContainer4.Text = "Base Price";
      this.lblBasePrice.AutoSize = true;
      this.lblBasePrice.BackColor = Color.Transparent;
      this.lblBasePrice.Location = new Point((int) byte.MaxValue, 7);
      this.lblBasePrice.Name = "lblBasePrice";
      this.lblBasePrice.Size = new Size(56, 13);
      this.lblBasePrice.TabIndex = 2;
      this.lblBasePrice.Tag = (object) "3420";
      this.lblBasePrice.Text = "base price";
      this.gvBasePrice.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.SpringToFit = true;
      gvColumn1.Text = "Rate Adjustment";
      gvColumn1.Width = 233;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.SortMethod = GVSortMethod.Numeric;
      gvColumn2.Text = "Amount";
      gvColumn2.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn2.Width = 80;
      this.gvBasePrice.Columns.AddRange(new GVColumn[2]
      {
        gvColumn1,
        gvColumn2
      });
      this.gvBasePrice.Dock = DockStyle.Fill;
      this.gvBasePrice.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvBasePrice.Location = new Point(1, 26);
      this.gvBasePrice.Name = "gvBasePrice";
      this.gvBasePrice.Size = new Size(313, 126);
      this.gvBasePrice.TabIndex = 1;
      this.groupContainer3.Controls.Add((Control) this.lblBaseRate);
      this.groupContainer3.Controls.Add((Control) this.gvBaseRate);
      this.groupContainer3.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer3.Location = new Point(13, 136);
      this.groupContainer3.Name = "groupContainer3";
      this.groupContainer3.Size = new Size(315, 153);
      this.groupContainer3.TabIndex = 17;
      this.groupContainer3.Text = "Base Rate";
      this.lblBaseRate.AutoSize = true;
      this.lblBaseRate.BackColor = Color.Transparent;
      this.lblBaseRate.Location = new Point(260, 7);
      this.lblBaseRate.Name = "lblBaseRate";
      this.lblBaseRate.Size = new Size(51, 13);
      this.lblBaseRate.TabIndex = 1;
      this.lblBaseRate.Tag = (object) "2152";
      this.lblBaseRate.Text = "base rate";
      this.gvBaseRate.BorderStyle = BorderStyle.None;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column1";
      gvColumn3.SpringToFit = true;
      gvColumn3.Text = "Rate Adjustment";
      gvColumn3.Width = 233;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column2";
      gvColumn4.SortMethod = GVSortMethod.Numeric;
      gvColumn4.Text = "Amount";
      gvColumn4.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn4.Width = 80;
      this.gvBaseRate.Columns.AddRange(new GVColumn[2]
      {
        gvColumn3,
        gvColumn4
      });
      this.gvBaseRate.Dock = DockStyle.Fill;
      this.gvBaseRate.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvBaseRate.Location = new Point(1, 26);
      this.gvBaseRate.Name = "gvBaseRate";
      this.gvBaseRate.Size = new Size(313, 126);
      this.gvBaseRate.TabIndex = 0;
      this.txtCurPriceAdj.Location = new Point(541, 102);
      this.txtCurPriceAdj.Name = "txtCurPriceAdj";
      this.txtCurPriceAdj.ReadOnly = true;
      this.txtCurPriceAdj.Size = new Size(171, 20);
      this.txtCurPriceAdj.TabIndex = 16;
      this.txtCurPriceAdj.Tag = (object) "3365";
      this.txtCurPriceAdj.Visible = false;
      this.txtCurNewLockExpDate.Location = new Point(541, 80);
      this.txtCurNewLockExpDate.Name = "txtCurNewLockExpDate";
      this.txtCurNewLockExpDate.ReadOnly = true;
      this.txtCurNewLockExpDate.Size = new Size(171, 20);
      this.txtCurNewLockExpDate.TabIndex = 15;
      this.txtCurNewLockExpDate.Tag = (object) "3364";
      this.txtCurNewLockExpDate.Visible = false;
      this.txtCurDaysToExtend.Location = new Point(541, 55);
      this.txtCurDaysToExtend.Name = "txtCurDaysToExtend";
      this.txtCurDaysToExtend.ReadOnly = true;
      this.txtCurDaysToExtend.Size = new Size(171, 20);
      this.txtCurDaysToExtend.TabIndex = 14;
      this.txtCurDaysToExtend.Tag = (object) "3363";
      this.txtCurDaysToExtend.Visible = false;
      this.txtCurLockExpDate.Location = new Point(541, 32);
      this.txtCurLockExpDate.Name = "txtCurLockExpDate";
      this.txtCurLockExpDate.ReadOnly = true;
      this.txtCurLockExpDate.Size = new Size(171, 20);
      this.txtCurLockExpDate.TabIndex = 13;
      this.txtCurLockExpDate.Tag = (object) "2151";
      this.txtCurLockExpDate.Visible = false;
      this.txtOriLockExpDate.Location = new Point(157, 99);
      this.txtOriLockExpDate.Name = "txtOriLockExpDate";
      this.txtOriLockExpDate.ReadOnly = true;
      this.txtOriLockExpDate.Size = new Size(171, 20);
      this.txtOriLockExpDate.TabIndex = 12;
      this.txtOriLockExpDate.Tag = (object) "2151";
      this.txtLockNumDays.Location = new Point(157, 76);
      this.txtLockNumDays.Name = "txtLockNumDays";
      this.txtLockNumDays.ReadOnly = true;
      this.txtLockNumDays.Size = new Size(171, 20);
      this.txtLockNumDays.TabIndex = 11;
      this.txtLockNumDays.Tag = (object) "2150";
      this.txtLockDate.Location = new Point(157, 54);
      this.txtLockDate.Name = "txtLockDate";
      this.txtLockDate.ReadOnly = true;
      this.txtLockDate.Size = new Size(171, 20);
      this.txtLockDate.TabIndex = 10;
      this.txtLockDate.Tag = (object) "2149";
      this.txtRateSheetID.Location = new Point(157, 32);
      this.txtRateSheetID.Name = "txtRateSheetID";
      this.txtRateSheetID.ReadOnly = true;
      this.txtRateSheetID.Size = new Size(171, 20);
      this.txtRateSheetID.TabIndex = 9;
      this.txtRateSheetID.Tag = (object) "2148";
      this.label14.AutoSize = true;
      this.label14.Location = new Point(9, 83);
      this.label14.Name = "label14";
      this.label14.Size = new Size(68, 13);
      this.label14.TabIndex = 8;
      this.label14.Text = "Lock # Days";
      this.lblOriLockExpDate.AutoSize = true;
      this.lblOriLockExpDate.Location = new Point(9, 105);
      this.lblOriLockExpDate.Name = "lblOriLockExpDate";
      this.lblOriLockExpDate.Size = new Size(106, 13);
      this.lblOriLockExpDate.TabIndex = 7;
      this.lblOriLockExpDate.Text = "Lock Expiration Date";
      this.lblCurLockExpDate.AutoSize = true;
      this.lblCurLockExpDate.Location = new Point(391, 35);
      this.lblCurLockExpDate.Name = "lblCurLockExpDate";
      this.lblCurLockExpDate.Size = new Size(150, 13);
      this.lblCurLockExpDate.TabIndex = 6;
      this.lblCurLockExpDate.Text = "Previous Lock Expiration Date";
      this.lblCurLockExpDate.Visible = false;
      this.lblCurNewLockExpDate.AutoSize = true;
      this.lblCurNewLockExpDate.Location = new Point(391, 83);
      this.lblCurNewLockExpDate.Name = "lblCurNewLockExpDate";
      this.lblCurNewLockExpDate.Size = new Size(131, 13);
      this.lblCurNewLockExpDate.TabIndex = 5;
      this.lblCurNewLockExpDate.Text = "New Lock Expiration Date";
      this.lblCurNewLockExpDate.Visible = false;
      this.label10.AutoSize = true;
      this.label10.Location = new Point(9, 58);
      this.label10.Name = "label10";
      this.label10.Size = new Size(57, 13);
      this.label10.TabIndex = 4;
      this.label10.Text = "Lock Date";
      this.lblCurDaysToExtend.AutoSize = true;
      this.lblCurDaysToExtend.Location = new Point(391, 58);
      this.lblCurDaysToExtend.Name = "lblCurDaysToExtend";
      this.lblCurDaysToExtend.Size = new Size(79, 13);
      this.lblCurDaysToExtend.TabIndex = 3;
      this.lblCurDaysToExtend.Text = "Days to Extend";
      this.lblCurDaysToExtend.Visible = false;
      this.lblCurPriceAdj.AutoSize = true;
      this.lblCurPriceAdj.Location = new Point(391, 105);
      this.lblCurPriceAdj.Name = "lblCurPriceAdj";
      this.lblCurPriceAdj.Size = new Size(86, 13);
      this.lblCurPriceAdj.TabIndex = 2;
      this.lblCurPriceAdj.Text = "Price Adjustment";
      this.lblCurPriceAdj.Visible = false;
      this.label6.AutoSize = true;
      this.label6.Location = new Point(9, 35);
      this.label6.Name = "label6";
      this.label6.Size = new Size(75, 13);
      this.label6.TabIndex = 0;
      this.label6.Text = "Rate Sheet ID";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.Controls.Add((Control) this.groupContainer2);
      this.Controls.Add((Control) this.groupContainer1);
      this.Name = nameof (LockExtensionRequestForm);
      this.Size = new Size(726, 563);
      this.groupContainer1.ResumeLayout(false);
      this.groupContainer1.PerformLayout();
      this.groupContainer2.ResumeLayout(false);
      this.groupContainer2.PerformLayout();
      this.groupContainer4.ResumeLayout(false);
      this.groupContainer4.PerformLayout();
      this.groupContainer3.ResumeLayout(false);
      this.groupContainer3.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
