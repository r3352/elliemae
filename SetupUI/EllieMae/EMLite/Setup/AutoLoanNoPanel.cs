// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.AutoLoanNoPanel
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using EllieMae.EMLite.Workflow;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class AutoLoanNoPanel : SettingsUserControl
  {
    private TextBox suffixTxt;
    private Label label3;
    private Label label2;
    private Label label;
    private TextBox prefixTxt;
    private TextBox nextNumTxt;
    private System.ComponentModel.Container components;
    private CheckBox orgCodeChk;
    private CheckBox yearChk;
    private CheckBox monthChk;
    private Label label8;
    private Label label5;
    private bool initUseOrgCode;
    private bool initUseYear;
    private bool initUseMonth;
    private string initPrefix = "";
    private string initSuffix = "";
    private string initNextNum = "";
    private string initOrgCode = "";
    private Label labelSE;
    private Button branchBtn;
    private Label label4;
    private IEnumerable<EllieMae.EMLite.Workflow.Milestone> milestoneList;
    private GroupContainer gContainer;
    private MilestoneDropdownBox cmbWSLN;
    private Label lblExample;
    private Button btnManageExceptions;
    private string currentSetting = "";
    private bool suspendEvent;
    private string priorMilestoneSelection = "";
    private Dictionary<MilestoneTemplate, string> updatedExceptionList;
    private Hashtable defaultSettings;

    public AutoLoanNoPanel(SetUpContainer setupContainer)
      : base(setupContainer)
    {
      this.suspendEvent = true;
      this.InitializeComponent();
      this.defaultSettings = Session.SessionObjects.BpmManager.GetMilestoneTemplateDefaultSettings();
      this.milestoneList = Session.SessionObjects.BpmManager.GetMilestones(true);
      this.cmbWSLN.PopulateAllMilestones(this.milestoneList, true, false);
      this.resetMilestoneControl();
      if (Session.EncompassEdition == EncompassEdition.Broker)
        this.btnManageExceptions.Visible = false;
      LoanNumberingInfo loanNumberingInfo = Session.ConfigurationManager.GetLoanNumberingInfo();
      this.initPrefix = loanNumberingInfo.Prefix;
      this.initSuffix = loanNumberingInfo.Suffix;
      this.initNextNum = loanNumberingInfo.NextNumber;
      this.initUseOrgCode = loanNumberingInfo.UseOrgCode;
      this.initUseYear = loanNumberingInfo.UseYear;
      this.initUseMonth = loanNumberingInfo.UseMonth;
      this.initOrgCode = this.getOrgCode();
      this.reset();
      this.suspendEvent = false;
    }

    private void resetMilestoneControl()
    {
      this.currentSetting = string.Concat(Session.ServerManager.GetServerSetting("Policies.LoanNumber"));
      this.cmbWSLN.MilestoneID = this.currentSetting;
      this.priorMilestoneSelection = this.currentSetting;
    }

    private void reset()
    {
      this.orgCodeChk.Checked = this.initUseOrgCode;
      this.yearChk.Checked = this.initUseYear;
      this.monthChk.Checked = this.initUseMonth;
      this.prefixTxt.Text = this.initPrefix;
      this.suffixTxt.Text = this.initSuffix;
      this.nextNumTxt.Text = this.initNextNum;
      this.resetMilestoneControl();
      this.setDirtyFlag(false);
      this.createExample();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.label4 = new Label();
      this.label5 = new Label();
      this.label8 = new Label();
      this.monthChk = new CheckBox();
      this.yearChk = new CheckBox();
      this.orgCodeChk = new CheckBox();
      this.labelSE = new Label();
      this.suffixTxt = new TextBox();
      this.label3 = new Label();
      this.label2 = new Label();
      this.nextNumTxt = new TextBox();
      this.prefixTxt = new TextBox();
      this.label = new Label();
      this.branchBtn = new Button();
      this.gContainer = new GroupContainer();
      this.cmbWSLN = new MilestoneDropdownBox();
      this.lblExample = new Label();
      this.btnManageExceptions = new Button();
      this.gContainer.SuspendLayout();
      this.SuspendLayout();
      this.label4.Location = new Point(7, 269);
      this.label4.Name = "label4";
      this.label4.Size = new Size(163, 22);
      this.label4.TabIndex = 27;
      this.label4.Text = "When to Start Loan Numbering";
      this.label5.Location = new Point(332, 176);
      this.label5.Name = "label5";
      this.label5.Size = new Size(124, 24);
      this.label5.TabIndex = 25;
      this.label5.Text = "(Max. 9 digits) ";
      this.label8.Location = new Point(7, 223);
      this.label8.Name = "label8";
      this.label8.Size = new Size(120, 24);
      this.label8.TabIndex = 23;
      this.label8.Text = "Example";
      this.monthChk.Location = new Point(172, 120);
      this.monthChk.Name = "monthChk";
      this.monthChk.Size = new Size(168, 16);
      this.monthChk.TabIndex = 3;
      this.monthChk.Text = "Use Month";
      this.monthChk.CheckedChanged += new EventHandler(this.monthChk_CheckedChanged);
      this.yearChk.Location = new Point(172, 98);
      this.yearChk.Name = "yearChk";
      this.yearChk.Size = new Size(168, 16);
      this.yearChk.TabIndex = 2;
      this.yearChk.Text = "Use Year";
      this.yearChk.CheckedChanged += new EventHandler(this.yearChk_CheckedChanged);
      this.orgCodeChk.Location = new Point(172, 72);
      this.orgCodeChk.Name = "orgCodeChk";
      this.orgCodeChk.Size = new Size(168, 20);
      this.orgCodeChk.TabIndex = 1;
      this.orgCodeChk.Text = "Use Organization Code";
      this.orgCodeChk.CheckedChanged += new EventHandler(this.orgCodeChk_CheckedChanged);
      this.labelSE.Location = new Point(7, 36);
      this.labelSE.Name = "labelSE";
      this.labelSE.Size = new Size(500, 33);
      this.labelSE.TabIndex = 18;
      this.labelSE.Text = "The maximum loan number length is 18 characters, however, Fannie Mae accepts only 15 characters. Set up the Organization Code on the Organization/Users screen.";
      this.suffixTxt.Location = new Point(172, 197);
      this.suffixTxt.MaxLength = 18;
      this.suffixTxt.Name = "suffixTxt";
      this.suffixTxt.Size = new Size(160, 20);
      this.suffixTxt.TabIndex = 6;
      this.suffixTxt.TextChanged += new EventHandler(this.TextChange);
      this.suffixTxt.KeyPress += new KeyPressEventHandler(this.suffixTxt_KeyPress);
      this.suffixTxt.KeyUp += new KeyEventHandler(this.keyup);
      this.label3.Location = new Point(7, 201);
      this.label3.Name = "label3";
      this.label3.Size = new Size(120, 20);
      this.label3.TabIndex = 17;
      this.label3.Text = "Loan Number Suffix";
      this.label2.Location = new Point(7, 176);
      this.label2.Name = "label2";
      this.label2.Size = new Size(120, 22);
      this.label2.TabIndex = 16;
      this.label2.Text = "Next Number";
      this.nextNumTxt.Location = new Point(172, 172);
      this.nextNumTxt.MaxLength = 9;
      this.nextNumTxt.Name = "nextNumTxt";
      this.nextNumTxt.Size = new Size(160, 20);
      this.nextNumTxt.TabIndex = 5;
      this.nextNumTxt.TextChanged += new EventHandler(this.TextChange);
      this.nextNumTxt.KeyPress += new KeyPressEventHandler(this.nextNumTxt_KeyPress);
      this.nextNumTxt.KeyUp += new KeyEventHandler(this.keyup);
      this.prefixTxt.Location = new Point(172, 147);
      this.prefixTxt.MaxLength = 18;
      this.prefixTxt.Name = "prefixTxt";
      this.prefixTxt.Size = new Size(160, 20);
      this.prefixTxt.TabIndex = 4;
      this.prefixTxt.TextChanged += new EventHandler(this.TextChange);
      this.prefixTxt.KeyPress += new KeyPressEventHandler(this.prefixTxt_KeyPress);
      this.prefixTxt.KeyUp += new KeyEventHandler(this.keyup);
      this.label.Location = new Point(7, 150);
      this.label.Name = "label";
      this.label.Size = new Size(120, 22);
      this.label.TabIndex = 13;
      this.label.Text = "Loan Number Prefix";
      this.branchBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.branchBtn.BackColor = SystemColors.Control;
      this.branchBtn.Font = new Font("Microsoft Sans Serif", 11f, FontStyle.Regular, GraphicsUnit.Pixel, (byte) 0);
      this.branchBtn.Location = new Point(424, 2);
      this.branchBtn.Name = "branchBtn";
      this.branchBtn.Size = new Size(184, 22);
      this.branchBtn.TabIndex = 9;
      this.branchBtn.Text = "&Organization Loan Numbering...";
      this.branchBtn.UseVisualStyleBackColor = true;
      this.branchBtn.Click += new EventHandler(this.branchBtn_Click);
      this.gContainer.Controls.Add((Control) this.btnManageExceptions);
      this.gContainer.Controls.Add((Control) this.cmbWSLN);
      this.gContainer.Controls.Add((Control) this.lblExample);
      this.gContainer.Controls.Add((Control) this.branchBtn);
      this.gContainer.Controls.Add((Control) this.labelSE);
      this.gContainer.Controls.Add((Control) this.label4);
      this.gContainer.Controls.Add((Control) this.prefixTxt);
      this.gContainer.Controls.Add((Control) this.label5);
      this.gContainer.Controls.Add((Control) this.label);
      this.gContainer.Controls.Add((Control) this.nextNumTxt);
      this.gContainer.Controls.Add((Control) this.label8);
      this.gContainer.Controls.Add((Control) this.label2);
      this.gContainer.Controls.Add((Control) this.monthChk);
      this.gContainer.Controls.Add((Control) this.label3);
      this.gContainer.Controls.Add((Control) this.yearChk);
      this.gContainer.Controls.Add((Control) this.suffixTxt);
      this.gContainer.Controls.Add((Control) this.orgCodeChk);
      this.gContainer.Dock = DockStyle.Fill;
      this.gContainer.HeaderForeColor = SystemColors.ControlText;
      this.gContainer.Location = new Point(0, 0);
      this.gContainer.Name = "gContainer";
      this.gContainer.Size = new Size(614, 334);
      this.gContainer.TabIndex = 12;
      this.gContainer.Text = "Auto Loan Number";
      this.cmbWSLN.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cmbWSLN.Location = new Point(172, 269);
      this.cmbWSLN.Name = "cmbWSLN";
      this.cmbWSLN.Size = new Size(209, 22);
      this.cmbWSLN.TabIndex = 30;
      this.cmbWSLN.SelectedIndexChanged += new EventHandler(this.cmbWSLN_SelectedIndexChanged);
      this.lblExample.AutoSize = true;
      this.lblExample.Location = new Point(169, 223);
      this.lblExample.Name = "lblExample";
      this.lblExample.Size = new Size(91, 13);
      this.lblExample.TabIndex = 29;
      this.lblExample.Text = "YYMMEM000032";
      this.btnManageExceptions.Location = new Point(387, 268);
      this.btnManageExceptions.Name = "btnManageExceptions";
      this.btnManageExceptions.Size = new Size(120, 23);
      this.btnManageExceptions.TabIndex = 31;
      this.btnManageExceptions.Text = "Manage Exceptions...";
      this.btnManageExceptions.UseVisualStyleBackColor = true;
      this.btnManageExceptions.Click += new EventHandler(this.btnManageExceptions_Click);
      this.Controls.Add((Control) this.gContainer);
      this.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (AutoLoanNoPanel);
      this.Size = new Size(614, 334);
      this.gContainer.ResumeLayout(false);
      this.gContainer.PerformLayout();
      this.ResumeLayout(false);
    }

    private void nextNumTxt_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar.Equals((object) Keys.Delete) || char.IsControl(e.KeyChar))
      {
        this.setDirtyFlag(true);
        this.createExample();
      }
      else if (char.IsDigit(e.KeyChar))
        this.setDirtyFlag(true);
      else
        e.Handled = true;
    }

    private void prefixTxt_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar.Equals((object) Keys.Delete) || char.IsControl(e.KeyChar))
      {
        this.setDirtyFlag(true);
        this.createExample();
      }
      else if (char.IsLetterOrDigit(e.KeyChar))
        this.setDirtyFlag(true);
      else
        e.Handled = true;
    }

    private void suffixTxt_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar.Equals((object) Keys.Delete) || char.IsControl(e.KeyChar))
      {
        this.setDirtyFlag(true);
        this.createExample();
      }
      else if (char.IsLetterOrDigit(e.KeyChar))
        this.setDirtyFlag(true);
      else
        e.Handled = true;
    }

    public override void Reset()
    {
      this.suspendEvent = true;
      this.reset();
      this.setDirtyFlag(false);
      this.suspendEvent = false;
    }

    public override void Save()
    {
      if (this.nextNumTxt.Text.Trim() == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The Next Number cannot be blank.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.nextNumTxt.Focus();
      }
      else if (!this.IsDirty)
      {
        this.Dispose();
      }
      else
      {
        if (Utils.Dialog((IWin32Window) this, "When changing Auto Loan Numbering settings, be sure to use a unique number configuration. Failure to do so may result in loan numbers that have already been assigned to existing loans being assigned to new loans.", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.OK)
          return;
        Session.ConfigurationManager.UpdateLoanNumberingInfo(new LoanNumberingInfo(this.prefixTxt.Text, this.nextNumTxt.Text, this.suffixTxt.Text, this.orgCodeChk.Checked, this.monthChk.Checked, this.yearChk.Checked));
        Session.ServerManager.UpdateServerSetting("Policies.LoanNumber", (object) this.cmbWSLN.MilestoneID);
        if (this.updatedExceptionList != null)
          Session.SessionObjects.BpmManager.UpdateMilestoneTemplateImpactedAreaSettings(this.updatedExceptionList, "LoanNumbering");
        this.setDirtyFlag(false);
      }
    }

    private string createExample()
    {
      string str = string.Empty;
      if (this.orgCodeChk.Checked)
        str = this.initOrgCode;
      if (this.yearChk.Checked)
        str += "YY";
      if (this.monthChk.Checked)
        str += "MM";
      string example = str + this.prefixTxt.Text.Trim() + this.nextNumTxt.Text.Trim() + this.suffixTxt.Text.Trim();
      this.lblExample.Text = example;
      return example;
    }

    private void orgCodeChk_CheckedChanged(object sender, EventArgs e)
    {
      if (this.createExample().Length > 18)
        this.orgCodeChk.Checked = false;
      this.setDirtyFlag(true);
    }

    private void yearChk_CheckedChanged(object sender, EventArgs e)
    {
      if (this.createExample().Length > 18)
        this.yearChk.Checked = false;
      this.setDirtyFlag(true);
    }

    private void monthChk_CheckedChanged(object sender, EventArgs e)
    {
      if (this.createExample().Length > 18)
        this.monthChk.Checked = false;
      this.setDirtyFlag(true);
    }

    private string getOrgCode()
    {
      OrgInfo avaliableOrganization = Session.OrganizationManager.GetFirstAvaliableOrganization(Session.UserInfo.OrgId);
      return avaliableOrganization == null ? string.Empty : avaliableOrganization.OrgCode.Trim();
    }

    private void keyup(object sender, KeyEventArgs e)
    {
      if (this.createExample().Length > 18)
      {
        if (sender != null && sender is TextBox)
        {
          TextBox textBox = (TextBox) sender;
          string text = textBox.Text;
          if (text != string.Empty)
          {
            textBox.Text = text.Substring(0, text.Length - 1);
            textBox.SelectionStart = textBox.Text.Trim().Length;
          }
        }
      }
      this.createExample();
    }

    private void TextChange(object sender, EventArgs e)
    {
      this.setDirtyFlag(true);
      this.createExample();
    }

    private void branchBtn_Click(object sender, EventArgs e)
    {
      int num = (int) new BranchLoanNumberDialog().ShowDialog((IWin32Window) this);
    }

    private void cmbWSLN_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.suspendEvent)
        return;
      Dictionary<MilestoneTemplate, bool> exceptionList = AutoLoanNoPanel.getExceptionMilestoneTemplateList(this.cmbWSLN.MilestoneID, "POLICIES.LOANNUMBER");
      if (exceptionList.ContainsValue(false))
      {
        CustomMilestoneExceptionNotification exceptionNotification = new CustomMilestoneExceptionNotification(exceptionList, "AutoLoanNumber", string.Concat(this.defaultSettings[(object) "POLICIES.LOANNUMBER"]));
        if (exceptionNotification.ShowDialog() == DialogResult.Yes)
        {
          Dictionary<MilestoneTemplate, string> exceptions = new Dictionary<MilestoneTemplate, string>();
          exceptionList.Keys.ToList<MilestoneTemplate>().ForEach((Action<MilestoneTemplate>) (x =>
          {
            if (exceptionList[x])
              exceptions.Add(x, x.AutoLoanNumberingMilestoneID);
            else
              exceptions.Add(x, (string) null);
          }));
          CustomMilestoneExceptionResolver exceptionResolver = new CustomMilestoneExceptionResolver(exceptions, this.cmbWSLN.MilestoneID, string.Concat(this.defaultSettings[(object) "POLICIES.LOANNUMBER"]), "AutoLoanNumber");
          if (!exceptionNotification.ResolveIssue || DialogResult.OK == exceptionResolver.ShowDialog())
            this.updatedExceptionList = exceptionResolver.ExceptionSettings();
        }
        else
        {
          this.suspendEvent = true;
          this.cmbWSLN.MilestoneID = this.priorMilestoneSelection;
          this.suspendEvent = false;
        }
      }
      this.priorMilestoneSelection = this.cmbWSLN.MilestoneID;
      this.setDirtyFlag(true);
    }

    private static Dictionary<MilestoneTemplate, bool> getExceptionMilestoneTemplateList(
      string selectedMilestoneID,
      string impactedArea)
    {
      Dictionary<MilestoneTemplate, bool> result = new Dictionary<MilestoneTemplate, bool>();
      IEnumerable<MilestoneTemplate> milestoneTemplates = Session.SessionObjects.BpmManager.GetMilestoneTemplates(true);
      if (impactedArea == "POLICIES.LOANNUMBER")
        milestoneTemplates.ToList<MilestoneTemplate>().ForEach((Action<MilestoneTemplate>) (x =>
        {
          if (x.AutoLoanNumberingMilestoneID != string.Empty)
          {
            result.Add(x, true);
          }
          else
          {
            if (x.SequentialMilestones.FirstOrDefault<TemplateMilestone>((Func<TemplateMilestone, bool>) (y => y.MilestoneID == selectedMilestoneID)) != null)
              return;
            result.Add(x, false);
          }
        }));
      return result;
    }

    private void btnManageExceptions_Click(object sender, EventArgs e)
    {
      CustomMilestoneExceptionResolver exceptionResolver;
      if (this.updatedExceptionList != null)
      {
        exceptionResolver = new CustomMilestoneExceptionResolver(this.updatedExceptionList, this.cmbWSLN.MilestoneID, string.Concat(this.defaultSettings[(object) "POLICIES.LOANNUMBER"]), "AutoLoanNumber");
      }
      else
      {
        Dictionary<MilestoneTemplate, bool> exceptionList = AutoLoanNoPanel.getExceptionMilestoneTemplateList(this.cmbWSLN.MilestoneID, "POLICIES.LOANNUMBER");
        Dictionary<MilestoneTemplate, string> exceptions = new Dictionary<MilestoneTemplate, string>();
        exceptionList.Keys.ToList<MilestoneTemplate>().ForEach((Action<MilestoneTemplate>) (x =>
        {
          if (exceptionList[x])
            exceptions.Add(x, x.AutoLoanNumberingMilestoneID);
          else
            exceptions.Add(x, (string) null);
        }));
        exceptionResolver = new CustomMilestoneExceptionResolver(exceptions, this.cmbWSLN.MilestoneID, string.Concat(this.defaultSettings[(object) "POLICIES.LOANNUMBER"]), "AutoLoanNumber");
      }
      if (exceptionResolver == null || DialogResult.OK != exceptionResolver.ShowDialog())
        return;
      this.updatedExceptionList = exceptionResolver.ExceptionSettings();
      this.setDirtyFlag(true);
    }
  }
}
