// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.StackingOrderMgmtControl
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DocEngine;
using EllieMae.EMLite.LoanServices;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class StackingOrderMgmtControl : SettingsUserControl
  {
    private Sessions.Session session;
    private DocEngineStackingOrder currentStackingOrder;
    private bool readOnly;
    private DocumentOrderType orderType;
    private const string DEFAULT_SELECTED_VALUE = "Yes";
    private IContainer components;
    private GroupContainer grpStackingOrders;
    private CollapsibleSplitter collapsibleSplitter1;
    private GridView gvStackingOrders;
    private GroupContainer grpDocuments;
    private FlowLayoutPanel pnlStackingOrderControls;
    private StandardIconButton btnDeleteStackingOrder;
    private StandardIconButton btnDuplicateStackingOrder;
    private StandardIconButton btnAddStackingOrder;
    private FlowLayoutPanel pnlDocumentControls;
    private StandardIconButton btnRemoveDocument;
    private StandardIconButton btnMoveDocumentDown;
    private StandardIconButton btnMoveDocumentUp;
    private StandardIconButton btnResetStackingOrder;
    private StandardIconButton btnSaveStackingOrder;
    private ToolTip toolTip1;
    private GridView gvDocOrder;
    private StandardIconButton btnAddDocuments;
    private VerticalSeparator verticalSeparator1;
    private Button btnSetAsDefault;
    private VerticalSeparator verticalSeparator2;
    private StandardIconButton btnExportStackingOrder;
    private SaveFileDialog sfdExport;
    private StandardIconButton btnImportStackingOrder;
    private OpenFileDialog ofdImport;

    public StackingOrderMgmtControl(
      SetUpContainer container,
      Sessions.Session session,
      DocumentOrderType orderType)
      : this(container, session, orderType, false)
    {
    }

    public StackingOrderMgmtControl(
      SetUpContainer container,
      Sessions.Session session,
      DocumentOrderType orderType,
      bool allowMultiSelect)
      : base(container)
    {
      this.session = session;
      this.orderType = orderType;
      this.InitializeComponent();
      if (!this.DesignMode)
      {
        this.clearStackingOrder();
        this.gvStackingOrders.Sort(0, SortOrder.Ascending);
        this.loadStackingOrders();
      }
      this.btnExportStackingOrder.Visible = EnConfigurationSettings.GlobalSettings.Debug;
      this.btnImportStackingOrder.Visible = EnConfigurationSettings.GlobalSettings.Debug;
      this.gvStackingOrders.AllowMultiselect = allowMultiSelect;
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
      DocEngineStackingOrderInfo[] engineStackingOrders = this.session.ConfigurationManager.GetDocEngineStackingOrders(this.orderType);
      this.gvStackingOrders.Items.Clear();
      foreach (DocEngineStackingOrderInfo orderInfo in engineStackingOrders)
        this.gvStackingOrders.Items.Add(this.createGVItemForStackingOrder(orderInfo));
      this.gvStackingOrders.ReSort();
      this.refreshStackingOrderCount();
    }

    private void refreshStackingOrderCount()
    {
      this.grpStackingOrders.Text = "Stacking Templates (" + (object) this.gvStackingOrders.Items.Count + ")";
    }

    private void refreshStakingDocumentCount()
    {
      this.grpDocuments.Text = "Documents (" + (object) this.gvDocOrder.Items.Count + ")";
    }

    private GVItem createGVItemForStackingOrder(DocEngineStackingOrderInfo orderInfo)
    {
      GVItem forStackingOrder = new GVItem();
      forStackingOrder.Tag = (object) orderInfo;
      this.populateStackingOrderGVItemProperties(forStackingOrder);
      return forStackingOrder;
    }

    private void populateStackingOrderGVItemProperties(GVItem item)
    {
      DocEngineStackingOrderInfo tag = (DocEngineStackingOrderInfo) item.Tag;
      item.SubItems[0].Text = tag.Name;
      item.SubItems[1].Text = tag.IsDefault ? "Yes" : "";
    }

    private void gvStackingOrders_SelectedIndexChanged(object sender, EventArgs e)
    {
      string guid = this.currentStackingOrder == null ? (string) null : this.currentStackingOrder.Guid;
      DocEngineStackingOrderInfo stackingOrderInfo = (DocEngineStackingOrderInfo) null;
      string orderID = (string) null;
      if (this.gvStackingOrders.SelectedItems.Count > 0)
      {
        stackingOrderInfo = (DocEngineStackingOrderInfo) this.gvStackingOrders.SelectedItems[0].Tag;
        orderID = stackingOrderInfo.Guid;
      }
      this.btnDeleteStackingOrder.Enabled = this.gvStackingOrders.SelectedItems.Count == 1;
      this.btnDuplicateStackingOrder.Enabled = this.gvStackingOrders.SelectedItems.Count == 1;
      this.btnExportStackingOrder.Enabled = this.gvStackingOrders.SelectedItems.Count > 0;
      this.btnSetAsDefault.Enabled = stackingOrderInfo != null && !stackingOrderInfo.IsDefault && this.gvStackingOrders.SelectedItems.Count == 1;
      this.btnAddDocuments.Enabled = this.gvStackingOrders.SelectedItems.Count == 1;
      if (orderID == guid)
        return;
      if (!this.PromptToCommit())
      {
        this.selectStackingOrder(guid);
      }
      else
      {
        if (this.gvStackingOrders.SelectedItems.Count > 0 && !this.setCurrentStackingOrder(orderID))
          this.selectStackingOrder(guid);
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
      if (this.currentStackingOrder == null || !this.btnSaveStackingOrder.Enabled)
        return true;
      switch (Utils.Dialog((IWin32Window) this, "Save the changes to the current stacking template?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
      {
        case DialogResult.Cancel:
          return false;
        case DialogResult.No:
          this.setCurrentStackingOrder(this.currentStackingOrder);
          return true;
        default:
          this.commitChanges();
          return true;
      }
    }

    private void selectStackingOrder(string orderID)
    {
      if (orderID == null)
      {
        this.clearStackingOrder();
      }
      else
      {
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvStackingOrders.Items)
        {
          if (((DocEngineStackingOrderInfo) gvItem.Tag).Guid == orderID)
          {
            gvItem.Selected = true;
            break;
          }
        }
      }
    }

    private void clearStackingOrder()
    {
      this.currentStackingOrder = (DocEngineStackingOrder) null;
      this.gvDocOrder.Items.Clear();
      this.grpDocuments.Enabled = false;
    }

    private bool setCurrentStackingOrder(string orderID)
    {
      DocEngineStackingOrder engineStackingOrder = this.session.ConfigurationManager.GetDocEngineStackingOrder(orderID);
      if (engineStackingOrder == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The selected stacking template has been deleted or is no longer accessible.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      this.setCurrentStackingOrder(engineStackingOrder);
      return true;
    }

    private void setCurrentStackingOrder(DocEngineStackingOrder stackingOrder)
    {
      this.grpDocuments.Enabled = true;
      this.currentStackingOrder = stackingOrder;
      this.reloadStackingDocumentList();
      this.btnSaveStackingOrder.Enabled = false;
      this.btnResetStackingOrder.Enabled = false;
      this.setDirtyFlag(false);
    }

    private void reloadStackingDocumentList()
    {
      this.gvDocOrder.Items.Clear();
      foreach (StackingElement element in this.currentStackingOrder.Elements)
        this.gvDocOrder.Items.Add(this.createGVItemForElement(element));
      this.refreshStakingDocumentCount();
    }

    private GVItem createGVItemForElement(StackingElement element)
    {
      return new GVItem()
      {
        SubItems = {
          [0] = {
            Value = (object) new StackingDisplayElement(element)
          }
        },
        Tag = (object) element
      };
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
      this.refreshStakingDocumentCount();
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

    private bool stackingOrderExists(DocumentOrderType orderType, string name)
    {
      return this.session.ConfigurationManager.GetDocEngineStackingOrder(orderType, name) != null;
    }

    private void commitChanges()
    {
      this.commitDocumentListToStackingOrder();
      this.session.ConfigurationManager.SaveDocEngineStackingOrder(this.currentStackingOrder);
      this.btnSaveStackingOrder.Enabled = false;
      this.btnResetStackingOrder.Enabled = false;
      this.setDirtyFlag(false);
      this.updateStackingOrderList(this.currentStackingOrder);
    }

    private void commitDocumentListToStackingOrder()
    {
      this.currentStackingOrder.Elements.Clear();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvDocOrder.Items)
        this.currentStackingOrder.Elements.Add((StackingElement) gvItem.Tag);
    }

    private void updateStackingOrderList(DocEngineStackingOrder stackingOrder)
    {
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvStackingOrders.Items)
      {
        DocEngineStackingOrderInfo tag = (DocEngineStackingOrderInfo) gvItem.Tag;
        if (tag.Guid == stackingOrder.Guid)
        {
          gvItem.Tag = (object) new DocEngineStackingOrderInfo(stackingOrder.Guid, stackingOrder.Name, stackingOrder.Type, tag.IsDefault, stackingOrder.Elements.Count);
          this.populateStackingOrderGVItemProperties(gvItem);
          break;
        }
      }
    }

    private void btnResetStackingOrder_Click(object sender, EventArgs e)
    {
      if (Utils.Dialog((IWin32Window) this, "Discard your changes to the current stacking template?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        return;
      this.setCurrentStackingOrder(((DocEngineStackingOrderInfo) this.gvStackingOrders.SelectedItems[0].Tag).Guid);
    }

    private void btnAddStackingOrder_Click(object sender, EventArgs e)
    {
      if (!this.PromptToCommit())
        return;
      this.gvStackingOrders.SelectedItems.Clear();
      DocumentOrderType orderType = this.orderType;
      DocEngineStackingOrder stackingOrder = new DocEngineStackingOrder(this.generateUniqueStackingOrderName(orderType, "New Stacking Template"), orderType);
      this.session.ConfigurationManager.SaveDocEngineStackingOrder(stackingOrder);
      GVItem forStackingOrder = this.createGVItemForStackingOrder(new DocEngineStackingOrderInfo(stackingOrder.Guid, stackingOrder.Name, stackingOrder.Type, false, stackingOrder.Elements.Count));
      this.gvStackingOrders.Items.Add(forStackingOrder);
      forStackingOrder.Selected = true;
      this.refreshStackingOrderCount();
    }

    private string generateUniqueStackingOrderName(DocumentOrderType orderType, string baseName)
    {
      if (baseName.Length > 90)
        baseName = baseName.Substring(0, 90);
      int num = 1;
      string name;
      while (true)
      {
        string str;
        if (num != 1)
          str = baseName + " (" + (object) num + ")";
        else
          str = baseName;
        name = str;
        if (this.stackingOrderExists(orderType, name))
          ++num;
        else
          break;
      }
      return name;
    }

    private void btnDuplicateStackingOrder_Click(object sender, EventArgs e)
    {
      if (!this.PromptToCommit())
        return;
      string stackingOrderName = this.generateUniqueStackingOrderName(this.orderType, "Copy of " + this.currentStackingOrder.Name);
      DocEngineStackingOrder stackingOrder = (DocEngineStackingOrder) this.currentStackingOrder.Duplicate();
      stackingOrder.Name = stackingOrderName;
      this.session.ConfigurationManager.SaveDocEngineStackingOrder(stackingOrder);
      GVItem forStackingOrder = this.createGVItemForStackingOrder(new DocEngineStackingOrderInfo(stackingOrder.Guid, stackingOrder.Name, stackingOrder.Type, false, stackingOrder.Elements.Count));
      this.gvStackingOrders.Items.Add(forStackingOrder);
      forStackingOrder.Selected = true;
      this.refreshStackingOrderCount();
    }

    private void btnDeleteStackingOrder_Click(object sender, EventArgs e)
    {
      if (Utils.Dialog((IWin32Window) this, "Are you sure you want to permanently delete the '" + this.currentStackingOrder.Name + "' stacking template?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
        return;
      this.session.ConfigurationManager.DeleteDocEngineStackingOrder(this.currentStackingOrder.Guid);
      this.currentStackingOrder = (DocEngineStackingOrder) null;
      this.gvStackingOrders.Items.Remove(this.gvStackingOrders.SelectedItems[0]);
      this.refreshStackingOrderCount();
    }

    private void btnAddDocuments_Click(object sender, EventArgs e)
    {
      this.commitDocumentListToStackingOrder();
      using (StackingOrderTrainingDialog orderTrainingDialog = new StackingOrderTrainingDialog(this.currentStackingOrder, this.session))
      {
        if (orderTrainingDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.reloadStackingDocumentList();
        this.onStackingOrderPropertyChanged(sender, e);
      }
    }

    private void btnSetAsDefault_Click(object sender, EventArgs e)
    {
      DocEngineStackingOrderInfo tag1 = (DocEngineStackingOrderInfo) this.gvStackingOrders.SelectedItems[0].Tag;
      this.session.ConfigurationManager.SetDefaultDocEngineStackingOrder(tag1.Guid);
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvStackingOrders.Items)
      {
        DocEngineStackingOrderInfo tag2 = (DocEngineStackingOrderInfo) gvItem.Tag;
        tag2.IsDefault = tag2.Guid == tag1.Guid;
        this.populateStackingOrderGVItemProperties(gvItem);
      }
      this.btnSetAsDefault.Enabled = false;
    }

    private void gvStackingOrders_EditorClosing(object source, GVSubItemEditingEventArgs e)
    {
      GVItem parent = e.SubItem.Parent;
      DocEngineStackingOrderInfo tag = (DocEngineStackingOrderInfo) e.SubItem.Item.Tag;
      string str = e.EditorControl.Text.Trim();
      if (str == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must specify a name for the stacking template.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        e.Cancel = true;
        e.EditorControl.Text = tag.Name;
      }
      else if (str.IndexOfAny("\\/:*?\"<>|".ToCharArray()) >= 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The stacking template name may not contain any of the following characters: \\/:*?\"<>|", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        e.Cancel = true;
        e.EditorControl.Text = tag.Name;
      }
      else
      {
        if (str == tag.Name)
          return;
        if (string.Compare(str, tag.Name, true) != 0 && this.stackingOrderExists(tag.Type, str))
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "A stacking template with the name '" + str + "' already exists. You must provide a unique name for this template.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          e.Cancel = true;
          e.EditorControl.Text = tag.Name;
        }
        else
        {
          DocEngineStackingOrder engineStackingOrder = this.session.ConfigurationManager.GetDocEngineStackingOrder(tag.Guid);
          if (engineStackingOrder == null)
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "The specified stacking template has been deleted or can no longer be accessed.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            this.loadStackingOrders();
          }
          else
          {
            engineStackingOrder.Name = str;
            this.session.ConfigurationManager.SaveDocEngineStackingOrder(engineStackingOrder);
            e.SubItem.Item.Tag = (object) new DocEngineStackingOrderInfo(tag.Guid, str, tag.Type, tag.IsDefault, tag.ElementCount);
            if (this.currentStackingOrder == null || !(this.currentStackingOrder.Guid == tag.Guid))
              return;
            this.currentStackingOrder.Name = str;
          }
        }
      }
    }

    private void btnExportStackingOrder_Click(object sender, EventArgs e)
    {
      this.sfdExport.FileName = this.currentStackingOrder.Name;
      if (this.sfdExport.ShowDialog((IWin32Window) this) != DialogResult.OK)
        return;
      ((BinaryObject) (BinaryConvertibleObject) this.currentStackingOrder).Write(this.sfdExport.FileName);
      int num = (int) Utils.Dialog((IWin32Window) this, "The stacking template has been exported successfully.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
    }

    private void btnImportStackingOrder_Click(object sender, EventArgs e)
    {
      if (this.ofdImport.ShowDialog((IWin32Window) this) != DialogResult.OK)
        return;
      try
      {
        using (BinaryObject binaryObject = new BinaryObject(this.ofdImport.FileName))
        {
          DocEngineStackingOrder engineStackingOrder = (DocEngineStackingOrder) binaryObject;
          if (engineStackingOrder.Type != this.orderType)
          {
            if (Utils.Dialog((IWin32Window) this, "The specified stacking template is not appropriate for this context. Would you like to proceed anyway?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
              return;
            engineStackingOrder.Type = this.orderType;
          }
          string stackingOrderName = this.generateUniqueStackingOrderName(engineStackingOrder.Type, engineStackingOrder.Name);
          DocEngineStackingOrder stackingOrder = (DocEngineStackingOrder) engineStackingOrder.Duplicate();
          stackingOrder.Name = stackingOrderName;
          this.session.ConfigurationManager.SaveDocEngineStackingOrder(stackingOrder);
          this.loadStackingOrders();
          int num = (int) Utils.Dialog((IWin32Window) this, "The stacking template has been imported with the name '" + stackingOrder.Name + "'.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "An error occurred while importing the stacking template: " + (object) ex, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
    }

    public string[] GetSelectedStackingTemplates
    {
      get
      {
        return this.gvStackingOrders.SelectedItems.Count == 0 ? (string[]) null : this.gvStackingOrders.SelectedItems.Select<GVItem, string>((Func<GVItem, string>) (items => ((DocEngineStackingOrderInfo) items.Tag).Name + "_" + (object) (int) ((DocEngineStackingOrderInfo) items.Tag).Type)).ToArray<string>();
      }
    }

    public void HighlightStackingTemplates(string[] values)
    {
      if (values.Length == 0)
        return;
      for (int index = 0; index < values.Length; ++index)
      {
        for (int nItemIndex = 0; nItemIndex < this.gvStackingOrders.Items.Count; ++nItemIndex)
        {
          if (values[index].Split('_')[0] == ((DocEngineStackingOrderInfo) this.gvStackingOrders.Items[nItemIndex].Tag).Name)
          {
            this.gvStackingOrders.Items[nItemIndex].Selected = true;
            break;
          }
        }
      }
    }

    public string SelectedDefaultTemplateName
    {
      get
      {
        return this.gvStackingOrders.SelectedItems.Count == 0 ? (string) null : this.gvStackingOrders.SelectedItems.Where<GVItem>((Func<GVItem, bool>) (item => item.SubItems[1].ToString().Equals("Yes"))).Select<GVItem, string>((Func<GVItem, string>) (item => item.SubItems[0].Text)).FirstOrDefault<string>();
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
      this.btnExportStackingOrder = new StandardIconButton();
      this.btnImportStackingOrder = new StandardIconButton();
      this.grpDocuments = new GroupContainer();
      this.gvDocOrder = new GridView();
      this.pnlDocumentControls = new FlowLayoutPanel();
      this.verticalSeparator2 = new VerticalSeparator();
      this.collapsibleSplitter1 = new CollapsibleSplitter();
      this.grpStackingOrders = new GroupContainer();
      this.gvStackingOrders = new GridView();
      this.pnlStackingOrderControls = new FlowLayoutPanel();
      this.btnSetAsDefault = new Button();
      this.verticalSeparator1 = new VerticalSeparator();
      this.sfdExport = new SaveFileDialog();
      this.ofdImport = new OpenFileDialog();
      ((ISupportInitialize) this.btnResetStackingOrder).BeginInit();
      ((ISupportInitialize) this.btnSaveStackingOrder).BeginInit();
      ((ISupportInitialize) this.btnRemoveDocument).BeginInit();
      ((ISupportInitialize) this.btnMoveDocumentDown).BeginInit();
      ((ISupportInitialize) this.btnMoveDocumentUp).BeginInit();
      ((ISupportInitialize) this.btnAddDocuments).BeginInit();
      ((ISupportInitialize) this.btnDeleteStackingOrder).BeginInit();
      ((ISupportInitialize) this.btnDuplicateStackingOrder).BeginInit();
      ((ISupportInitialize) this.btnAddStackingOrder).BeginInit();
      ((ISupportInitialize) this.btnExportStackingOrder).BeginInit();
      ((ISupportInitialize) this.btnImportStackingOrder).BeginInit();
      this.grpDocuments.SuspendLayout();
      this.pnlDocumentControls.SuspendLayout();
      this.grpStackingOrders.SuspendLayout();
      this.pnlStackingOrderControls.SuspendLayout();
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
      this.btnDeleteStackingOrder.Location = new Point(89, 3);
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
      this.btnExportStackingOrder.BackColor = Color.Transparent;
      this.btnExportStackingOrder.Enabled = false;
      this.btnExportStackingOrder.Location = new Point(68, 3);
      this.btnExportStackingOrder.Margin = new Padding(5, 3, 0, 3);
      this.btnExportStackingOrder.MouseDownImage = (Image) null;
      this.btnExportStackingOrder.Name = "btnExportStackingOrder";
      this.btnExportStackingOrder.Size = new Size(16, 16);
      this.btnExportStackingOrder.StandardButtonType = StandardIconButton.ButtonType.ExportDataToFileButton;
      this.btnExportStackingOrder.TabIndex = 5;
      this.btnExportStackingOrder.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnExportStackingOrder, "Export Stacking Template");
      this.btnExportStackingOrder.Click += new EventHandler(this.btnExportStackingOrder_Click);
      this.btnImportStackingOrder.BackColor = Color.Transparent;
      this.btnImportStackingOrder.Location = new Point(47, 3);
      this.btnImportStackingOrder.Margin = new Padding(5, 3, 0, 3);
      this.btnImportStackingOrder.MouseDownImage = (Image) null;
      this.btnImportStackingOrder.Name = "btnImportStackingOrder";
      this.btnImportStackingOrder.Size = new Size(16, 16);
      this.btnImportStackingOrder.StandardButtonType = StandardIconButton.ButtonType.ImportLoanButton;
      this.btnImportStackingOrder.TabIndex = 6;
      this.btnImportStackingOrder.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnImportStackingOrder, "Import Stacking Template");
      this.btnImportStackingOrder.Click += new EventHandler(this.btnImportStackingOrder_Click);
      this.grpDocuments.Controls.Add((Control) this.gvDocOrder);
      this.grpDocuments.Controls.Add((Control) this.pnlDocumentControls);
      this.grpDocuments.Dock = DockStyle.Fill;
      this.grpDocuments.HeaderForeColor = SystemColors.ControlText;
      this.grpDocuments.Location = new Point(346, 0);
      this.grpDocuments.Name = "grpDocuments";
      this.grpDocuments.Size = new Size(389, 509);
      this.grpDocuments.TabIndex = 2;
      this.grpDocuments.Text = "Documents";
      this.gvDocOrder.AllowDrop = true;
      this.gvDocOrder.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.SpringToFit = true;
      gvColumn1.Text = "Document";
      gvColumn1.Width = 387;
      this.gvDocOrder.Columns.AddRange(new GVColumn[1]
      {
        gvColumn1
      });
      this.gvDocOrder.Dock = DockStyle.Fill;
      this.gvDocOrder.DropTarget = GVDropTarget.BetweenItems;
      this.gvDocOrder.HeaderHeight = 0;
      this.gvDocOrder.HeaderVisible = false;
      this.gvDocOrder.Location = new Point(1, 26);
      this.gvDocOrder.Name = "gvDocOrder";
      this.gvDocOrder.Size = new Size(387, 482);
      this.gvDocOrder.SortIconVisible = false;
      this.gvDocOrder.SortOption = GVSortOption.None;
      this.gvDocOrder.TabIndex = 3;
      this.gvDocOrder.SelectedIndexChanged += new EventHandler(this.gvDocOrder_SelectedIndexChanged);
      this.gvDocOrder.DragDrop += new DragEventHandler(this.gvDocOrder_DragDrop);
      this.gvDocOrder.ItemDrag += new GVItemEventHandler(this.gvDocOrder_ItemDrag);
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
      this.pnlDocumentControls.Location = new Point(253, 2);
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
      this.collapsibleSplitter1.AnimationDelay = 20;
      this.collapsibleSplitter1.AnimationStep = 20;
      this.collapsibleSplitter1.BorderStyle3D = Border3DStyle.Flat;
      this.collapsibleSplitter1.ControlToHide = (Control) this.grpStackingOrders;
      this.collapsibleSplitter1.ExpandParentForm = false;
      this.collapsibleSplitter1.Location = new Point(339, 0);
      this.collapsibleSplitter1.Name = "collapsibleSplitter1";
      this.collapsibleSplitter1.TabIndex = 1;
      this.collapsibleSplitter1.TabStop = false;
      this.collapsibleSplitter1.UseAnimations = false;
      this.collapsibleSplitter1.VisualStyle = VisualStyles.Encompass;
      this.grpStackingOrders.Controls.Add((Control) this.gvStackingOrders);
      this.grpStackingOrders.Controls.Add((Control) this.pnlStackingOrderControls);
      this.grpStackingOrders.Dock = DockStyle.Left;
      this.grpStackingOrders.HeaderForeColor = SystemColors.ControlText;
      this.grpStackingOrders.Location = new Point(0, 0);
      this.grpStackingOrders.Name = "grpStackingOrders";
      this.grpStackingOrders.Size = new Size(339, 509);
      this.grpStackingOrders.TabIndex = 0;
      this.grpStackingOrders.Text = "Stacking Orders";
      this.gvStackingOrders.AllowMultiselect = false;
      this.gvStackingOrders.BorderStyle = BorderStyle.None;
      this.gvStackingOrders.ClearSelectionsOnEmptyRowClick = false;
      gvColumn2.ActivatedEditorType = GVActivatedEditorType.TextBox;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column1";
      gvColumn2.SpringToFit = true;
      gvColumn2.Text = "Name";
      gvColumn2.Width = 277;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column2";
      gvColumn3.Text = "Default";
      gvColumn3.Width = 60;
      this.gvStackingOrders.Columns.AddRange(new GVColumn[2]
      {
        gvColumn2,
        gvColumn3
      });
      this.gvStackingOrders.Dock = DockStyle.Fill;
      this.gvStackingOrders.Location = new Point(1, 26);
      this.gvStackingOrders.Name = "gvStackingOrders";
      this.gvStackingOrders.Size = new Size(337, 482);
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
      this.pnlStackingOrderControls.Controls.Add((Control) this.btnExportStackingOrder);
      this.pnlStackingOrderControls.Controls.Add((Control) this.btnImportStackingOrder);
      this.pnlStackingOrderControls.Controls.Add((Control) this.btnDuplicateStackingOrder);
      this.pnlStackingOrderControls.Controls.Add((Control) this.btnAddStackingOrder);
      this.pnlStackingOrderControls.FlowDirection = FlowDirection.RightToLeft;
      this.pnlStackingOrderControls.Location = new Point(133, 2);
      this.pnlStackingOrderControls.Name = "pnlStackingOrderControls";
      this.pnlStackingOrderControls.Size = new Size(200, 22);
      this.pnlStackingOrderControls.TabIndex = 1;
      this.btnSetAsDefault.Enabled = false;
      this.btnSetAsDefault.Location = new Point(112, 0);
      this.btnSetAsDefault.Margin = new Padding(0);
      this.btnSetAsDefault.Name = "btnSetAsDefault";
      this.btnSetAsDefault.Size = new Size(88, 22);
      this.btnSetAsDefault.TabIndex = 4;
      this.btnSetAsDefault.Text = "Set as Default";
      this.btnSetAsDefault.UseVisualStyleBackColor = true;
      this.btnSetAsDefault.Click += new EventHandler(this.btnSetAsDefault_Click);
      this.verticalSeparator1.Location = new Point(108, 3);
      this.verticalSeparator1.Margin = new Padding(3, 3, 2, 3);
      this.verticalSeparator1.MaximumSize = new Size(2, 16);
      this.verticalSeparator1.MinimumSize = new Size(2, 16);
      this.verticalSeparator1.Name = "verticalSeparator1";
      this.verticalSeparator1.Size = new Size(2, 16);
      this.verticalSeparator1.TabIndex = 3;
      this.verticalSeparator1.Text = "verticalSeparator1";
      this.sfdExport.DefaultExt = "xml";
      this.sfdExport.Filter = "XML Files|*.xml|All Files|*.*";
      this.ofdImport.DefaultExt = "xml";
      this.ofdImport.Filter = "XML Files|*.xml|All Files|*.*";
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.Controls.Add((Control) this.grpDocuments);
      this.Controls.Add((Control) this.collapsibleSplitter1);
      this.Controls.Add((Control) this.grpStackingOrders);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (StackingOrderMgmtControl);
      this.Size = new Size(735, 509);
      ((ISupportInitialize) this.btnResetStackingOrder).EndInit();
      ((ISupportInitialize) this.btnSaveStackingOrder).EndInit();
      ((ISupportInitialize) this.btnRemoveDocument).EndInit();
      ((ISupportInitialize) this.btnMoveDocumentDown).EndInit();
      ((ISupportInitialize) this.btnMoveDocumentUp).EndInit();
      ((ISupportInitialize) this.btnAddDocuments).EndInit();
      ((ISupportInitialize) this.btnDeleteStackingOrder).EndInit();
      ((ISupportInitialize) this.btnDuplicateStackingOrder).EndInit();
      ((ISupportInitialize) this.btnAddStackingOrder).EndInit();
      ((ISupportInitialize) this.btnExportStackingOrder).EndInit();
      ((ISupportInitialize) this.btnImportStackingOrder).EndInit();
      this.grpDocuments.ResumeLayout(false);
      this.grpDocuments.PerformLayout();
      this.pnlDocumentControls.ResumeLayout(false);
      this.grpStackingOrders.ResumeLayout(false);
      this.grpStackingOrders.PerformLayout();
      this.pnlStackingOrderControls.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
