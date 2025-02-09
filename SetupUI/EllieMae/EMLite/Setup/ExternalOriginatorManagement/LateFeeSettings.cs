// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ExternalOriginatorManagement.LateFeeSettings
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.ExternalOriginatorManagement;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup.ExternalOriginatorManagement.RestApi;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.ExternalOriginatorManagement
{
  public class LateFeeSettings : Form
  {
    private Sessions.Session session;
    private bool edit;
    private bool TPOSpecific;
    private int externalOrgID = -1;
    private ExternalLateFeeSettings settings;
    private ExternalLateFeeSettings globalSettings;
    private ExternalLateFeeSettings specificTPOSettings;
    private int globalOrSpecificTPO;
    private IConfigurationManager mngr;
    private LoanReportFieldDef currFieldDefinition;
    private LoanReportFieldDef dayClearedOtherDtFieldDef;
    private bool readOnly;
    private LoanReportFieldDefs fieldDefinitions;
    private string SourceTabText = string.Empty;
    private IContainer components;
    private Panel pnlLateFeeSettings;
    private RadioButton rdbIncludeDayClearedNo;
    private RadioButton rdbCalculateDaily;
    private RadioButton rdbIncludeDayClearedYes;
    private RadioButton rdbCalculateFlat;
    private RadioButton rdbWeekendYes;
    private RadioButton rdbWeekendNo;
    private TextBox txtDateFieldLateFee;
    private CheckBox chkLatestConditionsIssuedDate;
    private CheckBox chkDeliveryExpirationDate;
    private CheckBox chkConditionsReceivedDate;
    private CheckBox chkInitialSuspenseDate;
    private Label lblLaterOf;
    private RadioButton rdbGracePeriodStartsDayAfter;
    private RadioButton rdbGracePeriodStartsOn;
    private ComboBox cmbGracePeriodUses;
    private TextBox txtGracePeriodDays;
    private Label lblMaxLateDays;
    private Label lblLateFee;
    private Label lblFeeHandled;
    private Label lblIncludeDayCleared;
    private Label lblStartonWeekend;
    private Label lblCalculateAs;
    private Label lblGracePeriodStarts;
    private Label lblGracePeriodUses;
    private Label lblGracePeriodDays;
    private Button btnOK;
    private Button btnCancel;
    private Label lblOf;
    private RadioButton rdbFeeHandledAdj;
    private RadioButton rdbFeeHandledFee;
    private TextBox txtLateFeePlus;
    private Label lblPlus;
    private TextBox txtLateFee;
    private TextBox txtMaxLateDays;
    private Panel pnlGracePeriod;
    private Panel pnlWeekend;
    private Panel pnlLateDay;
    private Panel pnlFeeHandledAs;
    private Panel pnlCalculateAs;
    private CheckBox chkOther;
    private Label label2;
    private PictureBox picSearch;
    private ImageList imgList;
    private Panel pnlCompanySettings;
    private RadioButton rdbTPOSpecific;
    private RadioButton rdbGlobal;
    private Panel pnlInstruction;
    private Label lblInstruction;
    private Label label1;
    private Label label3;
    private Label label4;
    private Panel panel1;
    private RadioButton rdbDCOtherDate;
    private RadioButton rdbDCClearPurchaseDate;
    private RadioButton rdbDCPurchaseApprovalDate;
    private PictureBox picOtherDate;
    private TextBox txtDayClearedOtherDate;
    private Label label5;

    public LateFeeSettings(SessionObjects obj, int externalOrgID, ExternalLateFeeSettings settings)
    {
      this.InitializeComponent();
      this.session = new Sessions.Session(obj.SessionID);
      this.mngr = obj.ConfigurationManager;
      this.externalOrgID = externalOrgID;
      this.settings = settings;
      this.edit = settings != null;
      this.TPOSpecific = externalOrgID != -1;
    }

    public LateFeeSettings(
      Sessions.Session session,
      int externalOrgID,
      ExternalLateFeeSettings settings,
      int globalOrSpecificTPO,
      bool readOnly,
      string sourceTabText = "")
    {
      this.InitializeComponent();
      this.session = session;
      this.SourceTabText = sourceTabText;
      this.mngr = session.ConfigurationManager;
      this.externalOrgID = externalOrgID;
      this.settings = settings;
      this.readOnly = readOnly;
      this.globalOrSpecificTPO = globalOrSpecificTPO;
      this.globalSettings = this.session.ConfigurationManager.GetGlobalLateFeeSettings();
      this.fieldDefinitions = new LoanReportFieldDefs(this.session);
      this.fieldDefinitions = LoanReportFieldDefs.GetFieldDefs(this.session, LoanReportFieldFlags.AllDatabaseFields);
      this.TPOSpecific = externalOrgID != -1;
      if (this.TPOSpecific)
      {
        if (this.settings == null)
        {
          this.specificTPOSettings = this.settings = this.globalSettings;
          this.rdbGlobal.Select();
        }
        else
        {
          this.specificTPOSettings = this.settings;
          this.rdbTPOSpecific.Select();
        }
      }
      if (!this.TPOSpecific)
      {
        this.pnlCompanySettings.Visible = false;
        this.pnlLateFeeSettings.Location = new Point(0, 38);
        this.ClientSize = new Size(this.ClientSize.Width, this.ClientSize.Height - 67);
      }
      this.edit = this.settings != null;
      if (!this.edit)
      {
        this.rdbGracePeriodStartsOn.Select();
        this.rdbWeekendYes.Select();
        this.rdbIncludeDayClearedYes.Select();
        this.rdbFeeHandledFee.Select();
        this.rdbCalculateFlat.Select();
        this.rdbDCPurchaseApprovalDate.Select();
      }
      else
        this.populate();
      this.rdbGlobal.Checked = this.TPOSpecific && globalOrSpecificTPO == 0;
      this.pnlLateFeeSettings.Enabled = !readOnly && !this.rdbGlobal.Checked;
      this.btnOK.Enabled = !readOnly;
      this.setIconButton();
    }

    private void populate()
    {
      this.txtGracePeriodDays.Text = this.settings.GracePeriodDays.ToString();
      this.cmbGracePeriodUses.SelectedIndex = this.settings.GracePeriodCalendar;
      this.rdbGracePeriodStartsDayAfter.Checked = !(this.rdbGracePeriodStartsOn.Checked = this.settings.GracePeriodStarts == 0);
      this.chkInitialSuspenseDate.Checked = (this.settings.GracePeriodLaterOf & 1) == 1;
      this.chkConditionsReceivedDate.Checked = (this.settings.GracePeriodLaterOf & 2) == 2;
      this.chkDeliveryExpirationDate.Checked = (this.settings.GracePeriodLaterOf & 4) == 4;
      this.chkLatestConditionsIssuedDate.Checked = (this.settings.GracePeriodLaterOf & 8) == 8;
      this.chkOther.Checked = (this.settings.GracePeriodLaterOf & 16) == 16;
      if (this.chkOther.Checked)
      {
        this.currFieldDefinition = this.fieldDefinitions.GetFieldByCriterionName(this.settings.OtherDate);
        this.txtDateFieldLateFee.Text = this.currFieldDefinition.Description;
      }
      this.rdbWeekendNo.Checked = !(this.rdbWeekendYes.Checked = this.settings.StartOnWeekend == 0);
      this.rdbIncludeDayClearedNo.Checked = !(this.rdbIncludeDayClearedYes.Checked = this.settings.IncludeDay == 0);
      this.rdbFeeHandledAdj.Checked = !(this.rdbFeeHandledFee.Checked = this.settings.FeeHandledAs == 0);
      this.txtLateFee.Text = this.settings.LateFee.ToString();
      this.txtLateFeePlus.Text = this.settings.Amount.ToString();
      this.rdbCalculateDaily.Checked = !(this.rdbCalculateFlat.Checked = this.settings.CalculateAs == 0);
      this.txtMaxLateDays.Text = this.settings.MaxLateDays.ToString();
      this.rdbDCPurchaseApprovalDate.Checked = (this.settings.DayCleared & 1) == 1;
      this.rdbDCClearPurchaseDate.Checked = (this.settings.DayCleared & 2) == 2;
      this.rdbDCOtherDate.Checked = (this.settings.DayCleared & 4) == 4;
      if (this.rdbDCOtherDate.Checked)
      {
        this.dayClearedOtherDtFieldDef = this.fieldDefinitions.GetFieldByCriterionName(this.settings.DayClearedOtherDate);
        if (this.dayClearedOtherDtFieldDef != null)
          this.txtDayClearedOtherDate.Text = this.dayClearedOtherDtFieldDef.Description;
      }
      this.txtLateFee_Leave((object) null, (EventArgs) null);
      this.txtLateFeePlus_Leave((object) null, (EventArgs) null);
      this.rdbFeeHandledAdj_CheckedChanged((object) null, (EventArgs) null);
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (!this.chkConditionsReceivedDate.Checked && !this.chkDeliveryExpirationDate.Checked && !this.chkInitialSuspenseDate.Checked && !this.chkLatestConditionsIssuedDate.Checked && !this.chkOther.Checked)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Select at least one value for The Later of condition.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else if (this.chkOther.Checked && this.txtDateFieldLateFee.Text == "" || this.rdbDCOtherDate.Checked && this.txtDayClearedOtherDate.Text == "")
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "Please select a date field, if 'other' is selected.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else if (this.chkOther.Checked && this.rdbDCOtherDate.Checked && this.currFieldDefinition.CriterionFieldName == this.dayClearedOtherDtFieldDef.CriterionFieldName || this.chkConditionsReceivedDate.Checked && this.rdbDCPurchaseApprovalDate.Checked)
      {
        int num3 = (int) Utils.Dialog((IWin32Window) this, "Day Cleared may not be the same as any of the selected Grace Period Start Dates. Please modify the date selection criteria.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        if (!this.edit)
          this.settings = new ExternalLateFeeSettings();
        this.settings.GracePeriodDays = this.txtGracePeriodDays.Text.Trim() == "" ? 0 : Convert.ToInt32(this.txtGracePeriodDays.Text);
        this.settings.GracePeriodCalendar = this.cmbGracePeriodUses.SelectedIndex;
        this.settings.GracePeriodStarts = this.rdbGracePeriodStartsOn.Checked ? 0 : 1;
        this.settings.GracePeriodLaterOf = 0;
        if (this.chkInitialSuspenseDate.Checked)
          this.settings.GracePeriodLaterOf |= 1;
        if (this.chkConditionsReceivedDate.Checked)
          this.settings.GracePeriodLaterOf |= 2;
        if (this.chkDeliveryExpirationDate.Checked)
          this.settings.GracePeriodLaterOf |= 4;
        if (this.chkLatestConditionsIssuedDate.Checked)
          this.settings.GracePeriodLaterOf |= 8;
        if (this.chkOther.Checked)
          this.settings.GracePeriodLaterOf |= 16;
        if (this.chkOther.Checked)
          this.settings.OtherDate = this.currFieldDefinition.CriterionFieldName;
        this.settings.StartOnWeekend = this.rdbWeekendYes.Checked ? 0 : 1;
        this.settings.IncludeDay = this.rdbIncludeDayClearedYes.Checked ? 0 : 1;
        this.settings.FeeHandledAs = this.rdbFeeHandledFee.Checked ? 0 : 1;
        this.settings.LateFee = this.txtLateFee.Text.Trim() == "" ? 0.0 : Convert.ToDouble(this.txtLateFee.Text);
        this.settings.Amount = this.txtLateFeePlus.Text.Trim() == "" ? 0.0 : Convert.ToDouble(this.txtLateFeePlus.Text);
        this.settings.CalculateAs = this.rdbCalculateFlat.Checked ? 0 : 1;
        this.settings.MaxLateDays = this.txtMaxLateDays.Text.Trim() == "" ? 0 : Convert.ToInt32(this.txtMaxLateDays.Text);
        this.settings.DayCleared = 0;
        if (this.rdbDCPurchaseApprovalDate.Checked)
          this.settings.DayCleared |= 1;
        if (this.rdbDCClearPurchaseDate.Checked)
          this.settings.DayCleared |= 2;
        if (this.rdbDCOtherDate.Checked)
          this.settings.DayCleared |= 4;
        if (this.rdbDCOtherDate.Checked && this.dayClearedOtherDtFieldDef != null)
          this.settings.DayClearedOtherDate = this.dayClearedOtherDtFieldDef.CriterionFieldName;
        this.globalOrSpecificTPO = this.rdbGlobal.Checked ? 0 : 1;
        if (!this.TPOSpecific)
        {
          this.mngr.UpdateGlobalLateFeeSettings(this.settings);
        }
        else
        {
          if (this.rdbTPOSpecific.Checked)
            this.mngr.UpdateOrgLateFeeSettings(this.settings, this.externalOrgID);
          this.mngr.UpdateGlobalOrSpecificTPOSetting(this.externalOrgID, this.globalOrSpecificTPO);
        }
        WebhookApiHelper.PublishExternalOrgWebhookEvent(this.session.UserID, this.SourceTabText, this.externalOrgID);
        this.DialogResult = DialogResult.OK;
      }
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
    }

    private void picSearch_Click(object sender, EventArgs e)
    {
      using (FindLoanFieldDialog findLoanFieldDialog = new FindLoanFieldDialog(this.fieldDefinitions, ReportingDatabaseColumnType.Date))
      {
        if (DialogResult.OK != findLoanFieldDialog.ShowDialog((IWin32Window) this))
          return;
        if (((Control) sender).Name == "picSearch")
        {
          this.currFieldDefinition = findLoanFieldDialog.GetSelectedField();
          this.txtDateFieldLateFee.Text = this.currFieldDefinition.Description;
        }
        else
        {
          this.dayClearedOtherDtFieldDef = findLoanFieldDialog.GetSelectedField();
          this.txtDayClearedOtherDate.Text = this.dayClearedOtherDtFieldDef.Description;
        }
      }
    }

    private void setIconButton()
    {
      if (this.chkOther.Checked)
      {
        this.picSearch.Image = this.imgList.Images[this.picSearch.Name];
        this.picSearch.Enabled = true;
      }
      else
      {
        this.picSearch.Image = this.imgList.Images[this.picSearch.Name + "Disabled"];
        this.picSearch.Enabled = false;
        this.txtDateFieldLateFee.Text = "";
      }
      if (this.rdbDCOtherDate.Checked)
      {
        this.picOtherDate.Image = this.imgList.Images[this.picOtherDate.Name];
        this.picOtherDate.Enabled = true;
      }
      else
      {
        this.picOtherDate.Image = this.imgList.Images[this.picOtherDate.Name + "Disabled"];
        this.picOtherDate.Enabled = false;
        this.txtDayClearedOtherDate.Text = "";
      }
    }

    private void picSearch_MouseEnter(object sender, EventArgs e)
    {
      PictureBox pictureBox = (PictureBox) sender;
      pictureBox.Image = this.imgList.Images[pictureBox.Name + "MouseOver"];
    }

    private void picSearch_MouseLeave(object sender, EventArgs e)
    {
      PictureBox pictureBox = (PictureBox) sender;
      if (!pictureBox.Enabled)
        return;
      pictureBox.Image = this.imgList.Images[pictureBox.Name];
    }

    private void chkOther_CheckedChanged(object sender, EventArgs e) => this.setIconButton();

    private void numericOnly_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar.Equals((object) Keys.Delete))
        return;
      char keyChar = e.KeyChar;
      if (keyChar.Equals('\b'))
        return;
      keyChar = e.KeyChar;
      if (keyChar.Equals('.'))
        return;
      if (!char.IsNumber(e.KeyChar))
        e.Handled = true;
      else
        e.Handled = false;
    }

    private void numericOnly_KeyPress1(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar.Equals((object) Keys.Delete) || e.KeyChar.Equals('\b'))
        return;
      if (!char.IsNumber(e.KeyChar))
        e.Handled = true;
      else
        e.Handled = false;
    }

    private void txtLateFee_Leave(object sender, EventArgs e)
    {
      if (this.txtLateFee.Text == "")
        return;
      try
      {
        if (Convert.ToDouble(this.txtLateFee.Text) == 0.0)
          return;
        this.txtLateFee.Text = Convert.ToDouble(this.txtLateFee.Text).ToString("#,0.00000");
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.txtLateFee.Text = "";
        this.txtLateFee.Focus();
      }
    }

    private void txtLateFeePlus_Leave(object sender, EventArgs e)
    {
      if (this.txtLateFeePlus.Text == "")
        return;
      try
      {
        if (Convert.ToDouble(this.txtLateFeePlus.Text) == 0.0)
          return;
        this.txtLateFeePlus.Text = Convert.ToDouble(this.txtLateFeePlus.Text).ToString("#,0.00");
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.txtLateFeePlus.Text = "";
        this.txtLateFeePlus.Focus();
      }
    }

    private void txtLateFee_Enter(object sender, EventArgs e)
    {
      if (this.txtLateFee.Text == "" || Convert.ToDouble(this.txtLateFee.Text) == 0.0)
        return;
      this.txtLateFee.Text = Convert.ToDouble(this.txtLateFee.Text).ToString("0.00000");
    }

    private void txtLateFeePlus_Enter(object sender, EventArgs e)
    {
      if (this.txtLateFeePlus.Text == "" || Convert.ToDouble(this.txtLateFeePlus.Text) == 0.0)
        return;
      this.txtLateFeePlus.Text = Convert.ToDouble(this.txtLateFeePlus.Text).ToString("0.00");
    }

    public ExternalLateFeeSettings Settings => this.settings;

    private void rdbTPOSpecific_CheckedChanged(object sender, EventArgs e)
    {
      if (!this.readOnly)
      {
        this.pnlLateFeeSettings.Enabled = true;
        this.btnOK.Enabled = true;
      }
      this.settings = this.specificTPOSettings;
      this.populate();
    }

    private void rdbGlobal_CheckedChanged(object sender, EventArgs e)
    {
      this.pnlLateFeeSettings.Enabled = false;
      this.settings = this.globalSettings;
      this.populate();
    }

    private void rdbFeeHandledAdj_CheckedChanged(object sender, EventArgs e)
    {
      if (this.rdbFeeHandledAdj.Checked)
        this.txtLateFeePlus.Text = "";
      this.txtLateFeePlus.Enabled = !this.rdbFeeHandledAdj.Checked;
    }

    private void rdbDayClearedOther_CheckedChanged(object sender, EventArgs e)
    {
      this.setIconButton();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (LateFeeSettings));
      this.pnlLateFeeSettings = new Panel();
      this.panel1 = new Panel();
      this.picOtherDate = new PictureBox();
      this.txtDayClearedOtherDate = new TextBox();
      this.rdbDCOtherDate = new RadioButton();
      this.rdbDCClearPurchaseDate = new RadioButton();
      this.rdbDCPurchaseApprovalDate = new RadioButton();
      this.label4 = new Label();
      this.label3 = new Label();
      this.label1 = new Label();
      this.picSearch = new PictureBox();
      this.label2 = new Label();
      this.chkOther = new CheckBox();
      this.pnlCalculateAs = new Panel();
      this.rdbCalculateFlat = new RadioButton();
      this.rdbCalculateDaily = new RadioButton();
      this.pnlFeeHandledAs = new Panel();
      this.rdbFeeHandledFee = new RadioButton();
      this.rdbFeeHandledAdj = new RadioButton();
      this.pnlLateDay = new Panel();
      this.rdbIncludeDayClearedYes = new RadioButton();
      this.rdbIncludeDayClearedNo = new RadioButton();
      this.pnlWeekend = new Panel();
      this.rdbWeekendYes = new RadioButton();
      this.rdbWeekendNo = new RadioButton();
      this.pnlGracePeriod = new Panel();
      this.rdbGracePeriodStartsOn = new RadioButton();
      this.rdbGracePeriodStartsDayAfter = new RadioButton();
      this.txtMaxLateDays = new TextBox();
      this.lblOf = new Label();
      this.txtLateFeePlus = new TextBox();
      this.lblPlus = new Label();
      this.txtLateFee = new TextBox();
      this.txtDateFieldLateFee = new TextBox();
      this.chkLatestConditionsIssuedDate = new CheckBox();
      this.chkDeliveryExpirationDate = new CheckBox();
      this.chkConditionsReceivedDate = new CheckBox();
      this.chkInitialSuspenseDate = new CheckBox();
      this.lblLaterOf = new Label();
      this.cmbGracePeriodUses = new ComboBox();
      this.txtGracePeriodDays = new TextBox();
      this.lblMaxLateDays = new Label();
      this.lblLateFee = new Label();
      this.lblFeeHandled = new Label();
      this.lblIncludeDayCleared = new Label();
      this.lblStartonWeekend = new Label();
      this.lblCalculateAs = new Label();
      this.lblGracePeriodStarts = new Label();
      this.lblGracePeriodUses = new Label();
      this.lblGracePeriodDays = new Label();
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.imgList = new ImageList(this.components);
      this.pnlCompanySettings = new Panel();
      this.rdbTPOSpecific = new RadioButton();
      this.rdbGlobal = new RadioButton();
      this.pnlInstruction = new Panel();
      this.lblInstruction = new Label();
      this.label5 = new Label();
      this.pnlLateFeeSettings.SuspendLayout();
      this.panel1.SuspendLayout();
      ((ISupportInitialize) this.picOtherDate).BeginInit();
      ((ISupportInitialize) this.picSearch).BeginInit();
      this.pnlCalculateAs.SuspendLayout();
      this.pnlFeeHandledAs.SuspendLayout();
      this.pnlLateDay.SuspendLayout();
      this.pnlWeekend.SuspendLayout();
      this.pnlGracePeriod.SuspendLayout();
      this.pnlCompanySettings.SuspendLayout();
      this.pnlInstruction.SuspendLayout();
      this.SuspendLayout();
      this.pnlLateFeeSettings.Controls.Add((Control) this.label5);
      this.pnlLateFeeSettings.Controls.Add((Control) this.panel1);
      this.pnlLateFeeSettings.Controls.Add((Control) this.label4);
      this.pnlLateFeeSettings.Controls.Add((Control) this.label3);
      this.pnlLateFeeSettings.Controls.Add((Control) this.label1);
      this.pnlLateFeeSettings.Controls.Add((Control) this.picSearch);
      this.pnlLateFeeSettings.Controls.Add((Control) this.label2);
      this.pnlLateFeeSettings.Controls.Add((Control) this.chkOther);
      this.pnlLateFeeSettings.Controls.Add((Control) this.pnlCalculateAs);
      this.pnlLateFeeSettings.Controls.Add((Control) this.pnlFeeHandledAs);
      this.pnlLateFeeSettings.Controls.Add((Control) this.pnlLateDay);
      this.pnlLateFeeSettings.Controls.Add((Control) this.pnlWeekend);
      this.pnlLateFeeSettings.Controls.Add((Control) this.pnlGracePeriod);
      this.pnlLateFeeSettings.Controls.Add((Control) this.txtMaxLateDays);
      this.pnlLateFeeSettings.Controls.Add((Control) this.lblOf);
      this.pnlLateFeeSettings.Controls.Add((Control) this.txtLateFeePlus);
      this.pnlLateFeeSettings.Controls.Add((Control) this.lblPlus);
      this.pnlLateFeeSettings.Controls.Add((Control) this.txtLateFee);
      this.pnlLateFeeSettings.Controls.Add((Control) this.txtDateFieldLateFee);
      this.pnlLateFeeSettings.Controls.Add((Control) this.chkLatestConditionsIssuedDate);
      this.pnlLateFeeSettings.Controls.Add((Control) this.chkDeliveryExpirationDate);
      this.pnlLateFeeSettings.Controls.Add((Control) this.chkConditionsReceivedDate);
      this.pnlLateFeeSettings.Controls.Add((Control) this.chkInitialSuspenseDate);
      this.pnlLateFeeSettings.Controls.Add((Control) this.lblLaterOf);
      this.pnlLateFeeSettings.Controls.Add((Control) this.cmbGracePeriodUses);
      this.pnlLateFeeSettings.Controls.Add((Control) this.txtGracePeriodDays);
      this.pnlLateFeeSettings.Controls.Add((Control) this.lblMaxLateDays);
      this.pnlLateFeeSettings.Controls.Add((Control) this.lblLateFee);
      this.pnlLateFeeSettings.Controls.Add((Control) this.lblFeeHandled);
      this.pnlLateFeeSettings.Controls.Add((Control) this.lblIncludeDayCleared);
      this.pnlLateFeeSettings.Controls.Add((Control) this.lblStartonWeekend);
      this.pnlLateFeeSettings.Controls.Add((Control) this.lblCalculateAs);
      this.pnlLateFeeSettings.Controls.Add((Control) this.lblGracePeriodStarts);
      this.pnlLateFeeSettings.Controls.Add((Control) this.lblGracePeriodUses);
      this.pnlLateFeeSettings.Controls.Add((Control) this.lblGracePeriodDays);
      this.pnlLateFeeSettings.Location = new Point(0, 97);
      this.pnlLateFeeSettings.Name = "pnlLateFeeSettings";
      this.pnlLateFeeSettings.Size = new Size(434, 589);
      this.pnlLateFeeSettings.TabIndex = 0;
      this.panel1.Controls.Add((Control) this.picOtherDate);
      this.panel1.Controls.Add((Control) this.txtDayClearedOtherDate);
      this.panel1.Controls.Add((Control) this.rdbDCOtherDate);
      this.panel1.Controls.Add((Control) this.rdbDCClearPurchaseDate);
      this.panel1.Controls.Add((Control) this.rdbDCPurchaseApprovalDate);
      this.panel1.Location = new Point(195, 256);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(227, 79);
      this.panel1.TabIndex = 41;
      this.picOtherDate.Location = new Point(187, 53);
      this.picOtherDate.Name = "picOtherDate";
      this.picOtherDate.Size = new Size(20, 20);
      this.picOtherDate.TabIndex = 44;
      this.picOtherDate.TabStop = false;
      this.picOtherDate.Click += new EventHandler(this.picSearch_Click);
      this.picOtherDate.MouseEnter += new EventHandler(this.picSearch_MouseEnter);
      this.picOtherDate.MouseLeave += new EventHandler(this.picSearch_MouseLeave);
      this.txtDayClearedOtherDate.Location = new Point(89, 54);
      this.txtDayClearedOtherDate.MaxLength = 64;
      this.txtDayClearedOtherDate.Name = "txtDayClearedOtherDate";
      this.txtDayClearedOtherDate.ReadOnly = true;
      this.txtDayClearedOtherDate.Size = new Size(92, 20);
      this.txtDayClearedOtherDate.TabIndex = 43;
      this.rdbDCOtherDate.AutoSize = true;
      this.rdbDCOtherDate.Location = new Point(12, 55);
      this.rdbDCOtherDate.Name = "rdbDCOtherDate";
      this.rdbDCOtherDate.Size = new Size(77, 17);
      this.rdbDCOtherDate.TabIndex = 42;
      this.rdbDCOtherDate.TabStop = true;
      this.rdbDCOtherDate.Text = "Other Date";
      this.rdbDCOtherDate.UseVisualStyleBackColor = true;
      this.rdbDCOtherDate.CheckedChanged += new EventHandler(this.rdbDayClearedOther_CheckedChanged);
      this.rdbDCClearPurchaseDate.AutoSize = true;
      this.rdbDCClearPurchaseDate.Location = new Point(12, 32);
      this.rdbDCClearPurchaseDate.Name = "rdbDCClearPurchaseDate";
      this.rdbDCClearPurchaseDate.Size = new Size(150, 17);
      this.rdbDCClearPurchaseDate.TabIndex = 41;
      this.rdbDCClearPurchaseDate.TabStop = true;
      this.rdbDCClearPurchaseDate.Text = "Cleared for Purchase Date";
      this.rdbDCClearPurchaseDate.UseVisualStyleBackColor = true;
      this.rdbDCPurchaseApprovalDate.AutoSize = true;
      this.rdbDCPurchaseApprovalDate.Location = new Point(12, 9);
      this.rdbDCPurchaseApprovalDate.Name = "rdbDCPurchaseApprovalDate";
      this.rdbDCPurchaseApprovalDate.Size = new Size(141, 17);
      this.rdbDCPurchaseApprovalDate.TabIndex = 40;
      this.rdbDCPurchaseApprovalDate.TabStop = true;
      this.rdbDCPurchaseApprovalDate.Text = "Purchase Approval Date";
      this.rdbDCPurchaseApprovalDate.UseVisualStyleBackColor = true;
      this.label4.AutoSize = true;
      this.label4.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label4.Location = new Point(27, 345);
      this.label4.Name = "label4";
      this.label4.Size = new Size(134, 13);
      this.label4.TabIndex = 40;
      this.label4.Text = "Late Fee Calculations:";
      this.label3.AutoSize = true;
      this.label3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label3.Location = new Point(27, 267);
      this.label3.Name = "label3";
      this.label3.Size = new Size(80, 13);
      this.label3.TabIndex = 36;
      this.label3.Text = "Day Cleared:";
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(27, 6);
      this.label1.Name = "label1";
      this.label1.Size = new Size(85, 13);
      this.label1.TabIndex = 35;
      this.label1.Text = "Grace Period:";
      this.picSearch.Location = new Point(382, 224);
      this.picSearch.Name = "picSearch";
      this.picSearch.Size = new Size(20, 20);
      this.picSearch.TabIndex = 34;
      this.picSearch.TabStop = false;
      this.picSearch.Click += new EventHandler(this.picSearch_Click);
      this.picSearch.MouseEnter += new EventHandler(this.picSearch_MouseEnter);
      this.picSearch.MouseLeave += new EventHandler(this.picSearch_MouseLeave);
      this.label2.AutoSize = true;
      this.label2.BackColor = Color.Transparent;
      this.label2.ForeColor = Color.Red;
      this.label2.Location = new Point(264, 102);
      this.label2.Name = "label2";
      this.label2.Size = new Size(11, 13);
      this.label2.TabIndex = 33;
      this.label2.Text = "*";
      this.chkOther.AutoSize = true;
      this.chkOther.Location = new Point(207, 227);
      this.chkOther.Name = "chkOther";
      this.chkOther.Size = new Size(78, 17);
      this.chkOther.TabIndex = 8;
      this.chkOther.Text = "Other Date";
      this.chkOther.UseVisualStyleBackColor = true;
      this.chkOther.CheckedChanged += new EventHandler(this.chkOther_CheckedChanged);
      this.pnlCalculateAs.Controls.Add((Control) this.rdbCalculateFlat);
      this.pnlCalculateAs.Controls.Add((Control) this.rdbCalculateDaily);
      this.pnlCalculateAs.Location = new Point(207, 506);
      this.pnlCalculateAs.Name = "pnlCalculateAs";
      this.pnlCalculateAs.Size = new Size(159, 20);
      this.pnlCalculateAs.TabIndex = 16;
      this.rdbCalculateFlat.AutoSize = true;
      this.rdbCalculateFlat.Location = new Point(0, 1);
      this.rdbCalculateFlat.Name = "rdbCalculateFlat";
      this.rdbCalculateFlat.Size = new Size(42, 17);
      this.rdbCalculateFlat.TabIndex = 20;
      this.rdbCalculateFlat.TabStop = true;
      this.rdbCalculateFlat.Text = "Flat";
      this.rdbCalculateFlat.UseVisualStyleBackColor = true;
      this.rdbCalculateDaily.AutoSize = true;
      this.rdbCalculateDaily.Location = new Point(65, 1);
      this.rdbCalculateDaily.Name = "rdbCalculateDaily";
      this.rdbCalculateDaily.Size = new Size(69, 17);
      this.rdbCalculateDaily.TabIndex = 21;
      this.rdbCalculateDaily.TabStop = true;
      this.rdbCalculateDaily.Text = "Daily Fee";
      this.rdbCalculateDaily.UseVisualStyleBackColor = true;
      this.pnlFeeHandledAs.Controls.Add((Control) this.rdbFeeHandledFee);
      this.pnlFeeHandledAs.Controls.Add((Control) this.rdbFeeHandledAdj);
      this.pnlFeeHandledAs.Location = new Point(207, 420);
      this.pnlFeeHandledAs.Name = "pnlFeeHandledAs";
      this.pnlFeeHandledAs.Size = new Size(180, 20);
      this.pnlFeeHandledAs.TabIndex = 12;
      this.rdbFeeHandledFee.AutoSize = true;
      this.rdbFeeHandledFee.Location = new Point(0, 0);
      this.rdbFeeHandledFee.Name = "rdbFeeHandledFee";
      this.rdbFeeHandledFee.Size = new Size(43, 17);
      this.rdbFeeHandledFee.TabIndex = 1;
      this.rdbFeeHandledFee.TabStop = true;
      this.rdbFeeHandledFee.Text = "Fee";
      this.rdbFeeHandledFee.UseVisualStyleBackColor = true;
      this.rdbFeeHandledFee.CheckedChanged += new EventHandler(this.rdbFeeHandledAdj_CheckedChanged);
      this.rdbFeeHandledAdj.AutoSize = true;
      this.rdbFeeHandledAdj.Location = new Point(65, 0);
      this.rdbFeeHandledAdj.Name = "rdbFeeHandledAdj";
      this.rdbFeeHandledAdj.Size = new Size(104, 17);
      this.rdbFeeHandledAdj.TabIndex = 2;
      this.rdbFeeHandledAdj.TabStop = true;
      this.rdbFeeHandledAdj.Text = "Price Adjustment";
      this.rdbFeeHandledAdj.UseVisualStyleBackColor = true;
      this.rdbFeeHandledAdj.CheckedChanged += new EventHandler(this.rdbFeeHandledAdj_CheckedChanged);
      this.pnlLateDay.Controls.Add((Control) this.rdbIncludeDayClearedYes);
      this.pnlLateDay.Controls.Add((Control) this.rdbIncludeDayClearedNo);
      this.pnlLateDay.Location = new Point(207, 395);
      this.pnlLateDay.Name = "pnlLateDay";
      this.pnlLateDay.Size = new Size(130, 20);
      this.pnlLateDay.TabIndex = 11;
      this.rdbIncludeDayClearedYes.AutoSize = true;
      this.rdbIncludeDayClearedYes.Location = new Point(0, 0);
      this.rdbIncludeDayClearedYes.Name = "rdbIncludeDayClearedYes";
      this.rdbIncludeDayClearedYes.Size = new Size(43, 17);
      this.rdbIncludeDayClearedYes.TabIndex = 1;
      this.rdbIncludeDayClearedYes.TabStop = true;
      this.rdbIncludeDayClearedYes.Text = "Yes";
      this.rdbIncludeDayClearedYes.UseVisualStyleBackColor = true;
      this.rdbIncludeDayClearedNo.AutoSize = true;
      this.rdbIncludeDayClearedNo.Location = new Point(65, 0);
      this.rdbIncludeDayClearedNo.Name = "rdbIncludeDayClearedNo";
      this.rdbIncludeDayClearedNo.Size = new Size(39, 17);
      this.rdbIncludeDayClearedNo.TabIndex = 2;
      this.rdbIncludeDayClearedNo.TabStop = true;
      this.rdbIncludeDayClearedNo.Text = "No";
      this.rdbIncludeDayClearedNo.UseVisualStyleBackColor = true;
      this.pnlWeekend.Controls.Add((Control) this.rdbWeekendYes);
      this.pnlWeekend.Controls.Add((Control) this.rdbWeekendNo);
      this.pnlWeekend.Location = new Point(207, 371);
      this.pnlWeekend.Name = "pnlWeekend";
      this.pnlWeekend.Size = new Size(130, 20);
      this.pnlWeekend.TabIndex = 10;
      this.rdbWeekendYes.AutoSize = true;
      this.rdbWeekendYes.Location = new Point(0, 0);
      this.rdbWeekendYes.Name = "rdbWeekendYes";
      this.rdbWeekendYes.Size = new Size(43, 17);
      this.rdbWeekendYes.TabIndex = 1;
      this.rdbWeekendYes.TabStop = true;
      this.rdbWeekendYes.Text = "Yes";
      this.rdbWeekendYes.UseVisualStyleBackColor = true;
      this.rdbWeekendNo.AutoSize = true;
      this.rdbWeekendNo.Location = new Point(65, 0);
      this.rdbWeekendNo.Name = "rdbWeekendNo";
      this.rdbWeekendNo.Size = new Size(39, 17);
      this.rdbWeekendNo.TabIndex = 2;
      this.rdbWeekendNo.TabStop = true;
      this.rdbWeekendNo.Text = "No";
      this.rdbWeekendNo.UseVisualStyleBackColor = true;
      this.pnlGracePeriod.Controls.Add((Control) this.rdbGracePeriodStartsOn);
      this.pnlGracePeriod.Controls.Add((Control) this.rdbGracePeriodStartsDayAfter);
      this.pnlGracePeriod.Location = new Point(207, 77);
      this.pnlGracePeriod.Name = "pnlGracePeriod";
      this.pnlGracePeriod.Size = new Size(139, 20);
      this.pnlGracePeriod.TabIndex = 3;
      this.rdbGracePeriodStartsOn.AutoSize = true;
      this.rdbGracePeriodStartsOn.Location = new Point(0, 0);
      this.rdbGracePeriodStartsOn.Name = "rdbGracePeriodStartsOn";
      this.rdbGracePeriodStartsOn.Size = new Size(39, 17);
      this.rdbGracePeriodStartsOn.TabIndex = 1;
      this.rdbGracePeriodStartsOn.TabStop = true;
      this.rdbGracePeriodStartsOn.Text = "On";
      this.rdbGracePeriodStartsOn.UseVisualStyleBackColor = true;
      this.rdbGracePeriodStartsDayAfter.AutoSize = true;
      this.rdbGracePeriodStartsDayAfter.Location = new Point(65, 0);
      this.rdbGracePeriodStartsDayAfter.Name = "rdbGracePeriodStartsDayAfter";
      this.rdbGracePeriodStartsDayAfter.Size = new Size(69, 17);
      this.rdbGracePeriodStartsDayAfter.TabIndex = 2;
      this.rdbGracePeriodStartsDayAfter.TabStop = true;
      this.rdbGracePeriodStartsDayAfter.Text = "Day After";
      this.rdbGracePeriodStartsDayAfter.UseVisualStyleBackColor = true;
      this.txtMaxLateDays.Location = new Point(207, 536);
      this.txtMaxLateDays.MaxLength = 9;
      this.txtMaxLateDays.Name = "txtMaxLateDays";
      this.txtMaxLateDays.ShortcutsEnabled = false;
      this.txtMaxLateDays.Size = new Size(100, 20);
      this.txtMaxLateDays.TabIndex = 17;
      this.txtMaxLateDays.KeyPress += new KeyPressEventHandler(this.numericOnly_KeyPress1);
      this.lblOf.AutoSize = true;
      this.lblOf.Location = new Point(312, 450);
      this.lblOf.Name = "lblOf";
      this.lblOf.Size = new Size(27, 13);
      this.lblOf.TabIndex = 31;
      this.lblOf.Text = "% of";
      this.txtLateFeePlus.Location = new Point(207, 474);
      this.txtLateFeePlus.MaxLength = 10;
      this.txtLateFeePlus.Name = "txtLateFeePlus";
      this.txtLateFeePlus.ShortcutsEnabled = false;
      this.txtLateFeePlus.Size = new Size(100, 20);
      this.txtLateFeePlus.TabIndex = 15;
      this.txtLateFeePlus.Enter += new EventHandler(this.txtLateFeePlus_Enter);
      this.txtLateFeePlus.KeyPress += new KeyPressEventHandler(this.numericOnly_KeyPress);
      this.txtLateFeePlus.Leave += new EventHandler(this.txtLateFeePlus_Leave);
      this.lblPlus.AutoSize = true;
      this.lblPlus.Location = new Point(183, 477);
      this.lblPlus.Name = "lblPlus";
      this.lblPlus.Size = new Size(22, 13);
      this.lblPlus.TabIndex = 27;
      this.lblPlus.Text = "+ $";
      this.txtLateFee.Location = new Point(207, 447);
      this.txtLateFee.MaxLength = 10;
      this.txtLateFee.Name = "txtLateFee";
      this.txtLateFee.ShortcutsEnabled = false;
      this.txtLateFee.Size = new Size(100, 20);
      this.txtLateFee.TabIndex = 13;
      this.txtLateFee.Enter += new EventHandler(this.txtLateFee_Enter);
      this.txtLateFee.KeyPress += new KeyPressEventHandler(this.numericOnly_KeyPress);
      this.txtLateFee.Leave += new EventHandler(this.txtLateFee_Leave);
      this.txtDateFieldLateFee.Location = new Point(284, 225);
      this.txtDateFieldLateFee.MaxLength = 64;
      this.txtDateFieldLateFee.Name = "txtDateFieldLateFee";
      this.txtDateFieldLateFee.ReadOnly = true;
      this.txtDateFieldLateFee.Size = new Size(92, 20);
      this.txtDateFieldLateFee.TabIndex = 9;
      this.chkLatestConditionsIssuedDate.AutoSize = true;
      this.chkLatestConditionsIssuedDate.Location = new Point(207, 202);
      this.chkLatestConditionsIssuedDate.Name = "chkLatestConditionsIssuedDate";
      this.chkLatestConditionsIssuedDate.Size = new Size(141, 17);
      this.chkLatestConditionsIssuedDate.TabIndex = 7;
      this.chkLatestConditionsIssuedDate.Text = "Latest Cond's Issued Date";
      this.chkLatestConditionsIssuedDate.UseVisualStyleBackColor = true;
      this.chkDeliveryExpirationDate.AutoSize = true;
      this.chkDeliveryExpirationDate.Location = new Point(207, 177);
      this.chkDeliveryExpirationDate.Name = "chkDeliveryExpirationDate";
      this.chkDeliveryExpirationDate.Size = new Size(139, 17);
      this.chkDeliveryExpirationDate.TabIndex = 6;
      this.chkDeliveryExpirationDate.Text = "Delivery Expiration Date";
      this.chkDeliveryExpirationDate.UseVisualStyleBackColor = true;
      this.chkConditionsReceivedDate.AutoSize = true;
      this.chkConditionsReceivedDate.Location = new Point(207, 152);
      this.chkConditionsReceivedDate.Name = "chkConditionsReceivedDate";
      this.chkConditionsReceivedDate.Size = new Size(142, 17);
      this.chkConditionsReceivedDate.TabIndex = 5;
      this.chkConditionsReceivedDate.Text = "Purchase Approval Date";
      this.chkConditionsReceivedDate.UseVisualStyleBackColor = true;
      this.chkInitialSuspenseDate.AutoSize = true;
      this.chkInitialSuspenseDate.Location = new Point(207, (int) sbyte.MaxValue);
      this.chkInitialSuspenseDate.Name = "chkInitialSuspenseDate";
      this.chkInitialSuspenseDate.Size = new Size(147, 17);
      this.chkInitialSuspenseDate.TabIndex = 4;
      this.chkInitialSuspenseDate.Text = "Purchase Suspense Date";
      this.chkInitialSuspenseDate.UseVisualStyleBackColor = true;
      this.lblLaterOf.AutoSize = true;
      this.lblLaterOf.Location = new Point(204, 102);
      this.lblLaterOf.Name = "lblLaterOf";
      this.lblLaterOf.Size = new Size(68, 13);
      this.lblLaterOf.TabIndex = 14;
      this.lblLaterOf.Text = "The Later of:";
      this.cmbGracePeriodUses.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbGracePeriodUses.FormattingEnabled = true;
      this.cmbGracePeriodUses.Items.AddRange(new object[3]
      {
        (object) "Week Days",
        (object) "Calendar Days",
        (object) "Company Calendar"
      });
      this.cmbGracePeriodUses.Location = new Point(207, 50);
      this.cmbGracePeriodUses.Name = "cmbGracePeriodUses";
      this.cmbGracePeriodUses.Size = new Size(167, 21);
      this.cmbGracePeriodUses.TabIndex = 2;
      this.txtGracePeriodDays.Location = new Point(207, 25);
      this.txtGracePeriodDays.MaxLength = 9;
      this.txtGracePeriodDays.Name = "txtGracePeriodDays";
      this.txtGracePeriodDays.ShortcutsEnabled = false;
      this.txtGracePeriodDays.Size = new Size(100, 20);
      this.txtGracePeriodDays.TabIndex = 1;
      this.txtGracePeriodDays.KeyPress += new KeyPressEventHandler(this.numericOnly_KeyPress1);
      this.lblMaxLateDays.AutoSize = true;
      this.lblMaxLateDays.Location = new Point(27, 539);
      this.lblMaxLateDays.Name = "lblMaxLateDays";
      this.lblMaxLateDays.Size = new Size(102, 13);
      this.lblMaxLateDays.TabIndex = 10;
      this.lblMaxLateDays.Text = "Maximum Late Days";
      this.lblLateFee.AutoSize = true;
      this.lblLateFee.Location = new Point(74, 450);
      this.lblLateFee.Name = "lblLateFee";
      this.lblLateFee.Size = new Size(89, 13);
      this.lblLateFee.TabIndex = 8;
      this.lblLateFee.Text = "Late Fee Percent";
      this.lblFeeHandled.AutoSize = true;
      this.lblFeeHandled.Location = new Point(27, 424);
      this.lblFeeHandled.Name = "lblFeeHandled";
      this.lblFeeHandled.Size = new Size(83, 13);
      this.lblFeeHandled.TabIndex = 7;
      this.lblFeeHandled.Text = "Fee Handled As";
      this.lblIncludeDayCleared.AutoSize = true;
      this.lblIncludeDayCleared.Location = new Point(27, 399);
      this.lblIncludeDayCleared.Name = "lblIncludeDayCleared";
      this.lblIncludeDayCleared.Size = new Size(163, 13);
      this.lblIncludeDayCleared.TabIndex = 6;
      this.lblIncludeDayCleared.Text = "Include Day Cleared as Late Day";
      this.lblStartonWeekend.AutoSize = true;
      this.lblStartonWeekend.Location = new Point(27, 374);
      this.lblStartonWeekend.Name = "lblStartonWeekend";
      this.lblStartonWeekend.Size = new Size(170, 13);
      this.lblStartonWeekend.TabIndex = 5;
      this.lblStartonWeekend.Text = "Late Fee Can Start on a Weekend";
      this.lblCalculateAs.AutoSize = true;
      this.lblCalculateAs.Location = new Point(27, 509);
      this.lblCalculateAs.Name = "lblCalculateAs";
      this.lblCalculateAs.Size = new Size(66, 13);
      this.lblCalculateAs.TabIndex = 9;
      this.lblCalculateAs.Text = "Calculate As";
      this.lblGracePeriodStarts.AutoSize = true;
      this.lblGracePeriodStarts.Location = new Point(27, 79);
      this.lblGracePeriodStarts.Name = "lblGracePeriodStarts";
      this.lblGracePeriodStarts.Size = new Size(99, 13);
      this.lblGracePeriodStarts.TabIndex = 2;
      this.lblGracePeriodStarts.Text = "Grace Period Starts";
      this.lblGracePeriodUses.AutoSize = true;
      this.lblGracePeriodUses.Location = new Point(27, 53);
      this.lblGracePeriodUses.Name = "lblGracePeriodUses";
      this.lblGracePeriodUses.Size = new Size(96, 13);
      this.lblGracePeriodUses.TabIndex = 1;
      this.lblGracePeriodUses.Text = "Grace Period Uses";
      this.lblGracePeriodDays.AutoSize = true;
      this.lblGracePeriodDays.Location = new Point(27, 28);
      this.lblGracePeriodDays.Name = "lblGracePeriodDays";
      this.lblGracePeriodDays.Size = new Size(145, 13);
      this.lblGracePeriodDays.TabIndex = 0;
      this.lblGracePeriodDays.Text = "Late Fee Grace Period (days)";
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.Location = new Point(253, 697);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 18;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(347, 697);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 19;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.imgList.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imgList.ImageStream");
      this.imgList.TransparentColor = Color.Transparent;
      this.imgList.Images.SetKeyName(0, "picSearch");
      this.imgList.Images.SetKeyName(1, "picSearchMouseOver");
      this.imgList.Images.SetKeyName(2, "picSearchDisabled");
      this.imgList.Images.SetKeyName(3, "picOtherDate");
      this.imgList.Images.SetKeyName(4, "picOtherDateMouseOver");
      this.imgList.Images.SetKeyName(5, "picOtherDateDisabled");
      this.pnlCompanySettings.Anchor = AnchorStyles.Left;
      this.pnlCompanySettings.Controls.Add((Control) this.rdbTPOSpecific);
      this.pnlCompanySettings.Controls.Add((Control) this.rdbGlobal);
      this.pnlCompanySettings.Location = new Point(0, 38);
      this.pnlCompanySettings.Name = "pnlCompanySettings";
      this.pnlCompanySettings.Size = new Size(434, 58);
      this.pnlCompanySettings.TabIndex = 1;
      this.rdbTPOSpecific.AutoSize = true;
      this.rdbTPOSpecific.Location = new Point(30, 32);
      this.rdbTPOSpecific.Name = "rdbTPOSpecific";
      this.rdbTPOSpecific.Size = new Size(129, 17);
      this.rdbTPOSpecific.TabIndex = 1;
      this.rdbTPOSpecific.TabStop = true;
      this.rdbTPOSpecific.Text = "TPO Specific Settings";
      this.rdbTPOSpecific.UseVisualStyleBackColor = true;
      this.rdbTPOSpecific.CheckedChanged += new EventHandler(this.rdbTPOSpecific_CheckedChanged);
      this.rdbGlobal.AutoSize = true;
      this.rdbGlobal.Location = new Point(30, 8);
      this.rdbGlobal.Name = "rdbGlobal";
      this.rdbGlobal.Size = new Size(121, 17);
      this.rdbGlobal.TabIndex = 0;
      this.rdbGlobal.TabStop = true;
      this.rdbGlobal.Text = "Global TPO Settings";
      this.rdbGlobal.UseVisualStyleBackColor = true;
      this.rdbGlobal.CheckedChanged += new EventHandler(this.rdbGlobal_CheckedChanged);
      this.pnlInstruction.Controls.Add((Control) this.lblInstruction);
      this.pnlInstruction.Location = new Point(0, 0);
      this.pnlInstruction.Name = "pnlInstruction";
      this.pnlInstruction.Size = new Size(434, 37);
      this.pnlInstruction.TabIndex = 20;
      this.lblInstruction.Location = new Point(27, 5);
      this.lblInstruction.Name = "lblInstruction";
      this.lblInstruction.Size = new Size(372, 29);
      this.lblInstruction.TabIndex = 0;
      this.lblInstruction.Text = "Establish Suspense Late Fees for Correspondent Loans.  Select Grace Period parameters, select Day Cleared date and specify Fee Calculations.";
      this.label5.AutoSize = true;
      this.label5.Location = new Point(74, 477);
      this.label5.Name = "label5";
      this.label5.Size = new Size(84, 13);
      this.label5.TabIndex = 42;
      this.label5.Text = "Late Fee Dollars";
      this.AcceptButton = (IButtonControl) this.btnOK;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(434, 728);
      this.Controls.Add((Control) this.pnlInstruction);
      this.Controls.Add((Control) this.pnlCompanySettings);
      this.Controls.Add((Control) this.pnlLateFeeSettings);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOK);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (LateFeeSettings);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Edit Late Fee Settings";
      this.pnlLateFeeSettings.ResumeLayout(false);
      this.pnlLateFeeSettings.PerformLayout();
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      ((ISupportInitialize) this.picOtherDate).EndInit();
      ((ISupportInitialize) this.picSearch).EndInit();
      this.pnlCalculateAs.ResumeLayout(false);
      this.pnlCalculateAs.PerformLayout();
      this.pnlFeeHandledAs.ResumeLayout(false);
      this.pnlFeeHandledAs.PerformLayout();
      this.pnlLateDay.ResumeLayout(false);
      this.pnlLateDay.PerformLayout();
      this.pnlWeekend.ResumeLayout(false);
      this.pnlWeekend.PerformLayout();
      this.pnlGracePeriod.ResumeLayout(false);
      this.pnlGracePeriod.PerformLayout();
      this.pnlCompanySettings.ResumeLayout(false);
      this.pnlCompanySettings.PerformLayout();
      this.pnlInstruction.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
