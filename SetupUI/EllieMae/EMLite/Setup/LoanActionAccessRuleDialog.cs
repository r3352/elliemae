// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.LoanActionAccessRuleDialog
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
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class LoanActionAccessRuleDialog : Form
  {
    private const string className = "LoanActionAccessRuleDialog";
    private Sessions.Session session;
    private bool hasNoAccess;
    private RuleConditionControl ruleCondForm;
    private Button okBtn;
    private Button cancelBtn;
    private TextBox textBoxName;
    private System.ComponentModel.Container components;
    private Persona[] personaList;
    private Label label4;
    private Label label5;
    private Panel panelCondition;
    private const string ALLPERSONAS = "All Personas";
    private EMHelpLink emHelpLink1;
    private Label label6;
    private Panel panelChannel;
    private ArrayList curSelection;
    private Panel panel2;
    private TextBox commentsTxt;
    private Label label7;
    private Panel panel1;
    private Label label3;
    private GridView gridViewRights;
    private Label label2;
    private GridView gridViewLoanActions;
    private Label label1;
    private Button removeBtn;
    private Button addBtn;
    private ChannelConditionControl channelControl;
    private LoanActionAccessRuleInfo ruleInfo;
    private bool waitForNextSelection;
    private bool shiftKeyPressed;
    private bool endKeyPressed;
    private bool homeKeyPressed;

    public LoanActionAccessRuleDialog(Sessions.Session session, LoanActionAccessRuleInfo ruleInfo)
    {
      this.ruleInfo = ruleInfo;
      this.InitializeComponent();
      this.session = session;
      this.emHelpLink1.AssignSession(this.session);
      this.channelControl = new ChannelConditionControl();
      if (this.ruleInfo != null)
        this.channelControl.ChannelValue = this.ruleInfo.Condition2;
      this.panelChannel.Controls.Add((Control) this.channelControl);
      this.ruleCondForm = new RuleConditionControl(this.session);
      this.ruleCondForm.InitControl(BpmCategory.LoanActionAccess);
      this.panelCondition.Controls.Add((Control) this.ruleCondForm);
      this.initForm();
      this.hasNoAccess = !((FeaturesAclManager) session.ACL.GetAclManager(AclCategory.Features)).GetUserApplicationRight(AclFeature.SettingsTab_PersonaAccesstoLoanActions);
      this.addBtn.Enabled = this.removeBtn.Enabled = this.okBtn.Enabled = !this.hasNoAccess;
      if (!this.hasNoAccess)
        return;
      this.textBoxName.Enabled = false;
      this.ruleCondForm.DisableControls();
      this.channelControl.DisableControls();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    public LoanActionAccessRuleInfo RuleInfo => this.ruleInfo;

    private void InitializeComponent()
    {
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      this.okBtn = new Button();
      this.cancelBtn = new Button();
      this.textBoxName = new TextBox();
      this.label4 = new Label();
      this.label5 = new Label();
      this.panelCondition = new Panel();
      this.emHelpLink1 = new EMHelpLink();
      this.label6 = new Label();
      this.panelChannel = new Panel();
      this.panel2 = new Panel();
      this.panel1 = new Panel();
      this.label3 = new Label();
      this.gridViewRights = new GridView();
      this.label2 = new Label();
      this.gridViewLoanActions = new GridView();
      this.label1 = new Label();
      this.removeBtn = new Button();
      this.addBtn = new Button();
      this.commentsTxt = new TextBox();
      this.label7 = new Label();
      this.panel2.SuspendLayout();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      this.okBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.okBtn.Location = new Point(708, 481);
      this.okBtn.Name = "okBtn";
      this.okBtn.Size = new Size(75, 23);
      this.okBtn.TabIndex = 14;
      this.okBtn.Text = "&Save";
      this.okBtn.Click += new EventHandler(this.okBtn_Click);
      this.cancelBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(789, 481);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 23);
      this.cancelBtn.TabIndex = 15;
      this.cancelBtn.Text = "Cancel";
      this.textBoxName.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.textBoxName.Location = new Point(32, 30);
      this.textBoxName.MaxLength = 64;
      this.textBoxName.Name = "textBoxName";
      this.textBoxName.Size = new Size(615, 20);
      this.textBoxName.TabIndex = 1;
      this.label4.AutoSize = true;
      this.label4.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label4.Location = new Point(16, 171);
      this.label4.Name = "label4";
      this.label4.Size = new Size(193, 13);
      this.label4.TabIndex = 4;
      this.label4.Text = "3. Select a condition for this rule";
      this.label5.AutoSize = true;
      this.label5.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label5.Location = new Point(16, 12);
      this.label5.Name = "label5";
      this.label5.Size = new Size(136, 13);
      this.label5.TabIndex = 0;
      this.label5.Text = "1. Create a Rule Name";
      this.panelCondition.BackColor = SystemColors.Control;
      this.panelCondition.Location = new Point(32, 187);
      this.panelCondition.Name = "panelCondition";
      this.panelCondition.Size = new Size(723, 122);
      this.panelCondition.TabIndex = 5;
      this.emHelpLink1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.emHelpLink1.BackColor = Color.Transparent;
      this.emHelpLink1.Cursor = Cursors.Hand;
      this.emHelpLink1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.emHelpLink1.HelpTag = "Setup\\Persona Access to Loan Actions";
      this.emHelpLink1.Location = new Point(12, 488);
      this.emHelpLink1.Name = "emHelpLink1";
      this.emHelpLink1.Size = new Size(90, 16);
      this.emHelpLink1.TabIndex = 13;
      this.label6.AutoSize = true;
      this.label6.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label6.Location = new Point(16, 64);
      this.label6.Name = "label6";
      this.label6.Size = new Size(239, 13);
      this.label6.TabIndex = 2;
      this.label6.Text = "2. Select all Channels this rule applies to";
      this.panelChannel.Location = new Point(35, 80);
      this.panelChannel.Name = "panelChannel";
      this.panelChannel.Size = new Size(612, 79);
      this.panelChannel.TabIndex = 3;
      this.panel2.AutoScroll = true;
      this.panel2.Controls.Add((Control) this.panel1);
      this.panel2.Controls.Add((Control) this.commentsTxt);
      this.panel2.Controls.Add((Control) this.label7);
      this.panel2.Controls.Add((Control) this.cancelBtn);
      this.panel2.Controls.Add((Control) this.okBtn);
      this.panel2.Controls.Add((Control) this.label4);
      this.panel2.Controls.Add((Control) this.textBoxName);
      this.panel2.Controls.Add((Control) this.panelChannel);
      this.panel2.Controls.Add((Control) this.label5);
      this.panel2.Controls.Add((Control) this.label6);
      this.panel2.Controls.Add((Control) this.panelCondition);
      this.panel2.Controls.Add((Control) this.emHelpLink1);
      this.panel2.Location = new Point(0, 0);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(917, 550);
      this.panel2.TabIndex = 33;
      this.panel1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.panel1.Controls.Add((Control) this.label3);
      this.panel1.Controls.Add((Control) this.gridViewRights);
      this.panel1.Controls.Add((Control) this.label2);
      this.panel1.Controls.Add((Control) this.gridViewLoanActions);
      this.panel1.Controls.Add((Control) this.label1);
      this.panel1.Controls.Add((Control) this.removeBtn);
      this.panel1.Controls.Add((Control) this.addBtn);
      this.panel1.Location = new Point(12, 296);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(660, 186);
      this.panel1.TabIndex = 34;
      this.label3.AutoSize = true;
      this.label3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label3.Location = new Point(4, 12);
      this.label3.Name = "label3";
      this.label3.Size = new Size(341, 13);
      this.label3.TabIndex = 6;
      this.label3.Text = "4. Define a persona's Loan Action access for the condition";
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Persona";
      gvColumn1.Width = 200;
      gvColumn2.ActivatedEditorType = GVActivatedEditorType.ComboBox;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.SpringToFit = true;
      gvColumn2.Text = "Rights";
      gvColumn2.Width = 79;
      this.gridViewRights.Columns.AddRange(new GVColumn[2]
      {
        gvColumn1,
        gvColumn2
      });
      this.gridViewRights.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gridViewRights.Location = new Point(376, 41);
      this.gridViewRights.Name = "gridViewRights";
      this.gridViewRights.Size = new Size(281, 142);
      this.gridViewRights.SortIconVisible = false;
      this.gridViewRights.SortOption = GVSortOption.None;
      this.gridViewRights.TabIndex = 10;
      this.gridViewRights.EditorOpening += new GVSubItemEditingEventHandler(this.gridViewRights_EditorOpening);
      this.gridViewRights.EditorClosing += new GVSubItemEditingEventHandler(this.gridViewRights_EditorClosing);
      this.label2.AutoSize = true;
      this.label2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(376, 28);
      this.label2.Name = "label2";
      this.label2.Size = new Size(66, 13);
      this.label2.TabIndex = 18;
      this.label2.Text = "Assign Right";
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column1";
      gvColumn3.Text = "Loan Action";
      gvColumn3.Width = 240;
      this.gridViewLoanActions.Columns.AddRange(new GVColumn[1]
      {
        gvColumn3
      });
      this.gridViewLoanActions.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gridViewLoanActions.Location = new Point(20, 44);
      this.gridViewLoanActions.Name = "gridViewLoanActions";
      this.gridViewLoanActions.Size = new Size(277, 142);
      this.gridViewLoanActions.TabIndex = 7;
      this.gridViewLoanActions.SelectedIndexChanged += new EventHandler(this.gridViewLoanActions_SelectedIndexChanged);
      this.gridViewLoanActions.KeyDown += new KeyEventHandler(this.gridViewLoanActions_KeyDown);
      this.gridViewLoanActions.ColumnClick += new GVColumnClickEventHandler(this.gridViewLoanActions_ColumnClick);
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(20, 28);
      this.label1.Name = "label1";
      this.label1.Size = new Size(74, 13);
      this.label1.TabIndex = 12;
      this.label1.Text = "Add an Action";
      this.removeBtn.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.removeBtn.Location = new Point(308, 73);
      this.removeBtn.Name = "removeBtn";
      this.removeBtn.Size = new Size(56, 23);
      this.removeBtn.TabIndex = 9;
      this.removeBtn.Text = "&Remove";
      this.removeBtn.Click += new EventHandler(this.removeBtn_Click);
      this.addBtn.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.addBtn.Location = new Point(308, 44);
      this.addBtn.Name = "addBtn";
      this.addBtn.Size = new Size(56, 23);
      this.addBtn.TabIndex = 8;
      this.addBtn.Text = "&Add";
      this.addBtn.Click += new EventHandler(this.addBtn_Click);
      this.commentsTxt.Location = new Point(655, 30);
      this.commentsTxt.Multiline = true;
      this.commentsTxt.Name = "commentsTxt";
      this.commentsTxt.ScrollBars = ScrollBars.Vertical;
      this.commentsTxt.Size = new Size(257, 138);
      this.commentsTxt.TabIndex = 12;
      this.label7.AutoSize = true;
      this.label7.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label7.Location = new Point(652, 14);
      this.label7.Name = "label7";
      this.label7.Size = new Size(103, 13);
      this.label7.TabIndex = 11;
      this.label7.Text = "Notes/Comments";
      this.AutoScaleBaseSize = new Size(5, 13);
      this.AutoScroll = true;
      this.ClientSize = new Size(936, 546);
      this.Controls.Add((Control) this.panel2);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (LoanActionAccessRuleDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Persona Access to Loan Actions";
      this.KeyDown += new KeyEventHandler(this.Help_KeyDown);
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.ResumeLayout(false);
    }

    private void initForm()
    {
      this.personaList = this.session.PersonaManager.GetAllPersonas();
      this.gridViewRights.Items.Clear();
      this.gridViewRights.BeginUpdate();
      for (int index = 0; index < this.personaList.Length; ++index)
      {
        if (this.personaList[index].ID != 0)
          this.gridViewRights.Items.Add(new GVItem(this.personaList[index].Name)
          {
            SubItems = {
              (object) ""
            },
            Tag = (object) this.personaList[index]
          });
      }
      this.gridViewRights.Items.Add(new GVItem("All Personas")
      {
        SubItems = {
          (object) ""
        }
      });
      this.gridViewRights.EndUpdate();
      this.gridViewLoanActions.Items.Clear();
      if (this.ruleInfo == null)
        return;
      this.gridViewLoanActions.BeginUpdate();
      TriggerActivationNameProvider activationNameProvider = new TriggerActivationNameProvider();
      for (int index = 0; index < this.ruleInfo.LoanActionAccessRights.Length; ++index)
      {
        TriggerActivationType triggerActivationType = (TriggerActivationType) Enum.Parse(typeof (TriggerActivationType), this.ruleInfo.LoanActionAccessRights[index].LoanActionID);
        this.gridViewLoanActions.Items.Add(new GVItem(activationNameProvider.GetDescriptionFromActivationType(triggerActivationType))
        {
          Tag = (object) this.ruleInfo.LoanActionAccessRights[index]
        });
      }
      this.gridViewLoanActions.EndUpdate();
      this.curSelection = new ArrayList();
      if (this.gridViewLoanActions.Items.Count > 0)
        this.gridViewLoanActions.Items[0].Selected = true;
      else
        this.gridViewRights.Enabled = false;
      this.ruleCondForm.SetCondition((BizRuleInfo) this.ruleInfo);
      this.textBoxName.Text = this.ruleInfo.RuleName;
      if (this.ruleInfo.CommentsTxt.Contains("\n") && !this.ruleInfo.CommentsTxt.Contains(Environment.NewLine))
        this.commentsTxt.Text = this.ruleInfo.CommentsTxt.Replace("\n", Environment.NewLine);
      else
        this.commentsTxt.Text = this.ruleInfo.CommentsTxt;
    }

    private void gridViewLoanActions_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.gridViewLoanActions.SelectedItems.Count == 0)
        return;
      if ((this.endKeyPressed || this.homeKeyPressed) && this.shiftKeyPressed)
      {
        if (this.waitForNextSelection)
        {
          if (this.gridViewLoanActions.SelectedItems.Count > 1)
            return;
        }
        else if (this.homeKeyPressed)
        {
          if (this.curSelection.Count > 0 && this.gridViewLoanActions.SelectedItems.Count < (int) this.curSelection[0] + 1)
            return;
        }
        else if (this.endKeyPressed && !this.gridViewLoanActions.Items[this.gridViewLoanActions.Items.Count - 1].Selected)
          return;
      }
      this.gridViewLoanActions.SelectedIndexChanged -= new EventHandler(this.gridViewLoanActions_SelectedIndexChanged);
      this.gridViewRights.Enabled = true;
      this.setListViewTag();
      this.curSelection = new ArrayList();
      for (int nItemIndex = 0; nItemIndex < this.gridViewLoanActions.Items.Count; ++nItemIndex)
      {
        if (this.gridViewLoanActions.Items[nItemIndex].Selected)
          this.curSelection.Add((object) nItemIndex);
      }
      this.checkPersonaAccessRights();
      this.gridViewLoanActions.SelectedIndexChanged += new EventHandler(this.gridViewLoanActions_SelectedIndexChanged);
      if (this.waitForNextSelection && this.shiftKeyPressed && (this.endKeyPressed || this.homeKeyPressed))
      {
        this.waitForNextSelection = false;
        this.endKeyPressed = false;
        this.shiftKeyPressed = false;
        this.homeKeyPressed = false;
      }
      if (!this.shiftKeyPressed || !this.endKeyPressed && !this.homeKeyPressed)
        return;
      this.waitForNextSelection = true;
    }

    private void checkPersonaAccessRights()
    {
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      this.gridViewLoanActions.BeginUpdate();
      this.gridViewRights.BeginUpdate();
      for (int nItemIndex = 0; nItemIndex < this.gridViewRights.Items.Count - 1; ++nItemIndex)
      {
        string strA = string.Empty;
        for (int index = 0; index < this.curSelection.Count; ++index)
        {
          BizRule.LoanActionAccessRight accessRight = ((LoanActionAccessRights) this.gridViewLoanActions.SelectedItems[index].Tag).GetAccessRight(((Persona) this.gridViewRights.Items[nItemIndex].Tag).ID);
          string strB = this.translateAccessRight(BizRule.LoanActionAccessRightString[2], false);
          switch (accessRight)
          {
            case BizRule.LoanActionAccessRight.Hide:
              strB = this.translateAccessRight(BizRule.LoanActionAccessRightString[0], false);
              break;
            case BizRule.LoanActionAccessRight.Enable:
              strB = this.translateAccessRight(BizRule.LoanActionAccessRightString[1], false);
              break;
            case BizRule.LoanActionAccessRight.Disable:
              strB = this.translateAccessRight(BizRule.LoanActionAccessRightString[2], false);
              break;
          }
          if (strA == string.Empty)
            strA = strB;
          if (string.Compare(strA, strB, true) != 0)
          {
            strA = "NotSame";
            break;
          }
        }
        this.gridViewRights.Items[nItemIndex].SubItems[1].Text = !(strA == "NotSame") ? strA : string.Empty;
      }
      this.gridViewRights.EndUpdate();
      this.gridViewLoanActions.EndUpdate();
    }

    private string translateAccessRight(string val, bool toInternalValue)
    {
      return BizRule.translateAccessRight(val, toInternalValue);
    }

    private void setListViewTag()
    {
      if (this.curSelection == null || this.curSelection.Count <= 0)
        return;
      for (int index = 0; index < this.curSelection.Count; ++index)
      {
        PersonaLoanActionAccessRight[] rights = new PersonaLoanActionAccessRight[this.personaList.Length - 1];
        LoanActionAccessRights tag1 = (LoanActionAccessRights) this.gridViewLoanActions.Items[Utils.ParseInt(this.curSelection[index])].Tag;
        for (int nItemIndex = 0; nItemIndex < this.gridViewRights.Items.Count - 1; ++nItemIndex)
        {
          BizRule.LoanActionAccessRight accessRight = BizRule.LoanActionAccessRight.Enable;
          string text = this.gridViewRights.Items[nItemIndex].SubItems[1].Text;
          Persona tag2 = (Persona) this.gridViewRights.Items[nItemIndex].Tag;
          if (text == this.translateAccessRight(BizRule.LoanActionAccessRightString[0], false))
            accessRight = BizRule.LoanActionAccessRight.Hide;
          else if (text == this.translateAccessRight(BizRule.LoanActionAccessRightString[1], false))
            accessRight = BizRule.LoanActionAccessRight.Enable;
          else if (text == this.translateAccessRight(BizRule.LoanActionAccessRightString[2], false))
            accessRight = BizRule.LoanActionAccessRight.Disable;
          else if (text == string.Empty)
            accessRight = tag1.GetAccessRight(tag2.ID);
          rights[nItemIndex] = new PersonaLoanActionAccessRight(tag2, accessRight);
        }
        this.gridViewLoanActions.Items[Utils.ParseInt(this.curSelection[index])].Tag = (object) new LoanActionAccessRights(tag1.LoanActionID, rights);
      }
    }

    private void addBtn_Click(object sender, EventArgs e)
    {
      this.setListViewTag();
      using (AddLoanActionsForPersonaAccess forPersonaAccess = new AddLoanActionsForPersonaAccess(this.session))
      {
        if (forPersonaAccess.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.addLoanActions(forPersonaAccess.SelectedLoanActions);
      }
    }

    private void addLoanActions(List<string> ids)
    {
      if (ids.Count == 0)
        return;
      this.gridViewLoanActions.BeginUpdate();
      this.curSelection = new ArrayList();
      for (int index1 = 0; index1 < ids.Count; ++index1)
      {
        bool flag = false;
        for (int nItemIndex = 0; nItemIndex < this.gridViewLoanActions.Items.Count; ++nItemIndex)
        {
          if (string.Compare(Convert.ToString(this.gridViewLoanActions.Items[nItemIndex].Text).Trim().ToLower(), Convert.ToString(ids[index1]).Trim().ToLower(), true) == 0)
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "The loan action list already contains loan action '" + ids[index1] + "'. This loan action will be ignored.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            flag = true;
            break;
          }
        }
        if (!flag)
        {
          TriggerActivationNameProvider activationNameProvider = new TriggerActivationNameProvider();
          GVItem gvItem = new GVItem(ids[index1]);
          PersonaLoanActionAccessRight[] rights = new PersonaLoanActionAccessRight[this.personaList.Length - 1];
          for (int index2 = 0; index2 < rights.Length; ++index2)
            rights[index2] = new PersonaLoanActionAccessRight(this.personaList[index2], BizRule.LoanActionAccessRight.Enable);
          gvItem.Tag = (object) new LoanActionAccessRights(Convert.ToString((object) (TriggerActivationType) new TriggerActivationNameProvider().GetValue(ids[index1])), rights);
          gvItem.Selected = true;
          this.gridViewLoanActions.Items.Add(gvItem);
          this.curSelection.Add((object) gvItem.Index);
          this.setListViewTag();
        }
      }
      this.gridViewLoanActions.EndUpdate();
    }

    private void removeBtn_Click(object sender, EventArgs e)
    {
      if (this.gridViewLoanActions.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You have to select a loan action first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        this.gridViewLoanActions.BeginUpdate();
        this.gridViewLoanActions.SelectedIndexChanged -= new EventHandler(this.gridViewLoanActions_SelectedIndexChanged);
        this.curSelection = new ArrayList();
        for (int nItemIndex = 0; nItemIndex < this.gridViewLoanActions.Items.Count; ++nItemIndex)
        {
          if (this.gridViewLoanActions.Items[nItemIndex].Selected)
            this.curSelection.Add((object) this.gridViewLoanActions.Items[nItemIndex]);
        }
        for (int index = 0; index < this.curSelection.Count; ++index)
          this.gridViewLoanActions.Items.Remove((GVItem) this.curSelection[index]);
        this.gridViewLoanActions.SelectedIndexChanged += new EventHandler(this.gridViewLoanActions_SelectedIndexChanged);
        this.curSelection = new ArrayList();
        if (this.gridViewLoanActions.Items.Count > 0)
        {
          this.gridViewLoanActions.Items[0].Selected = true;
        }
        else
        {
          for (int nItemIndex = 0; nItemIndex < this.gridViewRights.Items.Count - 1; ++nItemIndex)
            this.gridViewRights.Items[nItemIndex].SubItems[1].Text = string.Empty;
        }
        this.gridViewLoanActions.EndUpdate();
      }
    }

    private void okBtn_Click(object sender, EventArgs e)
    {
      this.setListViewTag();
      this.curSelection = new ArrayList();
      if (this.textBoxName.Text == "")
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please enter a rule name.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        BizRuleInfo[] rules = ((BpmManager) this.session.BPM.GetBpmManager(BpmCategory.LoanActionAccess)).GetRules(this.ruleCondForm.IsGeneralRule);
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
        if (this.gridViewLoanActions.Items.Count == 0)
        {
          int num3 = (int) Utils.Dialog((IWin32Window) this, "Please add a loan action.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          LoanActionAccessRights[] actionAccessRightsArray = new LoanActionAccessRights[this.gridViewLoanActions.Items.Count];
          for (int nItemIndex = 0; nItemIndex < this.gridViewLoanActions.Items.Count; ++nItemIndex)
            actionAccessRightsArray[nItemIndex] = (LoanActionAccessRights) this.gridViewLoanActions.Items[nItemIndex].Tag;
          this.ruleInfo = this.ruleInfo == null ? new LoanActionAccessRuleInfo(this.textBoxName.Text.Trim()) : new LoanActionAccessRuleInfo(this.ruleInfo.RuleID, this.textBoxName.Text.Trim());
          this.ruleInfo.LoanActionAccessRights = actionAccessRightsArray;
          this.ruleCondForm.ApplyCondition((BizRuleInfo) this.ruleInfo);
          this.ruleInfo.Condition2 = this.channelControl.ChannelValue;
          this.ruleInfo.CommentsTxt = this.commentsTxt.Text;
          this.DialogResult = DialogResult.OK;
        }
      }
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
      JedHelp.ShowHelp((Control) this, SystemSettings.HelpFile, nameof (LoanActionAccessRuleDialog));
    }

    private void gridViewLoanActions_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.ShiftKey)
        this.shiftKeyPressed = true;
      else if (e.KeyCode == Keys.End)
      {
        this.endKeyPressed = true;
      }
      else
      {
        if (e.KeyCode != Keys.Home)
          return;
        this.homeKeyPressed = true;
      }
    }

    private void gridViewLoanActions_ColumnClick(object source, GVColumnClickEventArgs e)
    {
      this.curSelection = new ArrayList();
      for (int nItemIndex = 0; nItemIndex < this.gridViewLoanActions.Items.Count; ++nItemIndex)
      {
        if (this.gridViewLoanActions.Items[nItemIndex].Selected)
          this.curSelection.Add((object) nItemIndex);
      }
    }

    private void gridViewRights_EditorOpening(object source, GVSubItemEditingEventArgs e)
    {
      ComboBox editorControl = (ComboBox) e.EditorControl;
      editorControl.DropDownStyle = ComboBoxStyle.DropDownList;
      editorControl.Items.Clear();
      editorControl.Items.AddRange((object[]) new string[4]
      {
        "",
        "Hide",
        "Enable",
        "Disable"
      });
      editorControl.Text = e.SubItem.Text;
    }

    private void gridViewRights_EditorClosing(object source, GVSubItemEditingEventArgs e)
    {
      ComboBox editorControl = (ComboBox) e.EditorControl;
      if (e.SubItem.Item.SubItems[0].Text == "All Personas")
      {
        string text = editorControl.Text;
        for (int nItemIndex = 0; nItemIndex < this.gridViewRights.Items.Count - 1; ++nItemIndex)
          this.gridViewRights.Items[nItemIndex].SubItems[1].Text = text;
        editorControl.Text = "";
      }
      e.SubItem.Text = editorControl.Text;
    }
  }
}
