// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.eFolder.DocumentGroupSetupControl
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.ePass;
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
  public class DocumentGroupSetupControl : SettingsUserControl
  {
    private DocumentGroupSetup groupSetup;
    private DocumentTrackingSetup docSetup;
    private DocumentGroup allGroup;
    private DocumentGroup closingGroup;
    private DocumentGroup disclosuresGroup;
    private DocumentGroup epassGroup;
    private GridViewDataManager gvDocumentsMgr;
    private GridViewDataManager gvGroupsMgr;
    private Point dragPoint = Point.Empty;
    private Sessions.Session session;
    private IContainer components;
    private StandardIconButton btnNewGroup;
    private ToolTip toolTip;
    private GroupContainer gcDocuments;
    private StandardIconButton btnDeleteGroup;
    private GroupContainer gcGroups;
    private ImageList imageList;
    private GridView gvDocuments;
    private GridView gvGroups;
    private Splitter splitter1;

    public DocumentGroupSetupControl(SetUpContainer setupContainer)
      : this(setupContainer, Session.DefaultInstance, false)
    {
    }

    public DocumentGroupSetupControl(
      SetUpContainer setupContainer,
      Sessions.Session session,
      bool allowMultiSelect)
      : base(setupContainer)
    {
      this.session = session;
      this.InitializeComponent();
      this.groupSetup = this.session.ConfigurationManager.GetDocumentGroupSetup();
      this.docSetup = this.session.ConfigurationManager.GetDocumentTrackingSetup();
      this.initDocumentList();
      this.initGroupList();
      this.loadDocumentList();
      this.loadGroupList();
      this.gvGroups.AllowMultiselect = allowMultiSelect;
      this.gvDocuments.Enabled = this.btnNewGroup.Visible = this.btnDeleteGroup.Visible = !allowMultiSelect;
    }

    private void initDocumentList()
    {
      this.gvDocumentsMgr = new GridViewDataManager(this.session, this.gvDocuments, (LoanDataMgr) null);
      this.gvDocumentsMgr.CreateLayout(new TableLayout.Column[1]
      {
        GridViewDataManager.NameColumn
      });
      this.gvDocuments.Sort(0, SortOrder.Ascending);
    }

    private void loadDocumentList()
    {
      foreach (DocumentTemplate template in this.docSetup)
        this.gvDocumentsMgr.AddItem(template).ImageIndex = 1;
      this.gvDocuments.ReSort();
    }

    private void initGroupList()
    {
      this.allGroup = new DocumentGroup("(All Documents)");
      this.closingGroup = new DocumentGroup("(Closing Documents)");
      this.disclosuresGroup = new DocumentGroup("(eDisclosures)");
      this.epassGroup = new DocumentGroup("(Settlement Services)");
      this.gvGroupsMgr = new GridViewDataManager(this.session, this.gvGroups, (LoanDataMgr) null);
      this.gvGroupsMgr.CreateLayout(new TableLayout.Column[1]
      {
        GridViewDataManager.NameColumn
      });
      this.gvGroups.Columns[0].ActivatedEditorType = GVActivatedEditorType.TextBox;
      this.gvGroups.Sort(0, SortOrder.Ascending);
    }

    private void loadGroupList()
    {
      this.gvGroupsMgr.ClearItems();
      this.addGroup(this.allGroup, false);
      this.addGroup(this.closingGroup, false);
      this.addGroup(this.disclosuresGroup, false);
      this.addGroup(this.epassGroup, false);
      foreach (DocumentGroup group in (CollectionBase) this.groupSetup)
        this.addGroup(group, false);
      this.gvGroups.ReSort();
      this.setDirtyFlag(false);
    }

    private GVItem addGroup(DocumentGroup group, bool selected)
    {
      List<DocumentTemplate> documentTemplateList = new List<DocumentTemplate>();
      if (group == this.allGroup)
        documentTemplateList.AddRange((IEnumerable<DocumentTemplate>) this.docSetup.ToArray());
      else if (group == this.closingGroup)
      {
        foreach (DocumentTemplate documentTemplate in this.docSetup)
        {
          if (documentTemplate.ClosingDocument)
            documentTemplateList.Add(documentTemplate);
        }
      }
      else if (group == this.disclosuresGroup)
      {
        foreach (DocumentTemplate documentTemplate in this.docSetup)
        {
          if (documentTemplate.OpeningDocument)
            documentTemplateList.Add(documentTemplate);
        }
      }
      else if (group == this.epassGroup)
      {
        foreach (DocumentTemplate documentTemplate in this.docSetup)
        {
          if (Epass.IsEpassDoc(documentTemplate.Name))
            documentTemplateList.Add(documentTemplate);
        }
      }
      else
        documentTemplateList.AddRange((IEnumerable<DocumentTemplate>) group.GetDocuments(this.docSetup));
      GVItem gvItem1 = this.gvGroupsMgr.AddItem(group);
      gvItem1.State = GVItemState.Collapsed;
      gvItem1.ImageIndex = 0;
      gvItem1.Selected = selected;
      foreach (DocumentTemplate template in documentTemplateList)
      {
        GVItem gvItem2 = this.gvGroupsMgr.CreateItem(template);
        gvItem2.ImageIndex = 1;
        gvItem1.GroupItems.Add(gvItem2);
      }
      return gvItem1;
    }

    private void btnNewGroup_Click(object sender, EventArgs e)
    {
      if (!this.gvGroups.StopEditing())
        return;
      DocumentGroup group = new DocumentGroup("New Group");
      List<string> stringList = new List<string>();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvGroups.Items)
        stringList.Add(gvItem.Text);
      for (int index = 2; index <= 10000 && stringList.Contains(group.Name); ++index)
        group.Name = "New Group (" + index.ToString() + ")";
      this.gvGroups.SelectedItems.Clear();
      GVItem gvItem1 = this.addGroup(group, true);
      this.setDirtyFlag(true);
      this.gvGroups.StartEditing(gvItem1.SubItems[0]);
    }

    private void btnDeleteGroup_Click(object sender, EventArgs e)
    {
      if (!this.gvGroups.StopEditing())
        return;
      this.gvGroups.Items.Remove(this.gvGroups.SelectedItems[0]);
      this.setDirtyFlag(true);
    }

    private void gvGroups_EditorOpening(object source, GVSubItemEditingEventArgs e)
    {
      if (e.SubItem.Item.Tag is DocumentTemplate)
        e.Cancel = true;
      if (e.SubItem.Item.Tag != this.allGroup && e.SubItem.Item.Tag != this.closingGroup && e.SubItem.Item.Tag != this.disclosuresGroup && e.SubItem.Item.Tag != this.epassGroup)
        return;
      e.Cancel = true;
    }

    private void gvGroups_EditorClosing(object source, GVSubItemEditingEventArgs e)
    {
      e.EditorControl.Text = e.EditorControl.Text.Trim();
      if (e.EditorControl.Text == e.SubItem.Text || e.EditorControl.Text == string.Empty)
      {
        this.gvGroups.CancelEditing();
      }
      else
      {
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvGroups.Items)
        {
          if (!(gvItem.Text != e.EditorControl.Text))
          {
            this.gvGroups.CancelEditing();
            int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "A group with the name you specified already exists. Specify a different group name.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return;
          }
        }
        this.setDirtyFlag(true);
      }
    }

    private void gvGroups_SelectedIndexChanged(object sender, EventArgs e)
    {
      bool flag = false;
      if (this.gvGroups.SelectedItems.Count > 0 && this.gvGroups.SelectedItems[0].Tag is DocumentGroup && this.gvGroups.SelectedItems[0].Tag != this.allGroup && this.gvGroups.SelectedItems[0].Tag != this.closingGroup && this.gvGroups.SelectedItems[0].Tag != this.disclosuresGroup && this.gvGroups.SelectedItems[0].Tag != this.epassGroup)
        flag = true;
      this.btnDeleteGroup.Enabled = flag;
    }

    private void gvDocuments_MouseDown(object sender, MouseEventArgs e)
    {
      if (this.gvDocuments.GetItemAt(e.X, e.Y) == null)
        return;
      this.dragPoint = e.Location;
    }

    private void gvDocuments_MouseMove(object sender, MouseEventArgs e)
    {
      if (!(this.dragPoint != Point.Empty) || this.dragPoint.Equals((object) e.Location))
        return;
      int num = (int) this.gvDocuments.DoDragDrop((object) this.gvDocuments.SelectedItems, DragDropEffects.Move);
      this.dragPoint = Point.Empty;
    }

    private void gvDocuments_MouseUp(object sender, MouseEventArgs e)
    {
      this.dragPoint = Point.Empty;
    }

    private void gvDocuments_DragEnter(object sender, DragEventArgs e)
    {
      if (e.Data.GetDataPresent(typeof (GVItem)))
        e.Effect = DragDropEffects.Move;
      else
        e.Effect = DragDropEffects.None;
    }

    private void gvDocuments_DragDrop(object sender, DragEventArgs e)
    {
      GVItem data = (GVItem) e.Data.GetData(typeof (GVItem));
      data.ParentItem.GroupItems.Remove(data);
      this.setDirtyFlag(true);
    }

    private void gvGroups_MouseDown(object sender, MouseEventArgs e)
    {
      GVItem itemAt = this.gvGroups.GetItemAt(e.X, e.Y);
      if (itemAt == null || !(itemAt.Tag is DocumentTemplate) || itemAt.ParentItem.Tag == this.allGroup || itemAt.ParentItem.Tag == this.closingGroup || itemAt.ParentItem.Tag == this.disclosuresGroup || itemAt.ParentItem.Tag == this.epassGroup)
        return;
      this.dragPoint = e.Location;
    }

    private void gvGroups_MouseMove(object sender, MouseEventArgs e)
    {
      if (!(this.dragPoint != Point.Empty) || this.dragPoint.Equals((object) e.Location))
        return;
      int num = (int) this.gvGroups.DoDragDrop((object) this.gvGroups.SelectedItems[0], DragDropEffects.Move);
      this.dragPoint = Point.Empty;
    }

    private void gvGroups_MouseUp(object sender, MouseEventArgs e) => this.dragPoint = Point.Empty;

    private void gvGroups_DragEnter(object sender, DragEventArgs e)
    {
      if (e.Data.GetDataPresent(typeof (GVSelectedItemCollection)))
        e.Effect = DragDropEffects.Move;
      else
        e.Effect = DragDropEffects.None;
    }

    private void gvGroups_DragOver(object sender, DragEventArgs e)
    {
      e.Effect = DragDropEffects.None;
      Point client = this.gvGroups.PointToClient(new Point(e.X, e.Y));
      GVItem itemAt = this.gvGroups.GetItemAt(client.X, client.Y);
      if (itemAt == null || !e.Data.GetDataPresent(typeof (GVSelectedItemCollection)) || !(itemAt.Tag is DocumentGroup) || itemAt.Tag == this.allGroup || itemAt.Tag == this.closingGroup || itemAt.Tag == this.disclosuresGroup || itemAt.Tag == this.epassGroup)
        return;
      e.Effect = DragDropEffects.Move;
    }

    private void gvGroups_DragDrop(object sender, DragEventArgs e)
    {
      Point client = this.gvGroups.PointToClient(new Point(e.X, e.Y));
      GVItem itemAt = this.gvGroups.GetItemAt(client.X, client.Y);
      List<DocumentTemplate> documentTemplateList = new List<DocumentTemplate>();
      foreach (GVItem groupItem in (IEnumerable<GVItem>) itemAt.GroupItems)
        documentTemplateList.Add((DocumentTemplate) groupItem.Tag);
      foreach (GVItem gvItem in (GVSelectedItemCollection) e.Data.GetData(typeof (GVSelectedItemCollection)))
      {
        DocumentTemplate tag = (DocumentTemplate) gvItem.Tag;
        if (!documentTemplateList.Contains(tag))
          itemAt.GroupItems.Add(gvItem.Clone());
      }
      itemAt.State = GVItemState.Normal;
      this.setDirtyFlag(true);
    }

    public override void Reset()
    {
      if (!this.gvGroups.StopEditing())
        return;
      this.loadGroupList();
    }

    public override void Save()
    {
      if (!this.gvGroups.StopEditing())
        return;
      this.groupSetup.Clear();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvGroups.Items)
      {
        if (gvItem.Tag != this.allGroup && gvItem.Tag != this.closingGroup && gvItem.Tag != this.disclosuresGroup && gvItem.Tag != this.epassGroup)
        {
          DocumentGroup documentGroup = this.groupSetup.Add(gvItem.Text);
          List<DocumentTemplate> documentTemplateList = new List<DocumentTemplate>();
          foreach (GVItem groupItem in (IEnumerable<GVItem>) gvItem.GroupItems)
            documentTemplateList.Add((DocumentTemplate) groupItem.Tag);
          documentGroup.SetDocuments(documentTemplateList.ToArray());
        }
      }
      this.session.ConfigurationManager.SaveDocumentGroupSetup(this.groupSetup);
      base.Save();
    }

    private void DocumentGroupSetupControl_Resize(object sender, EventArgs e)
    {
      this.gcDocuments.Width = this.ClientSize.Width / 2;
    }

    public string[] SelectedTemplates
    {
      get
      {
        return this.gvGroups.SelectedItems.Count == 0 ? (string[]) null : this.gvGroups.SelectedItems.Select<GVItem, string>((Func<GVItem, string>) (item => item.Text)).ToArray<string>();
      }
    }

    public void HightlightSelectedTemplate(List<string> selectedGroupNames)
    {
      for (int index = 0; index < selectedGroupNames.Count; ++index)
      {
        for (int nItemIndex = 0; nItemIndex < this.gvGroups.Items.Count; ++nItemIndex)
        {
          if (this.gvGroups.Items[nItemIndex].Text == selectedGroupNames[index])
          {
            this.gvGroups.Items[nItemIndex].Selected = true;
            break;
          }
        }
      }
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (DocumentGroupSetupControl));
      this.btnNewGroup = new StandardIconButton();
      this.gcDocuments = new GroupContainer();
      this.gvDocuments = new GridView();
      this.toolTip = new ToolTip(this.components);
      this.btnDeleteGroup = new StandardIconButton();
      this.imageList = new ImageList(this.components);
      this.gcGroups = new GroupContainer();
      this.gvGroups = new GridView();
      this.splitter1 = new Splitter();
      ((ISupportInitialize) this.btnNewGroup).BeginInit();
      this.gcDocuments.SuspendLayout();
      ((ISupportInitialize) this.btnDeleteGroup).BeginInit();
      this.gcGroups.SuspendLayout();
      this.SuspendLayout();
      this.btnNewGroup.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnNewGroup.BackColor = Color.Transparent;
      this.btnNewGroup.Location = new Point(250, 5);
      this.btnNewGroup.Name = "btnNewGroup";
      this.btnNewGroup.Size = new Size(16, 16);
      this.btnNewGroup.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnNewGroup.TabIndex = 13;
      this.btnNewGroup.TabStop = false;
      this.toolTip.SetToolTip((Control) this.btnNewGroup, "New");
      this.btnNewGroup.Click += new EventHandler(this.btnNewGroup_Click);
      this.gcDocuments.Controls.Add((Control) this.gvDocuments);
      this.gcDocuments.Dock = DockStyle.Left;
      this.gcDocuments.Location = new Point(0, 0);
      this.gcDocuments.Name = "gcDocuments";
      this.gcDocuments.Size = new Size(280, 244);
      this.gcDocuments.TabIndex = 0;
      this.gcDocuments.Text = "Documents";
      this.gvDocuments.AllowDrop = true;
      this.gvDocuments.BorderStyle = BorderStyle.None;
      this.gvDocuments.ClearSelectionsOnEmptyRowClick = false;
      this.gvDocuments.Dock = DockStyle.Fill;
      this.gvDocuments.HeaderHeight = 0;
      this.gvDocuments.HeaderVisible = false;
      this.gvDocuments.HoverToolTip = this.toolTip;
      this.gvDocuments.ImageList = this.imageList;
      this.gvDocuments.Location = new Point(1, 26);
      this.gvDocuments.Name = "gvDocuments";
      this.gvDocuments.Size = new Size(278, 217);
      this.gvDocuments.TabIndex = 1;
      this.gvDocuments.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gvDocuments.MouseUp += new MouseEventHandler(this.gvDocuments_MouseUp);
      this.gvDocuments.DragEnter += new DragEventHandler(this.gvDocuments_DragEnter);
      this.gvDocuments.DragDrop += new DragEventHandler(this.gvDocuments_DragDrop);
      this.gvDocuments.MouseDown += new MouseEventHandler(this.gvDocuments_MouseDown);
      this.gvDocuments.MouseMove += new MouseEventHandler(this.gvDocuments_MouseMove);
      this.btnDeleteGroup.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDeleteGroup.BackColor = Color.Transparent;
      this.btnDeleteGroup.Enabled = false;
      this.btnDeleteGroup.Location = new Point(272, 5);
      this.btnDeleteGroup.Name = "btnDeleteGroup";
      this.btnDeleteGroup.Size = new Size(16, 16);
      this.btnDeleteGroup.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnDeleteGroup.TabIndex = 12;
      this.btnDeleteGroup.TabStop = false;
      this.toolTip.SetToolTip((Control) this.btnDeleteGroup, "Delete");
      this.btnDeleteGroup.Click += new EventHandler(this.btnDeleteGroup_Click);
      this.imageList.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imageList.ImageStream");
      this.imageList.TransparentColor = Color.Transparent;
      this.imageList.Images.SetKeyName(0, "document-group-private.png");
      this.imageList.Images.SetKeyName(1, "document.png");
      this.gcGroups.Controls.Add((Control) this.gvGroups);
      this.gcGroups.Controls.Add((Control) this.btnNewGroup);
      this.gcGroups.Controls.Add((Control) this.btnDeleteGroup);
      this.gcGroups.Dock = DockStyle.Fill;
      this.gcGroups.Location = new Point(283, 0);
      this.gcGroups.Name = "gcGroups";
      this.gcGroups.Size = new Size(297, 244);
      this.gcGroups.TabIndex = 2;
      this.gcGroups.Text = "Document Groups";
      this.gvGroups.AllowDrop = true;
      this.gvGroups.AllowMultiselect = false;
      this.gvGroups.BorderStyle = BorderStyle.None;
      this.gvGroups.ClearSelectionsOnEmptyRowClick = false;
      this.gvGroups.Dock = DockStyle.Fill;
      this.gvGroups.HeaderHeight = 0;
      this.gvGroups.HeaderVisible = false;
      this.gvGroups.HoverToolTip = this.toolTip;
      this.gvGroups.ImageList = this.imageList;
      this.gvGroups.ItemGrouping = true;
      this.gvGroups.Location = new Point(1, 26);
      this.gvGroups.Name = "gvGroups";
      this.gvGroups.Size = new Size(295, 217);
      this.gvGroups.TabIndex = 3;
      this.gvGroups.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gvGroups.MouseUp += new MouseEventHandler(this.gvGroups_MouseUp);
      this.gvGroups.DragEnter += new DragEventHandler(this.gvGroups_DragEnter);
      this.gvGroups.EditorOpening += new GVSubItemEditingEventHandler(this.gvGroups_EditorOpening);
      this.gvGroups.SelectedIndexChanged += new EventHandler(this.gvGroups_SelectedIndexChanged);
      this.gvGroups.DragDrop += new DragEventHandler(this.gvGroups_DragDrop);
      this.gvGroups.MouseDown += new MouseEventHandler(this.gvGroups_MouseDown);
      this.gvGroups.EditorClosing += new GVSubItemEditingEventHandler(this.gvGroups_EditorClosing);
      this.gvGroups.MouseMove += new MouseEventHandler(this.gvGroups_MouseMove);
      this.gvGroups.DragOver += new DragEventHandler(this.gvGroups_DragOver);
      this.splitter1.Location = new Point(280, 0);
      this.splitter1.Name = "splitter1";
      this.splitter1.Size = new Size(3, 244);
      this.splitter1.TabIndex = 2;
      this.splitter1.TabStop = false;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.gcGroups);
      this.Controls.Add((Control) this.splitter1);
      this.Controls.Add((Control) this.gcDocuments);
      this.Name = nameof (DocumentGroupSetupControl);
      this.Size = new Size(580, 244);
      this.Resize += new EventHandler(this.DocumentGroupSetupControl_Resize);
      ((ISupportInitialize) this.btnNewGroup).EndInit();
      this.gcDocuments.ResumeLayout(false);
      ((ISupportInitialize) this.btnDeleteGroup).EndInit();
      this.gcGroups.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
