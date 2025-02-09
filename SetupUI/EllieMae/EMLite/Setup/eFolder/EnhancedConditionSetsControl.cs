// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.eFolder.EnhancedConditionSetsControl
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using Elli.Metrics.Client;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.eFolder
{
  public class EnhancedConditionSetsControl : UserControl
  {
    private Sessions.Session session;
    private IContainer components;
    private GroupContainer gcConditionSets;
    private StandardIconButton btnNew;
    private StandardIconButton btnDuplicate;
    private StandardIconButton btnEdit;
    private StandardIconButton btnDelete;
    private GridView gvConditionSets;
    private ToolTip tooltip;

    public EnhancedConditionSetsControl(Sessions.Session session)
    {
      this.InitializeComponent();
      this.session = session;
      this.initConditionSetList();
      this.loadConditionSetList((string) null);
    }

    private void initConditionSetList() => this.gvConditionSets.Sort(0, SortOrder.Ascending);

    private void loadConditionSetList(string defaultItem)
    {
      this.gvConditionSets.Items.Clear();
      foreach (EnhancedConditionSet enhancedConditionSet in this.session.ConfigurationManager.GetEnhancedConditionSets())
      {
        string description = enhancedConditionSet.Description;
        GVItem gvItem = new GVItem();
        gvItem.SubItems[0].Text = enhancedConditionSet.SetName;
        gvItem.SubItems[1].Text = enhancedConditionSet.Description;
        gvItem.Tag = (object) enhancedConditionSet.Id;
        if (enhancedConditionSet.SetName == defaultItem)
          gvItem.Selected = true;
        this.gvConditionSets.Items.Add(gvItem);
      }
      this.gcConditionSets.Text = "Enhanced Condition Sets (" + this.gvConditionSets.Items.Count.ToString() + ")";
      this.gvConditionSets.ReSort();
    }

    private void editConditionSet(Guid id)
    {
      using (EditEnhancedConditionSet enhancedConditionSet = new EditEnhancedConditionSet(this.session, (object) id))
      {
        if (enhancedConditionSet.ShowDialog((IWin32Window) Form.ActiveForm) != DialogResult.OK)
          return;
        this.loadConditionSetList((string) null);
      }
    }

    private EnhancedConditionSet getSelectedSet()
    {
      if (this.gvConditionSets.SelectedItems.Count != 1)
        return (EnhancedConditionSet) null;
      GVItem selectedItem = this.gvConditionSets.SelectedItems[0];
      try
      {
        return this.session.ConfigurationManager.GetEnhancedConditionSetDetail((Guid) selectedItem.Tag, false, false);
      }
      catch (Exception ex)
      {
        MetricsFactory.IncrementErrorCounter(ex, "The following error occurred when", "D:\\ws\\24.3.0.0\\EmLite\\SetupUI\\Setup\\eFolder\\EnhancedConditionSetsControl.cs", nameof (getSelectedSet), 121);
        int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "The following error occurred when trying to retrieve the enhanced condition set:\r\n\r\n" + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return (EnhancedConditionSet) null;
      }
    }

    private void gvConditionSets_ItemDoubleClick(object source, GVItemEventArgs e)
    {
      this.btnEdit_Click(source, EventArgs.Empty);
    }

    private void gvConditionSets_SelectedIndexChanged(object sender, EventArgs e)
    {
      int count = this.gvConditionSets.SelectedItems.Count;
      this.btnDuplicate.Enabled = count == 1;
      this.btnEdit.Enabled = count == 1;
      this.btnDelete.Enabled = count > 0;
    }

    private void btnNew_Click(object sender, EventArgs e)
    {
      using (EditEnhancedConditionSet enhancedConditionSet = new EditEnhancedConditionSet(this.session, (object) null))
      {
        if (enhancedConditionSet.ShowDialog((IWin32Window) Form.ActiveForm) != DialogResult.OK)
          return;
        if (enhancedConditionSet.SetName != "")
          this.loadConditionSetList(enhancedConditionSet.SetName);
        else
          this.loadConditionSetList((string) null);
      }
    }

    private void btnDuplicate_Click(object sender, EventArgs e)
    {
      EnhancedConditionSet selectedSet = this.getSelectedSet();
      if (selectedSet == null)
        return;
      EnhancedConditionSet conditionset = new EnhancedConditionSet();
      conditionset.Id = Guid.NewGuid();
      conditionset.CreatedBy = this.session.UserID;
      conditionset.LastModifiedBy = this.session.UserID;
      string str = "Copy of " + selectedSet.SetName;
      int num1 = 1;
      conditionset.Description = selectedSet.Description;
      conditionset.ConditionTemplates = selectedSet.ConditionTemplates;
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvConditionSets.Items)
      {
        if (gvItem.SubItems[0].Text.ToLower().IndexOf(str.ToLower()) == 0)
          ++num1;
      }
      if (num1 < 2)
        conditionset.SetName = str;
      else
        conditionset.SetName = str + "(" + (object) num1 + ")";
      if (conditionset.SetName.Length > 200)
        conditionset.SetName = conditionset.SetName.Substring(0, 200);
      try
      {
        if (this.session.ConfigurationManager.IsUniqueSetName(conditionset.SetName, conditionset.Id))
          this.session.ConfigurationManager.UpdateEnhancedConditionSet(conditionset);
        else if (Utils.Dialog((IWin32Window) Form.ActiveForm, "There is a duplicate already exists", MessageBoxButtons.OK, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.OK)
          return;
        this.loadConditionSetList(conditionset.SetName);
      }
      catch (Exception ex)
      {
        MetricsFactory.IncrementErrorCounter(ex, "The following error occurred when", "D:\\ws\\24.3.0.0\\EmLite\\SetupUI\\Setup\\eFolder\\EnhancedConditionSetsControl.cs", nameof (btnDuplicate_Click), 229);
        int num2 = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "The following error occurred when trying to save the '" + conditionset.SetName + "' condition set:\r\n\r\n" + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void btnEdit_Click(object sender, EventArgs e)
    {
      Guid tag = (Guid) this.gvConditionSets.SelectedItems[0].Tag;
      string text = this.gvConditionSets.SelectedItems[0].Text;
      using (EditEnhancedConditionSet enhancedConditionSet = new EditEnhancedConditionSet(this.session, (object) tag))
      {
        if (enhancedConditionSet.ShowDialog((IWin32Window) Form.ActiveForm) != DialogResult.OK)
          return;
        this.loadConditionSetList(enhancedConditionSet.SetName);
      }
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
      if (Utils.Dialog((IWin32Window) Form.ActiveForm, "Are you sure that you want to permanently delete the selected condition sets?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
        return;
      foreach (GVItem selectedItem in this.gvConditionSets.SelectedItems)
      {
        EnhancedConditionSet conditionset = new EnhancedConditionSet();
        conditionset.Id = (Guid) selectedItem.Tag;
        conditionset.SetName = selectedItem.SubItems[0].Text;
        conditionset.Description = selectedItem.SubItems[1].Text;
        try
        {
          conditionset.Deleted = true;
          this.session.ConfigurationManager.UpdateEnhancedConditionSet(conditionset);
        }
        catch (Exception ex)
        {
          MetricsFactory.IncrementErrorCounter(ex, "The following error occurred when", "D:\\ws\\24.3.0.0\\EmLite\\SetupUI\\Setup\\eFolder\\EnhancedConditionSetsControl.cs", nameof (btnDelete_Click), 286);
          int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "The following error occurred when trying to save the delete the '" + conditionset.SetName + "' condition set:\r\n\r\n" + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
      }
      this.loadConditionSetList((string) null);
    }

    public void SetSelectedItems(
      AclFileType type,
      string[] fileTypePathList,
      Dictionary<string, string> conditionNewGuids = null)
    {
    }

    public Dictionary<string, string> SelectedConditionNames
    {
      get
      {
        Dictionary<string, string> dictionary = new Dictionary<string, string>();
        return this.gvConditionSets.SelectedItems.Count == 0 ? (Dictionary<string, string>) null : dictionary;
      }
    }

    public Dictionary<string, string> GetSelectedConditionNewGuids(
      Dictionary<string, string> selectedConditions)
    {
      Dictionary<string, string> dictionary = new Dictionary<string, string>();
      return selectedConditions.Count == 0 ? (Dictionary<string, string>) null : dictionary;
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
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      this.gcConditionSets = new GroupContainer();
      this.btnNew = new StandardIconButton();
      this.btnDuplicate = new StandardIconButton();
      this.btnEdit = new StandardIconButton();
      this.btnDelete = new StandardIconButton();
      this.gvConditionSets = new GridView();
      this.tooltip = new ToolTip(this.components);
      this.gcConditionSets.SuspendLayout();
      ((ISupportInitialize) this.btnNew).BeginInit();
      ((ISupportInitialize) this.btnDuplicate).BeginInit();
      ((ISupportInitialize) this.btnEdit).BeginInit();
      ((ISupportInitialize) this.btnDelete).BeginInit();
      this.SuspendLayout();
      this.gcConditionSets.Controls.Add((Control) this.btnNew);
      this.gcConditionSets.Controls.Add((Control) this.btnDuplicate);
      this.gcConditionSets.Controls.Add((Control) this.btnEdit);
      this.gcConditionSets.Controls.Add((Control) this.btnDelete);
      this.gcConditionSets.Controls.Add((Control) this.gvConditionSets);
      this.gcConditionSets.Dock = DockStyle.Fill;
      this.gcConditionSets.Location = new Point(0, 0);
      this.gcConditionSets.Name = "gcConditionSets";
      this.gcConditionSets.Size = new Size(581, 316);
      this.gcConditionSets.TabIndex = 0;
      this.gcConditionSets.Text = "Enhanced Condition Sets";
      this.btnNew.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnNew.BackColor = Color.Transparent;
      this.btnNew.Location = new Point(493, 5);
      this.btnNew.Name = "btnNew";
      this.btnNew.Size = new Size(16, 16);
      this.btnNew.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnNew.TabIndex = 5;
      this.btnNew.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnNew, "New");
      this.btnNew.Click += new EventHandler(this.btnNew_Click);
      this.btnDuplicate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDuplicate.BackColor = Color.Transparent;
      this.btnDuplicate.Enabled = false;
      this.btnDuplicate.Location = new Point(515, 5);
      this.btnDuplicate.Name = "btnDuplicate";
      this.btnDuplicate.Size = new Size(16, 16);
      this.btnDuplicate.StandardButtonType = StandardIconButton.ButtonType.DuplicateButton;
      this.btnDuplicate.TabIndex = 4;
      this.btnDuplicate.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnDuplicate, "Duplicate");
      this.btnDuplicate.Click += new EventHandler(this.btnDuplicate_Click);
      this.btnEdit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnEdit.BackColor = Color.Transparent;
      this.btnEdit.Enabled = false;
      this.btnEdit.Location = new Point(537, 5);
      this.btnEdit.Name = "btnEdit";
      this.btnEdit.Size = new Size(16, 16);
      this.btnEdit.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.btnEdit.TabIndex = 3;
      this.btnEdit.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnEdit, "Edit");
      this.btnEdit.Click += new EventHandler(this.btnEdit_Click);
      this.btnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDelete.BackColor = Color.Transparent;
      this.btnDelete.Enabled = false;
      this.btnDelete.Location = new Point(559, 5);
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new Size(16, 16);
      this.btnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnDelete.TabIndex = 2;
      this.btnDelete.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnDelete, "Delete");
      this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
      this.gvConditionSets.BorderStyle = BorderStyle.None;
      this.gvConditionSets.ClearSelectionsOnEmptyRowClick = false;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Name";
      gvColumn1.Width = 237;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Description";
      gvColumn2.Width = 312;
      this.gvConditionSets.Columns.AddRange(new GVColumn[2]
      {
        gvColumn1,
        gvColumn2
      });
      this.gvConditionSets.Dock = DockStyle.Fill;
      this.gvConditionSets.HoverToolTip = this.tooltip;
      this.gvConditionSets.Location = new Point(1, 26);
      this.gvConditionSets.Name = "gvConditionSets";
      this.gvConditionSets.Size = new Size(579, 289);
      this.gvConditionSets.TabIndex = 1;
      this.gvConditionSets.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gvConditionSets.SelectedIndexChanged += new EventHandler(this.gvConditionSets_SelectedIndexChanged);
      this.gvConditionSets.ItemDoubleClick += new GVItemEventHandler(this.gvConditionSets_ItemDoubleClick);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.gcConditionSets);
      this.Name = "ConditionSetsSetupControl";
      this.Size = new Size(581, 316);
      this.gcConditionSets.ResumeLayout(false);
      ((ISupportInitialize) this.btnNew).EndInit();
      ((ISupportInitialize) this.btnDuplicate).EndInit();
      ((ISupportInitialize) this.btnEdit).EndInit();
      ((ISupportInitialize) this.btnDelete).EndInit();
      this.ResumeLayout(false);
    }
  }
}
