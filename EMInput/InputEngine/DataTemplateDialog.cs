// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.DataTemplateDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.CalculationEngine;
using EllieMae.EMLite.ClientCommon;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.DocEngine;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup;
using EllieMae.EMLite.UI;
using EllieMae.Encompass.AsmResolver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class DataTemplateDialog : Form, IOnlineHelpTarget, IHelp, ILoanEditor
  {
    private const string className = "DataTemplateDialog";
    private static string sw = Tracing.SwInputEngine;
    private System.ComponentModel.Container components;
    private InputFormList inputList;
    private DataTemplate dataTemplate;
    private InputFormInfo currForm;
    private LoanScreen freeScreen;
    private TextBox nameTxt;
    private Label label1;
    private Panel bottomPanel;
    private Button cancelBtn;
    private Button saveBtn;
    private Panel rightPanel;
    private Panel namePanel;
    private Panel middlePanel;
    private Panel formPanel;
    private Panel webPanel;
    private Label label2;
    private TextBox descTxt;
    private CollapsibleSplitter splitter1;
    private EMFormMenu emFormMenuBox;
    private EMHelpLink emHelpLink1;
    private LoanData dataTempLoan;
    private bool isPublic;
    private CheckBox chkIgnore;
    private Panel panel1;
    private GradientPanel gradientPanel1;
    private CheckBox chkAlphaOrder;
    private bool ignoreBR;
    private ComboBox cboRESPAVersion;
    private Label label3;
    private Sessions.Session session;
    private const string DT_DELIMITER = "|";
    private const string DT_DELIMITER_1 = "|1";
    private const string DT_DELIMITER_0 = "|0";
    private const string BADCHARS = "/:*?<>|.";

    public DataTemplateDialog(
      Sessions.Session session,
      DataTemplate dataTemplate,
      bool readOnly,
      bool isPublic)
      : this(session, dataTemplate, isPublic)
    {
      if (readOnly)
      {
        this.nameTxt.ReadOnly = true;
        this.descTxt.ReadOnly = true;
        this.saveBtn.Enabled = false;
        this.cancelBtn.Text = "Close";
        this.AcceptButton = (IButtonControl) this.cancelBtn;
        this.Text = "Data Template";
        this.chkIgnore.Enabled = false;
      }
      this.chkIgnore.Visible = false;
      if (!this.isPublic || EnableDisableSetting.Enabled != (EnableDisableSetting) this.session.ServerManager.GetServerSetting("Components.DisplayBusinessRuleOption"))
        return;
      this.chkIgnore.Visible = true;
      if (this.dataTemplate == null || !this.dataTemplate.IgnoreBusinessRules)
        return;
      this.chkIgnore.Checked = true;
      this.ignoreBR = true;
    }

    public DataTemplateDialog(Sessions.Session session, DataTemplate dataTemplate, bool isPublic)
    {
      this.session = session;
      this.isPublic = isPublic;
      this.dataTemplate = dataTemplate;
      this.InitializeComponent();
      this.emHelpLink1.AssignSession(this.session);
      if (this.dataTemplate != null)
      {
        this.nameTxt.Text = this.dataTemplate.TemplateName;
        this.descTxt.Text = this.dataTemplate.Description;
        this.initializeRESPAVersion(dataTemplate.RESPAVersion);
      }
      this.inputList = new InputFormList(this.session.SessionObjects);
      InputFormInfo[] inputFormInfoArray = (InputFormInfo[]) null;
      try
      {
        inputFormInfoArray = this.inputList.GetFormList("DataTemplate", true);
      }
      catch (Exception ex)
      {
        Tracing.Log(DataTemplateDialog.sw, TraceLevel.Error, nameof (DataTemplateDialog), "FormListDialog: Can't access Form List. Error: " + ex.Message);
      }
      if (inputFormInfoArray.Length == 0)
        Tracing.Log(DataTemplateDialog.sw, TraceLevel.Error, nameof (DataTemplateDialog), "exception in loading Data Template Form List in InputFormList.");
      this.emFormMenuBox.ClearFormList();
      bool flag = false;
      List<string> stringList1 = new List<string>();
      List<string> stringList2 = new List<string>((IEnumerable<string>) SecurityUtil.BankerOnlyTools(this.session.MainScreen.IsUnderwriterSummaryAccessibleForBroker));
      string clientId = this.session.CompanyInfo.ClientID;
      foreach (InputFormInfo inputFormInfo in inputFormInfoArray)
      {
        if ((this.session.EncompassEdition != EncompassEdition.Broker || !stringList2.Contains(inputFormInfo.Name)) && (this.session.StartupInfo.AllowURLA2020 || !ShipInDarkValidation.IsURLA2020Form(inputFormInfo.FormID)) && inputFormInfo != (InputFormInfo) null && (!(inputFormInfo.FormID == "ULDD") || this.session.MainScreen.IsClientEnabledToExportFNMFRE) && !(inputFormInfo.Name == "HELOC Management"))
        {
          if (inputFormInfo.Name != "State-Specific Disclosure Information")
          {
            stringList1.Add(inputFormInfo.Name);
            if (inputFormInfo.Name == "MLDS - CA GFE")
            {
              stringList1.Add("MLDS - CA GFE Page 4");
              stringList1.Add("MLDS - CA GFE (RE882)");
            }
            if (inputFormInfo.Name == "HMDA Information")
            {
              stringList1.Add("2018 HMDA Originated/Adverse Action Loans");
              stringList1.Add("2018 HMDA Purchased Loans");
              stringList1.Add("Repurchased Loans");
            }
          }
          else
            flag = true;
        }
      }
      if (flag)
      {
        string[] strArray = LoanScreen.STATESPECIFICFORMS.Split(',');
        for (int index = 0; index < strArray.Length; ++index)
        {
          if (!(strArray[index] == string.Empty))
            stringList1.Add("State Specific Information - " + Utils.GetFullStateName(strArray[index]));
        }
      }
      int num = stringList1.Count;
      for (int index = 0; index < stringList1.Count; ++index)
      {
        if (stringList1[index].StartsWith("------") || stringList1[index].StartsWith("State Specific Information -"))
        {
          num = index;
          break;
        }
      }
      this.emFormMenuBox.LoadFormList(stringList1.ToArray());
      if (this.session.EncompassEdition != EncompassEdition.Banker)
      {
        this.emFormMenuBox.RemoveForm("Shipping Detail");
        this.emFormMenuBox.RemoveForm("Funding Worksheet");
      }
      this.emFormMenuBox.RefreshFormList();
      if (stringList1 != null && stringList1.Count > 0)
        this.emFormMenuBox.SelectedIndex = 0;
      this.chkIgnore.Visible = false;
      if (this.isPublic && EnableDisableSetting.Enabled == (EnableDisableSetting) this.session.ServerManager.GetServerSetting("Components.DisplayBusinessRuleOption"))
      {
        this.chkIgnore.Visible = true;
        if (this.dataTemplate != null && this.dataTemplate.IgnoreBusinessRules)
        {
          this.chkIgnore.Checked = true;
          this.ignoreBR = true;
        }
      }
      this.chkIgnore.CheckedChanged += new EventHandler(this.chkIgnore_CheckedChanged);
      this.cboRESPAVersion.SelectedIndexChanged += new EventHandler(this.cboRESPAVersion_SelectedIndexChanged);
      this.descTxt.Focus();
    }

    private void initializeRESPAVersion(string respaVersion)
    {
      switch (respaVersion)
      {
        case "2009":
          this.cboRESPAVersion.SelectedIndex = 1;
          break;
        case "2010":
          this.cboRESPAVersion.SelectedIndex = 2;
          break;
        case "2015":
          this.cboRESPAVersion.SelectedIndex = 3;
          break;
        default:
          this.cboRESPAVersion.SelectedIndex = 0;
          break;
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        this.Unload();
        if (this.components != null)
          this.components.Dispose();
      }
      base.Dispose(disposing);
    }

    public void Unload()
    {
      this.emFormMenuBox.SelectedIndexChanged -= new EventHandler(this.emFormMenuList_SelectedIndexChanged);
      this.chkAlphaOrder.Click -= new EventHandler(this.chkAlphaOrder_Click);
      this.saveBtn.Click -= new EventHandler(this.saveBtn_Click);
      this.nameTxt.KeyPress -= new KeyPressEventHandler(this.keypress);
      this.Closing -= new CancelEventHandler(this.DataTemplateDialog_Closing);
      this.chkIgnore.CheckedChanged -= new EventHandler(this.chkIgnore_CheckedChanged);
      this.cboRESPAVersion.SelectedIndexChanged -= new EventHandler(this.cboRESPAVersion_SelectedIndexChanged);
    }

    public void ShowAUSTrackingTool()
    {
    }

    private void InitializeComponent()
    {
      this.rightPanel = new Panel();
      this.middlePanel = new Panel();
      this.webPanel = new Panel();
      this.splitter1 = new CollapsibleSplitter();
      this.formPanel = new Panel();
      this.panel1 = new Panel();
      this.emFormMenuBox = new EMFormMenu();
      this.gradientPanel1 = new GradientPanel();
      this.chkAlphaOrder = new CheckBox();
      this.bottomPanel = new Panel();
      this.chkIgnore = new CheckBox();
      this.emHelpLink1 = new EMHelpLink();
      this.cancelBtn = new Button();
      this.saveBtn = new Button();
      this.namePanel = new Panel();
      this.cboRESPAVersion = new ComboBox();
      this.label3 = new Label();
      this.label2 = new Label();
      this.descTxt = new TextBox();
      this.nameTxt = new TextBox();
      this.label1 = new Label();
      this.rightPanel.SuspendLayout();
      this.middlePanel.SuspendLayout();
      this.formPanel.SuspendLayout();
      this.panel1.SuspendLayout();
      this.gradientPanel1.SuspendLayout();
      this.bottomPanel.SuspendLayout();
      this.namePanel.SuspendLayout();
      this.SuspendLayout();
      this.rightPanel.Controls.Add((Control) this.middlePanel);
      this.rightPanel.Controls.Add((Control) this.bottomPanel);
      this.rightPanel.Controls.Add((Control) this.namePanel);
      this.rightPanel.Dock = DockStyle.Fill;
      this.rightPanel.Location = new Point(0, 0);
      this.rightPanel.Name = "rightPanel";
      this.rightPanel.Size = new Size(926, 624);
      this.rightPanel.TabIndex = 11;
      this.middlePanel.Controls.Add((Control) this.webPanel);
      this.middlePanel.Controls.Add((Control) this.splitter1);
      this.middlePanel.Controls.Add((Control) this.formPanel);
      this.middlePanel.Dock = DockStyle.Fill;
      this.middlePanel.Location = new Point(0, 130);
      this.middlePanel.Name = "middlePanel";
      this.middlePanel.Size = new Size(926, 446);
      this.middlePanel.TabIndex = 12;
      this.webPanel.Dock = DockStyle.Fill;
      this.webPanel.Location = new Point(203, 0);
      this.webPanel.Name = "webPanel";
      this.webPanel.Size = new Size(723, 446);
      this.webPanel.TabIndex = 3;
      this.splitter1.AnimationDelay = 20;
      this.splitter1.AnimationStep = 20;
      this.splitter1.BorderStyle3D = Border3DStyle.Raised;
      this.splitter1.ControlToHide = (Control) this.formPanel;
      this.splitter1.Cursor = Cursors.Default;
      this.splitter1.ExpandParentForm = false;
      this.splitter1.Location = new Point(196, 0);
      this.splitter1.Name = "splitter1";
      this.splitter1.TabIndex = 0;
      this.splitter1.TabStop = false;
      this.splitter1.UseAnimations = false;
      this.splitter1.VisualStyle = VisualStyles.Encompass;
      this.formPanel.Controls.Add((Control) this.panel1);
      this.formPanel.Controls.Add((Control) this.gradientPanel1);
      this.formPanel.Dock = DockStyle.Left;
      this.formPanel.Location = new Point(0, 0);
      this.formPanel.Name = "formPanel";
      this.formPanel.Size = new Size(196, 446);
      this.formPanel.TabIndex = 0;
      this.panel1.Controls.Add((Control) this.emFormMenuBox);
      this.panel1.Dock = DockStyle.Fill;
      this.panel1.Location = new Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(196, 422);
      this.panel1.TabIndex = 1;
      this.emFormMenuBox.AlternatingColors = false;
      this.emFormMenuBox.Dock = DockStyle.Fill;
      this.emFormMenuBox.GridLines = false;
      this.emFormMenuBox.Location = new Point(0, 0);
      this.emFormMenuBox.Name = "emFormMenuBox";
      this.emFormMenuBox.Size = new Size(196, 422);
      this.emFormMenuBox.TabIndex = 3;
      this.emFormMenuBox.SelectedIndexChanged += new EventHandler(this.emFormMenuList_SelectedIndexChanged);
      this.gradientPanel1.Controls.Add((Control) this.chkAlphaOrder);
      this.gradientPanel1.Dock = DockStyle.Bottom;
      this.gradientPanel1.GradientColor1 = Color.White;
      this.gradientPanel1.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel1.Location = new Point(0, 422);
      this.gradientPanel1.Name = "gradientPanel1";
      this.gradientPanel1.Size = new Size(196, 24);
      this.gradientPanel1.TabIndex = 0;
      this.chkAlphaOrder.AutoSize = true;
      this.chkAlphaOrder.BackColor = Color.Transparent;
      this.chkAlphaOrder.Location = new Point(4, 4);
      this.chkAlphaOrder.Name = "chkAlphaOrder";
      this.chkAlphaOrder.Size = new Size(123, 17);
      this.chkAlphaOrder.TabIndex = 4;
      this.chkAlphaOrder.Text = "Show in Alpha Order";
      this.chkAlphaOrder.UseVisualStyleBackColor = false;
      this.chkAlphaOrder.Click += new EventHandler(this.chkAlphaOrder_Click);
      this.bottomPanel.Controls.Add((Control) this.chkIgnore);
      this.bottomPanel.Controls.Add((Control) this.emHelpLink1);
      this.bottomPanel.Controls.Add((Control) this.cancelBtn);
      this.bottomPanel.Controls.Add((Control) this.saveBtn);
      this.bottomPanel.Dock = DockStyle.Bottom;
      this.bottomPanel.Location = new Point(0, 576);
      this.bottomPanel.Name = "bottomPanel";
      this.bottomPanel.Size = new Size(926, 48);
      this.bottomPanel.TabIndex = 11;
      this.chkIgnore.AutoSize = true;
      this.chkIgnore.Location = new Point(204, 16);
      this.chkIgnore.Name = "chkIgnore";
      this.chkIgnore.Size = new Size(384, 17);
      this.chkIgnore.TabIndex = 8;
      this.chkIgnore.Text = "Template data will ignore business rules and fee management persona rights";
      this.chkIgnore.UseVisualStyleBackColor = true;
      this.emHelpLink1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.emHelpLink1.BackColor = Color.Transparent;
      this.emHelpLink1.Cursor = Cursors.Hand;
      this.emHelpLink1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.emHelpLink1.HelpTag = "Data Templates";
      this.emHelpLink1.Location = new Point(8, 19);
      this.emHelpLink1.Name = "emHelpLink1";
      this.emHelpLink1.Size = new Size(90, 16);
      this.emHelpLink1.TabIndex = 7;
      this.cancelBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(838, 15);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 24);
      this.cancelBtn.TabIndex = 6;
      this.cancelBtn.Text = "&Cancel";
      this.saveBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.saveBtn.Location = new Point(758, 15);
      this.saveBtn.Name = "saveBtn";
      this.saveBtn.Size = new Size(75, 24);
      this.saveBtn.TabIndex = 5;
      this.saveBtn.Text = "&Save";
      this.saveBtn.Click += new EventHandler(this.saveBtn_Click);
      this.namePanel.Controls.Add((Control) this.cboRESPAVersion);
      this.namePanel.Controls.Add((Control) this.label3);
      this.namePanel.Controls.Add((Control) this.label2);
      this.namePanel.Controls.Add((Control) this.descTxt);
      this.namePanel.Controls.Add((Control) this.nameTxt);
      this.namePanel.Controls.Add((Control) this.label1);
      this.namePanel.Dock = DockStyle.Top;
      this.namePanel.Location = new Point(0, 0);
      this.namePanel.Name = "namePanel";
      this.namePanel.Size = new Size(926, 130);
      this.namePanel.TabIndex = 10;
      this.cboRESPAVersion.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboRESPAVersion.FormattingEnabled = true;
      this.cboRESPAVersion.Items.AddRange(new object[4]
      {
        (object) "",
        (object) "Old GFE and HUD-1",
        (object) "RESPA 2010 GFE and HUD-1",
        (object) "RESPA-TILA 2015 LE and CD"
      });
      this.cboRESPAVersion.Location = new Point(147, 33);
      this.cboRESPAVersion.Name = "cboRESPAVersion";
      this.cboRESPAVersion.Size = new Size(205, 21);
      this.cboRESPAVersion.TabIndex = 0;
      this.label3.AutoSize = true;
      this.label3.Location = new Point(8, 36);
      this.label3.Name = "label3";
      this.label3.Size = new Size(133, 13);
      this.label3.TabIndex = 4;
      this.label3.Text = "RESPA-TILA Form Version";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(8, 63);
      this.label2.Name = "label2";
      this.label2.Size = new Size(60, 13);
      this.label2.TabIndex = 3;
      this.label2.Text = "Description";
      this.descTxt.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.descTxt.Location = new Point(147, 60);
      this.descTxt.Multiline = true;
      this.descTxt.Name = "descTxt";
      this.descTxt.ScrollBars = ScrollBars.Both;
      this.descTxt.Size = new Size(771, 60);
      this.descTxt.TabIndex = 1;
      this.nameTxt.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.nameTxt.Location = new Point(147, 6);
      this.nameTxt.MaxLength = 256;
      this.nameTxt.Name = "nameTxt";
      this.nameTxt.ReadOnly = true;
      this.nameTxt.Size = new Size(771, 20);
      this.nameTxt.TabIndex = 1;
      this.nameTxt.KeyPress += new KeyPressEventHandler(this.keypress);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(8, 8);
      this.label1.Name = "label1";
      this.label1.Size = new Size(82, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Template Name";
      this.AutoScaleBaseSize = new Size(5, 13);
      this.BackColor = Color.WhiteSmoke;
      this.CancelButton = (IButtonControl) this.cancelBtn;
      this.ClientSize = new Size(926, 624);
      this.Controls.Add((Control) this.rightPanel);
      this.KeyPreview = true;
      this.MinimizeBox = false;
      this.Name = nameof (DataTemplateDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Data Template Details";
      this.Closing += new CancelEventHandler(this.DataTemplateDialog_Closing);
      this.rightPanel.ResumeLayout(false);
      this.middlePanel.ResumeLayout(false);
      this.formPanel.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.gradientPanel1.ResumeLayout(false);
      this.gradientPanel1.PerformLayout();
      this.bottomPanel.ResumeLayout(false);
      this.bottomPanel.PerformLayout();
      this.namePanel.ResumeLayout(false);
      this.namePanel.PerformLayout();
      this.ResumeLayout(false);
    }

    private void emFormMenuList_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.OpenForm(this.emFormMenuBox.SelectedFormName);
    }

    private void saveBtn_Click(object sender, EventArgs e)
    {
      this.saveBtn.Focus();
      if (!this.dataTempLoan.Dirty)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You didn't setup value on a field.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        this.dataTemplate.MarkAsClean();
        this.dataTemplate.TemplateName = this.nameTxt.Text;
        this.dataTemplate.Description = this.descTxt.Text.Trim();
        Hashtable dirtyTable = this.dataTempLoan.DirtyTable;
        Hashtable userModifiedTable = this.dataTempLoan.DirtyUserModifiedTable;
        if (this.dataTemplate.DdmDtVersion == "18.2.0" || this.dataTemplate.DdmDtVersion == "18.2.0")
        {
          foreach (KeyValuePair<string, object> fieldDataWithValue in (Dictionary<string, object>) this.dataTemplate.FieldDataWithValues)
          {
            if (!userModifiedTable.Contains((object) fieldDataWithValue.Key))
              userModifiedTable.Add((object) fieldDataWithValue.Key, fieldDataWithValue.Value);
          }
        }
        string simpleField = this.dataTempLoan.GetSimpleField("3969");
        if (!string.IsNullOrEmpty(simpleField) && !userModifiedTable.Contains((object) "3969"))
          userModifiedTable.Add((object) "3969", (object) simpleField);
        if (dirtyTable != null)
        {
          IDictionaryEnumerator enumerator = dirtyTable.GetEnumerator();
          while (enumerator.MoveNext())
          {
            string key = (string) enumerator.Key;
            string str = this.dataTempLoan.GetSimpleField(key);
            if (key.IndexOf("RESPA.X") == 0 && str == "N")
            {
              if (key == "RESPA.X1" || key == "RESPA.X6" || key == "RESPA.X28")
                this.dataTemplate.RemoveField(key);
            }
            else if (string.Compare(key, "MS.START", true) != 0 && key != string.Empty)
            {
              if (str != string.Empty)
              {
                if (userModifiedTable.ContainsKey((object) key))
                {
                  if (str.EndsWith("|0") || str.EndsWith("|1"))
                  {
                    string[] strArray = Regex.Split(str, "|");
                    if (strArray[1] != "1")
                      str = strArray[0] + "|1";
                    this.dataTemplate.SetCurrentField(key, str);
                  }
                  else
                    this.dataTemplate.SetCurrentField(key, str + "|1");
                }
                else if (str.EndsWith("|0") || str.EndsWith("|1"))
                {
                  string[] strArray = Regex.Split(str, "|");
                  if (strArray[1] != "1")
                    str = strArray[0] + "|0";
                  this.dataTemplate.SetCurrentField(key, str);
                }
                else
                  this.dataTemplate.SetCurrentField(key, str + "|0");
              }
              else
                this.dataTemplate.RemoveField(key);
            }
          }
        }
        switch (this.cboRESPAVersion.SelectedIndex)
        {
          case 1:
            this.dataTemplate.RESPAVersion = "2009";
            this.dataTemplate.SetCurrentField("3969", "Old GFE and HUD-1|1");
            break;
          case 2:
            this.dataTemplate.RESPAVersion = "2010";
            this.dataTemplate.SetCurrentField("3969", "RESPA 2010 GFE and HUD-1|1");
            break;
          case 3:
            this.dataTemplate.RESPAVersion = "2015";
            this.dataTemplate.SetCurrentField("3969", "RESPA-TILA 2015 LE and CD|1");
            break;
          default:
            this.dataTemplate.RESPAVersion = "";
            this.dataTemplate.RemoveField("3969");
            this.dataTemplate.RemoveField("NEWHUD.X1139");
            this.dataTemplate.RemoveField("NEWHUD.X713");
            break;
        }
        string[] removedLockedFieldIds = this.dataTempLoan.GetRemovedLockedFieldIDs();
        if (removedLockedFieldIds != null && removedLockedFieldIds.Length != 0)
        {
          for (int index = 0; index < removedLockedFieldIds.Length; ++index)
            this.dataTemplate.RemoveLock(removedLockedFieldIds[index]);
        }
        string[] lockedFieldIds = this.dataTempLoan.GetLockedFieldIDs();
        this.dataTemplate.AddLocks(lockedFieldIds);
        if (lockedFieldIds != null)
        {
          for (int index = 0; index < lockedFieldIds.Length; ++index)
            this.dataTemplate.AddLockValue(lockedFieldIds[index], this.dataTempLoan.GetOrgField(lockedFieldIds[index]));
        }
        this.dataTemplate.IgnoreBusinessRules = this.isPublic && this.ignoreBR;
        this.DialogResult = DialogResult.OK;
      }
    }

    private void keypress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar.Equals((object) Keys.Delete) || char.IsControl(e.KeyChar))
        return;
      if ("/:*?<>|.".IndexOf(e.KeyChar) == -1)
      {
        char keyChar = e.KeyChar;
        if (!keyChar.Equals('\\'))
        {
          keyChar = e.KeyChar;
          if (!keyChar.Equals('"'))
          {
            e.Handled = false;
            return;
          }
        }
        e.Handled = true;
      }
      else
        e.Handled = true;
    }

    public string GetHelpTargetName() => nameof (DataTemplateDialog);

    public void ShowHelp()
    {
      JedHelp.ShowHelp((Control) this, SystemSettings.HelpFile, this.GetHelpTargetName());
    }

    private void DataTemplateDialog_Closing(object sender, CancelEventArgs e)
    {
      if (this.dataTempLoan != null)
        this.dataTempLoan.Close();
      if (this.freeScreen == null)
        return;
      this.freeScreen.Dispose();
    }

    private void chkIgnore_CheckedChanged(object sender, EventArgs e)
    {
      if (this.chkIgnore.Checked)
        this.ignoreBR = true;
      else
        this.ignoreBR = false;
    }

    public TriggerEmailTemplate MilestoneTemplateEmailTemplate
    {
      get => throw new Exception("The method or operation is not implemented.");
      set => throw new Exception("The method or operation is not implemented.");
    }

    public bool SelectSettlementServiceProviders()
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public bool SelectAffilatesTemplate()
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void ShowMilestoneWorksheet(MilestoneLog ms)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void ShoweDisclosureTrackingRecord(string packageID)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void RefreshLoan() => throw new Exception("The method or operation is not implemented.");

    public void ShowVerifPanel(string verifType)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void RefreshContents()
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void RefreshLoanContents()
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void RefreshContents(string fieldId)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void RefreshLogPanel()
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void StartConversation(ConversationLog con)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void RefreshLoanTeamMembers()
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void AddMilestoneWorksheet(MilestoneLog milestoneLog)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void ApplyBusinessRules()
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void SetMilestoneStatus(MilestoneLog milestoneLog, int milestoneIndex, bool finished)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void ClearMilestoneLogArea()
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void AddToWorkArea(Control worksheet)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void AddToWorkArea(Control worksheet, bool rememberCurrentFormID)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void RemoveFromWorkArea()
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public DateTime AddDays(DateTime date, int dayCount)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public int MinusBusinessDays(DateTime previous, DateTime currentLog)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void SaveLoan() => throw new Exception("The method or operation is not implemented.");

    public string CurrentForm
    {
      get => throw new Exception("The method or operation is not implemented.");
      set => throw new Exception("The method or operation is not implemented.");
    }

    public object GetFormScreen()
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public object GetVerifScreen()
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void GoToField(string fieldID)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void GoToField(string fieldID, BorrowerPair targetPair)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void GoToField(string fieldID, string formName)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void GoToField(string fieldID, bool findNext)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void BAMGoToField(string fieldID, bool findNext)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void GoToField(string fieldID, bool findNext, bool searchToolPageOnly)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void ApplyOnDemandBusinessRules()
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public bool OpenForm(string formOrToolName) => this.OpenForm(formOrToolName, (Control) null);

    public bool OpenForm(string formOrToolName, Control navControl)
    {
      if (formOrToolName == "MLDS - CA GFE (RE882)" || formOrToolName.StartsWith("State Specific Information"))
        return this.OpenFormByID(formOrToolName, navControl);
      InputFormInfo inputFormInfo = this.inputList.GetFormByName(formOrToolName);
      if (inputFormInfo == (InputFormInfo) null)
      {
        switch (formOrToolName)
        {
          case "2018 HMDA Originated/Adverse Action Loans":
            inputFormInfo = new InputFormInfo("HMDA2018_Originated", "2018 HMDA Originated/Adverse Action Loans");
            break;
          case "2018 HMDA Purchased Loans":
            inputFormInfo = new InputFormInfo("HMDA2018_Purchased", "2018 HMDA Purchased Loans");
            break;
          case "Repurchased Loans":
            inputFormInfo = new InputFormInfo("HMDA2018_Repurchased", "Repurchased Loans");
            break;
          default:
            return false;
        }
      }
      return this.OpenFormByID(inputFormInfo.FormID, navControl);
    }

    public bool OpenFormByID(string formOrToolID)
    {
      return this.OpenFormByID(formOrToolID, (Control) null);
    }

    private void DisposeWebPanelControls()
    {
      foreach (Control control in (ArrangedElementCollection) this.webPanel.Controls)
      {
        if (!(control is LoanScreen))
          control.Dispose();
      }
      GC.Collect();
      GC.WaitForPendingFinalizers();
    }

    public bool OpenFormByID(string formOrToolID, Control navControl)
    {
      Cursor.Current = Cursors.WaitCursor;
      this.currForm = !(formOrToolID == "MLDS - CA GFE (RE882)") ? (!formOrToolID.StartsWith("State Specific Information") ? this.inputList.GetForm(formOrToolID) : new InputFormInfo(this.getStateForm(formOrToolID), formOrToolID, InputFormType.Standard)) : new InputFormInfo("RE882", "MLDS - CA GFE", InputFormType.Standard);
      if (this.currForm == (InputFormInfo) null)
      {
        switch (formOrToolID)
        {
          case "HMDA2018_Originated":
            this.currForm = new InputFormInfo("HMDA2018_Originated", "2018 HMDA Originated/Adverse Action Loans");
            break;
          case "HMDA2018_Purchased":
            this.currForm = new InputFormInfo("HMDA2018_Purchased", "2018 HMDA Purchased Loans");
            break;
          case "HMDA2018_Repurchased":
            this.currForm = new InputFormInfo("HMDA2018_Repurchased", "Repurchased Loans");
            break;
        }
      }
      if (this.currForm.FormID == "HMDA_DENIAL")
        this.currForm = new InputFormInfo("HMDA_DENIAL04", this.currForm.MnemonicName);
      if (this.currForm.FormID == "FUNDINGWORKSHEET")
        this.currForm = new InputFormInfo("FUNDERWORKSHEET", this.currForm.MnemonicName);
      this.DisposeWebPanelControls();
      this.webPanel.Controls.Clear();
      if (this.freeScreen == null)
      {
        string empty = string.Empty;
        string xmlData;
        try
        {
          FileStream fileStream = File.OpenRead(AssemblyResolver.GetResourceFileFullPath("Documents\\Templates\\BlankLoan\\BlankData.XML", SystemSettings.LocalAppDir));
          byte[] numArray = new byte[fileStream.Length];
          fileStream.Read(numArray, 0, numArray.Length);
          fileStream.Close();
          xmlData = Encoding.ASCII.GetString(numArray);
        }
        catch (Exception ex)
        {
          Tracing.Log(DataTemplateDialog.sw, TraceLevel.Error, nameof (DataTemplateDialog), "Error opening BlankLoan template. Message: " + ex.Message);
          return false;
        }
        ILoanConfigurationInfo configurationInfo = this.session.SessionObjects.LoanManager.GetLoanConfigurationInfo();
        this.dataTempLoan = new LoanData(xmlData, configurationInfo.LoanSettings, false);
        this.dataTempLoan.IsTemplate = true;
        this.dataTempLoan.SetCurrentField("181", "");
        this.dataTempLoan.SetCurrentField("CPA.RetainUserInputs", "N");
        this.dataTempLoan.SetCurrentField("VASUMM.X140", "Y");
        this.dataTempLoan.SetCurrentField("VASUMM.X141", "Y");
        this.dataTempLoan.SetCurrentField("VASUMM.X142", "Y");
        this.dataTempLoan.SetCurrentField("VASUMM.X143", "Y");
        this.dataTempLoan.SetCurrentField("VASUMM.X144", "Y");
        this.dataTempLoan.SetCurrentField("VASUMM.X145", "Y");
        this.dataTempLoan.SetCurrentField("VASUMM.X146", "Y");
        this.dataTempLoan.SetCurrentField("VASUMM.X147", "Y");
        this.dataTempLoan.SetCurrentField("VASUMM.X148", "Y");
        LoanCalculator loanCalculator = new LoanCalculator(this.session.SessionObjects, configurationInfo, this.dataTempLoan);
        this.dataTempLoan.IgnoreValidationErrors = true;
        this.dataTempLoan.Calculator.SkipFieldChangeEvent = true;
        foreach (string assignedFieldId in this.dataTemplate.GetAssignedFieldIDs())
        {
          if (!(assignedFieldId == "1393"))
          {
            string simpleField = this.dataTemplate.GetSimpleField(assignedFieldId);
            if (assignedFieldId != string.Empty)
            {
              if (simpleField != string.Empty)
              {
                try
                {
                  if (this.dataTemplate.IsLocked(assignedFieldId))
                  {
                    string orgField = this.dataTemplate.GetOrgField(assignedFieldId);
                    if (orgField != "True")
                      this.dataTempLoan.SetCurrentField(assignedFieldId, orgField);
                    else
                      this.dataTempLoan.SetCurrentField(assignedFieldId, simpleField);
                  }
                  else
                    this.dataTempLoan.SetCurrentField(assignedFieldId, simpleField);
                }
                catch (Exception ex)
                {
                  Tracing.Log(DataTemplateDialog.sw, TraceLevel.Error, nameof (DataTemplateDialog), "Can not set field " + assignedFieldId + " value. Message: " + ex.Message);
                }
              }
            }
          }
        }
        foreach (string lockedFieldId in this.dataTemplate.GetLockedFieldIDs())
        {
          if (lockedFieldId != string.Empty)
          {
            if (lockedFieldId != null)
            {
              try
              {
                string simpleField = this.dataTemplate.GetSimpleField(lockedFieldId);
                this.dataTempLoan.AddLock(lockedFieldId);
                this.dataTempLoan.SetCurrentField(lockedFieldId, simpleField);
              }
              catch (Exception ex)
              {
                Tracing.Log(DataTemplateDialog.sw, TraceLevel.Error, nameof (DataTemplateDialog), "Can not lock field " + lockedFieldId + ". Message: " + ex.Message);
              }
            }
          }
        }
        this.cboRESPAVersion_SelectedIndexChanged((object) null, (EventArgs) null);
        this.dataTempLoan.Calculator.SkipFieldChangeEvent = false;
        this.freeScreen = new LoanScreen(this.session, (IWin32Window) this, (IHtmlInput) this.dataTempLoan);
        this.freeScreen.SetHelpTarget((IOnlineHelpTarget) this);
        this.webPanel.Controls.Add((Control) this.freeScreen);
      }
      if (this.currForm.Name == "Self-Employed Income 1084")
      {
        this.currForm = new InputFormInfo("FM1084A", "108&4A Cash Analysis");
        this.freeScreen.SetTitle(this.currForm.Name, (Control) new QuickLinksControl((ILoanEditor) this, QuickLinksControl.GetQuickLinksForForm(this.currForm.FormID, this.session), this.currForm.FormID, this.session));
      }
      else if (this.currForm.Name == "Custom Fields")
        this.freeScreen.SetTitle(this.currForm.Name, (Control) new CustomFieldsPanel(this.freeScreen, this.session));
      else if (this.currForm.Name == "Borrower Information - Vesting" || this.currForm.Name == "File Contacts" || this.currForm.Name == "1003 Page 4" || this.currForm.Name == "Home Counseling Providers")
      {
        this.freeScreen.LoadWindowsForm(this.currForm.FormID);
        this.freeScreen.SetTitleOnly(this.currForm.Name);
      }
      else
      {
        if (TabLinksControl.UseTabLinks(this.session, this.currForm, this.dataTempLoan))
        {
          this.freeScreen.BrwHandler.UnloadForm();
          this.webPanel.Controls.Add((Control) new TabLinksControl(this.session, this.currForm, (IWin32Window) this, this.dataTempLoan));
          return true;
        }
        this.freeScreen.SetTitle(this.currForm.Name);
        this.freeScreen.LoadForm(this.currForm);
      }
      this.webPanel.Controls.Add((Control) this.freeScreen);
      return true;
    }

    private void loadHMDAForm(string id, string val)
    {
      InputFormInfo inputFormInfo = new InputFormInfo("HMDA_DENIAL04", "HMDA Information");
      this.OpenForm("HMDA Information");
    }

    private string getStateForm(string formName)
    {
      int num = formName.IndexOf("-");
      return "State_Specific_" + Utils.GetStateAbbr(formName.Substring(num + 1).Trim());
    }

    public bool OpenLogRecord(LogRecordBase rec)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void OpenMilestoneLogReview(MilestoneLog log, MilestoneHistoryLog historyLog)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void OpenMilestoneLogReview(MilestoneLog log)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void SendLockRequest(bool closeFile)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void PromptCreateNewLogRecord()
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public bool IsPrimaryEditor => false;

    public bool GetInputEngineService(LoanData loan, InputEngineServiceType serviceType)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public bool GetInputEngineService(LoanDataMgr loanDataMgr, InputEngineServiceType serviceType)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public bool Print(string[] formNames)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public bool ShowRegulationAlerts()
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public bool ShowRegulationAlertsOrderDoc()
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void ShoweDisclosureTrackingRecord(
      DisclosureTrackingBase selectedLog,
      bool clearNotification)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void ShowPlanCodeComparison(string fieldId, DocumentOrderType orderType)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    private void chkAlphaOrder_Click(object sender, EventArgs e)
    {
      this.emFormMenuBox.ListDisplayMode = this.chkAlphaOrder.Checked ? EMFormMenu.DisplayMode.Alphabetical : EMFormMenu.DisplayMode.Default;
    }

    private void cboRESPAVersion_SelectedIndexChanged(object sender, EventArgs e)
    {
      switch (this.cboRESPAVersion.SelectedIndex)
      {
        case 1:
          this.dataTemplate.SetCurrentField("3969", "Old GFE and HUD-1");
          break;
        case 2:
          this.dataTemplate.SetCurrentField("3969", "RESPA 2010 GFE and HUD-1");
          this.dataTemplate.SetCurrentField("NEWHUD.X354", "Y");
          break;
        default:
          this.dataTemplate.SetCurrentField("3969", "RESPA-TILA 2015 LE and CD");
          this.dataTemplate.SetCurrentField("NEWHUD.X1139", "Y");
          this.dataTemplate.SetCurrentField("NEWHUD.X713", "");
          break;
      }
      if (this.dataTempLoan != null)
      {
        this.dataTempLoan.SetCurrentField("3969", this.dataTemplate.GetField("3969"));
        this.dataTempLoan.SetCurrentField("NEWHUD.X1139", this.dataTemplate.GetField("NEWHUD.X1139"));
        this.dataTempLoan.SetCurrentField("NEWHUD.X713", this.dataTemplate.GetField("NEWHUD.X713"));
        this.dataTempLoan.SetCurrentField("NEWHUD.X354", this.dataTemplate.GetField("NEWHUD.X354"));
        if (sender != null)
          this.dataTempLoan.Calculator.FormCalculation("SWITCHTO2015", "", "");
      }
      if (sender == null)
        return;
      this.OpenForm(this.emFormMenuBox.SelectedFormName);
    }

    public string[] SelectLinkAndSyncTemplate()
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void ShowAIQAnalyzerMessage(
      string analyzerType,
      DateTime alertDateTime,
      string description,
      string messageID)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    public void LaunchAIQIncomeAnalyzer()
    {
      throw new Exception("The method or operation is not implemented.");
    }

    DialogResult ILoanEditor.OpenModal(string openModalOptions)
    {
      throw new NotImplementedException();
    }

    public void RedirectToUrl(string targetName) => throw new NotImplementedException();
  }
}
