// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.SyncTemplateSetupPanel
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class SyncTemplateSetupPanel : UserControl
  {
    private Sessions.Session session;
    private FeaturesAclManager aclMgr;
    private FieldSettings fieldSettings;
    private string containerHeader = "Sync Templates";
    private GridView listView;
    private GroupContainer gContainer;
    private StandardIconButton stdIconBtnEdit;
    private StandardIconButton stdIconBtnNew;
    private StandardIconButton stdIconBtnDelete;
    private ToolTip toolTip1;
    private StandardIconButton stdIconBtnDuplicate;
    private IContainer components;
    private Panel panelList;

    public SyncTemplateSetupPanel(Sessions.Session session)
    {
      this.session = session;
      this.InitializeComponent();
      this.aclMgr = (FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features);
      this.initForm();
      this.listView.SelectedIndexChanged += new EventHandler(this.listView_SelectedIndexChanged);
      this.fieldSettings = this.session.LoanManager.GetFieldSettings();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void initForm()
    {
      this.listView.Items.Clear();
      this.listView.BeginUpdate();
      List<SyncTemplate> allSyncTemplates = this.session.ConfigurationManager.GetAllSyncTemplates();
      if (allSyncTemplates != null)
      {
        foreach (SyncTemplate syncTemplate in allSyncTemplates)
          this.listView.Items.Add(this.buildGVItem(syncTemplate, false));
      }
      this.listView.Sort(0, SortOrder.Ascending);
      this.listView.EndUpdate();
      this.listView_SelectedIndexChanged((object) null, (EventArgs) null);
      this.refreshListViewHeader();
    }

    private GVItem buildGVItem(SyncTemplate syncTemplate, bool selected)
    {
      return new GVItem(syncTemplate.TemplateName)
      {
        SubItems = {
          (object) syncTemplate.TemplateDescription
        },
        Selected = selected,
        Tag = (object) syncTemplate
      };
    }

    private void refreshListViewHeader()
    {
      this.gContainer.Text = this.containerHeader + " (" + (object) this.listView.Items.Count + ")";
    }

    private void newBtn_Click(object sender, EventArgs e)
    {
      using (SyncTemplateSetupDialog templateSetupDialog = new SyncTemplateSetupDialog(this.session, this.fieldSettings, (SyncTemplate) null))
      {
        if (templateSetupDialog.ShowDialog() != DialogResult.OK)
          return;
        int num = this.session.ConfigurationManager.UpdateSyncTemplate(templateSetupDialog.NewSyncTemplate);
        templateSetupDialog.NewSyncTemplate.TemplateID = num;
        this.listView.SelectedItems.Clear();
        this.listView.Items.Add(this.buildGVItem(templateSetupDialog.NewSyncTemplate, true));
        this.refreshListViewHeader();
      }
    }

    private void editSelectedItem()
    {
      using (SyncTemplateSetupDialog templateSetupDialog = new SyncTemplateSetupDialog(this.session, this.fieldSettings, (SyncTemplate) this.listView.SelectedItems[0].Tag))
      {
        if (templateSetupDialog.ShowDialog() != DialogResult.OK)
          return;
        this.session.ConfigurationManager.UpdateSyncTemplate(templateSetupDialog.NewSyncTemplate);
        this.listView.SelectedItems[0].Text = templateSetupDialog.NewSyncTemplate.TemplateName;
        this.listView.SelectedItems[0].SubItems[1].Text = templateSetupDialog.NewSyncTemplate.TemplateDescription;
        this.refreshListViewHeader();
      }
    }

    private void deleteBtn_Click(object sender, EventArgs e)
    {
      if (Utils.Dialog((IWin32Window) this, "The selected Sync Template" + (this.listView.SelectedItems.Count > 1 ? "s" : "") + " will be deleted permanently. Do you want to continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
        return;
      int index1 = this.listView.SelectedItems[0].Index;
      List<int> ids = new List<int>();
      foreach (GVItem selectedItem in this.listView.SelectedItems)
      {
        SyncTemplate tag = (SyncTemplate) selectedItem.Tag;
        ids.Add(tag.TemplateID);
      }
      try
      {
        List<int> intList = Session.ConfigurationManager.RemoveSyncTemplates(ids);
        string str = string.Empty;
        for (int index2 = this.listView.SelectedItems.Count - 1; index2 >= 0; --index2)
        {
          SyncTemplate tag = (SyncTemplate) this.listView.SelectedItems[index2].Tag;
          if (intList != null && intList.Contains(tag.TemplateID))
            this.listView.Items.Remove(this.listView.SelectedItems[index2]);
          else
            str = str + (str != string.Empty ? "," : "") + "\"" + tag.TemplateName + "\"";
        }
        if (intList.Count != ids.Count)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "The following Sync Template" + (intList.Count > 1 ? "s" : "") + " cannot be deleted:\r\n\r\n" + str, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        this.refreshListViewHeader();
        if (this.listView.Items.Count == 0)
          return;
        this.listView.SelectedItems.Clear();
        if (this.listView.Items.Count < index1 + 1)
          this.listView.Items[this.listView.Items.Count - 1].Selected = true;
        else
          this.listView.Items[index1].Selected = true;
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
    }

    private void listView_ItemDoubleClick(object source, GVItemEventArgs e)
    {
      this.editSelectedItem();
    }

    private void listView_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.stdIconBtnEdit.Enabled = this.stdIconBtnDuplicate.Enabled = this.listView.SelectedItems.Count == 1;
      this.stdIconBtnDelete.Enabled = this.listView.SelectedItems.Count > 0 && (this.session.UserInfo.IsAdministrator() || this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_LOCompensation));
    }

    private void editBtn_Click(object sender, EventArgs e) => this.editSelectedItem();

    private void stdIconBtnDuplicate_Click(object sender, EventArgs e)
    {
      using (SyncTemplateSetupDialog templateSetupDialog = new SyncTemplateSetupDialog(this.session, this.fieldSettings, ((SyncTemplate) this.listView.SelectedItems[0].Tag).Clone()))
      {
        if (templateSetupDialog.ShowDialog() != DialogResult.OK)
          return;
        int num = this.session.ConfigurationManager.UpdateSyncTemplate(templateSetupDialog.NewSyncTemplate);
        templateSetupDialog.NewSyncTemplate.TemplateID = num;
        this.listView.SelectedItems.Clear();
        this.listView.Items.Add(this.buildGVItem(templateSetupDialog.NewSyncTemplate, true));
        this.refreshListViewHeader();
      }
    }

    private void InitializeComponent()
    {
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      this.toolTip1 = new ToolTip();
      this.panelList = new Panel();
      this.gContainer = new GroupContainer();
      this.listView = new GridView();
      this.stdIconBtnDuplicate = new StandardIconButton();
      this.stdIconBtnEdit = new StandardIconButton();
      this.stdIconBtnNew = new StandardIconButton();
      this.stdIconBtnDelete = new StandardIconButton();
      this.panelList.SuspendLayout();
      this.gContainer.SuspendLayout();
      ((ISupportInitialize) this.stdIconBtnDuplicate).BeginInit();
      ((ISupportInitialize) this.stdIconBtnEdit).BeginInit();
      ((ISupportInitialize) this.stdIconBtnNew).BeginInit();
      ((ISupportInitialize) this.stdIconBtnDelete).BeginInit();
      this.SuspendLayout();
      this.panelList.Controls.Add((Control) this.gContainer);
      this.panelList.Dock = DockStyle.Fill;
      this.panelList.Location = new Point(0, 0);
      this.panelList.Name = "panelList";
      this.panelList.Size = new Size(957, 653);
      this.panelList.TabIndex = 11;
      this.gContainer.Controls.Add((Control) this.stdIconBtnDuplicate);
      this.gContainer.Controls.Add((Control) this.stdIconBtnEdit);
      this.gContainer.Controls.Add((Control) this.stdIconBtnNew);
      this.gContainer.Controls.Add((Control) this.stdIconBtnDelete);
      this.gContainer.Controls.Add((Control) this.listView);
      this.gContainer.Dock = DockStyle.Fill;
      this.gContainer.HeaderForeColor = SystemColors.ControlText;
      this.gContainer.Location = new Point(0, 0);
      this.gContainer.Name = "gContainer";
      this.gContainer.Size = new Size(957, 653);
      this.gContainer.TabIndex = 8;
      this.listView.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Name";
      gvColumn1.Text = "Name";
      gvColumn1.Width = 300;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Description";
      gvColumn2.SpringToFit = true;
      gvColumn2.Text = "Description";
      gvColumn2.Width = 655;
      this.listView.Columns.AddRange(new GVColumn[2]
      {
        gvColumn1,
        gvColumn2
      });
      this.listView.Dock = DockStyle.Fill;
      this.listView.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.listView.Location = new Point(1, 26);
      this.listView.Name = "listView";
      this.listView.Size = new Size(955, 626);
      this.listView.TabIndex = 7;
      this.listView.ItemDoubleClick += new GVItemEventHandler(this.listView_ItemDoubleClick);
      this.stdIconBtnDuplicate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnDuplicate.BackColor = Color.Transparent;
      this.stdIconBtnDuplicate.Location = new Point(885, 4);
      this.stdIconBtnDuplicate.MouseDownImage = (Image) null;
      this.stdIconBtnDuplicate.Name = "stdIconBtnDuplicate";
      this.stdIconBtnDuplicate.Size = new Size(16, 16);
      this.stdIconBtnDuplicate.StandardButtonType = StandardIconButton.ButtonType.DuplicateButton;
      this.stdIconBtnDuplicate.TabIndex = 13;
      this.stdIconBtnDuplicate.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnDuplicate, "Duplicate");
      this.stdIconBtnDuplicate.Click += new EventHandler(this.stdIconBtnDuplicate_Click);
      this.stdIconBtnEdit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnEdit.BackColor = Color.Transparent;
      this.stdIconBtnEdit.Location = new Point(909, 4);
      this.stdIconBtnEdit.MouseDownImage = (Image) null;
      this.stdIconBtnEdit.Name = "stdIconBtnEdit";
      this.stdIconBtnEdit.Size = new Size(16, 16);
      this.stdIconBtnEdit.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.stdIconBtnEdit.TabIndex = 11;
      this.stdIconBtnEdit.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnEdit, "Edit");
      this.stdIconBtnEdit.Click += new EventHandler(this.editBtn_Click);
      this.stdIconBtnNew.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnNew.BackColor = Color.Transparent;
      this.stdIconBtnNew.Location = new Point(861, 4);
      this.stdIconBtnNew.MouseDownImage = (Image) null;
      this.stdIconBtnNew.Name = "stdIconBtnNew";
      this.stdIconBtnNew.Size = new Size(16, 16);
      this.stdIconBtnNew.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.stdIconBtnNew.TabIndex = 10;
      this.stdIconBtnNew.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnNew, "New");
      this.stdIconBtnNew.Click += new EventHandler(this.newBtn_Click);
      this.stdIconBtnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnDelete.BackColor = Color.Transparent;
      this.stdIconBtnDelete.Location = new Point(933, 4);
      this.stdIconBtnDelete.MouseDownImage = (Image) null;
      this.stdIconBtnDelete.Name = "stdIconBtnDelete";
      this.stdIconBtnDelete.Size = new Size(16, 16);
      this.stdIconBtnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.stdIconBtnDelete.TabIndex = 9;
      this.stdIconBtnDelete.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnDelete, "Delete");
      this.stdIconBtnDelete.Click += new EventHandler(this.deleteBtn_Click);
      this.Controls.Add((Control) this.panelList);
      this.Name = nameof (SyncTemplateSetupPanel);
      this.Size = new Size(957, 653);
      this.panelList.ResumeLayout(false);
      this.gContainer.ResumeLayout(false);
      ((ISupportInitialize) this.stdIconBtnDuplicate).EndInit();
      ((ISupportInitialize) this.stdIconBtnEdit).EndInit();
      ((ISupportInitialize) this.stdIconBtnNew).EndInit();
      ((ISupportInitialize) this.stdIconBtnDelete).EndInit();
      this.ResumeLayout(false);
    }
  }
}
