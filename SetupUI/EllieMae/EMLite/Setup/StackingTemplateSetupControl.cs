// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.StackingTemplateSetupControl
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.DocEngine;
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
namespace EllieMae.EMLite.Setup
{
  public class StackingTemplateSetupControl : SettingsUserControl
  {
    private Sessions.Session session;
    private TemplateIFSExplorer ifsExplorer;
    private StackingOrderSetTemplate currentStackingTemplate;
    private FileSystemEntry currentEntry;
    private string defaultTemplateName;
    private bool readOnly;
    private IContainer components;
    private GroupContainer grpStackingOrders;
    private GridView gvStackingOrders;
    private FlowLayoutPanel pnlStackingOrderControls;
    private StandardIconButton btnDeleteStackingOrder;
    private StandardIconButton btnDuplicateStackingOrder;
    private StandardIconButton btnAddStackingOrder;
    private ToolTip toolTip1;
    private VerticalSeparator verticalSeparator1;
    private Button btnSetAsDefault;
    private SaveFileDialog sfdExport;
    private OpenFileDialog ofdImport;
    private GroupContainer grpDocuments;
    private GridView gvDocOrder;
    private FlowLayoutPanel pnlDocumentControls;
    private StandardIconButton btnResetStackingOrder;
    private StandardIconButton btnSaveStackingOrder;
    private VerticalSeparator verticalSeparator2;
    private StandardIconButton btnRemoveDocument;
    private StandardIconButton btnMoveDocumentDown;
    private StandardIconButton btnMoveDocumentUp;
    private StandardIconButton btnAddDocuments;
    private GradientPanel gradientPanel1;
    private Label label1;
    private ListView lvwDefault;
    private PictureBox pctSplit;

    public StackingTemplateSetupControl(SetUpContainer container, Sessions.Session session)
      : this(container, session, false)
    {
    }

    public StackingTemplateSetupControl(
      SetUpContainer container,
      Sessions.Session session,
      bool allowMultiSelect)
      : base(container)
    {
      this.session = session;
      this.InitializeComponent();
      if (this.DesignMode)
        return;
      this.clearStackingOrder();
      this.gvStackingOrders.Sort(0, SortOrder.Ascending);
      this.gvStackingOrders.AllowMultiselect = allowMultiSelect;
      this.ifsExplorer = new TemplateIFSExplorer(this.session, TemplateSettingsType.StackingOrder);
      this.ifsExplorer.Init(FileSystemEntry.PublicRoot, true);
      this.getCurrentDefaultTemplate();
      this.loadStackingOrders();
    }

    public bool ReadOnly
    {
      get => this.readOnly;
      set
      {
        this.readOnly = value;
        this.setEditControlStates();
      }
    }

    private void setEditControlStates()
    {
      this.pnlDocumentControls.Visible = !this.readOnly;
      this.pnlStackingOrderControls.Visible = !this.readOnly;
      this.gvDocOrder.DropTarget = this.readOnly ? GVDropTarget.None : GVDropTarget.BetweenItems;
    }

    private void loadStackingOrders()
    {
      FileSystemEntry[] fileSystemEntries = this.ifsExplorer.CurrentFileSystemEntries;
      this.gvStackingOrders.Items.Clear();
      foreach (FileSystemEntry entry in fileSystemEntries)
        this.gvStackingOrders.Items.Add(this.createGVItemForStackingOrder(entry));
      this.gvStackingOrders.ReSort();
      this.refreshStackingOrderCount();
    }

    private void refreshStackingOrderCount()
    {
      this.grpStackingOrders.Text = "Stacking Templates (" + (object) this.gvStackingOrders.Items.Count + ")";
    }

    private void refreshStackingDocumentCount()
    {
      this.grpDocuments.Text = "Documents (" + (object) this.gvDocOrder.Items.Count + ")";
    }

    private GVItem createGVItemForStackingOrder(FileSystemEntry entry)
    {
      GVItem forStackingOrder = new GVItem(this.ifsExplorer.GetDisplayName(entry, true));
      if (entry.Name == this.defaultTemplateName)
        forStackingOrder.SubItems.Add((object) "Yes");
      forStackingOrder.ImageIndex = this.ifsExplorer.GetFileImageIcon();
      forStackingOrder.Tag = (object) entry;
      return forStackingOrder;
    }

    private void gvStackingOrders_SelectedIndexChanged(object sender, EventArgs e)
    {
      StackingOrderSetTemplate orderSetTemplate = (StackingOrderSetTemplate) null;
      FileSystemEntry fileSystemEntry = (FileSystemEntry) null;
      string str = (string) null;
      string templateName = this.currentStackingTemplate == null ? (string) null : this.currentStackingTemplate.TemplateName;
      if (this.gvStackingOrders.SelectedItems.Count > 0)
      {
        fileSystemEntry = (FileSystemEntry) this.gvStackingOrders.SelectedItems[0].Tag;
        orderSetTemplate = (StackingOrderSetTemplate) this.session.ConfigurationManager.GetTemplateSettings(TemplateSettingsType.StackingOrder, fileSystemEntry);
        str = orderSetTemplate.TemplateName;
      }
      this.btnDeleteStackingOrder.Enabled = this.btnDuplicateStackingOrder.Enabled = this.gvStackingOrders.SelectedItems.Count == 1;
      this.btnSetAsDefault.Enabled = orderSetTemplate != null && !orderSetTemplate.IsDefault && this.gvStackingOrders.SelectedItems.Count == 1;
      if (str == templateName)
        return;
      if (!this.PromptToCommit())
      {
        this.selectStackingOrder(templateName);
      }
      else
      {
        if (this.gvStackingOrders.SelectedItems.Count > 0 && !this.setCurrentStackingOrder(fileSystemEntry))
          this.selectStackingOrder(templateName);
        if (this.gvStackingOrders.SelectedItems.Count != 0)
          return;
        this.clearStackingOrder();
      }
    }

    public override void Save()
    {
      this.commitChanges();
      base.Save();
    }

    public bool PromptToCommit()
    {
      if (this.currentStackingTemplate == null || !this.btnSaveStackingOrder.Enabled)
        return true;
      switch (Utils.Dialog((IWin32Window) this, "Save the changes to the current stacking template?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
      {
        case DialogResult.Cancel:
          return false;
        case DialogResult.No:
          this.setCurrentStackingOrder(this.currentEntry, this.currentStackingTemplate);
          return true;
        default:
          this.commitChanges();
          return true;
      }
    }

    private void selectStackingOrder(string templateName)
    {
      if (templateName == null)
      {
        this.clearStackingOrder();
      }
      else
      {
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvStackingOrders.Items)
        {
          if (((StackingOrderSetTemplate) this.session.ConfigurationManager.GetTemplateSettings(TemplateSettingsType.StackingOrder, (FileSystemEntry) gvItem.Tag)).TemplateName == templateName)
          {
            gvItem.Selected = true;
            break;
          }
        }
      }
    }

    private void clearStackingOrder()
    {
      this.currentStackingTemplate = (StackingOrderSetTemplate) null;
      this.currentEntry = (FileSystemEntry) null;
      this.gvDocOrder.Items.Clear();
      this.grpDocuments.Enabled = false;
    }

    private bool setCurrentStackingOrder(FileSystemEntry entry)
    {
      StackingOrderSetTemplate templateSettings = (StackingOrderSetTemplate) this.session.ConfigurationManager.GetTemplateSettings(TemplateSettingsType.StackingOrder, entry);
      if (templateSettings == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The selected stacking template has been deleted or is no longer accessible.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      this.setCurrentStackingOrder(entry, templateSettings);
      return true;
    }

    private void setCurrentStackingOrder(
      FileSystemEntry entry,
      StackingOrderSetTemplate stackingTemplate)
    {
      this.grpDocuments.Enabled = true;
      this.currentEntry = entry;
      this.currentStackingTemplate = stackingTemplate;
      this.reloadStackingDocumentList();
      this.btnSaveStackingOrder.Enabled = false;
      this.btnResetStackingOrder.Enabled = false;
      this.setDirtyFlag(false);
    }

    private void reloadStackingDocumentList()
    {
      this.gvDocOrder.Items.Clear();
      DocumentTrackingSetup documentTrackingSetup = this.session.ConfigurationManager.GetDocumentTrackingSetup();
      ArrayList requiredDocs = this.currentStackingTemplate.RequiredDocs;
      foreach (string docName in this.currentStackingTemplate.DocNames)
      {
        DocumentTemplate doc = documentTrackingSetup.GetByName(docName) ?? new DocumentTemplate(docName);
        this.gvDocOrder.Items.Add(!this.currentStackingTemplate.NDEDocGroups.Contains((object) docName) ? this.createGVItemForDocument(doc, requiredDocs.Contains((object) docName)) : this.createGVItemForDocument(new StackingElement(StackingElementType.DocumentType, docName), requiredDocs.Contains((object) docName)));
      }
      this.refreshStackingDocumentCount();
    }

    private GVItem createGVItemForDocument(DocumentTemplate doc, bool isRequired)
    {
      GVItem gvItemForDocument = new GVItem();
      gvItemForDocument.SubItems[0].Value = (object) doc.Name;
      gvItemForDocument.SubItems[1].Value = (object) "No";
      if (isRequired)
        gvItemForDocument.SubItems[1].Value = (object) "Yes";
      gvItemForDocument.Tag = (object) new StackingElement(StackingElementType.Document, doc.Name);
      return gvItemForDocument;
    }

    private GVItem createGVItemForDocument(StackingElement element, bool isRequired)
    {
      GVItem gvItemForDocument = new GVItem();
      gvItemForDocument.SubItems[0].Value = (object) new StackingDisplayElement(element);
      gvItemForDocument.SubItems[1].Value = (object) "No";
      if (isRequired)
        gvItemForDocument.SubItems[1].Value = (object) "Yes";
      gvItemForDocument.Tag = (object) element;
      return gvItemForDocument;
    }

    private void gvDocOrder_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnMoveDocumentUp.Enabled = this.gvDocOrder.SelectedItems.Count > 0;
      this.btnMoveDocumentDown.Enabled = this.gvDocOrder.SelectedItems.Count > 0;
      this.btnRemoveDocument.Enabled = this.gvDocOrder.SelectedItems.Count > 0;
    }

    private void btnRemoveDocument_Click(object sender, EventArgs e)
    {
      if (MessageBox.Show((IWin32Window) this, "Are you sure you want to remove the " + (object) this.gvDocOrder.SelectedItems.Count + " selected document(s) from the stacking template?", "Encompass Docs", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        return;
      for (int nItemIndex = this.gvDocOrder.Items.Count - 1; nItemIndex >= 0; --nItemIndex)
      {
        if (this.gvDocOrder.Items[nItemIndex].Selected)
          this.gvDocOrder.Items.RemoveAt(nItemIndex);
      }
      this.refreshStackingDocumentCount();
      this.onStackingOrderPropertyChanged(sender, e);
    }

    private void gvDocOrder_ItemDrag(object source, GVItemEventArgs e)
    {
      int num = (int) this.gvDocOrder.DoDragDrop((object) this.gvDocOrder, DragDropEffects.Move);
    }

    private void gvDocOrder_DragOver(object sender, DragEventArgs e)
    {
      if (e.Data.GetData(typeof (GridView)) == this.gvDocOrder)
        e.Effect = DragDropEffects.Move;
      else
        e.Effect = DragDropEffects.None;
    }

    private void gvDocOrder_DragDrop(object sender, DragEventArgs e)
    {
      GVDragEventArgs gvDragEventArgs = (GVDragEventArgs) e;
      int num = 0;
      for (int nItemIndex = 0; nItemIndex < gvDragEventArgs.TargetLocation.InsertIndex; ++nItemIndex)
      {
        if (!this.gvDocOrder.Items[nItemIndex].Selected)
          ++num;
      }
      List<GVItem> gvItemList = new List<GVItem>();
      for (int nItemIndex = this.gvDocOrder.Items.Count - 1; nItemIndex >= 0; --nItemIndex)
      {
        if (this.gvDocOrder.Items[nItemIndex].Selected)
        {
          gvItemList.Insert(0, this.gvDocOrder.Items[nItemIndex]);
          this.gvDocOrder.Items.RemoveAt(nItemIndex);
        }
      }
      for (int index = 0; index < gvItemList.Count; ++index)
      {
        this.gvDocOrder.Items.Insert(num + index, gvItemList[index]);
        gvItemList[index].Selected = true;
      }
      this.onStackingOrderPropertyChanged(sender, (EventArgs) e);
    }

    private void btnMoveDocumentUp_Click(object sender, EventArgs e)
    {
      for (int nItemIndex = 1; nItemIndex < this.gvDocOrder.Items.Count; ++nItemIndex)
      {
        if (this.gvDocOrder.Items[nItemIndex].Selected && !this.gvDocOrder.Items[nItemIndex - 1].Selected)
        {
          GVItem gvItem = this.gvDocOrder.Items[nItemIndex];
          this.gvDocOrder.Items.RemoveAt(nItemIndex);
          this.gvDocOrder.Items.Insert(nItemIndex - 1, gvItem);
          gvItem.Selected = true;
        }
      }
      this.onStackingOrderPropertyChanged(sender, e);
    }

    private void btnMoveDocumentDown_Click(object sender, EventArgs e)
    {
      for (int nItemIndex = this.gvDocOrder.Items.Count - 2; nItemIndex >= 0; --nItemIndex)
      {
        if (this.gvDocOrder.Items[nItemIndex].Selected && !this.gvDocOrder.Items[nItemIndex + 1].Selected)
        {
          GVItem gvItem = this.gvDocOrder.Items[nItemIndex];
          this.gvDocOrder.Items.RemoveAt(nItemIndex);
          this.gvDocOrder.Items.Insert(nItemIndex + 1, gvItem);
          gvItem.Selected = true;
        }
      }
      this.onStackingOrderPropertyChanged(sender, e);
    }

    private void onStackingOrderPropertyChanged(object sender, EventArgs e)
    {
      this.btnSaveStackingOrder.Enabled = true;
      this.btnResetStackingOrder.Enabled = true;
      this.setDirtyFlag(true);
    }

    private void btnSaveStackingOrder_Click(object sender, EventArgs e)
    {
      if (!this.validateStackingOrder())
        return;
      this.commitChanges();
    }

    private bool validateStackingOrder() => true;

    private bool stackingOrderExists(FileSystemEntry entry)
    {
      return this.ifsExplorer.EntryExistsOfAnyType(entry);
    }

    private void commitChanges()
    {
      this.commitDocumentListToStackingOrder();
      this.session.ConfigurationManager.SaveTemplateSettings(TemplateSettingsType.StackingOrder, this.currentEntry, (BinaryObject) (BinaryConvertibleObject) this.currentStackingTemplate);
      this.btnSaveStackingOrder.Enabled = false;
      this.btnResetStackingOrder.Enabled = false;
      this.setDirtyFlag(false);
    }

    private void commitDocumentListToStackingOrder()
    {
      this.currentStackingTemplate.DocNames.Clear();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvDocOrder.Items)
        this.currentStackingTemplate.DocNames.Add((object) ((StackingElement) gvItem.Tag).Name);
    }

    private void btnResetStackingOrder_Click(object sender, EventArgs e)
    {
      if (Utils.Dialog((IWin32Window) this, "Discard your changes to the current stacking template?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        return;
      this.setCurrentStackingOrder((FileSystemEntry) this.gvStackingOrders.SelectedItems[0].Tag);
    }

    private void btnAddStackingOrder_Click(object sender, EventArgs e)
    {
      if (!this.PromptToCommit())
        return;
      this.gvStackingOrders.SelectedItems.Clear();
      FileSystemEntry entry = this.ifsExplorer.AddEntry(true);
      if (entry != null)
      {
        GVItem forStackingOrder = this.createGVItemForStackingOrder(entry);
        this.gvStackingOrders.Items.Add(forStackingOrder);
        forStackingOrder.Selected = true;
      }
      this.refreshStackingOrderCount();
    }

    private void btnDuplicateStackingOrder_Click(object sender, EventArgs e)
    {
      if (!this.PromptToCommit() || this.currentEntry == null)
        return;
      if (!this.stackingOrderExists(this.currentEntry))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Cannot find the source file " + this.currentEntry.Name + ".", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        if (!this.ifsExplorer.DuplicateEntry(this.currentEntry))
          return;
        this.loadStackingOrders();
      }
    }

    private void btnDeleteStackingOrder_Click(object sender, EventArgs e)
    {
      if (Utils.Dialog((IWin32Window) this, "Are you sure you want to permanently delete the '" + this.currentStackingTemplate.TemplateName + "' stacking template?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
        return;
      if (this.currentEntry.Name == this.defaultTemplateName)
        this.lvwDefault.Items.Clear();
      this.ifsExplorer.DeleteEntry(this.currentEntry);
      this.currentStackingTemplate = (StackingOrderSetTemplate) null;
      this.gvStackingOrders.Items.Remove(this.gvStackingOrders.SelectedItems[0]);
      this.refreshStackingOrderCount();
    }

    private void btnAddDocuments_Click(object sender, EventArgs e)
    {
      this.commitDocumentListToStackingOrder();
      using (StackingOrderSetTemplateDialog setTemplateDialog = new StackingOrderSetTemplateDialog(this.currentStackingTemplate, this.session))
      {
        if (setTemplateDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.currentStackingTemplate = setTemplateDialog.StackingOrderTemplate;
        this.reloadStackingDocumentList();
        this.onStackingOrderPropertyChanged(sender, e);
      }
    }

    private void btnSetAsDefault_Click(object sender, EventArgs e)
    {
      if (this.gvStackingOrders.SelectedItems.Count != 1)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "To setup a default stacking order template, you have to select a single template first.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        this.session.ConfigurationManager.SetCompanySetting("StackingOrder", "Default", ((FileSystemEntry) this.gvStackingOrders.SelectedItems[0].Tag).ToString());
        this.getCurrentDefaultTemplate();
        this.loadStackingOrders();
      }
    }

    private void getCurrentDefaultTemplate()
    {
      this.lvwDefault.Items.Clear();
      string companySetting = this.session.ConfigurationManager.GetCompanySetting("StackingOrder", "Default");
      try
      {
        FileSystemEntry fileSystemEntry = FileSystemEntry.Parse(companySetting);
        this.defaultTemplateName = fileSystemEntry.Name;
        this.lvwDefault.Items.Add(FileSystemEntry.RemoveRoot(fileSystemEntry.ToDisplayString()));
        this.lvwDefault.Items[0].ImageIndex = 4;
      }
      catch
      {
      }
    }

    private void gvStackingOrders_EditorOpening(object sender, GVSubItemEditingEventArgs e)
    {
      if (this.gvStackingOrders.SelectedItems.Count <= 0)
        return;
      FileSystemEntry tag = (FileSystemEntry) this.gvStackingOrders.SelectedItems[0].Tag;
      if (tag == null || tag.Access != AclResourceAccess.ReadOnly)
        return;
      int num = (int) Utils.Dialog((IWin32Window) this, "You do not have the necessary access rights to rename this file.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      e.Cancel = true;
    }

    private void gvStackingOrders_EditorClosing(object sender, GVSubItemEditingEventArgs e)
    {
      GVItem parent = e.SubItem.Parent;
      FileSystemEntry tag = (FileSystemEntry) parent.Tag;
      if (e.EditorControl.Text == e.SubItem.Text)
        return;
      if (e.EditorControl.Text.IndexOf("\\") > -1)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "A file or folder name cannot contain the \"\\\" character.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        e.Cancel = true;
        e.EditorControl.Text = tag.Name;
      }
      else
      {
        int length = e.EditorControl.Text.IndexOf(".");
        if (length > -1 && e.EditorControl.Text.Substring(0, length).Trim() == "")
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "You must type a file name.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          e.Cancel = true;
          e.EditorControl.Text = tag.Name;
        }
        else
        {
          int num1 = 260;
          if (FileSystemEntry.Types.File == ((FileSystemEntry) parent.Tag).Type)
          {
            string str = e.EditorControl.Text.Trim();
            if (num1 < str.Length)
            {
              int num2 = (int) Utils.Dialog((IWin32Window) this, "File name is limited to " + (object) num1 + " characters.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              e.Cancel = true;
              e.EditorControl.Text = tag.Name;
              return;
            }
          }
          string name = tag.Name;
          string newName = e.EditorControl.Text.Trim();
          if (newName == "")
          {
            int num3 = (int) Utils.Dialog((IWin32Window) this, "A file or folder name cannot be blank.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            e.Cancel = true;
            e.EditorControl.Text = tag.Name;
          }
          else
          {
            string lower = e.EditorControl.Text.Trim().ToLower();
            FileSystemEntry[] fileSystemEntries = this.ifsExplorer.CurrentFileSystemEntries;
            for (int index = 0; index < fileSystemEntries.Length; ++index)
            {
              if (fileSystemEntries[index] != tag && this.ifsExplorer.GetDisplayName(fileSystemEntries[index], true).ToLower() == lower)
              {
                int num4 = (int) Utils.Dialog((IWin32Window) this, "The selected item cannot be renamed because an item in this folder already exists with the specified name.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                e.EditorControl.Text = tag.Name;
                e.Cancel = true;
                return;
              }
            }
            FileSystemEntry fileSystemEntry = this.ifsExplorer.Rename(tag, name, newName);
            if (fileSystemEntry == null)
            {
              e.Cancel = true;
            }
            else
            {
              e.SubItem.Item.Tag = (object) fileSystemEntry;
              this.currentEntry = fileSystemEntry;
              if (this.currentStackingTemplate == null)
                return;
              this.currentStackingTemplate.TemplateName = newName;
            }
          }
        }
      }
    }

    private void StackingTemplateSetupControl_ClientSizeChanged(object sender, EventArgs e)
    {
      int num = (this.ClientSize.Width - this.pctSplit.Width) / 2;
      if (num <= 0)
        return;
      this.grpStackingOrders.Width = num;
    }

    public string[] SelectedTemplates
    {
      get
      {
        return this.gvStackingOrders.SelectedItems.Count == 0 ? (string[]) null : this.gvStackingOrders.SelectedItems.Select<GVItem, string>((Func<GVItem, string>) (item => item.Text)).ToArray<string>();
      }
    }

    public void HightlightSelectedTemplate(List<string> selectedTemplateNames)
    {
      for (int index = 0; index < selectedTemplateNames.Count; ++index)
      {
        for (int nItemIndex = 0; nItemIndex < this.gvStackingOrders.Items.Count; ++nItemIndex)
        {
          if (this.gvStackingOrders.Items[nItemIndex].Text == selectedTemplateNames[index])
          {
            this.gvStackingOrders.Items[nItemIndex].Selected = true;
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
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      GVColumn gvColumn4 = new GVColumn();
      this.toolTip1 = new ToolTip(this.components);
      this.btnResetStackingOrder = new StandardIconButton();
      this.btnSaveStackingOrder = new StandardIconButton();
      this.btnRemoveDocument = new StandardIconButton();
      this.btnMoveDocumentDown = new StandardIconButton();
      this.btnMoveDocumentUp = new StandardIconButton();
      this.btnAddDocuments = new StandardIconButton();
      this.btnDeleteStackingOrder = new StandardIconButton();
      this.btnDuplicateStackingOrder = new StandardIconButton();
      this.btnAddStackingOrder = new StandardIconButton();
      this.sfdExport = new SaveFileDialog();
      this.ofdImport = new OpenFileDialog();
      this.gradientPanel1 = new GradientPanel();
      this.label1 = new Label();
      this.lvwDefault = new ListView();
      this.grpDocuments = new GroupContainer();
      this.gvDocOrder = new GridView();
      this.pnlDocumentControls = new FlowLayoutPanel();
      this.verticalSeparator2 = new VerticalSeparator();
      this.grpStackingOrders = new GroupContainer();
      this.gvStackingOrders = new GridView();
      this.pnlStackingOrderControls = new FlowLayoutPanel();
      this.btnSetAsDefault = new Button();
      this.verticalSeparator1 = new VerticalSeparator();
      this.pctSplit = new PictureBox();
      ((ISupportInitialize) this.btnResetStackingOrder).BeginInit();
      ((ISupportInitialize) this.btnSaveStackingOrder).BeginInit();
      ((ISupportInitialize) this.btnRemoveDocument).BeginInit();
      ((ISupportInitialize) this.btnMoveDocumentDown).BeginInit();
      ((ISupportInitialize) this.btnMoveDocumentUp).BeginInit();
      ((ISupportInitialize) this.btnAddDocuments).BeginInit();
      ((ISupportInitialize) this.btnDeleteStackingOrder).BeginInit();
      ((ISupportInitialize) this.btnDuplicateStackingOrder).BeginInit();
      ((ISupportInitialize) this.btnAddStackingOrder).BeginInit();
      this.gradientPanel1.SuspendLayout();
      this.grpDocuments.SuspendLayout();
      this.pnlDocumentControls.SuspendLayout();
      this.grpStackingOrders.SuspendLayout();
      this.pnlStackingOrderControls.SuspendLayout();
      ((ISupportInitialize) this.pctSplit).BeginInit();
      this.SuspendLayout();
      this.btnResetStackingOrder.BackColor = Color.Transparent;
      this.btnResetStackingOrder.Location = new Point(114, 3);
      this.btnResetStackingOrder.Margin = new Padding(5, 3, 0, 3);
      this.btnResetStackingOrder.MouseDownImage = (Image) null;
      this.btnResetStackingOrder.Name = "btnResetStackingOrder";
      this.btnResetStackingOrder.Size = new Size(16, 16);
      this.btnResetStackingOrder.StandardButtonType = StandardIconButton.ButtonType.ResetButton;
      this.btnResetStackingOrder.TabIndex = 0;
      this.btnResetStackingOrder.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnResetStackingOrder, "Reset Stacking Order");
      this.btnResetStackingOrder.Click += new EventHandler(this.btnResetStackingOrder_Click);
      this.btnSaveStackingOrder.BackColor = Color.Transparent;
      this.btnSaveStackingOrder.Location = new Point(93, 3);
      this.btnSaveStackingOrder.Margin = new Padding(2, 3, 0, 3);
      this.btnSaveStackingOrder.MouseDownImage = (Image) null;
      this.btnSaveStackingOrder.Name = "btnSaveStackingOrder";
      this.btnSaveStackingOrder.Size = new Size(16, 16);
      this.btnSaveStackingOrder.StandardButtonType = StandardIconButton.ButtonType.SaveButton;
      this.btnSaveStackingOrder.TabIndex = 1;
      this.btnSaveStackingOrder.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnSaveStackingOrder, "Save Stacking Order");
      this.btnSaveStackingOrder.Click += new EventHandler(this.btnSaveStackingOrder_Click);
      this.btnRemoveDocument.BackColor = Color.Transparent;
      this.btnRemoveDocument.Enabled = false;
      this.btnRemoveDocument.Location = new Point(68, 3);
      this.btnRemoveDocument.Margin = new Padding(5, 3, 0, 3);
      this.btnRemoveDocument.MouseDownImage = (Image) null;
      this.btnRemoveDocument.Name = "btnRemoveDocument";
      this.btnRemoveDocument.Size = new Size(16, 16);
      this.btnRemoveDocument.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnRemoveDocument.TabIndex = 0;
      this.btnRemoveDocument.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnRemoveDocument, "Remove Document(s) from Stacking Order");
      this.btnRemoveDocument.Click += new EventHandler(this.btnRemoveDocument_Click);
      this.btnMoveDocumentDown.BackColor = Color.Transparent;
      this.btnMoveDocumentDown.Enabled = false;
      this.btnMoveDocumentDown.Location = new Point(47, 3);
      this.btnMoveDocumentDown.Margin = new Padding(5, 3, 0, 3);
      this.btnMoveDocumentDown.MouseDownImage = (Image) null;
      this.btnMoveDocumentDown.Name = "btnMoveDocumentDown";
      this.btnMoveDocumentDown.Size = new Size(16, 16);
      this.btnMoveDocumentDown.StandardButtonType = StandardIconButton.ButtonType.DownArrowButton;
      this.btnMoveDocumentDown.TabIndex = 1;
      this.btnMoveDocumentDown.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnMoveDocumentDown, "Move Document(s) down in Stacking Order");
      this.btnMoveDocumentDown.Click += new EventHandler(this.btnMoveDocumentDown_Click);
      this.btnMoveDocumentUp.BackColor = Color.Transparent;
      this.btnMoveDocumentUp.Enabled = false;
      this.btnMoveDocumentUp.Location = new Point(26, 3);
      this.btnMoveDocumentUp.Margin = new Padding(5, 3, 0, 3);
      this.btnMoveDocumentUp.MouseDownImage = (Image) null;
      this.btnMoveDocumentUp.Name = "btnMoveDocumentUp";
      this.btnMoveDocumentUp.Size = new Size(16, 16);
      this.btnMoveDocumentUp.StandardButtonType = StandardIconButton.ButtonType.UpArrowButton;
      this.btnMoveDocumentUp.TabIndex = 2;
      this.btnMoveDocumentUp.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnMoveDocumentUp, "Move Document(s) up in Stacking Order");
      this.btnMoveDocumentUp.Click += new EventHandler(this.btnMoveDocumentUp_Click);
      this.btnAddDocuments.BackColor = Color.Transparent;
      this.btnAddDocuments.Location = new Point(5, 3);
      this.btnAddDocuments.Margin = new Padding(5, 3, 0, 3);
      this.btnAddDocuments.MouseDownImage = (Image) null;
      this.btnAddDocuments.Name = "btnAddDocuments";
      this.btnAddDocuments.Size = new Size(16, 16);
      this.btnAddDocuments.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAddDocuments.TabIndex = 3;
      this.btnAddDocuments.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnAddDocuments, "Add Document(s) to Stacking Order");
      this.btnAddDocuments.Click += new EventHandler(this.btnAddDocuments_Click);
      this.btnDeleteStackingOrder.BackColor = Color.Transparent;
      this.btnDeleteStackingOrder.Enabled = false;
      this.btnDeleteStackingOrder.Location = new Point(47, 3);
      this.btnDeleteStackingOrder.Margin = new Padding(5, 3, 0, 3);
      this.btnDeleteStackingOrder.MouseDownImage = (Image) null;
      this.btnDeleteStackingOrder.Name = "btnDeleteStackingOrder";
      this.btnDeleteStackingOrder.Size = new Size(16, 16);
      this.btnDeleteStackingOrder.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnDeleteStackingOrder.TabIndex = 0;
      this.btnDeleteStackingOrder.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnDeleteStackingOrder, "Delete Stacking Template");
      this.btnDeleteStackingOrder.Click += new EventHandler(this.btnDeleteStackingOrder_Click);
      this.btnDuplicateStackingOrder.BackColor = Color.Transparent;
      this.btnDuplicateStackingOrder.Enabled = false;
      this.btnDuplicateStackingOrder.Location = new Point(26, 3);
      this.btnDuplicateStackingOrder.Margin = new Padding(5, 3, 0, 3);
      this.btnDuplicateStackingOrder.MouseDownImage = (Image) null;
      this.btnDuplicateStackingOrder.Name = "btnDuplicateStackingOrder";
      this.btnDuplicateStackingOrder.Size = new Size(16, 16);
      this.btnDuplicateStackingOrder.StandardButtonType = StandardIconButton.ButtonType.DuplicateButton;
      this.btnDuplicateStackingOrder.TabIndex = 1;
      this.btnDuplicateStackingOrder.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnDuplicateStackingOrder, "Duplicate Stacking Template");
      this.btnDuplicateStackingOrder.Click += new EventHandler(this.btnDuplicateStackingOrder_Click);
      this.btnAddStackingOrder.BackColor = Color.Transparent;
      this.btnAddStackingOrder.Location = new Point(5, 3);
      this.btnAddStackingOrder.Margin = new Padding(5, 3, 0, 3);
      this.btnAddStackingOrder.MouseDownImage = (Image) null;
      this.btnAddStackingOrder.Name = "btnAddStackingOrder";
      this.btnAddStackingOrder.Size = new Size(16, 16);
      this.btnAddStackingOrder.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAddStackingOrder.TabIndex = 2;
      this.btnAddStackingOrder.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnAddStackingOrder, "New Stacking Template");
      this.btnAddStackingOrder.Click += new EventHandler(this.btnAddStackingOrder_Click);
      this.sfdExport.DefaultExt = "xml";
      this.sfdExport.Filter = "XML Files|*.xml|All Files|*.*";
      this.ofdImport.DefaultExt = "xml";
      this.ofdImport.Filter = "XML Files|*.xml|All Files|*.*";
      this.gradientPanel1.Borders = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.gradientPanel1.Controls.Add((Control) this.label1);
      this.gradientPanel1.Controls.Add((Control) this.lvwDefault);
      this.gradientPanel1.Dock = DockStyle.Top;
      this.gradientPanel1.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradientPanel1.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel1.Location = new Point(0, 0);
      this.gradientPanel1.Name = "gradientPanel1";
      this.gradientPanel1.Size = new Size(735, 33);
      this.gradientPanel1.TabIndex = 34;
      this.label1.AutoSize = true;
      this.label1.BackColor = Color.Transparent;
      this.label1.Location = new Point(5, 10);
      this.label1.Name = "label1";
      this.label1.Size = new Size(125, 14);
      this.label1.TabIndex = 33;
      this.label1.Text = "Current Default Template";
      this.lvwDefault.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.lvwDefault.BackColor = SystemColors.Window;
      this.lvwDefault.Location = new Point(131, 7);
      this.lvwDefault.Name = "lvwDefault";
      this.lvwDefault.Size = new Size(599, 20);
      this.lvwDefault.TabIndex = 32;
      this.lvwDefault.UseCompatibleStateImageBehavior = false;
      this.lvwDefault.View = View.List;
      this.grpDocuments.Controls.Add((Control) this.gvDocOrder);
      this.grpDocuments.Controls.Add((Control) this.pnlDocumentControls);
      this.grpDocuments.Dock = DockStyle.Fill;
      this.grpDocuments.HeaderForeColor = SystemColors.ControlText;
      this.grpDocuments.Location = new Point(343, 33);
      this.grpDocuments.Name = "grpDocuments";
      this.grpDocuments.Size = new Size(392, 476);
      this.grpDocuments.TabIndex = 3;
      this.grpDocuments.Text = "Documents";
      this.gvDocOrder.AllowDrop = true;
      this.gvDocOrder.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.SpringToFit = true;
      gvColumn1.Text = "Document";
      gvColumn1.Width = 290;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Required";
      gvColumn2.Width = 100;
      this.gvDocOrder.Columns.AddRange(new GVColumn[2]
      {
        gvColumn1,
        gvColumn2
      });
      this.gvDocOrder.Dock = DockStyle.Fill;
      this.gvDocOrder.DropTarget = GVDropTarget.BetweenItems;
      this.gvDocOrder.Location = new Point(1, 26);
      this.gvDocOrder.Name = "gvDocOrder";
      this.gvDocOrder.Size = new Size(390, 449);
      this.gvDocOrder.SortIconVisible = false;
      this.gvDocOrder.SortOption = GVSortOption.None;
      this.gvDocOrder.TabIndex = 3;
      this.gvDocOrder.SelectedIndexChanged += new EventHandler(this.gvDocOrder_SelectedIndexChanged);
      this.pnlDocumentControls.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.pnlDocumentControls.AutoSize = true;
      this.pnlDocumentControls.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      this.pnlDocumentControls.BackColor = Color.Transparent;
      this.pnlDocumentControls.Controls.Add((Control) this.btnResetStackingOrder);
      this.pnlDocumentControls.Controls.Add((Control) this.btnSaveStackingOrder);
      this.pnlDocumentControls.Controls.Add((Control) this.verticalSeparator2);
      this.pnlDocumentControls.Controls.Add((Control) this.btnRemoveDocument);
      this.pnlDocumentControls.Controls.Add((Control) this.btnMoveDocumentDown);
      this.pnlDocumentControls.Controls.Add((Control) this.btnMoveDocumentUp);
      this.pnlDocumentControls.Controls.Add((Control) this.btnAddDocuments);
      this.pnlDocumentControls.FlowDirection = FlowDirection.RightToLeft;
      this.pnlDocumentControls.Location = new Point(256, 2);
      this.pnlDocumentControls.Name = "pnlDocumentControls";
      this.pnlDocumentControls.Size = new Size(130, 22);
      this.pnlDocumentControls.TabIndex = 2;
      this.verticalSeparator2.Location = new Point(87, 3);
      this.verticalSeparator2.Margin = new Padding(3, 3, 2, 3);
      this.verticalSeparator2.MaximumSize = new Size(2, 16);
      this.verticalSeparator2.MinimumSize = new Size(2, 16);
      this.verticalSeparator2.Name = "verticalSeparator2";
      this.verticalSeparator2.Size = new Size(2, 16);
      this.verticalSeparator2.TabIndex = 4;
      this.verticalSeparator2.Text = "verticalSeparator2";
      this.grpStackingOrders.Controls.Add((Control) this.gvStackingOrders);
      this.grpStackingOrders.Controls.Add((Control) this.pnlStackingOrderControls);
      this.grpStackingOrders.Dock = DockStyle.Left;
      this.grpStackingOrders.HeaderForeColor = SystemColors.ControlText;
      this.grpStackingOrders.Location = new Point(0, 33);
      this.grpStackingOrders.Name = "grpStackingOrders";
      this.grpStackingOrders.Size = new Size(339, 476);
      this.grpStackingOrders.TabIndex = 0;
      this.grpStackingOrders.Text = "Stacking Orders";
      this.gvStackingOrders.AllowMultiselect = false;
      this.gvStackingOrders.BorderStyle = BorderStyle.None;
      this.gvStackingOrders.ClearSelectionsOnEmptyRowClick = false;
      gvColumn3.ActivatedEditorType = GVActivatedEditorType.TextBox;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column1";
      gvColumn3.SpringToFit = true;
      gvColumn3.Text = "Name";
      gvColumn3.Width = 277;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column2";
      gvColumn4.Text = "Default";
      gvColumn4.Width = 60;
      this.gvStackingOrders.Columns.AddRange(new GVColumn[2]
      {
        gvColumn3,
        gvColumn4
      });
      this.gvStackingOrders.Dock = DockStyle.Fill;
      this.gvStackingOrders.Location = new Point(1, 26);
      this.gvStackingOrders.Name = "gvStackingOrders";
      this.gvStackingOrders.Size = new Size(337, 449);
      this.gvStackingOrders.TabIndex = 0;
      this.gvStackingOrders.SelectedIndexChanged += new EventHandler(this.gvStackingOrders_SelectedIndexChanged);
      this.gvStackingOrders.EditorClosing += new GVSubItemEditingEventHandler(this.gvStackingOrders_EditorClosing);
      this.pnlStackingOrderControls.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.pnlStackingOrderControls.AutoSize = true;
      this.pnlStackingOrderControls.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      this.pnlStackingOrderControls.BackColor = Color.Transparent;
      this.pnlStackingOrderControls.Controls.Add((Control) this.btnSetAsDefault);
      this.pnlStackingOrderControls.Controls.Add((Control) this.verticalSeparator1);
      this.pnlStackingOrderControls.Controls.Add((Control) this.btnDeleteStackingOrder);
      this.pnlStackingOrderControls.Controls.Add((Control) this.btnDuplicateStackingOrder);
      this.pnlStackingOrderControls.Controls.Add((Control) this.btnAddStackingOrder);
      this.pnlStackingOrderControls.FlowDirection = FlowDirection.RightToLeft;
      this.pnlStackingOrderControls.Location = new Point(175, 2);
      this.pnlStackingOrderControls.Name = "pnlStackingOrderControls";
      this.pnlStackingOrderControls.Size = new Size(158, 22);
      this.pnlStackingOrderControls.TabIndex = 1;
      this.btnSetAsDefault.Enabled = false;
      this.btnSetAsDefault.Location = new Point(70, 0);
      this.btnSetAsDefault.Margin = new Padding(0);
      this.btnSetAsDefault.Name = "btnSetAsDefault";
      this.btnSetAsDefault.Size = new Size(88, 22);
      this.btnSetAsDefault.TabIndex = 4;
      this.btnSetAsDefault.Text = "Set as Default";
      this.btnSetAsDefault.UseVisualStyleBackColor = true;
      this.btnSetAsDefault.Click += new EventHandler(this.btnSetAsDefault_Click);
      this.verticalSeparator1.Location = new Point(66, 3);
      this.verticalSeparator1.Margin = new Padding(3, 3, 2, 3);
      this.verticalSeparator1.MaximumSize = new Size(2, 16);
      this.verticalSeparator1.MinimumSize = new Size(2, 16);
      this.verticalSeparator1.Name = "verticalSeparator1";
      this.verticalSeparator1.Size = new Size(2, 16);
      this.verticalSeparator1.TabIndex = 3;
      this.verticalSeparator1.Text = "verticalSeparator1";
      this.pctSplit.Dock = DockStyle.Left;
      this.pctSplit.Location = new Point(339, 33);
      this.pctSplit.Name = "pctSplit";
      this.pctSplit.Size = new Size(4, 476);
      this.pctSplit.TabIndex = 35;
      this.pctSplit.TabStop = false;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.Controls.Add((Control) this.grpDocuments);
      this.Controls.Add((Control) this.pctSplit);
      this.Controls.Add((Control) this.grpStackingOrders);
      this.Controls.Add((Control) this.gradientPanel1);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (StackingTemplateSetupControl);
      this.Size = new Size(735, 509);
      this.ClientSizeChanged += new EventHandler(this.StackingTemplateSetupControl_ClientSizeChanged);
      ((ISupportInitialize) this.btnResetStackingOrder).EndInit();
      ((ISupportInitialize) this.btnSaveStackingOrder).EndInit();
      ((ISupportInitialize) this.btnRemoveDocument).EndInit();
      ((ISupportInitialize) this.btnMoveDocumentDown).EndInit();
      ((ISupportInitialize) this.btnMoveDocumentUp).EndInit();
      ((ISupportInitialize) this.btnAddDocuments).EndInit();
      ((ISupportInitialize) this.btnDeleteStackingOrder).EndInit();
      ((ISupportInitialize) this.btnDuplicateStackingOrder).EndInit();
      ((ISupportInitialize) this.btnAddStackingOrder).EndInit();
      this.gradientPanel1.ResumeLayout(false);
      this.gradientPanel1.PerformLayout();
      this.grpDocuments.ResumeLayout(false);
      this.grpDocuments.PerformLayout();
      this.pnlDocumentControls.ResumeLayout(false);
      this.grpStackingOrders.ResumeLayout(false);
      this.grpStackingOrders.PerformLayout();
      this.pnlStackingOrderControls.ResumeLayout(false);
      ((ISupportInitialize) this.pctSplit).EndInit();
      this.ResumeLayout(false);
    }
  }
}
