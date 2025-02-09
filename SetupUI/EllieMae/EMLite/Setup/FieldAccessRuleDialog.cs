// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.FieldAccessRuleDialog
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.InputEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class FieldAccessRuleDialog : Form
  {
    private const string className = "FieldAccessRuleDialog";
    private bool hasNoAccess;
    private Sessions.Session session;
    private RuleConditionControl ruleCondForm;
    private Button okBtn;
    private Button cancelBtn;
    private TextBox textBoxName;
    private Label label1;
    private Button removeBtn;
    private Button addBtn;
    private Button findBtn;
    private Label label2;
    private System.ComponentModel.Container components;
    private Persona[] personaList;
    private Label label3;
    private Label label4;
    private Label label5;
    private Panel panelCondition;
    private const string ALLPERSONAS = "All Personas";
    private FieldSettings fieldSettings;
    private EMHelpLink emHelpLink1;
    private Label label6;
    private Panel panelChannel;
    private ArrayList curSelection;
    private GridView gridViewFields;
    private GridView gridViewRights;
    private Panel panelSection4;
    private Panel panelDialog;
    private TextBox commentsTxt;
    private Label label7;
    private ChannelConditionControl channelControl;
    private FieldAccessRuleInfo ruleInfo;
    private bool waitForNextSelection;
    private bool shiftKeyPressed;
    private bool endKeyPressed;
    private bool homeKeyPressed;

    public FieldAccessRuleDialog(Sessions.Session session, FieldAccessRuleInfo ruleInfo)
    {
      this.ruleInfo = ruleInfo;
      this.InitializeComponent();
      this.session = session;
      this.emHelpLink1.AssignSession(this.session);
      this.fieldSettings = this.session.LoanManager.GetFieldSettings();
      this.channelControl = new ChannelConditionControl();
      if (this.ruleInfo != null)
        this.channelControl.ChannelValue = this.ruleInfo.Condition2;
      this.panelChannel.Controls.Add((Control) this.channelControl);
      this.ruleCondForm = new RuleConditionControl(this.session);
      this.ruleCondForm.InitControl(BpmCategory.FieldAccess);
      this.panelCondition.Controls.Add((Control) this.ruleCondForm);
      this.initForm();
      this.hasNoAccess = !((FeaturesAclManager) session.ACL.GetAclManager(AclCategory.Features)).GetUserApplicationRight(AclFeature.SettingsTab_PersonaAccesstoFields);
      this.addBtn.Enabled = this.removeBtn.Enabled = this.findBtn.Enabled = this.okBtn.Enabled = !this.hasNoAccess;
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

    public FieldAccessRuleInfo RuleInfo => this.ruleInfo;

    private void InitializeComponent()
    {
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      GVColumn gvColumn4 = new GVColumn();
      this.okBtn = new Button();
      this.cancelBtn = new Button();
      this.textBoxName = new TextBox();
      this.label2 = new Label();
      this.removeBtn = new Button();
      this.addBtn = new Button();
      this.findBtn = new Button();
      this.label1 = new Label();
      this.label3 = new Label();
      this.label4 = new Label();
      this.label5 = new Label();
      this.panelCondition = new Panel();
      this.gridViewRights = new GridView();
      this.emHelpLink1 = new EMHelpLink();
      this.label6 = new Label();
      this.panelChannel = new Panel();
      this.gridViewFields = new GridView();
      this.panelSection4 = new Panel();
      this.panelDialog = new Panel();
      this.commentsTxt = new TextBox();
      this.label7 = new Label();
      this.panelSection4.SuspendLayout();
      this.panelDialog.SuspendLayout();
      this.SuspendLayout();
      this.okBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.okBtn.Location = new Point(864, 559);
      this.okBtn.Name = "okBtn";
      this.okBtn.Size = new Size(75, 23);
      this.okBtn.TabIndex = 13;
      this.okBtn.Text = "&Save";
      this.okBtn.Click += new EventHandler(this.okBtn_Click);
      this.cancelBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(945, 559);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 23);
      this.cancelBtn.TabIndex = 14;
      this.cancelBtn.Text = "Cancel";
      this.textBoxName.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.textBoxName.Location = new Point(32, 30);
      this.textBoxName.MaxLength = 64;
      this.textBoxName.Name = "textBoxName";
      this.textBoxName.Size = new Size(677, 20);
      this.textBoxName.TabIndex = 1;
      this.label2.AutoSize = true;
      this.label2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(351, 25);
      this.label2.Name = "label2";
      this.label2.Size = new Size(66, 13);
      this.label2.TabIndex = 18;
      this.label2.Text = "Assign Right";
      this.removeBtn.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.removeBtn.Location = new Point(282, 99);
      this.removeBtn.Name = "removeBtn";
      this.removeBtn.Size = new Size(56, 23);
      this.removeBtn.TabIndex = 9;
      this.removeBtn.Text = "&Remove";
      this.removeBtn.Click += new EventHandler(this.removeBtn_Click);
      this.addBtn.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.addBtn.Location = new Point(282, 41);
      this.addBtn.Name = "addBtn";
      this.addBtn.Size = new Size(56, 23);
      this.addBtn.TabIndex = 7;
      this.addBtn.Text = "&Add";
      this.addBtn.Click += new EventHandler(this.addBtn_Click);
      this.findBtn.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.findBtn.Location = new Point(282, 70);
      this.findBtn.Name = "findBtn";
      this.findBtn.Size = new Size(56, 23);
      this.findBtn.TabIndex = 8;
      this.findBtn.Text = "&Find";
      this.findBtn.Click += new EventHandler(this.findBtn_Click);
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(13, 25);
      this.label1.Name = "label1";
      this.label1.Size = new Size(60, 13);
      this.label1.TabIndex = 12;
      this.label1.Text = "Add a Field";
      this.label3.AutoSize = true;
      this.label3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label3.Location = new Point(-3, 6);
      this.label3.Name = "label3";
      this.label3.Size = new Size(286, 13);
      this.label3.TabIndex = 6;
      this.label3.Text = "4. Define persona's field access for the condition";
      this.label4.AutoSize = true;
      this.label4.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label4.Location = new Point(16, 170);
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
      this.panelCondition.Location = new Point(32, 186);
      this.panelCondition.Name = "panelCondition";
      this.panelCondition.Size = new Size(677, 94);
      this.panelCondition.TabIndex = 5;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Persona";
      gvColumn1.Width = 200;
      gvColumn2.ActivatedEditorType = GVActivatedEditorType.ComboBox;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.SpringToFit = true;
      gvColumn2.Text = "Rights";
      gvColumn2.Width = 129;
      this.gridViewRights.Columns.AddRange(new GVColumn[2]
      {
        gvColumn1,
        gvColumn2
      });
      this.gridViewRights.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gridViewRights.Location = new Point(354, 41);
      this.gridViewRights.Name = "gridViewRights";
      this.gridViewRights.Size = new Size(333, 191);
      this.gridViewRights.SortIconVisible = false;
      this.gridViewRights.SortOption = GVSortOption.None;
      this.gridViewRights.TabIndex = 31;
      this.gridViewRights.EditorOpening += new GVSubItemEditingEventHandler(this.gridViewRights_EditorOpening);
      this.gridViewRights.EditorClosing += new GVSubItemEditingEventHandler(this.gridViewRights_EditorClosing);
      this.emHelpLink1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.emHelpLink1.BackColor = Color.Transparent;
      this.emHelpLink1.Cursor = Cursors.Hand;
      this.emHelpLink1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.emHelpLink1.HelpTag = "Setup\\Persona Access to Fields";
      this.emHelpLink1.Location = new Point(3, 566);
      this.emHelpLink1.Name = "emHelpLink1";
      this.emHelpLink1.Size = new Size(90, 16);
      this.emHelpLink1.TabIndex = 12;
      this.label6.AutoSize = true;
      this.label6.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label6.Location = new Point(16, 63);
      this.label6.Name = "label6";
      this.label6.Size = new Size(239, 13);
      this.label6.TabIndex = 2;
      this.label6.Text = "2. Select all Channels this rule applies to";
      this.panelChannel.Location = new Point(32, 79);
      this.panelChannel.Name = "panelChannel";
      this.panelChannel.Size = new Size(677, 79);
      this.panelChannel.TabIndex = 3;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column1";
      gvColumn3.Text = "ID";
      gvColumn3.Width = 72;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column2";
      gvColumn4.Text = "Description";
      gvColumn4.Width = 175;
      this.gridViewFields.Columns.AddRange(new GVColumn[2]
      {
        gvColumn3,
        gvColumn4
      });
      this.gridViewFields.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gridViewFields.Location = new Point(13, 41);
      this.gridViewFields.Name = "gridViewFields";
      this.gridViewFields.Size = new Size(252, 189);
      this.gridViewFields.TabIndex = 30;
      this.gridViewFields.SelectedIndexChanged += new EventHandler(this.gridViewFields_SelectedIndexChanged);
      this.gridViewFields.KeyDown += new KeyEventHandler(this.gridViewFields_KeyDown);
      this.gridViewFields.ColumnClick += new GVColumnClickEventHandler(this.gridViewFields_ColumnClick);
      this.panelSection4.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.panelSection4.Controls.Add((Control) this.label3);
      this.panelSection4.Controls.Add((Control) this.gridViewRights);
      this.panelSection4.Controls.Add((Control) this.label2);
      this.panelSection4.Controls.Add((Control) this.gridViewFields);
      this.panelSection4.Controls.Add((Control) this.label1);
      this.panelSection4.Controls.Add((Control) this.removeBtn);
      this.panelSection4.Controls.Add((Control) this.addBtn);
      this.panelSection4.Controls.Add((Control) this.findBtn);
      this.panelSection4.Location = new Point(19, 325);
      this.panelSection4.Name = "panelSection4";
      this.panelSection4.Size = new Size(690, 235);
      this.panelSection4.TabIndex = 32;
      this.panelDialog.AutoScroll = true;
      this.panelDialog.Controls.Add((Control) this.commentsTxt);
      this.panelDialog.Controls.Add((Control) this.label7);
      this.panelDialog.Controls.Add((Control) this.cancelBtn);
      this.panelDialog.Controls.Add((Control) this.okBtn);
      this.panelDialog.Controls.Add((Control) this.label4);
      this.panelDialog.Controls.Add((Control) this.panelSection4);
      this.panelDialog.Controls.Add((Control) this.textBoxName);
      this.panelDialog.Controls.Add((Control) this.panelChannel);
      this.panelDialog.Controls.Add((Control) this.label5);
      this.panelDialog.Controls.Add((Control) this.label6);
      this.panelDialog.Controls.Add((Control) this.panelCondition);
      this.panelDialog.Controls.Add((Control) this.emHelpLink1);
      this.panelDialog.Dock = DockStyle.Fill;
      this.panelDialog.Location = new Point(0, 0);
      this.panelDialog.Name = "panelDialog";
      this.panelDialog.Size = new Size(1080, 585);
      this.panelDialog.TabIndex = 33;
      this.commentsTxt.Location = new Point(743, 28);
      this.commentsTxt.Multiline = true;
      this.commentsTxt.Name = "commentsTxt";
      this.commentsTxt.ScrollBars = ScrollBars.Vertical;
      this.commentsTxt.Size = new Size(312, 141);
      this.commentsTxt.TabIndex = 11;
      this.label7.AutoSize = true;
      this.label7.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label7.Location = new Point(740, 7);
      this.label7.Name = "label7";
      this.label7.Size = new Size(103, 13);
      this.label7.TabIndex = 10;
      this.label7.Text = "Notes/Comments";
      this.AutoScaleBaseSize = new Size(5, 13);
      this.AutoScroll = true;
      this.ClientSize = new Size(1080, 585);
      this.Controls.Add((Control) this.panelDialog);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (FieldAccessRuleDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Field Access Rule";
      this.KeyDown += new KeyEventHandler(this.Help_KeyDown);
      this.panelSection4.ResumeLayout(false);
      this.panelSection4.PerformLayout();
      this.panelDialog.ResumeLayout(false);
      this.panelDialog.PerformLayout();
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
      this.gridViewFields.Items.Clear();
      if (this.ruleInfo == null)
        return;
      this.gridViewFields.BeginUpdate();
      for (int index = 0; index < this.ruleInfo.FieldAccessRights.Length; ++index)
      {
        string fieldId = this.ruleInfo.FieldAccessRights[index].FieldID;
        if (EncompassFields.IsReportable(fieldId, this.fieldSettings) || fieldId.ToUpper().StartsWith("LOCKBUTTON_") || fieldId.ToUpper().StartsWith("HUD") && EncompassFields.GetField(fieldId) != null)
          this.gridViewFields.Items.Add(new GVItem(fieldId)
          {
            SubItems = {
              (object) this.getFieldDescription(fieldId)
            },
            Tag = (object) this.ruleInfo.FieldAccessRights[index]
          });
      }
      this.gridViewFields.EndUpdate();
      this.curSelection = new ArrayList();
      if (this.gridViewFields.Items.Count > 0)
        this.gridViewFields.Items[0].Selected = true;
      else
        this.gridViewRights.Enabled = false;
      this.ruleCondForm.SetCondition((BizRuleInfo) this.ruleInfo);
      this.textBoxName.Text = this.ruleInfo.RuleName;
      if (this.ruleInfo.CommentsTxt.Contains("\n") && !this.ruleInfo.CommentsTxt.Contains(Environment.NewLine))
        this.commentsTxt.Text = this.ruleInfo.CommentsTxt.Replace("\n", Environment.NewLine);
      else
        this.commentsTxt.Text = this.ruleInfo.CommentsTxt;
    }

    private void gridViewFields_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.gridViewFields.SelectedItems.Count == 0)
        return;
      if ((this.endKeyPressed || this.homeKeyPressed) && this.shiftKeyPressed)
      {
        if (this.waitForNextSelection)
        {
          if (this.gridViewFields.SelectedItems.Count > 1)
            return;
        }
        else if (this.homeKeyPressed)
        {
          if (this.curSelection.Count > 0 && this.gridViewFields.SelectedItems.Count < (int) this.curSelection[0] + 1)
            return;
        }
        else if (this.endKeyPressed && !this.gridViewFields.Items[this.gridViewFields.Items.Count - 1].Selected)
          return;
      }
      this.gridViewFields.SelectedIndexChanged -= new EventHandler(this.gridViewFields_SelectedIndexChanged);
      this.gridViewRights.Enabled = true;
      this.setListViewTag();
      this.curSelection = new ArrayList();
      for (int nItemIndex = 0; nItemIndex < this.gridViewFields.Items.Count; ++nItemIndex)
      {
        if (this.gridViewFields.Items[nItemIndex].Selected)
          this.curSelection.Add((object) nItemIndex);
      }
      this.checkPersonaAccessRights();
      this.gridViewFields.SelectedIndexChanged += new EventHandler(this.gridViewFields_SelectedIndexChanged);
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
      this.gridViewFields.BeginUpdate();
      this.gridViewRights.BeginUpdate();
      for (int nItemIndex = 0; nItemIndex < this.gridViewRights.Items.Count - 1; ++nItemIndex)
      {
        string strA = string.Empty;
        for (int index = 0; index < this.curSelection.Count; ++index)
        {
          BizRule.FieldAccessRight accessRight = ((FieldAccessRights) this.gridViewFields.SelectedItems[index].Tag).GetAccessRight(((Persona) this.gridViewRights.Items[nItemIndex].Tag).ID);
          string strB = this.translateAccessRight(BizRule.FieldAccessRightStrings[2], false);
          switch (accessRight)
          {
            case BizRule.FieldAccessRight.Hide:
              strB = this.translateAccessRight(BizRule.FieldAccessRightStrings[0], false);
              break;
            case BizRule.FieldAccessRight.ViewOnly:
              strB = this.translateAccessRight(BizRule.FieldAccessRightStrings[1], false);
              break;
            case BizRule.FieldAccessRight.DoesNotApply:
              strB = this.translateAccessRight(BizRule.FieldAccessRightStrings[3], false);
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
      this.gridViewFields.EndUpdate();
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
        PersonaFieldAccessRight[] rights = new PersonaFieldAccessRight[this.personaList.Length - 1];
        for (int nItemIndex = 0; nItemIndex < this.gridViewRights.Items.Count - 1; ++nItemIndex)
        {
          BizRule.FieldAccessRight accessRight = BizRule.FieldAccessRight.Edit;
          string text = this.gridViewRights.Items[nItemIndex].SubItems[1].Text;
          Persona tag = (Persona) this.gridViewRights.Items[nItemIndex].Tag;
          if (text == this.translateAccessRight(BizRule.FieldAccessRightStrings[0], false))
            accessRight = BizRule.FieldAccessRight.Hide;
          else if (text == this.translateAccessRight(BizRule.FieldAccessRightStrings[1], false))
            accessRight = BizRule.FieldAccessRight.ViewOnly;
          else if (text == this.translateAccessRight(BizRule.FieldAccessRightStrings[3], false))
            accessRight = BizRule.FieldAccessRight.DoesNotApply;
          else if (text == string.Empty)
            accessRight = ((FieldAccessRights) this.gridViewFields.Items[Utils.ParseInt(this.curSelection[index])].Tag).GetAccessRight(tag.ID);
          rights[nItemIndex] = new PersonaFieldAccessRight(tag, accessRight);
        }
        this.gridViewFields.Items[Utils.ParseInt(this.curSelection[index])].Tag = (object) new FieldAccessRights(this.gridViewFields.Items[Utils.ParseInt(this.curSelection[index])].Text, rights);
      }
    }

    private void addBtn_Click(object sender, EventArgs e)
    {
      this.setListViewTag();
      using (AddFields addFields = new AddFields(this.session, (string) null, AddFieldOptions.AllowCustomFields | AddFieldOptions.AllowVirtualFields | AddFieldOptions.AllowButtons))
      {
        addFields.OnAddMoreButtonClick += new EventHandler(this.addFieldDlg_OnAddMoreButtonClick);
        if (addFields.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.addFields(addFields.SelectedFieldIDs);
      }
    }

    private void addFields(string[] ids)
    {
      if (ids.Length == 0)
        return;
      this.gridViewFields.BeginUpdate();
      this.curSelection = new ArrayList();
      for (int index1 = 0; index1 < ids.Length; ++index1)
      {
        bool flag = false;
        for (int nItemIndex = 0; nItemIndex < this.gridViewFields.Items.Count; ++nItemIndex)
        {
          if (string.Compare(this.gridViewFields.Items[nItemIndex].Text, ids[index1], true) == 0)
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "The field list already contains field '" + ids[index1] + "'. This field will be ignored.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            flag = true;
            break;
          }
        }
        if (!flag)
        {
          GVItem gvItem = new GVItem(ids[index1]);
          gvItem.SubItems.Add((object) this.getFieldDescription(ids[index1]));
          PersonaFieldAccessRight[] rights = new PersonaFieldAccessRight[this.personaList.Length - 1];
          for (int index2 = 0; index2 < rights.Length; ++index2)
            rights[index2] = new PersonaFieldAccessRight(this.personaList[index2], BizRule.FieldAccessRight.DoesNotApply);
          gvItem.Tag = (object) new FieldAccessRights(ids[index1], rights);
          gvItem.Selected = true;
          this.gridViewFields.Items.Add(gvItem);
          this.curSelection.Add((object) gvItem.Index);
          this.setListViewTag();
        }
      }
      this.gridViewFields.EndUpdate();
    }

    private void removeBtn_Click(object sender, EventArgs e)
    {
      if (this.gridViewFields.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You have to select a field first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        this.gridViewFields.BeginUpdate();
        this.gridViewFields.SelectedIndexChanged -= new EventHandler(this.gridViewFields_SelectedIndexChanged);
        this.curSelection = new ArrayList();
        for (int nItemIndex = 0; nItemIndex < this.gridViewFields.Items.Count; ++nItemIndex)
        {
          if (this.gridViewFields.Items[nItemIndex].Selected)
            this.curSelection.Add((object) this.gridViewFields.Items[nItemIndex]);
        }
        for (int index = 0; index < this.curSelection.Count; ++index)
          this.gridViewFields.Items.Remove((GVItem) this.curSelection[index]);
        this.gridViewFields.SelectedIndexChanged += new EventHandler(this.gridViewFields_SelectedIndexChanged);
        this.curSelection = new ArrayList();
        if (this.gridViewFields.Items.Count > 0)
        {
          this.gridViewFields.Items[0].Selected = true;
        }
        else
        {
          for (int nItemIndex = 0; nItemIndex < this.gridViewRights.Items.Count - 1; ++nItemIndex)
            this.gridViewRights.Items[nItemIndex].SubItems[1].Text = string.Empty;
        }
        this.gridViewFields.EndUpdate();
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
        BizRuleInfo[] rules = ((BpmManager) this.session.BPM.GetBpmManager(BpmCategory.FieldAccess)).GetRules(this.ruleCondForm.IsGeneralRule);
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
        if (this.gridViewFields.Items.Count == 0)
        {
          int num3 = (int) Utils.Dialog((IWin32Window) this, "Please add a field.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          FieldAccessRights[] fieldAccessRightsArray = new FieldAccessRights[this.gridViewFields.Items.Count];
          for (int nItemIndex = 0; nItemIndex < this.gridViewFields.Items.Count; ++nItemIndex)
            fieldAccessRightsArray[nItemIndex] = (FieldAccessRights) this.gridViewFields.Items[nItemIndex].Tag;
          this.ruleInfo = this.ruleInfo == null ? new FieldAccessRuleInfo(this.textBoxName.Text.Trim()) : new FieldAccessRuleInfo(this.ruleInfo.RuleID, this.textBoxName.Text.Trim());
          this.ruleInfo.FieldAccessRights = fieldAccessRightsArray;
          this.ruleCondForm.ApplyCondition((BizRuleInfo) this.ruleInfo);
          this.ruleInfo.Condition2 = this.channelControl.ChannelValue;
          this.ruleInfo.CommentsTxt = this.commentsTxt.Text;
          this.DialogResult = DialogResult.OK;
        }
      }
    }

    private void findBtn_Click(object sender, EventArgs e)
    {
      ArrayList arrayList = new ArrayList();
      for (int nItemIndex = 0; nItemIndex < this.gridViewFields.Items.Count; ++nItemIndex)
        arrayList.Add((object) this.gridViewFields.Items[nItemIndex].Text);
      using (BusinessRuleFindFieldDialog ruleFindFieldDialog = new BusinessRuleFindFieldDialog(this.session, (string[]) arrayList.ToArray(typeof (string)), false, string.Empty, false, true))
      {
        if (ruleFindFieldDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        Cursor.Current = Cursors.WaitCursor;
        PersonaFieldAccessRight[] rights = new PersonaFieldAccessRight[this.personaList.Length - 1];
        BizRule.FieldAccessRight accessRight = BizRule.FieldAccessRight.DoesNotApply;
        if (ruleFindFieldDialog.AccessRightSelected != (BizRule.FieldAccessRight) 255)
          accessRight = ruleFindFieldDialog.AccessRightSelected;
        for (int index = 0; index < rights.Length; ++index)
          rights[index] = new PersonaFieldAccessRight(this.personaList[index], accessRight);
        this.gridViewFields.BeginUpdate();
        for (int index = 0; index < ruleFindFieldDialog.SelectedRequiredFields.Length; ++index)
        {
          if (!(ruleFindFieldDialog.SelectedRequiredFields[index] == "") && !arrayList.Contains((object) ruleFindFieldDialog.SelectedRequiredFields[index]))
            this.gridViewFields.Items.Add(new GVItem(ruleFindFieldDialog.SelectedRequiredFields[index])
            {
              SubItems = {
                (object) this.getFieldDescription(ruleFindFieldDialog.SelectedRequiredFields[index])
              },
              Tag = (object) new FieldAccessRights(ruleFindFieldDialog.SelectedRequiredFields[index], rights),
              Selected = true
            });
        }
        this.gridViewFields.EndUpdate();
        Cursor.Current = Cursors.Default;
      }
    }

    private string getFieldDescription(string fieldID)
    {
      return EncompassFields.GetDescription(fieldID, this.fieldSettings);
    }

    private void addFieldDlg_OnAddMoreButtonClick(object sender, EventArgs e)
    {
      AddFields addFields = (AddFields) sender;
      if (addFields == null)
        return;
      this.addFields(addFields.SelectedFieldIDs);
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
      JedHelp.ShowHelp((Control) this, SystemSettings.HelpFile, nameof (FieldAccessRuleDialog));
    }

    private void gridViewFields_KeyDown(object sender, KeyEventArgs e)
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

    private void gridViewRights_EditorOpening(object source, GVSubItemEditingEventArgs e)
    {
      ComboBox editorControl = (ComboBox) e.EditorControl;
      editorControl.DropDownStyle = ComboBoxStyle.DropDownList;
      editorControl.Items.Clear();
      editorControl.Items.AddRange((object[]) new string[5]
      {
        "",
        "Hide",
        "View Only / Disabled",
        "Edit / Enabled",
        "Does Not Apply"
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

    private void gridViewFields_ColumnClick(object source, GVColumnClickEventArgs e)
    {
      this.curSelection = new ArrayList();
      for (int nItemIndex = 0; nItemIndex < this.gridViewFields.Items.Count; ++nItemIndex)
      {
        if (this.gridViewFields.Items[nItemIndex].Selected)
          this.curSelection.Add((object) nItemIndex);
      }
    }
  }
}
