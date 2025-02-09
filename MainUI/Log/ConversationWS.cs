// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.ConversationWS
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.Log
{
  public class ConversationWS : UserControl, IOnlineHelpTarget, IRefreshContents
  {
    private const string className = "ConversationWS";
    protected static string sw = Tracing.SwOutsideLoan;
    public const int DateColumnIndex = 0;
    public const int UserIdColumnIndex = 1;
    public const int NameColumnIndex = 2;
    public const int CompanyColumnIndex = 3;
    public const int FollowUpColumnIndex = 4;
    private LoanData loan;
    private LogList logList;
    private GVItem selectedItem;
    private GroupContainer groupContainerTop;
    private StandardIconButton btnDelete;
    private StandardIconButton btnNew;
    private Panel pnlConversationDialog;
    private ToolTip toolTip1;
    private GridView gridViewConv;
    private int selectedIndex = -1;
    private IContainer components;
    private Panel pnlListView;

    private int selected
    {
      get => this.selectedIndex;
      set
      {
        if (value < 0)
          return;
        if (this.selectedItem != null && this.gridViewConv.Items.Contains(this.selectedItem))
          this.selectedItem.BackColor = Color.White;
        this.selectedIndex = value;
        this.selectedItem = this.gridViewConv.Items[this.selectedIndex];
        this.selectedItem.BackColor = Color.LightGray;
        this.btnDelete.Enabled = ((LogRecordBase) this.selectedItem.Tag).IsNew;
      }
    }

    public ConversationWS(LoanData loan)
    {
      this.loan = loan;
      this.InitializeComponent();
      this.Dock = DockStyle.Fill;
      this.logList = loan.GetLogList();
      this.refreshListView();
      if ((this.loan.ContentAccess & LoanContentAccess.ConversationLog) == LoanContentAccess.ConversationLog || this.loan.ContentAccess == LoanContentAccess.FullAccess)
        return;
      this.btnNew.Visible = false;
      this.btnDelete.Visible = false;
    }

    public void RefreshContents() => this.refreshListView();

    public void RefreshLoanContents()
    {
      this.logList = this.loan.GetLogList();
      this.RefreshContents();
    }

    public string GetHelpTargetName() => nameof (ConversationWS);

    internal void SelectDialog(ConversationLog convLog)
    {
      this.clearConversationDialog();
      int nItemIndex1 = -1;
      for (int nItemIndex2 = 0; nItemIndex2 < this.gridViewConv.Items.Count; ++nItemIndex2)
      {
        if (convLog.Equals(this.gridViewConv.Items[nItemIndex2].Tag))
        {
          nItemIndex1 = nItemIndex2;
          break;
        }
      }
      if (-1 == nItemIndex1)
      {
        Tracing.Log(ConversationWS.sw, nameof (ConversationWS), TraceLevel.Error, "Conversation log not found in the conversation list: " + (object) convLog);
        ConversationDialog conversationDialog = new ConversationDialog((ConversationLog) null, (GVItem) null);
        conversationDialog.LogLockEvent += new LogLockEventHandler(this.dlgConvLog_LogLockEvent);
        this.pnlConversationDialog.Controls.Add((Control) conversationDialog);
      }
      else
      {
        this.selected = nItemIndex1;
        ConversationDialog conversationDialog = new ConversationDialog(convLog, this.gridViewConv.Items[nItemIndex1]);
        conversationDialog.LogLockEvent += new LogLockEventHandler(this.dlgConvLog_LogLockEvent);
        this.pnlConversationDialog.Controls.Add((Control) conversationDialog);
      }
    }

    private void refreshListView()
    {
      ConversationLog[] allConversations = this.logList.GetAllConversations();
      this.gridViewConv.BeginUpdate();
      this.gridViewConv.Items.Clear();
      foreach (ConversationLog conversationLog in allConversations)
        this.gridViewConv.Items.Add(new GVItem(conversationLog.Date.ToString("MM/dd/yy"))
        {
          SubItems = {
            (object) conversationLog.UserId,
            (object) conversationLog.Name,
            (object) conversationLog.Company,
            conversationLog.AlertList.IsFollowUpRequired() ? (object) "Yes" : (object) "No"
          },
          Tag = (object) conversationLog
        });
      this.gridViewConv.EndUpdate();
      if (this.gridViewConv.Items.Count > 0)
      {
        this.SelectDialog((ConversationLog) this.gridViewConv.Items[0].Tag);
      }
      else
      {
        this.SelectDialog((ConversationLog) null);
        if (this.gridViewConv.Items.Count != 0)
          return;
        this.btnDelete.Enabled = false;
      }
    }

    private void clearConversationDialog()
    {
      ArrayList arrayList = new ArrayList();
      foreach (Control control in (ArrangedElementCollection) this.pnlConversationDialog.Controls)
        arrayList.Add((object) control);
      this.pnlConversationDialog.Controls.Clear();
      foreach (Component component in arrayList)
        component.Dispose();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        this.clearConversationDialog();
        if (this.components != null)
          this.components.Dispose();
      }
      base.Dispose(disposing);
    }

    public void newBtn_Click(object sender, EventArgs e)
    {
      this.gridViewConv.SelectedItems.Clear();
      ConversationLog conversationLog = new ConversationLog(DateTime.Now, Session.UserInfo.Userid);
      this.logList.AddRecord((LogRecordBase) conversationLog);
      GVItem listViewItem = new GVItem(conversationLog.Date.ToString("MM/dd/yy"));
      listViewItem.SubItems.Add((object) conversationLog.UserId);
      listViewItem.SubItems.Add((object) conversationLog.Name);
      listViewItem.SubItems.Add((object) conversationLog.Company);
      listViewItem.SubItems.Add(conversationLog.AlertList.IsFollowUpRequired() ? (object) "Yes" : (object) "No");
      listViewItem.Tag = (object) conversationLog;
      listViewItem.Selected = true;
      this.gridViewConv.Items.Add(listViewItem);
      this.selected = listViewItem.Index;
      this.clearConversationDialog();
      ConversationDialog conversationDialog = new ConversationDialog(conversationLog, listViewItem);
      conversationDialog.LogLockEvent += new LogLockEventHandler(this.dlgConvLog_LogLockEvent);
      this.pnlConversationDialog.Controls.Add((Control) conversationDialog);
      if (conversationLog.DisplayInLog)
        Session.Application.GetService<ILoanEditor>().RefreshLogPanel();
      this.loan.Dirty = true;
    }

    private void deleteBtn_Click(object sender, EventArgs e)
    {
      if (this.selected >= 0)
      {
        if (DialogResult.Yes != Utils.Dialog((IWin32Window) this, "We strongly recommend that you not delete Conversation Log records. Are you sure you want to delete the selected item?", MessageBoxButtons.YesNo, MessageBoxIcon.Hand))
          return;
        try
        {
          this.logList.RemoveRecord((LogRecordBase) this.gridViewConv.Items[this.selected].Tag);
          this.gridViewConv.Items.Remove(this.gridViewConv.Items[this.selected]);
          this.refreshListView();
          Session.Application.GetService<ILoanEditor>().RefreshLogPanel();
        }
        catch (Exception ex)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "This conversation log cannot be removed. Error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
      }
      else
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You must select a record first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void editBtn_Click(object sender, EventArgs e)
    {
      if (this.gridViewConv.SelectedItems.Count == 0)
        return;
      ConversationLog tag = (ConversationLog) this.gridViewConv.SelectedItems[0].Tag;
      if (tag == null)
        return;
      this.SelectDialog(tag);
    }

    private void closeBtn_Click(object sender, EventArgs e)
    {
      Session.Application.GetService<ILoanEditor>().RemoveFromWorkArea();
    }

    private void dlgConvLog_LogLockEvent(object sender, LogLockEventArgs e)
    {
      this.btnDelete.Enabled = !e.IsLogLocked;
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      GVColumn gvColumn4 = new GVColumn();
      GVColumn gvColumn5 = new GVColumn();
      this.pnlListView = new Panel();
      this.gridViewConv = new GridView();
      this.groupContainerTop = new GroupContainer();
      this.btnDelete = new StandardIconButton();
      this.btnNew = new StandardIconButton();
      this.pnlConversationDialog = new Panel();
      this.toolTip1 = new ToolTip(this.components);
      this.pnlListView.SuspendLayout();
      this.groupContainerTop.SuspendLayout();
      ((ISupportInitialize) this.btnDelete).BeginInit();
      ((ISupportInitialize) this.btnNew).BeginInit();
      this.SuspendLayout();
      this.pnlListView.BackColor = Color.Ivory;
      this.pnlListView.Controls.Add((Control) this.gridViewConv);
      this.pnlListView.Dock = DockStyle.Fill;
      this.pnlListView.Location = new Point(1, 26);
      this.pnlListView.Name = "pnlListView";
      this.pnlListView.Size = new Size(639, 94);
      this.pnlListView.TabIndex = 1;
      this.gridViewConv.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.SortMethod = GVSortMethod.DateTime;
      gvColumn1.Text = "Date";
      gvColumn1.Width = 82;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "User ID";
      gvColumn2.Width = 86;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Text = "Name";
      gvColumn3.Width = 152;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column4";
      gvColumn4.Text = "Company";
      gvColumn4.Width = 184;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column5";
      gvColumn5.Text = "Follow Up Needed";
      gvColumn5.Width = 107;
      this.gridViewConv.Columns.AddRange(new GVColumn[5]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5
      });
      this.gridViewConv.Dock = DockStyle.Fill;
      this.gridViewConv.Location = new Point(0, 0);
      this.gridViewConv.Name = "gridViewConv";
      this.gridViewConv.Size = new Size(639, 94);
      this.gridViewConv.TabIndex = 1;
      this.gridViewConv.Click += new EventHandler(this.editBtn_Click);
      this.gridViewConv.DoubleClick += new EventHandler(this.editBtn_Click);
      this.groupContainerTop.Controls.Add((Control) this.btnDelete);
      this.groupContainerTop.Controls.Add((Control) this.btnNew);
      this.groupContainerTop.Controls.Add((Control) this.pnlListView);
      this.groupContainerTop.Dock = DockStyle.Top;
      this.groupContainerTop.HeaderForeColor = SystemColors.ControlText;
      this.groupContainerTop.Location = new Point(0, 0);
      this.groupContainerTop.Name = "groupContainerTop";
      this.groupContainerTop.Size = new Size(641, 121);
      this.groupContainerTop.TabIndex = 3;
      this.groupContainerTop.Text = "Conversation Log";
      this.btnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDelete.BackColor = Color.Transparent;
      this.btnDelete.Location = new Point(619, 4);
      this.btnDelete.MouseDownImage = (Image) null;
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new Size(16, 16);
      this.btnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnDelete.TabIndex = 3;
      this.btnDelete.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnDelete, "Delete Conversation Log");
      this.btnDelete.Click += new EventHandler(this.deleteBtn_Click);
      this.btnNew.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnNew.BackColor = Color.Transparent;
      this.btnNew.Location = new Point(597, 4);
      this.btnNew.MouseDownImage = (Image) null;
      this.btnNew.Name = "btnNew";
      this.btnNew.Size = new Size(16, 16);
      this.btnNew.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnNew.TabIndex = 2;
      this.btnNew.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnNew, "New Conversation Log");
      this.btnNew.Click += new EventHandler(this.newBtn_Click);
      this.pnlConversationDialog.Dock = DockStyle.Fill;
      this.pnlConversationDialog.Location = new Point(0, 121);
      this.pnlConversationDialog.Name = "pnlConversationDialog";
      this.pnlConversationDialog.Size = new Size(641, 371);
      this.pnlConversationDialog.TabIndex = 4;
      this.Controls.Add((Control) this.pnlConversationDialog);
      this.Controls.Add((Control) this.groupContainerTop);
      this.Name = nameof (ConversationWS);
      this.Size = new Size(641, 492);
      this.pnlListView.ResumeLayout(false);
      this.groupContainerTop.ResumeLayout(false);
      ((ISupportInitialize) this.btnDelete).EndInit();
      ((ISupportInitialize) this.btnNew).EndInit();
      this.ResumeLayout(false);
    }
  }
}
