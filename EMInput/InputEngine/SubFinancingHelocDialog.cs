// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.SubFinancingHelocDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using EllieMae.Encompass.Forms;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class SubFinancingHelocDialog : System.Windows.Forms.Form
  {
    private const string className = "LoanPage";
    protected static string sw = Tracing.SwDataEngine;
    private LoanData loanData;
    private Sessions.Session session;
    private GenericFormInputHandler formInputHandler;
    private PiggybackFields piggybackFields;
    private FieldSource fieldSource;
    private IContainer components;
    private PictureBox pboxDownArrow;
    private PictureBox pboxAsterisk;
    private System.Windows.Forms.Button okBtn;
    private GroupContainer groupContainer1;
    private GridView gvVOLs;
    private GroupContainer groupContainer2;
    private System.Windows.Forms.Button btnRemoveLink;
    private System.Windows.Forms.Button btnAddNewHELOC;
    private System.Windows.Forms.Button btnAddClosedEnd;
    private System.Windows.Forms.Button btnLinkLoan;
    private GroupContainer groupContainerNFLCT;
    private System.Windows.Forms.TextBox txtLink1888;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.TextBox txtLink2;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox txtLink364;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox textBox7;
    private System.Windows.Forms.TextBox textBox6;
    private System.Windows.Forms.TextBox textBox5;
    private System.Windows.Forms.TextBox textBox4;
    private System.Windows.Forms.Label label4;
    private GroupContainer groupContainerTFSP;
    private GroupContainer groupContainerTRSP;
    private System.Windows.Forms.Label label9;
    private System.Windows.Forms.Label label8;
    private System.Windows.Forms.Label label14;
    private System.Windows.Forms.TextBox textBoxCASASRNX168;
    private System.Windows.Forms.TextBox textBoxCASASRNX167;
    private System.Windows.Forms.Label label13;
    private System.Windows.Forms.Label label12;
    private System.Windows.Forms.Label label11;
    private System.Windows.Forms.TextBox textBox428;
    private System.Windows.Forms.TextBox textBox427;
    private System.Windows.Forms.TextBox textBox19;
    private System.Windows.Forms.Label label10;
    private FieldLockButton fieldLockButton4;
    private FieldLockButton fieldLockButton3;
    private FieldLockButton fieldLockButton2;
    private FieldLockButton fieldLockButton1;
    private FieldLockButton lBtnFirst;
    private FieldLockButton fieldLockButton7;
    private FieldLockButton fieldLockButton8;
    private System.Windows.Forms.Label label16;
    private System.Windows.Forms.TextBox textBox1540;
    private System.Windows.Forms.TextBox textBox976;
    private System.Windows.Forms.Label label17;
    private System.Windows.Forms.Label label18;
    private System.Windows.Forms.TextBox textBox353;
    private System.Windows.Forms.Button btnShowVOL;
    private ToolTip toolTipField;
    private EMHelpLink emHelpLink1;
    private System.Windows.Forms.TextBox txtLink4494;
    private System.Windows.Forms.Label label23;
    private System.Windows.Forms.TextBox txtLink420;
    private System.Windows.Forms.Label label22;
    private FieldLockButton fieldLockButton6;
    private GroupContainer groupContainerVOAL;
    private System.Windows.Forms.TextBox textBox37;
    private System.Windows.Forms.TextBox textBox36;
    private System.Windows.Forms.TextBox textBox35;
    private System.Windows.Forms.TextBox textBox29;
    private System.Windows.Forms.TextBox textBox23;
    private System.Windows.Forms.TextBox textBox16;
    private System.Windows.Forms.TextBox textBox10;
    private System.Windows.Forms.TextBox textBox34;
    private System.Windows.Forms.TextBox textBox28;
    private System.Windows.Forms.TextBox textBox22;
    private System.Windows.Forms.TextBox textBox15;
    private System.Windows.Forms.TextBox textBox9;
    private System.Windows.Forms.TextBox textBox33;
    private System.Windows.Forms.Button btnShowVOAL;
    private System.Windows.Forms.TextBox textBox27;
    private System.Windows.Forms.TextBox textBox21;
    private System.Windows.Forms.TextBox textBox14;
    private System.Windows.Forms.TextBox textBox32;
    private System.Windows.Forms.TextBox textBox1;
    private System.Windows.Forms.TextBox textBox26;
    private System.Windows.Forms.Label label25;
    private System.Windows.Forms.Label label24;
    private System.Windows.Forms.Label label20;
    private System.Windows.Forms.TextBox textBox20;
    private System.Windows.Forms.Label label19;
    private System.Windows.Forms.TextBox textBox13;
    private System.Windows.Forms.TextBox textBox31;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.TextBox textBox25;
    private System.Windows.Forms.TextBox textBox8;
    private System.Windows.Forms.TextBox textBox18;
    private System.Windows.Forms.TextBox textBox30;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.TextBox textBox24;
    private System.Windows.Forms.TextBox textBox12;
    private System.Windows.Forms.TextBox textBox17;
    private System.Windows.Forms.Label label15;
    private System.Windows.Forms.TextBox textBox11;
    private System.Windows.Forms.TextBox textBox2;
    private System.Windows.Forms.TextBox textBox3;
    private System.Windows.Forms.Label label7;

    public SubFinancingHelocDialog(
      Sessions.Session session,
      LoanData loanData,
      FieldSource fieldSource)
    {
      this.session = session;
      this.loanData = loanData;
      this.fieldSource = fieldSource;
      this.InitializeComponent();
      this.formInputHandler = new GenericFormInputHandler((IHtmlInput) this.loanData, this.Controls, this.session);
      this.formInputHandler.SetFieldValuesToForm();
      this.formInputHandler.SetBusinessRules(new ResourceManager(typeof (SubFinancingHelocDialog)));
      this.formInputHandler.SetLockIconStatus();
      this.formInputHandler.SetFieldTip(this.toolTipField);
      for (int index = 0; index < this.formInputHandler.FieldControls.Count; ++index)
        this.formInputHandler.SetFieldEvents(this.formInputHandler.FieldControls[index]);
      this.formInputHandler.OnLockClicked += new EventHandler(this.lockButton_Clicked);
      this.btnShowVOAL.Visible = this.fieldSource == FieldSource.CurrentLoan;
      this.initForm();
    }

    private void initForm()
    {
      this.gvVOLs.Items.Clear();
      this.gvVOLs.BeginUpdate();
      int exlcudingAlimonyJobExp = this.loanData.GetNumberOfLiabilitesExlcudingAlimonyJobExp();
      for (int index = 1; index <= exlcudingAlimonyJobExp; ++index)
      {
        string str = "FL" + index.ToString("00");
        string field = this.loanData.GetField(str + "08");
        if ((!(field != "HELOC") || !(field != "MortgageLoan") || !(field != "Mortgage")) && !(this.loanData.GetField(str + "27") != "Y"))
          this.gvVOLs.Items.Add(new GVItem(this.loanData.GetField(str + "02"))
          {
            SubItems = {
              (object) this.loanData.GetField(str + "08"),
              (object) this.loanData.GetField(str + "32"),
              (object) this.loanData.GetField(str + "13"),
              (object) this.loanData.GetField(str + "16"),
              (object) this.loanData.GetField(str + "11"),
              (object) this.loanData.GetField(str + "31"),
              (object) this.loanData.GetField(str + "28"),
              (object) this.loanData.GetField(str + "29"),
              (object) this.loanData.GetField(str + "26"),
              (object) this.loanData.GetField(str + "18"),
              (object) this.loanData.GetField(str + "27"),
              (object) this.loanData.GetField(str + "30")
            }
          });
      }
      this.gvVOLs.EndUpdate();
      this.refreshContents();
    }

    private void lockButton_Clicked(object sender, EventArgs e)
    {
      FieldLockButton fieldLockButton = (FieldLockButton) sender;
      if (fieldLockButton.Tag == null)
        return;
      string str = fieldLockButton.Tag.ToString();
      if (str == string.Empty)
        return;
      if (fieldLockButton.Locked)
      {
        this.loanData.AddLock(str);
      }
      else
      {
        this.loanData.RemoveLock(str);
        this.loanData.Calculator.FormCalculation(str);
        this.refreshContents();
      }
    }

    private void button_Clicked(object sender, EventArgs e)
    {
      switch (((System.Windows.Forms.Control) sender).Name)
      {
        case "btnShowVOL":
          this.loadQuickEntryScreen("VOL", "VOLPanel", 688, 512, "");
          break;
        case "btnLinkLoan":
          if (this.piggybackFields == null)
            this.loadPiggybackSyncFields();
          PIGGYBACKInputHandler.LinkToLoan(this.session, (IHtmlInput) this.loanData, this.piggybackFields, (PIGGYBACKInputHandler) null);
          this.refreshContents();
          break;
        case "btnAddNewHELOC":
          if (this.piggybackFields == null)
            this.loadPiggybackSyncFields();
          if (!PIGGYBACKInputHandler.AddNewLinkedLoan(this.session, (IHtmlInput) this.loanData, this.piggybackFields, (PIGGYBACKInputHandler) null, true))
            break;
          this.loadPiggybackInputScreen();
          this.refreshContents();
          break;
        case "btnAddClosedEnd":
          if (this.piggybackFields == null)
            this.loadPiggybackSyncFields();
          if (!PIGGYBACKInputHandler.AddNewLinkedLoan(this.session, (IHtmlInput) this.loanData, this.piggybackFields, (PIGGYBACKInputHandler) null, false))
            break;
          this.loadPiggybackInputScreen();
          this.refreshContents();
          break;
        case "btnRemoveLink":
          PIGGYBACKInputHandler.RemoveLinkedLoan(this.session, (IHtmlInput) this.loanData, (InputHandlerBase) null, true);
          this.refreshContents();
          break;
        case "btnShowVOAL":
          this.loadQuickEntryScreen("VOAL", "VOALPanel", 688, 512, "VOALPanel");
          break;
      }
    }

    private void loadPiggybackInputScreen()
    {
      this.session.Application.GetService<ILoanEditor>().OpenForm("Piggyback Loans");
    }

    private void loadPiggybackSyncFields()
    {
      this.piggybackFields = this.session.LoanDataMgr == null ? (PiggybackFields) this.session.GetSystemSettings(typeof (PiggybackFields)) : this.session.LoanDataMgr.SystemConfiguration.PiggybackSyncFields;
    }

    private void staticField_Leave(object sender, EventArgs e)
    {
      if (!(sender is System.Windows.Forms.Control))
        return;
      this.handleFieldInControl((System.Windows.Forms.Control) sender, true);
    }

    private void handleFieldInControl(System.Windows.Forms.Control c, bool setFieldValue)
    {
      if (!(c is System.Windows.Forms.TextBox) || c.Tag == null)
        return;
      string id = c.Tag.ToString();
      if (id == "")
        return;
      System.Windows.Forms.TextBox textBox = (System.Windows.Forms.TextBox) c;
      if (setFieldValue)
      {
        if ((id == "CASASRN.X167" || id == "CASASRN.X168") && Decimal.TryParse(textBox.Text, out Decimal _))
          textBox.Text = Utils.ParseDecimal((object) textBox.Text, 0M).ToString("N2").Replace(",", "");
        this.loanData.SetField(id, textBox.Text);
      }
      else
      {
        textBox.Text = this.loanData.GetField(id);
        if (this.loanData.IsLocked(id))
          textBox.ReadOnly = true;
      }
      this.refreshContents();
    }

    private void loadQuickEntryScreen(
      string formTitle,
      string htmFile,
      int sizeWidth,
      int sizeHeight,
      string helpTag)
    {
      using (QuickEntryPopupDialog entryPopupDialog = new QuickEntryPopupDialog((IHtmlInput) this.loanData, formTitle, new InputFormInfo(htmFile, htmFile), sizeWidth, sizeHeight, FieldSource.CurrentLoan, helpTag, this.session))
      {
        int num = (int) entryPopupDialog.ShowDialog((IWin32Window) this.session.MainForm);
      }
      this.initForm();
    }

    private void form_SizeChanged(object sender, EventArgs e)
    {
      this.groupContainerTFSP.Left = this.groupContainerNFLCT.Left;
      this.groupContainerTFSP.Width = this.groupContainerNFLCT.Width / 2 - 5;
      this.groupContainerTRSP.Width = this.groupContainerTFSP.Width;
      this.groupContainerTRSP.Left = this.groupContainerTFSP.Left + this.groupContainerTFSP.Width + 10;
    }

    private void refreshContents()
    {
      if (this.fieldSource != FieldSource.CurrentLoan)
        this.btnLinkLoan.Visible = this.btnAddClosedEnd.Visible = this.btnAddNewHELOC.Visible = this.btnRemoveLink.Visible = this.btnShowVOL.Visible = false;
      if (this.loanData.LinkedData != null)
      {
        this.txtLink364.Text = this.loanData.LinkedData.GetField("364");
        this.txtLink2.Text = this.loanData.LinkedData.GetField("2");
        this.txtLink1888.Text = this.loanData.LinkedData.GetField("1888");
        string field = this.loanData.LinkedData.GetField("420");
        System.Windows.Forms.TextBox txtLink420 = this.txtLink420;
        string str;
        switch (field)
        {
          case "SecondLien":
            str = "Subordinate";
            break;
          case "FirstLien":
            str = "First";
            break;
          default:
            str = "";
            break;
        }
        txtLink420.Text = str;
        this.txtLink4494.Text = this.loanData.LinkedData.GetField("4494");
      }
      else
      {
        this.txtLink364.Text = "";
        this.txtLink2.Text = "";
        this.txtLink1888.Text = "";
        this.txtLink420.Text = "";
        this.txtLink4494.Text = "";
      }
      this.btnRemoveLink.Enabled = this.loanData.LinkedData != null;
      System.Windows.Forms.Button btnLinkLoan = this.btnLinkLoan;
      System.Windows.Forms.Button btnAddClosedEnd = this.btnAddClosedEnd;
      bool flag1;
      this.btnAddNewHELOC.Enabled = flag1 = this.loanData.LinkedData == null && this.loanData.GetField("19") != "ConstructionOnly" && this.loanData.GetField("19") != "ConstructionToPermanent";
      int num1;
      bool flag2 = (num1 = flag1 ? 1 : 0) != 0;
      btnAddClosedEnd.Enabled = num1 != 0;
      int num2 = flag2 ? 1 : 0;
      btnLinkLoan.Enabled = num2 != 0;
      if (this.loanData != null)
      {
        System.Windows.Forms.TextBox textBox6 = this.textBox6;
        double num3 = this.loanData.FltVal("4489");
        string str1 = num3.ToString();
        textBox6.Text = str1;
        System.Windows.Forms.TextBox textBox7 = this.textBox7;
        num3 = this.loanData.FltVal("4490");
        string str2 = num3.ToString();
        textBox7.Text = str2;
        this.ViewAdditionalLoans();
        try
        {
          PopupBusinessRules popupBusinessRules = new PopupBusinessRules(this.loanData, (ResourceManager) null, (System.Drawing.Image) null, (System.Drawing.Image) null, this.session);
          popupBusinessRules.SetBusinessRules(this.btnLinkLoan);
          popupBusinessRules.SetBusinessRules(this.btnAddClosedEnd);
          popupBusinessRules.SetBusinessRules(this.btnAddNewHELOC);
          popupBusinessRules.SetBusinessRules(this.btnRemoveLink);
        }
        catch (Exception ex)
        {
          Tracing.Log(SubFinancingHelocDialog.sw, TraceLevel.Error, "LoanPage", "Cannot set Button access right. Error: " + ex.Message);
        }
      }
      this.formInputHandler.SetFieldValuesToForm();
    }

    private void ViewAdditionalLoans()
    {
      if (this.loanData != null && this.loanData.GetField("1825") == "2020")
      {
        this.groupContainerNFLCT.Visible = false;
        this.groupContainerVOAL.Visible = true;
        this.groupContainerVOAL.Left = this.groupContainerNFLCT.Left;
        this.groupContainerVOAL.Top = this.groupContainerNFLCT.Top;
        this.groupContainerTFSP.Top = this.groupContainerVOAL.Top + this.groupContainerVOAL.Height + 8;
        this.groupContainerTRSP.Top = this.groupContainerTFSP.Top;
      }
      else
      {
        this.groupContainerNFLCT.Visible = true;
        this.groupContainerVOAL.Visible = false;
        this.ClientSize = new Size(884, 596);
      }
    }

    private void Help_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F1)
        return;
      JedHelp.ShowHelp(this.emHelpLink1.HelpTag);
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (SubFinancingHelocDialog));
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      GVColumn gvColumn4 = new GVColumn();
      GVColumn gvColumn5 = new GVColumn();
      GVColumn gvColumn6 = new GVColumn();
      GVColumn gvColumn7 = new GVColumn();
      GVColumn gvColumn8 = new GVColumn();
      GVColumn gvColumn9 = new GVColumn();
      GVColumn gvColumn10 = new GVColumn();
      GVColumn gvColumn11 = new GVColumn();
      GVColumn gvColumn12 = new GVColumn();
      GVColumn gvColumn13 = new GVColumn();
      this.okBtn = new System.Windows.Forms.Button();
      this.pboxDownArrow = new PictureBox();
      this.pboxAsterisk = new PictureBox();
      this.groupContainer1 = new GroupContainer();
      this.btnShowVOL = new System.Windows.Forms.Button();
      this.gvVOLs = new GridView();
      this.groupContainer2 = new GroupContainer();
      this.txtLink4494 = new System.Windows.Forms.TextBox();
      this.label23 = new System.Windows.Forms.Label();
      this.txtLink420 = new System.Windows.Forms.TextBox();
      this.label22 = new System.Windows.Forms.Label();
      this.txtLink1888 = new System.Windows.Forms.TextBox();
      this.label3 = new System.Windows.Forms.Label();
      this.txtLink2 = new System.Windows.Forms.TextBox();
      this.label2 = new System.Windows.Forms.Label();
      this.txtLink364 = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.btnRemoveLink = new System.Windows.Forms.Button();
      this.btnAddNewHELOC = new System.Windows.Forms.Button();
      this.btnAddClosedEnd = new System.Windows.Forms.Button();
      this.btnLinkLoan = new System.Windows.Forms.Button();
      this.groupContainerNFLCT = new GroupContainer();
      this.label9 = new System.Windows.Forms.Label();
      this.label8 = new System.Windows.Forms.Label();
      this.textBox7 = new System.Windows.Forms.TextBox();
      this.textBox6 = new System.Windows.Forms.TextBox();
      this.textBox5 = new System.Windows.Forms.TextBox();
      this.textBox4 = new System.Windows.Forms.TextBox();
      this.label4 = new System.Windows.Forms.Label();
      this.groupContainerTFSP = new GroupContainer();
      this.fieldLockButton4 = new FieldLockButton();
      this.fieldLockButton3 = new FieldLockButton();
      this.fieldLockButton2 = new FieldLockButton();
      this.fieldLockButton1 = new FieldLockButton();
      this.lBtnFirst = new FieldLockButton();
      this.label14 = new System.Windows.Forms.Label();
      this.textBoxCASASRNX168 = new System.Windows.Forms.TextBox();
      this.textBoxCASASRNX167 = new System.Windows.Forms.TextBox();
      this.label13 = new System.Windows.Forms.Label();
      this.label12 = new System.Windows.Forms.Label();
      this.label11 = new System.Windows.Forms.Label();
      this.textBox428 = new System.Windows.Forms.TextBox();
      this.textBox427 = new System.Windows.Forms.TextBox();
      this.textBox19 = new System.Windows.Forms.TextBox();
      this.label10 = new System.Windows.Forms.Label();
      this.groupContainerTRSP = new GroupContainer();
      this.fieldLockButton6 = new FieldLockButton();
      this.fieldLockButton7 = new FieldLockButton();
      this.fieldLockButton8 = new FieldLockButton();
      this.label16 = new System.Windows.Forms.Label();
      this.textBox1540 = new System.Windows.Forms.TextBox();
      this.textBox976 = new System.Windows.Forms.TextBox();
      this.label17 = new System.Windows.Forms.Label();
      this.label18 = new System.Windows.Forms.Label();
      this.textBox353 = new System.Windows.Forms.TextBox();
      this.toolTipField = new ToolTip(this.components);
      this.groupContainerVOAL = new GroupContainer();
      this.textBox37 = new System.Windows.Forms.TextBox();
      this.textBox36 = new System.Windows.Forms.TextBox();
      this.textBox35 = new System.Windows.Forms.TextBox();
      this.textBox29 = new System.Windows.Forms.TextBox();
      this.textBox23 = new System.Windows.Forms.TextBox();
      this.textBox16 = new System.Windows.Forms.TextBox();
      this.textBox10 = new System.Windows.Forms.TextBox();
      this.textBox34 = new System.Windows.Forms.TextBox();
      this.textBox28 = new System.Windows.Forms.TextBox();
      this.textBox22 = new System.Windows.Forms.TextBox();
      this.textBox15 = new System.Windows.Forms.TextBox();
      this.textBox9 = new System.Windows.Forms.TextBox();
      this.textBox33 = new System.Windows.Forms.TextBox();
      this.btnShowVOAL = new System.Windows.Forms.Button();
      this.textBox27 = new System.Windows.Forms.TextBox();
      this.textBox21 = new System.Windows.Forms.TextBox();
      this.textBox14 = new System.Windows.Forms.TextBox();
      this.textBox32 = new System.Windows.Forms.TextBox();
      this.textBox1 = new System.Windows.Forms.TextBox();
      this.textBox26 = new System.Windows.Forms.TextBox();
      this.label25 = new System.Windows.Forms.Label();
      this.label24 = new System.Windows.Forms.Label();
      this.label20 = new System.Windows.Forms.Label();
      this.textBox20 = new System.Windows.Forms.TextBox();
      this.label19 = new System.Windows.Forms.Label();
      this.textBox13 = new System.Windows.Forms.TextBox();
      this.textBox31 = new System.Windows.Forms.TextBox();
      this.label5 = new System.Windows.Forms.Label();
      this.textBox25 = new System.Windows.Forms.TextBox();
      this.textBox8 = new System.Windows.Forms.TextBox();
      this.textBox18 = new System.Windows.Forms.TextBox();
      this.textBox30 = new System.Windows.Forms.TextBox();
      this.label6 = new System.Windows.Forms.Label();
      this.textBox24 = new System.Windows.Forms.TextBox();
      this.textBox12 = new System.Windows.Forms.TextBox();
      this.textBox17 = new System.Windows.Forms.TextBox();
      this.label15 = new System.Windows.Forms.Label();
      this.textBox11 = new System.Windows.Forms.TextBox();
      this.textBox2 = new System.Windows.Forms.TextBox();
      this.textBox3 = new System.Windows.Forms.TextBox();
      this.label7 = new System.Windows.Forms.Label();
      this.emHelpLink1 = new EMHelpLink();
      ((ISupportInitialize) this.pboxDownArrow).BeginInit();
      ((ISupportInitialize) this.pboxAsterisk).BeginInit();
      this.groupContainer1.SuspendLayout();
      this.groupContainer2.SuspendLayout();
      this.groupContainerNFLCT.SuspendLayout();
      this.groupContainerTFSP.SuspendLayout();
      this.groupContainerTRSP.SuspendLayout();
      this.groupContainerVOAL.SuspendLayout();
      this.SuspendLayout();
      this.okBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.okBtn.DialogResult = DialogResult.OK;
      this.okBtn.Location = new Point(796, 690);
      this.okBtn.Name = "okBtn";
      this.okBtn.Size = new Size(75, 24);
      this.okBtn.TabIndex = 69;
      this.okBtn.Text = "&Close";
      this.pboxDownArrow.Image = (System.Drawing.Image) componentResourceManager.GetObject("pboxDownArrow.Image");
      this.pboxDownArrow.Location = new Point(344, 696);
      this.pboxDownArrow.Name = "pboxDownArrow";
      this.pboxDownArrow.Size = new Size(17, 17);
      this.pboxDownArrow.TabIndex = 72;
      this.pboxDownArrow.TabStop = false;
      this.pboxDownArrow.Visible = false;
      this.pboxAsterisk.Image = (System.Drawing.Image) componentResourceManager.GetObject("pboxAsterisk.Image");
      this.pboxAsterisk.Location = new Point(314, 696);
      this.pboxAsterisk.Name = "pboxAsterisk";
      this.pboxAsterisk.Size = new Size(24, 12);
      this.pboxAsterisk.TabIndex = 71;
      this.pboxAsterisk.TabStop = false;
      this.pboxAsterisk.Visible = false;
      this.groupContainer1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.groupContainer1.Controls.Add((System.Windows.Forms.Control) this.btnShowVOL);
      this.groupContainer1.Controls.Add((System.Windows.Forms.Control) this.gvVOLs);
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(12, 12);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(860, 174);
      this.groupContainer1.TabIndex = 73;
      this.groupContainer1.Text = "Mortgage and HELOC Liabilities";
      this.btnShowVOL.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnShowVOL.Location = new Point(757, 1);
      this.btnShowVOL.Name = "btnShowVOL";
      this.btnShowVOL.Size = new Size(99, 23);
      this.btnShowVOL.TabIndex = 77;
      this.btnShowVOL.Text = "Show all VOL";
      this.btnShowVOL.UseVisualStyleBackColor = true;
      this.btnShowVOL.Click += new EventHandler(this.button_Clicked);
      this.gvVOLs.AllowMultiselect = false;
      this.gvVOLs.BorderStyle = System.Windows.Forms.BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.SortMethod = GVSortMethod.None;
      gvColumn1.Text = "Creditor/ Company Name";
      gvColumn1.Width = 170;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.SortMethod = GVSortMethod.None;
      gvColumn2.Text = "Liability Type";
      gvColumn2.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn2.Width = 80;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Text = "Mortgage Type";
      gvColumn3.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn3.Width = 88;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column4";
      gvColumn4.SortMethod = GVSortMethod.None;
      gvColumn4.Text = "Balance";
      gvColumn4.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn4.Width = 90;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column5";
      gvColumn5.SortMethod = GVSortMethod.None;
      gvColumn5.Text = "Payoff";
      gvColumn5.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn5.Width = 90;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "Column6";
      gvColumn6.SortMethod = GVSortMethod.None;
      gvColumn6.Text = "Payment";
      gvColumn6.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn6.Width = 90;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "Column7";
      gvColumn7.SortMethod = GVSortMethod.None;
      gvColumn7.Text = "Credit Limit";
      gvColumn7.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn7.Width = 90;
      gvColumn8.ImageIndex = -1;
      gvColumn8.Name = "Column8";
      gvColumn8.SortMethod = GVSortMethod.None;
      gvColumn8.Text = "Cur. Lien Pos.";
      gvColumn8.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn8.Width = 80;
      gvColumn9.ImageIndex = -1;
      gvColumn9.Name = "Column9";
      gvColumn9.Text = "Prop. Lien Pos.";
      gvColumn9.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn9.Width = 80;
      gvColumn10.ImageIndex = -1;
      gvColumn10.Name = "Column11";
      gvColumn10.Text = "Resub.";
      gvColumn10.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn10.Width = 60;
      gvColumn11.ImageIndex = -1;
      gvColumn11.Name = "Column12";
      gvColumn11.Text = "Paid Off";
      gvColumn11.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn11.Width = 80;
      gvColumn12.ImageIndex = -1;
      gvColumn12.Name = "Column13";
      gvColumn12.Text = "Subj. Property";
      gvColumn12.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn12.Width = 100;
      gvColumn13.ImageIndex = -1;
      gvColumn13.Name = "Column14";
      gvColumn13.Text = "Bank Liability";
      gvColumn13.Width = 100;
      this.gvVOLs.Columns.AddRange(new GVColumn[13]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5,
        gvColumn6,
        gvColumn7,
        gvColumn8,
        gvColumn9,
        gvColumn10,
        gvColumn11,
        gvColumn12,
        gvColumn13
      });
      this.gvVOLs.Dock = DockStyle.Fill;
      this.gvVOLs.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvVOLs.Location = new Point(1, 26);
      this.gvVOLs.Name = "gvVOLs";
      this.gvVOLs.Size = new Size(858, 147);
      this.gvVOLs.SortOption = GVSortOption.None;
      this.gvVOLs.TabIndex = 21;
      this.groupContainer2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.groupContainer2.Controls.Add((System.Windows.Forms.Control) this.txtLink4494);
      this.groupContainer2.Controls.Add((System.Windows.Forms.Control) this.label23);
      this.groupContainer2.Controls.Add((System.Windows.Forms.Control) this.txtLink420);
      this.groupContainer2.Controls.Add((System.Windows.Forms.Control) this.label22);
      this.groupContainer2.Controls.Add((System.Windows.Forms.Control) this.txtLink1888);
      this.groupContainer2.Controls.Add((System.Windows.Forms.Control) this.label3);
      this.groupContainer2.Controls.Add((System.Windows.Forms.Control) this.txtLink2);
      this.groupContainer2.Controls.Add((System.Windows.Forms.Control) this.label2);
      this.groupContainer2.Controls.Add((System.Windows.Forms.Control) this.txtLink364);
      this.groupContainer2.Controls.Add((System.Windows.Forms.Control) this.label1);
      this.groupContainer2.Controls.Add((System.Windows.Forms.Control) this.btnRemoveLink);
      this.groupContainer2.Controls.Add((System.Windows.Forms.Control) this.btnAddNewHELOC);
      this.groupContainer2.Controls.Add((System.Windows.Forms.Control) this.btnAddClosedEnd);
      this.groupContainer2.Controls.Add((System.Windows.Forms.Control) this.btnLinkLoan);
      this.groupContainer2.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer2.Location = new Point(12, 191);
      this.groupContainer2.Name = "groupContainer2";
      this.groupContainer2.Size = new Size(860, 89);
      this.groupContainer2.TabIndex = 74;
      this.groupContainer2.Text = "Loan linked to current transaction";
      this.txtLink4494.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.txtLink4494.Location = new Point(632, 55);
      this.txtLink4494.Name = "txtLink4494";
      this.txtLink4494.ReadOnly = true;
      this.txtLink4494.Size = new Size(129, 20);
      this.txtLink4494.TabIndex = 88;
      this.txtLink4494.TextAlign = HorizontalAlignment.Center;
      this.label23.AutoSize = true;
      this.label23.Location = new Point(629, 39);
      this.label23.Name = "label23";
      this.label23.Size = new Size(67, 13);
      this.label23.TabIndex = 87;
      this.label23.Text = "Lien Position";
      this.txtLink420.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.txtLink420.Location = new Point(498, 55);
      this.txtLink420.Name = "txtLink420";
      this.txtLink420.ReadOnly = true;
      this.txtLink420.Size = new Size(129, 20);
      this.txtLink420.TabIndex = 86;
      this.txtLink420.TextAlign = HorizontalAlignment.Center;
      this.label22.AutoSize = true;
      this.label22.Location = new Point(495, 39);
      this.label22.Name = "label22";
      this.label22.Size = new Size(94, 13);
      this.label22.TabIndex = 85;
      this.label22.Text = "Lien Position Type";
      this.txtLink1888.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.txtLink1888.Location = new Point(363, 55);
      this.txtLink1888.Name = "txtLink1888";
      this.txtLink1888.ReadOnly = true;
      this.txtLink1888.Size = new Size(129, 20);
      this.txtLink1888.TabIndex = 84;
      this.txtLink1888.TextAlign = HorizontalAlignment.Right;
      this.label3.AutoSize = true;
      this.label3.Location = new Point(360, 39);
      this.label3.Name = "label3";
      this.label3.Size = new Size(59, 13);
      this.label3.TabIndex = 83;
      this.label3.Text = "Initial Draw";
      this.txtLink2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.txtLink2.Location = new Point(228, 55);
      this.txtLink2.Name = "txtLink2";
      this.txtLink2.ReadOnly = true;
      this.txtLink2.Size = new Size(129, 20);
      this.txtLink2.TabIndex = 82;
      this.txtLink2.TextAlign = HorizontalAlignment.Right;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(225, 39);
      this.label2.Name = "label2";
      this.label2.Size = new Size(97, 13);
      this.label2.TabIndex = 81;
      this.label2.Text = "Total Loan Amount";
      this.txtLink364.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.txtLink364.Location = new Point(9, 55);
      this.txtLink364.Name = "txtLink364";
      this.txtLink364.ReadOnly = true;
      this.txtLink364.Size = new Size(213, 20);
      this.txtLink364.TabIndex = 80;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(6, 39);
      this.label1.Name = "label1";
      this.label1.Size = new Size(71, 13);
      this.label1.TabIndex = 79;
      this.label1.Text = "Loan Number";
      this.btnRemoveLink.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnRemoveLink.Location = new Point(776, 1);
      this.btnRemoveLink.Name = "btnRemoveLink";
      this.btnRemoveLink.Size = new Size(80, 23);
      this.btnRemoveLink.TabIndex = 78;
      this.btnRemoveLink.Text = "Remove Link";
      this.btnRemoveLink.UseVisualStyleBackColor = true;
      this.btnRemoveLink.Click += new EventHandler(this.button_Clicked);
      this.btnAddNewHELOC.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAddNewHELOC.Location = new Point(674, 1);
      this.btnAddNewHELOC.Name = "btnAddNewHELOC";
      this.btnAddNewHELOC.Size = new Size(101, 23);
      this.btnAddNewHELOC.TabIndex = 77;
      this.btnAddNewHELOC.Text = "Add New HELOC";
      this.btnAddNewHELOC.UseVisualStyleBackColor = true;
      this.btnAddNewHELOC.Click += new EventHandler(this.button_Clicked);
      this.btnAddClosedEnd.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAddClosedEnd.Location = new Point(547, 1);
      this.btnAddClosedEnd.Name = "btnAddClosedEnd";
      this.btnAddClosedEnd.Size = new Size(125, 23);
      this.btnAddClosedEnd.TabIndex = 76;
      this.btnAddClosedEnd.Text = "Add New Closed End";
      this.btnAddClosedEnd.UseVisualStyleBackColor = true;
      this.btnAddClosedEnd.Click += new EventHandler(this.button_Clicked);
      this.btnLinkLoan.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnLinkLoan.Location = new Point(470, 1);
      this.btnLinkLoan.Name = "btnLinkLoan";
      this.btnLinkLoan.Size = new Size(75, 23);
      this.btnLinkLoan.TabIndex = 75;
      this.btnLinkLoan.Text = "Link to Loan";
      this.btnLinkLoan.UseVisualStyleBackColor = true;
      this.btnLinkLoan.Click += new EventHandler(this.button_Clicked);
      this.groupContainerNFLCT.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.groupContainerNFLCT.Controls.Add((System.Windows.Forms.Control) this.label9);
      this.groupContainerNFLCT.Controls.Add((System.Windows.Forms.Control) this.label8);
      this.groupContainerNFLCT.Controls.Add((System.Windows.Forms.Control) this.textBox7);
      this.groupContainerNFLCT.Controls.Add((System.Windows.Forms.Control) this.textBox6);
      this.groupContainerNFLCT.Controls.Add((System.Windows.Forms.Control) this.textBox5);
      this.groupContainerNFLCT.Controls.Add((System.Windows.Forms.Control) this.textBox4);
      this.groupContainerNFLCT.Controls.Add((System.Windows.Forms.Control) this.label4);
      this.groupContainerNFLCT.HeaderForeColor = SystemColors.ControlText;
      this.groupContainerNFLCT.Location = new Point(12, 287);
      this.groupContainerNFLCT.Name = "groupContainerNFLCT";
      this.groupContainerNFLCT.Size = new Size(860, 109);
      this.groupContainerNFLCT.TabIndex = 75;
      this.groupContainerNFLCT.Text = "New Financing not Linked to Current Transaction";
      this.label9.AutoSize = true;
      this.label9.Location = new Point(6, 82);
      this.label9.Name = "label9";
      this.label9.Size = new Size(158, 13);
      this.label9.TabIndex = 98;
      this.label9.Text = "New HELOC Draw / Credit Limit";
      this.label8.AutoSize = true;
      this.label8.Location = new Point(6, 60);
      this.label8.Name = "label8";
      this.label8.Size = new Size(194, 13);
      this.label8.TabIndex = 97;
      this.label8.Text = "New Closed End Subordinate Mortgage";
      this.textBox7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.textBox7.Location = new Point(363, 79);
      this.textBox7.Name = "textBox7";
      this.textBox7.Size = new Size(100, 20);
      this.textBox7.TabIndex = 10;
      this.textBox7.Tag = (object) "4490";
      this.textBox7.TextAlign = HorizontalAlignment.Right;
      this.textBox7.Leave += new EventHandler(this.staticField_Leave);
      this.textBox6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.textBox6.Location = new Point(226, 79);
      this.textBox6.Name = "textBox6";
      this.textBox6.Size = new Size(100, 20);
      this.textBox6.TabIndex = 9;
      this.textBox6.Tag = (object) "4489";
      this.textBox6.TextAlign = HorizontalAlignment.Right;
      this.textBox6.Leave += new EventHandler(this.staticField_Leave);
      this.textBox5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.textBox5.Location = new Point(226, 57);
      this.textBox5.Name = "textBox5";
      this.textBox5.Size = new Size(100, 20);
      this.textBox5.TabIndex = 5;
      this.textBox5.Tag = (object) "4488";
      this.textBox5.TextAlign = HorizontalAlignment.Right;
      this.textBox5.Leave += new EventHandler(this.staticField_Leave);
      this.textBox4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.textBox4.Location = new Point(226, 35);
      this.textBox4.Name = "textBox4";
      this.textBox4.Size = new Size(100, 20);
      this.textBox4.TabIndex = 1;
      this.textBox4.Tag = (object) "4487";
      this.textBox4.TextAlign = HorizontalAlignment.Right;
      this.textBox4.Leave += new EventHandler(this.staticField_Leave);
      this.label4.AutoSize = true;
      this.label4.Location = new Point(6, 38);
      this.label4.Name = "label4";
      this.label4.Size = new Size(171, 13);
      this.label4.TabIndex = 80;
      this.label4.Text = "New Closed End Primary Mortgage";
      this.groupContainerTFSP.Controls.Add((System.Windows.Forms.Control) this.fieldLockButton4);
      this.groupContainerTFSP.Controls.Add((System.Windows.Forms.Control) this.fieldLockButton3);
      this.groupContainerTFSP.Controls.Add((System.Windows.Forms.Control) this.fieldLockButton2);
      this.groupContainerTFSP.Controls.Add((System.Windows.Forms.Control) this.fieldLockButton1);
      this.groupContainerTFSP.Controls.Add((System.Windows.Forms.Control) this.lBtnFirst);
      this.groupContainerTFSP.Controls.Add((System.Windows.Forms.Control) this.label14);
      this.groupContainerTFSP.Controls.Add((System.Windows.Forms.Control) this.textBoxCASASRNX168);
      this.groupContainerTFSP.Controls.Add((System.Windows.Forms.Control) this.textBoxCASASRNX167);
      this.groupContainerTFSP.Controls.Add((System.Windows.Forms.Control) this.label13);
      this.groupContainerTFSP.Controls.Add((System.Windows.Forms.Control) this.label12);
      this.groupContainerTFSP.Controls.Add((System.Windows.Forms.Control) this.label11);
      this.groupContainerTFSP.Controls.Add((System.Windows.Forms.Control) this.textBox428);
      this.groupContainerTFSP.Controls.Add((System.Windows.Forms.Control) this.textBox427);
      this.groupContainerTFSP.Controls.Add((System.Windows.Forms.Control) this.textBox19);
      this.groupContainerTFSP.Controls.Add((System.Windows.Forms.Control) this.label10);
      this.groupContainerTFSP.HeaderForeColor = SystemColors.ControlText;
      this.groupContainerTFSP.Location = new Point(12, 402);
      this.groupContainerTFSP.Name = "groupContainerTFSP";
      this.groupContainerTFSP.Size = new Size(424, 150);
      this.groupContainerTFSP.TabIndex = 76;
      this.groupContainerTFSP.Text = "Total Financing for Subject Property";
      this.fieldLockButton4.Location = new Point(208, 123);
      this.fieldLockButton4.LockedStateToolTip = "Use Default Value";
      this.fieldLockButton4.MaximumSize = new Size(16, 17);
      this.fieldLockButton4.MinimumSize = new Size(16, 17);
      this.fieldLockButton4.Name = "fieldLockButton4";
      this.fieldLockButton4.Size = new Size(16, 17);
      this.fieldLockButton4.TabIndex = 99;
      this.fieldLockButton4.Tag = (object) "CASASRN.X168";
      this.fieldLockButton4.UnlockedStateToolTip = "Enter Data Manually";
      this.fieldLockButton3.Location = new Point(208, 101);
      this.fieldLockButton3.LockedStateToolTip = "Use Default Value";
      this.fieldLockButton3.MaximumSize = new Size(16, 17);
      this.fieldLockButton3.MinimumSize = new Size(16, 17);
      this.fieldLockButton3.Name = "fieldLockButton3";
      this.fieldLockButton3.Size = new Size(16, 17);
      this.fieldLockButton3.TabIndex = 98;
      this.fieldLockButton3.Tag = (object) "CASASRN.X167";
      this.fieldLockButton3.UnlockedStateToolTip = "Enter Data Manually";
      this.fieldLockButton2.Location = new Point(208, 79);
      this.fieldLockButton2.LockedStateToolTip = "Use Default Value";
      this.fieldLockButton2.MaximumSize = new Size(16, 17);
      this.fieldLockButton2.MinimumSize = new Size(16, 17);
      this.fieldLockButton2.Name = "fieldLockButton2";
      this.fieldLockButton2.Size = new Size(16, 17);
      this.fieldLockButton2.TabIndex = 97;
      this.fieldLockButton2.Tag = (object) "428";
      this.fieldLockButton2.UnlockedStateToolTip = "Enter Data Manually";
      this.fieldLockButton1.Location = new Point(208, 57);
      this.fieldLockButton1.LockedStateToolTip = "Use Default Value";
      this.fieldLockButton1.MaximumSize = new Size(16, 17);
      this.fieldLockButton1.MinimumSize = new Size(16, 17);
      this.fieldLockButton1.Name = "fieldLockButton1";
      this.fieldLockButton1.Size = new Size(16, 17);
      this.fieldLockButton1.TabIndex = 96;
      this.fieldLockButton1.Tag = (object) "427";
      this.fieldLockButton1.UnlockedStateToolTip = "Enter Data Manually";
      this.lBtnFirst.Location = new Point(208, 35);
      this.lBtnFirst.LockedStateToolTip = "Use Default Value";
      this.lBtnFirst.MaximumSize = new Size(16, 17);
      this.lBtnFirst.MinimumSize = new Size(16, 17);
      this.lBtnFirst.Name = "lBtnFirst";
      this.lBtnFirst.Size = new Size(16, 17);
      this.lBtnFirst.TabIndex = 95;
      this.lBtnFirst.Tag = (object) "26";
      this.lBtnFirst.UnlockedStateToolTip = "Enter Data Manually";
      this.label14.AutoSize = true;
      this.label14.Location = new Point(6, 123);
      this.label14.Name = "label14";
      this.label14.Size = new Size(181, 13);
      this.label14.TabIndex = 93;
      this.label14.Text = "Total Open End (HELOC) Credit Limit";
      this.textBoxCASASRNX168.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.textBoxCASASRNX168.Location = new Point(226, 120);
      this.textBoxCASASRNX168.Name = "textBoxCASASRNX168";
      this.textBoxCASASRNX168.Size = new Size(100, 20);
      this.textBoxCASASRNX168.TabIndex = 18;
      this.textBoxCASASRNX168.Tag = (object) "CASASRN.X168";
      this.textBoxCASASRNX168.TextAlign = HorizontalAlignment.Right;
      this.textBoxCASASRNX168.Leave += new EventHandler(this.staticField_Leave);
      this.textBoxCASASRNX167.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.textBoxCASASRNX167.Location = new Point(226, 98);
      this.textBoxCASASRNX167.Name = "textBoxCASASRNX167";
      this.textBoxCASASRNX167.Size = new Size(100, 20);
      this.textBoxCASASRNX167.TabIndex = 17;
      this.textBoxCASASRNX167.Tag = (object) "CASASRN.X167";
      this.textBoxCASASRNX167.TextAlign = HorizontalAlignment.Right;
      this.textBoxCASASRNX167.Leave += new EventHandler(this.staticField_Leave);
      this.label13.AutoSize = true;
      this.label13.Location = new Point(6, 101);
      this.label13.Name = "label13";
      this.label13.Size = new Size(199, 13);
      this.label13.TabIndex = 89;
      this.label13.Text = "Total Open End (HELOC) Draw Amounts";
      this.label12.AutoSize = true;
      this.label12.Location = new Point(6, 79);
      this.label12.Name = "label12";
      this.label12.Size = new Size(196, 13);
      this.label12.TabIndex = 88;
      this.label12.Text = "Closed End Subordinate Mortgage Total";
      this.label11.AutoSize = true;
      this.label11.Location = new Point(6, 57);
      this.label11.Name = "label11";
      this.label11.Size = new Size(173, 13);
      this.label11.TabIndex = 87;
      this.label11.Text = "Closed End Primary Mortgage Total";
      this.textBox428.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.textBox428.Location = new Point(226, 76);
      this.textBox428.Name = "textBox428";
      this.textBox428.Size = new Size(100, 20);
      this.textBox428.TabIndex = 16;
      this.textBox428.Tag = (object) "428";
      this.textBox428.TextAlign = HorizontalAlignment.Right;
      this.textBox428.Leave += new EventHandler(this.staticField_Leave);
      this.textBox427.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.textBox427.Location = new Point(226, 54);
      this.textBox427.Name = "textBox427";
      this.textBox427.Size = new Size(100, 20);
      this.textBox427.TabIndex = 15;
      this.textBox427.Tag = (object) "427";
      this.textBox427.TextAlign = HorizontalAlignment.Right;
      this.textBox427.Leave += new EventHandler(this.staticField_Leave);
      this.textBox19.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.textBox19.Location = new Point(226, 32);
      this.textBox19.Name = "textBox19";
      this.textBox19.Size = new Size(100, 20);
      this.textBox19.TabIndex = 14;
      this.textBox19.Tag = (object) "26";
      this.textBox19.TextAlign = HorizontalAlignment.Right;
      this.textBox19.Leave += new EventHandler(this.staticField_Leave);
      this.label10.AutoSize = true;
      this.label10.Location = new Point(6, 35);
      this.label10.Name = "label10";
      this.label10.Size = new Size(112, 13);
      this.label10.TabIndex = 81;
      this.label10.Text = "Existing Liens Paid Off";
      this.groupContainerTRSP.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.groupContainerTRSP.Controls.Add((System.Windows.Forms.Control) this.fieldLockButton6);
      this.groupContainerTRSP.Controls.Add((System.Windows.Forms.Control) this.fieldLockButton7);
      this.groupContainerTRSP.Controls.Add((System.Windows.Forms.Control) this.fieldLockButton8);
      this.groupContainerTRSP.Controls.Add((System.Windows.Forms.Control) this.label16);
      this.groupContainerTRSP.Controls.Add((System.Windows.Forms.Control) this.textBox1540);
      this.groupContainerTRSP.Controls.Add((System.Windows.Forms.Control) this.textBox976);
      this.groupContainerTRSP.Controls.Add((System.Windows.Forms.Control) this.label17);
      this.groupContainerTRSP.Controls.Add((System.Windows.Forms.Control) this.label18);
      this.groupContainerTRSP.Controls.Add((System.Windows.Forms.Control) this.textBox353);
      this.groupContainerTRSP.HeaderForeColor = SystemColors.ControlText;
      this.groupContainerTRSP.Location = new Point(447, 402);
      this.groupContainerTRSP.Name = "groupContainerTRSP";
      this.groupContainerTRSP.Size = new Size(424, 150);
      this.groupContainerTRSP.TabIndex = 77;
      this.groupContainerTRSP.Text = "Total Ratios for Subject Property";
      this.fieldLockButton6.Location = new Point(208, 79);
      this.fieldLockButton6.LockedStateToolTip = "Use Default Value";
      this.fieldLockButton6.MaximumSize = new Size(16, 17);
      this.fieldLockButton6.MinimumSize = new Size(16, 17);
      this.fieldLockButton6.Name = "fieldLockButton6";
      this.fieldLockButton6.Size = new Size(16, 17);
      this.fieldLockButton6.TabIndex = 114;
      this.fieldLockButton6.Tag = (object) "1540";
      this.fieldLockButton6.UnlockedStateToolTip = "Enter Data Manually";
      this.fieldLockButton7.Location = new Point(208, 57);
      this.fieldLockButton7.LockedStateToolTip = "Use Default Value";
      this.fieldLockButton7.MaximumSize = new Size(16, 17);
      this.fieldLockButton7.MinimumSize = new Size(16, 17);
      this.fieldLockButton7.Name = "fieldLockButton7";
      this.fieldLockButton7.Size = new Size(16, 17);
      this.fieldLockButton7.TabIndex = 113;
      this.fieldLockButton7.Tag = (object) "976";
      this.fieldLockButton7.UnlockedStateToolTip = "Enter Data Manually";
      this.fieldLockButton8.Location = new Point(208, 35);
      this.fieldLockButton8.LockedStateToolTip = "Use Default Value";
      this.fieldLockButton8.MaximumSize = new Size(16, 17);
      this.fieldLockButton8.MinimumSize = new Size(16, 17);
      this.fieldLockButton8.Name = "fieldLockButton8";
      this.fieldLockButton8.Size = new Size(16, 17);
      this.fieldLockButton8.TabIndex = 112;
      this.fieldLockButton8.Tag = (object) "353";
      this.fieldLockButton8.UnlockedStateToolTip = "Enter Data Manually";
      this.label16.AutoSize = true;
      this.label16.Location = new Point(6, 79);
      this.label16.Name = "label16";
      this.label16.Size = new Size(42, 13);
      this.label16.TabIndex = 109;
      this.label16.Text = "HCLTV";
      this.textBox1540.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.textBox1540.Location = new Point(226, 76);
      this.textBox1540.Name = "textBox1540";
      this.textBox1540.Size = new Size(100, 20);
      this.textBox1540.TabIndex = 25;
      this.textBox1540.Tag = (object) "1540";
      this.textBox1540.TextAlign = HorizontalAlignment.Right;
      this.textBox1540.Leave += new EventHandler(this.staticField_Leave);
      this.textBox976.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.textBox976.Location = new Point(226, 54);
      this.textBox976.Name = "textBox976";
      this.textBox976.Size = new Size(100, 20);
      this.textBox976.TabIndex = 24;
      this.textBox976.Tag = (object) "976";
      this.textBox976.TextAlign = HorizontalAlignment.Right;
      this.textBox976.Leave += new EventHandler(this.staticField_Leave);
      this.label17.AutoSize = true;
      this.label17.Location = new Point(6, 57);
      this.label17.Name = "label17";
      this.label17.Size = new Size(34, 13);
      this.label17.TabIndex = 106;
      this.label17.Text = "CLTV";
      this.label18.AutoSize = true;
      this.label18.Location = new Point(6, 35);
      this.label18.Name = "label18";
      this.label18.Size = new Size(27, 13);
      this.label18.TabIndex = 105;
      this.label18.Text = "LTV";
      this.textBox353.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.textBox353.Location = new Point(226, 32);
      this.textBox353.Name = "textBox353";
      this.textBox353.Size = new Size(100, 20);
      this.textBox353.TabIndex = 23;
      this.textBox353.Tag = (object) "353";
      this.textBox353.TextAlign = HorizontalAlignment.Right;
      this.textBox353.Leave += new EventHandler(this.staticField_Leave);
      this.groupContainerVOAL.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.groupContainerVOAL.Controls.Add((System.Windows.Forms.Control) this.textBox37);
      this.groupContainerVOAL.Controls.Add((System.Windows.Forms.Control) this.textBox36);
      this.groupContainerVOAL.Controls.Add((System.Windows.Forms.Control) this.textBox35);
      this.groupContainerVOAL.Controls.Add((System.Windows.Forms.Control) this.textBox29);
      this.groupContainerVOAL.Controls.Add((System.Windows.Forms.Control) this.textBox23);
      this.groupContainerVOAL.Controls.Add((System.Windows.Forms.Control) this.textBox16);
      this.groupContainerVOAL.Controls.Add((System.Windows.Forms.Control) this.textBox10);
      this.groupContainerVOAL.Controls.Add((System.Windows.Forms.Control) this.textBox34);
      this.groupContainerVOAL.Controls.Add((System.Windows.Forms.Control) this.textBox28);
      this.groupContainerVOAL.Controls.Add((System.Windows.Forms.Control) this.textBox22);
      this.groupContainerVOAL.Controls.Add((System.Windows.Forms.Control) this.textBox15);
      this.groupContainerVOAL.Controls.Add((System.Windows.Forms.Control) this.textBox9);
      this.groupContainerVOAL.Controls.Add((System.Windows.Forms.Control) this.textBox33);
      this.groupContainerVOAL.Controls.Add((System.Windows.Forms.Control) this.btnShowVOAL);
      this.groupContainerVOAL.Controls.Add((System.Windows.Forms.Control) this.textBox27);
      this.groupContainerVOAL.Controls.Add((System.Windows.Forms.Control) this.textBox21);
      this.groupContainerVOAL.Controls.Add((System.Windows.Forms.Control) this.textBox14);
      this.groupContainerVOAL.Controls.Add((System.Windows.Forms.Control) this.textBox32);
      this.groupContainerVOAL.Controls.Add((System.Windows.Forms.Control) this.textBox1);
      this.groupContainerVOAL.Controls.Add((System.Windows.Forms.Control) this.textBox26);
      this.groupContainerVOAL.Controls.Add((System.Windows.Forms.Control) this.label25);
      this.groupContainerVOAL.Controls.Add((System.Windows.Forms.Control) this.label24);
      this.groupContainerVOAL.Controls.Add((System.Windows.Forms.Control) this.label20);
      this.groupContainerVOAL.Controls.Add((System.Windows.Forms.Control) this.textBox20);
      this.groupContainerVOAL.Controls.Add((System.Windows.Forms.Control) this.label19);
      this.groupContainerVOAL.Controls.Add((System.Windows.Forms.Control) this.textBox13);
      this.groupContainerVOAL.Controls.Add((System.Windows.Forms.Control) this.textBox31);
      this.groupContainerVOAL.Controls.Add((System.Windows.Forms.Control) this.label5);
      this.groupContainerVOAL.Controls.Add((System.Windows.Forms.Control) this.textBox25);
      this.groupContainerVOAL.Controls.Add((System.Windows.Forms.Control) this.textBox8);
      this.groupContainerVOAL.Controls.Add((System.Windows.Forms.Control) this.textBox18);
      this.groupContainerVOAL.Controls.Add((System.Windows.Forms.Control) this.textBox30);
      this.groupContainerVOAL.Controls.Add((System.Windows.Forms.Control) this.label6);
      this.groupContainerVOAL.Controls.Add((System.Windows.Forms.Control) this.textBox24);
      this.groupContainerVOAL.Controls.Add((System.Windows.Forms.Control) this.textBox12);
      this.groupContainerVOAL.Controls.Add((System.Windows.Forms.Control) this.textBox17);
      this.groupContainerVOAL.Controls.Add((System.Windows.Forms.Control) this.label15);
      this.groupContainerVOAL.Controls.Add((System.Windows.Forms.Control) this.textBox11);
      this.groupContainerVOAL.Controls.Add((System.Windows.Forms.Control) this.textBox2);
      this.groupContainerVOAL.Controls.Add((System.Windows.Forms.Control) this.textBox3);
      this.groupContainerVOAL.Controls.Add((System.Windows.Forms.Control) this.label7);
      this.groupContainerVOAL.HeaderForeColor = SystemColors.ControlText;
      this.groupContainerVOAL.Location = new Point(12, 185);
      this.groupContainerVOAL.Name = "groupContainerVOAL";
      this.groupContainerVOAL.Size = new Size(860, 231);
      this.groupContainerVOAL.TabIndex = 101;
      this.groupContainerVOAL.Text = "Other New Mortgage Loans on the Property You are Buying or Refinancing";
      this.textBox37.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.textBox37.Location = new Point(724, 203);
      this.textBox37.Name = "textBox37";
      this.textBox37.ReadOnly = true;
      this.textBox37.Size = new Size(120, 20);
      this.textBox37.TabIndex = 88;
      this.textBox37.TabStop = false;
      this.textBox37.Tag = (object) "URLA.X230";
      this.textBox37.TextAlign = HorizontalAlignment.Right;
      this.textBox36.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.textBox36.Location = new Point(724, 181);
      this.textBox36.Name = "textBox36";
      this.textBox36.ReadOnly = true;
      this.textBox36.Size = new Size(120, 20);
      this.textBox36.TabIndex = 88;
      this.textBox36.TabStop = false;
      this.textBox36.Tag = (object) "URLA.X229";
      this.textBox36.TextAlign = HorizontalAlignment.Right;
      this.textBox35.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.textBox35.Location = new Point(724, 159);
      this.textBox35.Name = "textBox35";
      this.textBox35.ReadOnly = true;
      this.textBox35.Size = new Size(120, 20);
      this.textBox35.TabIndex = 88;
      this.textBox35.TabStop = false;
      this.textBox35.Tag = (object) "URLARAL0518";
      this.textBox35.TextAlign = HorizontalAlignment.Right;
      this.textBox29.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.textBox29.Location = new Point(724, 137);
      this.textBox29.Name = "textBox29";
      this.textBox29.ReadOnly = true;
      this.textBox29.Size = new Size(120, 20);
      this.textBox29.TabIndex = 88;
      this.textBox29.TabStop = false;
      this.textBox29.Tag = (object) "URLARAL0418";
      this.textBox29.TextAlign = HorizontalAlignment.Right;
      this.textBox23.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.textBox23.Location = new Point(724, 115);
      this.textBox23.Name = "textBox23";
      this.textBox23.ReadOnly = true;
      this.textBox23.Size = new Size(120, 20);
      this.textBox23.TabIndex = 88;
      this.textBox23.TabStop = false;
      this.textBox23.Tag = (object) "URLARAL0318";
      this.textBox23.TextAlign = HorizontalAlignment.Right;
      this.textBox16.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.textBox16.Location = new Point(724, 93);
      this.textBox16.Name = "textBox16";
      this.textBox16.ReadOnly = true;
      this.textBox16.Size = new Size(120, 20);
      this.textBox16.TabIndex = 88;
      this.textBox16.TabStop = false;
      this.textBox16.Tag = (object) "URLARAL0218";
      this.textBox16.TextAlign = HorizontalAlignment.Right;
      this.textBox10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.textBox10.Location = new Point(724, 71);
      this.textBox10.Name = "textBox10";
      this.textBox10.ReadOnly = true;
      this.textBox10.Size = new Size(120, 20);
      this.textBox10.TabIndex = 88;
      this.textBox10.TabStop = false;
      this.textBox10.Tag = (object) "URLARAL0118";
      this.textBox10.TextAlign = HorizontalAlignment.Right;
      this.textBox34.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.textBox34.Location = new Point(598, 159);
      this.textBox34.Name = "textBox34";
      this.textBox34.ReadOnly = true;
      this.textBox34.Size = new Size(120, 20);
      this.textBox34.TabIndex = 88;
      this.textBox34.TabStop = false;
      this.textBox34.Tag = (object) "URLARAL0522";
      this.textBox34.TextAlign = HorizontalAlignment.Right;
      this.textBox28.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.textBox28.Location = new Point(598, 137);
      this.textBox28.Name = "textBox28";
      this.textBox28.ReadOnly = true;
      this.textBox28.Size = new Size(120, 20);
      this.textBox28.TabIndex = 88;
      this.textBox28.TabStop = false;
      this.textBox28.Tag = (object) "URLARAL0422";
      this.textBox28.TextAlign = HorizontalAlignment.Right;
      this.textBox22.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.textBox22.Location = new Point(598, 115);
      this.textBox22.Name = "textBox22";
      this.textBox22.ReadOnly = true;
      this.textBox22.Size = new Size(120, 20);
      this.textBox22.TabIndex = 88;
      this.textBox22.TabStop = false;
      this.textBox22.Tag = (object) "URLARAL0322";
      this.textBox22.TextAlign = HorizontalAlignment.Right;
      this.textBox15.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.textBox15.Location = new Point(598, 93);
      this.textBox15.Name = "textBox15";
      this.textBox15.ReadOnly = true;
      this.textBox15.Size = new Size(120, 20);
      this.textBox15.TabIndex = 88;
      this.textBox15.TabStop = false;
      this.textBox15.Tag = (object) "URLARAL0222";
      this.textBox15.TextAlign = HorizontalAlignment.Right;
      this.textBox9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.textBox9.Location = new Point(598, 71);
      this.textBox9.Name = "textBox9";
      this.textBox9.ReadOnly = true;
      this.textBox9.Size = new Size(120, 20);
      this.textBox9.TabIndex = 88;
      this.textBox9.TabStop = false;
      this.textBox9.Tag = (object) "URLARAL0122";
      this.textBox9.TextAlign = HorizontalAlignment.Right;
      this.textBox33.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.textBox33.Location = new Point(9, 159);
      this.textBox33.Name = "textBox33";
      this.textBox33.ReadOnly = true;
      this.textBox33.Size = new Size((int) byte.MaxValue, 20);
      this.textBox33.TabIndex = 80;
      this.textBox33.TabStop = false;
      this.textBox33.Tag = (object) "URLARAL0502";
      this.btnShowVOAL.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnShowVOAL.Location = new Point(704, 1);
      this.btnShowVOAL.Name = "btnShowVOAL";
      this.btnShowVOAL.Size = new Size(152, 23);
      this.btnShowVOAL.TabIndex = 88;
      this.btnShowVOAL.Text = "View All Additional Loans";
      this.btnShowVOAL.UseVisualStyleBackColor = true;
      this.btnShowVOAL.Click += new EventHandler(this.button_Clicked);
      this.textBox27.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.textBox27.Location = new Point(9, 137);
      this.textBox27.Name = "textBox27";
      this.textBox27.ReadOnly = true;
      this.textBox27.Size = new Size((int) byte.MaxValue, 20);
      this.textBox27.TabIndex = 80;
      this.textBox27.TabStop = false;
      this.textBox27.Tag = (object) "URLARAL0402";
      this.textBox21.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.textBox21.Location = new Point(9, 115);
      this.textBox21.Name = "textBox21";
      this.textBox21.ReadOnly = true;
      this.textBox21.Size = new Size((int) byte.MaxValue, 20);
      this.textBox21.TabIndex = 80;
      this.textBox21.TabStop = false;
      this.textBox21.Tag = (object) "URLARAL0302";
      this.textBox14.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.textBox14.Location = new Point(9, 93);
      this.textBox14.Name = "textBox14";
      this.textBox14.ReadOnly = true;
      this.textBox14.Size = new Size((int) byte.MaxValue, 20);
      this.textBox14.TabIndex = 80;
      this.textBox14.TabStop = false;
      this.textBox14.Tag = (object) "URLARAL0202";
      this.textBox32.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.textBox32.Location = new Point(472, 159);
      this.textBox32.Name = "textBox32";
      this.textBox32.ReadOnly = true;
      this.textBox32.Size = new Size(120, 20);
      this.textBox32.TabIndex = 86;
      this.textBox32.TabStop = false;
      this.textBox32.Tag = (object) "URLARAL0521";
      this.textBox32.TextAlign = HorizontalAlignment.Right;
      this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.textBox1.Location = new Point(9, 71);
      this.textBox1.Name = "textBox1";
      this.textBox1.ReadOnly = true;
      this.textBox1.Size = new Size((int) byte.MaxValue, 20);
      this.textBox1.TabIndex = 80;
      this.textBox1.TabStop = false;
      this.textBox1.Tag = (object) "URLARAL0102";
      this.textBox26.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.textBox26.Location = new Point(472, 137);
      this.textBox26.Name = "textBox26";
      this.textBox26.ReadOnly = true;
      this.textBox26.Size = new Size(120, 20);
      this.textBox26.TabIndex = 86;
      this.textBox26.TabStop = false;
      this.textBox26.Tag = (object) "URLARAL0421";
      this.textBox26.TextAlign = HorizontalAlignment.Right;
      this.label25.AutoSize = true;
      this.label25.Location = new Point(567, 210);
      this.label25.Name = "label25";
      this.label25.Size = new Size(152, 13);
      this.label25.TabIndex = 87;
      this.label25.Text = "Total Applied to Down Payment";
      this.label24.AutoSize = true;
      this.label24.Location = new Point(567, 188);
      this.label24.Name = "label24";
      this.label24.Size = new Size(151, 13);
      this.label24.TabIndex = 87;
      this.label24.Text = "Total Additional Loans Amount";
      this.label20.AutoSize = true;
      this.label20.Location = new Point(719, 38);
      this.label20.Name = "label20";
      this.label20.Size = new Size(88, 13);
      this.label20.TabIndex = 87;
      this.label20.Text = "Monthly Payment";
      this.textBox20.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.textBox20.Location = new Point(472, 115);
      this.textBox20.Name = "textBox20";
      this.textBox20.ReadOnly = true;
      this.textBox20.Size = new Size(120, 20);
      this.textBox20.TabIndex = 86;
      this.textBox20.TabStop = false;
      this.textBox20.Tag = (object) "URLARAL0321";
      this.textBox20.TextAlign = HorizontalAlignment.Right;
      this.label19.Location = new Point(595, 38);
      this.label19.Name = "label19";
      this.label19.Size = new Size(80, 30);
      this.label19.TabIndex = 87;
      this.label19.Text = "Applied to Down Payment";
      this.textBox13.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.textBox13.Location = new Point(472, 93);
      this.textBox13.Name = "textBox13";
      this.textBox13.ReadOnly = true;
      this.textBox13.Size = new Size(120, 20);
      this.textBox13.TabIndex = 86;
      this.textBox13.TabStop = false;
      this.textBox13.Tag = (object) "URLARAL0221";
      this.textBox13.TextAlign = HorizontalAlignment.Right;
      this.textBox31.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.textBox31.Location = new Point(270, 159);
      this.textBox31.Name = "textBox31";
      this.textBox31.ReadOnly = true;
      this.textBox31.Size = new Size(70, 20);
      this.textBox31.TabIndex = 82;
      this.textBox31.TabStop = false;
      this.textBox31.Tag = (object) "URLARAL0517";
      this.textBox31.TextAlign = HorizontalAlignment.Center;
      this.label5.AutoSize = true;
      this.label5.Location = new Point(6, 38);
      this.label5.Name = "label5";
      this.label5.Size = new Size(74, 13);
      this.label5.TabIndex = 79;
      this.label5.Text = "Creditor Name";
      this.textBox25.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.textBox25.Location = new Point(270, 137);
      this.textBox25.Name = "textBox25";
      this.textBox25.ReadOnly = true;
      this.textBox25.Size = new Size(70, 20);
      this.textBox25.TabIndex = 82;
      this.textBox25.TabStop = false;
      this.textBox25.Tag = (object) "URLARAL0417";
      this.textBox25.TextAlign = HorizontalAlignment.Center;
      this.textBox8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.textBox8.Location = new Point(472, 71);
      this.textBox8.Name = "textBox8";
      this.textBox8.ReadOnly = true;
      this.textBox8.Size = new Size(120, 20);
      this.textBox8.TabIndex = 86;
      this.textBox8.TabStop = false;
      this.textBox8.Tag = (object) "URLARAL0121";
      this.textBox8.TextAlign = HorizontalAlignment.Right;
      this.textBox18.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.textBox18.Location = new Point(270, 115);
      this.textBox18.Name = "textBox18";
      this.textBox18.ReadOnly = true;
      this.textBox18.Size = new Size(70, 20);
      this.textBox18.TabIndex = 82;
      this.textBox18.TabStop = false;
      this.textBox18.Tag = (object) "URLARAL0317";
      this.textBox18.TextAlign = HorizontalAlignment.Center;
      this.textBox30.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.textBox30.Location = new Point(346, 159);
      this.textBox30.Name = "textBox30";
      this.textBox30.ReadOnly = true;
      this.textBox30.Size = new Size(120, 20);
      this.textBox30.TabIndex = 84;
      this.textBox30.TabStop = false;
      this.textBox30.Tag = (object) "URLARAL0520";
      this.textBox30.TextAlign = HorizontalAlignment.Right;
      this.label6.AutoSize = true;
      this.label6.Location = new Point(267, 38);
      this.label6.Name = "label6";
      this.label6.Size = new Size(67, 13);
      this.label6.TabIndex = 81;
      this.label6.Text = "Lien Position";
      this.textBox24.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.textBox24.Location = new Point(346, 137);
      this.textBox24.Name = "textBox24";
      this.textBox24.ReadOnly = true;
      this.textBox24.Size = new Size(120, 20);
      this.textBox24.TabIndex = 84;
      this.textBox24.TabStop = false;
      this.textBox24.Tag = (object) "URLARAL0420";
      this.textBox24.TextAlign = HorizontalAlignment.Right;
      this.textBox12.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.textBox12.Location = new Point(270, 93);
      this.textBox12.Name = "textBox12";
      this.textBox12.ReadOnly = true;
      this.textBox12.Size = new Size(70, 20);
      this.textBox12.TabIndex = 82;
      this.textBox12.TabStop = false;
      this.textBox12.Tag = (object) "URLARAL0217";
      this.textBox12.TextAlign = HorizontalAlignment.Center;
      this.textBox17.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.textBox17.Location = new Point(346, 115);
      this.textBox17.Name = "textBox17";
      this.textBox17.ReadOnly = true;
      this.textBox17.Size = new Size(120, 20);
      this.textBox17.TabIndex = 84;
      this.textBox17.TabStop = false;
      this.textBox17.Tag = (object) "URLARAL0320";
      this.textBox17.TextAlign = HorizontalAlignment.Right;
      this.label15.AutoSize = true;
      this.label15.Location = new Point(469, 38);
      this.label15.Name = "label15";
      this.label15.Size = new Size(98, 13);
      this.label15.TabIndex = 85;
      this.label15.Text = "HELOC Initial Draw";
      this.textBox11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.textBox11.Location = new Point(346, 93);
      this.textBox11.Name = "textBox11";
      this.textBox11.ReadOnly = true;
      this.textBox11.Size = new Size(120, 20);
      this.textBox11.TabIndex = 84;
      this.textBox11.TabStop = false;
      this.textBox11.Tag = (object) "URLARAL0220";
      this.textBox11.TextAlign = HorizontalAlignment.Right;
      this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.textBox2.Location = new Point(270, 71);
      this.textBox2.Name = "textBox2";
      this.textBox2.ReadOnly = true;
      this.textBox2.Size = new Size(70, 20);
      this.textBox2.TabIndex = 82;
      this.textBox2.TabStop = false;
      this.textBox2.Tag = (object) "URLARAL0117";
      this.textBox2.TextAlign = HorizontalAlignment.Center;
      this.textBox3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.textBox3.Location = new Point(346, 71);
      this.textBox3.Name = "textBox3";
      this.textBox3.ReadOnly = true;
      this.textBox3.Size = new Size(120, 20);
      this.textBox3.TabIndex = 84;
      this.textBox3.TabStop = false;
      this.textBox3.Tag = (object) "URLARAL0120";
      this.textBox3.TextAlign = HorizontalAlignment.Right;
      this.label7.Location = new Point(348, 38);
      this.label7.Name = "label7";
      this.label7.Size = new Size(109, 32);
      this.label7.TabIndex = 83;
      this.label7.Text = "Loan Amount/ HELOC Credit Limit";
      this.emHelpLink1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.emHelpLink1.BackColor = Color.Transparent;
      this.emHelpLink1.Cursor = Cursors.Hand;
      this.emHelpLink1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.emHelpLink1.HelpTag = "SubMortgage";
      this.emHelpLink1.Location = new Point(12, 690);
      this.emHelpLink1.Name = "emHelpLink1";
      this.emHelpLink1.Size = new Size(90, 16);
      this.emHelpLink1.TabIndex = 78;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(884, 720);
      this.Controls.Add((System.Windows.Forms.Control) this.okBtn);
      this.Controls.Add((System.Windows.Forms.Control) this.groupContainerVOAL);
      this.Controls.Add((System.Windows.Forms.Control) this.emHelpLink1);
      this.Controls.Add((System.Windows.Forms.Control) this.groupContainerTRSP);
      this.Controls.Add((System.Windows.Forms.Control) this.groupContainerTFSP);
      this.Controls.Add((System.Windows.Forms.Control) this.groupContainerNFLCT);
      this.Controls.Add((System.Windows.Forms.Control) this.groupContainer2);
      this.Controls.Add((System.Windows.Forms.Control) this.groupContainer1);
      this.Controls.Add((System.Windows.Forms.Control) this.pboxDownArrow);
      this.Controls.Add((System.Windows.Forms.Control) this.pboxAsterisk);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (SubFinancingHelocDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Subordinate Mortgage Loan Amounts";
      this.SizeChanged += new EventHandler(this.form_SizeChanged);
      this.KeyDown += new KeyEventHandler(this.Help_KeyDown);
      ((ISupportInitialize) this.pboxDownArrow).EndInit();
      ((ISupportInitialize) this.pboxAsterisk).EndInit();
      this.groupContainer1.ResumeLayout(false);
      this.groupContainer2.ResumeLayout(false);
      this.groupContainer2.PerformLayout();
      this.groupContainerNFLCT.ResumeLayout(false);
      this.groupContainerNFLCT.PerformLayout();
      this.groupContainerTFSP.ResumeLayout(false);
      this.groupContainerTFSP.PerformLayout();
      this.groupContainerTRSP.ResumeLayout(false);
      this.groupContainerTRSP.PerformLayout();
      this.groupContainerVOAL.ResumeLayout(false);
      this.groupContainerVOAL.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
