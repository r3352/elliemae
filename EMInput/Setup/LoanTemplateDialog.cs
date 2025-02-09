// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.LoanTemplateDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using EllieMae.EMLite.Workflow;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class LoanTemplateDialog : Form, IHelp
  {
    private const string className = "LoanTemplateDialog";
    private static string sw = Tracing.SwOutsideLoan;
    private Label label2;
    private TextBox descTxt;
    private TextBox nameTxt;
    private Label label1;
    private Button cancelBtn;
    private Button saveBtn;
    private TextBox docTxt;
    private TextBox formTxt;
    private TextBox dataTxt;
    private TextBox loanTxt;
    private TextBox costTxt;
    private System.ComponentModel.Container components;
    private LoanTemplate loanTemplate;
    private bool isPublic;
    private string currentFile = string.Empty;
    private string userID;
    private TextBox taskTxt;
    private GroupContainer groupContainer1;
    private Label label9;
    private Label label8;
    private Label label7;
    private Label label6;
    private Label label5;
    private Label label4;
    private Label label3;
    private StandardIconButton stdBtnDelDataTemplate;
    private StandardIconButton stdBtnDelTaskSet;
    private StandardIconButton stdBtnDelDocumentSet;
    private StandardIconButton stdBtnDelProviders;
    private StandardIconButton stdBtnDelInputFormSet;
    private StandardIconButton stdBtnDelClosingCosts;
    private StandardIconButton stdBtnDelLoanProgram;
    private TextBox providerTxt;
    private StandardIconButton stdBtnProviders;
    private StandardIconButton stdBtnDataTemplate;
    private StandardIconButton stdBtnTaskSet;
    private StandardIconButton stdBtnDocumentSet;
    private StandardIconButton stdBtnInputFormSet;
    private StandardIconButton stdBtnClosingCosts;
    private StandardIconButton stdBtnLoanProgram;
    private EMHelpLink emHelpLink1;
    private StandardIconButton stdBtnDelMilestoneTemplate;
    private StandardIconButton stdBtnMilestoneTemplate;
    private Label label10;
    private TextBox txtMilestoneTemplate;
    private MilestoneTemplate milestoneTemplate;
    private StandardIconButton stdBtnDelAffiliates;
    private TextBox affiliateTxt;
    private StandardIconButton stdBtnAffiliates;
    private Label label11;
    private Sessions.Session session;
    private const string BADCHARS = "/:*?<>|.";

    public LoanTemplateDialog(
      Sessions.Session session,
      LoanTemplate loanTemplate,
      bool isPublic,
      bool readOnly)
      : this(session, loanTemplate, isPublic)
    {
      if (!readOnly)
        return;
      this.nameTxt.ReadOnly = true;
      this.descTxt.ReadOnly = true;
      this.loanTxt.ReadOnly = true;
      this.costTxt.ReadOnly = true;
      this.formTxt.ReadOnly = true;
      this.providerTxt.ReadOnly = true;
      this.docTxt.ReadOnly = true;
      this.taskTxt.ReadOnly = true;
      this.dataTxt.ReadOnly = true;
      this.txtMilestoneTemplate.ReadOnly = true;
      this.affiliateTxt.ReadOnly = true;
      this.stdBtnLoanProgram.Enabled = false;
      this.stdBtnClosingCosts.Enabled = false;
      this.stdBtnInputFormSet.Enabled = false;
      this.stdBtnProviders.Enabled = false;
      this.stdBtnAffiliates.Enabled = false;
      this.stdBtnDocumentSet.Enabled = false;
      this.stdBtnTaskSet.Enabled = false;
      this.stdBtnDataTemplate.Enabled = false;
      this.stdBtnMilestoneTemplate.Enabled = false;
      this.stdBtnAffiliates.Enabled = false;
      this.stdBtnDelLoanProgram.Enabled = false;
      this.stdBtnDelClosingCosts.Enabled = false;
      this.stdBtnDelInputFormSet.Enabled = false;
      this.stdBtnDelProviders.Enabled = false;
      this.stdBtnDelDocumentSet.Enabled = false;
      this.stdBtnDelTaskSet.Enabled = false;
      this.stdBtnDelDataTemplate.Enabled = false;
      this.stdBtnDelMilestoneTemplate.Enabled = false;
      this.stdBtnDelAffiliates.Enabled = false;
      this.saveBtn.Visible = false;
      this.cancelBtn.Text = "Close";
      this.AcceptButton = (IButtonControl) this.cancelBtn;
      this.Text = "Loan Template";
    }

    public LoanTemplateDialog(Sessions.Session session, LoanTemplate loanTemplate, bool isPublic)
    {
      this.session = session;
      this.userID = this.session.UserInfo.Userid;
      this.InitializeComponent();
      this.emHelpLink1.AssignSession(this.session);
      this.isPublic = isPublic;
      foreach (string assignedFieldId in loanTemplate.GetAssignedFieldIDs())
      {
        string simpleField = loanTemplate.GetSimpleField(assignedFieldId);
        // ISSUE: reference to a compiler-generated method
        switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(assignedFieldId))
        {
          case 154547182:
            if (assignedFieldId == "MILETEMP")
            {
              try
              {
                if (simpleField != string.Empty)
                  this.milestoneTemplate = this.session.SessionObjects.BpmManager.GetMilestoneTemplateByGuid(simpleField);
              }
              catch
              {
              }
              if (this.milestoneTemplate != null)
              {
                if (this.milestoneTemplate.Active)
                {
                  this.txtMilestoneTemplate.Text = this.milestoneTemplate.Name;
                  break;
                }
                this.txtMilestoneTemplate.Text = this.milestoneTemplate.Name + " (Deactivated)";
                break;
              }
              break;
            }
            break;
          case 632186936:
            if (assignedFieldId == "COST")
            {
              this.costTxt.Text = simpleField;
              break;
            }
            break;
          case 1148687780:
            if (assignedFieldId == "PROVIDERLIST")
            {
              this.providerTxt.Text = simpleField;
              break;
            }
            break;
          case 1603920811:
            if (assignedFieldId == "PROGRAM")
            {
              this.loanTxt.Text = simpleField;
              break;
            }
            break;
          case 2023548019:
            if (assignedFieldId == "DOCSET")
            {
              this.docTxt.Text = simpleField;
              break;
            }
            break;
          case 2193314268:
            if (assignedFieldId == "AFFILIATELIST")
            {
              this.affiliateTxt.Text = simpleField;
              break;
            }
            break;
          case 2307682329:
            if (assignedFieldId == "MISCDATA")
            {
              this.dataTxt.Text = simpleField;
              break;
            }
            break;
          case 3531113935:
            if (assignedFieldId == "FORMLIST")
            {
              this.formTxt.Text = simpleField;
              break;
            }
            break;
          case 3788298884:
            if (assignedFieldId == "TASKSET")
            {
              this.taskTxt.Text = simpleField;
              break;
            }
            break;
        }
      }
      string str = loanTemplate.TemplateName;
      int num = str.LastIndexOf("\\");
      if (num > -1)
        str = str.Substring(num + 1);
      this.nameTxt.Text = str;
      this.descTxt.Text = loanTemplate.Description;
      this.loanTemplate = loanTemplate;
      if (this.session.EncompassEdition != EncompassEdition.Broker)
        return;
      this.brokerVersion();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.label2 = new Label();
      this.descTxt = new TextBox();
      this.nameTxt = new TextBox();
      this.label1 = new Label();
      this.cancelBtn = new Button();
      this.saveBtn = new Button();
      this.taskTxt = new TextBox();
      this.costTxt = new TextBox();
      this.loanTxt = new TextBox();
      this.dataTxt = new TextBox();
      this.formTxt = new TextBox();
      this.docTxt = new TextBox();
      this.emHelpLink1 = new EMHelpLink();
      this.groupContainer1 = new GroupContainer();
      this.stdBtnDelAffiliates = new StandardIconButton();
      this.affiliateTxt = new TextBox();
      this.stdBtnAffiliates = new StandardIconButton();
      this.label11 = new Label();
      this.stdBtnDelMilestoneTemplate = new StandardIconButton();
      this.stdBtnMilestoneTemplate = new StandardIconButton();
      this.label10 = new Label();
      this.txtMilestoneTemplate = new TextBox();
      this.stdBtnDelDataTemplate = new StandardIconButton();
      this.stdBtnDelTaskSet = new StandardIconButton();
      this.stdBtnDelDocumentSet = new StandardIconButton();
      this.stdBtnDelProviders = new StandardIconButton();
      this.stdBtnDelInputFormSet = new StandardIconButton();
      this.stdBtnDelClosingCosts = new StandardIconButton();
      this.stdBtnDelLoanProgram = new StandardIconButton();
      this.providerTxt = new TextBox();
      this.stdBtnProviders = new StandardIconButton();
      this.stdBtnDataTemplate = new StandardIconButton();
      this.stdBtnTaskSet = new StandardIconButton();
      this.stdBtnDocumentSet = new StandardIconButton();
      this.stdBtnInputFormSet = new StandardIconButton();
      this.stdBtnClosingCosts = new StandardIconButton();
      this.stdBtnLoanProgram = new StandardIconButton();
      this.label9 = new Label();
      this.label8 = new Label();
      this.label7 = new Label();
      this.label6 = new Label();
      this.label5 = new Label();
      this.label4 = new Label();
      this.label3 = new Label();
      this.groupContainer1.SuspendLayout();
      ((ISupportInitialize) this.stdBtnDelAffiliates).BeginInit();
      ((ISupportInitialize) this.stdBtnAffiliates).BeginInit();
      ((ISupportInitialize) this.stdBtnDelMilestoneTemplate).BeginInit();
      ((ISupportInitialize) this.stdBtnMilestoneTemplate).BeginInit();
      ((ISupportInitialize) this.stdBtnDelDataTemplate).BeginInit();
      ((ISupportInitialize) this.stdBtnDelTaskSet).BeginInit();
      ((ISupportInitialize) this.stdBtnDelDocumentSet).BeginInit();
      ((ISupportInitialize) this.stdBtnDelProviders).BeginInit();
      ((ISupportInitialize) this.stdBtnDelInputFormSet).BeginInit();
      ((ISupportInitialize) this.stdBtnDelClosingCosts).BeginInit();
      ((ISupportInitialize) this.stdBtnDelLoanProgram).BeginInit();
      ((ISupportInitialize) this.stdBtnProviders).BeginInit();
      ((ISupportInitialize) this.stdBtnDataTemplate).BeginInit();
      ((ISupportInitialize) this.stdBtnTaskSet).BeginInit();
      ((ISupportInitialize) this.stdBtnDocumentSet).BeginInit();
      ((ISupportInitialize) this.stdBtnInputFormSet).BeginInit();
      ((ISupportInitialize) this.stdBtnClosingCosts).BeginInit();
      ((ISupportInitialize) this.stdBtnLoanProgram).BeginInit();
      this.SuspendLayout();
      this.label2.AutoSize = true;
      this.label2.Location = new Point(2, 38);
      this.label2.Name = "label2";
      this.label2.Size = new Size(60, 13);
      this.label2.TabIndex = 7;
      this.label2.Text = "Description";
      this.descTxt.Location = new Point(149, 35);
      this.descTxt.Multiline = true;
      this.descTxt.Name = "descTxt";
      this.descTxt.ScrollBars = ScrollBars.Both;
      this.descTxt.Size = new Size(370, 87);
      this.descTxt.TabIndex = 0;
      this.nameTxt.Location = new Point(149, 10);
      this.nameTxt.MaxLength = 256;
      this.nameTxt.Name = "nameTxt";
      this.nameTxt.ReadOnly = true;
      this.nameTxt.Size = new Size(370, 20);
      this.nameTxt.TabIndex = 1;
      this.nameTxt.TabStop = false;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(2, 13);
      this.label1.Name = "label1";
      this.label1.Size = new Size(82, 13);
      this.label1.TabIndex = 5;
      this.label1.Text = "Template Name";
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(441, 395);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 24);
      this.cancelBtn.TabIndex = 9;
      this.cancelBtn.Text = "Cancel";
      this.saveBtn.Location = new Point(361, 395);
      this.saveBtn.Name = "saveBtn";
      this.saveBtn.Size = new Size(75, 24);
      this.saveBtn.TabIndex = 8;
      this.saveBtn.Text = "&Save";
      this.saveBtn.Click += new EventHandler(this.saveBtn_Click);
      this.taskTxt.Location = new Point(151, 173);
      this.taskTxt.Name = "taskTxt";
      this.taskTxt.ReadOnly = true;
      this.taskTxt.Size = new Size(322, 20);
      this.taskTxt.TabIndex = 20;
      this.taskTxt.TabStop = false;
      this.costTxt.Location = new Point(151, 77);
      this.costTxt.Name = "costTxt";
      this.costTxt.ReadOnly = true;
      this.costTxt.Size = new Size(322, 20);
      this.costTxt.TabIndex = 6;
      this.costTxt.TabStop = false;
      this.costTxt.KeyPress += new KeyPressEventHandler(this.keypress);
      this.loanTxt.Location = new Point(151, 29);
      this.loanTxt.Name = "loanTxt";
      this.loanTxt.ReadOnly = true;
      this.loanTxt.Size = new Size(322, 20);
      this.loanTxt.TabIndex = 5;
      this.loanTxt.TabStop = false;
      this.loanTxt.KeyPress += new KeyPressEventHandler(this.keypress);
      this.dataTxt.Location = new Point(151, 197);
      this.dataTxt.Name = "dataTxt";
      this.dataTxt.ReadOnly = true;
      this.dataTxt.Size = new Size(322, 20);
      this.dataTxt.TabIndex = 4;
      this.dataTxt.TabStop = false;
      this.dataTxt.KeyPress += new KeyPressEventHandler(this.keypress);
      this.formTxt.Location = new Point(151, 101);
      this.formTxt.Name = "formTxt";
      this.formTxt.ReadOnly = true;
      this.formTxt.Size = new Size(322, 20);
      this.formTxt.TabIndex = 3;
      this.formTxt.TabStop = false;
      this.formTxt.KeyPress += new KeyPressEventHandler(this.keypress);
      this.docTxt.Location = new Point(151, 149);
      this.docTxt.Name = "docTxt";
      this.docTxt.ReadOnly = true;
      this.docTxt.Size = new Size(322, 20);
      this.docTxt.TabIndex = 2;
      this.docTxt.TabStop = false;
      this.docTxt.KeyPress += new KeyPressEventHandler(this.keypress);
      this.emHelpLink1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.emHelpLink1.BackColor = Color.Transparent;
      this.emHelpLink1.Cursor = Cursors.Hand;
      this.emHelpLink1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.emHelpLink1.HelpTag = "Loan Template";
      this.emHelpLink1.Location = new Point(7, 403);
      this.emHelpLink1.Name = "emHelpLink1";
      this.emHelpLink1.Size = new Size(90, 16);
      this.emHelpLink1.TabIndex = 11;
      this.groupContainer1.Controls.Add((Control) this.stdBtnDelAffiliates);
      this.groupContainer1.Controls.Add((Control) this.affiliateTxt);
      this.groupContainer1.Controls.Add((Control) this.stdBtnAffiliates);
      this.groupContainer1.Controls.Add((Control) this.label11);
      this.groupContainer1.Controls.Add((Control) this.stdBtnDelMilestoneTemplate);
      this.groupContainer1.Controls.Add((Control) this.stdBtnMilestoneTemplate);
      this.groupContainer1.Controls.Add((Control) this.label10);
      this.groupContainer1.Controls.Add((Control) this.txtMilestoneTemplate);
      this.groupContainer1.Controls.Add((Control) this.stdBtnDelDataTemplate);
      this.groupContainer1.Controls.Add((Control) this.stdBtnDelTaskSet);
      this.groupContainer1.Controls.Add((Control) this.stdBtnDelDocumentSet);
      this.groupContainer1.Controls.Add((Control) this.stdBtnDelProviders);
      this.groupContainer1.Controls.Add((Control) this.stdBtnDelInputFormSet);
      this.groupContainer1.Controls.Add((Control) this.stdBtnDelClosingCosts);
      this.groupContainer1.Controls.Add((Control) this.stdBtnDelLoanProgram);
      this.groupContainer1.Controls.Add((Control) this.providerTxt);
      this.groupContainer1.Controls.Add((Control) this.stdBtnProviders);
      this.groupContainer1.Controls.Add((Control) this.stdBtnDataTemplate);
      this.groupContainer1.Controls.Add((Control) this.stdBtnTaskSet);
      this.groupContainer1.Controls.Add((Control) this.stdBtnDocumentSet);
      this.groupContainer1.Controls.Add((Control) this.stdBtnInputFormSet);
      this.groupContainer1.Controls.Add((Control) this.stdBtnClosingCosts);
      this.groupContainer1.Controls.Add((Control) this.stdBtnLoanProgram);
      this.groupContainer1.Controls.Add((Control) this.label9);
      this.groupContainer1.Controls.Add((Control) this.label8);
      this.groupContainer1.Controls.Add((Control) this.taskTxt);
      this.groupContainer1.Controls.Add((Control) this.label7);
      this.groupContainer1.Controls.Add((Control) this.label6);
      this.groupContainer1.Controls.Add((Control) this.label5);
      this.groupContainer1.Controls.Add((Control) this.label4);
      this.groupContainer1.Controls.Add((Control) this.label3);
      this.groupContainer1.Controls.Add((Control) this.docTxt);
      this.groupContainer1.Controls.Add((Control) this.formTxt);
      this.groupContainer1.Controls.Add((Control) this.dataTxt);
      this.groupContainer1.Controls.Add((Control) this.loanTxt);
      this.groupContainer1.Controls.Add((Control) this.costTxt);
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(-2, 128);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(530, 250);
      this.groupContainer1.TabIndex = 12;
      this.groupContainer1.Text = "Templates";
      this.stdBtnDelAffiliates.BackColor = Color.Transparent;
      this.stdBtnDelAffiliates.Location = new Point(502, 224);
      this.stdBtnDelAffiliates.MouseDownImage = (Image) null;
      this.stdBtnDelAffiliates.Name = "stdBtnDelAffiliates";
      this.stdBtnDelAffiliates.Size = new Size(16, 16);
      this.stdBtnDelAffiliates.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.stdBtnDelAffiliates.TabIndex = 43;
      this.stdBtnDelAffiliates.TabStop = false;
      this.stdBtnDelAffiliates.Click += new EventHandler(this.clearBtn_Click);
      this.affiliateTxt.Location = new Point(151, 221);
      this.affiliateTxt.Name = "affiliateTxt";
      this.affiliateTxt.ReadOnly = true;
      this.affiliateTxt.Size = new Size(322, 20);
      this.affiliateTxt.TabIndex = 42;
      this.affiliateTxt.TabStop = false;
      this.stdBtnAffiliates.BackColor = Color.Transparent;
      this.stdBtnAffiliates.Location = new Point(480, 224);
      this.stdBtnAffiliates.MouseDownImage = (Image) null;
      this.stdBtnAffiliates.Name = "stdBtnAffiliates";
      this.stdBtnAffiliates.Size = new Size(16, 16);
      this.stdBtnAffiliates.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.stdBtnAffiliates.TabIndex = 41;
      this.stdBtnAffiliates.TabStop = false;
      this.stdBtnAffiliates.Click += new EventHandler(this.templateButtons_Click);
      this.label11.AutoSize = true;
      this.label11.Location = new Point(4, 224);
      this.label11.Name = "label11";
      this.label11.Size = new Size(88, 13);
      this.label11.TabIndex = 40;
      this.label11.Text = "Affiliate Template";
      this.stdBtnDelMilestoneTemplate.BackColor = Color.Transparent;
      this.stdBtnDelMilestoneTemplate.Location = new Point(502, 56);
      this.stdBtnDelMilestoneTemplate.MouseDownImage = (Image) null;
      this.stdBtnDelMilestoneTemplate.Name = "stdBtnDelMilestoneTemplate";
      this.stdBtnDelMilestoneTemplate.Size = new Size(16, 16);
      this.stdBtnDelMilestoneTemplate.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.stdBtnDelMilestoneTemplate.TabIndex = 39;
      this.stdBtnDelMilestoneTemplate.TabStop = false;
      this.stdBtnDelMilestoneTemplate.Click += new EventHandler(this.clearBtn_Click);
      this.stdBtnMilestoneTemplate.BackColor = Color.Transparent;
      this.stdBtnMilestoneTemplate.Location = new Point(480, 56);
      this.stdBtnMilestoneTemplate.MouseDownImage = (Image) null;
      this.stdBtnMilestoneTemplate.Name = "stdBtnMilestoneTemplate";
      this.stdBtnMilestoneTemplate.Size = new Size(16, 16);
      this.stdBtnMilestoneTemplate.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.stdBtnMilestoneTemplate.TabIndex = 38;
      this.stdBtnMilestoneTemplate.TabStop = false;
      this.stdBtnMilestoneTemplate.Click += new EventHandler(this.templateButtons_Click);
      this.label10.AutoSize = true;
      this.label10.Location = new Point(4, 56);
      this.label10.Name = "label10";
      this.label10.Size = new Size(99, 13);
      this.label10.TabIndex = 36;
      this.label10.Text = "Milestone Template";
      this.txtMilestoneTemplate.Location = new Point(151, 53);
      this.txtMilestoneTemplate.Name = "txtMilestoneTemplate";
      this.txtMilestoneTemplate.ReadOnly = true;
      this.txtMilestoneTemplate.Size = new Size(322, 20);
      this.txtMilestoneTemplate.TabIndex = 37;
      this.txtMilestoneTemplate.TabStop = false;
      this.stdBtnDelDataTemplate.BackColor = Color.Transparent;
      this.stdBtnDelDataTemplate.Location = new Point(502, 200);
      this.stdBtnDelDataTemplate.MouseDownImage = (Image) null;
      this.stdBtnDelDataTemplate.Name = "stdBtnDelDataTemplate";
      this.stdBtnDelDataTemplate.Size = new Size(16, 16);
      this.stdBtnDelDataTemplate.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.stdBtnDelDataTemplate.TabIndex = 35;
      this.stdBtnDelDataTemplate.TabStop = false;
      this.stdBtnDelDataTemplate.Click += new EventHandler(this.clearBtn_Click);
      this.stdBtnDelTaskSet.BackColor = Color.Transparent;
      this.stdBtnDelTaskSet.Location = new Point(502, 176);
      this.stdBtnDelTaskSet.MouseDownImage = (Image) null;
      this.stdBtnDelTaskSet.Name = "stdBtnDelTaskSet";
      this.stdBtnDelTaskSet.Size = new Size(16, 16);
      this.stdBtnDelTaskSet.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.stdBtnDelTaskSet.TabIndex = 34;
      this.stdBtnDelTaskSet.TabStop = false;
      this.stdBtnDelTaskSet.Click += new EventHandler(this.clearBtn_Click);
      this.stdBtnDelDocumentSet.BackColor = Color.Transparent;
      this.stdBtnDelDocumentSet.Location = new Point(502, 152);
      this.stdBtnDelDocumentSet.MouseDownImage = (Image) null;
      this.stdBtnDelDocumentSet.Name = "stdBtnDelDocumentSet";
      this.stdBtnDelDocumentSet.Size = new Size(16, 16);
      this.stdBtnDelDocumentSet.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.stdBtnDelDocumentSet.TabIndex = 33;
      this.stdBtnDelDocumentSet.TabStop = false;
      this.stdBtnDelDocumentSet.Click += new EventHandler(this.clearBtn_Click);
      this.stdBtnDelProviders.BackColor = Color.Transparent;
      this.stdBtnDelProviders.Location = new Point(502, 128);
      this.stdBtnDelProviders.MouseDownImage = (Image) null;
      this.stdBtnDelProviders.Name = "stdBtnDelProviders";
      this.stdBtnDelProviders.Size = new Size(16, 16);
      this.stdBtnDelProviders.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.stdBtnDelProviders.TabIndex = 32;
      this.stdBtnDelProviders.TabStop = false;
      this.stdBtnDelProviders.Click += new EventHandler(this.clearBtn_Click);
      this.stdBtnDelInputFormSet.BackColor = Color.Transparent;
      this.stdBtnDelInputFormSet.Location = new Point(502, 104);
      this.stdBtnDelInputFormSet.MouseDownImage = (Image) null;
      this.stdBtnDelInputFormSet.Name = "stdBtnDelInputFormSet";
      this.stdBtnDelInputFormSet.Size = new Size(16, 16);
      this.stdBtnDelInputFormSet.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.stdBtnDelInputFormSet.TabIndex = 31;
      this.stdBtnDelInputFormSet.TabStop = false;
      this.stdBtnDelInputFormSet.Click += new EventHandler(this.clearBtn_Click);
      this.stdBtnDelClosingCosts.BackColor = Color.Transparent;
      this.stdBtnDelClosingCosts.Location = new Point(502, 80);
      this.stdBtnDelClosingCosts.MouseDownImage = (Image) null;
      this.stdBtnDelClosingCosts.Name = "stdBtnDelClosingCosts";
      this.stdBtnDelClosingCosts.Size = new Size(16, 16);
      this.stdBtnDelClosingCosts.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.stdBtnDelClosingCosts.TabIndex = 30;
      this.stdBtnDelClosingCosts.TabStop = false;
      this.stdBtnDelClosingCosts.Click += new EventHandler(this.clearBtn_Click);
      this.stdBtnDelLoanProgram.BackColor = Color.Transparent;
      this.stdBtnDelLoanProgram.Location = new Point(502, 32);
      this.stdBtnDelLoanProgram.MouseDownImage = (Image) null;
      this.stdBtnDelLoanProgram.Name = "stdBtnDelLoanProgram";
      this.stdBtnDelLoanProgram.Size = new Size(16, 16);
      this.stdBtnDelLoanProgram.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.stdBtnDelLoanProgram.TabIndex = 29;
      this.stdBtnDelLoanProgram.TabStop = false;
      this.stdBtnDelLoanProgram.Click += new EventHandler(this.clearBtn_Click);
      this.providerTxt.Location = new Point(151, 125);
      this.providerTxt.Name = "providerTxt";
      this.providerTxt.ReadOnly = true;
      this.providerTxt.Size = new Size(322, 20);
      this.providerTxt.TabIndex = 28;
      this.providerTxt.TabStop = false;
      this.providerTxt.KeyPress += new KeyPressEventHandler(this.keypress);
      this.stdBtnProviders.BackColor = Color.Transparent;
      this.stdBtnProviders.Location = new Point(480, 128);
      this.stdBtnProviders.MouseDownImage = (Image) null;
      this.stdBtnProviders.Name = "stdBtnProviders";
      this.stdBtnProviders.Size = new Size(16, 16);
      this.stdBtnProviders.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.stdBtnProviders.TabIndex = 27;
      this.stdBtnProviders.TabStop = false;
      this.stdBtnProviders.Click += new EventHandler(this.templateButtons_Click);
      this.stdBtnDataTemplate.BackColor = Color.Transparent;
      this.stdBtnDataTemplate.Location = new Point(480, 200);
      this.stdBtnDataTemplate.MouseDownImage = (Image) null;
      this.stdBtnDataTemplate.Name = "stdBtnDataTemplate";
      this.stdBtnDataTemplate.Size = new Size(16, 16);
      this.stdBtnDataTemplate.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.stdBtnDataTemplate.TabIndex = 26;
      this.stdBtnDataTemplate.TabStop = false;
      this.stdBtnDataTemplate.Click += new EventHandler(this.templateButtons_Click);
      this.stdBtnTaskSet.BackColor = Color.Transparent;
      this.stdBtnTaskSet.Location = new Point(480, 176);
      this.stdBtnTaskSet.MouseDownImage = (Image) null;
      this.stdBtnTaskSet.Name = "stdBtnTaskSet";
      this.stdBtnTaskSet.Size = new Size(16, 16);
      this.stdBtnTaskSet.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.stdBtnTaskSet.TabIndex = 25;
      this.stdBtnTaskSet.TabStop = false;
      this.stdBtnTaskSet.Click += new EventHandler(this.templateButtons_Click);
      this.stdBtnDocumentSet.BackColor = Color.Transparent;
      this.stdBtnDocumentSet.Location = new Point(480, 152);
      this.stdBtnDocumentSet.MouseDownImage = (Image) null;
      this.stdBtnDocumentSet.Name = "stdBtnDocumentSet";
      this.stdBtnDocumentSet.Size = new Size(16, 16);
      this.stdBtnDocumentSet.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.stdBtnDocumentSet.TabIndex = 24;
      this.stdBtnDocumentSet.TabStop = false;
      this.stdBtnDocumentSet.Click += new EventHandler(this.templateButtons_Click);
      this.stdBtnInputFormSet.BackColor = Color.Transparent;
      this.stdBtnInputFormSet.Location = new Point(480, 104);
      this.stdBtnInputFormSet.MouseDownImage = (Image) null;
      this.stdBtnInputFormSet.Name = "stdBtnInputFormSet";
      this.stdBtnInputFormSet.Size = new Size(16, 16);
      this.stdBtnInputFormSet.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.stdBtnInputFormSet.TabIndex = 23;
      this.stdBtnInputFormSet.TabStop = false;
      this.stdBtnInputFormSet.Click += new EventHandler(this.templateButtons_Click);
      this.stdBtnClosingCosts.BackColor = Color.Transparent;
      this.stdBtnClosingCosts.Location = new Point(480, 80);
      this.stdBtnClosingCosts.MouseDownImage = (Image) null;
      this.stdBtnClosingCosts.Name = "stdBtnClosingCosts";
      this.stdBtnClosingCosts.Size = new Size(16, 16);
      this.stdBtnClosingCosts.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.stdBtnClosingCosts.TabIndex = 22;
      this.stdBtnClosingCosts.TabStop = false;
      this.stdBtnClosingCosts.Click += new EventHandler(this.templateButtons_Click);
      this.stdBtnLoanProgram.BackColor = Color.Transparent;
      this.stdBtnLoanProgram.Location = new Point(480, 32);
      this.stdBtnLoanProgram.MouseDownImage = (Image) null;
      this.stdBtnLoanProgram.Name = "stdBtnLoanProgram";
      this.stdBtnLoanProgram.Size = new Size(16, 16);
      this.stdBtnLoanProgram.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.stdBtnLoanProgram.TabIndex = 21;
      this.stdBtnLoanProgram.TabStop = false;
      this.stdBtnLoanProgram.Click += new EventHandler(this.templateButtons_Click);
      this.label9.AutoSize = true;
      this.label9.Location = new Point(4, 200);
      this.label9.Name = "label9";
      this.label9.Size = new Size(77, 13);
      this.label9.TabIndex = 6;
      this.label9.Text = "Data Template";
      this.label8.AutoSize = true;
      this.label8.Location = new Point(4, 176);
      this.label8.Name = "label8";
      this.label8.Size = new Size(50, 13);
      this.label8.TabIndex = 5;
      this.label8.Text = "Task Set";
      this.label7.AutoSize = true;
      this.label7.Location = new Point(4, 152);
      this.label7.Name = "label7";
      this.label7.Size = new Size(75, 13);
      this.label7.TabIndex = 4;
      this.label7.Text = "Document Set";
      this.label6.AutoSize = true;
      this.label6.Location = new Point(4, 128);
      this.label6.Name = "label6";
      this.label6.Size = new Size(143, 13);
      this.label6.TabIndex = 3;
      this.label6.Text = "Settlement Service Providers";
      this.label5.AutoSize = true;
      this.label5.Location = new Point(4, 104);
      this.label5.Name = "label5";
      this.label5.Size = new Size(76, 13);
      this.label5.TabIndex = 2;
      this.label5.Text = "Input Form Set";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(4, 80);
      this.label4.Name = "label4";
      this.label4.Size = new Size(70, 13);
      this.label4.TabIndex = 1;
      this.label4.Text = "Closing Costs";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(4, 32);
      this.label3.Name = "label3";
      this.label3.Size = new Size(73, 13);
      this.label3.TabIndex = 0;
      this.label3.Text = "Loan Program";
      this.AutoScaleBaseSize = new Size(5, 13);
      this.BackColor = Color.WhiteSmoke;
      this.CancelButton = (IButtonControl) this.cancelBtn;
      this.ClientSize = new Size(525, 428);
      this.Controls.Add((Control) this.groupContainer1);
      this.Controls.Add((Control) this.emHelpLink1);
      this.Controls.Add((Control) this.cancelBtn);
      this.Controls.Add((Control) this.saveBtn);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.descTxt);
      this.Controls.Add((Control) this.nameTxt);
      this.Controls.Add((Control) this.label1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (LoanTemplateDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Loan Template Details";
      this.KeyDown += new KeyEventHandler(this.Help_KeyDown);
      this.groupContainer1.ResumeLayout(false);
      this.groupContainer1.PerformLayout();
      ((ISupportInitialize) this.stdBtnDelAffiliates).EndInit();
      ((ISupportInitialize) this.stdBtnAffiliates).EndInit();
      ((ISupportInitialize) this.stdBtnDelMilestoneTemplate).EndInit();
      ((ISupportInitialize) this.stdBtnMilestoneTemplate).EndInit();
      ((ISupportInitialize) this.stdBtnDelDataTemplate).EndInit();
      ((ISupportInitialize) this.stdBtnDelTaskSet).EndInit();
      ((ISupportInitialize) this.stdBtnDelDocumentSet).EndInit();
      ((ISupportInitialize) this.stdBtnDelProviders).EndInit();
      ((ISupportInitialize) this.stdBtnDelInputFormSet).EndInit();
      ((ISupportInitialize) this.stdBtnDelClosingCosts).EndInit();
      ((ISupportInitialize) this.stdBtnDelLoanProgram).EndInit();
      ((ISupportInitialize) this.stdBtnProviders).EndInit();
      ((ISupportInitialize) this.stdBtnDataTemplate).EndInit();
      ((ISupportInitialize) this.stdBtnTaskSet).EndInit();
      ((ISupportInitialize) this.stdBtnDocumentSet).EndInit();
      ((ISupportInitialize) this.stdBtnInputFormSet).EndInit();
      ((ISupportInitialize) this.stdBtnClosingCosts).EndInit();
      ((ISupportInitialize) this.stdBtnLoanProgram).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
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

    private void saveBtn_Click(object sender, EventArgs e)
    {
      if (this.loanTxt.Text.Trim() == string.Empty && this.costTxt.Text.Trim() == string.Empty && this.formTxt.Text.Trim() == string.Empty && this.providerTxt.Text.Trim() == string.Empty && this.taskTxt.Text.Trim() == string.Empty && this.dataTxt.Text.Trim() == string.Empty && this.docTxt.Text.Trim() == string.Empty && this.txtMilestoneTemplate.Text.Trim() == string.Empty && this.affiliateTxt.Text.Trim() == string.Empty)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You need to setup at least one template.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        if (!this.verifyTemplate(this.loanTxt.Text, "loan program", TemplateSettingsType.LoanProgram) || !this.verifyTemplate(this.costTxt.Text, "closing cost", TemplateSettingsType.ClosingCost) || !this.verifyTemplate(this.formTxt.Text, "form list", TemplateSettingsType.FormList) || !this.verifyTemplate(this.providerTxt.Text, "settlement service providers", TemplateSettingsType.SettlementServiceProviders) || !this.verifyTemplate(this.affiliateTxt.Text, "affiliate template", TemplateSettingsType.AffiliatedBusinessArrangements) || !this.verifyTemplate(this.docTxt.Text, "document set", TemplateSettingsType.DocumentSet) || !this.verifyTemplate(this.taskTxt.Text, "task set", TemplateSettingsType.TaskSet) || !this.verifyTemplate(this.dataTxt.Text, "data", TemplateSettingsType.MiscData))
          return;
        this.loanTemplate.TemplateName = this.nameTxt.Text;
        this.loanTemplate.Description = this.descTxt.Text.Trim();
        this.loanTemplate.SetCurrentField("PROGRAM", this.loanTxt.Text);
        this.loanTemplate.SetCurrentField("COST", this.costTxt.Text);
        this.loanTemplate.SetCurrentField("FORMLIST", this.formTxt.Text);
        this.loanTemplate.SetCurrentField("PROVIDERLIST", this.providerTxt.Text);
        this.loanTemplate.SetCurrentField("DOCSET", this.docTxt.Text);
        this.loanTemplate.SetCurrentField("TASKSET", this.taskTxt.Text);
        this.loanTemplate.SetCurrentField("MISCDATA", this.dataTxt.Text);
        this.loanTemplate.SetCurrentField("AFFILIATELIST", this.affiliateTxt.Text);
        if (this.txtMilestoneTemplate.Text != "" && this.milestoneTemplate != null)
          this.loanTemplate.SetCurrentField("MILETEMP", this.milestoneTemplate.TemplateGuid.ToString());
        else
          this.loanTemplate.SetCurrentField("MILETEMP", "");
        this.DialogResult = DialogResult.OK;
      }
    }

    private bool verifyTemplate(
      string templateName,
      string msgID,
      TemplateSettingsType templateType)
    {
      if (templateName != string.Empty)
      {
        FileSystemEntry fileEntry = FileSystemEntry.Parse(templateName, this.session.UserID);
        if (!this.session.ConfigurationManager.TemplateSettingsObjectExists(templateType, fileEntry))
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "The '" + templateName + "' " + msgID + " template is not found.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return false;
        }
      }
      return true;
    }

    private void templateButtons_Click(object sender, EventArgs e)
    {
      StandardIconButton standardIconButton = (StandardIconButton) sender;
      string uri = string.Empty;
      string name1 = standardIconButton.Name;
      TemplateSettingsType templateType;
      // ISSUE: reference to a compiler-generated method
      switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(name1))
      {
        case 180049348:
          if (!(name1 == "stdBtnLoanProgram"))
            return;
          templateType = TemplateSettingsType.LoanProgram;
          uri = this.loanTxt.Text;
          break;
        case 383142185:
          if (!(name1 == "stdBtnTaskSet"))
            return;
          templateType = TemplateSettingsType.TaskSet;
          uri = this.taskTxt.Text;
          break;
        case 620297425:
          if (!(name1 == "stdBtnClosingCosts"))
            return;
          templateType = TemplateSettingsType.ClosingCost;
          uri = this.costTxt.Text;
          break;
        case 711601202:
          if (!(name1 == "stdBtnMilestoneTemplate"))
            return;
          templateType = TemplateSettingsType.MilestoneTemplate;
          break;
        case 1501647960:
          if (!(name1 == "stdBtnDataTemplate"))
            return;
          templateType = TemplateSettingsType.MiscData;
          uri = this.dataTxt.Text;
          break;
        case 3369964964:
          if (!(name1 == "stdBtnInputFormSet"))
            return;
          templateType = TemplateSettingsType.FormList;
          uri = this.formTxt.Text;
          break;
        case 3521714678:
          if (!(name1 == "stdBtnAffiliates"))
            return;
          templateType = TemplateSettingsType.AffiliatedBusinessArrangements;
          uri = this.affiliateTxt.Text;
          break;
        case 3570426729:
          if (!(name1 == "stdBtnDocumentSet"))
            return;
          templateType = TemplateSettingsType.DocumentSet;
          uri = this.docTxt.Text;
          break;
        case 3648787760:
          if (!(name1 == "stdBtnProviders"))
            return;
          templateType = TemplateSettingsType.SettlementServiceProviders;
          uri = this.providerTxt.Text;
          break;
        default:
          return;
      }
      FileSystemEntry defaultFolder = (FileSystemEntry) null;
      if (uri != string.Empty)
      {
        try
        {
          defaultFolder = FileSystemEntry.Parse(uri, this.session.UserID).ParentFolder;
        }
        catch
        {
        }
      }
      TemplateSelectionDialog templateSelectionDialog = new TemplateSelectionDialog(this.session, templateType, defaultFolder, this.isPublic);
      if (templateSelectionDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
        return;
      string str = string.Empty;
      if (templateType == TemplateSettingsType.MilestoneTemplate)
      {
        this.milestoneTemplate = templateSelectionDialog.SelectedMilestoneTemplate;
        this.txtMilestoneTemplate.Text = templateSelectionDialog.SelectedMilestoneTemplate.Name;
      }
      else
      {
        FileSystemEntry selectedItem = templateSelectionDialog.SelectedItem;
        if (this.isPublic && !selectedItem.IsPublic)
        {
          switch (standardIconButton.Name)
          {
            case "stdBtnAffiliates":
              str = " affiliates ";
              break;
            case "stdBtnClosingCosts":
              str = " closing cost ";
              break;
            case "stdBtnDataTemplate":
              str = " data ";
              break;
            case "stdBtnDocumentSet":
              str = " document set ";
              break;
            case "stdBtnInputFormSet":
              str = " form list ";
              break;
            case "stdBtnLoanProgram":
              str = " loan program ";
              break;
            case "stdBtnProviders":
              str = " settelment service providers ";
              break;
            case "stdBtnTaskSet":
              str = " task set ";
              break;
          }
          int num = (int) Utils.Dialog((IWin32Window) this, "You can only select public" + str + "template for public loan template.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          string displayString = selectedItem.ToDisplayString();
          string name2 = standardIconButton.Name;
          // ISSUE: reference to a compiler-generated method
          switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(name2))
          {
            case 180049348:
              if (!(name2 == "stdBtnLoanProgram"))
                break;
              this.loanTxt.Text = displayString;
              break;
            case 383142185:
              if (!(name2 == "stdBtnTaskSet"))
                break;
              this.taskTxt.Text = displayString;
              break;
            case 620297425:
              if (!(name2 == "stdBtnClosingCosts"))
                break;
              this.costTxt.Text = displayString;
              break;
            case 1501647960:
              if (!(name2 == "stdBtnDataTemplate"))
                break;
              this.dataTxt.Text = displayString;
              break;
            case 3369964964:
              if (!(name2 == "stdBtnInputFormSet"))
                break;
              this.formTxt.Text = displayString;
              break;
            case 3521714678:
              if (!(name2 == "stdBtnAffiliates"))
                break;
              this.affiliateTxt.Text = displayString;
              break;
            case 3570426729:
              if (!(name2 == "stdBtnDocumentSet"))
                break;
              this.docTxt.Text = displayString;
              break;
            case 3648787760:
              if (!(name2 == "stdBtnProviders"))
                break;
              this.providerTxt.Text = displayString;
              break;
          }
        }
      }
    }

    private void clearBtn_Click(object sender, EventArgs e)
    {
      string name = ((Control) sender).Name;
      // ISSUE: reference to a compiler-generated method
      switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(name))
      {
        case 1207013403:
          if (!(name == "stdBtnDelLoanProgram"))
            break;
          this.loanTxt.Text = string.Empty;
          break;
        case 1335684276:
          if (!(name == "stdBtnDelClosingCosts"))
            break;
          this.costTxt.Text = string.Empty;
          break;
        case 1743504741:
          if (!(name == "stdBtnDelInputFormSet"))
            break;
          this.formTxt.Text = string.Empty;
          break;
        case 2381382829:
          if (!(name == "stdBtnDelMilestoneTemplate"))
            break;
          this.txtMilestoneTemplate.Text = string.Empty;
          break;
        case 3091942539:
          if (!(name == "stdBtnDelAffiliates"))
            break;
          this.affiliateTxt.Text = string.Empty;
          break;
        case 3584219770:
          if (!(name == "stdBtnDelDocumentSet"))
            break;
          this.docTxt.Text = string.Empty;
          break;
        case 3766046773:
          if (!(name == "stdBtnDelDataTemplate"))
            break;
          this.dataTxt.Text = string.Empty;
          break;
        case 4187045783:
          if (!(name == "stdBtnDelProviders"))
            break;
          this.providerTxt.Text = string.Empty;
          break;
        case 4235789502:
          if (!(name == "stdBtnDelTaskSet"))
            break;
          this.taskTxt.Text = string.Empty;
          break;
      }
    }

    private void Help_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F1)
        return;
      this.ShowHelp();
    }

    public void ShowHelp()
    {
      JedHelp.ShowHelp((Control) this, SystemSettings.HelpFile, nameof (LoanTemplateDialog));
    }

    private void brokerVersion()
    {
      this.label10.Visible = this.txtMilestoneTemplate.Visible = this.stdBtnMilestoneTemplate.Visible = this.stdBtnDelMilestoneTemplate.Visible = false;
      foreach (Control control1 in (ArrangedElementCollection) this.groupContainer1.Controls)
      {
        if (!(control1.Name == "label3") && !(control1.Name == "loanTxt") && !(control1.Name == "stdBtnLoanProgram") && !(control1.Name == "stdBtnDelLoanProgram"))
        {
          Control control2 = control1;
          Point location = control1.Location;
          int x = location.X;
          location = control1.Location;
          int y = location.Y - 24;
          Point point = new Point(x, y);
          control2.Location = point;
        }
      }
      GroupContainer groupContainer1 = this.groupContainer1;
      Size size1 = this.groupContainer1.Size;
      int width = size1.Width;
      size1 = this.groupContainer1.Size;
      int height = size1.Height - 24;
      Size size2 = new Size(width, height);
      groupContainer1.Size = size2;
      this.cancelBtn.Location = new Point(this.cancelBtn.Location.X, this.cancelBtn.Location.Y - 24);
      this.saveBtn.Location = new Point(this.saveBtn.Location.X, this.saveBtn.Location.Y - 24);
      this.ClientSize = new Size(this.ClientSize.Width, this.ClientSize.Height - 24);
    }
  }
}
