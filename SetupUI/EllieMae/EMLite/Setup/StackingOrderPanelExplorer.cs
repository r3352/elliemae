// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.StackingOrderPanelExplorer
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class StackingOrderPanelExplorer : UserControl
  {
    private FSExplorer tempExplorer;
    private StackingOrderSetTemplate currentStackingTemplate;
    private FileSystemEntry currentEntry;
    private IContainer components;
    private ListView lvwDefault;
    private GradientPanel gradientPanel1;
    private Label label1;
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
    private ImageList imageListSmall;
    private bool isDirty;

    public StackingOrderPanelExplorer()
    {
      this.InitializeComponent();
      this.tempExplorer.SetAsDefaultButtonClick += new EventHandler(this.btnSetDefault_Click);
      TemplateIFSExplorer ifsExplorer = new TemplateIFSExplorer(Session.DefaultInstance, EllieMae.EMLite.ClientServer.TemplateSettingsType.StackingOrder);
      ifsExplorer.DefaultLoanTemplateChanged += new EventHandler(this.refreshDefaultStackingOrderTemplate);
      this.tempExplorer.SelectedEntryChanged += new EventHandler(this.tempExplorer_SelectedEntryChanged);
      this.tempExplorer.FileChanged += new EventHandler(this.tempExplorer_FileChanged);
      this.tempExplorer.FileType = FSExplorer.FileTypes.StackingOrderSets;
      this.tempExplorer.HasPublicRight = true;
      this.tempExplorer.SetProperties(false, false, false, 7, true);
      this.tempExplorer.Init((IFSExplorerBase) ifsExplorer, FileSystemEntry.PublicRoot, true);
      this.tempExplorer.SetupForStackingOrder();
      this.getCurrentDefaultTemplate();
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (StackingOrderPanelExplorer));
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      this.gradientPanel1 = new GradientPanel();
      this.label1 = new Label();
      this.lvwDefault = new ListView();
      this.imageListSmall = new ImageList(this.components);
      this.grpDocuments = new GroupContainer();
      this.gvDocOrder = new GridView();
      this.pnlDocumentControls = new FlowLayoutPanel();
      this.btnResetStackingOrder = new StandardIconButton();
      this.btnSaveStackingOrder = new StandardIconButton();
      this.verticalSeparator2 = new VerticalSeparator();
      this.btnRemoveDocument = new StandardIconButton();
      this.btnMoveDocumentDown = new StandardIconButton();
      this.btnMoveDocumentUp = new StandardIconButton();
      this.btnAddDocuments = new StandardIconButton();
      this.tempExplorer = new FSExplorer();
      this.gradientPanel1.SuspendLayout();
      this.grpDocuments.SuspendLayout();
      this.pnlDocumentControls.SuspendLayout();
      ((ISupportInitialize) this.btnResetStackingOrder).BeginInit();
      ((ISupportInitialize) this.btnSaveStackingOrder).BeginInit();
      ((ISupportInitialize) this.btnRemoveDocument).BeginInit();
      ((ISupportInitialize) this.btnMoveDocumentDown).BeginInit();
      ((ISupportInitialize) this.btnMoveDocumentUp).BeginInit();
      ((ISupportInitialize) this.btnAddDocuments).BeginInit();
      this.SuspendLayout();
      this.tempExplorer.Dock = DockStyle.Left;
      this.tempExplorer.FolderComboSelectedIndex = -1;
      this.tempExplorer.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.tempExplorer.HasPublicRight = true;
      this.tempExplorer.Location = new Point(0, 33);
      this.tempExplorer.Name = "tempExplorer";
      this.tempExplorer.RenameButtonSize = new Size(62, 22);
      this.tempExplorer.RESPAMode = FSExplorer.RESPAFilter.All;
      this.tempExplorer.setContactType = EllieMae.EMLite.ContactUI.ContactType.BizPartner;
      this.tempExplorer.Size = new Size(355, 463);
      this.tempExplorer.TabIndex = 0;
      this.gradientPanel1.Borders = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.gradientPanel1.Controls.Add((Control) this.label1);
      this.gradientPanel1.Controls.Add((Control) this.lvwDefault);
      this.gradientPanel1.Dock = DockStyle.Top;
      this.gradientPanel1.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradientPanel1.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel1.Location = new Point(0, 0);
      this.gradientPanel1.Name = "gradientPanel1";
      this.gradientPanel1.Size = new Size(928, 33);
      this.gradientPanel1.TabIndex = 33;
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
      this.lvwDefault.Size = new Size(792, 20);
      this.lvwDefault.SmallImageList = this.imageListSmall;
      this.lvwDefault.TabIndex = 32;
      this.lvwDefault.UseCompatibleStateImageBehavior = false;
      this.lvwDefault.View = View.List;
      this.imageListSmall.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imageListSmall.ImageStream");
      this.imageListSmall.TransparentColor = Color.Transparent;
      this.imageListSmall.Images.SetKeyName(0, "");
      this.imageListSmall.Images.SetKeyName(1, "");
      this.imageListSmall.Images.SetKeyName(2, "");
      this.imageListSmall.Images.SetKeyName(3, "");
      this.imageListSmall.Images.SetKeyName(4, "template.bmp");
      this.grpDocuments.Controls.Add((Control) this.gvDocOrder);
      this.grpDocuments.Controls.Add((Control) this.pnlDocumentControls);
      this.grpDocuments.Dock = DockStyle.Fill;
      this.grpDocuments.HeaderForeColor = SystemColors.ControlText;
      this.grpDocuments.Location = new Point(355, 33);
      this.grpDocuments.Name = "grpDocuments";
      this.grpDocuments.Size = new Size(573, 463);
      this.grpDocuments.TabIndex = 34;
      this.grpDocuments.Text = "Documents";
      this.gvDocOrder.AllowDrop = true;
      this.gvDocOrder.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.SpringToFit = true;
      gvColumn1.Text = "Name";
      gvColumn1.Width = 471;
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
      this.gvDocOrder.Size = new Size(571, 436);
      this.gvDocOrder.SortIconVisible = false;
      this.gvDocOrder.SortOption = GVSortOption.None;
      this.gvDocOrder.TabIndex = 3;
      this.gvDocOrder.SelectedIndexChanged += new EventHandler(this.gvDocOrder_SelectedIndexChanged);
      this.gvDocOrder.ItemDrag += new GVItemEventHandler(this.gvDocOrder_ItemDrag);
      this.gvDocOrder.DragDrop += new DragEventHandler(this.gvDocOrder_DragDrop);
      this.gvDocOrder.DragOver += new DragEventHandler(this.gvDocOrder_DragOver);
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
      this.pnlDocumentControls.Location = new Point(437, 2);
      this.pnlDocumentControls.Name = "pnlDocumentControls";
      this.pnlDocumentControls.Size = new Size(130, 22);
      this.pnlDocumentControls.TabIndex = 2;
      this.btnResetStackingOrder.BackColor = Color.Transparent;
      this.btnResetStackingOrder.Location = new Point(114, 3);
      this.btnResetStackingOrder.Margin = new Padding(5, 3, 0, 3);
      this.btnResetStackingOrder.MouseDownImage = (Image) null;
      this.btnResetStackingOrder.Name = "btnResetStackingOrder";
      this.btnResetStackingOrder.Size = new Size(16, 16);
      this.btnResetStackingOrder.StandardButtonType = StandardIconButton.ButtonType.ResetButton;
      this.btnResetStackingOrder.TabIndex = 0;
      this.btnResetStackingOrder.TabStop = false;
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
      this.btnSaveStackingOrder.Click += new EventHandler(this.btnSaveStackingOrder_Click);
      this.verticalSeparator2.Location = new Point(87, 3);
      this.verticalSeparator2.Margin = new Padding(3, 3, 2, 3);
      this.verticalSeparator2.MaximumSize = new Size(2, 16);
      this.verticalSeparator2.MinimumSize = new Size(2, 16);
      this.verticalSeparator2.Name = "verticalSeparator2";
      this.verticalSeparator2.Size = new Size(2, 16);
      this.verticalSeparator2.TabIndex = 4;
      this.verticalSeparator2.Text = "verticalSeparator2";
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
      this.btnAddDocuments.Click += new EventHandler(this.btnAddDocuments_Click);
      this.tempExplorer.Dock = DockStyle.Left;
      this.tempExplorer.FolderComboSelectedIndex = -1;
      this.tempExplorer.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.tempExplorer.HasPublicRight = true;
      this.tempExplorer.Location = new Point(0, 33);
      this.tempExplorer.Name = "tempExplorer";
      this.tempExplorer.RenameButtonSize = new Size(62, 22);
      this.tempExplorer.RESPAMode = FSExplorer.RESPAFilter.All;
      this.tempExplorer.setContactType = EllieMae.EMLite.ContactUI.ContactType.BizPartner;
      this.tempExplorer.Size = new Size(355, 463);
      this.tempExplorer.TabIndex = 0;
      this.Controls.Add((Control) this.grpDocuments);
      this.Controls.Add((Control) this.tempExplorer);
      this.Controls.Add((Control) this.gradientPanel1);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (StackingOrderPanelExplorer);
      this.Size = new Size(928, 496);
      this.gradientPanel1.ResumeLayout(false);
      this.gradientPanel1.PerformLayout();
      this.grpDocuments.ResumeLayout(false);
      this.grpDocuments.PerformLayout();
      this.pnlDocumentControls.ResumeLayout(false);
      ((ISupportInitialize) this.btnResetStackingOrder).EndInit();
      ((ISupportInitialize) this.btnSaveStackingOrder).EndInit();
      ((ISupportInitialize) this.btnRemoveDocument).EndInit();
      ((ISupportInitialize) this.btnMoveDocumentDown).EndInit();
      ((ISupportInitialize) this.btnMoveDocumentUp).EndInit();
      ((ISupportInitialize) this.btnAddDocuments).EndInit();
      this.ResumeLayout(false);
    }

    public virtual bool IsDirty => this.isDirty;

    protected virtual void setDirtyFlag(bool val) => this.isDirty = val;

    private void btnSetDefault_Click(object sender, EventArgs e)
    {
      if (this.tempExplorer.SelectedItems.Count != 1)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "To setup a default stacking order template, you have to select a single template first.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        Session.ConfigurationManager.SetCompanySetting("StackingOrder", "Default", ((FileSystemEntry) this.tempExplorer.SelectedItems[0].Tag).ToString());
        this.getCurrentDefaultTemplate();
      }
    }

    private void refreshDefaultStackingOrderTemplate(object sender, EventArgs e)
    {
      this.getCurrentDefaultTemplate();
    }

    private void getCurrentDefaultTemplate()
    {
      this.lvwDefault.Items.Clear();
      string companySetting = Session.ConfigurationManager.GetCompanySetting("StackingOrder", "Default");
      try
      {
        this.lvwDefault.Items.Add(FileSystemEntry.RemoveRoot(FileSystemEntry.Parse(companySetting).ToDisplayString()));
        this.lvwDefault.Items[0].ImageIndex = 4;
      }
      catch
      {
      }
    }

    private void tempExplorer_SelectedEntryChanged(object sender, EventArgs e)
    {
      this.PromptToCommit();
      this.currentEntry = (FileSystemEntry) null;
      if (this.tempExplorer.SelectedItems.Count > 0)
      {
        this.currentEntry = (FileSystemEntry) this.tempExplorer.SelectedItems[0].Tag;
        this.currentStackingTemplate = (StackingOrderSetTemplate) Session.DefaultInstance.ConfigurationManager.GetTemplateSettings(EllieMae.EMLite.ClientServer.TemplateSettingsType.StackingOrder, this.currentEntry);
      }
      this.setCurrentStackingOrder(this.currentStackingTemplate);
    }

    private void tempExplorer_FileChanged(object sender, EventArgs e) => this.refreshDocuments();

    private void setCurrentStackingOrder(StackingOrderSetTemplate stackingOrder)
    {
      this.grpDocuments.Enabled = true;
      this.currentStackingTemplate = stackingOrder;
      this.refreshDocuments();
      this.btnSaveStackingOrder.Enabled = false;
      this.btnResetStackingOrder.Enabled = false;
      this.setDirtyFlag(false);
    }

    private void refreshDocuments()
    {
      if (this.tempExplorer.SelectedItems.Count == 0)
        return;
      DocumentTrackingSetup documentTrackingSetup = Session.ConfigurationManager.GetDocumentTrackingSetup();
      StackingOrderSetTemplate orderSetTemplate;
      if (this.currentStackingTemplate == null)
      {
        FileSystemEntry tag = (FileSystemEntry) this.tempExplorer.SelectedItems[0].Tag;
        orderSetTemplate = (StackingOrderSetTemplate) Session.DefaultInstance.ConfigurationManager.GetTemplateSettings(EllieMae.EMLite.ClientServer.TemplateSettingsType.StackingOrder, tag);
        if (orderSetTemplate == null)
        {
          int num = (int) Utils.Dialog((IWin32Window) null, "The template \"" + tag.Name + "\" has been deleted.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
      }
      else
        orderSetTemplate = this.currentStackingTemplate;
      ArrayList requiredDocs = orderSetTemplate.RequiredDocs;
      this.gvDocOrder.Items.Clear();
      foreach (string docName in orderSetTemplate.DocNames)
        this.gvDocOrder.Items.Add(this.createGVItemForStackingOrder(documentTrackingSetup.GetByName(docName) ?? new DocumentTemplate(docName), requiredDocs.Contains((object) docName)));
      this.grpDocuments.Text = "Documents (" + (object) this.gvDocOrder.Items.Count + ")";
    }

    private GVItem createGVItemForStackingOrder(DocumentTemplate doc, bool isRequired)
    {
      GVItem forStackingOrder = new GVItem();
      forStackingOrder.SubItems[0].Value = (object) doc.Name;
      forStackingOrder.SubItems[1].Value = (object) "No";
      if (isRequired)
        forStackingOrder.SubItems[1].Value = (object) "Yes";
      forStackingOrder.Tag = (object) doc;
      return forStackingOrder;
    }

    private void btnAddDocuments_Click(object sender, EventArgs e)
    {
      if (!this.PromptToCommit())
        return;
      this.commitDocumentListToStackingOrder();
      using (StackingOrderSetTemplateDialog setTemplateDialog = new StackingOrderSetTemplateDialog(this.currentStackingTemplate))
      {
        if (setTemplateDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.currentStackingTemplate = setTemplateDialog.StackingOrderTemplate;
        this.refreshDocuments();
        this.onStackingOrderPropertyChanged(sender, e);
      }
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

    private void btnRemoveDocument_Click(object sender, EventArgs e)
    {
      if (MessageBox.Show((IWin32Window) this, "Are you sure you want to remove the " + (object) this.gvDocOrder.SelectedItems.Count + " selected document(s) from the stacking template?", "Encompass Docs", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        return;
      for (int nItemIndex = this.gvDocOrder.Items.Count - 1; nItemIndex >= 0; --nItemIndex)
      {
        if (this.gvDocOrder.Items[nItemIndex].Selected)
          this.gvDocOrder.Items.RemoveAt(nItemIndex);
      }
      this.grpDocuments.Text = "Documents (" + (object) this.gvDocOrder.Items.Count + ")";
      this.onStackingOrderPropertyChanged(sender, e);
    }

    private void btnSaveStackingOrder_Click(object sender, EventArgs e) => this.commitChanges();

    private void btnResetStackingOrder_Click(object sender, EventArgs e)
    {
      if (Utils.Dialog((IWin32Window) this, "Discard your changes to the current stacking template?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        return;
      this.currentEntry = (FileSystemEntry) this.tempExplorer.SelectedItems[0].Tag;
      this.setCurrentStackingOrder((StackingOrderSetTemplate) Session.DefaultInstance.ConfigurationManager.GetTemplateSettings(EllieMae.EMLite.ClientServer.TemplateSettingsType.StackingOrder, this.currentEntry));
    }

    private void gvDocOrder_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnMoveDocumentUp.Enabled = this.gvDocOrder.SelectedItems.Count > 0;
      this.btnMoveDocumentDown.Enabled = this.gvDocOrder.SelectedItems.Count > 0;
      this.btnRemoveDocument.Enabled = this.gvDocOrder.SelectedItems.Count > 0;
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

    private void commitDocumentListToStackingOrder()
    {
      this.currentStackingTemplate.DocNames.Clear();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvDocOrder.Items)
        this.currentStackingTemplate.DocNames.Add((object) ((DocumentTemplate) gvItem.Tag).Name);
    }

    private void onStackingOrderPropertyChanged(object sender, EventArgs e)
    {
      this.btnSaveStackingOrder.Enabled = true;
      this.btnResetStackingOrder.Enabled = true;
      this.setDirtyFlag(true);
    }

    public bool PromptToCommit()
    {
      if (this.currentStackingTemplate == null || !this.btnSaveStackingOrder.Enabled)
        return true;
      if (Utils.Dialog((IWin32Window) this, "Save the changes to the current stacking template?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
      {
        this.setCurrentStackingOrder(this.currentStackingTemplate);
        return true;
      }
      this.commitChanges();
      return true;
    }

    private void commitChanges()
    {
      this.commitDocumentListToStackingOrder();
      Session.ConfigurationManager.SaveTemplateSettings(EllieMae.EMLite.ClientServer.TemplateSettingsType.StackingOrder, this.currentEntry, (BinaryObject) (BinaryConvertibleObject) this.currentStackingTemplate);
      this.btnSaveStackingOrder.Enabled = false;
      this.btnResetStackingOrder.Enabled = false;
      this.setDirtyFlag(false);
    }
  }
}
