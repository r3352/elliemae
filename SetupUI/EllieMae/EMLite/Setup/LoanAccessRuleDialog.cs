// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.LoanAccessRuleDialog
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class LoanAccessRuleDialog : Form
  {
    private const string className = "LoanAccessRuleDialog";
    private Sessions.Session session;
    private bool hasNoAccess;
    private RuleConditionControl ruleCondForm;
    private Button okBtn;
    private Button cancelBtn;
    private TextBox textBoxName;
    private LoanAccessRights loanRights;
    private ControlListView emListViewRights;
    private ColumnHeader headerPersona;
    private ColumnHeader headerRights;
    private Label label3;
    private Label label2;
    private Label label1;
    private Panel panelCondition;
    private const string ALLPERSONAS = "All Personas";
    private EMHelpLink emHelpLink1;
    private Label label6;
    private Panel panelChannel;
    private ChannelConditionControl channelControl;
    private Panel panelSection4;
    private Panel panelDialog;
    private TextBox commentsTxt;
    private Label label7;
    private System.ComponentModel.Container components;
    private LoanAccessRuleInfo ruleInfo;

    public LoanAccessRuleDialog(Sessions.Session session, LoanAccessRuleInfo ruleInfo)
    {
      this.ruleInfo = ruleInfo;
      if (this.ruleInfo != null)
        this.loanRights = ruleInfo.GetLoanAccessRights();
      this.InitializeComponent();
      this.session = session;
      this.emHelpLink1.AssignSession(this.session);
      this.channelControl = new ChannelConditionControl();
      if (this.ruleInfo != null)
        this.channelControl.ChannelValue = this.ruleInfo.Condition2;
      this.panelChannel.Controls.Add((Control) this.channelControl);
      this.ruleCondForm = new RuleConditionControl(this.session);
      this.ruleCondForm.InitControl(BpmCategory.LoanAccess);
      this.panelCondition.Controls.Add((Control) this.ruleCondForm);
      this.initForm();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    public LoanAccessRuleInfo RuleInfo => this.ruleInfo;

    private void InitializeComponent()
    {
      this.okBtn = new Button();
      this.cancelBtn = new Button();
      this.textBoxName = new TextBox();
      this.emListViewRights = new ControlListView();
      this.headerPersona = new ColumnHeader();
      this.headerRights = new ColumnHeader();
      this.label3 = new Label();
      this.label2 = new Label();
      this.label1 = new Label();
      this.panelCondition = new Panel();
      this.emHelpLink1 = new EMHelpLink();
      this.label6 = new Label();
      this.panelChannel = new Panel();
      this.panelSection4 = new Panel();
      this.panelDialog = new Panel();
      this.commentsTxt = new TextBox();
      this.label7 = new Label();
      this.panelSection4.SuspendLayout();
      this.panelDialog.SuspendLayout();
      this.SuspendLayout();
      this.okBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.okBtn.Location = new Point(908, 554);
      this.okBtn.Name = "okBtn";
      this.okBtn.Size = new Size(75, 23);
      this.okBtn.TabIndex = 11;
      this.okBtn.Text = "&Save";
      this.okBtn.Click += new EventHandler(this.okBtn_Click);
      this.cancelBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(989, 554);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 23);
      this.cancelBtn.TabIndex = 12;
      this.cancelBtn.Text = "Cancel";
      this.textBoxName.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.textBoxName.Location = new Point(32, 30);
      this.textBoxName.MaxLength = 64;
      this.textBoxName.Name = "textBoxName";
      this.textBoxName.Size = new Size(677, 20);
      this.textBoxName.TabIndex = 1;
      this.emListViewRights.Columns.AddRange(new ColumnHeader[2]
      {
        this.headerPersona,
        this.headerRights
      });
      this.emListViewRights.GridLines = true;
      this.emListViewRights.HideSelection = false;
      this.emListViewRights.Location = new Point(13, 25);
      this.emListViewRights.Name = "emListViewRights";
      this.emListViewRights.Size = new Size(677, 190);
      this.emListViewRights.SubControlFont = new Font("Microsoft Sans Serif", 8f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.emListViewRights.TabIndex = 7;
      this.emListViewRights.UseCompatibleStateImageBehavior = false;
      this.emListViewRights.View = View.Details;
      this.emListViewRights.Resize += new EventHandler(this.emListViewRights_Resize);
      this.headerPersona.Text = "Persona";
      this.headerPersona.Width = 147;
      this.headerRights.Text = "Persona Access to Loans";
      this.headerRights.Width = 519;
      this.label3.AutoSize = true;
      this.label3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label3.Location = new Point(-3, 9);
      this.label3.Name = "label3";
      this.label3.Size = new Size(252, 13);
      this.label3.TabIndex = 6;
      this.label3.Text = "4. Define loan file access for each persona";
      this.label2.AutoSize = true;
      this.label2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(16, 182);
      this.label2.Name = "label2";
      this.label2.Size = new Size(193, 13);
      this.label2.TabIndex = 4;
      this.label2.Text = "3. Select a condition for this rule";
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(16, 16);
      this.label1.Name = "label1";
      this.label1.Size = new Size(136, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "1. Create a Rule Name";
      this.panelCondition.Location = new Point(32, 208);
      this.panelCondition.Name = "panelCondition";
      this.panelCondition.Size = new Size(677, 94);
      this.panelCondition.TabIndex = 5;
      this.emHelpLink1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.emHelpLink1.BackColor = Color.Transparent;
      this.emHelpLink1.Cursor = Cursors.Hand;
      this.emHelpLink1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.emHelpLink1.HelpTag = "Setup\\Persona Access to Loans";
      this.emHelpLink1.Location = new Point(10, 561);
      this.emHelpLink1.Name = "emHelpLink1";
      this.emHelpLink1.Size = new Size(90, 16);
      this.emHelpLink1.TabIndex = 10;
      this.label6.AutoSize = true;
      this.label6.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label6.Location = new Point(16, 64);
      this.label6.Name = "label6";
      this.label6.Size = new Size(239, 13);
      this.label6.TabIndex = 2;
      this.label6.Text = "2. Select all Channels this rule applies to";
      this.panelChannel.Location = new Point(32, 90);
      this.panelChannel.Name = "panelChannel";
      this.panelChannel.Size = new Size(677, 79);
      this.panelChannel.TabIndex = 3;
      this.panelSection4.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.panelSection4.Controls.Add((Control) this.emListViewRights);
      this.panelSection4.Controls.Add((Control) this.label3);
      this.panelSection4.Location = new Point(19, 320);
      this.panelSection4.Name = "panelSection4";
      this.panelSection4.Size = new Size(690, 235);
      this.panelSection4.TabIndex = 32;
      this.panelDialog.AutoScroll = true;
      this.panelDialog.Controls.Add((Control) this.commentsTxt);
      this.panelDialog.Controls.Add((Control) this.label7);
      this.panelDialog.Controls.Add((Control) this.cancelBtn);
      this.panelDialog.Controls.Add((Control) this.okBtn);
      this.panelDialog.Controls.Add((Control) this.panelSection4);
      this.panelDialog.Controls.Add((Control) this.textBoxName);
      this.panelDialog.Controls.Add((Control) this.panelChannel);
      this.panelDialog.Controls.Add((Control) this.label6);
      this.panelDialog.Controls.Add((Control) this.panelCondition);
      this.panelDialog.Controls.Add((Control) this.emHelpLink1);
      this.panelDialog.Controls.Add((Control) this.label1);
      this.panelDialog.Controls.Add((Control) this.label2);
      this.panelDialog.Location = new Point(2, 2);
      this.panelDialog.Name = "panelDialog";
      this.panelDialog.Size = new Size(1147, 580);
      this.panelDialog.TabIndex = 33;
      this.commentsTxt.Location = new Point(743, 30);
      this.commentsTxt.Multiline = true;
      this.commentsTxt.Name = "commentsTxt";
      this.commentsTxt.ScrollBars = ScrollBars.Vertical;
      this.commentsTxt.Size = new Size(364, 139);
      this.commentsTxt.TabIndex = 9;
      this.label7.AutoSize = true;
      this.label7.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label7.Location = new Point(740, 14);
      this.label7.Name = "label7";
      this.label7.Size = new Size(103, 13);
      this.label7.TabIndex = 8;
      this.label7.Text = "Notes/Comments";
      this.AutoScaleBaseSize = new Size(5, 13);
      this.AutoScroll = true;
      this.ClientSize = new Size(1152, 585);
      this.Controls.Add((Control) this.panelDialog);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (LoanAccessRuleDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Persona Access to Loans Business Rule";
      this.KeyDown += new KeyEventHandler(this.Help_KeyDown);
      this.panelSection4.ResumeLayout(false);
      this.panelSection4.PerformLayout();
      this.panelDialog.ResumeLayout(false);
      this.panelDialog.PerformLayout();
      this.ResumeLayout(false);
    }

    private void initForm()
    {
      this.hasNoAccess = !((FeaturesAclManager) this.session.ACL.GetAclManager(AclCategory.Features)).GetUserApplicationRight(AclFeature.SettingsTab_PersonaAccesstoLoans);
      this.okBtn.Enabled = !this.hasNoAccess;
      Persona[] allPersonas = this.session.PersonaManager.GetAllPersonas();
      this.emListViewRights.Items.Clear();
      this.emListViewRights.BeginUpdate();
      for (int index = 0; index < allPersonas.Length; ++index)
      {
        if (allPersonas[index].ID > 1)
          this.emListViewRights.Items.Add(new ListViewItem(allPersonas[index].Name)
          {
            SubItems = {
              ""
            },
            Tag = (object) allPersonas[index]
          });
      }
      this.emListViewRights.Items.Add(new ListViewItem("All Personas")
      {
        SubItems = {
          ""
        }
      });
      LoanFileRightsControl c = new LoanFileRightsControl(this.session, (Persona) null, BizRule.LoanAccessRight.EditAll, (string[]) null);
      c.AllPersonsClick += new EventHandler(this.rights_AllPersonsClick);
      this.emListViewRights.EndUpdate();
      if (this.ruleInfo == null)
      {
        for (int index = 0; index < this.emListViewRights.Items.Count - 1; ++index)
          this.emListViewRights.AddEmbeddedControl((Control) new LoanFileRightsControl(this.session, (Persona) this.emListViewRights.Items[index].Tag, BizRule.LoanAccessRight.EditAll, (string[]) null, this.hasNoAccess), 1, index, DockStyle.None);
        this.emListViewRights.AddEmbeddedControl((Control) c, 1, this.emListViewRights.Items.Count - 1, DockStyle.None);
      }
      else
      {
        for (int index = 0; index < this.emListViewRights.Items.Count - 1; ++index)
        {
          Persona tag = (Persona) this.emListViewRights.Items[index].Tag;
          BizRule.LoanAccessRight accessRight = (BizRule.LoanAccessRight) this.loanRights.GetAccessRight(tag.ID);
          string[] editableFields = this.loanRights.GetEditableFields(tag.ID);
          this.emListViewRights.AddEmbeddedControl((Control) new LoanFileRightsControl(this.session, tag, accessRight, editableFields, this.hasNoAccess), 1, index, DockStyle.Fill);
        }
        this.emListViewRights.AddEmbeddedControl((Control) c, 1, this.emListViewRights.Items.Count - 1, DockStyle.None);
        this.ruleCondForm.SetCondition((BizRuleInfo) this.ruleInfo);
        this.textBoxName.Text = this.ruleInfo.RuleName;
        if (this.ruleInfo.CommentsTxt.Contains("\n") && !this.ruleInfo.CommentsTxt.Contains(Environment.NewLine))
          this.commentsTxt.Text = this.ruleInfo.CommentsTxt.Replace("\n", Environment.NewLine);
        else
          this.commentsTxt.Text = this.ruleInfo.CommentsTxt;
        if (!this.hasNoAccess)
          return;
        this.textBoxName.Enabled = false;
        this.ruleCondForm.DisableControls();
        this.channelControl.DisableControls();
      }
    }

    private void rights_AllPersonsClick(object sender, EventArgs e)
    {
      ComboBox comboBox = (ComboBox) sender;
      if (comboBox == null)
        return;
      for (int row = 0; row < this.emListViewRights.Items.Count - 1; ++row)
        ((LoanFileRightsControl) this.emListViewRights.GetEmbeddedControl(1, row)).SetComboboxText(comboBox.Text);
      comboBox.Text = "";
    }

    private void okBtn_Click(object sender, EventArgs e)
    {
      if (this.textBoxName.Text == "")
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please enter a rule name.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        BizRuleInfo[] rules = ((BpmManager) this.session.BPM.GetBpmManager(BpmCategory.LoanAccess)).GetRules(this.ruleCondForm.IsGeneralRule, true);
        for (int index = 0; index < rules.Length; ++index)
        {
          if (string.Compare(this.textBoxName.Text.Trim(), rules[index].RuleName, StringComparison.OrdinalIgnoreCase) == 0)
          {
            bool flag = false;
            if (this.ruleInfo == null)
              flag = true;
            else if (this.ruleInfo.RuleID != rules[index].RuleID || this.ruleInfo.RuleID == 0)
              flag = true;
            if (flag)
            {
              int num2 = (int) Utils.Dialog((IWin32Window) this, "The rule name that you entered is already in use. Please try a different rule name.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              this.textBoxName.Focus();
              return;
            }
          }
        }
        if (!this.ruleCondForm.ValidateCondition())
          return;
        PersonaLoanAccessRight[] personaLoanAccessRightArray = new PersonaLoanAccessRight[this.emListViewRights.Items.Count - 1];
        for (int row = 0; row < this.emListViewRights.Items.Count - 1; ++row)
        {
          LoanFileRightsControl embeddedControl = (LoanFileRightsControl) this.emListViewRights.GetEmbeddedControl(1, row);
          personaLoanAccessRightArray[row] = new PersonaLoanAccessRight(embeddedControl.PersonaBelongTo, embeddedControl.LoanRights, embeddedControl.EditableFields);
        }
        this.ruleInfo = this.ruleInfo == null ? new LoanAccessRuleInfo(this.textBoxName.Text.Trim()) : new LoanAccessRuleInfo(this.ruleInfo.RuleID, this.textBoxName.Text.Trim());
        this.ruleInfo.LoanAccessRights = personaLoanAccessRightArray;
        this.ruleCondForm.ApplyCondition((BizRuleInfo) this.ruleInfo);
        this.ruleInfo.Condition2 = this.channelControl.ChannelValue;
        this.ruleInfo.CommentsTxt = this.commentsTxt.Text;
        this.DialogResult = DialogResult.OK;
      }
    }

    private void emListViewRights_Resize(object sender, EventArgs e)
    {
      int width = this.emListViewRights.ClientSize.Width;
      this.headerPersona.Width = (int) ((double) width * 0.3);
      this.headerRights.Width = (int) ((double) width * 0.7);
    }

    private void Help_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.F1)
      {
        this.ShowHelp();
      }
      else
      {
        if (e.KeyCode != Keys.Escape)
          return;
        this.cancelBtn.PerformClick();
      }
    }

    public void ShowHelp()
    {
      JedHelp.ShowHelp((Control) this, SystemSettings.HelpFile, nameof (LoanAccessRuleDialog));
    }
  }
}
