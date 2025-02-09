// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.eFolder.ConditionalRequirementDialog
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.InputEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using EllieMae.EMLite.Workflow;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.eFolder
{
  public class ConditionalRequirementDialog : Form
  {
    private Sessions.Session session;
    private EDisclosurePackage package;
    private FieldSettings fieldSettings;
    private Hashtable defaultSettings;
    private Dictionary<MilestoneTemplate, string> updatedExceptionList;
    private string condition;
    private MilestoneLabel priorMilestoneSelection;
    private IContainer components;
    private Button btnCancel;
    private Button btnSave;
    private Label lblCondition;
    private Label lblRequirementType;
    private ComboBox cboRequirementType;
    private GroupContainer gcFields;
    private GridView gvFields;
    private ComboBoxEx cboMilestone;
    private Label lblMilestone;
    private FlowLayoutPanel pnlToolbar;
    private StandardIconButton btnRemoveFields;
    private StandardIconButton btnAddFields;
    private ToolTip tooltip;
    private StandardIconButton btnFindFields;
    private Button btnManageExceptions;

    public ConditionalRequirementDialog(EDisclosurePackage package, string condition)
      : this(Session.DefaultInstance, package, condition)
    {
    }

    public ConditionalRequirementDialog(
      Sessions.Session session,
      EDisclosurePackage package,
      string condition)
    {
      this.InitializeComponent();
      this.session = session;
      this.package = package;
      this.condition = condition;
      this.fieldSettings = this.session.LoanManager.GetFieldSettings();
      this.defaultSettings = this.session.SessionObjects.BpmManager.GetMilestoneTemplateDefaultSettings();
      this.initMilestoneField();
      this.loadRequirements();
      if (this.session.EncompassEdition != EncompassEdition.Broker)
        return;
      this.btnManageExceptions.Visible = false;
    }

    private void initMilestoneField()
    {
      foreach (EllieMae.EMLite.Workflow.Milestone activeMilestones in ((MilestoneTemplatesBpmManager) this.session.BPM.GetBpmManager(BpmCategory.Milestones)).GetAllActiveMilestonesList())
        this.cboMilestone.Items.Add((object) new MilestoneLabel(activeMilestones));
    }

    private void loadRequirements()
    {
      switch (this.package.RequirementType)
      {
        case PackageRequirementType.Fields:
          this.cboRequirementType.SelectedIndex = 0;
          break;
        case PackageRequirementType.Milestone:
          this.cboRequirementType.SelectedIndex = 1;
          break;
      }
      if (this.package.RequiredFields != null)
      {
        foreach (string requiredField in this.package.RequiredFields)
          this.addField(requiredField);
      }
      foreach (MilestoneLabel milestoneLabel in this.cboMilestone.Items)
      {
        if (((MilestoneTemplatesBpmManager) this.session.BPM.GetBpmManager(BpmCategory.Milestones)).GetMilestoneByName(milestoneLabel.MilestoneName).MilestoneID == this.package.RequiredMilestone)
        {
          this.cboMilestone.SelectedItem = (object) milestoneLabel;
          this.priorMilestoneSelection = milestoneLabel;
        }
      }
      this.gvFields.Sort(0, SortOrder.Ascending);
    }

    private void addField(string field)
    {
      if (this.gvFields.Items.Contains((object) field))
        return;
      GVItem gvItem = this.gvFields.Items.Add(field);
      FieldDefinition field1 = EncompassFields.GetField(field, this.fieldSettings, true);
      if (field1 == null)
        return;
      gvItem.SubItems.Add((object) field1.Description);
    }

    private bool saveRequirements()
    {
      if (this.cboRequirementType.SelectedIndex == 0)
      {
        if (this.gvFields.Items.Count == 0)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "You must have at least one field.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return false;
        }
        this.package.RequirementType = PackageRequirementType.Fields;
        List<string> stringList = new List<string>();
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvFields.Items)
          stringList.Add(gvItem.Text);
        this.package.RequiredFields = stringList.ToArray();
      }
      else if (this.cboRequirementType.SelectedIndex == 1)
      {
        MilestoneLabel selectedItem = (MilestoneLabel) this.cboMilestone.SelectedItem;
        if (selectedItem == null)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "You must select a milestone.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return false;
        }
        this.package.RequirementType = PackageRequirementType.Milestone;
        this.package.RequiredMilestone = ((MilestoneTemplatesBpmManager) this.session.BPM.GetBpmManager(BpmCategory.Milestones)).GetMilestoneByName(selectedItem.MilestoneName).MilestoneID;
      }
      else
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must select a condition type.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      return true;
    }

    private void cboRequirementType_SelectedIndexChanged(object sender, EventArgs e)
    {
      bool flag = this.cboRequirementType.SelectedIndex == 0;
      this.gcFields.Visible = flag;
      this.lblMilestone.Visible = !flag;
      this.cboMilestone.Visible = !flag;
      this.btnManageExceptions.Visible = !flag;
    }

    private void cboRequirementType_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.btnSave.Enabled = true;
    }

    private void btnAddFields_Click(object sender, EventArgs e)
    {
      using (AddFields addFields = new AddFields(this.session, "Add Fields", AddFieldOptions.AllowAnyField))
      {
        addFields.OnAddMoreButtonClick += new EventHandler(this.dialog_OnAddMoreButtonClick);
        if (addFields.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        foreach (string selectedFieldId in addFields.SelectedFieldIDs)
          this.addField(selectedFieldId);
        this.gvFields.ReSort();
        this.btnSave.Enabled = true;
      }
    }

    private void dialog_OnAddMoreButtonClick(object sender, EventArgs e)
    {
      foreach (string selectedFieldId in ((AddFields) sender).SelectedFieldIDs)
        this.addField(selectedFieldId);
      this.gvFields.ReSort();
      this.btnSave.Enabled = true;
    }

    private void btnFindFields_Click(object sender, EventArgs e)
    {
      List<string> stringList = new List<string>();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvFields.Items)
        stringList.Add(gvItem.Text);
      using (BusinessRuleFindFieldDialog ruleFindFieldDialog = new BusinessRuleFindFieldDialog(this.session, stringList.ToArray(), true, string.Empty, false, true))
      {
        if (ruleFindFieldDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        foreach (string selectedRequiredField in ruleFindFieldDialog.SelectedRequiredFields)
          this.addField(selectedRequiredField);
        this.gvFields.ReSort();
        this.btnSave.Enabled = true;
      }
    }

    private void btnRemoveFields_Click(object sender, EventArgs e)
    {
      List<GVItem> gvItemList = new List<GVItem>();
      foreach (GVItem selectedItem in this.gvFields.SelectedItems)
        gvItemList.Add(selectedItem);
      foreach (GVItem gvItem in gvItemList)
        this.gvFields.Items.Remove(gvItem);
      this.btnSave.Enabled = true;
    }

    private void gvFields_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnRemoveFields.Enabled = this.gvFields.SelectedItems.Count > 0;
    }

    private void cboMilestone_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.btnSave.Enabled = true;
      this.resolveException();
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      if (!this.saveRequirements())
        return;
      this.DialogResult = DialogResult.OK;
    }

    private void btnManageExceptions_Click(object sender, EventArgs e)
    {
      if (this.cboMilestone.SelectedIndex == -1)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select a milestone as condition for finished milestone", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        EllieMae.EMLite.Workflow.Milestone milestoneByName = ((MilestoneTemplatesBpmManager) this.session.BPM.GetBpmManager(BpmCategory.Milestones)).GetMilestoneByName(((MilestoneLabel) this.cboMilestone.SelectedItem).MilestoneName);
        CustomMilestoneExceptionResolver exceptionResolver;
        if (this.updatedExceptionList != null)
          exceptionResolver = new CustomMilestoneExceptionResolver(this.session, this.updatedExceptionList, milestoneByName.MilestoneID, string.Concat(this.defaultSettings[(object) "EDISCLOSURE"]), "eDisclosure");
        else if (this.package.UpdatedExceptionsList != null)
        {
          exceptionResolver = new CustomMilestoneExceptionResolver(this.session, this.package.UpdatedExceptionsList, milestoneByName.MilestoneID, string.Concat(this.defaultSettings[(object) "EDISCLOSURE"]), "eDisclosure");
        }
        else
        {
          Dictionary<MilestoneTemplate, bool> exceptionList = ConditionalRequirementDialog.getExceptionMilestoneTemplateList(milestoneByName.MilestoneID, this.condition, this.session);
          Dictionary<MilestoneTemplate, string> exceptions = new Dictionary<MilestoneTemplate, string>();
          exceptionList.Keys.ToList<MilestoneTemplate>().ForEach((Action<MilestoneTemplate>) (x =>
          {
            if (exceptionList[x])
              exceptions.Add(x, x.EDisclosureMilestoneSettings[this.condition]);
            else
              exceptions.Add(x, (string) null);
          }));
          exceptionResolver = new CustomMilestoneExceptionResolver(this.session, exceptions, milestoneByName.MilestoneID, string.Concat(this.defaultSettings[(object) "EDISCLOSURE"]), "eDisclosure");
        }
        if (exceptionResolver == null || DialogResult.OK != exceptionResolver.ShowDialog())
          return;
        this.updatedExceptionList = exceptionResolver.ExceptionSettings();
        this.btnSave.Enabled = true;
      }
    }

    private void resolveException()
    {
      EllieMae.EMLite.Workflow.Milestone milestoneByName = ((MilestoneTemplatesBpmManager) this.session.BPM.GetBpmManager(BpmCategory.Milestones)).GetMilestoneByName(((MilestoneLabel) this.cboMilestone.SelectedItem).MilestoneName);
      Dictionary<MilestoneTemplate, bool> exceptionList = ConditionalRequirementDialog.getExceptionMilestoneTemplateList(milestoneByName.MilestoneID, this.condition, this.session);
      if (exceptionList.ContainsValue(false))
      {
        CustomMilestoneExceptionNotification exceptionNotification = new CustomMilestoneExceptionNotification(exceptionList, "eDisclosure", string.Concat(this.defaultSettings[(object) "EDISCLOSURE"]));
        if (exceptionNotification.ShowDialog() == DialogResult.Yes)
        {
          Dictionary<MilestoneTemplate, string> exceptions = new Dictionary<MilestoneTemplate, string>();
          exceptionList.Keys.ToList<MilestoneTemplate>().ForEach((Action<MilestoneTemplate>) (x =>
          {
            if (exceptionList[x])
              exceptions.Add(x, x.EDisclosureMilestoneSettings[this.condition]);
            else
              exceptions.Add(x, (string) null);
          }));
          CustomMilestoneExceptionResolver exceptionResolver = new CustomMilestoneExceptionResolver(this.session, exceptions, milestoneByName.MilestoneID, string.Concat(this.defaultSettings[(object) "EDISCLOSURE"]), "eDisclosure");
          if (!exceptionNotification.ResolveIssue || DialogResult.OK == exceptionResolver.ShowDialog())
            this.updatedExceptionList = exceptionResolver.ExceptionSettings();
        }
        else
          this.cboMilestone.SelectedItem = (object) this.priorMilestoneSelection;
      }
      this.priorMilestoneSelection = (MilestoneLabel) this.cboMilestone.SelectedItem;
    }

    private static Dictionary<MilestoneTemplate, bool> getExceptionMilestoneTemplateList(
      string selectedMilestoneID,
      string condition,
      Sessions.Session session)
    {
      Dictionary<MilestoneTemplate, bool> result = new Dictionary<MilestoneTemplate, bool>();
      session.SessionObjects.BpmManager.GetMilestoneTemplates(true).ToList<MilestoneTemplate>().ForEach((Action<MilestoneTemplate>) (x =>
      {
        if (x.EDisclosureMilestoneSettings != null && x.EDisclosureMilestoneSettings.Count != 0 && x.EDisclosureMilestoneSettings.ContainsKey(condition))
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

    public Dictionary<MilestoneTemplate, string> UpdatedExceptionsList => this.updatedExceptionList;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      this.btnCancel = new Button();
      this.btnSave = new Button();
      this.lblCondition = new Label();
      this.lblRequirementType = new Label();
      this.cboRequirementType = new ComboBox();
      this.gcFields = new GroupContainer();
      this.pnlToolbar = new FlowLayoutPanel();
      this.btnRemoveFields = new StandardIconButton();
      this.btnFindFields = new StandardIconButton();
      this.btnAddFields = new StandardIconButton();
      this.gvFields = new GridView();
      this.cboMilestone = new ComboBoxEx();
      this.lblMilestone = new Label();
      this.tooltip = new ToolTip(this.components);
      this.btnManageExceptions = new Button();
      this.gcFields.SuspendLayout();
      this.pnlToolbar.SuspendLayout();
      ((ISupportInitialize) this.btnRemoveFields).BeginInit();
      ((ISupportInitialize) this.btnFindFields).BeginInit();
      ((ISupportInitialize) this.btnAddFields).BeginInit();
      this.SuspendLayout();
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(382, 404);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 24);
      this.btnCancel.TabIndex = 7;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnSave.Enabled = false;
      this.btnSave.Location = new Point(301, 404);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(75, 24);
      this.btnSave.TabIndex = 6;
      this.btnSave.Text = "Save";
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.lblCondition.AutoSize = true;
      this.lblCondition.Location = new Point(12, 12);
      this.lblCondition.Name = "lblCondition";
      this.lblCondition.Size = new Size(390, 14);
      this.lblCondition.TabIndex = 0;
      this.lblCondition.Text = "This package will be automatically selected when the following condition is met:";
      this.lblRequirementType.AutoSize = true;
      this.lblRequirementType.Location = new Point(12, 36);
      this.lblRequirementType.Name = "lblRequirementType";
      this.lblRequirementType.Size = new Size(110, 14);
      this.lblRequirementType.TabIndex = 1;
      this.lblRequirementType.Text = "Select Condition Type";
      this.cboRequirementType.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cboRequirementType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboRequirementType.FormattingEnabled = true;
      this.cboRequirementType.Items.AddRange(new object[2]
      {
        (object) "Field Value Entered",
        (object) "Milestone Finished"
      });
      this.cboRequirementType.Location = new Point(124, 32);
      this.cboRequirementType.Name = "cboRequirementType";
      this.cboRequirementType.Size = new Size(332, 22);
      this.cboRequirementType.TabIndex = 2;
      this.cboRequirementType.SelectedIndexChanged += new EventHandler(this.cboRequirementType_SelectedIndexChanged);
      this.cboRequirementType.SelectionChangeCommitted += new EventHandler(this.cboRequirementType_SelectionChangeCommitted);
      this.gcFields.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.gcFields.Controls.Add((Control) this.pnlToolbar);
      this.gcFields.Controls.Add((Control) this.gvFields);
      this.gcFields.HeaderForeColor = SystemColors.ControlText;
      this.gcFields.Location = new Point(12, 60);
      this.gcFields.Name = "gcFields";
      this.gcFields.Size = new Size(444, 336);
      this.gcFields.TabIndex = 5;
      this.gcFields.Text = "Fields";
      this.gcFields.Visible = false;
      this.pnlToolbar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.pnlToolbar.BackColor = Color.Transparent;
      this.pnlToolbar.Controls.Add((Control) this.btnRemoveFields);
      this.pnlToolbar.Controls.Add((Control) this.btnFindFields);
      this.pnlToolbar.Controls.Add((Control) this.btnAddFields);
      this.pnlToolbar.FlowDirection = FlowDirection.RightToLeft;
      this.pnlToolbar.Location = new Point(340, 2);
      this.pnlToolbar.Name = "pnlToolbar";
      this.pnlToolbar.Size = new Size(100, 22);
      this.pnlToolbar.TabIndex = 0;
      this.btnRemoveFields.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnRemoveFields.BackColor = Color.Transparent;
      this.btnRemoveFields.Enabled = false;
      this.btnRemoveFields.Location = new Point(84, 3);
      this.btnRemoveFields.Margin = new Padding(4, 3, 0, 3);
      this.btnRemoveFields.MouseDownImage = (Image) null;
      this.btnRemoveFields.Name = "btnRemoveFields";
      this.btnRemoveFields.Size = new Size(16, 16);
      this.btnRemoveFields.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnRemoveFields.TabIndex = 13;
      this.btnRemoveFields.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnRemoveFields, "Remove Fields");
      this.btnRemoveFields.Click += new EventHandler(this.btnRemoveFields_Click);
      this.btnFindFields.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnFindFields.BackColor = Color.Transparent;
      this.btnFindFields.Location = new Point(64, 3);
      this.btnFindFields.Margin = new Padding(4, 3, 0, 3);
      this.btnFindFields.MouseDownImage = (Image) null;
      this.btnFindFields.Name = "btnFindFields";
      this.btnFindFields.Size = new Size(16, 16);
      this.btnFindFields.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.btnFindFields.TabIndex = 14;
      this.btnFindFields.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnFindFields, "Find Fields");
      this.btnFindFields.Click += new EventHandler(this.btnFindFields_Click);
      this.btnAddFields.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAddFields.BackColor = Color.Transparent;
      this.btnAddFields.Location = new Point(44, 3);
      this.btnAddFields.Margin = new Padding(4, 3, 0, 3);
      this.btnAddFields.MouseDownImage = (Image) null;
      this.btnAddFields.Name = "btnAddFields";
      this.btnAddFields.Size = new Size(16, 16);
      this.btnAddFields.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAddFields.TabIndex = 12;
      this.btnAddFields.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnAddFields, "Add Fields");
      this.btnAddFields.Click += new EventHandler(this.btnAddFields_Click);
      this.gvFields.BorderStyle = BorderStyle.None;
      this.gvFields.ClearSelectionsOnEmptyRowClick = false;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "colField";
      gvColumn1.Text = "Field ID";
      gvColumn1.Width = 120;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "colDescription";
      gvColumn2.SpringToFit = true;
      gvColumn2.Text = "Description";
      gvColumn2.Width = 322;
      this.gvFields.Columns.AddRange(new GVColumn[2]
      {
        gvColumn1,
        gvColumn2
      });
      this.gvFields.Dock = DockStyle.Fill;
      this.gvFields.Location = new Point(1, 26);
      this.gvFields.Name = "gvFields";
      this.gvFields.Size = new Size(442, 309);
      this.gvFields.TabIndex = 1;
      this.gvFields.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gvFields.SelectedIndexChanged += new EventHandler(this.gvFields_SelectedIndexChanged);
      this.cboMilestone.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cboMilestone.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboMilestone.FormattingEnabled = true;
      this.cboMilestone.ItemHeight = 16;
      this.cboMilestone.Location = new Point(124, 60);
      this.cboMilestone.Name = "cboMilestone";
      this.cboMilestone.SelectedBGColor = SystemColors.Highlight;
      this.cboMilestone.Size = new Size(332, 22);
      this.cboMilestone.TabIndex = 4;
      this.cboMilestone.Visible = false;
      this.cboMilestone.SelectionChangeCommitted += new EventHandler(this.cboMilestone_SelectionChangeCommitted);
      this.lblMilestone.AutoSize = true;
      this.lblMilestone.Location = new Point(12, 64);
      this.lblMilestone.Name = "lblMilestone";
      this.lblMilestone.Size = new Size(95, 14);
      this.lblMilestone.TabIndex = 3;
      this.lblMilestone.Text = "Finished Milestone";
      this.lblMilestone.Visible = false;
      this.btnManageExceptions.Location = new Point(337, 88);
      this.btnManageExceptions.Name = "btnManageExceptions";
      this.btnManageExceptions.Size = new Size(120, 23);
      this.btnManageExceptions.TabIndex = 32;
      this.btnManageExceptions.Text = "Manage Exceptions...";
      this.btnManageExceptions.UseVisualStyleBackColor = true;
      this.btnManageExceptions.Visible = false;
      this.btnManageExceptions.Click += new EventHandler(this.btnManageExceptions_Click);
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(469, 435);
      this.Controls.Add((Control) this.btnManageExceptions);
      this.Controls.Add((Control) this.gcFields);
      this.Controls.Add((Control) this.cboMilestone);
      this.Controls.Add((Control) this.lblMilestone);
      this.Controls.Add((Control) this.cboRequirementType);
      this.Controls.Add((Control) this.lblRequirementType);
      this.Controls.Add((Control) this.lblCondition);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnSave);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ConditionalRequirementDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Condition";
      this.gcFields.ResumeLayout(false);
      this.pnlToolbar.ResumeLayout(false);
      ((ISupportInitialize) this.btnRemoveFields).EndInit();
      ((ISupportInitialize) this.btnFindFields).EndInit();
      ((ISupportInitialize) this.btnAddFields).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
