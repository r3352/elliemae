// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.WorstCasePricingColumn
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.InputEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Trading;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.Log
{
  public class WorstCasePricingColumn : UserControl
  {
    private Hashtable dataTables = new Hashtable();
    private LoanDataMgr loanMgr;
    private bool readOnly;
    private string[] rateOptions = new string[0];
    private string[] priceOptions = new string[0];
    private string[] marginOptions = new string[0];
    private string[] cmtTypeOptions = new string[0];
    private PopupBusinessRules popupRules;
    private bool isFirstLoading = true;
    private bool hideBR;
    private bool hideBP;
    private bool hideEX;
    private bool hideBM;
    private bool hideRL;
    private bool hideCP;
    private ResourceManager UIresources;
    private LockRequestCalculator lockCalculator = new LockRequestCalculator(Session.SessionObjects, Session.LoanDataMgr.LoanData);
    private bool use10Digits;
    private bool isDirty;
    private string[] profitabilityOptions = new string[0];
    private Hashtable companySettings;
    private LockExtensionUtils lockExtensionUtils;
    private string[] locktypeOptions = new string[0];
    public bool IsDeliveryPanelVisible;
    private IContainer components;
    private GroupContainer gcPrice;
    private Panel pnlLoanProgram;
    private TextBox textBoxLoanProgram;
    private Label labelLoanProgram;
    private Panel pnlLockInfo;
    private GroupBox groupBox10;
    private Label label40;
    private Label label41;
    private TextBox txtLockDays;
    private Panel pnlRateRequested;
    private Label label7;
    private TextBox textBoxRate22;
    private Label label6;
    private TextBox textBoxRate23;
    private Panel pnlBRExpand;
    private TextBox textBoxRate21;
    private ComboBox comboBoxRate7;
    private ComboBox comboBoxRate20;
    private TextBox textBoxRate8;
    private TextBox textBoxRate20;
    private ComboBox comboBoxRate8;
    private ComboBox comboBoxRate19;
    private TextBox textBoxRate9;
    private TextBox textBoxRate19;
    private ComboBox comboBoxRate9;
    private ComboBox comboBoxRate18;
    private TextBox textBoxRate10;
    private TextBox textBoxRate18;
    private ComboBox comboBoxRate10;
    private ComboBox comboBoxRate17;
    private TextBox textBoxRate11;
    private TextBox textBoxRate17;
    private ComboBox comboBoxRate11;
    private ComboBox comboBoxRate16;
    private TextBox textBoxRate12;
    private TextBox textBoxRate16;
    private ComboBox comboBoxRate12;
    private ComboBox comboBoxRate15;
    private TextBox textBoxRate13;
    private TextBox textBoxRate15;
    private ComboBox comboBoxRate13;
    private ComboBox comboBoxRate14;
    private TextBox textBoxRate14;
    private Panel pnlBR;
    private PictureBox picBRZoomOut;
    private PictureBox picBRZoomIn;
    private Label label8;
    private TextBox textBoxRate1;
    private Panel pnlBPExtension;
    private ComboBox comboBoxPrice13;
    private TextBox textBoxPrice14;
    private ComboBox comboBoxPrice14;
    private TextBox textBoxPrice15;
    private ComboBox comboBoxPrice9;
    private TextBox textBoxPrice10;
    private ComboBox comboBoxPrice10;
    private TextBox textBoxPrice11;
    private ComboBox comboBoxPrice11;
    private TextBox textBoxPrice12;
    private ComboBox comboBoxPrice12;
    private TextBox textBoxPrice13;
    private ComboBox comboBoxPrice5;
    private TextBox textBoxPrice6;
    private ComboBox comboBoxPrice6;
    private TextBox textBoxPrice7;
    private ComboBox comboBoxPrice7;
    private TextBox textBoxPrice8;
    private ComboBox comboBoxPrice8;
    private TextBox textBoxPrice9;
    private Panel pnlBP;
    private ComboBox comboBoxPrice1;
    private TextBox textBoxPrice2;
    private ComboBox comboBoxPrice2;
    private Label label11;
    private TextBox textBoxPrice1;
    private TextBox textBoxPrice3;
    private ComboBox comboBoxPrice3;
    private TextBox textBoxPrice4;
    private ComboBox comboBoxPrice4;
    private TextBox textBoxPrice5;
    private ComboBox comboBoxRate1;
    private TextBox textBoxRate2;
    private ComboBox comboBoxRate2;
    private TextBox textBoxRate3;
    private ComboBox comboBoxRate3;
    private TextBox textBoxRate4;
    private ComboBox comboBoxRate4;
    private TextBox textBoxRate5;
    private ComboBox comboBoxRate5;
    private TextBox textBoxRate6;
    private ComboBox comboBoxRate6;
    private TextBox textBoxRate7;
    private PictureBox picBPZoomOut;
    private PictureBox picBPZoomIn;
    private ComboBox comboBoxPrice19;
    private TextBox textBoxPrice20;
    private ComboBox comboBoxPrice20;
    private TextBox textBoxPrice21;
    private ComboBox comboBoxPrice15;
    private TextBox textBoxPrice16;
    private ComboBox comboBoxPrice16;
    private TextBox textBoxPrice17;
    private ComboBox comboBoxPrice17;
    private TextBox textBoxPrice18;
    private ComboBox comboBoxPrice18;
    private TextBox textBoxPrice19;
    private Label label3;
    private Panel pnlDeliveryType;
    private Label label2;
    private Label label1;
    private TextBox txtRateSheetID;
    private Label label77;
    private TextBox textBox6;
    private Label label75;
    private TextBox textBox2;
    private Label label76;
    private TextBox textBox4;
    private Button btnClear;
    private ToolTip toolTipField;
    private TextBox comboBoxDeliveryType;
    private ComboBox cBoxCmtType;
    private Label labelCt;
    private DatePicker dtLastRateSetDate;
    private DatePicker txtLockDate;
    private DatePicker txtExpireDate;
    private Label lblLockExpirationDate;
    private Panel pnlLockExtension;
    private Label label17;
    private TextBox txtPriceAdj;
    private DatePicker txtNewLockExpDate;
    private Label label13;
    private TextBox txtDaysToExtent;
    private Label label12;
    private DatePicker txtCurLockExpDate;
    private Label label5;
    private Panel pnlComment;
    private Label label59;
    private TextBox txtComments;
    private Panel pnlNetBM;
    private Label label31;
    private TextBox txtRequestSRP;
    private Label label15;
    private TextBox textBoxMargin28;
    private GroupBox groupBox2;
    private Label label16;
    private TextBox textBoxMargin29;
    private Panel pnlBMExpand;
    private ComboBox comboBoxMargin20;
    private ComboBox comboBoxMargin5;
    private TextBox textBoxMargin7;
    private TextBox textBoxMargin21;
    private ComboBox comboBoxMargin6;
    private ComboBox comboBoxMargin7;
    private TextBox textBoxMargin6;
    private TextBox textBoxMargin8;
    private TextBox textBoxMargin20;
    private ComboBox comboBoxMargin8;
    private ComboBox comboBoxMargin19;
    private TextBox textBoxMargin9;
    private TextBox textBoxMargin19;
    private ComboBox comboBoxMargin9;
    private ComboBox comboBoxMargin18;
    private TextBox textBoxMargin10;
    private TextBox textBoxMargin18;
    private ComboBox comboBoxMargin10;
    private ComboBox comboBoxMargin17;
    private TextBox textBoxMargin11;
    private TextBox textBoxMargin17;
    private ComboBox comboBoxMargin11;
    private ComboBox comboBoxMargin16;
    private TextBox textBoxMargin12;
    private TextBox textBoxMargin16;
    private ComboBox comboBoxMargin12;
    private ComboBox comboBoxMargin15;
    private TextBox textBoxMargin13;
    private TextBox textBoxMargin15;
    private ComboBox comboBoxMargin13;
    private ComboBox comboBoxMargin14;
    private TextBox textBoxMargin14;
    private Panel pnlBM;
    private ComboBox comboBoxMargin1;
    private TextBox textBoxMargin2;
    private ComboBox comboBoxMargin2;
    private TextBox textBoxMargin3;
    private ComboBox comboBoxMargin3;
    private TextBox textBoxMargin4;
    private ComboBox comboBoxMargin4;
    private TextBox textBoxMargin5;
    private PictureBox picBMZoomOut;
    private PictureBox picBMZoomIn;
    private Label label14;
    private TextBox textBoxMargin1;
    private Panel pnlNetPrice;
    private Label label10;
    private TextBox textBoxPrice22;
    private GroupBox groupBox5;
    private Label label9;
    private TextBox textBoxPrice23;
    private Panel pnlCpExpand;
    private Label label42;
    private TextBox textBoxCpa10Desc;
    private TextBox textBoxCpa10;
    private Label label43;
    private TextBox textBoxCpa9Desc;
    private TextBox textBoxCpa9;
    private Label label44;
    private TextBox textBoxCpa8Desc;
    private TextBox textBoxCpa8;
    private Label label45;
    private TextBox textBoxCpa7Desc;
    private TextBox textBoxCpa7;
    private Label label46;
    private TextBox textBoxCpa6Desc;
    private TextBox textBoxCpa6;
    private Label label47;
    private TextBox textBoxCpa5Desc;
    private TextBox textBoxCpa5;
    private Label label48;
    private TextBox textBoxCpa4Desc;
    private TextBox textBoxCpa4;
    private Panel pnlCp;
    private Label label49;
    private Label label50;
    private Label label51;
    private TextBox textBoxCpa3Desc;
    private TextBox textBoxCpa3;
    private TextBox textBoxCpa2Desc;
    private TextBox textBoxCpa2;
    private TextBox textBoxCpa1Desc;
    private TextBox textBoxCpa1;
    private PictureBox picCPZoomOut;
    private PictureBox picCPZoomIn;
    private Label label52;
    private Panel pnlRlExpand;
    private Label label28;
    private TextBox textBoxReLock10Desc;
    private TextBox textBoxReLock10;
    private Label label29;
    private TextBox textBoxReLock9Desc;
    private TextBox textBoxReLock9;
    private Label label30;
    private TextBox textBoxReLock8Desc;
    private TextBox textBoxReLock8;
    private Label label32;
    private TextBox textBoxReLock7Desc;
    private TextBox textBoxReLock7;
    private Label label33;
    private TextBox textBoxReLock6Desc;
    private TextBox textBoxReLock6;
    private Label label34;
    private TextBox textBoxReLock5Desc;
    private TextBox textBoxReLock5;
    private Label label35;
    private TextBox textBoxReLock4Desc;
    private TextBox textBoxReLock4;
    private Panel pnlRl;
    private Label label36;
    private Label label37;
    private Label label38;
    private TextBox textBoxReLock3Desc;
    private TextBox textBoxReLock3;
    private TextBox textBoxReLock2Desc;
    private TextBox textBoxReLock2;
    private TextBox textBoxReLock1Desc;
    private TextBox textBoxReLock1;
    private PictureBox picRLZoomOut;
    private PictureBox picRLZoomIn;
    private Label label39;
    private Panel pnlExExpand;
    private Label label97;
    private TextBox textBoxExtension10Desc;
    private TextBox textBoxExtension10;
    private Label label95;
    private TextBox textBoxExtension9Desc;
    private TextBox textBoxExtension9;
    private Label label96;
    private TextBox textBoxExtension8Desc;
    private TextBox textBoxExtension8;
    private Label label93;
    private TextBox textBoxExtension7Desc;
    private TextBox textBoxExtension7;
    private Label label94;
    private TextBox textBoxExtension6Desc;
    private TextBox textBoxExtension6;
    private Label label92;
    private TextBox textBoxExtension5Desc;
    private TextBox textBoxExtension5;
    private Label label87;
    private TextBox textBoxExtension4Desc;
    private TextBox textBoxExtension4;
    private Panel pnlEx;
    private Label label86;
    private Label label85;
    private Label label84;
    private TextBox textBoxExtension3Desc;
    private TextBox textBoxExtension3;
    private TextBox textBoxExtension2Desc;
    private TextBox textBoxExtension2;
    private TextBox textBoxExtension1Desc;
    private TextBox textBoxExtension1;
    private PictureBox picEXZoomOut;
    private PictureBox picEXZoomIn;
    private Label label82;

    public event EventHandler ZoomButtonClicked;

    public event EventHandler ClearButtonClicked;

    public bool IsDirty
    {
      get => this.isDirty;
      set => this.isDirty = value;
    }

    public WorstCasePricingColumn(
      bool readOnly,
      Hashtable inputDataTable,
      bool isCurrentPricing,
      bool isLockExtension)
    {
      this.use10Digits = Session.SessionObjects.Use10DecimalDigitInLockRequestSecondaryTradeAreas;
      this.readOnly = readOnly;
      this.loanMgr = Session.LoanDataMgr;
      this.InitializeComponent();
      this.FieldDataTables = inputDataTable;
      this.companySettings = Session.SessionObjects.GetCompanySettingsFromCache("POLICIES");
      this.lockExtensionUtils = new LockExtensionUtils(Session.SessionObjects, this.loanMgr.LoanData);
      this.UIresources = new ResourceManager(typeof (LockForm));
      this.picBMZoomOut.Left = this.picBMZoomIn.Left;
      this.picBMZoomOut.Top = this.picBMZoomIn.Top;
      this.picBPZoomOut.Left = this.picBPZoomIn.Left;
      this.picBPZoomOut.Top = this.picBPZoomIn.Top;
      this.picBRZoomOut.Left = this.picBRZoomIn.Left;
      this.picBRZoomOut.Top = this.picBRZoomIn.Top;
      this.picEXZoomOut.Left = this.picEXZoomIn.Left;
      this.picEXZoomOut.Top = this.picEXZoomIn.Top;
      this.picRLZoomOut.Left = this.picEXZoomIn.Left;
      this.picRLZoomOut.Top = this.picEXZoomIn.Top;
      this.picCPZoomOut.Left = this.picCPZoomIn.Left;
      this.picCPZoomOut.Top = this.picCPZoomIn.Top;
      ResourceManager resources = new ResourceManager(typeof (LockForm));
      this.popupRules = new PopupBusinessRules(this.loanMgr.LoanData, resources, (Image) resources.GetObject("pboxAsterisk.Image"), (Image) resources.GetObject("pboxDownArrow.Image"), Session.DefaultInstance);
      object[] allSecondaryFields = Session.ConfigurationManager.GetAllSecondaryFields();
      if (allSecondaryFields != null && allSecondaryFields.Length >= 3)
      {
        if (allSecondaryFields[0] != null)
          this.rateOptions = (string[]) allSecondaryFields[0];
        if (allSecondaryFields[1] != null)
          this.priceOptions = (string[]) allSecondaryFields[1];
        if (allSecondaryFields[2] != null)
          this.marginOptions = (string[]) allSecondaryFields[2];
        if (allSecondaryFields[4] != null)
          this.profitabilityOptions = (string[]) allSecondaryFields[4];
        if (allSecondaryFields[5] != null)
          this.locktypeOptions = (string[]) allSecondaryFields[5];
      }
      this.IsDeliveryPanelVisible = this.setCommitmentTypeOptions();
      this.pnlDeliveryType.Visible = this.IsDeliveryPanelVisible;
      this.populateDropdownOptions(this.Controls);
      this.populateFieldValues(this.Controls);
      this.populateToolTips(this.Controls);
      this.isFirstLoading = false;
      this.hideBR = !(this.getField("2420") != "") && this.getNumField("2421") == 0.0;
      this.hideBP = !(this.getField("2110") != "") && this.getNumField("2111") == 0.0;
      this.hideEX = !(this.getField("3460") != "") && this.getNumField("3461") == 0.0;
      this.hideBM = !(this.getField("2656") != "") && this.getNumField("2657") == 0.0;
      this.hideRL = !(this.getField("4262") != "") && this.getNumField("4263") == 0.0;
      this.hideCP = !(this.getField("4342") != "") && this.getNumField("4343") == 0.0;
      this.zoomInAndOut();
      this.isDirty = false;
      if (!isLockExtension | isCurrentPricing)
      {
        this.lblLockExpirationDate.Text = "Lock Expiration Date";
        this.pnlLockExtension.Visible = false;
        if (isLockExtension)
          return;
        this.pnlLockInfo.Height -= this.pnlLockExtension.Height;
        this.gcPrice.Height -= this.pnlLockExtension.Height;
        this.Height -= this.pnlLockExtension.Height;
      }
      else
      {
        if (!isLockExtension)
          return;
        this.lblLockExpirationDate.Text = "Original Lock Expiration Date";
        this.pnlLockExtension.Visible = true;
        this.txtLockDate.Enabled = false;
        this.txtLockDays.Enabled = false;
        this.txtExpireDate.Enabled = false;
        this.txtCurLockExpDate.Enabled = false;
      }
    }

    public Hashtable FieldDataTables
    {
      get => this.dataTables;
      set => this.dataTables = value;
    }

    internal void RefreshScreen()
    {
      if (this.dataTables != null && !this.readOnly)
        this.lockCalculator.PerformSnapshotCalculations(this.dataTables);
      this.populateFieldValues(this.Controls);
      if (this.readOnly)
      {
        this.toolTipField.RemoveAll();
        this.populateToolTips(this.Controls);
      }
      this.isDirty = false;
    }

    public void RecalcLockExpirationDate()
    {
      this.calculateLockDate("432", this.txtExpireDate, this.txtLockDate, this.txtLockDays);
      this.setField("2091", this.txtExpireDate.Text.Trim());
    }

    public void setTitle(string title) => this.gcPrice.Text = title;

    private void setHeight()
    {
      this.Height = this.pnlBR.Bottom + this.pnlBP.Height + this.pnlEx.Height + this.pnlBM.Height + this.pnlRateRequested.Height + this.pnlComment.Height + this.pnlNetBM.Height + this.pnlNetPrice.Height + this.pnlRl.Height + this.pnlCp.Height;
      if (!this.hideBR)
        this.Height += this.pnlBRExpand.Height;
      if (!this.hideBP)
        this.Height += this.pnlBPExtension.Height;
      if (!this.hideBM)
        this.Height += this.pnlBMExpand.Height;
      if (!this.hideEX)
        this.Height += this.pnlExExpand.Height;
      if (!this.hideRL)
        this.Height += this.pnlRlExpand.Height;
      if (this.hideCP)
        return;
      this.Height += this.pnlCpExpand.Height;
    }

    private void populateFieldValues(Control.ControlCollection cs)
    {
      foreach (Control c in (ArrangedElementCollection) cs)
      {
        switch (c)
        {
          case TextBox _:
          case ComboBox _:
          case DatePicker _:
            if (c.Tag != null)
            {
              string str1 = c.Tag.ToString();
              if (!(str1 == string.Empty))
              {
                c.Text = this.defaultValue(str1);
                if (this.readOnly && c is TextBox)
                {
                  ((TextBoxBase) c).ReadOnly = true;
                  c.BackColor = Color.WhiteSmoke;
                }
                string str2 = string.Empty;
                if (this.dataTables != null && this.dataTables.ContainsKey((object) str1))
                {
                  if (LockRequestUtils.IsLockRequestSecondary10DigitFormatingFields(str1))
                  {
                    if (this.dataTables[(object) str1].ToString().Trim() == string.Empty)
                    {
                      str2 = "";
                    }
                    else
                    {
                      Decimal num = Utils.ParseDecimal((object) this.dataTables[(object) str1].ToString(), false, 10);
                      str2 = !Session.SessionObjects.Use10DecimalDigitInLockRequestSecondaryTradeAreas ? num.ToString("N3") : num.ToString("N10");
                    }
                  }
                  else if (c.Name.StartsWith("textBoxReLock") && !c.Name.Contains("Desc") || c.Name.StartsWith("textBoxCpa") && !c.Name.Contains("Desc"))
                  {
                    if (this.dataTables[(object) str1].ToString().Trim() == string.Empty)
                    {
                      str2 = "";
                    }
                    else
                    {
                      Decimal num = Utils.ParseDecimal((object) this.dataTables[(object) str1].ToString(), false, 10);
                      str2 = !Session.SessionObjects.Use10DecimalDigitInLockRequestSecondaryTradeAreas ? num.ToString("N3") : num.ToString("N10");
                    }
                  }
                  else
                    str2 = this.dataTables[(object) str1].ToString();
                }
                switch (c)
                {
                  case TextBox _:
                    TextBox ctrl1 = (TextBox) c;
                    ctrl1.Text = str2;
                    if (this.isFirstLoading)
                    {
                      this.popupRules.SetBusinessRules((object) ctrl1, str1);
                      continue;
                    }
                    continue;
                  case ComboBox _:
                    ComboBox ctrl2 = (ComboBox) c;
                    ctrl2.Text = str2;
                    if (this.isFirstLoading)
                    {
                      this.popupRules.SetBusinessRules((object) ctrl2, str1);
                      continue;
                    }
                    continue;
                  case DatePicker _:
                    DatePicker ctrl3 = (DatePicker) c;
                    ctrl3.Text = str2;
                    if (this.isFirstLoading)
                    {
                      this.popupRules.SetBusinessRules((object) ctrl3, str1);
                      continue;
                    }
                    continue;
                  default:
                    continue;
                }
              }
              else
                continue;
            }
            else
              continue;
          default:
            this.populateFieldValues(c.Controls);
            continue;
        }
      }
      this.btnClear.Enabled = !this.readOnly;
    }

    private string defaultValue(string id)
    {
      return id == "2287" || id == "3832" || id == "2288" || id == "2286" || id == "2232" || id == "3714" || id == "4187" || id == "3965" ? this.getField(id) : string.Empty;
    }

    private string getLoanFieldValue(string id)
    {
      return Session.LoanData.GetFieldDefinition(id).ToDisplayValue(Session.LoanData.GetField(id));
    }

    private void populateDropdownOptions(Control.ControlCollection cs)
    {
      foreach (Control c in (ArrangedElementCollection) cs)
      {
        if (c is ComboBox)
        {
          ComboBox comboBox = (ComboBox) c;
          if (comboBox != null)
          {
            string name = comboBox.Name;
            if (name == "comboBoxCmtType")
              comboBox.Items.AddRange((object[]) this.cmtTypeOptions);
            else if (name.StartsWith("comboBoxRate") && this.rateOptions != null)
              comboBox.Items.AddRange((object[]) this.rateOptions);
            else if (name.StartsWith("comboBoxMargin") && this.marginOptions != null)
              comboBox.Items.AddRange((object[]) this.marginOptions);
            else if (name.StartsWith("comboBoxLockRate"))
              comboBox.Items.AddRange((object[]) this.locktypeOptions);
            else if (this.priceOptions != null)
              comboBox.Items.AddRange((object[]) this.priceOptions);
          }
        }
        else
          this.populateDropdownOptions(c.Controls);
      }
    }

    private void displayFieldId(string fieldId)
    {
      Session.Application.GetService<IStatusDisplay>().DisplayFieldID(fieldId);
    }

    private void textField_Enter(object sender, EventArgs e)
    {
      Control control = (Control) sender;
      if (control == null || control.Tag == null)
        return;
      this.displayFieldId(control.Tag.ToString());
    }

    private void comboBoxField_Enter(object sender, EventArgs e)
    {
      ComboBox comboBox = (ComboBox) sender;
      if (comboBox == null || comboBox.Tag == null)
        return;
      this.displayFieldId(comboBox.Tag.ToString());
    }

    public Hashtable ReadFieldValues()
    {
      Hashtable newData = new Hashtable();
      this.readFieldValues(this.Controls, newData);
      this.setField("2029", Session.UserInfo.FullName);
      this.setField("2030", Session.UserInfo.FullName);
      return newData;
    }

    public Hashtable SetFieldValues(Hashtable newData)
    {
      if (newData == null)
        newData = new Hashtable();
      string dataTable = (string) this.dataTables[(object) "2866"];
      this.dataTables = newData;
      this.readFieldValues(this.Controls, this.dataTables);
      this.setField("2029", Session.UserInfo.FullName);
      this.setField("2030", Session.UserInfo.FullName);
      if (dataTable != null)
        this.setField("2866", dataTable);
      return this.dataTables;
    }

    private void readFieldValues(Control.ControlCollection cs, Hashtable newData)
    {
      foreach (Control c in (ArrangedElementCollection) cs)
      {
        switch (c)
        {
          case TextBox _:
            TextBox textBox = (TextBox) c;
            if (textBox.Tag != null)
            {
              string key = textBox.Tag.ToString();
              if ((textBox.Visible || !(key == "2222") && !(key == "2151")) && !(key == string.Empty))
              {
                if (newData.ContainsKey((object) key))
                {
                  newData[(object) key] = (object) textBox.Text.Trim();
                  continue;
                }
                newData.Add((object) key, (object) textBox.Text.Trim());
                continue;
              }
              continue;
            }
            continue;
          case ComboBox _:
            ComboBox comboBox = (ComboBox) c;
            if (comboBox.Tag != null)
            {
              string key = comboBox.Tag.ToString();
              if (!(key == string.Empty))
              {
                if (newData.ContainsKey((object) key))
                {
                  newData[(object) key] = (object) comboBox.Text.Trim();
                  continue;
                }
                newData.Add((object) key, (object) comboBox.Text.Trim());
                continue;
              }
              continue;
            }
            continue;
          case DatePicker _:
            DatePicker datePicker = (DatePicker) c;
            if (datePicker.Tag != null)
            {
              string key = datePicker.Tag.ToString();
              if (!(key == string.Empty))
              {
                if (newData.ContainsKey((object) key))
                {
                  newData[(object) key] = (object) datePicker.Text.Trim();
                  continue;
                }
                newData.Add((object) key, (object) datePicker.Text.Trim());
                continue;
              }
              continue;
            }
            continue;
          default:
            this.readFieldValues(c.Controls, newData);
            continue;
        }
      }
    }

    private void numField_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar.Equals((object) Keys.Delete) || char.IsControl(e.KeyChar))
        return;
      if (!char.IsDigit(e.KeyChar))
      {
        char keyChar = e.KeyChar;
        if (!keyChar.Equals('.'))
        {
          keyChar = e.KeyChar;
          if (!keyChar.Equals('-'))
          {
            e.Handled = true;
            return;
          }
        }
      }
      e.Handled = false;
    }

    private void zipcodeField_KeyUp(object sender, KeyEventArgs e)
    {
      this.formatFieldValue(sender, FieldFormat.ZIPCODE);
    }

    private void stateField_KeyUp(object sender, KeyEventArgs e)
    {
      this.formatFieldValue(sender, FieldFormat.STATE);
    }

    private void numField_KeyUp(object sender, KeyEventArgs e)
    {
      if (LockRequestUtils.IsLockRequestSecondary10DigitFormatingFields(string.Concat(((Control) sender).Tag)))
      {
        if (this.use10Digits)
          this.formatFieldValue(sender, FieldFormat.DECIMAL_10);
        else
          this.formatFieldValue(sender, FieldFormat.DECIMAL_3);
      }
      else
        this.formatFieldValue(sender, FieldFormat.DECIMAL_3);
    }

    private void numFieldDecimal4_KeyUp(object sender, KeyEventArgs e)
    {
      this.formatFieldValue(sender, FieldFormat.DECIMAL_4);
    }

    private void phoneField_KeyUp(object sender, KeyEventArgs e)
    {
      this.formatFieldValue(sender, FieldFormat.PHONE);
    }

    private void formatFieldValue(object sender, FieldFormat format)
    {
      TextBox textBox = (TextBox) sender;
      bool needsUpdate = false;
      string str = Utils.FormatInput(textBox.Text, format, ref needsUpdate);
      if (!needsUpdate)
        return;
      textBox.Text = str;
      textBox.SelectionStart = str.Length;
    }

    private void populateToolTips(Control.ControlCollection cs)
    {
      foreach (Control c in (ArrangedElementCollection) cs)
      {
        switch (c)
        {
          case TextBox _:
            TextBox textBox1 = (TextBox) c;
            if (textBox1 != null && textBox1.Tag != null)
            {
              string caption = textBox1.Tag.ToString();
              if (!(caption == string.Empty))
              {
                if (textBox1.Text.Trim().ToString() != string.Empty && textBox1.Name.StartsWith("COMBOFIELD"))
                  caption = caption + "\r\n" + textBox1.Text.ToString();
                this.toolTipField.SetToolTip((Control) textBox1, caption);
                continue;
              }
              continue;
            }
            continue;
          case ComboBox _:
            ComboBox comboBox = (ComboBox) c;
            if (comboBox != null && comboBox.Tag != null)
            {
              string caption = comboBox.Tag.ToString();
              if (!(caption == string.Empty))
              {
                if (this.readOnly)
                {
                  TextBox textBox2 = new TextBox();
                  textBox2.Name = "COMBOFIELD" + comboBox.Tag.ToString();
                  textBox2.Top = comboBox.Top;
                  textBox2.Left = comboBox.Left;
                  textBox2.Width = comboBox.Width;
                  textBox2.Height = comboBox.Height;
                  textBox2.Text = comboBox.Text.ToString();
                  textBox2.Tag = comboBox.Tag;
                  textBox2.ReadOnly = true;
                  textBox2.BackColor = Color.WhiteSmoke;
                  textBox2.BringToFront();
                  comboBox.Parent.Controls.Add((Control) textBox2);
                  this.toolTipField.SetToolTip((Control) textBox2, caption);
                  comboBox.Visible = false;
                  continue;
                }
                this.toolTipField.SetToolTip((Control) comboBox, caption);
                continue;
              }
              continue;
            }
            continue;
          case DatePicker _:
            DatePicker datePicker = (DatePicker) c;
            if (datePicker != null && datePicker.Tag != null)
            {
              string caption = datePicker.Tag.ToString();
              if (!(caption == string.Empty))
              {
                if (this.readOnly)
                {
                  TextBox textBox3 = new TextBox();
                  textBox3.Name = "COMBOFIELD" + datePicker.Tag.ToString();
                  textBox3.Top = datePicker.Top;
                  textBox3.Left = datePicker.Left;
                  textBox3.Width = datePicker.Width;
                  textBox3.Height = datePicker.Height;
                  textBox3.Text = datePicker.Text.ToString();
                  textBox3.Tag = datePicker.Tag;
                  textBox3.ReadOnly = true;
                  textBox3.BackColor = Color.WhiteSmoke;
                  textBox3.BringToFront();
                  datePicker.Parent.Controls.Add((Control) textBox3);
                  this.toolTipField.SetToolTip((Control) textBox3, caption);
                  datePicker.Visible = false;
                  continue;
                }
                this.toolTipField.SetToolTip((Control) datePicker, caption);
                continue;
              }
              continue;
            }
            continue;
          default:
            this.populateToolTips(c.Controls);
            continue;
        }
      }
    }

    private void numField_Leave(object sender, EventArgs e)
    {
      if (this.readOnly)
        return;
      TextBox ctrl = (TextBox) sender;
      if (ctrl.Tag.ToString() + string.Empty == string.Empty)
        return;
      string str = ctrl.Tag.ToString();
      this.popupRules.RuleValidate((object) ctrl, str);
      if (ctrl.Name.StartsWith("textBoxRate") || ctrl.Name.StartsWith("textBoxPrice") || ctrl.Name.StartsWith("textBoxMargin") || ctrl.Name.StartsWith("textBoxExtension") && !ctrl.Name.Contains("Desc") || ctrl.Name.StartsWith("textBoxReLock") && !ctrl.Name.Contains("Desc") || ctrl.Name.StartsWith("textBoxCpa") && !ctrl.Name.Contains("Desc") || str == "2205" || str == "2276" || str == "2277")
      {
        double num = Utils.ParseDouble((object) ctrl.Text.Trim());
        if (ctrl.Text.Trim() != string.Empty)
        {
          if (LockRequestUtils.IsLockRequestSecondary10DigitFormatingFields(str))
          {
            if (this.use10Digits)
              ctrl.Text = num.ToString("N10");
            else
              ctrl.Text = num.ToString("N3");
          }
          else
            ctrl.Text = num.ToString("N3");
        }
      }
      this.setField(str, ctrl.Text.Trim());
      this.RefreshScreen();
    }

    private double getNumField(string id)
    {
      return this.dataTables == null || !this.dataTables.ContainsKey((object) id) ? 0.0 : Utils.ParseDouble(this.dataTables[(object) id]);
    }

    private void setField(string id, string val)
    {
      if (this.dataTables == null)
        return;
      if (!this.dataTables.ContainsKey((object) id))
        this.dataTables.Add((object) id, (object) val);
      else
        this.dataTables[(object) id] = (object) val;
      this.isDirty = false;
    }

    public string getField(string id)
    {
      return this.dataTables == null || !this.dataTables.ContainsKey((object) id) ? "" : this.dataTables[(object) id].ToString();
    }

    private void strField_Leave(object sender, EventArgs e)
    {
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      if (this.readOnly)
        return;
      string tag;
      string val;
      switch (sender)
      {
        case TextBox _:
          TextBox ctrl1 = (TextBox) sender;
          tag = ctrl1.Tag.ToString();
          this.popupRules.RuleValidate((object) ctrl1, tag);
          val = ctrl1.Text.Trim();
          if (ctrl1.Name == "txtLockDate")
          {
            if (this.txtLockDate.Text != this.getField("2149"))
              this.calculateLockDate("761", this.txtExpireDate, this.txtLockDate, this.txtLockDays);
          }
          else if (ctrl1.Name == "txtExpireDate")
          {
            if (this.txtExpireDate.Text != this.getField("2151"))
            {
              this.calculateLockDate("762", this.txtExpireDate, this.txtLockDate, this.txtLockDays);
              val = ctrl1.Text.Trim();
            }
          }
          else if (ctrl1.Name == "txtLockDays" && this.txtLockDays.Text != this.getField("2150"))
            this.calculateLockDate("432", this.txtExpireDate, this.txtLockDate, this.txtLockDays);
          if (ctrl1.Name == "txtLockDate" || ctrl1.Name == "txtExpireDate" || ctrl1.Name == "txtLockDays")
          {
            this.setField("2089", this.txtLockDate.Text.Trim());
            this.setField("2090", this.txtLockDays.Text.Trim());
            this.setField("2091", this.txtExpireDate.Text.Trim());
            break;
          }
          break;
        case ComboBox _:
          ComboBox ctrl2 = (ComboBox) sender;
          tag = ctrl2.Tag.ToString();
          this.popupRules.RuleValidate((object) ctrl2, tag);
          val = ctrl2.Text.Trim();
          break;
        case DatePicker _:
          DatePicker ctrl3 = (DatePicker) sender;
          if (ctrl3 == null)
            return;
          tag = (string) ctrl3.Tag;
          if ((tag ?? "") == "")
            return;
          this.popupRules.RuleValidate((object) ctrl3, tag);
          val = ctrl3.Text.Trim();
          if (ctrl3.Name == "txtLockDate")
          {
            if (this.txtLockDate.Text != this.getField("2089"))
              this.calculateLockDate("761", this.txtExpireDate, this.txtLockDate, this.txtLockDays);
          }
          else if (ctrl3.Name == "txtExpireDate" && this.txtExpireDate.Text != this.getField("2151"))
          {
            this.calculateLockDate("762", this.txtExpireDate, this.txtLockDate, this.txtLockDays);
            val = ctrl3.Text.Trim();
          }
          if (ctrl3.Name == "txtLockDate" || ctrl3.Name == "txtExpireDate")
          {
            this.setField("2089", this.txtLockDate.Text.Trim());
            this.setField("2090", this.txtLockDays.Text.Trim());
            this.setField("2091", this.txtExpireDate.Text.Trim());
            break;
          }
          break;
        default:
          return;
      }
      this.setField(tag, val);
    }

    private void calculateLockDate(
      string id,
      DatePicker fieldExpire,
      DatePicker fieldLockDate,
      TextBox fieldDays)
    {
      LockExpDayCountSetting policySetting = (LockExpDayCountSetting) Session.StartupInfo.PolicySettings[(object) "Policies.LockExpDayCount"];
      DateTime lockDate = DateTime.MaxValue;
      DateTime dateTime = DateTime.MaxValue;
      string str1 = "762";
      string str2 = "432";
      string str3 = "761";
      string s1 = fieldExpire.Text.Trim();
      string s2 = fieldLockDate.Text.Trim();
      int lockDays1 = Utils.ParseInt((object) fieldDays.Text.Trim());
      if (s2 == "//")
        s2 = string.Empty;
      if (s1 == "//")
        s1 = string.Empty;
      if (s2 == string.Empty && id == str3)
      {
        fieldDays.Text = "";
        fieldExpire.Text = "";
      }
      else
      {
        try
        {
          if (s2 != string.Empty)
            lockDate = DateTime.Parse(s2).Date;
        }
        catch (Exception ex)
        {
          return;
        }
        try
        {
          if (s1 != string.Empty)
            dateTime = DateTime.Parse(s1).Date;
        }
        catch (Exception ex)
        {
          return;
        }
        if (id == str2 || id == str3 && lockDays1 != 0)
        {
          if (lockDays1 > 0)
          {
            if (s2 == string.Empty && s1 != string.Empty)
            {
              fieldLockDate.Text = this.lockCalculator.CalculateLockDate(dateTime, lockDays1).ToString("MM/dd/yyyy");
            }
            else
            {
              if (s2 == string.Empty && s1 == string.Empty)
                return;
              fieldExpire.Text = this.lockCalculator.CalculateLockExpirationDate(lockDate, lockDays1).ToString("MM/dd/yyyy");
            }
          }
          else
          {
            fieldExpire.Text = string.Empty;
            fieldDays.Text = string.Empty;
          }
        }
        else
        {
          if (!(id == str1) && (!(id == str3) || lockDays1 != 0))
            return;
          if (s1 != string.Empty)
          {
            DateTime lockExpirationDate = this.lockCalculator.GetNextClosestLockExpirationDate(dateTime);
            fieldExpire.Text = lockExpirationDate.ToString("MM/dd/yyyy");
            if (s2 == string.Empty && lockDays1 != 0)
            {
              fieldLockDate.Text = this.lockCalculator.CalculateLockDate(lockExpirationDate, lockDays1).ToString("MM/dd/yyyy");
            }
            else
            {
              if (!(s2 != string.Empty))
                return;
              int lockDays2 = this.lockCalculator.CalculateLockDays(lockDate, lockExpirationDate);
              if (lockDays2 > 0)
                fieldDays.Text = lockDays2.ToString();
              else
                fieldDays.Text = "";
            }
          }
          else
            fieldDays.Text = "";
        }
      }
    }

    private void dateField_Leave(object sender, EventArgs e)
    {
      if (this.readOnly)
        return;
      DatePicker ctrl = (DatePicker) sender;
      if (ctrl == null)
        return;
      string tag = (string) ctrl.Tag;
      if ((tag ?? "") == "")
        return;
      this.popupRules.RuleValidate((object) ctrl, tag);
      if (ctrl.Text == "//")
        this.setField(tag, "");
      else
        this.setField(tag, ctrl.Text);
    }

    internal void ZoomPanels(bool hideBR, bool hideBM, bool hideEX, bool hideBP)
    {
      this.hideBR = hideBR;
      this.hideBM = hideBM;
      this.hideEX = hideEX;
      this.hideBP = hideBP;
      this.zoomInAndOut();
    }

    private void zoomInAndOut()
    {
      if (this.hideBR)
      {
        this.pnlBRExpand.Visible = false;
        this.picBRZoomIn.Visible = true;
        this.picBRZoomOut.Visible = false;
      }
      else
      {
        this.pnlBRExpand.Visible = true;
        this.picBRZoomIn.Visible = false;
        this.picBRZoomOut.Visible = true;
      }
      if (this.hideBP)
      {
        this.pnlBPExtension.Visible = false;
        this.picBPZoomIn.Visible = true;
        this.picBPZoomOut.Visible = false;
      }
      else
      {
        this.pnlBPExtension.Visible = true;
        this.picBPZoomIn.Visible = false;
        this.picBPZoomOut.Visible = true;
      }
      if (this.hideBM)
      {
        this.pnlBMExpand.Visible = false;
        this.picBMZoomIn.Visible = true;
        this.picBMZoomOut.Visible = false;
      }
      else
      {
        this.pnlBMExpand.Visible = true;
        this.picBMZoomIn.Visible = false;
        this.picBMZoomOut.Visible = true;
      }
      if (this.hideEX)
      {
        this.pnlExExpand.Visible = false;
        this.picEXZoomIn.Visible = true;
        this.picEXZoomOut.Visible = false;
      }
      else
      {
        this.pnlExExpand.Visible = true;
        this.picEXZoomIn.Visible = false;
        this.picEXZoomOut.Visible = true;
      }
      if (this.hideRL)
      {
        this.pnlRlExpand.Visible = false;
        this.picRLZoomIn.Visible = true;
        this.picRLZoomOut.Visible = false;
      }
      else
      {
        this.pnlRlExpand.Visible = true;
        this.picRLZoomIn.Visible = false;
        this.picRLZoomOut.Visible = true;
      }
      if (this.hideCP)
      {
        this.pnlCpExpand.Visible = false;
        this.picCPZoomIn.Visible = true;
        this.picCPZoomOut.Visible = false;
      }
      else
      {
        this.pnlCpExpand.Visible = true;
        this.picCPZoomIn.Visible = false;
        this.picCPZoomOut.Visible = true;
      }
      this.setHeight();
    }

    private void ZoomButton_Clicked(object sender, EventArgs e)
    {
      string name = ((Control) sender).Name;
      // ISSUE: reference to a compiler-generated method
      switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(name))
      {
        case 387864817:
          if (!(name == "picBPZoomIn"))
            return;
          this.hideBP = false;
          break;
        case 518219510:
          if (!(name == "picEXZoomIn"))
            return;
          this.hideEX = false;
          break;
        case 686178818:
          if (!(name == "picBMZoomIn"))
            return;
          this.hideBM = false;
          break;
        case 1413198263:
          if (!(name == "picEXZoomOut"))
            return;
          this.hideEX = true;
          break;
        case 1580154386:
          if (!(name == "picBPZoomOut"))
            return;
          this.hideBP = true;
          break;
        case 1947951515:
          if (!(name == "picBMZoomOut"))
            return;
          this.hideBM = true;
          break;
        case 2019191421:
          if (!(name == "picCPZoomOut"))
            return;
          this.hideCP = true;
          break;
        case 2239828019:
          if (!(name == "picBRZoomIn"))
            return;
          this.hideBR = false;
          break;
        case 2413136416:
          if (!(name == "picBRZoomOut"))
            return;
          this.hideBR = true;
          break;
        case 2435625525:
          if (!(name == "picRLZoomIn"))
            return;
          this.hideRL = false;
          break;
        case 2435779704:
          if (!(name == "picCPZoomIn"))
            return;
          this.hideCP = false;
          break;
        case 3084277534:
          if (!(name == "picRLZoomOut"))
            return;
          this.hideRL = true;
          break;
        default:
          return;
      }
      this.zoomInAndOut();
      if (this.ZoomButtonClicked == null)
        return;
      this.ZoomButtonClicked(sender, e);
    }

    public bool IsBaseRateDisplayedAll => !this.hideBR;

    public bool IsBasePriceDisplayedAll => !this.hideBP;

    public bool IsBaseMarginDisplayedAll => !this.hideBM;

    public bool IsExtensionDisplayedAll => !this.hideEX;

    public bool isReadOnly
    {
      get => this.readOnly;
      set => this.readOnly = value;
    }

    private void commitControlValueToField(TextBox control)
    {
      string id = string.Concat(control.Tag);
      if (!(id != "") || !this.popupRules.RuleValidate((object) control, id))
        return;
      this.setField(id, control.Text.Trim());
    }

    private void zoomIn_MouseEnter(object sender, EventArgs e)
    {
      ((PictureBox) sender).Image = (Image) this.UIresources.GetObject("picZoomInOver.Image");
    }

    private void zoomOut_MouseEnter(object sender, EventArgs e)
    {
      ((PictureBox) sender).Image = (Image) this.UIresources.GetObject("picZoomOutOver.Image");
    }

    private void zoomOut_MouseLeave(object sender, EventArgs e)
    {
      ((PictureBox) sender).Image = (Image) this.UIresources.GetObject("picBRZoomOut.Image");
    }

    private void zoomIn_MouseLeave(object sender, EventArgs e)
    {
      ((PictureBox) sender).Image = (Image) this.UIresources.GetObject("picBRZoomIn.Image");
    }

    private void txtComments_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (!this.readOnly)
        return;
      e.Handled = true;
    }

    private void txtComments_KeyDown(object sender, KeyEventArgs e)
    {
      if (!this.readOnly)
        return;
      e.Handled = true;
    }

    private void btnClear_Click(object sender, EventArgs e)
    {
      if (this.ClearButtonClicked == null)
        return;
      this.ClearButtonClicked(sender, e);
    }

    private DateTime getExpirationDate(string currentLockExpirationDateString, int daysToExtend)
    {
      DateTime rawExpireDate = Utils.ParseDate((object) currentLockExpirationDateString, DateTime.MinValue).AddDays((double) daysToExtend);
      return string.Concat(this.companySettings[(object) "LockExtensionCalendarOpt"]) != "True" ? rawExpireDate : new LockRequestCalculator(Session.DefaultInstance.SessionObjects, this.loanMgr.LoanData).GetNextClosestLockExpirationDate(rawExpireDate);
    }

    internal void setFieldinRange(int begin, int end, int diff)
    {
      for (int index = begin; index <= end; ++index)
        this.setField((index + diff).ToString(), this.getField(index.ToString()));
    }

    internal void setFieldEmptyinRange(int begin, int end)
    {
      for (int index = begin; index <= end; ++index)
        this.setField(index.ToString(), "");
    }

    private void lockExtensionField_Leave(object sender, EventArgs e)
    {
      if (sender == this.txtDaysToExtent && this.txtDaysToExtent.Text != this.getField("3360"))
      {
        string id = this.txtDaysToExtent.Tag.ToString();
        string val = this.txtDaysToExtent.Text.Trim();
        this.popupRules.RuleValidate((object) this.txtDaysToExtent, id);
        this.setField(id, val);
        this.calculateLockExtension(sender);
        this.UpdateLockExtensionAdjustment();
      }
      else if (sender == this.txtNewLockExpDate && this.txtNewLockExpDate.Text != this.getField("3361"))
      {
        string id = this.txtNewLockExpDate.Tag.ToString();
        string val = this.txtNewLockExpDate.Text.Trim();
        this.popupRules.RuleValidate((object) this.txtNewLockExpDate, id);
        if (Utils.ParseDate((object) this.txtNewLockExpDate.Text, DateTime.MinValue) <= Utils.ToDate(this.getField("2091")))
          val = this.getField("2091");
        this.setField(id, val);
        this.calculateLockExtension(sender);
        this.UpdateLockExtensionAdjustment();
      }
      else if (sender == this.txtPriceAdj && this.txtPriceAdj.Text != this.getField("3362"))
      {
        string id = this.txtPriceAdj.Tag.ToString();
        string val = this.txtPriceAdj.Text.Trim();
        this.popupRules.RuleValidate((object) this.txtPriceAdj, id);
        if (val != "")
          val = Utils.ParseDouble((object) val).ToString("N3");
        this.setField(id, val);
        this.calculateLockExtension(sender);
        this.UpdateLockExtensionAdjustment();
      }
      this.txtDaysToExtent.Text = this.getField("3360").ToString();
      this.txtNewLockExpDate.Text = this.getField("3361").ToString();
      this.txtPriceAdj.Text = this.getField("3362").ToString();
    }

    private void calculateLockExtension(object sender)
    {
      string field = this.getField("3369");
      if (sender == this.txtDaysToExtent)
      {
        if (Utils.IsInt((object) this.getField("3360")))
        {
          int daysToExtend = Utils.ParseInt((object) this.getField("3360"));
          this.setField("3361", this.getExpirationDate(field, daysToExtend).ToString("MM/dd/yyyy"));
          Decimal priceAdjustment = this.getPriceAdjustment(daysToExtend, (Decimal) Utils.ToDouble(this.getField("3362")), this.getField("2091"));
          this.setField("3362", priceAdjustment == 0M ? "" : priceAdjustment.ToString("N3"));
        }
        else
        {
          this.setField("3361", "");
          this.setField("3362", "");
        }
      }
      else if (sender == this.txtNewLockExpDate)
      {
        int days = (Utils.ToDate(this.getField("3361")) - Utils.ToDate(field)).Days;
        this.setField("3360", days.ToString());
        Decimal priceAdjustment = this.getPriceAdjustment(days, (Decimal) Utils.ToDouble(this.getField("3362")), this.getField("2091"));
        this.setField("3362", priceAdjustment == 0M ? "" : priceAdjustment.ToString("N3"));
      }
      else
      {
        TextBox txtPriceAdj = this.txtPriceAdj;
      }
    }

    private void UpdateLockExtensionAdjustment()
    {
      int num1 = Utils.ParseInt((object) this.getField("3433"), 1);
      if (num1 > 10)
        return;
      int num2 = 3474 + (num1 - 1) * 2;
      if (Utils.IsInt((object) this.getField("3360")) && Utils.IsDouble((object) this.getField("3362")))
      {
        int num3 = Utils.ParseInt((object) this.getField("3360"));
        string field = this.getField("3362");
        this.setField(num2.ToString(), "(" + (object) num3 + (num3 == 1 ? (object) " day" : (object) " days") + ")");
        this.setField((num2 + 1).ToString(), field);
      }
      else
      {
        this.setField(num2.ToString(), "");
        this.setField((num2 + 1).ToString(), "");
      }
    }

    private Decimal getPriceAdjustment(
      int daysToExtend,
      Decimal originalPriceAdjustment,
      string originalExpirationDateString)
    {
      Decimal priceAdjustment1 = 0M;
      if (this.lockExtensionUtils.HasPriceAdjustment(daysToExtend))
        priceAdjustment1 = this.lockExtensionUtils.GetPriceAdjustment(daysToExtend);
      if (string.Concat(this.companySettings[(object) "LockExtensionCalendarOpt"]) == "False" || string.Concat(this.companySettings[(object) "LockExtCalOpt_ApplyPriceAdj"]) != "True")
        return priceAdjustment1;
      DateTime dateTime = Utils.ParseDate((object) originalExpirationDateString, DateTime.MinValue).AddDays((double) daysToExtend);
      DateTime expirationDate = this.getExpirationDate(originalExpirationDateString, daysToExtend);
      Decimal priceAdjustment2 = 0M;
      int days = expirationDate.Subtract(dateTime).Days;
      if (this.lockExtensionUtils.HasPriceAdjustment(daysToExtend + days))
        priceAdjustment2 = this.lockExtensionUtils.GetPriceAdjustment(daysToExtend + days);
      return priceAdjustment2;
    }

    private void cBoxCmtType_SelectedValueChanged(object sender, EventArgs e)
    {
      string text = ((Control) sender).Text;
      string id1 = this.cBoxCmtType.Tag.ToString();
      string id2 = this.comboBoxDeliveryType.Tag.ToString();
      if (text == CorrespondentTradeCommitmentType.BestEfforts.ToDescription())
      {
        this.comboBoxDeliveryType.Text = CorrespondentMasterDeliveryType.IndividualBestEfforts.ToDescription();
        this.setField(id1, CorrespondentTradeCommitmentType.BestEfforts.ToDescription());
        this.setField(id2, CorrespondentMasterDeliveryType.IndividualBestEfforts.ToDescription());
      }
      else if (text == CorrespondentTradeCommitmentType.Mandatory.ToDescription())
      {
        this.comboBoxDeliveryType.Text = CorrespondentMasterDeliveryType.IndividualMandatory.ToDescription();
        this.setField(id1, CorrespondentTradeCommitmentType.Mandatory.ToDescription());
        this.setField(id2, CorrespondentMasterDeliveryType.IndividualMandatory.ToDescription());
      }
      else
      {
        this.comboBoxDeliveryType.Text = "";
        this.setField(id1, "");
        this.setField(id2, "");
      }
    }

    private bool setCommitmentTypeOptions()
    {
      List<string> stringList = new List<string>();
      LoanDataMgr loanDataMgr = Session.LoanDataMgr;
      LoanChannel result;
      if (loanDataMgr == null || !Enum.TryParse<LoanChannel>(loanDataMgr.LoanData.GetField("2626"), out result))
        return false;
      string field = loanDataMgr.LoanData.GetField("TPO.X15");
      if (field.Trim() == string.Empty || result != LoanChannel.Correspondent || Session.ConfigurationManager == null)
        return false;
      List<ExternalOriginatorManagementData> organizationByTpoid = Session.ConfigurationManager.GetExternalOrganizationByTPOID(field);
      if (organizationByTpoid == null)
        return false;
      stringList.Add("");
      ExternalOriginatorManagementData originatorManagementData = organizationByTpoid.Where<ExternalOriginatorManagementData>((Func<ExternalOriginatorManagementData, bool>) (x => x.OrganizationType == ExternalOriginatorOrgType.Company)).First<ExternalOriginatorManagementData>();
      if (originatorManagementData != null)
      {
        if (originatorManagementData.CommitmentUseBestEffort || originatorManagementData.CommitmentUseBestEffortLimited)
          stringList.Add(CorrespondentTradeCommitmentType.BestEfforts.ToDescription());
        if (originatorManagementData.IsCommitmentDeliveryIndividual)
          stringList.Add(CorrespondentTradeCommitmentType.Mandatory.ToDescription());
      }
      this.cmtTypeOptions = stringList.ToArray();
      return true;
    }

    public string getComment() => this.txtComments.Text;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (WorstCasePricingColumn));
      this.gcPrice = new GroupContainer();
      this.pnlComment = new Panel();
      this.label59 = new Label();
      this.txtComments = new TextBox();
      this.pnlNetBM = new Panel();
      this.label31 = new Label();
      this.txtRequestSRP = new TextBox();
      this.label15 = new Label();
      this.textBoxMargin28 = new TextBox();
      this.groupBox2 = new GroupBox();
      this.label16 = new Label();
      this.textBoxMargin29 = new TextBox();
      this.pnlBMExpand = new Panel();
      this.comboBoxMargin20 = new ComboBox();
      this.comboBoxMargin5 = new ComboBox();
      this.textBoxMargin7 = new TextBox();
      this.textBoxMargin21 = new TextBox();
      this.comboBoxMargin6 = new ComboBox();
      this.comboBoxMargin7 = new ComboBox();
      this.textBoxMargin6 = new TextBox();
      this.textBoxMargin8 = new TextBox();
      this.textBoxMargin20 = new TextBox();
      this.comboBoxMargin8 = new ComboBox();
      this.comboBoxMargin19 = new ComboBox();
      this.textBoxMargin9 = new TextBox();
      this.textBoxMargin19 = new TextBox();
      this.comboBoxMargin9 = new ComboBox();
      this.comboBoxMargin18 = new ComboBox();
      this.textBoxMargin10 = new TextBox();
      this.textBoxMargin18 = new TextBox();
      this.comboBoxMargin10 = new ComboBox();
      this.comboBoxMargin17 = new ComboBox();
      this.textBoxMargin11 = new TextBox();
      this.textBoxMargin17 = new TextBox();
      this.comboBoxMargin11 = new ComboBox();
      this.comboBoxMargin16 = new ComboBox();
      this.textBoxMargin12 = new TextBox();
      this.textBoxMargin16 = new TextBox();
      this.comboBoxMargin12 = new ComboBox();
      this.comboBoxMargin15 = new ComboBox();
      this.textBoxMargin13 = new TextBox();
      this.textBoxMargin15 = new TextBox();
      this.comboBoxMargin13 = new ComboBox();
      this.comboBoxMargin14 = new ComboBox();
      this.textBoxMargin14 = new TextBox();
      this.pnlBM = new Panel();
      this.comboBoxMargin1 = new ComboBox();
      this.textBoxMargin2 = new TextBox();
      this.comboBoxMargin2 = new ComboBox();
      this.textBoxMargin3 = new TextBox();
      this.comboBoxMargin3 = new ComboBox();
      this.textBoxMargin4 = new TextBox();
      this.comboBoxMargin4 = new ComboBox();
      this.textBoxMargin5 = new TextBox();
      this.picBMZoomOut = new PictureBox();
      this.picBMZoomIn = new PictureBox();
      this.label14 = new Label();
      this.textBoxMargin1 = new TextBox();
      this.pnlNetPrice = new Panel();
      this.label10 = new Label();
      this.textBoxPrice22 = new TextBox();
      this.groupBox5 = new GroupBox();
      this.label9 = new Label();
      this.textBoxPrice23 = new TextBox();
      this.pnlCpExpand = new Panel();
      this.label42 = new Label();
      this.textBoxCpa10Desc = new TextBox();
      this.textBoxCpa10 = new TextBox();
      this.label43 = new Label();
      this.textBoxCpa9Desc = new TextBox();
      this.textBoxCpa9 = new TextBox();
      this.label44 = new Label();
      this.textBoxCpa8Desc = new TextBox();
      this.textBoxCpa8 = new TextBox();
      this.label45 = new Label();
      this.textBoxCpa7Desc = new TextBox();
      this.textBoxCpa7 = new TextBox();
      this.label46 = new Label();
      this.textBoxCpa6Desc = new TextBox();
      this.textBoxCpa6 = new TextBox();
      this.label47 = new Label();
      this.textBoxCpa5Desc = new TextBox();
      this.textBoxCpa5 = new TextBox();
      this.label48 = new Label();
      this.textBoxCpa4Desc = new TextBox();
      this.textBoxCpa4 = new TextBox();
      this.pnlCp = new Panel();
      this.label49 = new Label();
      this.label50 = new Label();
      this.label51 = new Label();
      this.textBoxCpa3Desc = new TextBox();
      this.textBoxCpa3 = new TextBox();
      this.textBoxCpa2Desc = new TextBox();
      this.textBoxCpa2 = new TextBox();
      this.textBoxCpa1Desc = new TextBox();
      this.textBoxCpa1 = new TextBox();
      this.picCPZoomOut = new PictureBox();
      this.picCPZoomIn = new PictureBox();
      this.label52 = new Label();
      this.pnlRlExpand = new Panel();
      this.label28 = new Label();
      this.textBoxReLock10Desc = new TextBox();
      this.textBoxReLock10 = new TextBox();
      this.label29 = new Label();
      this.textBoxReLock9Desc = new TextBox();
      this.textBoxReLock9 = new TextBox();
      this.label30 = new Label();
      this.textBoxReLock8Desc = new TextBox();
      this.textBoxReLock8 = new TextBox();
      this.label32 = new Label();
      this.textBoxReLock7Desc = new TextBox();
      this.textBoxReLock7 = new TextBox();
      this.label33 = new Label();
      this.textBoxReLock6Desc = new TextBox();
      this.textBoxReLock6 = new TextBox();
      this.label34 = new Label();
      this.textBoxReLock5Desc = new TextBox();
      this.textBoxReLock5 = new TextBox();
      this.label35 = new Label();
      this.textBoxReLock4Desc = new TextBox();
      this.textBoxReLock4 = new TextBox();
      this.pnlRl = new Panel();
      this.label36 = new Label();
      this.label37 = new Label();
      this.label38 = new Label();
      this.textBoxReLock3Desc = new TextBox();
      this.textBoxReLock3 = new TextBox();
      this.textBoxReLock2Desc = new TextBox();
      this.textBoxReLock2 = new TextBox();
      this.textBoxReLock1Desc = new TextBox();
      this.textBoxReLock1 = new TextBox();
      this.picRLZoomOut = new PictureBox();
      this.picRLZoomIn = new PictureBox();
      this.label39 = new Label();
      this.btnClear = new Button();
      this.pnlExExpand = new Panel();
      this.label97 = new Label();
      this.textBoxExtension10Desc = new TextBox();
      this.textBoxExtension10 = new TextBox();
      this.label95 = new Label();
      this.textBoxExtension9Desc = new TextBox();
      this.textBoxExtension9 = new TextBox();
      this.label96 = new Label();
      this.textBoxExtension8Desc = new TextBox();
      this.textBoxExtension8 = new TextBox();
      this.label93 = new Label();
      this.textBoxExtension7Desc = new TextBox();
      this.textBoxExtension7 = new TextBox();
      this.label94 = new Label();
      this.textBoxExtension6Desc = new TextBox();
      this.textBoxExtension6 = new TextBox();
      this.label92 = new Label();
      this.textBoxExtension5Desc = new TextBox();
      this.textBoxExtension5 = new TextBox();
      this.label87 = new Label();
      this.textBoxExtension4Desc = new TextBox();
      this.textBoxExtension4 = new TextBox();
      this.pnlEx = new Panel();
      this.label86 = new Label();
      this.label85 = new Label();
      this.label84 = new Label();
      this.textBoxExtension3Desc = new TextBox();
      this.textBoxExtension3 = new TextBox();
      this.textBoxExtension2Desc = new TextBox();
      this.textBoxExtension2 = new TextBox();
      this.textBoxExtension1Desc = new TextBox();
      this.textBoxExtension1 = new TextBox();
      this.picEXZoomOut = new PictureBox();
      this.picEXZoomIn = new PictureBox();
      this.label82 = new Label();
      this.pnlBPExtension = new Panel();
      this.comboBoxPrice19 = new ComboBox();
      this.textBoxPrice20 = new TextBox();
      this.comboBoxPrice20 = new ComboBox();
      this.textBoxPrice21 = new TextBox();
      this.comboBoxPrice15 = new ComboBox();
      this.textBoxPrice16 = new TextBox();
      this.comboBoxPrice16 = new ComboBox();
      this.textBoxPrice17 = new TextBox();
      this.comboBoxPrice17 = new ComboBox();
      this.textBoxPrice18 = new TextBox();
      this.comboBoxPrice18 = new ComboBox();
      this.textBoxPrice19 = new TextBox();
      this.comboBoxPrice13 = new ComboBox();
      this.textBoxPrice14 = new TextBox();
      this.comboBoxPrice14 = new ComboBox();
      this.textBoxPrice15 = new TextBox();
      this.comboBoxPrice9 = new ComboBox();
      this.textBoxPrice10 = new TextBox();
      this.comboBoxPrice10 = new ComboBox();
      this.textBoxPrice11 = new TextBox();
      this.comboBoxPrice11 = new ComboBox();
      this.textBoxPrice12 = new TextBox();
      this.comboBoxPrice12 = new ComboBox();
      this.textBoxPrice13 = new TextBox();
      this.comboBoxPrice5 = new ComboBox();
      this.textBoxPrice6 = new TextBox();
      this.comboBoxPrice6 = new ComboBox();
      this.textBoxPrice7 = new TextBox();
      this.comboBoxPrice7 = new ComboBox();
      this.textBoxPrice8 = new TextBox();
      this.comboBoxPrice8 = new ComboBox();
      this.textBoxPrice9 = new TextBox();
      this.pnlBP = new Panel();
      this.picBPZoomOut = new PictureBox();
      this.picBPZoomIn = new PictureBox();
      this.comboBoxPrice1 = new ComboBox();
      this.textBoxPrice2 = new TextBox();
      this.comboBoxPrice2 = new ComboBox();
      this.label11 = new Label();
      this.textBoxPrice1 = new TextBox();
      this.textBoxPrice3 = new TextBox();
      this.comboBoxPrice3 = new ComboBox();
      this.textBoxPrice4 = new TextBox();
      this.comboBoxPrice4 = new ComboBox();
      this.textBoxPrice5 = new TextBox();
      this.pnlRateRequested = new Panel();
      this.label77 = new Label();
      this.textBox6 = new TextBox();
      this.label75 = new Label();
      this.textBox2 = new TextBox();
      this.label76 = new Label();
      this.textBox4 = new TextBox();
      this.label7 = new Label();
      this.textBoxRate22 = new TextBox();
      this.label6 = new Label();
      this.textBoxRate23 = new TextBox();
      this.pnlBRExpand = new Panel();
      this.textBoxRate21 = new TextBox();
      this.comboBoxRate7 = new ComboBox();
      this.comboBoxRate20 = new ComboBox();
      this.textBoxRate8 = new TextBox();
      this.textBoxRate20 = new TextBox();
      this.comboBoxRate8 = new ComboBox();
      this.comboBoxRate19 = new ComboBox();
      this.textBoxRate9 = new TextBox();
      this.textBoxRate19 = new TextBox();
      this.comboBoxRate9 = new ComboBox();
      this.comboBoxRate18 = new ComboBox();
      this.textBoxRate10 = new TextBox();
      this.textBoxRate18 = new TextBox();
      this.comboBoxRate10 = new ComboBox();
      this.comboBoxRate17 = new ComboBox();
      this.textBoxRate11 = new TextBox();
      this.textBoxRate17 = new TextBox();
      this.comboBoxRate11 = new ComboBox();
      this.comboBoxRate16 = new ComboBox();
      this.textBoxRate12 = new TextBox();
      this.textBoxRate16 = new TextBox();
      this.comboBoxRate12 = new ComboBox();
      this.comboBoxRate15 = new ComboBox();
      this.textBoxRate13 = new TextBox();
      this.textBoxRate15 = new TextBox();
      this.comboBoxRate13 = new ComboBox();
      this.comboBoxRate14 = new ComboBox();
      this.textBoxRate14 = new TextBox();
      this.pnlBR = new Panel();
      this.comboBoxRate1 = new ComboBox();
      this.textBoxRate2 = new TextBox();
      this.comboBoxRate2 = new ComboBox();
      this.textBoxRate3 = new TextBox();
      this.comboBoxRate3 = new ComboBox();
      this.textBoxRate4 = new TextBox();
      this.comboBoxRate4 = new ComboBox();
      this.textBoxRate5 = new TextBox();
      this.comboBoxRate5 = new ComboBox();
      this.textBoxRate6 = new TextBox();
      this.comboBoxRate6 = new ComboBox();
      this.textBoxRate7 = new TextBox();
      this.picBRZoomOut = new PictureBox();
      this.picBRZoomIn = new PictureBox();
      this.label8 = new Label();
      this.textBoxRate1 = new TextBox();
      this.pnlLockInfo = new Panel();
      this.txtExpireDate = new DatePicker();
      this.lblLockExpirationDate = new Label();
      this.pnlLockExtension = new Panel();
      this.label17 = new Label();
      this.txtPriceAdj = new TextBox();
      this.txtNewLockExpDate = new DatePicker();
      this.label13 = new Label();
      this.txtDaysToExtent = new TextBox();
      this.label12 = new Label();
      this.txtCurLockExpDate = new DatePicker();
      this.label5 = new Label();
      this.txtLockDate = new DatePicker();
      this.dtLastRateSetDate = new DatePicker();
      this.label3 = new Label();
      this.groupBox10 = new GroupBox();
      this.txtLockDays = new TextBox();
      this.label41 = new Label();
      this.label40 = new Label();
      this.pnlDeliveryType = new Panel();
      this.comboBoxDeliveryType = new TextBox();
      this.label2 = new Label();
      this.cBoxCmtType = new ComboBox();
      this.labelCt = new Label();
      this.pnlLoanProgram = new Panel();
      this.textBoxLoanProgram = new TextBox();
      this.labelLoanProgram = new Label();
      this.label1 = new Label();
      this.txtRateSheetID = new TextBox();
      this.toolTipField = new ToolTip(this.components);
      this.gcPrice.SuspendLayout();
      this.pnlComment.SuspendLayout();
      this.pnlNetBM.SuspendLayout();
      this.pnlBMExpand.SuspendLayout();
      this.pnlBM.SuspendLayout();
      ((ISupportInitialize) this.picBMZoomOut).BeginInit();
      ((ISupportInitialize) this.picBMZoomIn).BeginInit();
      this.pnlNetPrice.SuspendLayout();
      this.pnlCpExpand.SuspendLayout();
      this.pnlCp.SuspendLayout();
      ((ISupportInitialize) this.picCPZoomOut).BeginInit();
      ((ISupportInitialize) this.picCPZoomIn).BeginInit();
      this.pnlRlExpand.SuspendLayout();
      this.pnlRl.SuspendLayout();
      ((ISupportInitialize) this.picRLZoomOut).BeginInit();
      ((ISupportInitialize) this.picRLZoomIn).BeginInit();
      this.pnlExExpand.SuspendLayout();
      this.pnlEx.SuspendLayout();
      ((ISupportInitialize) this.picEXZoomOut).BeginInit();
      ((ISupportInitialize) this.picEXZoomIn).BeginInit();
      this.pnlBPExtension.SuspendLayout();
      this.pnlBP.SuspendLayout();
      ((ISupportInitialize) this.picBPZoomOut).BeginInit();
      ((ISupportInitialize) this.picBPZoomIn).BeginInit();
      this.pnlRateRequested.SuspendLayout();
      this.pnlBRExpand.SuspendLayout();
      this.pnlBR.SuspendLayout();
      ((ISupportInitialize) this.picBRZoomOut).BeginInit();
      ((ISupportInitialize) this.picBRZoomIn).BeginInit();
      this.pnlLockInfo.SuspendLayout();
      this.pnlLockExtension.SuspendLayout();
      this.pnlDeliveryType.SuspendLayout();
      this.pnlLoanProgram.SuspendLayout();
      this.SuspendLayout();
      this.gcPrice.Controls.Add((Control) this.pnlComment);
      this.gcPrice.Controls.Add((Control) this.pnlNetBM);
      this.gcPrice.Controls.Add((Control) this.pnlBMExpand);
      this.gcPrice.Controls.Add((Control) this.pnlBM);
      this.gcPrice.Controls.Add((Control) this.pnlNetPrice);
      this.gcPrice.Controls.Add((Control) this.pnlCpExpand);
      this.gcPrice.Controls.Add((Control) this.pnlCp);
      this.gcPrice.Controls.Add((Control) this.pnlRlExpand);
      this.gcPrice.Controls.Add((Control) this.pnlRl);
      this.gcPrice.Controls.Add((Control) this.btnClear);
      this.gcPrice.Controls.Add((Control) this.pnlExExpand);
      this.gcPrice.Controls.Add((Control) this.pnlEx);
      this.gcPrice.Controls.Add((Control) this.pnlBPExtension);
      this.gcPrice.Controls.Add((Control) this.pnlBP);
      this.gcPrice.Controls.Add((Control) this.pnlRateRequested);
      this.gcPrice.Controls.Add((Control) this.pnlBRExpand);
      this.gcPrice.Controls.Add((Control) this.pnlBR);
      this.gcPrice.Controls.Add((Control) this.pnlLockInfo);
      this.gcPrice.Controls.Add((Control) this.pnlDeliveryType);
      this.gcPrice.Controls.Add((Control) this.pnlLoanProgram);
      this.gcPrice.HeaderForeColor = SystemColors.ControlText;
      this.gcPrice.Location = new Point(0, 0);
      this.gcPrice.Margin = new Padding(0);
      this.gcPrice.Name = "gcPrice";
      this.gcPrice.Size = new Size(335, 3067);
      this.gcPrice.TabIndex = 0;
      this.gcPrice.Text = "Pricing Title";
      this.pnlComment.Controls.Add((Control) this.label59);
      this.pnlComment.Controls.Add((Control) this.txtComments);
      this.pnlComment.Dock = DockStyle.Top;
      this.pnlComment.Location = new Point(1, 2963);
      this.pnlComment.Name = "pnlComment";
      this.pnlComment.Size = new Size(333, 93);
      this.pnlComment.TabIndex = 188;
      this.label59.AutoSize = true;
      this.label59.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label59.Location = new Point(7, 4);
      this.label59.Name = "label59";
      this.label59.Size = new Size(57, 14);
      this.label59.TabIndex = 1;
      this.label59.Text = "Comments";
      this.txtComments.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.txtComments.Location = new Point(10, 21);
      this.txtComments.Multiline = true;
      this.txtComments.Name = "txtComments";
      this.txtComments.ScrollBars = ScrollBars.Both;
      this.txtComments.Size = new Size(312, 59);
      this.txtComments.TabIndex = 91;
      this.txtComments.Tag = (object) "2144";
      this.pnlNetBM.Controls.Add((Control) this.label31);
      this.pnlNetBM.Controls.Add((Control) this.txtRequestSRP);
      this.pnlNetBM.Controls.Add((Control) this.label15);
      this.pnlNetBM.Controls.Add((Control) this.textBoxMargin28);
      this.pnlNetBM.Controls.Add((Control) this.groupBox2);
      this.pnlNetBM.Controls.Add((Control) this.label16);
      this.pnlNetBM.Controls.Add((Control) this.textBoxMargin29);
      this.pnlNetBM.Dock = DockStyle.Top;
      this.pnlNetBM.Location = new Point(1, 2875);
      this.pnlNetBM.Name = "pnlNetBM";
      this.pnlNetBM.Size = new Size(333, 88);
      this.pnlNetBM.TabIndex = 187;
      this.label31.AutoSize = true;
      this.label31.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label31.Location = new Point(7, 48);
      this.label31.Name = "label31";
      this.label31.Size = new Size(70, 14);
      this.label31.TabIndex = 29;
      this.label31.Text = "SRP Paid Out";
      this.txtRequestSRP.BackColor = Color.WhiteSmoke;
      this.txtRequestSRP.Font = new Font("Arial", 8.25f);
      this.txtRequestSRP.Location = new Point(219, 45);
      this.txtRequestSRP.Name = "txtRequestSRP";
      this.txtRequestSRP.ReadOnly = true;
      this.txtRequestSRP.Size = new Size(102, 20);
      this.txtRequestSRP.TabIndex = 28;
      this.txtRequestSRP.TabStop = false;
      this.txtRequestSRP.Tag = (object) "4201";
      this.txtRequestSRP.TextAlign = HorizontalAlignment.Right;
      this.label15.AutoSize = true;
      this.label15.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label15.Location = new Point(7, 3);
      this.label15.Name = "label15";
      this.label15.Size = new Size(173, 14);
      this.label15.TabIndex = 14;
      this.label15.Text = "Total Buy ARM Margin Adjustments";
      this.textBoxMargin28.BackColor = Color.WhiteSmoke;
      this.textBoxMargin28.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.textBoxMargin28.Location = new Point(219, 0);
      this.textBoxMargin28.Name = "textBoxMargin28";
      this.textBoxMargin28.ReadOnly = true;
      this.textBoxMargin28.Size = new Size(102, 20);
      this.textBoxMargin28.TabIndex = 15;
      this.textBoxMargin28.TabStop = false;
      this.textBoxMargin28.Tag = (object) "2688";
      this.textBoxMargin28.TextAlign = HorizontalAlignment.Right;
      this.groupBox2.Dock = DockStyle.Bottom;
      this.groupBox2.Location = new Point(0, 84);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new Size(333, 4);
      this.groupBox2.TabIndex = 18;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "groupBox2";
      this.label16.AutoSize = true;
      this.label16.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label16.Location = new Point(7, 25);
      this.label16.Name = "label16";
      this.label16.Size = new Size(117, 14);
      this.label16.TabIndex = 16;
      this.label16.Text = "Net Buy ARM Margin";
      this.textBoxMargin29.BackColor = Color.WhiteSmoke;
      this.textBoxMargin29.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.textBoxMargin29.Location = new Point(219, 22);
      this.textBoxMargin29.Name = "textBoxMargin29";
      this.textBoxMargin29.ReadOnly = true;
      this.textBoxMargin29.Size = new Size(102, 20);
      this.textBoxMargin29.TabIndex = 16;
      this.textBoxMargin29.TabStop = false;
      this.textBoxMargin29.Tag = (object) "2689";
      this.textBoxMargin29.TextAlign = HorizontalAlignment.Right;
      this.pnlBMExpand.Controls.Add((Control) this.comboBoxMargin20);
      this.pnlBMExpand.Controls.Add((Control) this.comboBoxMargin5);
      this.pnlBMExpand.Controls.Add((Control) this.textBoxMargin7);
      this.pnlBMExpand.Controls.Add((Control) this.textBoxMargin21);
      this.pnlBMExpand.Controls.Add((Control) this.comboBoxMargin6);
      this.pnlBMExpand.Controls.Add((Control) this.comboBoxMargin7);
      this.pnlBMExpand.Controls.Add((Control) this.textBoxMargin6);
      this.pnlBMExpand.Controls.Add((Control) this.textBoxMargin8);
      this.pnlBMExpand.Controls.Add((Control) this.textBoxMargin20);
      this.pnlBMExpand.Controls.Add((Control) this.comboBoxMargin8);
      this.pnlBMExpand.Controls.Add((Control) this.comboBoxMargin19);
      this.pnlBMExpand.Controls.Add((Control) this.textBoxMargin9);
      this.pnlBMExpand.Controls.Add((Control) this.textBoxMargin19);
      this.pnlBMExpand.Controls.Add((Control) this.comboBoxMargin9);
      this.pnlBMExpand.Controls.Add((Control) this.comboBoxMargin18);
      this.pnlBMExpand.Controls.Add((Control) this.textBoxMargin10);
      this.pnlBMExpand.Controls.Add((Control) this.textBoxMargin18);
      this.pnlBMExpand.Controls.Add((Control) this.comboBoxMargin10);
      this.pnlBMExpand.Controls.Add((Control) this.comboBoxMargin17);
      this.pnlBMExpand.Controls.Add((Control) this.textBoxMargin11);
      this.pnlBMExpand.Controls.Add((Control) this.textBoxMargin17);
      this.pnlBMExpand.Controls.Add((Control) this.comboBoxMargin11);
      this.pnlBMExpand.Controls.Add((Control) this.comboBoxMargin16);
      this.pnlBMExpand.Controls.Add((Control) this.textBoxMargin12);
      this.pnlBMExpand.Controls.Add((Control) this.textBoxMargin16);
      this.pnlBMExpand.Controls.Add((Control) this.comboBoxMargin12);
      this.pnlBMExpand.Controls.Add((Control) this.comboBoxMargin15);
      this.pnlBMExpand.Controls.Add((Control) this.textBoxMargin13);
      this.pnlBMExpand.Controls.Add((Control) this.textBoxMargin15);
      this.pnlBMExpand.Controls.Add((Control) this.comboBoxMargin13);
      this.pnlBMExpand.Controls.Add((Control) this.comboBoxMargin14);
      this.pnlBMExpand.Controls.Add((Control) this.textBoxMargin14);
      this.pnlBMExpand.Dock = DockStyle.Top;
      this.pnlBMExpand.Location = new Point(1, 2491);
      this.pnlBMExpand.Name = "pnlBMExpand";
      this.pnlBMExpand.Size = new Size(333, 384);
      this.pnlBMExpand.TabIndex = 186;
      this.comboBoxMargin20.Font = new Font("Arial", 8.25f);
      this.comboBoxMargin20.Location = new Point(10, 360);
      this.comboBoxMargin20.Name = "comboBoxMargin20";
      this.comboBoxMargin20.Size = new Size(203, 22);
      this.comboBoxMargin20.TabIndex = 48;
      this.comboBoxMargin20.Tag = (object) "2686";
      this.comboBoxMargin5.Font = new Font("Arial", 8.25f);
      this.comboBoxMargin5.Location = new Point(10, 0);
      this.comboBoxMargin5.Name = "comboBoxMargin5";
      this.comboBoxMargin5.Size = new Size(203, 22);
      this.comboBoxMargin5.TabIndex = 16;
      this.comboBoxMargin5.Tag = (object) "2656";
      this.textBoxMargin7.Font = new Font("Arial", 8.25f);
      this.textBoxMargin7.Location = new Point(219, 25);
      this.textBoxMargin7.Name = "textBoxMargin7";
      this.textBoxMargin7.Size = new Size(102, 20);
      this.textBoxMargin7.TabIndex = 19;
      this.textBoxMargin7.Tag = (object) "2659";
      this.textBoxMargin7.TextAlign = HorizontalAlignment.Right;
      this.textBoxMargin21.Font = new Font("Arial", 8.25f);
      this.textBoxMargin21.Location = new Point(219, 361);
      this.textBoxMargin21.Name = "textBoxMargin21";
      this.textBoxMargin21.Size = new Size(102, 20);
      this.textBoxMargin21.TabIndex = 47;
      this.textBoxMargin21.Tag = (object) "2687";
      this.textBoxMargin21.TextAlign = HorizontalAlignment.Right;
      this.comboBoxMargin6.Font = new Font("Arial", 8.25f);
      this.comboBoxMargin6.Location = new Point(10, 24);
      this.comboBoxMargin6.Name = "comboBoxMargin6";
      this.comboBoxMargin6.Size = new Size(203, 22);
      this.comboBoxMargin6.TabIndex = 18;
      this.comboBoxMargin6.Tag = (object) "2658";
      this.comboBoxMargin7.Font = new Font("Arial", 8.25f);
      this.comboBoxMargin7.Location = new Point(10, 48);
      this.comboBoxMargin7.Name = "comboBoxMargin7";
      this.comboBoxMargin7.Size = new Size(203, 22);
      this.comboBoxMargin7.TabIndex = 20;
      this.comboBoxMargin7.Tag = (object) "2660";
      this.textBoxMargin6.Font = new Font("Arial", 8.25f);
      this.textBoxMargin6.Location = new Point(219, 1);
      this.textBoxMargin6.Name = "textBoxMargin6";
      this.textBoxMargin6.Size = new Size(102, 20);
      this.textBoxMargin6.TabIndex = 17;
      this.textBoxMargin6.Tag = (object) "2657";
      this.textBoxMargin6.TextAlign = HorizontalAlignment.Right;
      this.textBoxMargin8.Font = new Font("Arial", 8.25f);
      this.textBoxMargin8.Location = new Point(219, 49);
      this.textBoxMargin8.Name = "textBoxMargin8";
      this.textBoxMargin8.Size = new Size(102, 20);
      this.textBoxMargin8.TabIndex = 21;
      this.textBoxMargin8.Tag = (object) "2661";
      this.textBoxMargin8.TextAlign = HorizontalAlignment.Right;
      this.textBoxMargin20.Font = new Font("Arial", 8.25f);
      this.textBoxMargin20.Location = new Point(219, 337);
      this.textBoxMargin20.Name = "textBoxMargin20";
      this.textBoxMargin20.Size = new Size(102, 20);
      this.textBoxMargin20.TabIndex = 45;
      this.textBoxMargin20.Tag = (object) "2685";
      this.textBoxMargin20.TextAlign = HorizontalAlignment.Right;
      this.comboBoxMargin8.Font = new Font("Arial", 8.25f);
      this.comboBoxMargin8.Location = new Point(10, 72);
      this.comboBoxMargin8.Name = "comboBoxMargin8";
      this.comboBoxMargin8.Size = new Size(203, 22);
      this.comboBoxMargin8.TabIndex = 22;
      this.comboBoxMargin8.Tag = (object) "2662";
      this.comboBoxMargin19.Font = new Font("Arial", 8.25f);
      this.comboBoxMargin19.Location = new Point(10, 336);
      this.comboBoxMargin19.Name = "comboBoxMargin19";
      this.comboBoxMargin19.Size = new Size(203, 22);
      this.comboBoxMargin19.TabIndex = 44;
      this.comboBoxMargin19.Tag = (object) "2684";
      this.textBoxMargin9.Font = new Font("Arial", 8.25f);
      this.textBoxMargin9.Location = new Point(219, 73);
      this.textBoxMargin9.Name = "textBoxMargin9";
      this.textBoxMargin9.Size = new Size(102, 20);
      this.textBoxMargin9.TabIndex = 23;
      this.textBoxMargin9.Tag = (object) "2663";
      this.textBoxMargin9.TextAlign = HorizontalAlignment.Right;
      this.textBoxMargin19.Font = new Font("Arial", 8.25f);
      this.textBoxMargin19.Location = new Point(219, 313);
      this.textBoxMargin19.Name = "textBoxMargin19";
      this.textBoxMargin19.Size = new Size(102, 20);
      this.textBoxMargin19.TabIndex = 43;
      this.textBoxMargin19.Tag = (object) "2683";
      this.textBoxMargin19.TextAlign = HorizontalAlignment.Right;
      this.comboBoxMargin9.Font = new Font("Arial", 8.25f);
      this.comboBoxMargin9.Location = new Point(10, 96);
      this.comboBoxMargin9.Name = "comboBoxMargin9";
      this.comboBoxMargin9.Size = new Size(203, 22);
      this.comboBoxMargin9.TabIndex = 24;
      this.comboBoxMargin9.Tag = (object) "2664";
      this.comboBoxMargin18.Font = new Font("Arial", 8.25f);
      this.comboBoxMargin18.Location = new Point(10, 312);
      this.comboBoxMargin18.Name = "comboBoxMargin18";
      this.comboBoxMargin18.Size = new Size(203, 22);
      this.comboBoxMargin18.TabIndex = 42;
      this.comboBoxMargin18.Tag = (object) "2682";
      this.textBoxMargin10.Font = new Font("Arial", 8.25f);
      this.textBoxMargin10.Location = new Point(219, 97);
      this.textBoxMargin10.Name = "textBoxMargin10";
      this.textBoxMargin10.Size = new Size(102, 20);
      this.textBoxMargin10.TabIndex = 25;
      this.textBoxMargin10.Tag = (object) "2665";
      this.textBoxMargin10.TextAlign = HorizontalAlignment.Right;
      this.textBoxMargin18.Font = new Font("Arial", 8.25f);
      this.textBoxMargin18.Location = new Point(219, 289);
      this.textBoxMargin18.Name = "textBoxMargin18";
      this.textBoxMargin18.Size = new Size(102, 20);
      this.textBoxMargin18.TabIndex = 41;
      this.textBoxMargin18.Tag = (object) "2681";
      this.textBoxMargin18.TextAlign = HorizontalAlignment.Right;
      this.comboBoxMargin10.Font = new Font("Arial", 8.25f);
      this.comboBoxMargin10.Location = new Point(10, 120);
      this.comboBoxMargin10.Name = "comboBoxMargin10";
      this.comboBoxMargin10.Size = new Size(203, 22);
      this.comboBoxMargin10.TabIndex = 26;
      this.comboBoxMargin10.Tag = (object) "2666";
      this.comboBoxMargin17.Font = new Font("Arial", 8.25f);
      this.comboBoxMargin17.Location = new Point(10, 288);
      this.comboBoxMargin17.Name = "comboBoxMargin17";
      this.comboBoxMargin17.Size = new Size(203, 22);
      this.comboBoxMargin17.TabIndex = 40;
      this.comboBoxMargin17.Tag = (object) "2680";
      this.textBoxMargin11.Font = new Font("Arial", 8.25f);
      this.textBoxMargin11.Location = new Point(219, 121);
      this.textBoxMargin11.Name = "textBoxMargin11";
      this.textBoxMargin11.Size = new Size(102, 20);
      this.textBoxMargin11.TabIndex = 27;
      this.textBoxMargin11.Tag = (object) "2667";
      this.textBoxMargin11.TextAlign = HorizontalAlignment.Right;
      this.textBoxMargin17.Font = new Font("Arial", 8.25f);
      this.textBoxMargin17.Location = new Point(219, 265);
      this.textBoxMargin17.Name = "textBoxMargin17";
      this.textBoxMargin17.Size = new Size(102, 20);
      this.textBoxMargin17.TabIndex = 39;
      this.textBoxMargin17.Tag = (object) "2679";
      this.textBoxMargin17.TextAlign = HorizontalAlignment.Right;
      this.comboBoxMargin11.Font = new Font("Arial", 8.25f);
      this.comboBoxMargin11.Location = new Point(10, 144);
      this.comboBoxMargin11.Name = "comboBoxMargin11";
      this.comboBoxMargin11.Size = new Size(203, 22);
      this.comboBoxMargin11.TabIndex = 28;
      this.comboBoxMargin11.Tag = (object) "2668";
      this.comboBoxMargin16.Font = new Font("Arial", 8.25f);
      this.comboBoxMargin16.Location = new Point(10, 264);
      this.comboBoxMargin16.Name = "comboBoxMargin16";
      this.comboBoxMargin16.Size = new Size(203, 22);
      this.comboBoxMargin16.TabIndex = 38;
      this.comboBoxMargin16.Tag = (object) "2678";
      this.textBoxMargin12.Font = new Font("Arial", 8.25f);
      this.textBoxMargin12.Location = new Point(219, 145);
      this.textBoxMargin12.Name = "textBoxMargin12";
      this.textBoxMargin12.Size = new Size(102, 20);
      this.textBoxMargin12.TabIndex = 29;
      this.textBoxMargin12.Tag = (object) "2669";
      this.textBoxMargin12.TextAlign = HorizontalAlignment.Right;
      this.textBoxMargin16.Font = new Font("Arial", 8.25f);
      this.textBoxMargin16.Location = new Point(219, 241);
      this.textBoxMargin16.Name = "textBoxMargin16";
      this.textBoxMargin16.Size = new Size(102, 20);
      this.textBoxMargin16.TabIndex = 37;
      this.textBoxMargin16.Tag = (object) "2677";
      this.textBoxMargin16.TextAlign = HorizontalAlignment.Right;
      this.comboBoxMargin12.Font = new Font("Arial", 8.25f);
      this.comboBoxMargin12.Location = new Point(10, 168);
      this.comboBoxMargin12.Name = "comboBoxMargin12";
      this.comboBoxMargin12.Size = new Size(203, 22);
      this.comboBoxMargin12.TabIndex = 30;
      this.comboBoxMargin12.Tag = (object) "2670";
      this.comboBoxMargin15.Font = new Font("Arial", 8.25f);
      this.comboBoxMargin15.Location = new Point(10, 240);
      this.comboBoxMargin15.Name = "comboBoxMargin15";
      this.comboBoxMargin15.Size = new Size(203, 22);
      this.comboBoxMargin15.TabIndex = 36;
      this.comboBoxMargin15.Tag = (object) "2676";
      this.textBoxMargin13.Font = new Font("Arial", 8.25f);
      this.textBoxMargin13.Location = new Point(219, 169);
      this.textBoxMargin13.Name = "textBoxMargin13";
      this.textBoxMargin13.Size = new Size(102, 20);
      this.textBoxMargin13.TabIndex = 31;
      this.textBoxMargin13.Tag = (object) "2671";
      this.textBoxMargin13.TextAlign = HorizontalAlignment.Right;
      this.textBoxMargin15.Font = new Font("Arial", 8.25f);
      this.textBoxMargin15.Location = new Point(219, 217);
      this.textBoxMargin15.Name = "textBoxMargin15";
      this.textBoxMargin15.Size = new Size(102, 20);
      this.textBoxMargin15.TabIndex = 35;
      this.textBoxMargin15.Tag = (object) "2675";
      this.textBoxMargin15.TextAlign = HorizontalAlignment.Right;
      this.comboBoxMargin13.Font = new Font("Arial", 8.25f);
      this.comboBoxMargin13.Location = new Point(10, 192);
      this.comboBoxMargin13.Name = "comboBoxMargin13";
      this.comboBoxMargin13.Size = new Size(203, 22);
      this.comboBoxMargin13.TabIndex = 32;
      this.comboBoxMargin13.Tag = (object) "2672";
      this.comboBoxMargin14.Font = new Font("Arial", 8.25f);
      this.comboBoxMargin14.Location = new Point(10, 216);
      this.comboBoxMargin14.Name = "comboBoxMargin14";
      this.comboBoxMargin14.Size = new Size(203, 22);
      this.comboBoxMargin14.TabIndex = 34;
      this.comboBoxMargin14.Tag = (object) "2674";
      this.textBoxMargin14.Font = new Font("Arial", 8.25f);
      this.textBoxMargin14.Location = new Point(219, 193);
      this.textBoxMargin14.Name = "textBoxMargin14";
      this.textBoxMargin14.Size = new Size(102, 20);
      this.textBoxMargin14.TabIndex = 33;
      this.textBoxMargin14.Tag = (object) "2673";
      this.textBoxMargin14.TextAlign = HorizontalAlignment.Right;
      this.pnlBM.Controls.Add((Control) this.comboBoxMargin1);
      this.pnlBM.Controls.Add((Control) this.textBoxMargin2);
      this.pnlBM.Controls.Add((Control) this.comboBoxMargin2);
      this.pnlBM.Controls.Add((Control) this.textBoxMargin3);
      this.pnlBM.Controls.Add((Control) this.comboBoxMargin3);
      this.pnlBM.Controls.Add((Control) this.textBoxMargin4);
      this.pnlBM.Controls.Add((Control) this.comboBoxMargin4);
      this.pnlBM.Controls.Add((Control) this.textBoxMargin5);
      this.pnlBM.Controls.Add((Control) this.picBMZoomOut);
      this.pnlBM.Controls.Add((Control) this.picBMZoomIn);
      this.pnlBM.Controls.Add((Control) this.label14);
      this.pnlBM.Controls.Add((Control) this.textBoxMargin1);
      this.pnlBM.Dock = DockStyle.Top;
      this.pnlBM.Location = new Point(1, 2361);
      this.pnlBM.Name = "pnlBM";
      this.pnlBM.Size = new Size(333, 130);
      this.pnlBM.TabIndex = 185;
      this.comboBoxMargin1.Font = new Font("Arial", 8.25f);
      this.comboBoxMargin1.Location = new Point(11, 32);
      this.comboBoxMargin1.Name = "comboBoxMargin1";
      this.comboBoxMargin1.Size = new Size(203, 22);
      this.comboBoxMargin1.TabIndex = 17;
      this.comboBoxMargin1.Tag = (object) "2648";
      this.textBoxMargin2.Font = new Font("Arial", 8.25f);
      this.textBoxMargin2.Location = new Point(220, 33);
      this.textBoxMargin2.Name = "textBoxMargin2";
      this.textBoxMargin2.Size = new Size(102, 20);
      this.textBoxMargin2.TabIndex = 18;
      this.textBoxMargin2.Tag = (object) "2649";
      this.textBoxMargin2.TextAlign = HorizontalAlignment.Right;
      this.comboBoxMargin2.Font = new Font("Arial", 8.25f);
      this.comboBoxMargin2.Location = new Point(11, 56);
      this.comboBoxMargin2.Name = "comboBoxMargin2";
      this.comboBoxMargin2.Size = new Size(203, 22);
      this.comboBoxMargin2.TabIndex = 19;
      this.comboBoxMargin2.Tag = (object) "2650";
      this.textBoxMargin3.Font = new Font("Arial", 8.25f);
      this.textBoxMargin3.Location = new Point(220, 57);
      this.textBoxMargin3.Name = "textBoxMargin3";
      this.textBoxMargin3.Size = new Size(102, 20);
      this.textBoxMargin3.TabIndex = 20;
      this.textBoxMargin3.Tag = (object) "2651";
      this.textBoxMargin3.TextAlign = HorizontalAlignment.Right;
      this.comboBoxMargin3.Font = new Font("Arial", 8.25f);
      this.comboBoxMargin3.Location = new Point(11, 81);
      this.comboBoxMargin3.Name = "comboBoxMargin3";
      this.comboBoxMargin3.Size = new Size(203, 22);
      this.comboBoxMargin3.TabIndex = 21;
      this.comboBoxMargin3.Tag = (object) "2652";
      this.textBoxMargin4.Font = new Font("Arial", 8.25f);
      this.textBoxMargin4.Location = new Point(220, 81);
      this.textBoxMargin4.Name = "textBoxMargin4";
      this.textBoxMargin4.Size = new Size(102, 20);
      this.textBoxMargin4.TabIndex = 22;
      this.textBoxMargin4.Tag = (object) "2653";
      this.textBoxMargin4.TextAlign = HorizontalAlignment.Right;
      this.comboBoxMargin4.Font = new Font("Arial", 8.25f);
      this.comboBoxMargin4.Location = new Point(11, 104);
      this.comboBoxMargin4.Name = "comboBoxMargin4";
      this.comboBoxMargin4.Size = new Size(203, 22);
      this.comboBoxMargin4.TabIndex = 23;
      this.comboBoxMargin4.Tag = (object) "2654";
      this.textBoxMargin5.Font = new Font("Arial", 8.25f);
      this.textBoxMargin5.Location = new Point(220, 105);
      this.textBoxMargin5.Name = "textBoxMargin5";
      this.textBoxMargin5.Size = new Size(102, 20);
      this.textBoxMargin5.TabIndex = 24;
      this.textBoxMargin5.Tag = (object) "2655";
      this.textBoxMargin5.TextAlign = HorizontalAlignment.Right;
      this.picBMZoomOut.BackgroundImageLayout = ImageLayout.None;
      this.picBMZoomOut.Cursor = Cursors.Hand;
      this.picBMZoomOut.Image = (Image) componentResourceManager.GetObject("picBMZoomOut.Image");
      this.picBMZoomOut.Location = new Point(148, 10);
      this.picBMZoomOut.Name = "picBMZoomOut";
      this.picBMZoomOut.Size = new Size(16, 16);
      this.picBMZoomOut.SizeMode = PictureBoxSizeMode.AutoSize;
      this.picBMZoomOut.TabIndex = 16;
      this.picBMZoomOut.TabStop = false;
      this.picBMZoomIn.BackgroundImageLayout = ImageLayout.None;
      this.picBMZoomIn.Cursor = Cursors.Hand;
      this.picBMZoomIn.Image = (Image) componentResourceManager.GetObject("picBMZoomIn.Image");
      this.picBMZoomIn.Location = new Point((int) sbyte.MaxValue, 10);
      this.picBMZoomIn.Name = "picBMZoomIn";
      this.picBMZoomIn.Size = new Size(16, 16);
      this.picBMZoomIn.SizeMode = PictureBoxSizeMode.AutoSize;
      this.picBMZoomIn.TabIndex = 15;
      this.picBMZoomIn.TabStop = false;
      this.label14.AutoSize = true;
      this.label14.Font = new Font("Arial", 8.25f);
      this.label14.Location = new Point(11, 11);
      this.label14.Name = "label14";
      this.label14.Size = new Size(114, 14);
      this.label14.TabIndex = 12;
      this.label14.Text = "Base Buy ARM Margin";
      this.textBoxMargin1.Location = new Point(219, 7);
      this.textBoxMargin1.Name = "textBoxMargin1";
      this.textBoxMargin1.Size = new Size(102, 20);
      this.textBoxMargin1.TabIndex = 6;
      this.textBoxMargin1.Tag = (object) "2647";
      this.textBoxMargin1.TextAlign = HorizontalAlignment.Right;
      this.pnlNetPrice.Controls.Add((Control) this.label10);
      this.pnlNetPrice.Controls.Add((Control) this.textBoxPrice22);
      this.pnlNetPrice.Controls.Add((Control) this.groupBox5);
      this.pnlNetPrice.Controls.Add((Control) this.label9);
      this.pnlNetPrice.Controls.Add((Control) this.textBoxPrice23);
      this.pnlNetPrice.Dock = DockStyle.Top;
      this.pnlNetPrice.Location = new Point(1, 2295);
      this.pnlNetPrice.Name = "pnlNetPrice";
      this.pnlNetPrice.Size = new Size(333, 66);
      this.pnlNetPrice.TabIndex = 184;
      this.label10.AutoSize = true;
      this.label10.Font = new Font("Arial", 8.25f);
      this.label10.Location = new Point(7, 16);
      this.label10.Name = "label10";
      this.label10.Size = new Size(118, 14);
      this.label10.TabIndex = 21;
      this.label10.Text = "Total Price Adjustments";
      this.textBoxPrice22.BackColor = Color.WhiteSmoke;
      this.textBoxPrice22.Font = new Font("Arial", 8.25f);
      this.textBoxPrice22.Location = new Point(219, 13);
      this.textBoxPrice22.Name = "textBoxPrice22";
      this.textBoxPrice22.ReadOnly = true;
      this.textBoxPrice22.Size = new Size(102, 20);
      this.textBoxPrice22.TabIndex = 57;
      this.textBoxPrice22.TabStop = false;
      this.textBoxPrice22.Tag = (object) "2142";
      this.textBoxPrice22.TextAlign = HorizontalAlignment.Right;
      this.groupBox5.Dock = DockStyle.Bottom;
      this.groupBox5.Location = new Point(0, 62);
      this.groupBox5.Name = "groupBox5";
      this.groupBox5.Size = new Size(333, 4);
      this.groupBox5.TabIndex = 29;
      this.groupBox5.TabStop = false;
      this.groupBox5.Text = "groupBox5";
      this.label9.AutoSize = true;
      this.label9.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label9.Location = new Point(7, 38);
      this.label9.Name = "label9";
      this.label9.Size = new Size(79, 14);
      this.label9.TabIndex = 23;
      this.label9.Text = "Net Buy Price";
      this.textBoxPrice23.BackColor = Color.WhiteSmoke;
      this.textBoxPrice23.Font = new Font("Arial", 8.25f);
      this.textBoxPrice23.Location = new Point(219, 35);
      this.textBoxPrice23.Name = "textBoxPrice23";
      this.textBoxPrice23.ReadOnly = true;
      this.textBoxPrice23.Size = new Size(102, 20);
      this.textBoxPrice23.TabIndex = 58;
      this.textBoxPrice23.TabStop = false;
      this.textBoxPrice23.Tag = (object) "2143";
      this.textBoxPrice23.TextAlign = HorizontalAlignment.Right;
      this.pnlCpExpand.Controls.Add((Control) this.label42);
      this.pnlCpExpand.Controls.Add((Control) this.textBoxCpa10Desc);
      this.pnlCpExpand.Controls.Add((Control) this.textBoxCpa10);
      this.pnlCpExpand.Controls.Add((Control) this.label43);
      this.pnlCpExpand.Controls.Add((Control) this.textBoxCpa9Desc);
      this.pnlCpExpand.Controls.Add((Control) this.textBoxCpa9);
      this.pnlCpExpand.Controls.Add((Control) this.label44);
      this.pnlCpExpand.Controls.Add((Control) this.textBoxCpa8Desc);
      this.pnlCpExpand.Controls.Add((Control) this.textBoxCpa8);
      this.pnlCpExpand.Controls.Add((Control) this.label45);
      this.pnlCpExpand.Controls.Add((Control) this.textBoxCpa7Desc);
      this.pnlCpExpand.Controls.Add((Control) this.textBoxCpa7);
      this.pnlCpExpand.Controls.Add((Control) this.label46);
      this.pnlCpExpand.Controls.Add((Control) this.textBoxCpa6Desc);
      this.pnlCpExpand.Controls.Add((Control) this.textBoxCpa6);
      this.pnlCpExpand.Controls.Add((Control) this.label47);
      this.pnlCpExpand.Controls.Add((Control) this.textBoxCpa5Desc);
      this.pnlCpExpand.Controls.Add((Control) this.textBoxCpa5);
      this.pnlCpExpand.Controls.Add((Control) this.label48);
      this.pnlCpExpand.Controls.Add((Control) this.textBoxCpa4Desc);
      this.pnlCpExpand.Controls.Add((Control) this.textBoxCpa4);
      this.pnlCpExpand.Dock = DockStyle.Top;
      this.pnlCpExpand.Location = new Point(1, 2125);
      this.pnlCpExpand.Name = "pnlCpExpand";
      this.pnlCpExpand.Size = new Size(333, 170);
      this.pnlCpExpand.TabIndex = 182;
      this.label42.AutoSize = true;
      this.label42.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label42.Location = new Point(8, 147);
      this.label42.Name = "label42";
      this.label42.Size = new Size(47, 14);
      this.label42.TabIndex = 91;
      this.label42.Text = "CPA #10";
      this.textBoxCpa10Desc.Font = new Font("Arial", 8.25f);
      this.textBoxCpa10Desc.Location = new Point(84, 144);
      this.textBoxCpa10Desc.Name = "textBoxCpa10Desc";
      this.textBoxCpa10Desc.Size = new Size(130, 20);
      this.textBoxCpa10Desc.TabIndex = 12;
      this.textBoxCpa10Desc.Tag = (object) "4354";
      this.textBoxCpa10Desc.TextAlign = HorizontalAlignment.Right;
      this.textBoxCpa10Desc.Enter += new EventHandler(this.textField_Enter);
      this.textBoxCpa10Desc.Leave += new EventHandler(this.strField_Leave);
      this.textBoxCpa10.Font = new Font("Arial", 8.25f);
      this.textBoxCpa10.Location = new Point(220, 144);
      this.textBoxCpa10.Name = "textBoxCpa10";
      this.textBoxCpa10.Size = new Size(102, 20);
      this.textBoxCpa10.TabIndex = 13;
      this.textBoxCpa10.Tag = (object) "4355";
      this.textBoxCpa10.TextAlign = HorizontalAlignment.Right;
      this.textBoxCpa10.Enter += new EventHandler(this.textField_Enter);
      this.textBoxCpa10.KeyPress += new KeyPressEventHandler(this.numField_KeyPress);
      this.textBoxCpa10.KeyUp += new KeyEventHandler(this.numField_KeyUp);
      this.textBoxCpa10.Leave += new EventHandler(this.numField_Leave);
      this.label43.AutoSize = true;
      this.label43.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label43.Location = new Point(8, 123);
      this.label43.Name = "label43";
      this.label43.Size = new Size(41, 14);
      this.label43.TabIndex = 88;
      this.label43.Text = "CPA #9";
      this.textBoxCpa9Desc.Font = new Font("Arial", 8.25f);
      this.textBoxCpa9Desc.Location = new Point(84, 120);
      this.textBoxCpa9Desc.Name = "textBoxCpa9Desc";
      this.textBoxCpa9Desc.Size = new Size(130, 20);
      this.textBoxCpa9Desc.TabIndex = 10;
      this.textBoxCpa9Desc.Tag = (object) "4352";
      this.textBoxCpa9Desc.TextAlign = HorizontalAlignment.Right;
      this.textBoxCpa9Desc.Enter += new EventHandler(this.textField_Enter);
      this.textBoxCpa9Desc.Leave += new EventHandler(this.strField_Leave);
      this.textBoxCpa9.Font = new Font("Arial", 8.25f);
      this.textBoxCpa9.Location = new Point(220, 120);
      this.textBoxCpa9.Name = "textBoxCpa9";
      this.textBoxCpa9.Size = new Size(102, 20);
      this.textBoxCpa9.TabIndex = 11;
      this.textBoxCpa9.Tag = (object) "4353";
      this.textBoxCpa9.TextAlign = HorizontalAlignment.Right;
      this.textBoxCpa9.Enter += new EventHandler(this.textField_Enter);
      this.textBoxCpa9.KeyPress += new KeyPressEventHandler(this.numField_KeyPress);
      this.textBoxCpa9.KeyUp += new KeyEventHandler(this.numField_KeyUp);
      this.textBoxCpa9.Leave += new EventHandler(this.numField_Leave);
      this.label44.AutoSize = true;
      this.label44.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label44.Location = new Point(8, 99);
      this.label44.Name = "label44";
      this.label44.Size = new Size(41, 14);
      this.label44.TabIndex = 85;
      this.label44.Text = "CPA #8";
      this.textBoxCpa8Desc.Font = new Font("Arial", 8.25f);
      this.textBoxCpa8Desc.Location = new Point(84, 96);
      this.textBoxCpa8Desc.Name = "textBoxCpa8Desc";
      this.textBoxCpa8Desc.Size = new Size(130, 20);
      this.textBoxCpa8Desc.TabIndex = 8;
      this.textBoxCpa8Desc.Tag = (object) "4350";
      this.textBoxCpa8Desc.TextAlign = HorizontalAlignment.Right;
      this.textBoxCpa8Desc.Enter += new EventHandler(this.textField_Enter);
      this.textBoxCpa8Desc.Leave += new EventHandler(this.strField_Leave);
      this.textBoxCpa8.Font = new Font("Arial", 8.25f);
      this.textBoxCpa8.Location = new Point(220, 96);
      this.textBoxCpa8.Name = "textBoxCpa8";
      this.textBoxCpa8.Size = new Size(102, 20);
      this.textBoxCpa8.TabIndex = 9;
      this.textBoxCpa8.Tag = (object) "4351";
      this.textBoxCpa8.TextAlign = HorizontalAlignment.Right;
      this.textBoxCpa8.Enter += new EventHandler(this.textField_Enter);
      this.textBoxCpa8.KeyPress += new KeyPressEventHandler(this.numField_KeyPress);
      this.textBoxCpa8.KeyUp += new KeyEventHandler(this.numField_KeyUp);
      this.textBoxCpa8.Leave += new EventHandler(this.numField_Leave);
      this.label45.AutoSize = true;
      this.label45.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label45.Location = new Point(8, 75);
      this.label45.Name = "label45";
      this.label45.Size = new Size(41, 14);
      this.label45.TabIndex = 82;
      this.label45.Text = "CPA #7";
      this.textBoxCpa7Desc.Font = new Font("Arial", 8.25f);
      this.textBoxCpa7Desc.Location = new Point(84, 72);
      this.textBoxCpa7Desc.Name = "textBoxCpa7Desc";
      this.textBoxCpa7Desc.Size = new Size(130, 20);
      this.textBoxCpa7Desc.TabIndex = 6;
      this.textBoxCpa7Desc.Tag = (object) "4348";
      this.textBoxCpa7Desc.TextAlign = HorizontalAlignment.Right;
      this.textBoxCpa7Desc.Enter += new EventHandler(this.textField_Enter);
      this.textBoxCpa7Desc.Leave += new EventHandler(this.strField_Leave);
      this.textBoxCpa7.Font = new Font("Arial", 8.25f);
      this.textBoxCpa7.Location = new Point(220, 72);
      this.textBoxCpa7.Name = "textBoxCpa7";
      this.textBoxCpa7.Size = new Size(102, 20);
      this.textBoxCpa7.TabIndex = 7;
      this.textBoxCpa7.Tag = (object) "4349";
      this.textBoxCpa7.TextAlign = HorizontalAlignment.Right;
      this.textBoxCpa7.Enter += new EventHandler(this.textField_Enter);
      this.textBoxCpa7.KeyPress += new KeyPressEventHandler(this.numField_KeyPress);
      this.textBoxCpa7.KeyUp += new KeyEventHandler(this.numField_KeyUp);
      this.textBoxCpa7.Leave += new EventHandler(this.numField_Leave);
      this.label46.AutoSize = true;
      this.label46.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label46.Location = new Point(8, 51);
      this.label46.Name = "label46";
      this.label46.Size = new Size(41, 14);
      this.label46.TabIndex = 79;
      this.label46.Text = "CPA #6";
      this.textBoxCpa6Desc.Font = new Font("Arial", 8.25f);
      this.textBoxCpa6Desc.Location = new Point(84, 48);
      this.textBoxCpa6Desc.Name = "textBoxCpa6Desc";
      this.textBoxCpa6Desc.Size = new Size(130, 20);
      this.textBoxCpa6Desc.TabIndex = 4;
      this.textBoxCpa6Desc.Tag = (object) "4346";
      this.textBoxCpa6Desc.TextAlign = HorizontalAlignment.Right;
      this.textBoxCpa6Desc.Enter += new EventHandler(this.textField_Enter);
      this.textBoxCpa6Desc.Leave += new EventHandler(this.strField_Leave);
      this.textBoxCpa6.Font = new Font("Arial", 8.25f);
      this.textBoxCpa6.Location = new Point(220, 48);
      this.textBoxCpa6.Name = "textBoxCpa6";
      this.textBoxCpa6.Size = new Size(102, 20);
      this.textBoxCpa6.TabIndex = 5;
      this.textBoxCpa6.Tag = (object) "4347";
      this.textBoxCpa6.TextAlign = HorizontalAlignment.Right;
      this.textBoxCpa6.Enter += new EventHandler(this.textField_Enter);
      this.textBoxCpa6.KeyPress += new KeyPressEventHandler(this.numField_KeyPress);
      this.textBoxCpa6.KeyUp += new KeyEventHandler(this.numField_KeyUp);
      this.textBoxCpa6.Leave += new EventHandler(this.numField_Leave);
      this.label47.AutoSize = true;
      this.label47.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label47.Location = new Point(7, 27);
      this.label47.Name = "label47";
      this.label47.Size = new Size(41, 14);
      this.label47.TabIndex = 76;
      this.label47.Text = "CPA #5";
      this.textBoxCpa5Desc.Font = new Font("Arial", 8.25f);
      this.textBoxCpa5Desc.Location = new Point(83, 24);
      this.textBoxCpa5Desc.Name = "textBoxCpa5Desc";
      this.textBoxCpa5Desc.Size = new Size(130, 20);
      this.textBoxCpa5Desc.TabIndex = 2;
      this.textBoxCpa5Desc.Tag = (object) "4344";
      this.textBoxCpa5Desc.TextAlign = HorizontalAlignment.Right;
      this.textBoxCpa5Desc.Enter += new EventHandler(this.textField_Enter);
      this.textBoxCpa5Desc.Leave += new EventHandler(this.strField_Leave);
      this.textBoxCpa5.Font = new Font("Arial", 8.25f);
      this.textBoxCpa5.Location = new Point(219, 24);
      this.textBoxCpa5.Name = "textBoxCpa5";
      this.textBoxCpa5.Size = new Size(102, 20);
      this.textBoxCpa5.TabIndex = 3;
      this.textBoxCpa5.Tag = (object) "4345";
      this.textBoxCpa5.TextAlign = HorizontalAlignment.Right;
      this.textBoxCpa5.Enter += new EventHandler(this.textField_Enter);
      this.textBoxCpa5.KeyPress += new KeyPressEventHandler(this.numField_KeyPress);
      this.textBoxCpa5.KeyUp += new KeyEventHandler(this.numField_KeyUp);
      this.textBoxCpa5.Leave += new EventHandler(this.numField_Leave);
      this.label48.AutoSize = true;
      this.label48.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label48.Location = new Point(7, 3);
      this.label48.Name = "label48";
      this.label48.Size = new Size(41, 14);
      this.label48.TabIndex = 73;
      this.label48.Text = "CPA #4";
      this.textBoxCpa4Desc.Font = new Font("Arial", 8.25f);
      this.textBoxCpa4Desc.Location = new Point(83, 0);
      this.textBoxCpa4Desc.Name = "textBoxCpa4Desc";
      this.textBoxCpa4Desc.Size = new Size(130, 20);
      this.textBoxCpa4Desc.TabIndex = 0;
      this.textBoxCpa4Desc.Tag = (object) "4342";
      this.textBoxCpa4Desc.TextAlign = HorizontalAlignment.Right;
      this.textBoxCpa4Desc.Enter += new EventHandler(this.textField_Enter);
      this.textBoxCpa4Desc.Leave += new EventHandler(this.strField_Leave);
      this.textBoxCpa4.Font = new Font("Arial", 8.25f);
      this.textBoxCpa4.Location = new Point(219, 0);
      this.textBoxCpa4.Name = "textBoxCpa4";
      this.textBoxCpa4.Size = new Size(102, 20);
      this.textBoxCpa4.TabIndex = 1;
      this.textBoxCpa4.Tag = (object) "4343";
      this.textBoxCpa4.TextAlign = HorizontalAlignment.Right;
      this.textBoxCpa4.Enter += new EventHandler(this.textField_Enter);
      this.textBoxCpa4.KeyPress += new KeyPressEventHandler(this.numField_KeyPress);
      this.textBoxCpa4.KeyUp += new KeyEventHandler(this.numField_KeyUp);
      this.textBoxCpa4.Leave += new EventHandler(this.numField_Leave);
      this.pnlCp.Controls.Add((Control) this.label49);
      this.pnlCp.Controls.Add((Control) this.label50);
      this.pnlCp.Controls.Add((Control) this.label51);
      this.pnlCp.Controls.Add((Control) this.textBoxCpa3Desc);
      this.pnlCp.Controls.Add((Control) this.textBoxCpa3);
      this.pnlCp.Controls.Add((Control) this.textBoxCpa2Desc);
      this.pnlCp.Controls.Add((Control) this.textBoxCpa2);
      this.pnlCp.Controls.Add((Control) this.textBoxCpa1Desc);
      this.pnlCp.Controls.Add((Control) this.textBoxCpa1);
      this.pnlCp.Controls.Add((Control) this.picCPZoomOut);
      this.pnlCp.Controls.Add((Control) this.picCPZoomIn);
      this.pnlCp.Controls.Add((Control) this.label52);
      this.pnlCp.Dock = DockStyle.Top;
      this.pnlCp.Location = new Point(1, 2025);
      this.pnlCp.Name = "pnlCp";
      this.pnlCp.Size = new Size(333, 100);
      this.pnlCp.TabIndex = 183;
      this.label49.AutoSize = true;
      this.label49.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label49.Location = new Point(7, 80);
      this.label49.Name = "label49";
      this.label49.Size = new Size(41, 14);
      this.label49.TabIndex = 72;
      this.label49.Text = "CPA #3";
      this.label50.AutoSize = true;
      this.label50.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label50.Location = new Point(7, 55);
      this.label50.Name = "label50";
      this.label50.Size = new Size(41, 14);
      this.label50.TabIndex = 71;
      this.label50.Text = "CPA #2";
      this.label51.AutoSize = true;
      this.label51.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label51.Location = new Point(7, 31);
      this.label51.Name = "label51";
      this.label51.Size = new Size(41, 14);
      this.label51.TabIndex = 70;
      this.label51.Text = "CPA #1";
      this.textBoxCpa3Desc.Font = new Font("Arial", 8.25f);
      this.textBoxCpa3Desc.Location = new Point(84, 76);
      this.textBoxCpa3Desc.Name = "textBoxCpa3Desc";
      this.textBoxCpa3Desc.Size = new Size(130, 20);
      this.textBoxCpa3Desc.TabIndex = 4;
      this.textBoxCpa3Desc.Tag = (object) "4340";
      this.textBoxCpa3Desc.TextAlign = HorizontalAlignment.Right;
      this.textBoxCpa3Desc.Enter += new EventHandler(this.textField_Enter);
      this.textBoxCpa3Desc.Leave += new EventHandler(this.strField_Leave);
      this.textBoxCpa3.Font = new Font("Arial", 8.25f);
      this.textBoxCpa3.Location = new Point(219, 76);
      this.textBoxCpa3.Name = "textBoxCpa3";
      this.textBoxCpa3.Size = new Size(102, 20);
      this.textBoxCpa3.TabIndex = 5;
      this.textBoxCpa3.Tag = (object) "4341";
      this.textBoxCpa3.TextAlign = HorizontalAlignment.Right;
      this.textBoxCpa3.Enter += new EventHandler(this.textField_Enter);
      this.textBoxCpa3.KeyPress += new KeyPressEventHandler(this.numField_KeyPress);
      this.textBoxCpa3.KeyUp += new KeyEventHandler(this.numField_KeyUp);
      this.textBoxCpa3.Leave += new EventHandler(this.numField_Leave);
      this.textBoxCpa2Desc.Font = new Font("Arial", 8.25f);
      this.textBoxCpa2Desc.Location = new Point(84, 52);
      this.textBoxCpa2Desc.Name = "textBoxCpa2Desc";
      this.textBoxCpa2Desc.Size = new Size(130, 20);
      this.textBoxCpa2Desc.TabIndex = 2;
      this.textBoxCpa2Desc.Tag = (object) "4338";
      this.textBoxCpa2Desc.TextAlign = HorizontalAlignment.Right;
      this.textBoxCpa2Desc.Enter += new EventHandler(this.textField_Enter);
      this.textBoxCpa2Desc.Leave += new EventHandler(this.strField_Leave);
      this.textBoxCpa2.Font = new Font("Arial", 8.25f);
      this.textBoxCpa2.Location = new Point(219, 52);
      this.textBoxCpa2.Name = "textBoxCpa2";
      this.textBoxCpa2.Size = new Size(102, 20);
      this.textBoxCpa2.TabIndex = 3;
      this.textBoxCpa2.Tag = (object) "4339";
      this.textBoxCpa2.TextAlign = HorizontalAlignment.Right;
      this.textBoxCpa2.Enter += new EventHandler(this.textField_Enter);
      this.textBoxCpa2.KeyPress += new KeyPressEventHandler(this.numField_KeyPress);
      this.textBoxCpa2.KeyUp += new KeyEventHandler(this.numField_KeyUp);
      this.textBoxCpa2.Leave += new EventHandler(this.numField_Leave);
      this.textBoxCpa1Desc.Font = new Font("Arial", 8.25f);
      this.textBoxCpa1Desc.Location = new Point(83, 28);
      this.textBoxCpa1Desc.Name = "textBoxCpa1Desc";
      this.textBoxCpa1Desc.Size = new Size(130, 20);
      this.textBoxCpa1Desc.TabIndex = 0;
      this.textBoxCpa1Desc.Tag = (object) "4336";
      this.textBoxCpa1Desc.TextAlign = HorizontalAlignment.Right;
      this.textBoxCpa1Desc.Enter += new EventHandler(this.textField_Enter);
      this.textBoxCpa1Desc.Leave += new EventHandler(this.strField_Leave);
      this.textBoxCpa1.Font = new Font("Arial", 8.25f);
      this.textBoxCpa1.Location = new Point(219, 28);
      this.textBoxCpa1.Name = "textBoxCpa1";
      this.textBoxCpa1.Size = new Size(102, 20);
      this.textBoxCpa1.TabIndex = 1;
      this.textBoxCpa1.Tag = (object) "4337";
      this.textBoxCpa1.TextAlign = HorizontalAlignment.Right;
      this.textBoxCpa1.Enter += new EventHandler(this.textField_Enter);
      this.textBoxCpa1.KeyPress += new KeyPressEventHandler(this.numField_KeyPress);
      this.textBoxCpa1.KeyUp += new KeyEventHandler(this.numField_KeyUp);
      this.textBoxCpa1.Leave += new EventHandler(this.numField_Leave);
      this.picCPZoomOut.BackgroundImageLayout = ImageLayout.None;
      this.picCPZoomOut.Cursor = Cursors.Hand;
      this.picCPZoomOut.Image = (Image) componentResourceManager.GetObject("picCPZoomOut.Image");
      this.picCPZoomOut.Location = new Point(161, 5);
      this.picCPZoomOut.Name = "picCPZoomOut";
      this.picCPZoomOut.Size = new Size(16, 16);
      this.picCPZoomOut.SizeMode = PictureBoxSizeMode.AutoSize;
      this.picCPZoomOut.TabIndex = 51;
      this.picCPZoomOut.TabStop = false;
      this.picCPZoomOut.Click += new EventHandler(this.ZoomButton_Clicked);
      this.picCPZoomIn.BackgroundImageLayout = ImageLayout.None;
      this.picCPZoomIn.Cursor = Cursors.Hand;
      this.picCPZoomIn.Image = (Image) componentResourceManager.GetObject("picCPZoomIn.Image");
      this.picCPZoomIn.Location = new Point(139, 5);
      this.picCPZoomIn.Name = "picCPZoomIn";
      this.picCPZoomIn.Size = new Size(16, 16);
      this.picCPZoomIn.SizeMode = PictureBoxSizeMode.AutoSize;
      this.picCPZoomIn.TabIndex = 50;
      this.picCPZoomIn.TabStop = false;
      this.picCPZoomIn.Click += new EventHandler(this.ZoomButton_Clicked);
      this.label52.AutoSize = true;
      this.label52.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label52.Location = new Point(7, 7);
      this.label52.Name = "label52";
      this.label52.Size = new Size(132, 14);
      this.label52.TabIndex = 0;
      this.label52.Text = "Custom Price Adjustments";
      this.pnlRlExpand.Controls.Add((Control) this.label28);
      this.pnlRlExpand.Controls.Add((Control) this.textBoxReLock10Desc);
      this.pnlRlExpand.Controls.Add((Control) this.textBoxReLock10);
      this.pnlRlExpand.Controls.Add((Control) this.label29);
      this.pnlRlExpand.Controls.Add((Control) this.textBoxReLock9Desc);
      this.pnlRlExpand.Controls.Add((Control) this.textBoxReLock9);
      this.pnlRlExpand.Controls.Add((Control) this.label30);
      this.pnlRlExpand.Controls.Add((Control) this.textBoxReLock8Desc);
      this.pnlRlExpand.Controls.Add((Control) this.textBoxReLock8);
      this.pnlRlExpand.Controls.Add((Control) this.label32);
      this.pnlRlExpand.Controls.Add((Control) this.textBoxReLock7Desc);
      this.pnlRlExpand.Controls.Add((Control) this.textBoxReLock7);
      this.pnlRlExpand.Controls.Add((Control) this.label33);
      this.pnlRlExpand.Controls.Add((Control) this.textBoxReLock6Desc);
      this.pnlRlExpand.Controls.Add((Control) this.textBoxReLock6);
      this.pnlRlExpand.Controls.Add((Control) this.label34);
      this.pnlRlExpand.Controls.Add((Control) this.textBoxReLock5Desc);
      this.pnlRlExpand.Controls.Add((Control) this.textBoxReLock5);
      this.pnlRlExpand.Controls.Add((Control) this.label35);
      this.pnlRlExpand.Controls.Add((Control) this.textBoxReLock4Desc);
      this.pnlRlExpand.Controls.Add((Control) this.textBoxReLock4);
      this.pnlRlExpand.Dock = DockStyle.Top;
      this.pnlRlExpand.Location = new Point(1, 1855);
      this.pnlRlExpand.Name = "pnlRlExpand";
      this.pnlRlExpand.Size = new Size(333, 170);
      this.pnlRlExpand.TabIndex = 175;
      this.label28.AutoSize = true;
      this.label28.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label28.Location = new Point(8, 147);
      this.label28.Name = "label28";
      this.label28.Size = new Size(68, 14);
      this.label28.TabIndex = 91;
      this.label28.Text = "Re-Lock #10";
      this.textBoxReLock10Desc.Font = new Font("Arial", 8.25f);
      this.textBoxReLock10Desc.Location = new Point(84, 144);
      this.textBoxReLock10Desc.Name = "textBoxReLock10Desc";
      this.textBoxReLock10Desc.Size = new Size(130, 20);
      this.textBoxReLock10Desc.TabIndex = 12;
      this.textBoxReLock10Desc.Tag = (object) "4274";
      this.textBoxReLock10Desc.TextAlign = HorizontalAlignment.Right;
      this.textBoxReLock10Desc.Enter += new EventHandler(this.textField_Enter);
      this.textBoxReLock10Desc.Leave += new EventHandler(this.strField_Leave);
      this.textBoxReLock10.Font = new Font("Arial", 8.25f);
      this.textBoxReLock10.Location = new Point(220, 144);
      this.textBoxReLock10.Name = "textBoxReLock10";
      this.textBoxReLock10.Size = new Size(102, 20);
      this.textBoxReLock10.TabIndex = 13;
      this.textBoxReLock10.Tag = (object) "4275";
      this.textBoxReLock10.TextAlign = HorizontalAlignment.Right;
      this.textBoxReLock10.Enter += new EventHandler(this.textField_Enter);
      this.textBoxReLock10.KeyPress += new KeyPressEventHandler(this.numField_KeyPress);
      this.textBoxReLock10.KeyUp += new KeyEventHandler(this.numField_KeyUp);
      this.textBoxReLock10.Leave += new EventHandler(this.numField_Leave);
      this.label29.AutoSize = true;
      this.label29.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label29.Location = new Point(8, 123);
      this.label29.Name = "label29";
      this.label29.Size = new Size(62, 14);
      this.label29.TabIndex = 88;
      this.label29.Text = "Re-Lock #9";
      this.textBoxReLock9Desc.Font = new Font("Arial", 8.25f);
      this.textBoxReLock9Desc.Location = new Point(84, 120);
      this.textBoxReLock9Desc.Name = "textBoxReLock9Desc";
      this.textBoxReLock9Desc.Size = new Size(130, 20);
      this.textBoxReLock9Desc.TabIndex = 10;
      this.textBoxReLock9Desc.Tag = (object) "4272";
      this.textBoxReLock9Desc.TextAlign = HorizontalAlignment.Right;
      this.textBoxReLock9Desc.Enter += new EventHandler(this.textField_Enter);
      this.textBoxReLock9Desc.Leave += new EventHandler(this.strField_Leave);
      this.textBoxReLock9.Font = new Font("Arial", 8.25f);
      this.textBoxReLock9.Location = new Point(220, 120);
      this.textBoxReLock9.Name = "textBoxReLock9";
      this.textBoxReLock9.Size = new Size(102, 20);
      this.textBoxReLock9.TabIndex = 11;
      this.textBoxReLock9.Tag = (object) "4273";
      this.textBoxReLock9.TextAlign = HorizontalAlignment.Right;
      this.textBoxReLock9.Enter += new EventHandler(this.textField_Enter);
      this.textBoxReLock9.KeyPress += new KeyPressEventHandler(this.numField_KeyPress);
      this.textBoxReLock9.KeyUp += new KeyEventHandler(this.numField_KeyUp);
      this.textBoxReLock9.Leave += new EventHandler(this.numField_Leave);
      this.label30.AutoSize = true;
      this.label30.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label30.Location = new Point(8, 99);
      this.label30.Name = "label30";
      this.label30.Size = new Size(62, 14);
      this.label30.TabIndex = 85;
      this.label30.Text = "Re-Lock #8";
      this.textBoxReLock8Desc.Font = new Font("Arial", 8.25f);
      this.textBoxReLock8Desc.Location = new Point(84, 96);
      this.textBoxReLock8Desc.Name = "textBoxReLock8Desc";
      this.textBoxReLock8Desc.Size = new Size(130, 20);
      this.textBoxReLock8Desc.TabIndex = 8;
      this.textBoxReLock8Desc.Tag = (object) "4270";
      this.textBoxReLock8Desc.TextAlign = HorizontalAlignment.Right;
      this.textBoxReLock8Desc.Enter += new EventHandler(this.textField_Enter);
      this.textBoxReLock8Desc.Leave += new EventHandler(this.strField_Leave);
      this.textBoxReLock8.Font = new Font("Arial", 8.25f);
      this.textBoxReLock8.Location = new Point(220, 96);
      this.textBoxReLock8.Name = "textBoxReLock8";
      this.textBoxReLock8.Size = new Size(102, 20);
      this.textBoxReLock8.TabIndex = 9;
      this.textBoxReLock8.Tag = (object) "4271";
      this.textBoxReLock8.TextAlign = HorizontalAlignment.Right;
      this.textBoxReLock8.Enter += new EventHandler(this.textField_Enter);
      this.textBoxReLock8.KeyPress += new KeyPressEventHandler(this.numField_KeyPress);
      this.textBoxReLock8.KeyUp += new KeyEventHandler(this.numField_KeyUp);
      this.textBoxReLock8.Leave += new EventHandler(this.numField_Leave);
      this.label32.AutoSize = true;
      this.label32.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label32.Location = new Point(8, 75);
      this.label32.Name = "label32";
      this.label32.Size = new Size(62, 14);
      this.label32.TabIndex = 82;
      this.label32.Text = "Re-Lock #7";
      this.textBoxReLock7Desc.Font = new Font("Arial", 8.25f);
      this.textBoxReLock7Desc.Location = new Point(84, 72);
      this.textBoxReLock7Desc.Name = "textBoxReLock7Desc";
      this.textBoxReLock7Desc.Size = new Size(130, 20);
      this.textBoxReLock7Desc.TabIndex = 6;
      this.textBoxReLock7Desc.Tag = (object) "4268";
      this.textBoxReLock7Desc.TextAlign = HorizontalAlignment.Right;
      this.textBoxReLock7Desc.Enter += new EventHandler(this.textField_Enter);
      this.textBoxReLock7Desc.Leave += new EventHandler(this.strField_Leave);
      this.textBoxReLock7.Font = new Font("Arial", 8.25f);
      this.textBoxReLock7.Location = new Point(220, 72);
      this.textBoxReLock7.Name = "textBoxReLock7";
      this.textBoxReLock7.Size = new Size(102, 20);
      this.textBoxReLock7.TabIndex = 7;
      this.textBoxReLock7.Tag = (object) "4269";
      this.textBoxReLock7.TextAlign = HorizontalAlignment.Right;
      this.textBoxReLock7.Enter += new EventHandler(this.textField_Enter);
      this.textBoxReLock7.KeyPress += new KeyPressEventHandler(this.numField_KeyPress);
      this.textBoxReLock7.KeyUp += new KeyEventHandler(this.numField_KeyUp);
      this.textBoxReLock7.Leave += new EventHandler(this.numField_Leave);
      this.label33.AutoSize = true;
      this.label33.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label33.Location = new Point(8, 51);
      this.label33.Name = "label33";
      this.label33.Size = new Size(62, 14);
      this.label33.TabIndex = 79;
      this.label33.Text = "Re-Lock #6";
      this.textBoxReLock6Desc.Font = new Font("Arial", 8.25f);
      this.textBoxReLock6Desc.Location = new Point(84, 48);
      this.textBoxReLock6Desc.Name = "textBoxReLock6Desc";
      this.textBoxReLock6Desc.Size = new Size(130, 20);
      this.textBoxReLock6Desc.TabIndex = 4;
      this.textBoxReLock6Desc.Tag = (object) "4266";
      this.textBoxReLock6Desc.TextAlign = HorizontalAlignment.Right;
      this.textBoxReLock6Desc.Enter += new EventHandler(this.textField_Enter);
      this.textBoxReLock6Desc.Leave += new EventHandler(this.strField_Leave);
      this.textBoxReLock6.Font = new Font("Arial", 8.25f);
      this.textBoxReLock6.Location = new Point(220, 48);
      this.textBoxReLock6.Name = "textBoxReLock6";
      this.textBoxReLock6.Size = new Size(102, 20);
      this.textBoxReLock6.TabIndex = 5;
      this.textBoxReLock6.Tag = (object) "4267";
      this.textBoxReLock6.TextAlign = HorizontalAlignment.Right;
      this.textBoxReLock6.Enter += new EventHandler(this.textField_Enter);
      this.textBoxReLock6.KeyPress += new KeyPressEventHandler(this.numField_KeyPress);
      this.textBoxReLock6.KeyUp += new KeyEventHandler(this.numField_KeyUp);
      this.textBoxReLock6.Leave += new EventHandler(this.numField_Leave);
      this.label34.AutoSize = true;
      this.label34.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label34.Location = new Point(7, 27);
      this.label34.Name = "label34";
      this.label34.Size = new Size(62, 14);
      this.label34.TabIndex = 76;
      this.label34.Text = "Re-Lock #5";
      this.textBoxReLock5Desc.Font = new Font("Arial", 8.25f);
      this.textBoxReLock5Desc.Location = new Point(83, 24);
      this.textBoxReLock5Desc.Name = "textBoxReLock5Desc";
      this.textBoxReLock5Desc.Size = new Size(130, 20);
      this.textBoxReLock5Desc.TabIndex = 2;
      this.textBoxReLock5Desc.Tag = (object) "4264";
      this.textBoxReLock5Desc.TextAlign = HorizontalAlignment.Right;
      this.textBoxReLock5Desc.Enter += new EventHandler(this.textField_Enter);
      this.textBoxReLock5Desc.Leave += new EventHandler(this.strField_Leave);
      this.textBoxReLock5.Font = new Font("Arial", 8.25f);
      this.textBoxReLock5.Location = new Point(219, 24);
      this.textBoxReLock5.Name = "textBoxReLock5";
      this.textBoxReLock5.Size = new Size(102, 20);
      this.textBoxReLock5.TabIndex = 3;
      this.textBoxReLock5.Tag = (object) "4265";
      this.textBoxReLock5.TextAlign = HorizontalAlignment.Right;
      this.textBoxReLock5.Enter += new EventHandler(this.textField_Enter);
      this.textBoxReLock5.KeyPress += new KeyPressEventHandler(this.numField_KeyPress);
      this.textBoxReLock5.KeyUp += new KeyEventHandler(this.numField_KeyUp);
      this.textBoxReLock5.Leave += new EventHandler(this.numField_Leave);
      this.label35.AutoSize = true;
      this.label35.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label35.Location = new Point(7, 3);
      this.label35.Name = "label35";
      this.label35.Size = new Size(62, 14);
      this.label35.TabIndex = 73;
      this.label35.Text = "Re-Lock #4";
      this.textBoxReLock4Desc.Font = new Font("Arial", 8.25f);
      this.textBoxReLock4Desc.Location = new Point(83, 0);
      this.textBoxReLock4Desc.Name = "textBoxReLock4Desc";
      this.textBoxReLock4Desc.Size = new Size(130, 20);
      this.textBoxReLock4Desc.TabIndex = 0;
      this.textBoxReLock4Desc.Tag = (object) "4262";
      this.textBoxReLock4Desc.TextAlign = HorizontalAlignment.Right;
      this.textBoxReLock4Desc.Enter += new EventHandler(this.textField_Enter);
      this.textBoxReLock4Desc.Leave += new EventHandler(this.strField_Leave);
      this.textBoxReLock4.Font = new Font("Arial", 8.25f);
      this.textBoxReLock4.Location = new Point(219, 0);
      this.textBoxReLock4.Name = "textBoxReLock4";
      this.textBoxReLock4.Size = new Size(102, 20);
      this.textBoxReLock4.TabIndex = 1;
      this.textBoxReLock4.Tag = (object) "4263";
      this.textBoxReLock4.TextAlign = HorizontalAlignment.Right;
      this.textBoxReLock4.Enter += new EventHandler(this.textField_Enter);
      this.textBoxReLock4.KeyPress += new KeyPressEventHandler(this.numField_KeyPress);
      this.textBoxReLock4.KeyUp += new KeyEventHandler(this.numField_KeyUp);
      this.textBoxReLock4.Leave += new EventHandler(this.numField_Leave);
      this.pnlRl.Controls.Add((Control) this.label36);
      this.pnlRl.Controls.Add((Control) this.label37);
      this.pnlRl.Controls.Add((Control) this.label38);
      this.pnlRl.Controls.Add((Control) this.textBoxReLock3Desc);
      this.pnlRl.Controls.Add((Control) this.textBoxReLock3);
      this.pnlRl.Controls.Add((Control) this.textBoxReLock2Desc);
      this.pnlRl.Controls.Add((Control) this.textBoxReLock2);
      this.pnlRl.Controls.Add((Control) this.textBoxReLock1Desc);
      this.pnlRl.Controls.Add((Control) this.textBoxReLock1);
      this.pnlRl.Controls.Add((Control) this.picRLZoomOut);
      this.pnlRl.Controls.Add((Control) this.picRLZoomIn);
      this.pnlRl.Controls.Add((Control) this.label39);
      this.pnlRl.Dock = DockStyle.Top;
      this.pnlRl.Location = new Point(1, 1755);
      this.pnlRl.Name = "pnlRl";
      this.pnlRl.Size = new Size(333, 100);
      this.pnlRl.TabIndex = 176;
      this.label36.AutoSize = true;
      this.label36.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label36.Location = new Point(7, 80);
      this.label36.Name = "label36";
      this.label36.Size = new Size(62, 14);
      this.label36.TabIndex = 72;
      this.label36.Text = "Re-Lock #3";
      this.label37.AutoSize = true;
      this.label37.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label37.Location = new Point(7, 55);
      this.label37.Name = "label37";
      this.label37.Size = new Size(62, 14);
      this.label37.TabIndex = 71;
      this.label37.Text = "Re-Lock #2";
      this.label38.AutoSize = true;
      this.label38.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label38.Location = new Point(7, 31);
      this.label38.Name = "label38";
      this.label38.Size = new Size(62, 14);
      this.label38.TabIndex = 70;
      this.label38.Text = "Re-Lock #1";
      this.textBoxReLock3Desc.Font = new Font("Arial", 8.25f);
      this.textBoxReLock3Desc.Location = new Point(84, 76);
      this.textBoxReLock3Desc.Name = "textBoxReLock3Desc";
      this.textBoxReLock3Desc.Size = new Size(130, 20);
      this.textBoxReLock3Desc.TabIndex = 4;
      this.textBoxReLock3Desc.Tag = (object) "4260";
      this.textBoxReLock3Desc.TextAlign = HorizontalAlignment.Right;
      this.textBoxReLock3Desc.Enter += new EventHandler(this.textField_Enter);
      this.textBoxReLock3Desc.Leave += new EventHandler(this.strField_Leave);
      this.textBoxReLock3.Font = new Font("Arial", 8.25f);
      this.textBoxReLock3.Location = new Point(219, 76);
      this.textBoxReLock3.Name = "textBoxReLock3";
      this.textBoxReLock3.Size = new Size(102, 20);
      this.textBoxReLock3.TabIndex = 5;
      this.textBoxReLock3.Tag = (object) "4261";
      this.textBoxReLock3.TextAlign = HorizontalAlignment.Right;
      this.textBoxReLock3.Enter += new EventHandler(this.textField_Enter);
      this.textBoxReLock3.KeyPress += new KeyPressEventHandler(this.numField_KeyPress);
      this.textBoxReLock3.KeyUp += new KeyEventHandler(this.numField_KeyUp);
      this.textBoxReLock3.Leave += new EventHandler(this.numField_Leave);
      this.textBoxReLock2Desc.Font = new Font("Arial", 8.25f);
      this.textBoxReLock2Desc.Location = new Point(84, 52);
      this.textBoxReLock2Desc.Name = "textBoxReLock2Desc";
      this.textBoxReLock2Desc.Size = new Size(130, 20);
      this.textBoxReLock2Desc.TabIndex = 2;
      this.textBoxReLock2Desc.Tag = (object) "4258";
      this.textBoxReLock2Desc.TextAlign = HorizontalAlignment.Right;
      this.textBoxReLock2Desc.Enter += new EventHandler(this.textField_Enter);
      this.textBoxReLock2Desc.Leave += new EventHandler(this.strField_Leave);
      this.textBoxReLock2.Font = new Font("Arial", 8.25f);
      this.textBoxReLock2.Location = new Point(219, 52);
      this.textBoxReLock2.Name = "textBoxReLock2";
      this.textBoxReLock2.Size = new Size(102, 20);
      this.textBoxReLock2.TabIndex = 3;
      this.textBoxReLock2.Tag = (object) "4259";
      this.textBoxReLock2.TextAlign = HorizontalAlignment.Right;
      this.textBoxReLock2.Enter += new EventHandler(this.textField_Enter);
      this.textBoxReLock2.KeyPress += new KeyPressEventHandler(this.numField_KeyPress);
      this.textBoxReLock2.KeyUp += new KeyEventHandler(this.numField_KeyUp);
      this.textBoxReLock2.Leave += new EventHandler(this.numField_Leave);
      this.textBoxReLock1Desc.Font = new Font("Arial", 8.25f);
      this.textBoxReLock1Desc.Location = new Point(83, 28);
      this.textBoxReLock1Desc.Name = "textBoxReLock1Desc";
      this.textBoxReLock1Desc.Size = new Size(130, 20);
      this.textBoxReLock1Desc.TabIndex = 0;
      this.textBoxReLock1Desc.Tag = (object) "4256";
      this.textBoxReLock1Desc.TextAlign = HorizontalAlignment.Right;
      this.textBoxReLock1Desc.Enter += new EventHandler(this.textField_Enter);
      this.textBoxReLock1Desc.Leave += new EventHandler(this.strField_Leave);
      this.textBoxReLock1.Font = new Font("Arial", 8.25f);
      this.textBoxReLock1.Location = new Point(219, 28);
      this.textBoxReLock1.Name = "textBoxReLock1";
      this.textBoxReLock1.Size = new Size(102, 20);
      this.textBoxReLock1.TabIndex = 1;
      this.textBoxReLock1.Tag = (object) "4257";
      this.textBoxReLock1.TextAlign = HorizontalAlignment.Right;
      this.textBoxReLock1.Enter += new EventHandler(this.textField_Enter);
      this.textBoxReLock1.KeyPress += new KeyPressEventHandler(this.numField_KeyPress);
      this.textBoxReLock1.KeyUp += new KeyEventHandler(this.numField_KeyUp);
      this.textBoxReLock1.Leave += new EventHandler(this.numField_Leave);
      this.picRLZoomOut.BackgroundImageLayout = ImageLayout.None;
      this.picRLZoomOut.Cursor = Cursors.Hand;
      this.picRLZoomOut.Image = (Image) componentResourceManager.GetObject("picRLZoomOut.Image");
      this.picRLZoomOut.Location = new Point(103, 5);
      this.picRLZoomOut.Name = "picRLZoomOut";
      this.picRLZoomOut.Size = new Size(16, 16);
      this.picRLZoomOut.SizeMode = PictureBoxSizeMode.AutoSize;
      this.picRLZoomOut.TabIndex = 51;
      this.picRLZoomOut.TabStop = false;
      this.picRLZoomOut.Click += new EventHandler(this.ZoomButton_Clicked);
      this.picRLZoomIn.BackgroundImageLayout = ImageLayout.None;
      this.picRLZoomIn.Cursor = Cursors.Hand;
      this.picRLZoomIn.Image = (Image) componentResourceManager.GetObject("picRLZoomIn.Image");
      this.picRLZoomIn.Location = new Point(81, 5);
      this.picRLZoomIn.Name = "picRLZoomIn";
      this.picRLZoomIn.Size = new Size(16, 16);
      this.picRLZoomIn.SizeMode = PictureBoxSizeMode.AutoSize;
      this.picRLZoomIn.TabIndex = 50;
      this.picRLZoomIn.TabStop = false;
      this.picRLZoomIn.Click += new EventHandler(this.ZoomButton_Clicked);
      this.label39.AutoSize = true;
      this.label39.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label39.Location = new Point(7, 7);
      this.label39.Name = "label39";
      this.label39.Size = new Size(74, 14);
      this.label39.TabIndex = 0;
      this.label39.Text = "Re-Lock Fees";
      this.btnClear.Location = new Point(256, 2);
      this.btnClear.Name = "btnClear";
      this.btnClear.Size = new Size(55, 20);
      this.btnClear.TabIndex = 174;
      this.btnClear.Text = "Clear";
      this.btnClear.UseVisualStyleBackColor = true;
      this.btnClear.Click += new EventHandler(this.btnClear_Click);
      this.pnlExExpand.Controls.Add((Control) this.label97);
      this.pnlExExpand.Controls.Add((Control) this.textBoxExtension10Desc);
      this.pnlExExpand.Controls.Add((Control) this.textBoxExtension10);
      this.pnlExExpand.Controls.Add((Control) this.label95);
      this.pnlExExpand.Controls.Add((Control) this.textBoxExtension9Desc);
      this.pnlExExpand.Controls.Add((Control) this.textBoxExtension9);
      this.pnlExExpand.Controls.Add((Control) this.label96);
      this.pnlExExpand.Controls.Add((Control) this.textBoxExtension8Desc);
      this.pnlExExpand.Controls.Add((Control) this.textBoxExtension8);
      this.pnlExExpand.Controls.Add((Control) this.label93);
      this.pnlExExpand.Controls.Add((Control) this.textBoxExtension7Desc);
      this.pnlExExpand.Controls.Add((Control) this.textBoxExtension7);
      this.pnlExExpand.Controls.Add((Control) this.label94);
      this.pnlExExpand.Controls.Add((Control) this.textBoxExtension6Desc);
      this.pnlExExpand.Controls.Add((Control) this.textBoxExtension6);
      this.pnlExExpand.Controls.Add((Control) this.label92);
      this.pnlExExpand.Controls.Add((Control) this.textBoxExtension5Desc);
      this.pnlExExpand.Controls.Add((Control) this.textBoxExtension5);
      this.pnlExExpand.Controls.Add((Control) this.label87);
      this.pnlExExpand.Controls.Add((Control) this.textBoxExtension4Desc);
      this.pnlExExpand.Controls.Add((Control) this.textBoxExtension4);
      this.pnlExExpand.Dock = DockStyle.Top;
      this.pnlExExpand.Location = new Point(1, 1585);
      this.pnlExExpand.Name = "pnlExExpand";
      this.pnlExExpand.Size = new Size(333, 170);
      this.pnlExExpand.TabIndex = 169;
      this.label97.AutoSize = true;
      this.label97.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label97.Location = new Point(8, 147);
      this.label97.Name = "label97";
      this.label97.Size = new Size(75, 14);
      this.label97.TabIndex = 91;
      this.label97.Text = "Extension #10";
      this.textBoxExtension10Desc.Font = new Font("Arial", 8.25f);
      this.textBoxExtension10Desc.Location = new Point(84, 144);
      this.textBoxExtension10Desc.Name = "textBoxExtension10Desc";
      this.textBoxExtension10Desc.Size = new Size(130, 20);
      this.textBoxExtension10Desc.TabIndex = 12;
      this.textBoxExtension10Desc.Tag = (object) "3472";
      this.textBoxExtension10Desc.TextAlign = HorizontalAlignment.Right;
      this.textBoxExtension10Desc.Enter += new EventHandler(this.textField_Enter);
      this.textBoxExtension10Desc.Leave += new EventHandler(this.strField_Leave);
      this.textBoxExtension10.Font = new Font("Arial", 8.25f);
      this.textBoxExtension10.Location = new Point(220, 144);
      this.textBoxExtension10.Name = "textBoxExtension10";
      this.textBoxExtension10.Size = new Size(102, 20);
      this.textBoxExtension10.TabIndex = 13;
      this.textBoxExtension10.Tag = (object) "3473";
      this.textBoxExtension10.TextAlign = HorizontalAlignment.Right;
      this.textBoxExtension10.Enter += new EventHandler(this.textField_Enter);
      this.textBoxExtension10.KeyPress += new KeyPressEventHandler(this.numField_KeyPress);
      this.textBoxExtension10.KeyUp += new KeyEventHandler(this.numField_KeyUp);
      this.textBoxExtension10.Leave += new EventHandler(this.numField_Leave);
      this.label95.AutoSize = true;
      this.label95.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label95.Location = new Point(8, 123);
      this.label95.Name = "label95";
      this.label95.Size = new Size(69, 14);
      this.label95.TabIndex = 88;
      this.label95.Text = "Extension #9";
      this.textBoxExtension9Desc.Font = new Font("Arial", 8.25f);
      this.textBoxExtension9Desc.Location = new Point(84, 120);
      this.textBoxExtension9Desc.Name = "textBoxExtension9Desc";
      this.textBoxExtension9Desc.Size = new Size(130, 20);
      this.textBoxExtension9Desc.TabIndex = 10;
      this.textBoxExtension9Desc.Tag = (object) "3470";
      this.textBoxExtension9Desc.TextAlign = HorizontalAlignment.Right;
      this.textBoxExtension9Desc.Enter += new EventHandler(this.textField_Enter);
      this.textBoxExtension9Desc.Leave += new EventHandler(this.strField_Leave);
      this.textBoxExtension9.Font = new Font("Arial", 8.25f);
      this.textBoxExtension9.Location = new Point(220, 120);
      this.textBoxExtension9.Name = "textBoxExtension9";
      this.textBoxExtension9.Size = new Size(102, 20);
      this.textBoxExtension9.TabIndex = 11;
      this.textBoxExtension9.Tag = (object) "3471";
      this.textBoxExtension9.TextAlign = HorizontalAlignment.Right;
      this.textBoxExtension9.Enter += new EventHandler(this.textField_Enter);
      this.textBoxExtension9.KeyPress += new KeyPressEventHandler(this.numField_KeyPress);
      this.textBoxExtension9.KeyUp += new KeyEventHandler(this.numField_KeyUp);
      this.textBoxExtension9.Leave += new EventHandler(this.numField_Leave);
      this.label96.AutoSize = true;
      this.label96.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label96.Location = new Point(8, 99);
      this.label96.Name = "label96";
      this.label96.Size = new Size(69, 14);
      this.label96.TabIndex = 85;
      this.label96.Text = "Extension #8";
      this.textBoxExtension8Desc.Font = new Font("Arial", 8.25f);
      this.textBoxExtension8Desc.Location = new Point(84, 96);
      this.textBoxExtension8Desc.Name = "textBoxExtension8Desc";
      this.textBoxExtension8Desc.Size = new Size(130, 20);
      this.textBoxExtension8Desc.TabIndex = 8;
      this.textBoxExtension8Desc.Tag = (object) "3468";
      this.textBoxExtension8Desc.TextAlign = HorizontalAlignment.Right;
      this.textBoxExtension8Desc.Enter += new EventHandler(this.textField_Enter);
      this.textBoxExtension8Desc.Leave += new EventHandler(this.strField_Leave);
      this.textBoxExtension8.Font = new Font("Arial", 8.25f);
      this.textBoxExtension8.Location = new Point(220, 96);
      this.textBoxExtension8.Name = "textBoxExtension8";
      this.textBoxExtension8.Size = new Size(102, 20);
      this.textBoxExtension8.TabIndex = 9;
      this.textBoxExtension8.Tag = (object) "3469";
      this.textBoxExtension8.TextAlign = HorizontalAlignment.Right;
      this.textBoxExtension8.Enter += new EventHandler(this.textField_Enter);
      this.textBoxExtension8.KeyPress += new KeyPressEventHandler(this.numField_KeyPress);
      this.textBoxExtension8.KeyUp += new KeyEventHandler(this.numField_KeyUp);
      this.textBoxExtension8.Leave += new EventHandler(this.numField_Leave);
      this.label93.AutoSize = true;
      this.label93.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label93.Location = new Point(8, 75);
      this.label93.Name = "label93";
      this.label93.Size = new Size(69, 14);
      this.label93.TabIndex = 82;
      this.label93.Text = "Extension #7";
      this.textBoxExtension7Desc.Font = new Font("Arial", 8.25f);
      this.textBoxExtension7Desc.Location = new Point(84, 72);
      this.textBoxExtension7Desc.Name = "textBoxExtension7Desc";
      this.textBoxExtension7Desc.Size = new Size(130, 20);
      this.textBoxExtension7Desc.TabIndex = 6;
      this.textBoxExtension7Desc.Tag = (object) "3466";
      this.textBoxExtension7Desc.TextAlign = HorizontalAlignment.Right;
      this.textBoxExtension7Desc.Enter += new EventHandler(this.textField_Enter);
      this.textBoxExtension7Desc.Leave += new EventHandler(this.strField_Leave);
      this.textBoxExtension7.Font = new Font("Arial", 8.25f);
      this.textBoxExtension7.Location = new Point(220, 72);
      this.textBoxExtension7.Name = "textBoxExtension7";
      this.textBoxExtension7.Size = new Size(102, 20);
      this.textBoxExtension7.TabIndex = 7;
      this.textBoxExtension7.Tag = (object) "3467";
      this.textBoxExtension7.TextAlign = HorizontalAlignment.Right;
      this.textBoxExtension7.Enter += new EventHandler(this.textField_Enter);
      this.textBoxExtension7.KeyPress += new KeyPressEventHandler(this.numField_KeyPress);
      this.textBoxExtension7.KeyUp += new KeyEventHandler(this.numField_KeyUp);
      this.textBoxExtension7.Leave += new EventHandler(this.numField_Leave);
      this.label94.AutoSize = true;
      this.label94.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label94.Location = new Point(8, 51);
      this.label94.Name = "label94";
      this.label94.Size = new Size(69, 14);
      this.label94.TabIndex = 79;
      this.label94.Text = "Extension #6";
      this.textBoxExtension6Desc.Font = new Font("Arial", 8.25f);
      this.textBoxExtension6Desc.Location = new Point(84, 48);
      this.textBoxExtension6Desc.Name = "textBoxExtension6Desc";
      this.textBoxExtension6Desc.Size = new Size(130, 20);
      this.textBoxExtension6Desc.TabIndex = 4;
      this.textBoxExtension6Desc.Tag = (object) "3464";
      this.textBoxExtension6Desc.TextAlign = HorizontalAlignment.Right;
      this.textBoxExtension6Desc.Enter += new EventHandler(this.textField_Enter);
      this.textBoxExtension6Desc.Leave += new EventHandler(this.strField_Leave);
      this.textBoxExtension6.Font = new Font("Arial", 8.25f);
      this.textBoxExtension6.Location = new Point(220, 48);
      this.textBoxExtension6.Name = "textBoxExtension6";
      this.textBoxExtension6.Size = new Size(102, 20);
      this.textBoxExtension6.TabIndex = 5;
      this.textBoxExtension6.Tag = (object) "3465";
      this.textBoxExtension6.TextAlign = HorizontalAlignment.Right;
      this.textBoxExtension6.Enter += new EventHandler(this.textField_Enter);
      this.textBoxExtension6.KeyPress += new KeyPressEventHandler(this.numField_KeyPress);
      this.textBoxExtension6.KeyUp += new KeyEventHandler(this.numField_KeyUp);
      this.textBoxExtension6.Leave += new EventHandler(this.numField_Leave);
      this.label92.AutoSize = true;
      this.label92.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label92.Location = new Point(7, 27);
      this.label92.Name = "label92";
      this.label92.Size = new Size(69, 14);
      this.label92.TabIndex = 76;
      this.label92.Text = "Extension #5";
      this.textBoxExtension5Desc.Font = new Font("Arial", 8.25f);
      this.textBoxExtension5Desc.Location = new Point(83, 24);
      this.textBoxExtension5Desc.Name = "textBoxExtension5Desc";
      this.textBoxExtension5Desc.Size = new Size(130, 20);
      this.textBoxExtension5Desc.TabIndex = 2;
      this.textBoxExtension5Desc.Tag = (object) "3462";
      this.textBoxExtension5Desc.TextAlign = HorizontalAlignment.Right;
      this.textBoxExtension5Desc.Enter += new EventHandler(this.textField_Enter);
      this.textBoxExtension5Desc.Leave += new EventHandler(this.strField_Leave);
      this.textBoxExtension5.Font = new Font("Arial", 8.25f);
      this.textBoxExtension5.Location = new Point(219, 24);
      this.textBoxExtension5.Name = "textBoxExtension5";
      this.textBoxExtension5.Size = new Size(102, 20);
      this.textBoxExtension5.TabIndex = 3;
      this.textBoxExtension5.Tag = (object) "3463";
      this.textBoxExtension5.TextAlign = HorizontalAlignment.Right;
      this.textBoxExtension5.Enter += new EventHandler(this.textField_Enter);
      this.textBoxExtension5.KeyPress += new KeyPressEventHandler(this.numField_KeyPress);
      this.textBoxExtension5.KeyUp += new KeyEventHandler(this.numField_KeyUp);
      this.textBoxExtension5.Leave += new EventHandler(this.numField_Leave);
      this.label87.AutoSize = true;
      this.label87.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label87.Location = new Point(7, 3);
      this.label87.Name = "label87";
      this.label87.Size = new Size(69, 14);
      this.label87.TabIndex = 73;
      this.label87.Text = "Extension #4";
      this.textBoxExtension4Desc.Font = new Font("Arial", 8.25f);
      this.textBoxExtension4Desc.Location = new Point(83, 0);
      this.textBoxExtension4Desc.Name = "textBoxExtension4Desc";
      this.textBoxExtension4Desc.Size = new Size(130, 20);
      this.textBoxExtension4Desc.TabIndex = 0;
      this.textBoxExtension4Desc.Tag = (object) "3460";
      this.textBoxExtension4Desc.TextAlign = HorizontalAlignment.Right;
      this.textBoxExtension4Desc.Enter += new EventHandler(this.textField_Enter);
      this.textBoxExtension4Desc.Leave += new EventHandler(this.strField_Leave);
      this.textBoxExtension4.Font = new Font("Arial", 8.25f);
      this.textBoxExtension4.Location = new Point(219, 0);
      this.textBoxExtension4.Name = "textBoxExtension4";
      this.textBoxExtension4.Size = new Size(102, 20);
      this.textBoxExtension4.TabIndex = 1;
      this.textBoxExtension4.Tag = (object) "3461";
      this.textBoxExtension4.TextAlign = HorizontalAlignment.Right;
      this.textBoxExtension4.Enter += new EventHandler(this.textField_Enter);
      this.textBoxExtension4.KeyPress += new KeyPressEventHandler(this.numField_KeyPress);
      this.textBoxExtension4.KeyUp += new KeyEventHandler(this.numField_KeyUp);
      this.textBoxExtension4.Leave += new EventHandler(this.numField_Leave);
      this.pnlEx.Controls.Add((Control) this.label86);
      this.pnlEx.Controls.Add((Control) this.label85);
      this.pnlEx.Controls.Add((Control) this.label84);
      this.pnlEx.Controls.Add((Control) this.textBoxExtension3Desc);
      this.pnlEx.Controls.Add((Control) this.textBoxExtension3);
      this.pnlEx.Controls.Add((Control) this.textBoxExtension2Desc);
      this.pnlEx.Controls.Add((Control) this.textBoxExtension2);
      this.pnlEx.Controls.Add((Control) this.textBoxExtension1Desc);
      this.pnlEx.Controls.Add((Control) this.textBoxExtension1);
      this.pnlEx.Controls.Add((Control) this.picEXZoomOut);
      this.pnlEx.Controls.Add((Control) this.picEXZoomIn);
      this.pnlEx.Controls.Add((Control) this.label82);
      this.pnlEx.Dock = DockStyle.Top;
      this.pnlEx.Location = new Point(1, 1485);
      this.pnlEx.Name = "pnlEx";
      this.pnlEx.Size = new Size(333, 100);
      this.pnlEx.TabIndex = 170;
      this.label86.AutoSize = true;
      this.label86.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label86.Location = new Point(7, 80);
      this.label86.Name = "label86";
      this.label86.Size = new Size(69, 14);
      this.label86.TabIndex = 72;
      this.label86.Text = "Extension #3";
      this.label85.AutoSize = true;
      this.label85.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label85.Location = new Point(7, 55);
      this.label85.Name = "label85";
      this.label85.Size = new Size(69, 14);
      this.label85.TabIndex = 71;
      this.label85.Text = "Extension #2";
      this.label84.AutoSize = true;
      this.label84.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label84.Location = new Point(7, 31);
      this.label84.Name = "label84";
      this.label84.Size = new Size(69, 14);
      this.label84.TabIndex = 70;
      this.label84.Text = "Extension #1";
      this.textBoxExtension3Desc.Font = new Font("Arial", 8.25f);
      this.textBoxExtension3Desc.Location = new Point(84, 76);
      this.textBoxExtension3Desc.Name = "textBoxExtension3Desc";
      this.textBoxExtension3Desc.Size = new Size(130, 20);
      this.textBoxExtension3Desc.TabIndex = 4;
      this.textBoxExtension3Desc.Tag = (object) "3458";
      this.textBoxExtension3Desc.TextAlign = HorizontalAlignment.Right;
      this.textBoxExtension3Desc.Enter += new EventHandler(this.textField_Enter);
      this.textBoxExtension3Desc.Leave += new EventHandler(this.strField_Leave);
      this.textBoxExtension3.Font = new Font("Arial", 8.25f);
      this.textBoxExtension3.Location = new Point(219, 76);
      this.textBoxExtension3.Name = "textBoxExtension3";
      this.textBoxExtension3.Size = new Size(102, 20);
      this.textBoxExtension3.TabIndex = 5;
      this.textBoxExtension3.Tag = (object) "3459";
      this.textBoxExtension3.TextAlign = HorizontalAlignment.Right;
      this.textBoxExtension3.Enter += new EventHandler(this.textField_Enter);
      this.textBoxExtension3.KeyPress += new KeyPressEventHandler(this.numField_KeyPress);
      this.textBoxExtension3.KeyUp += new KeyEventHandler(this.numField_KeyUp);
      this.textBoxExtension3.Leave += new EventHandler(this.numField_Leave);
      this.textBoxExtension2Desc.Font = new Font("Arial", 8.25f);
      this.textBoxExtension2Desc.Location = new Point(84, 52);
      this.textBoxExtension2Desc.Name = "textBoxExtension2Desc";
      this.textBoxExtension2Desc.Size = new Size(130, 20);
      this.textBoxExtension2Desc.TabIndex = 2;
      this.textBoxExtension2Desc.Tag = (object) "3456";
      this.textBoxExtension2Desc.TextAlign = HorizontalAlignment.Right;
      this.textBoxExtension2Desc.Enter += new EventHandler(this.textField_Enter);
      this.textBoxExtension2Desc.Leave += new EventHandler(this.strField_Leave);
      this.textBoxExtension2.Font = new Font("Arial", 8.25f);
      this.textBoxExtension2.Location = new Point(219, 52);
      this.textBoxExtension2.Name = "textBoxExtension2";
      this.textBoxExtension2.Size = new Size(102, 20);
      this.textBoxExtension2.TabIndex = 3;
      this.textBoxExtension2.Tag = (object) "3457";
      this.textBoxExtension2.TextAlign = HorizontalAlignment.Right;
      this.textBoxExtension2.Enter += new EventHandler(this.textField_Enter);
      this.textBoxExtension2.KeyPress += new KeyPressEventHandler(this.numField_KeyPress);
      this.textBoxExtension2.KeyUp += new KeyEventHandler(this.numField_KeyUp);
      this.textBoxExtension2.Leave += new EventHandler(this.numField_Leave);
      this.textBoxExtension1Desc.Font = new Font("Arial", 8.25f);
      this.textBoxExtension1Desc.Location = new Point(83, 28);
      this.textBoxExtension1Desc.Name = "textBoxExtension1Desc";
      this.textBoxExtension1Desc.Size = new Size(130, 20);
      this.textBoxExtension1Desc.TabIndex = 0;
      this.textBoxExtension1Desc.Tag = (object) "3454";
      this.textBoxExtension1Desc.TextAlign = HorizontalAlignment.Right;
      this.textBoxExtension1Desc.Enter += new EventHandler(this.textField_Enter);
      this.textBoxExtension1Desc.Leave += new EventHandler(this.strField_Leave);
      this.textBoxExtension1.Font = new Font("Arial", 8.25f);
      this.textBoxExtension1.Location = new Point(219, 28);
      this.textBoxExtension1.Name = "textBoxExtension1";
      this.textBoxExtension1.Size = new Size(102, 20);
      this.textBoxExtension1.TabIndex = 1;
      this.textBoxExtension1.Tag = (object) "3455";
      this.textBoxExtension1.TextAlign = HorizontalAlignment.Right;
      this.textBoxExtension1.Enter += new EventHandler(this.textField_Enter);
      this.textBoxExtension1.KeyPress += new KeyPressEventHandler(this.numField_KeyPress);
      this.textBoxExtension1.KeyUp += new KeyEventHandler(this.numField_KeyUp);
      this.textBoxExtension1.Leave += new EventHandler(this.numField_Leave);
      this.picEXZoomOut.BackgroundImageLayout = ImageLayout.None;
      this.picEXZoomOut.Cursor = Cursors.Hand;
      this.picEXZoomOut.Image = (Image) componentResourceManager.GetObject("picEXZoomOut.Image");
      this.picEXZoomOut.Location = new Point(112, 5);
      this.picEXZoomOut.Name = "picEXZoomOut";
      this.picEXZoomOut.Size = new Size(16, 16);
      this.picEXZoomOut.SizeMode = PictureBoxSizeMode.AutoSize;
      this.picEXZoomOut.TabIndex = 51;
      this.picEXZoomOut.TabStop = false;
      this.picEXZoomOut.Click += new EventHandler(this.ZoomButton_Clicked);
      this.picEXZoomIn.BackgroundImageLayout = ImageLayout.None;
      this.picEXZoomIn.Cursor = Cursors.Hand;
      this.picEXZoomIn.Image = (Image) componentResourceManager.GetObject("picEXZoomIn.Image");
      this.picEXZoomIn.Location = new Point(90, 5);
      this.picEXZoomIn.Name = "picEXZoomIn";
      this.picEXZoomIn.Size = new Size(16, 16);
      this.picEXZoomIn.SizeMode = PictureBoxSizeMode.AutoSize;
      this.picEXZoomIn.TabIndex = 50;
      this.picEXZoomIn.TabStop = false;
      this.picEXZoomIn.Click += new EventHandler(this.ZoomButton_Clicked);
      this.label82.AutoSize = true;
      this.label82.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label82.Location = new Point(7, 7);
      this.label82.Name = "label82";
      this.label82.Size = new Size(86, 14);
      this.label82.TabIndex = 0;
      this.label82.Text = "Lock Extensions";
      this.pnlBPExtension.Controls.Add((Control) this.comboBoxPrice19);
      this.pnlBPExtension.Controls.Add((Control) this.textBoxPrice20);
      this.pnlBPExtension.Controls.Add((Control) this.comboBoxPrice20);
      this.pnlBPExtension.Controls.Add((Control) this.textBoxPrice21);
      this.pnlBPExtension.Controls.Add((Control) this.comboBoxPrice15);
      this.pnlBPExtension.Controls.Add((Control) this.textBoxPrice16);
      this.pnlBPExtension.Controls.Add((Control) this.comboBoxPrice16);
      this.pnlBPExtension.Controls.Add((Control) this.textBoxPrice17);
      this.pnlBPExtension.Controls.Add((Control) this.comboBoxPrice17);
      this.pnlBPExtension.Controls.Add((Control) this.textBoxPrice18);
      this.pnlBPExtension.Controls.Add((Control) this.comboBoxPrice18);
      this.pnlBPExtension.Controls.Add((Control) this.textBoxPrice19);
      this.pnlBPExtension.Controls.Add((Control) this.comboBoxPrice13);
      this.pnlBPExtension.Controls.Add((Control) this.textBoxPrice14);
      this.pnlBPExtension.Controls.Add((Control) this.comboBoxPrice14);
      this.pnlBPExtension.Controls.Add((Control) this.textBoxPrice15);
      this.pnlBPExtension.Controls.Add((Control) this.comboBoxPrice9);
      this.pnlBPExtension.Controls.Add((Control) this.textBoxPrice10);
      this.pnlBPExtension.Controls.Add((Control) this.comboBoxPrice10);
      this.pnlBPExtension.Controls.Add((Control) this.textBoxPrice11);
      this.pnlBPExtension.Controls.Add((Control) this.comboBoxPrice11);
      this.pnlBPExtension.Controls.Add((Control) this.textBoxPrice12);
      this.pnlBPExtension.Controls.Add((Control) this.comboBoxPrice12);
      this.pnlBPExtension.Controls.Add((Control) this.textBoxPrice13);
      this.pnlBPExtension.Controls.Add((Control) this.comboBoxPrice5);
      this.pnlBPExtension.Controls.Add((Control) this.textBoxPrice6);
      this.pnlBPExtension.Controls.Add((Control) this.comboBoxPrice6);
      this.pnlBPExtension.Controls.Add((Control) this.textBoxPrice7);
      this.pnlBPExtension.Controls.Add((Control) this.comboBoxPrice7);
      this.pnlBPExtension.Controls.Add((Control) this.textBoxPrice8);
      this.pnlBPExtension.Controls.Add((Control) this.comboBoxPrice8);
      this.pnlBPExtension.Controls.Add((Control) this.textBoxPrice9);
      this.pnlBPExtension.Dock = DockStyle.Top;
      this.pnlBPExtension.Location = new Point(1, 1089);
      this.pnlBPExtension.Name = "pnlBPExtension";
      this.pnlBPExtension.Size = new Size(333, 396);
      this.pnlBPExtension.TabIndex = 162;
      this.comboBoxPrice19.Font = new Font("Arial", 8.25f);
      this.comboBoxPrice19.Location = new Point(11, 343);
      this.comboBoxPrice19.Name = "comboBoxPrice19";
      this.comboBoxPrice19.Size = new Size(203, 22);
      this.comboBoxPrice19.TabIndex = 85;
      this.comboBoxPrice19.Tag = (object) "2138";
      this.comboBoxPrice19.Enter += new EventHandler(this.comboBoxField_Enter);
      this.comboBoxPrice19.Leave += new EventHandler(this.strField_Leave);
      this.textBoxPrice20.Font = new Font("Arial", 8.25f);
      this.textBoxPrice20.Location = new Point(220, 344);
      this.textBoxPrice20.Name = "textBoxPrice20";
      this.textBoxPrice20.Size = new Size(102, 20);
      this.textBoxPrice20.TabIndex = 86;
      this.textBoxPrice20.Tag = (object) "2139";
      this.textBoxPrice20.TextAlign = HorizontalAlignment.Right;
      this.textBoxPrice20.Enter += new EventHandler(this.textField_Enter);
      this.textBoxPrice20.KeyPress += new KeyPressEventHandler(this.numField_KeyPress);
      this.textBoxPrice20.KeyUp += new KeyEventHandler(this.numField_KeyUp);
      this.textBoxPrice20.Leave += new EventHandler(this.numField_Leave);
      this.comboBoxPrice20.Font = new Font("Arial", 8.25f);
      this.comboBoxPrice20.Location = new Point(11, 367);
      this.comboBoxPrice20.Name = "comboBoxPrice20";
      this.comboBoxPrice20.Size = new Size(203, 22);
      this.comboBoxPrice20.TabIndex = 87;
      this.comboBoxPrice20.Tag = (object) "2140";
      this.comboBoxPrice20.Enter += new EventHandler(this.comboBoxField_Enter);
      this.comboBoxPrice20.Leave += new EventHandler(this.strField_Leave);
      this.textBoxPrice21.Font = new Font("Arial", 8.25f);
      this.textBoxPrice21.Location = new Point(220, 368);
      this.textBoxPrice21.Name = "textBoxPrice21";
      this.textBoxPrice21.Size = new Size(102, 20);
      this.textBoxPrice21.TabIndex = 88;
      this.textBoxPrice21.Tag = (object) "2141";
      this.textBoxPrice21.TextAlign = HorizontalAlignment.Right;
      this.textBoxPrice21.Enter += new EventHandler(this.textField_Enter);
      this.textBoxPrice21.KeyPress += new KeyPressEventHandler(this.numField_KeyPress);
      this.textBoxPrice21.KeyUp += new KeyEventHandler(this.numField_KeyUp);
      this.textBoxPrice21.Leave += new EventHandler(this.numField_Leave);
      this.comboBoxPrice15.Font = new Font("Arial", 8.25f);
      this.comboBoxPrice15.Location = new Point(11, 245);
      this.comboBoxPrice15.Name = "comboBoxPrice15";
      this.comboBoxPrice15.Size = new Size(203, 22);
      this.comboBoxPrice15.TabIndex = 77;
      this.comboBoxPrice15.Tag = (object) "2130";
      this.comboBoxPrice15.Enter += new EventHandler(this.comboBoxField_Enter);
      this.comboBoxPrice15.Leave += new EventHandler(this.strField_Leave);
      this.textBoxPrice16.Font = new Font("Arial", 8.25f);
      this.textBoxPrice16.Location = new Point(220, 246);
      this.textBoxPrice16.Name = "textBoxPrice16";
      this.textBoxPrice16.Size = new Size(102, 20);
      this.textBoxPrice16.TabIndex = 78;
      this.textBoxPrice16.Tag = (object) "2131";
      this.textBoxPrice16.TextAlign = HorizontalAlignment.Right;
      this.textBoxPrice16.Enter += new EventHandler(this.textField_Enter);
      this.textBoxPrice16.KeyPress += new KeyPressEventHandler(this.numField_KeyPress);
      this.textBoxPrice16.KeyUp += new KeyEventHandler(this.numField_KeyUp);
      this.textBoxPrice16.Leave += new EventHandler(this.numField_Leave);
      this.comboBoxPrice16.Font = new Font("Arial", 8.25f);
      this.comboBoxPrice16.Location = new Point(11, 269);
      this.comboBoxPrice16.Name = "comboBoxPrice16";
      this.comboBoxPrice16.Size = new Size(203, 22);
      this.comboBoxPrice16.TabIndex = 79;
      this.comboBoxPrice16.Tag = (object) "2132";
      this.comboBoxPrice16.Enter += new EventHandler(this.comboBoxField_Enter);
      this.comboBoxPrice16.Leave += new EventHandler(this.strField_Leave);
      this.textBoxPrice17.Font = new Font("Arial", 8.25f);
      this.textBoxPrice17.Location = new Point(220, 270);
      this.textBoxPrice17.Name = "textBoxPrice17";
      this.textBoxPrice17.Size = new Size(102, 20);
      this.textBoxPrice17.TabIndex = 80;
      this.textBoxPrice17.Tag = (object) "2133";
      this.textBoxPrice17.TextAlign = HorizontalAlignment.Right;
      this.textBoxPrice17.Enter += new EventHandler(this.textField_Enter);
      this.textBoxPrice17.KeyPress += new KeyPressEventHandler(this.numField_KeyPress);
      this.textBoxPrice17.KeyUp += new KeyEventHandler(this.numField_KeyUp);
      this.textBoxPrice17.Leave += new EventHandler(this.numField_Leave);
      this.comboBoxPrice17.Font = new Font("Arial", 8.25f);
      this.comboBoxPrice17.Location = new Point(11, 293);
      this.comboBoxPrice17.Name = "comboBoxPrice17";
      this.comboBoxPrice17.Size = new Size(203, 22);
      this.comboBoxPrice17.TabIndex = 81;
      this.comboBoxPrice17.Tag = (object) "2134";
      this.comboBoxPrice17.Enter += new EventHandler(this.comboBoxField_Enter);
      this.comboBoxPrice17.Leave += new EventHandler(this.strField_Leave);
      this.textBoxPrice18.Font = new Font("Arial", 8.25f);
      this.textBoxPrice18.Location = new Point(220, 294);
      this.textBoxPrice18.Name = "textBoxPrice18";
      this.textBoxPrice18.Size = new Size(102, 20);
      this.textBoxPrice18.TabIndex = 82;
      this.textBoxPrice18.Tag = (object) "2135";
      this.textBoxPrice18.TextAlign = HorizontalAlignment.Right;
      this.textBoxPrice18.Enter += new EventHandler(this.textField_Enter);
      this.textBoxPrice18.KeyPress += new KeyPressEventHandler(this.numField_KeyPress);
      this.textBoxPrice18.KeyUp += new KeyEventHandler(this.numField_KeyUp);
      this.textBoxPrice18.Leave += new EventHandler(this.numField_Leave);
      this.comboBoxPrice18.Font = new Font("Arial", 8.25f);
      this.comboBoxPrice18.Location = new Point(11, 317);
      this.comboBoxPrice18.Name = "comboBoxPrice18";
      this.comboBoxPrice18.Size = new Size(203, 22);
      this.comboBoxPrice18.TabIndex = 83;
      this.comboBoxPrice18.Tag = (object) "2136";
      this.comboBoxPrice18.Enter += new EventHandler(this.comboBoxField_Enter);
      this.comboBoxPrice18.Leave += new EventHandler(this.strField_Leave);
      this.textBoxPrice19.Font = new Font("Arial", 8.25f);
      this.textBoxPrice19.Location = new Point(220, 318);
      this.textBoxPrice19.Name = "textBoxPrice19";
      this.textBoxPrice19.Size = new Size(102, 20);
      this.textBoxPrice19.TabIndex = 84;
      this.textBoxPrice19.Tag = (object) "2137";
      this.textBoxPrice19.TextAlign = HorizontalAlignment.Right;
      this.textBoxPrice19.Enter += new EventHandler(this.textField_Enter);
      this.textBoxPrice19.KeyPress += new KeyPressEventHandler(this.numField_KeyPress);
      this.textBoxPrice19.KeyUp += new KeyEventHandler(this.numField_KeyUp);
      this.textBoxPrice19.Leave += new EventHandler(this.numField_Leave);
      this.comboBoxPrice13.Font = new Font("Arial", 8.25f);
      this.comboBoxPrice13.Location = new Point(10, 195);
      this.comboBoxPrice13.Name = "comboBoxPrice13";
      this.comboBoxPrice13.Size = new Size(203, 22);
      this.comboBoxPrice13.TabIndex = 73;
      this.comboBoxPrice13.Tag = (object) "2126";
      this.comboBoxPrice13.Enter += new EventHandler(this.comboBoxField_Enter);
      this.comboBoxPrice13.Leave += new EventHandler(this.strField_Leave);
      this.textBoxPrice14.Font = new Font("Arial", 8.25f);
      this.textBoxPrice14.Location = new Point(219, 196);
      this.textBoxPrice14.Name = "textBoxPrice14";
      this.textBoxPrice14.Size = new Size(102, 20);
      this.textBoxPrice14.TabIndex = 74;
      this.textBoxPrice14.Tag = (object) "2127";
      this.textBoxPrice14.TextAlign = HorizontalAlignment.Right;
      this.textBoxPrice14.Enter += new EventHandler(this.textField_Enter);
      this.textBoxPrice14.KeyPress += new KeyPressEventHandler(this.numField_KeyPress);
      this.textBoxPrice14.KeyUp += new KeyEventHandler(this.numField_KeyUp);
      this.textBoxPrice14.Leave += new EventHandler(this.numField_Leave);
      this.comboBoxPrice14.Font = new Font("Arial", 8.25f);
      this.comboBoxPrice14.Location = new Point(10, 219);
      this.comboBoxPrice14.Name = "comboBoxPrice14";
      this.comboBoxPrice14.Size = new Size(203, 22);
      this.comboBoxPrice14.TabIndex = 75;
      this.comboBoxPrice14.Tag = (object) "2128";
      this.comboBoxPrice14.Enter += new EventHandler(this.comboBoxField_Enter);
      this.comboBoxPrice14.Leave += new EventHandler(this.strField_Leave);
      this.textBoxPrice15.Font = new Font("Arial", 8.25f);
      this.textBoxPrice15.Location = new Point(219, 220);
      this.textBoxPrice15.Name = "textBoxPrice15";
      this.textBoxPrice15.Size = new Size(102, 20);
      this.textBoxPrice15.TabIndex = 76;
      this.textBoxPrice15.Tag = (object) "2129";
      this.textBoxPrice15.TextAlign = HorizontalAlignment.Right;
      this.textBoxPrice15.Enter += new EventHandler(this.textField_Enter);
      this.textBoxPrice15.KeyPress += new KeyPressEventHandler(this.numField_KeyPress);
      this.textBoxPrice15.KeyUp += new KeyEventHandler(this.numField_KeyUp);
      this.textBoxPrice15.Leave += new EventHandler(this.numField_Leave);
      this.comboBoxPrice9.Font = new Font("Arial", 8.25f);
      this.comboBoxPrice9.Location = new Point(10, 97);
      this.comboBoxPrice9.Name = "comboBoxPrice9";
      this.comboBoxPrice9.Size = new Size(203, 22);
      this.comboBoxPrice9.TabIndex = 65;
      this.comboBoxPrice9.Tag = (object) "2118";
      this.comboBoxPrice9.Enter += new EventHandler(this.comboBoxField_Enter);
      this.comboBoxPrice9.Leave += new EventHandler(this.strField_Leave);
      this.textBoxPrice10.Font = new Font("Arial", 8.25f);
      this.textBoxPrice10.Location = new Point(219, 98);
      this.textBoxPrice10.Name = "textBoxPrice10";
      this.textBoxPrice10.Size = new Size(102, 20);
      this.textBoxPrice10.TabIndex = 66;
      this.textBoxPrice10.Tag = (object) "2119";
      this.textBoxPrice10.TextAlign = HorizontalAlignment.Right;
      this.textBoxPrice10.Enter += new EventHandler(this.textField_Enter);
      this.textBoxPrice10.KeyPress += new KeyPressEventHandler(this.numField_KeyPress);
      this.textBoxPrice10.KeyUp += new KeyEventHandler(this.numField_KeyUp);
      this.textBoxPrice10.Leave += new EventHandler(this.numField_Leave);
      this.comboBoxPrice10.Font = new Font("Arial", 8.25f);
      this.comboBoxPrice10.Location = new Point(10, 121);
      this.comboBoxPrice10.Name = "comboBoxPrice10";
      this.comboBoxPrice10.Size = new Size(203, 22);
      this.comboBoxPrice10.TabIndex = 67;
      this.comboBoxPrice10.Tag = (object) "2120";
      this.comboBoxPrice10.Enter += new EventHandler(this.comboBoxField_Enter);
      this.comboBoxPrice10.Leave += new EventHandler(this.strField_Leave);
      this.textBoxPrice11.Font = new Font("Arial", 8.25f);
      this.textBoxPrice11.Location = new Point(219, 122);
      this.textBoxPrice11.Name = "textBoxPrice11";
      this.textBoxPrice11.Size = new Size(102, 20);
      this.textBoxPrice11.TabIndex = 68;
      this.textBoxPrice11.Tag = (object) "2121";
      this.textBoxPrice11.TextAlign = HorizontalAlignment.Right;
      this.textBoxPrice11.Enter += new EventHandler(this.textField_Enter);
      this.textBoxPrice11.KeyPress += new KeyPressEventHandler(this.numField_KeyPress);
      this.textBoxPrice11.KeyUp += new KeyEventHandler(this.numField_KeyUp);
      this.textBoxPrice11.Leave += new EventHandler(this.numField_Leave);
      this.comboBoxPrice11.Font = new Font("Arial", 8.25f);
      this.comboBoxPrice11.Location = new Point(10, 145);
      this.comboBoxPrice11.Name = "comboBoxPrice11";
      this.comboBoxPrice11.Size = new Size(203, 22);
      this.comboBoxPrice11.TabIndex = 69;
      this.comboBoxPrice11.Tag = (object) "2122";
      this.comboBoxPrice11.Enter += new EventHandler(this.comboBoxField_Enter);
      this.comboBoxPrice11.Leave += new EventHandler(this.strField_Leave);
      this.textBoxPrice12.Font = new Font("Arial", 8.25f);
      this.textBoxPrice12.Location = new Point(219, 146);
      this.textBoxPrice12.Name = "textBoxPrice12";
      this.textBoxPrice12.Size = new Size(102, 20);
      this.textBoxPrice12.TabIndex = 70;
      this.textBoxPrice12.Tag = (object) "2123";
      this.textBoxPrice12.TextAlign = HorizontalAlignment.Right;
      this.textBoxPrice12.Enter += new EventHandler(this.textField_Enter);
      this.textBoxPrice12.KeyPress += new KeyPressEventHandler(this.numField_KeyPress);
      this.textBoxPrice12.KeyUp += new KeyEventHandler(this.numField_KeyUp);
      this.textBoxPrice12.Leave += new EventHandler(this.numField_Leave);
      this.comboBoxPrice12.Font = new Font("Arial", 8.25f);
      this.comboBoxPrice12.Location = new Point(10, 169);
      this.comboBoxPrice12.Name = "comboBoxPrice12";
      this.comboBoxPrice12.Size = new Size(203, 22);
      this.comboBoxPrice12.TabIndex = 71;
      this.comboBoxPrice12.Tag = (object) "2124";
      this.comboBoxPrice12.Enter += new EventHandler(this.comboBoxField_Enter);
      this.comboBoxPrice12.Leave += new EventHandler(this.strField_Leave);
      this.textBoxPrice13.Font = new Font("Arial", 8.25f);
      this.textBoxPrice13.Location = new Point(219, 170);
      this.textBoxPrice13.Name = "textBoxPrice13";
      this.textBoxPrice13.Size = new Size(102, 20);
      this.textBoxPrice13.TabIndex = 72;
      this.textBoxPrice13.Tag = (object) "2125";
      this.textBoxPrice13.TextAlign = HorizontalAlignment.Right;
      this.textBoxPrice13.Enter += new EventHandler(this.textField_Enter);
      this.textBoxPrice13.KeyPress += new KeyPressEventHandler(this.numField_KeyPress);
      this.textBoxPrice13.KeyUp += new KeyEventHandler(this.numField_KeyUp);
      this.textBoxPrice13.Leave += new EventHandler(this.numField_Leave);
      this.comboBoxPrice5.Font = new Font("Arial", 8.25f);
      this.comboBoxPrice5.Location = new Point(10, 0);
      this.comboBoxPrice5.Name = "comboBoxPrice5";
      this.comboBoxPrice5.Size = new Size(203, 22);
      this.comboBoxPrice5.TabIndex = 57;
      this.comboBoxPrice5.Tag = (object) "2110";
      this.comboBoxPrice5.Enter += new EventHandler(this.comboBoxField_Enter);
      this.comboBoxPrice5.Leave += new EventHandler(this.strField_Leave);
      this.textBoxPrice6.Font = new Font("Arial", 8.25f);
      this.textBoxPrice6.Location = new Point(219, 1);
      this.textBoxPrice6.Name = "textBoxPrice6";
      this.textBoxPrice6.Size = new Size(102, 20);
      this.textBoxPrice6.TabIndex = 58;
      this.textBoxPrice6.Tag = (object) "2111";
      this.textBoxPrice6.TextAlign = HorizontalAlignment.Right;
      this.textBoxPrice6.Enter += new EventHandler(this.textField_Enter);
      this.textBoxPrice6.KeyPress += new KeyPressEventHandler(this.numField_KeyPress);
      this.textBoxPrice6.KeyUp += new KeyEventHandler(this.numField_KeyUp);
      this.textBoxPrice6.Leave += new EventHandler(this.numField_Leave);
      this.comboBoxPrice6.Font = new Font("Arial", 8.25f);
      this.comboBoxPrice6.Location = new Point(10, 24);
      this.comboBoxPrice6.Name = "comboBoxPrice6";
      this.comboBoxPrice6.Size = new Size(203, 22);
      this.comboBoxPrice6.TabIndex = 59;
      this.comboBoxPrice6.Tag = (object) "2112";
      this.comboBoxPrice6.Enter += new EventHandler(this.comboBoxField_Enter);
      this.comboBoxPrice6.Leave += new EventHandler(this.strField_Leave);
      this.textBoxPrice7.Font = new Font("Arial", 8.25f);
      this.textBoxPrice7.Location = new Point(219, 25);
      this.textBoxPrice7.Name = "textBoxPrice7";
      this.textBoxPrice7.Size = new Size(102, 20);
      this.textBoxPrice7.TabIndex = 60;
      this.textBoxPrice7.Tag = (object) "2113";
      this.textBoxPrice7.TextAlign = HorizontalAlignment.Right;
      this.textBoxPrice7.Enter += new EventHandler(this.textField_Enter);
      this.textBoxPrice7.KeyPress += new KeyPressEventHandler(this.numField_KeyPress);
      this.textBoxPrice7.KeyUp += new KeyEventHandler(this.numField_KeyUp);
      this.textBoxPrice7.Leave += new EventHandler(this.numField_Leave);
      this.comboBoxPrice7.Font = new Font("Arial", 8.25f);
      this.comboBoxPrice7.Location = new Point(10, 48);
      this.comboBoxPrice7.Name = "comboBoxPrice7";
      this.comboBoxPrice7.Size = new Size(203, 22);
      this.comboBoxPrice7.TabIndex = 61;
      this.comboBoxPrice7.Tag = (object) "2114";
      this.comboBoxPrice7.Enter += new EventHandler(this.comboBoxField_Enter);
      this.comboBoxPrice7.Leave += new EventHandler(this.strField_Leave);
      this.textBoxPrice8.Font = new Font("Arial", 8.25f);
      this.textBoxPrice8.Location = new Point(219, 49);
      this.textBoxPrice8.Name = "textBoxPrice8";
      this.textBoxPrice8.Size = new Size(102, 20);
      this.textBoxPrice8.TabIndex = 62;
      this.textBoxPrice8.Tag = (object) "2115";
      this.textBoxPrice8.TextAlign = HorizontalAlignment.Right;
      this.textBoxPrice8.Enter += new EventHandler(this.textField_Enter);
      this.textBoxPrice8.KeyPress += new KeyPressEventHandler(this.numField_KeyPress);
      this.textBoxPrice8.KeyUp += new KeyEventHandler(this.numField_KeyUp);
      this.textBoxPrice8.Leave += new EventHandler(this.numField_Leave);
      this.comboBoxPrice8.Font = new Font("Arial", 8.25f);
      this.comboBoxPrice8.Location = new Point(10, 72);
      this.comboBoxPrice8.Name = "comboBoxPrice8";
      this.comboBoxPrice8.Size = new Size(203, 22);
      this.comboBoxPrice8.TabIndex = 63;
      this.comboBoxPrice8.Tag = (object) "2116";
      this.comboBoxPrice8.Enter += new EventHandler(this.comboBoxField_Enter);
      this.comboBoxPrice8.Leave += new EventHandler(this.strField_Leave);
      this.textBoxPrice9.Font = new Font("Arial", 8.25f);
      this.textBoxPrice9.Location = new Point(219, 73);
      this.textBoxPrice9.Name = "textBoxPrice9";
      this.textBoxPrice9.Size = new Size(102, 20);
      this.textBoxPrice9.TabIndex = 64;
      this.textBoxPrice9.Tag = (object) "2117";
      this.textBoxPrice9.TextAlign = HorizontalAlignment.Right;
      this.textBoxPrice9.Enter += new EventHandler(this.textField_Enter);
      this.textBoxPrice9.KeyPress += new KeyPressEventHandler(this.numField_KeyPress);
      this.textBoxPrice9.KeyUp += new KeyEventHandler(this.numField_KeyUp);
      this.textBoxPrice9.Leave += new EventHandler(this.numField_Leave);
      this.pnlBP.Controls.Add((Control) this.picBPZoomOut);
      this.pnlBP.Controls.Add((Control) this.picBPZoomIn);
      this.pnlBP.Controls.Add((Control) this.comboBoxPrice1);
      this.pnlBP.Controls.Add((Control) this.textBoxPrice2);
      this.pnlBP.Controls.Add((Control) this.comboBoxPrice2);
      this.pnlBP.Controls.Add((Control) this.label11);
      this.pnlBP.Controls.Add((Control) this.textBoxPrice1);
      this.pnlBP.Controls.Add((Control) this.textBoxPrice3);
      this.pnlBP.Controls.Add((Control) this.comboBoxPrice3);
      this.pnlBP.Controls.Add((Control) this.textBoxPrice4);
      this.pnlBP.Controls.Add((Control) this.comboBoxPrice4);
      this.pnlBP.Controls.Add((Control) this.textBoxPrice5);
      this.pnlBP.Dock = DockStyle.Top;
      this.pnlBP.Location = new Point(1, 959);
      this.pnlBP.Name = "pnlBP";
      this.pnlBP.Size = new Size(333, 130);
      this.pnlBP.TabIndex = 161;
      this.picBPZoomOut.BackgroundImageLayout = ImageLayout.None;
      this.picBPZoomOut.Cursor = Cursors.Hand;
      this.picBPZoomOut.Image = (Image) componentResourceManager.GetObject("picBPZoomOut.Image");
      this.picBPZoomOut.Location = new Point(107, 11);
      this.picBPZoomOut.Name = "picBPZoomOut";
      this.picBPZoomOut.Size = new Size(16, 16);
      this.picBPZoomOut.SizeMode = PictureBoxSizeMode.AutoSize;
      this.picBPZoomOut.TabIndex = 66;
      this.picBPZoomOut.TabStop = false;
      this.picBPZoomOut.Click += new EventHandler(this.ZoomButton_Clicked);
      this.picBPZoomIn.BackgroundImageLayout = ImageLayout.None;
      this.picBPZoomIn.Cursor = Cursors.Hand;
      this.picBPZoomIn.Image = (Image) componentResourceManager.GetObject("picBPZoomIn.Image");
      this.picBPZoomIn.Location = new Point(87, 11);
      this.picBPZoomIn.Name = "picBPZoomIn";
      this.picBPZoomIn.Size = new Size(16, 16);
      this.picBPZoomIn.SizeMode = PictureBoxSizeMode.AutoSize;
      this.picBPZoomIn.TabIndex = 65;
      this.picBPZoomIn.TabStop = false;
      this.picBPZoomIn.Click += new EventHandler(this.ZoomButton_Clicked);
      this.comboBoxPrice1.Font = new Font("Arial", 8.25f);
      this.comboBoxPrice1.Location = new Point(10, 31);
      this.comboBoxPrice1.Name = "comboBoxPrice1";
      this.comboBoxPrice1.Size = new Size(203, 22);
      this.comboBoxPrice1.TabIndex = 57;
      this.comboBoxPrice1.Tag = (object) "2102";
      this.comboBoxPrice1.Enter += new EventHandler(this.comboBoxField_Enter);
      this.comboBoxPrice1.Leave += new EventHandler(this.strField_Leave);
      this.textBoxPrice2.Font = new Font("Arial", 8.25f);
      this.textBoxPrice2.Location = new Point(219, 32);
      this.textBoxPrice2.Name = "textBoxPrice2";
      this.textBoxPrice2.Size = new Size(102, 20);
      this.textBoxPrice2.TabIndex = 58;
      this.textBoxPrice2.Tag = (object) "2103";
      this.textBoxPrice2.TextAlign = HorizontalAlignment.Right;
      this.textBoxPrice2.Enter += new EventHandler(this.textField_Enter);
      this.textBoxPrice2.KeyPress += new KeyPressEventHandler(this.numField_KeyPress);
      this.textBoxPrice2.KeyUp += new KeyEventHandler(this.numField_KeyUp);
      this.textBoxPrice2.Leave += new EventHandler(this.numField_Leave);
      this.comboBoxPrice2.Font = new Font("Arial", 8.25f);
      this.comboBoxPrice2.Location = new Point(10, 55);
      this.comboBoxPrice2.Name = "comboBoxPrice2";
      this.comboBoxPrice2.Size = new Size(203, 22);
      this.comboBoxPrice2.TabIndex = 59;
      this.comboBoxPrice2.Tag = (object) "2104";
      this.comboBoxPrice2.Enter += new EventHandler(this.comboBoxField_Enter);
      this.comboBoxPrice2.Leave += new EventHandler(this.strField_Leave);
      this.label11.AutoSize = true;
      this.label11.Location = new Point(7, 14);
      this.label11.Name = "label11";
      this.label11.Size = new Size(79, 13);
      this.label11.TabIndex = 19;
      this.label11.Text = "Base Buy Price";
      this.textBoxPrice1.Location = new Point(220, 7);
      this.textBoxPrice1.Name = "textBoxPrice1";
      this.textBoxPrice1.Size = new Size(102, 20);
      this.textBoxPrice1.TabIndex = 48;
      this.textBoxPrice1.Tag = (object) "2101";
      this.textBoxPrice1.TextAlign = HorizontalAlignment.Right;
      this.textBoxPrice1.Enter += new EventHandler(this.textField_Enter);
      this.textBoxPrice1.KeyPress += new KeyPressEventHandler(this.numField_KeyPress);
      this.textBoxPrice1.KeyUp += new KeyEventHandler(this.numField_KeyUp);
      this.textBoxPrice1.Leave += new EventHandler(this.numField_Leave);
      this.textBoxPrice3.Font = new Font("Arial", 8.25f);
      this.textBoxPrice3.Location = new Point(219, 56);
      this.textBoxPrice3.Name = "textBoxPrice3";
      this.textBoxPrice3.Size = new Size(102, 20);
      this.textBoxPrice3.TabIndex = 60;
      this.textBoxPrice3.Tag = (object) "2105";
      this.textBoxPrice3.TextAlign = HorizontalAlignment.Right;
      this.textBoxPrice3.Enter += new EventHandler(this.textField_Enter);
      this.textBoxPrice3.KeyPress += new KeyPressEventHandler(this.numField_KeyPress);
      this.textBoxPrice3.KeyUp += new KeyEventHandler(this.numField_KeyUp);
      this.textBoxPrice3.Leave += new EventHandler(this.numField_Leave);
      this.comboBoxPrice3.Font = new Font("Arial", 8.25f);
      this.comboBoxPrice3.Location = new Point(10, 79);
      this.comboBoxPrice3.Name = "comboBoxPrice3";
      this.comboBoxPrice3.Size = new Size(203, 22);
      this.comboBoxPrice3.TabIndex = 61;
      this.comboBoxPrice3.Tag = (object) "2106";
      this.comboBoxPrice3.Enter += new EventHandler(this.comboBoxField_Enter);
      this.comboBoxPrice3.Leave += new EventHandler(this.strField_Leave);
      this.textBoxPrice4.Font = new Font("Arial", 8.25f);
      this.textBoxPrice4.Location = new Point(219, 80);
      this.textBoxPrice4.Name = "textBoxPrice4";
      this.textBoxPrice4.Size = new Size(102, 20);
      this.textBoxPrice4.TabIndex = 62;
      this.textBoxPrice4.Tag = (object) "2107";
      this.textBoxPrice4.TextAlign = HorizontalAlignment.Right;
      this.textBoxPrice4.Enter += new EventHandler(this.textField_Enter);
      this.textBoxPrice4.KeyPress += new KeyPressEventHandler(this.numField_KeyPress);
      this.textBoxPrice4.KeyUp += new KeyEventHandler(this.numField_KeyUp);
      this.textBoxPrice4.Leave += new EventHandler(this.numField_Leave);
      this.comboBoxPrice4.Font = new Font("Arial", 8.25f);
      this.comboBoxPrice4.Location = new Point(10, 103);
      this.comboBoxPrice4.Name = "comboBoxPrice4";
      this.comboBoxPrice4.Size = new Size(203, 22);
      this.comboBoxPrice4.TabIndex = 63;
      this.comboBoxPrice4.Tag = (object) "2108";
      this.comboBoxPrice4.Enter += new EventHandler(this.comboBoxField_Enter);
      this.comboBoxPrice4.Leave += new EventHandler(this.strField_Leave);
      this.textBoxPrice5.Font = new Font("Arial", 8.25f);
      this.textBoxPrice5.Location = new Point(219, 104);
      this.textBoxPrice5.Name = "textBoxPrice5";
      this.textBoxPrice5.Size = new Size(102, 20);
      this.textBoxPrice5.TabIndex = 64;
      this.textBoxPrice5.Tag = (object) "2109";
      this.textBoxPrice5.TextAlign = HorizontalAlignment.Right;
      this.textBoxPrice5.Enter += new EventHandler(this.textField_Enter);
      this.textBoxPrice5.KeyPress += new KeyPressEventHandler(this.numField_KeyPress);
      this.textBoxPrice5.KeyUp += new KeyEventHandler(this.numField_KeyUp);
      this.textBoxPrice5.Leave += new EventHandler(this.numField_Leave);
      this.pnlRateRequested.Controls.Add((Control) this.label77);
      this.pnlRateRequested.Controls.Add((Control) this.textBox6);
      this.pnlRateRequested.Controls.Add((Control) this.label75);
      this.pnlRateRequested.Controls.Add((Control) this.textBox2);
      this.pnlRateRequested.Controls.Add((Control) this.label76);
      this.pnlRateRequested.Controls.Add((Control) this.textBox4);
      this.pnlRateRequested.Controls.Add((Control) this.label7);
      this.pnlRateRequested.Controls.Add((Control) this.textBoxRate22);
      this.pnlRateRequested.Controls.Add((Control) this.label6);
      this.pnlRateRequested.Controls.Add((Control) this.textBoxRate23);
      this.pnlRateRequested.Dock = DockStyle.Top;
      this.pnlRateRequested.Location = new Point(1, 846);
      this.pnlRateRequested.Name = "pnlRateRequested";
      this.pnlRateRequested.Size = new Size(333, 113);
      this.pnlRateRequested.TabIndex = 160;
      this.label77.AutoSize = true;
      this.label77.Font = new Font("Arial", 8.25f);
      this.label77.Location = new Point(7, 92);
      this.label77.Name = "label77";
      this.label77.Size = new Size(104, 14);
      this.label77.TabIndex = 97;
      this.label77.Tag = (object) "387";
      this.label77.Text = "Starting Adjust Price";
      this.textBox6.BackColor = Color.White;
      this.textBox6.Font = new Font("Arial", 8.25f);
      this.textBox6.Location = new Point(219, 89);
      this.textBox6.Name = "textBox6";
      this.textBox6.Size = new Size(102, 20);
      this.textBox6.TabIndex = 98;
      this.textBox6.TabStop = false;
      this.textBox6.Tag = (object) "3874";
      this.textBox6.TextAlign = HorizontalAlignment.Right;
      this.textBox6.Enter += new EventHandler(this.textField_Enter);
      this.textBox6.KeyPress += new KeyPressEventHandler(this.numField_KeyPress);
      this.textBox6.KeyUp += new KeyEventHandler(this.numField_KeyUp);
      this.textBox6.Leave += new EventHandler(this.numField_Leave);
      this.label75.AutoSize = true;
      this.label75.Font = new Font("Arial", 8.25f);
      this.label75.Location = new Point(7, 70);
      this.label75.Name = "label75";
      this.label75.Size = new Size(102, 14);
      this.label75.TabIndex = 95;
      this.label75.Tag = (object) "3873";
      this.label75.Text = "Starting Adjust Rate";
      this.textBox2.BackColor = Color.White;
      this.textBox2.Font = new Font("Arial", 8.25f);
      this.textBox2.Location = new Point(219, 67);
      this.textBox2.Name = "textBox2";
      this.textBox2.Size = new Size(102, 20);
      this.textBox2.TabIndex = 96;
      this.textBox2.TabStop = false;
      this.textBox2.Tag = (object) "3872";
      this.textBox2.TextAlign = HorizontalAlignment.Right;
      this.textBox2.Enter += new EventHandler(this.textField_Enter);
      this.textBox2.KeyPress += new KeyPressEventHandler(this.numField_KeyPress);
      this.textBox2.KeyUp += new KeyEventHandler(this.numField_KeyUp);
      this.textBox2.Leave += new EventHandler(this.numField_Leave);
      this.label76.AutoSize = true;
      this.label76.Font = new Font("Arial", 8.25f);
      this.label76.Location = new Point(7, 48);
      this.label76.Name = "label76";
      this.label76.Size = new Size(99, 14);
      this.label76.TabIndex = 93;
      this.label76.Tag = (object) "3848";
      this.label76.Text = "UnDiscounted Rate";
      this.textBox4.BackColor = Color.White;
      this.textBox4.Font = new Font("Arial", 8.25f);
      this.textBox4.Location = new Point(219, 45);
      this.textBox4.Name = "textBox4";
      this.textBox4.Size = new Size(102, 20);
      this.textBox4.TabIndex = 94;
      this.textBox4.TabStop = false;
      this.textBox4.Tag = (object) "3847";
      this.textBox4.TextAlign = HorizontalAlignment.Right;
      this.textBox4.Enter += new EventHandler(this.textField_Enter);
      this.textBox4.KeyPress += new KeyPressEventHandler(this.numField_KeyPress);
      this.textBox4.KeyUp += new KeyEventHandler(this.numField_KeyUp);
      this.textBox4.Leave += new EventHandler(this.numField_Leave);
      this.label7.AutoSize = true;
      this.label7.Font = new Font("Arial", 8.25f);
      this.label7.Location = new Point(7, 3);
      this.label7.Name = "label7";
      this.label7.Size = new Size(116, 14);
      this.label7.TabIndex = 14;
      this.label7.Text = "Total Rate Adjustments";
      this.textBoxRate22.BackColor = Color.WhiteSmoke;
      this.textBoxRate22.Font = new Font("Arial", 8.25f);
      this.textBoxRate22.Location = new Point(219, 0);
      this.textBoxRate22.Name = "textBoxRate22";
      this.textBoxRate22.ReadOnly = true;
      this.textBoxRate22.Size = new Size(102, 20);
      this.textBoxRate22.TabIndex = 15;
      this.textBoxRate22.TabStop = false;
      this.textBoxRate22.Tag = (object) "2099";
      this.textBoxRate22.TextAlign = HorizontalAlignment.Right;
      this.label6.AutoSize = true;
      this.label6.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label6.Location = new Point(7, 25);
      this.label6.Name = "label6";
      this.label6.Size = new Size(75, 14);
      this.label6.TabIndex = 16;
      this.label6.Text = "Net Buy Rate";
      this.textBoxRate23.BackColor = Color.WhiteSmoke;
      this.textBoxRate23.Font = new Font("Arial", 8.25f);
      this.textBoxRate23.Location = new Point(219, 22);
      this.textBoxRate23.Name = "textBoxRate23";
      this.textBoxRate23.ReadOnly = true;
      this.textBoxRate23.Size = new Size(102, 20);
      this.textBoxRate23.TabIndex = 16;
      this.textBoxRate23.TabStop = false;
      this.textBoxRate23.Tag = (object) "2100";
      this.textBoxRate23.TextAlign = HorizontalAlignment.Right;
      this.pnlBRExpand.Controls.Add((Control) this.textBoxRate21);
      this.pnlBRExpand.Controls.Add((Control) this.comboBoxRate7);
      this.pnlBRExpand.Controls.Add((Control) this.comboBoxRate20);
      this.pnlBRExpand.Controls.Add((Control) this.textBoxRate8);
      this.pnlBRExpand.Controls.Add((Control) this.textBoxRate20);
      this.pnlBRExpand.Controls.Add((Control) this.comboBoxRate8);
      this.pnlBRExpand.Controls.Add((Control) this.comboBoxRate19);
      this.pnlBRExpand.Controls.Add((Control) this.textBoxRate9);
      this.pnlBRExpand.Controls.Add((Control) this.textBoxRate19);
      this.pnlBRExpand.Controls.Add((Control) this.comboBoxRate9);
      this.pnlBRExpand.Controls.Add((Control) this.comboBoxRate18);
      this.pnlBRExpand.Controls.Add((Control) this.textBoxRate10);
      this.pnlBRExpand.Controls.Add((Control) this.textBoxRate18);
      this.pnlBRExpand.Controls.Add((Control) this.comboBoxRate10);
      this.pnlBRExpand.Controls.Add((Control) this.comboBoxRate17);
      this.pnlBRExpand.Controls.Add((Control) this.textBoxRate11);
      this.pnlBRExpand.Controls.Add((Control) this.textBoxRate17);
      this.pnlBRExpand.Controls.Add((Control) this.comboBoxRate11);
      this.pnlBRExpand.Controls.Add((Control) this.comboBoxRate16);
      this.pnlBRExpand.Controls.Add((Control) this.textBoxRate12);
      this.pnlBRExpand.Controls.Add((Control) this.textBoxRate16);
      this.pnlBRExpand.Controls.Add((Control) this.comboBoxRate12);
      this.pnlBRExpand.Controls.Add((Control) this.comboBoxRate15);
      this.pnlBRExpand.Controls.Add((Control) this.textBoxRate13);
      this.pnlBRExpand.Controls.Add((Control) this.textBoxRate15);
      this.pnlBRExpand.Controls.Add((Control) this.comboBoxRate13);
      this.pnlBRExpand.Controls.Add((Control) this.comboBoxRate14);
      this.pnlBRExpand.Controls.Add((Control) this.textBoxRate14);
      this.pnlBRExpand.Dock = DockStyle.Top;
      this.pnlBRExpand.Location = new Point(1, 510);
      this.pnlBRExpand.Name = "pnlBRExpand";
      this.pnlBRExpand.Size = new Size(333, 336);
      this.pnlBRExpand.TabIndex = 159;
      this.textBoxRate21.Font = new Font("Arial", 8.25f);
      this.textBoxRate21.Location = new Point(219, 313);
      this.textBoxRate21.Name = "textBoxRate21";
      this.textBoxRate21.Size = new Size(102, 20);
      this.textBoxRate21.TabIndex = 47;
      this.textBoxRate21.Tag = (object) "2447";
      this.textBoxRate21.TextAlign = HorizontalAlignment.Right;
      this.textBoxRate21.Enter += new EventHandler(this.textField_Enter);
      this.textBoxRate21.KeyPress += new KeyPressEventHandler(this.numField_KeyPress);
      this.textBoxRate21.KeyUp += new KeyEventHandler(this.numField_KeyUp);
      this.textBoxRate21.Leave += new EventHandler(this.numField_Leave);
      this.comboBoxRate7.Font = new Font("Arial", 8.25f);
      this.comboBoxRate7.Location = new Point(10, 0);
      this.comboBoxRate7.Name = "comboBoxRate7";
      this.comboBoxRate7.Size = new Size(203, 22);
      this.comboBoxRate7.TabIndex = 20;
      this.comboBoxRate7.Tag = (object) "2420";
      this.comboBoxRate7.Enter += new EventHandler(this.comboBoxField_Enter);
      this.comboBoxRate7.Leave += new EventHandler(this.strField_Leave);
      this.comboBoxRate20.Font = new Font("Arial", 8.25f);
      this.comboBoxRate20.Location = new Point(10, 312);
      this.comboBoxRate20.Name = "comboBoxRate20";
      this.comboBoxRate20.Size = new Size(203, 22);
      this.comboBoxRate20.TabIndex = 46;
      this.comboBoxRate20.Tag = (object) "2446";
      this.comboBoxRate20.Enter += new EventHandler(this.comboBoxField_Enter);
      this.comboBoxRate20.Leave += new EventHandler(this.strField_Leave);
      this.textBoxRate8.Font = new Font("Arial", 8.25f);
      this.textBoxRate8.Location = new Point(219, 1);
      this.textBoxRate8.Name = "textBoxRate8";
      this.textBoxRate8.Size = new Size(102, 20);
      this.textBoxRate8.TabIndex = 21;
      this.textBoxRate8.Tag = (object) "2421";
      this.textBoxRate8.TextAlign = HorizontalAlignment.Right;
      this.textBoxRate8.Enter += new EventHandler(this.textField_Enter);
      this.textBoxRate8.KeyPress += new KeyPressEventHandler(this.numField_KeyPress);
      this.textBoxRate8.KeyUp += new KeyEventHandler(this.numField_KeyUp);
      this.textBoxRate8.Leave += new EventHandler(this.numField_Leave);
      this.textBoxRate20.Font = new Font("Arial", 8.25f);
      this.textBoxRate20.Location = new Point(219, 289);
      this.textBoxRate20.Name = "textBoxRate20";
      this.textBoxRate20.Size = new Size(102, 20);
      this.textBoxRate20.TabIndex = 45;
      this.textBoxRate20.Tag = (object) "2445";
      this.textBoxRate20.TextAlign = HorizontalAlignment.Right;
      this.textBoxRate20.Enter += new EventHandler(this.textField_Enter);
      this.textBoxRate20.KeyPress += new KeyPressEventHandler(this.numField_KeyPress);
      this.textBoxRate20.KeyUp += new KeyEventHandler(this.numField_KeyUp);
      this.textBoxRate20.Leave += new EventHandler(this.numField_Leave);
      this.comboBoxRate8.Font = new Font("Arial", 8.25f);
      this.comboBoxRate8.Location = new Point(10, 24);
      this.comboBoxRate8.Name = "comboBoxRate8";
      this.comboBoxRate8.Size = new Size(203, 22);
      this.comboBoxRate8.TabIndex = 22;
      this.comboBoxRate8.Tag = (object) "2422";
      this.comboBoxRate8.Enter += new EventHandler(this.comboBoxField_Enter);
      this.comboBoxRate8.Leave += new EventHandler(this.strField_Leave);
      this.comboBoxRate19.Font = new Font("Arial", 8.25f);
      this.comboBoxRate19.Location = new Point(10, 288);
      this.comboBoxRate19.Name = "comboBoxRate19";
      this.comboBoxRate19.Size = new Size(203, 22);
      this.comboBoxRate19.TabIndex = 44;
      this.comboBoxRate19.Tag = (object) "2444";
      this.comboBoxRate19.Enter += new EventHandler(this.comboBoxField_Enter);
      this.comboBoxRate19.Leave += new EventHandler(this.strField_Leave);
      this.textBoxRate9.Font = new Font("Arial", 8.25f);
      this.textBoxRate9.Location = new Point(219, 25);
      this.textBoxRate9.Name = "textBoxRate9";
      this.textBoxRate9.Size = new Size(102, 20);
      this.textBoxRate9.TabIndex = 23;
      this.textBoxRate9.Tag = (object) "2423";
      this.textBoxRate9.TextAlign = HorizontalAlignment.Right;
      this.textBoxRate9.Enter += new EventHandler(this.textField_Enter);
      this.textBoxRate9.KeyPress += new KeyPressEventHandler(this.numField_KeyPress);
      this.textBoxRate9.KeyUp += new KeyEventHandler(this.numField_KeyUp);
      this.textBoxRate9.Leave += new EventHandler(this.numField_Leave);
      this.textBoxRate19.Font = new Font("Arial", 8.25f);
      this.textBoxRate19.Location = new Point(219, 265);
      this.textBoxRate19.Name = "textBoxRate19";
      this.textBoxRate19.Size = new Size(102, 20);
      this.textBoxRate19.TabIndex = 43;
      this.textBoxRate19.Tag = (object) "2443";
      this.textBoxRate19.TextAlign = HorizontalAlignment.Right;
      this.textBoxRate19.Enter += new EventHandler(this.textField_Enter);
      this.textBoxRate19.KeyPress += new KeyPressEventHandler(this.numField_KeyPress);
      this.textBoxRate19.KeyUp += new KeyEventHandler(this.numField_KeyUp);
      this.textBoxRate19.Leave += new EventHandler(this.numField_Leave);
      this.comboBoxRate9.Font = new Font("Arial", 8.25f);
      this.comboBoxRate9.Location = new Point(10, 48);
      this.comboBoxRate9.Name = "comboBoxRate9";
      this.comboBoxRate9.Size = new Size(203, 22);
      this.comboBoxRate9.TabIndex = 24;
      this.comboBoxRate9.Tag = (object) "2424";
      this.comboBoxRate9.Enter += new EventHandler(this.comboBoxField_Enter);
      this.comboBoxRate9.Leave += new EventHandler(this.strField_Leave);
      this.comboBoxRate18.Font = new Font("Arial", 8.25f);
      this.comboBoxRate18.Location = new Point(10, 264);
      this.comboBoxRate18.Name = "comboBoxRate18";
      this.comboBoxRate18.Size = new Size(203, 22);
      this.comboBoxRate18.TabIndex = 42;
      this.comboBoxRate18.Tag = (object) "2442";
      this.comboBoxRate18.Enter += new EventHandler(this.comboBoxField_Enter);
      this.comboBoxRate18.Leave += new EventHandler(this.strField_Leave);
      this.textBoxRate10.Font = new Font("Arial", 8.25f);
      this.textBoxRate10.Location = new Point(219, 49);
      this.textBoxRate10.Name = "textBoxRate10";
      this.textBoxRate10.Size = new Size(102, 20);
      this.textBoxRate10.TabIndex = 25;
      this.textBoxRate10.Tag = (object) "2425";
      this.textBoxRate10.TextAlign = HorizontalAlignment.Right;
      this.textBoxRate10.Enter += new EventHandler(this.textField_Enter);
      this.textBoxRate10.KeyPress += new KeyPressEventHandler(this.numField_KeyPress);
      this.textBoxRate10.KeyUp += new KeyEventHandler(this.numField_KeyUp);
      this.textBoxRate10.Leave += new EventHandler(this.numField_Leave);
      this.textBoxRate18.Font = new Font("Arial", 8.25f);
      this.textBoxRate18.Location = new Point(219, 241);
      this.textBoxRate18.Name = "textBoxRate18";
      this.textBoxRate18.Size = new Size(102, 20);
      this.textBoxRate18.TabIndex = 41;
      this.textBoxRate18.Tag = (object) "2441";
      this.textBoxRate18.TextAlign = HorizontalAlignment.Right;
      this.textBoxRate18.Enter += new EventHandler(this.textField_Enter);
      this.textBoxRate18.KeyPress += new KeyPressEventHandler(this.numField_KeyPress);
      this.textBoxRate18.KeyUp += new KeyEventHandler(this.numField_KeyUp);
      this.textBoxRate18.Leave += new EventHandler(this.numField_Leave);
      this.comboBoxRate10.Font = new Font("Arial", 8.25f);
      this.comboBoxRate10.Location = new Point(10, 72);
      this.comboBoxRate10.Name = "comboBoxRate10";
      this.comboBoxRate10.Size = new Size(203, 22);
      this.comboBoxRate10.TabIndex = 26;
      this.comboBoxRate10.Tag = (object) "2426";
      this.comboBoxRate10.Enter += new EventHandler(this.comboBoxField_Enter);
      this.comboBoxRate10.Leave += new EventHandler(this.strField_Leave);
      this.comboBoxRate17.Font = new Font("Arial", 8.25f);
      this.comboBoxRate17.Location = new Point(10, 240);
      this.comboBoxRate17.Name = "comboBoxRate17";
      this.comboBoxRate17.Size = new Size(203, 22);
      this.comboBoxRate17.TabIndex = 40;
      this.comboBoxRate17.Tag = (object) "2440";
      this.comboBoxRate17.Enter += new EventHandler(this.comboBoxField_Enter);
      this.comboBoxRate17.Leave += new EventHandler(this.strField_Leave);
      this.textBoxRate11.Font = new Font("Arial", 8.25f);
      this.textBoxRate11.Location = new Point(219, 73);
      this.textBoxRate11.Name = "textBoxRate11";
      this.textBoxRate11.Size = new Size(102, 20);
      this.textBoxRate11.TabIndex = 27;
      this.textBoxRate11.Tag = (object) "2427";
      this.textBoxRate11.TextAlign = HorizontalAlignment.Right;
      this.textBoxRate11.Enter += new EventHandler(this.textField_Enter);
      this.textBoxRate11.KeyPress += new KeyPressEventHandler(this.numField_KeyPress);
      this.textBoxRate11.KeyUp += new KeyEventHandler(this.numField_KeyUp);
      this.textBoxRate11.Leave += new EventHandler(this.numField_Leave);
      this.textBoxRate17.Font = new Font("Arial", 8.25f);
      this.textBoxRate17.Location = new Point(219, 217);
      this.textBoxRate17.Name = "textBoxRate17";
      this.textBoxRate17.Size = new Size(102, 20);
      this.textBoxRate17.TabIndex = 39;
      this.textBoxRate17.Tag = (object) "2439";
      this.textBoxRate17.TextAlign = HorizontalAlignment.Right;
      this.textBoxRate17.Enter += new EventHandler(this.textField_Enter);
      this.textBoxRate17.KeyPress += new KeyPressEventHandler(this.numField_KeyPress);
      this.textBoxRate17.KeyUp += new KeyEventHandler(this.numField_KeyUp);
      this.textBoxRate17.Leave += new EventHandler(this.numField_Leave);
      this.comboBoxRate11.Font = new Font("Arial", 8.25f);
      this.comboBoxRate11.Location = new Point(10, 96);
      this.comboBoxRate11.Name = "comboBoxRate11";
      this.comboBoxRate11.Size = new Size(203, 22);
      this.comboBoxRate11.TabIndex = 28;
      this.comboBoxRate11.Tag = (object) "2428";
      this.comboBoxRate11.Enter += new EventHandler(this.comboBoxField_Enter);
      this.comboBoxRate11.Leave += new EventHandler(this.strField_Leave);
      this.comboBoxRate16.Font = new Font("Arial", 8.25f);
      this.comboBoxRate16.Location = new Point(10, 216);
      this.comboBoxRate16.Name = "comboBoxRate16";
      this.comboBoxRate16.Size = new Size(203, 22);
      this.comboBoxRate16.TabIndex = 38;
      this.comboBoxRate16.Tag = (object) "2438";
      this.comboBoxRate16.Enter += new EventHandler(this.comboBoxField_Enter);
      this.comboBoxRate16.Leave += new EventHandler(this.strField_Leave);
      this.textBoxRate12.Font = new Font("Arial", 8.25f);
      this.textBoxRate12.Location = new Point(219, 97);
      this.textBoxRate12.Name = "textBoxRate12";
      this.textBoxRate12.Size = new Size(102, 20);
      this.textBoxRate12.TabIndex = 29;
      this.textBoxRate12.Tag = (object) "2429";
      this.textBoxRate12.TextAlign = HorizontalAlignment.Right;
      this.textBoxRate12.Enter += new EventHandler(this.textField_Enter);
      this.textBoxRate12.KeyPress += new KeyPressEventHandler(this.numField_KeyPress);
      this.textBoxRate12.KeyUp += new KeyEventHandler(this.numField_KeyUp);
      this.textBoxRate12.Leave += new EventHandler(this.numField_Leave);
      this.textBoxRate16.Font = new Font("Arial", 8.25f);
      this.textBoxRate16.Location = new Point(219, 193);
      this.textBoxRate16.Name = "textBoxRate16";
      this.textBoxRate16.Size = new Size(102, 20);
      this.textBoxRate16.TabIndex = 37;
      this.textBoxRate16.Tag = (object) "2437";
      this.textBoxRate16.TextAlign = HorizontalAlignment.Right;
      this.textBoxRate16.Enter += new EventHandler(this.textField_Enter);
      this.textBoxRate16.KeyPress += new KeyPressEventHandler(this.numField_KeyPress);
      this.textBoxRate16.KeyUp += new KeyEventHandler(this.numField_KeyUp);
      this.textBoxRate16.Leave += new EventHandler(this.numField_Leave);
      this.comboBoxRate12.Font = new Font("Arial", 8.25f);
      this.comboBoxRate12.Location = new Point(10, 120);
      this.comboBoxRate12.Name = "comboBoxRate12";
      this.comboBoxRate12.Size = new Size(203, 22);
      this.comboBoxRate12.TabIndex = 30;
      this.comboBoxRate12.Tag = (object) "2430";
      this.comboBoxRate12.Enter += new EventHandler(this.comboBoxField_Enter);
      this.comboBoxRate12.Leave += new EventHandler(this.strField_Leave);
      this.comboBoxRate15.Font = new Font("Arial", 8.25f);
      this.comboBoxRate15.Location = new Point(10, 192);
      this.comboBoxRate15.Name = "comboBoxRate15";
      this.comboBoxRate15.Size = new Size(203, 22);
      this.comboBoxRate15.TabIndex = 36;
      this.comboBoxRate15.Tag = (object) "2436";
      this.comboBoxRate15.Enter += new EventHandler(this.comboBoxField_Enter);
      this.comboBoxRate15.Leave += new EventHandler(this.strField_Leave);
      this.textBoxRate13.Font = new Font("Arial", 8.25f);
      this.textBoxRate13.Location = new Point(219, 121);
      this.textBoxRate13.Name = "textBoxRate13";
      this.textBoxRate13.Size = new Size(102, 20);
      this.textBoxRate13.TabIndex = 31;
      this.textBoxRate13.Tag = (object) "2431";
      this.textBoxRate13.TextAlign = HorizontalAlignment.Right;
      this.textBoxRate13.Enter += new EventHandler(this.textField_Enter);
      this.textBoxRate13.KeyPress += new KeyPressEventHandler(this.numField_KeyPress);
      this.textBoxRate13.KeyUp += new KeyEventHandler(this.numField_KeyUp);
      this.textBoxRate13.Leave += new EventHandler(this.numField_Leave);
      this.textBoxRate15.Font = new Font("Arial", 8.25f);
      this.textBoxRate15.Location = new Point(219, 169);
      this.textBoxRate15.Name = "textBoxRate15";
      this.textBoxRate15.Size = new Size(102, 20);
      this.textBoxRate15.TabIndex = 35;
      this.textBoxRate15.Tag = (object) "2435";
      this.textBoxRate15.TextAlign = HorizontalAlignment.Right;
      this.textBoxRate15.Enter += new EventHandler(this.textField_Enter);
      this.textBoxRate15.KeyPress += new KeyPressEventHandler(this.numField_KeyPress);
      this.textBoxRate15.KeyUp += new KeyEventHandler(this.numField_KeyUp);
      this.textBoxRate15.Leave += new EventHandler(this.numField_Leave);
      this.comboBoxRate13.Font = new Font("Arial", 8.25f);
      this.comboBoxRate13.Location = new Point(10, 144);
      this.comboBoxRate13.Name = "comboBoxRate13";
      this.comboBoxRate13.Size = new Size(203, 22);
      this.comboBoxRate13.TabIndex = 32;
      this.comboBoxRate13.Tag = (object) "2432";
      this.comboBoxRate13.Enter += new EventHandler(this.comboBoxField_Enter);
      this.comboBoxRate13.Leave += new EventHandler(this.strField_Leave);
      this.comboBoxRate14.Font = new Font("Arial", 8.25f);
      this.comboBoxRate14.Location = new Point(10, 168);
      this.comboBoxRate14.Name = "comboBoxRate14";
      this.comboBoxRate14.Size = new Size(203, 22);
      this.comboBoxRate14.TabIndex = 34;
      this.comboBoxRate14.Tag = (object) "2434";
      this.comboBoxRate14.Enter += new EventHandler(this.comboBoxField_Enter);
      this.comboBoxRate14.Leave += new EventHandler(this.strField_Leave);
      this.textBoxRate14.Font = new Font("Arial", 8.25f);
      this.textBoxRate14.Location = new Point(219, 145);
      this.textBoxRate14.Name = "textBoxRate14";
      this.textBoxRate14.Size = new Size(102, 20);
      this.textBoxRate14.TabIndex = 33;
      this.textBoxRate14.Tag = (object) "2433";
      this.textBoxRate14.TextAlign = HorizontalAlignment.Right;
      this.textBoxRate14.Enter += new EventHandler(this.textField_Enter);
      this.textBoxRate14.KeyPress += new KeyPressEventHandler(this.numField_KeyPress);
      this.textBoxRate14.KeyUp += new KeyEventHandler(this.numField_KeyUp);
      this.textBoxRate14.Leave += new EventHandler(this.numField_Leave);
      this.pnlBR.Controls.Add((Control) this.comboBoxRate1);
      this.pnlBR.Controls.Add((Control) this.textBoxRate2);
      this.pnlBR.Controls.Add((Control) this.comboBoxRate2);
      this.pnlBR.Controls.Add((Control) this.textBoxRate3);
      this.pnlBR.Controls.Add((Control) this.comboBoxRate3);
      this.pnlBR.Controls.Add((Control) this.textBoxRate4);
      this.pnlBR.Controls.Add((Control) this.comboBoxRate4);
      this.pnlBR.Controls.Add((Control) this.textBoxRate5);
      this.pnlBR.Controls.Add((Control) this.comboBoxRate5);
      this.pnlBR.Controls.Add((Control) this.textBoxRate6);
      this.pnlBR.Controls.Add((Control) this.comboBoxRate6);
      this.pnlBR.Controls.Add((Control) this.textBoxRate7);
      this.pnlBR.Controls.Add((Control) this.picBRZoomOut);
      this.pnlBR.Controls.Add((Control) this.picBRZoomIn);
      this.pnlBR.Controls.Add((Control) this.label8);
      this.pnlBR.Controls.Add((Control) this.textBoxRate1);
      this.pnlBR.Dock = DockStyle.Top;
      this.pnlBR.Location = new Point(1, 329);
      this.pnlBR.Name = "pnlBR";
      this.pnlBR.Size = new Size(333, 181);
      this.pnlBR.TabIndex = 157;
      this.comboBoxRate1.Font = new Font("Arial", 8.25f);
      this.comboBoxRate1.Location = new Point(11, 33);
      this.comboBoxRate1.Name = "comboBoxRate1";
      this.comboBoxRate1.Size = new Size(203, 22);
      this.comboBoxRate1.TabIndex = 160;
      this.comboBoxRate1.Tag = (object) "2093";
      this.comboBoxRate1.Enter += new EventHandler(this.comboBoxField_Enter);
      this.comboBoxRate1.Leave += new EventHandler(this.strField_Leave);
      this.textBoxRate2.Font = new Font("Arial", 8.25f);
      this.textBoxRate2.Location = new Point(220, 34);
      this.textBoxRate2.Name = "textBoxRate2";
      this.textBoxRate2.Size = new Size(102, 20);
      this.textBoxRate2.TabIndex = 161;
      this.textBoxRate2.Tag = (object) "2094";
      this.textBoxRate2.TextAlign = HorizontalAlignment.Right;
      this.textBoxRate2.Enter += new EventHandler(this.textField_Enter);
      this.textBoxRate2.KeyPress += new KeyPressEventHandler(this.numField_KeyPress);
      this.textBoxRate2.KeyUp += new KeyEventHandler(this.numField_KeyUp);
      this.textBoxRate2.Leave += new EventHandler(this.numField_Leave);
      this.comboBoxRate2.Font = new Font("Arial", 8.25f);
      this.comboBoxRate2.Location = new Point(11, 57);
      this.comboBoxRate2.Name = "comboBoxRate2";
      this.comboBoxRate2.Size = new Size(203, 22);
      this.comboBoxRate2.TabIndex = 162;
      this.comboBoxRate2.Tag = (object) "2095";
      this.comboBoxRate2.Enter += new EventHandler(this.comboBoxField_Enter);
      this.comboBoxRate2.Leave += new EventHandler(this.strField_Leave);
      this.textBoxRate3.Font = new Font("Arial", 8.25f);
      this.textBoxRate3.Location = new Point(220, 58);
      this.textBoxRate3.Name = "textBoxRate3";
      this.textBoxRate3.Size = new Size(102, 20);
      this.textBoxRate3.TabIndex = 163;
      this.textBoxRate3.Tag = (object) "2096";
      this.textBoxRate3.TextAlign = HorizontalAlignment.Right;
      this.textBoxRate3.Enter += new EventHandler(this.textField_Enter);
      this.textBoxRate3.KeyPress += new KeyPressEventHandler(this.numField_KeyPress);
      this.textBoxRate3.KeyUp += new KeyEventHandler(this.numField_KeyUp);
      this.textBoxRate3.Leave += new EventHandler(this.numField_Leave);
      this.comboBoxRate3.Font = new Font("Arial", 8.25f);
      this.comboBoxRate3.Location = new Point(11, 81);
      this.comboBoxRate3.Name = "comboBoxRate3";
      this.comboBoxRate3.Size = new Size(203, 22);
      this.comboBoxRate3.TabIndex = 164;
      this.comboBoxRate3.Tag = (object) "2097";
      this.comboBoxRate3.Enter += new EventHandler(this.comboBoxField_Enter);
      this.comboBoxRate3.Leave += new EventHandler(this.strField_Leave);
      this.textBoxRate4.Font = new Font("Arial", 8.25f);
      this.textBoxRate4.Location = new Point(220, 82);
      this.textBoxRate4.Name = "textBoxRate4";
      this.textBoxRate4.Size = new Size(102, 20);
      this.textBoxRate4.TabIndex = 165;
      this.textBoxRate4.Tag = (object) "2098";
      this.textBoxRate4.TextAlign = HorizontalAlignment.Right;
      this.textBoxRate4.Enter += new EventHandler(this.textField_Enter);
      this.textBoxRate4.KeyPress += new KeyPressEventHandler(this.numField_KeyPress);
      this.textBoxRate4.KeyUp += new KeyEventHandler(this.numField_KeyUp);
      this.textBoxRate4.Leave += new EventHandler(this.numField_Leave);
      this.comboBoxRate4.Font = new Font("Arial", 8.25f);
      this.comboBoxRate4.Location = new Point(11, 105);
      this.comboBoxRate4.Name = "comboBoxRate4";
      this.comboBoxRate4.Size = new Size(203, 22);
      this.comboBoxRate4.TabIndex = 166;
      this.comboBoxRate4.Tag = (object) "2414";
      this.comboBoxRate4.Enter += new EventHandler(this.comboBoxField_Enter);
      this.comboBoxRate4.Leave += new EventHandler(this.strField_Leave);
      this.textBoxRate5.Font = new Font("Arial", 8.25f);
      this.textBoxRate5.Location = new Point(220, 106);
      this.textBoxRate5.Name = "textBoxRate5";
      this.textBoxRate5.Size = new Size(102, 20);
      this.textBoxRate5.TabIndex = 167;
      this.textBoxRate5.Tag = (object) "2449";
      this.textBoxRate5.TextAlign = HorizontalAlignment.Right;
      this.textBoxRate5.Enter += new EventHandler(this.textField_Enter);
      this.textBoxRate5.KeyPress += new KeyPressEventHandler(this.numField_KeyPress);
      this.textBoxRate5.KeyUp += new KeyEventHandler(this.numField_KeyUp);
      this.textBoxRate5.Leave += new EventHandler(this.numField_Leave);
      this.comboBoxRate5.Font = new Font("Arial", 8.25f);
      this.comboBoxRate5.Location = new Point(11, 129);
      this.comboBoxRate5.Name = "comboBoxRate5";
      this.comboBoxRate5.Size = new Size(203, 22);
      this.comboBoxRate5.TabIndex = 168;
      this.comboBoxRate5.Tag = (object) "2416";
      this.comboBoxRate5.Enter += new EventHandler(this.comboBoxField_Enter);
      this.comboBoxRate5.Leave += new EventHandler(this.strField_Leave);
      this.textBoxRate6.Font = new Font("Arial", 8.25f);
      this.textBoxRate6.Location = new Point(220, 130);
      this.textBoxRate6.Name = "textBoxRate6";
      this.textBoxRate6.Size = new Size(102, 20);
      this.textBoxRate6.TabIndex = 169;
      this.textBoxRate6.Tag = (object) "2417";
      this.textBoxRate6.TextAlign = HorizontalAlignment.Right;
      this.textBoxRate6.Enter += new EventHandler(this.textField_Enter);
      this.textBoxRate6.KeyPress += new KeyPressEventHandler(this.numField_KeyPress);
      this.textBoxRate6.KeyUp += new KeyEventHandler(this.numField_KeyUp);
      this.textBoxRate6.Leave += new EventHandler(this.numField_Leave);
      this.comboBoxRate6.Font = new Font("Arial", 8.25f);
      this.comboBoxRate6.Location = new Point(11, 153);
      this.comboBoxRate6.Name = "comboBoxRate6";
      this.comboBoxRate6.Size = new Size(203, 22);
      this.comboBoxRate6.TabIndex = 170;
      this.comboBoxRate6.Tag = (object) "2418";
      this.comboBoxRate6.Enter += new EventHandler(this.comboBoxField_Enter);
      this.comboBoxRate6.Leave += new EventHandler(this.strField_Leave);
      this.textBoxRate7.Font = new Font("Arial", 8.25f);
      this.textBoxRate7.Location = new Point(220, 154);
      this.textBoxRate7.Name = "textBoxRate7";
      this.textBoxRate7.Size = new Size(102, 20);
      this.textBoxRate7.TabIndex = 171;
      this.textBoxRate7.Tag = (object) "2419";
      this.textBoxRate7.TextAlign = HorizontalAlignment.Right;
      this.textBoxRate7.Enter += new EventHandler(this.textField_Enter);
      this.textBoxRate7.KeyPress += new KeyPressEventHandler(this.numField_KeyPress);
      this.textBoxRate7.KeyUp += new KeyEventHandler(this.numField_KeyUp);
      this.textBoxRate7.Leave += new EventHandler(this.numField_Leave);
      this.picBRZoomOut.BackgroundImageLayout = ImageLayout.None;
      this.picBRZoomOut.Cursor = Cursors.Hand;
      this.picBRZoomOut.Image = (Image) componentResourceManager.GetObject("picBRZoomOut.Image");
      this.picBRZoomOut.Location = new Point(112, 9);
      this.picBRZoomOut.Name = "picBRZoomOut";
      this.picBRZoomOut.Size = new Size(16, 16);
      this.picBRZoomOut.SizeMode = PictureBoxSizeMode.AutoSize;
      this.picBRZoomOut.TabIndex = 14;
      this.picBRZoomOut.TabStop = false;
      this.picBRZoomOut.Click += new EventHandler(this.ZoomButton_Clicked);
      this.picBRZoomIn.BackgroundImageLayout = ImageLayout.None;
      this.picBRZoomIn.Cursor = Cursors.Hand;
      this.picBRZoomIn.Image = (Image) componentResourceManager.GetObject("picBRZoomIn.Image");
      this.picBRZoomIn.Location = new Point(90, 9);
      this.picBRZoomIn.Name = "picBRZoomIn";
      this.picBRZoomIn.Size = new Size(16, 16);
      this.picBRZoomIn.SizeMode = PictureBoxSizeMode.AutoSize;
      this.picBRZoomIn.TabIndex = 13;
      this.picBRZoomIn.TabStop = false;
      this.picBRZoomIn.Click += new EventHandler(this.ZoomButton_Clicked);
      this.label8.AutoSize = true;
      this.label8.Font = new Font("Arial", 8.25f);
      this.label8.Location = new Point(10, 11);
      this.label8.Name = "label8";
      this.label8.Size = new Size(79, 14);
      this.label8.TabIndex = 12;
      this.label8.Text = "Base Buy Rate";
      this.textBoxRate1.Font = new Font("Arial", 8.25f);
      this.textBoxRate1.Location = new Point(219, 7);
      this.textBoxRate1.Name = "textBoxRate1";
      this.textBoxRate1.Size = new Size(102, 20);
      this.textBoxRate1.TabIndex = 6;
      this.textBoxRate1.Tag = (object) "2092";
      this.textBoxRate1.TextAlign = HorizontalAlignment.Right;
      this.textBoxRate1.Enter += new EventHandler(this.textField_Enter);
      this.textBoxRate1.KeyPress += new KeyPressEventHandler(this.numField_KeyPress);
      this.textBoxRate1.KeyUp += new KeyEventHandler(this.numField_KeyUp);
      this.textBoxRate1.Leave += new EventHandler(this.numField_Leave);
      this.pnlLockInfo.BackColor = Color.WhiteSmoke;
      this.pnlLockInfo.Controls.Add((Control) this.txtExpireDate);
      this.pnlLockInfo.Controls.Add((Control) this.lblLockExpirationDate);
      this.pnlLockInfo.Controls.Add((Control) this.pnlLockExtension);
      this.pnlLockInfo.Controls.Add((Control) this.txtLockDate);
      this.pnlLockInfo.Controls.Add((Control) this.dtLastRateSetDate);
      this.pnlLockInfo.Controls.Add((Control) this.label3);
      this.pnlLockInfo.Controls.Add((Control) this.groupBox10);
      this.pnlLockInfo.Controls.Add((Control) this.txtLockDays);
      this.pnlLockInfo.Controls.Add((Control) this.label41);
      this.pnlLockInfo.Controls.Add((Control) this.label40);
      this.pnlLockInfo.Dock = DockStyle.Top;
      this.pnlLockInfo.Location = new Point(1, 130);
      this.pnlLockInfo.Name = "pnlLockInfo";
      this.pnlLockInfo.Size = new Size(333, 199);
      this.pnlLockInfo.TabIndex = 156;
      this.txtExpireDate.BackColor = SystemColors.Window;
      this.txtExpireDate.Location = new Point(154, 73);
      this.txtExpireDate.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.txtExpireDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.txtExpireDate.Name = "txtExpireDate";
      this.txtExpireDate.Size = new Size(167, 21);
      this.txtExpireDate.TabIndex = 92;
      this.txtExpireDate.Tag = (object) "2091";
      this.txtExpireDate.ToolTip = "";
      this.txtExpireDate.Value = new DateTime(0L);
      this.txtExpireDate.ValueChanged += new EventHandler(this.strField_Leave);
      this.txtExpireDate.Leave += new EventHandler(this.strField_Leave);
      this.lblLockExpirationDate.AutoSize = true;
      this.lblLockExpirationDate.Font = new Font("Arial", 8.25f);
      this.lblLockExpirationDate.Location = new Point(7, 77);
      this.lblLockExpirationDate.Name = "lblLockExpirationDate";
      this.lblLockExpirationDate.Size = new Size(105, 14);
      this.lblLockExpirationDate.TabIndex = 91;
      this.lblLockExpirationDate.Text = "Lock Expiration Date";
      this.pnlLockExtension.Controls.Add((Control) this.label17);
      this.pnlLockExtension.Controls.Add((Control) this.txtPriceAdj);
      this.pnlLockExtension.Controls.Add((Control) this.txtNewLockExpDate);
      this.pnlLockExtension.Controls.Add((Control) this.label13);
      this.pnlLockExtension.Controls.Add((Control) this.txtDaysToExtent);
      this.pnlLockExtension.Controls.Add((Control) this.label12);
      this.pnlLockExtension.Controls.Add((Control) this.txtCurLockExpDate);
      this.pnlLockExtension.Controls.Add((Control) this.label5);
      this.pnlLockExtension.Location = new Point(4, 96);
      this.pnlLockExtension.Name = "pnlLockExtension";
      this.pnlLockExtension.Size = new Size(326, 98);
      this.pnlLockExtension.TabIndex = 90;
      this.label17.AutoSize = true;
      this.label17.Font = new Font("Arial", 8.25f);
      this.label17.Location = new Point(3, 77);
      this.label17.Name = "label17";
      this.label17.Size = new Size(87, 14);
      this.label17.TabIndex = 98;
      this.label17.Text = "Price Adjustment";
      this.txtPriceAdj.Font = new Font("Arial", 8.25f);
      this.txtPriceAdj.Location = new Point(150, 74);
      this.txtPriceAdj.Name = "txtPriceAdj";
      this.txtPriceAdj.Size = new Size(169, 20);
      this.txtPriceAdj.TabIndex = 97;
      this.txtPriceAdj.Tag = (object) "3362";
      this.txtPriceAdj.TextAlign = HorizontalAlignment.Right;
      this.txtPriceAdj.Leave += new EventHandler(this.lockExtensionField_Leave);
      this.txtNewLockExpDate.BackColor = SystemColors.Window;
      this.txtNewLockExpDate.Location = new Point(150, 49);
      this.txtNewLockExpDate.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.txtNewLockExpDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.txtNewLockExpDate.Name = "txtNewLockExpDate";
      this.txtNewLockExpDate.Size = new Size(167, 21);
      this.txtNewLockExpDate.TabIndex = 96;
      this.txtNewLockExpDate.Tag = (object) "3361";
      this.txtNewLockExpDate.ToolTip = "";
      this.txtNewLockExpDate.Value = new DateTime(0L);
      this.txtNewLockExpDate.ValueChanged += new EventHandler(this.lockExtensionField_Leave);
      this.txtNewLockExpDate.Leave += new EventHandler(this.lockExtensionField_Leave);
      this.label13.AutoSize = true;
      this.label13.Font = new Font("Arial", 8.25f);
      this.label13.Location = new Point(3, 53);
      this.label13.Name = "label13";
      this.label13.Size = new Size(131, 14);
      this.label13.TabIndex = 95;
      this.label13.Text = "New Lock Expiration Date";
      this.txtDaysToExtent.Font = new Font("Arial", 8.25f);
      this.txtDaysToExtent.Location = new Point(150, 26);
      this.txtDaysToExtent.Name = "txtDaysToExtent";
      this.txtDaysToExtent.Size = new Size(169, 20);
      this.txtDaysToExtent.TabIndex = 94;
      this.txtDaysToExtent.Tag = (object) "3360";
      this.txtDaysToExtent.TextAlign = HorizontalAlignment.Right;
      this.txtDaysToExtent.Leave += new EventHandler(this.lockExtensionField_Leave);
      this.label12.AutoSize = true;
      this.label12.Font = new Font("Arial", 8.25f);
      this.label12.Location = new Point(3, 29);
      this.label12.Name = "label12";
      this.label12.Size = new Size(80, 14);
      this.label12.TabIndex = 93;
      this.label12.Text = "Days to Extend";
      this.txtCurLockExpDate.BackColor = SystemColors.Window;
      this.txtCurLockExpDate.Location = new Point(150, 2);
      this.txtCurLockExpDate.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.txtCurLockExpDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.txtCurLockExpDate.Name = "txtCurLockExpDate";
      this.txtCurLockExpDate.Size = new Size(167, 21);
      this.txtCurLockExpDate.TabIndex = 92;
      this.txtCurLockExpDate.Tag = (object) "3369";
      this.txtCurLockExpDate.ToolTip = "";
      this.txtCurLockExpDate.Value = new DateTime(0L);
      this.label5.AutoSize = true;
      this.label5.Font = new Font("Arial", 8.25f);
      this.label5.Location = new Point(3, 6);
      this.label5.Name = "label5";
      this.label5.Size = new Size(144, 14);
      this.label5.TabIndex = 91;
      this.label5.Text = "Current Lock Expiration Date";
      this.txtLockDate.BackColor = SystemColors.Window;
      this.txtLockDate.Location = new Point(154, 25);
      this.txtLockDate.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.txtLockDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.txtLockDate.Name = "txtLockDate";
      this.txtLockDate.Size = new Size(167, 21);
      this.txtLockDate.TabIndex = 77;
      this.txtLockDate.Tag = (object) "2089";
      this.txtLockDate.ToolTip = "";
      this.txtLockDate.Value = new DateTime(0L);
      this.txtLockDate.ValueChanged += new EventHandler(this.strField_Leave);
      this.txtLockDate.Leave += new EventHandler(this.strField_Leave);
      this.dtLastRateSetDate.BackColor = SystemColors.Window;
      this.dtLastRateSetDate.Location = new Point(154, 0);
      this.dtLastRateSetDate.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.dtLastRateSetDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dtLastRateSetDate.Name = "dtLastRateSetDate";
      this.dtLastRateSetDate.Size = new Size(167, 21);
      this.dtLastRateSetDate.TabIndex = 76;
      this.dtLastRateSetDate.Tag = (object) "3254";
      this.dtLastRateSetDate.ToolTip = "";
      this.dtLastRateSetDate.Value = new DateTime(0L);
      this.dtLastRateSetDate.ValueChanged += new EventHandler(this.strField_Leave);
      this.dtLastRateSetDate.Enter += new EventHandler(this.textField_Enter);
      this.dtLastRateSetDate.Leave += new EventHandler(this.strField_Leave);
      this.label3.AutoSize = true;
      this.label3.Font = new Font("Arial", 8.25f);
      this.label3.Location = new Point(7, 3);
      this.label3.Name = "label3";
      this.label3.Size = new Size(97, 14);
      this.label3.TabIndex = 75;
      this.label3.Text = "Last Rate Set Date";
      this.groupBox10.Dock = DockStyle.Bottom;
      this.groupBox10.Location = new Point(0, 195);
      this.groupBox10.Name = "groupBox10";
      this.groupBox10.Size = new Size(333, 4);
      this.groupBox10.TabIndex = 71;
      this.groupBox10.TabStop = false;
      this.groupBox10.Text = "groupBox10";
      this.txtLockDays.Font = new Font("Arial", 8.25f);
      this.txtLockDays.Location = new Point(154, 50);
      this.txtLockDays.Name = "txtLockDays";
      this.txtLockDays.Size = new Size(169, 20);
      this.txtLockDays.TabIndex = 73;
      this.txtLockDays.Tag = (object) "2090";
      this.txtLockDays.TextAlign = HorizontalAlignment.Right;
      this.txtLockDays.Enter += new EventHandler(this.textField_Enter);
      this.txtLockDays.KeyPress += new KeyPressEventHandler(this.numField_KeyPress);
      this.txtLockDays.Leave += new EventHandler(this.strField_Leave);
      this.label41.AutoSize = true;
      this.label41.Font = new Font("Arial", 8.25f);
      this.label41.Location = new Point(7, 27);
      this.label41.Name = "label41";
      this.label41.Size = new Size(55, 14);
      this.label41.TabIndex = 55;
      this.label41.Text = "Lock Date";
      this.label40.AutoSize = true;
      this.label40.Font = new Font("Arial", 8.25f);
      this.label40.Location = new Point(7, 53);
      this.label40.Name = "label40";
      this.label40.Size = new Size(67, 14);
      this.label40.TabIndex = 57;
      this.label40.Text = "Lock # Days";
      this.pnlDeliveryType.Controls.Add((Control) this.comboBoxDeliveryType);
      this.pnlDeliveryType.Controls.Add((Control) this.label2);
      this.pnlDeliveryType.Controls.Add((Control) this.cBoxCmtType);
      this.pnlDeliveryType.Controls.Add((Control) this.labelCt);
      this.pnlDeliveryType.Dock = DockStyle.Top;
      this.pnlDeliveryType.Location = new Point(1, 80);
      this.pnlDeliveryType.Name = "pnlDeliveryType";
      this.pnlDeliveryType.Size = new Size(333, 50);
      this.pnlDeliveryType.TabIndex = 79;
      this.comboBoxDeliveryType.Font = new Font("Arial", 8.25f);
      this.comboBoxDeliveryType.Location = new Point(154, 25);
      this.comboBoxDeliveryType.Name = "comboBoxDeliveryType";
      this.comboBoxDeliveryType.ReadOnly = true;
      this.comboBoxDeliveryType.Size = new Size(169, 20);
      this.comboBoxDeliveryType.TabIndex = 78;
      this.comboBoxDeliveryType.Tag = (object) "3965";
      this.comboBoxDeliveryType.Leave += new EventHandler(this.strField_Leave);
      this.label2.AutoSize = true;
      this.label2.Font = new Font("Arial", 8.25f);
      this.label2.Location = new Point(7, 28);
      this.label2.Name = "label2";
      this.label2.Size = new Size(72, 14);
      this.label2.TabIndex = 77;
      this.label2.Text = "Delivery Type";
      this.cBoxCmtType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cBoxCmtType.FormattingEnabled = true;
      this.cBoxCmtType.Location = new Point(154, 0);
      this.cBoxCmtType.Name = "comboBoxCmtType";
      this.cBoxCmtType.Size = new Size(169, 21);
      this.cBoxCmtType.TabIndex = 78;
      this.cBoxCmtType.Tag = (object) "4187";
      this.cBoxCmtType.SelectedValueChanged += new EventHandler(this.cBoxCmtType_SelectedValueChanged);
      this.labelCt.AutoSize = true;
      this.labelCt.Font = new Font("Arial", 8.25f);
      this.labelCt.Location = new Point(7, 3);
      this.labelCt.Name = "labelCt";
      this.labelCt.Size = new Size(90, 14);
      this.labelCt.TabIndex = 77;
      this.labelCt.Text = "Commitment Type";
      this.pnlLoanProgram.Controls.Add((Control) this.textBoxLoanProgram);
      this.pnlLoanProgram.Controls.Add((Control) this.labelLoanProgram);
      this.pnlLoanProgram.Controls.Add((Control) this.label1);
      this.pnlLoanProgram.Controls.Add((Control) this.txtRateSheetID);
      this.pnlLoanProgram.Dock = DockStyle.Top;
      this.pnlLoanProgram.Location = new Point(1, 26);
      this.pnlLoanProgram.Name = "pnlLoanProgram";
      this.pnlLoanProgram.Size = new Size(333, 54);
      this.pnlLoanProgram.TabIndex = 155;
      this.textBoxLoanProgram.BackColor = Color.WhiteSmoke;
      this.textBoxLoanProgram.Font = new Font("Arial", 8.25f);
      this.textBoxLoanProgram.Location = new Point(154, 6);
      this.textBoxLoanProgram.Name = "textBoxLoanProgram";
      this.textBoxLoanProgram.ReadOnly = true;
      this.textBoxLoanProgram.Size = new Size(169, 20);
      this.textBoxLoanProgram.TabIndex = 4;
      this.textBoxLoanProgram.TabStop = false;
      this.textBoxLoanProgram.Tag = (object) "2866";
      this.labelLoanProgram.AutoSize = true;
      this.labelLoanProgram.Font = new Font("Arial", 8.25f);
      this.labelLoanProgram.Location = new Point(7, 9);
      this.labelLoanProgram.Name = "labelLoanProgram";
      this.labelLoanProgram.Size = new Size(74, 14);
      this.labelLoanProgram.TabIndex = 5;
      this.labelLoanProgram.Text = "Loan Program";
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Arial", 8.25f);
      this.label1.Location = new Point(7, 33);
      this.label1.Name = "label1";
      this.label1.Size = new Size(72, 14);
      this.label1.TabIndex = 75;
      this.label1.Text = "Rate Sheet ID";
      this.txtRateSheetID.Font = new Font("Arial", 8.25f);
      this.txtRateSheetID.Location = new Point(154, 30);
      this.txtRateSheetID.Name = "txtRateSheetID";
      this.txtRateSheetID.Size = new Size(169, 20);
      this.txtRateSheetID.TabIndex = 76;
      this.txtRateSheetID.Tag = (object) "2088";
      this.txtRateSheetID.TextAlign = HorizontalAlignment.Right;
      this.txtRateSheetID.Enter += new EventHandler(this.textField_Enter);
      this.txtRateSheetID.Leave += new EventHandler(this.strField_Leave);
      this.AutoScaleMode = AutoScaleMode.Inherit;
      this.BackColor = SystemColors.Control;
      this.Controls.Add((Control) this.gcPrice);
      this.Margin = new Padding(0);
      this.Name = nameof (WorstCasePricingColumn);
      this.Size = new Size(338, 3070);
      this.gcPrice.ResumeLayout(false);
      this.pnlComment.ResumeLayout(false);
      this.pnlComment.PerformLayout();
      this.pnlNetBM.ResumeLayout(false);
      this.pnlNetBM.PerformLayout();
      this.pnlBMExpand.ResumeLayout(false);
      this.pnlBMExpand.PerformLayout();
      this.pnlBM.ResumeLayout(false);
      this.pnlBM.PerformLayout();
      ((ISupportInitialize) this.picBMZoomOut).EndInit();
      ((ISupportInitialize) this.picBMZoomIn).EndInit();
      this.pnlNetPrice.ResumeLayout(false);
      this.pnlNetPrice.PerformLayout();
      this.pnlCpExpand.ResumeLayout(false);
      this.pnlCpExpand.PerformLayout();
      this.pnlCp.ResumeLayout(false);
      this.pnlCp.PerformLayout();
      ((ISupportInitialize) this.picCPZoomOut).EndInit();
      ((ISupportInitialize) this.picCPZoomIn).EndInit();
      this.pnlRlExpand.ResumeLayout(false);
      this.pnlRlExpand.PerformLayout();
      this.pnlRl.ResumeLayout(false);
      this.pnlRl.PerformLayout();
      ((ISupportInitialize) this.picRLZoomOut).EndInit();
      ((ISupportInitialize) this.picRLZoomIn).EndInit();
      this.pnlExExpand.ResumeLayout(false);
      this.pnlExExpand.PerformLayout();
      this.pnlEx.ResumeLayout(false);
      this.pnlEx.PerformLayout();
      ((ISupportInitialize) this.picEXZoomOut).EndInit();
      ((ISupportInitialize) this.picEXZoomIn).EndInit();
      this.pnlBPExtension.ResumeLayout(false);
      this.pnlBPExtension.PerformLayout();
      this.pnlBP.ResumeLayout(false);
      this.pnlBP.PerformLayout();
      ((ISupportInitialize) this.picBPZoomOut).EndInit();
      ((ISupportInitialize) this.picBPZoomIn).EndInit();
      this.pnlRateRequested.ResumeLayout(false);
      this.pnlRateRequested.PerformLayout();
      this.pnlBRExpand.ResumeLayout(false);
      this.pnlBRExpand.PerformLayout();
      this.pnlBR.ResumeLayout(false);
      this.pnlBR.PerformLayout();
      ((ISupportInitialize) this.picBRZoomOut).EndInit();
      ((ISupportInitialize) this.picBRZoomIn).EndInit();
      this.pnlLockInfo.ResumeLayout(false);
      this.pnlLockInfo.PerformLayout();
      this.pnlLockExtension.ResumeLayout(false);
      this.pnlLockExtension.PerformLayout();
      this.pnlDeliveryType.ResumeLayout(false);
      this.pnlDeliveryType.PerformLayout();
      this.pnlLoanProgram.ResumeLayout(false);
      this.pnlLoanProgram.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
