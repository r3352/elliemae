// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.eFolder.ConditionSetsSetupControl
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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.eFolder
{
  public class ConditionSetsSetupControl : UserControl
  {
    private ConditionType condType;
    private TemplateSettingsType templateType;
    private Sessions.Session session;
    private IContainer components;
    private GroupContainer gcConditionSets;
    private StandardIconButton btnNew;
    private StandardIconButton btnDuplicate;
    private StandardIconButton btnEdit;
    private StandardIconButton btnDelete;
    private GridView gvConditionSets;
    private ToolTip tooltip;

    public ConditionSetsSetupControl(Sessions.Session session, ConditionType condType)
    {
      this.InitializeComponent();
      this.session = session;
      this.condType = condType;
      this.templateType = TemplateSettingsTypeConverter.FromConditionType(condType);
      this.initConditionSetList();
      this.loadConditionSetList((string) null);
    }

    private void initConditionSetList() => this.gvConditionSets.Sort(0, SortOrder.Ascending);

    private void loadConditionSetList(string defaultItem)
    {
      this.gvConditionSets.Items.Clear();
      foreach (FileSystemEntry templateDirEntry in this.session.ConfigurationManager.GetTemplateDirEntries(this.templateType, FileSystemEntry.PublicRoot))
      {
        string str = string.Concat(templateDirEntry.Properties[(object) "Description"]);
        GVItem gvItem = this.gvConditionSets.Items.Add(templateDirEntry.Name);
        gvItem.SubItems.Add((object) str);
        gvItem.Tag = (object) templateDirEntry;
        if (templateDirEntry.Name == defaultItem)
          gvItem.Selected = true;
      }
      this.gcConditionSets.Text = "Condition Sets (" + this.gvConditionSets.Items.Count.ToString() + ")";
      this.gvConditionSets.ReSort();
    }

    private void editConditionSet(ConditionSetTemplate template)
    {
      using (ConditionSetTemplateDialog setTemplateDialog = new ConditionSetTemplateDialog(template, this.session))
      {
        if (setTemplateDialog.ShowDialog((IWin32Window) Form.ActiveForm) != DialogResult.OK)
          return;
        this.loadConditionSetList(template.TemplateName);
      }
    }

    private ConditionSetTemplate getSelectedTemplate()
    {
      if (this.gvConditionSets.SelectedItems.Count != 1)
        return (ConditionSetTemplate) null;
      FileSystemEntry tag = (FileSystemEntry) this.gvConditionSets.SelectedItems[0].Tag;
      try
      {
        return (ConditionSetTemplate) this.session.ConfigurationManager.GetTemplateSettings(this.templateType, tag);
      }
      catch (Exception ex)
      {
        MetricsFactory.IncrementErrorCounter(ex, "The following error occurred when", "D:\\ws\\24.3.0.0\\EmLite\\SetupUI\\Setup\\eFolder\\ConditionSetsSetupControl.cs", nameof (getSelectedTemplate), 119);
        int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "The following error occurred when trying to retrieve the '" + tag.Name + "' condition set:\r\n\r\n" + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return (ConditionSetTemplate) null;
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
      this.editConditionSet(new ConditionSetTemplate(this.condType));
    }

    private void btnDuplicate_Click(object sender, EventArgs e)
    {
      ConditionSetTemplate selectedTemplate = this.getSelectedTemplate();
      if (selectedTemplate == null)
        return;
      string defaultItem = "Copy of " + selectedTemplate.TemplateName;
      string str = defaultItem;
      int num1 = 2;
      FileSystemEntry fileSystemEntry;
      for (fileSystemEntry = FileSystemEntry.Parse("public:\\" + defaultItem); this.session.ConfigurationManager.TemplateSettingsObjectExists(this.templateType, fileSystemEntry); fileSystemEntry = FileSystemEntry.Parse("public:\\" + defaultItem))
        defaultItem = str + " (" + (object) num1++ + ")";
      ConditionSetTemplate data = new ConditionSetTemplate(this.condType);
      data.TemplateName = defaultItem;
      data.Description = selectedTemplate.Description;
      foreach (string condition in selectedTemplate.Conditions)
        data.Conditions.Add((object) condition);
      try
      {
        this.session.ConfigurationManager.SaveTemplateSettings(this.templateType, fileSystemEntry, (BinaryObject) (BinaryConvertibleObject) data);
        this.loadConditionSetList(defaultItem);
      }
      catch (Exception ex)
      {
        MetricsFactory.IncrementErrorCounter(ex, "The following error occurred when", "D:\\ws\\24.3.0.0\\EmLite\\SetupUI\\Setup\\eFolder\\ConditionSetsSetupControl.cs", nameof (btnDuplicate_Click), 201);
        int num2 = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "The following error occurred when trying to save the '" + fileSystemEntry.Name + "' condition set:\r\n\r\n" + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void btnEdit_Click(object sender, EventArgs e)
    {
      ConditionSetTemplate selectedTemplate = this.getSelectedTemplate();
      if (selectedTemplate == null)
        return;
      this.editConditionSet(selectedTemplate);
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
      if (Utils.Dialog((IWin32Window) Form.ActiveForm, "Are you sure that you want to permanently delete the selected condition sets?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
        return;
      foreach (GVItem selectedItem in this.gvConditionSets.SelectedItems)
      {
        FileSystemEntry tag = (FileSystemEntry) selectedItem.Tag;
        try
        {
          this.session.ConfigurationManager.DeleteTemplateSettingsObject(this.templateType, tag);
        }
        catch (Exception ex)
        {
          MetricsFactory.IncrementErrorCounter(ex, "The following error occurred when", "D:\\ws\\24.3.0.0\\EmLite\\SetupUI\\Setup\\eFolder\\ConditionSetsSetupControl.cs", nameof (btnDelete_Click), 245);
          int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "The following error occurred when trying to save the delete the '" + tag.Name + "' condition set:\r\n\r\n" + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
      }
      this.loadConditionSetList((string) null);
    }

    public void SetSelectedItems(
      AclFileType type,
      string[] fileTypePathList,
      Dictionary<string, string> conditionNewGuids = null)
    {
      List<string> stringList = new List<string>((IEnumerable<string>) fileTypePathList);
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvConditionSets.Items)
      {
        FileSystemEntry tag = (FileSystemEntry) gvItem.Tag;
        AclFileResource aclFileResource = new AclFileResource(-1, tag.ToString(), type, tag.Type == FileSystemEntry.Types.Folder, tag.Owner);
        if (stringList.Contains(aclFileResource.FileTypePath))
        {
          gvItem.Selected = true;
          if (conditionNewGuids != null)
          {
            ConditionSetTemplate templateSettings = (ConditionSetTemplate) this.session.ConfigurationManager.GetTemplateSettings(this.templateType, tag);
            ArrayList arrayList = (ArrayList) templateSettings.Conditions.Clone();
            templateSettings.Conditions.Clear();
            bool flag = false;
            foreach (string key in arrayList)
            {
              string str;
              if (conditionNewGuids.ContainsKey(key))
              {
                str = conditionNewGuids[key];
                flag = true;
              }
              else
                str = key;
              templateSettings.Conditions.Add((object) str);
            }
            if (flag)
              this.session.ConfigurationManager.SaveTemplateSettings(this.templateType, tag, (BinaryObject) (BinaryConvertibleObject) templateSettings);
          }
        }
        else
          gvItem.Selected = false;
      }
    }

    public List<FileSystemEntry> SelectedFileSystemEntries
    {
      get
      {
        List<FileSystemEntry> fileSystemEntries = new List<FileSystemEntry>();
        foreach (GVItem selectedItem in this.gvConditionSets.SelectedItems)
          fileSystemEntries.Add((FileSystemEntry) selectedItem.Tag);
        return fileSystemEntries;
      }
    }

    public Dictionary<string, string> SelectedConditionNames
    {
      get
      {
        Dictionary<string, string> selectedConditionNames = new Dictionary<string, string>();
        if (this.gvConditionSets.SelectedItems.Count == 0)
          return (Dictionary<string, string>) null;
        ConditionTrackingSetup conditionTrackingSetup = this.session.ConfigurationManager.GetConditionTrackingSetup(this.condType);
        foreach (GVItem selectedItem in this.gvConditionSets.SelectedItems)
        {
          FileSystemEntry tag = (FileSystemEntry) selectedItem.Tag;
          try
          {
            foreach (string condition in ((ConditionSetTemplate) this.session.ConfigurationManager.GetTemplateSettings(this.templateType, tag)).Conditions)
            {
              if (!selectedConditionNames.ContainsKey(condition))
                selectedConditionNames.Add(condition, conditionTrackingSetup.GetByID(condition).Name);
            }
          }
          catch (Exception ex)
          {
            MetricsFactory.IncrementErrorCounter(ex, "The following error occurred when", "D:\\ws\\24.3.0.0\\EmLite\\SetupUI\\Setup\\eFolder\\ConditionSetsSetupControl.cs", nameof (SelectedConditionNames), 354);
            int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "The following error occurred when trying to retrieve the '" + tag.Name + "' condition set:\r\n\r\n" + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return (Dictionary<string, string>) null;
          }
        }
        return selectedConditionNames;
      }
    }

    public Dictionary<string, string> GetSelectedConditionNewGuids(
      Dictionary<string, string> selectedConditions)
    {
      Dictionary<string, string> conditionNewGuids = new Dictionary<string, string>();
      if (selectedConditions.Count == 0)
        return (Dictionary<string, string>) null;
      ConditionTrackingSetup conditionTrackingSetup = this.session.ConfigurationManager.GetConditionTrackingSetup(this.condType);
      foreach (KeyValuePair<string, string> keyValuePair in selectedConditions.ToArray<KeyValuePair<string, string>>())
      {
        if (!conditionNewGuids.ContainsKey(keyValuePair.Value))
        {
          ConditionTemplate byName = conditionTrackingSetup.GetByName(keyValuePair.Value);
          if (byName != null)
          {
            string guid = byName.Guid;
            if (!guid.Equals(keyValuePair.Key))
              conditionNewGuids.Add(keyValuePair.Key, guid);
          }
        }
      }
      return conditionNewGuids;
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
      this.gcConditionSets.Text = "Condition Sets (0)";
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
      this.Name = nameof (ConditionSetsSetupControl);
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
