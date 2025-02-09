// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.LinkedGUIDLink
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common.Properties;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class LinkedGUIDLink : PipelineImageLink
  {
    private Control parentControl;
    private PipelineInfo pinfo;
    private PipelineElementData data;
    private LoanData loanData;
    private LoanDataMgr loanDataMgr;

    public LinkedGUIDLink(Control parentControl, PipelineInfo pinfo, PipelineElementData data)
      : base(parentControl, (Image) Resources.linked_loan, (Image) Resources.linked_loan_hover)
    {
      this.pinfo = pinfo;
      this.data = data;
      this.parentControl = parentControl;
    }

    public LinkedGUIDLink(Control parentControl, LoanData loanData, LoanDataMgr loanDataMgr)
      : base(parentControl, (Image) Resources.linked_loan, (Image) Resources.linked_loan_hover)
    {
      this.loanData = loanData;
      this.loanDataMgr = loanDataMgr;
    }

    protected override void OnLinkClicked(object sender, EventArgs e)
    {
      if (this.loanData != null)
      {
        Point position = Cursor.Position;
        this.createContextMenu()?.Show(position);
      }
      else
      {
        string linkedLoanNumber = this.getLinkedLoanNumber();
        if (linkedLoanNumber == null)
          return;
        using (LinkedLoanDialog linkedLoanDialog = new LinkedLoanDialog(this.pinfo.LoanNumber, linkedLoanNumber))
        {
          if (linkedLoanDialog.ShowDialog((IWin32Window) this.parentControl) != DialogResult.OK)
            return;
          Session.Application.GetService<ILoanConsole>().OpenLoan(this.pinfo.LinkGuid);
        }
      }
    }

    private ContextMenuStrip createContextMenu()
    {
      ContextMenuStrip contextMenu = new ContextMenuStrip();
      contextMenu.ShowImageMargin = false;
      if (this.loanDataMgr == null || this.loanDataMgr.GetFieldAccessRights("Button_MakeToCurrent") == BizRule.FieldAccessRight.DoesNotApply || this.loanDataMgr.GetFieldAccessRights("Button_MakeToCurrent") == BizRule.FieldAccessRight.Edit)
        contextMenu.Items.Add((ToolStripItem) this.createActionMenuItem("Go to Linked Loan", new EventHandler(this.onOpenLinkedLoan), true));
      else if (this.loanDataMgr.GetFieldAccessRights("Button_MakeToCurrent") == BizRule.FieldAccessRight.ViewOnly)
        contextMenu.Items.Add((ToolStripItem) this.createActionMenuItem("Go to Linked Loan", new EventHandler(this.onOpenLinkedLoan), false));
      if (this.loanDataMgr == null || this.loanDataMgr.GetFieldAccessRights("Button_SyncData") == BizRule.FieldAccessRight.DoesNotApply || this.loanDataMgr.GetFieldAccessRights("Button_SyncData") == BizRule.FieldAccessRight.Edit)
        contextMenu.Items.Add((ToolStripItem) this.createActionMenuItem("Sync Data", new EventHandler(this.onSyncData), true));
      else if (this.loanDataMgr.GetFieldAccessRights("Button_SyncData") == BizRule.FieldAccessRight.ViewOnly)
        contextMenu.Items.Add((ToolStripItem) this.createActionMenuItem("Sync Data", new EventHandler(this.onSyncData), false));
      return contextMenu;
    }

    private void onOpenLinkedLoan(object sender, EventArgs e)
    {
      if (this.loanData.LinkedData == null)
        return;
      Session.Application.GetService<ILoanConsole>().OpenLoan(this.loanData.LinkedData.GUID);
    }

    private void onSyncData(object sender, EventArgs e)
    {
      bool runPostSyncOnly = false;
      ILoanEditor service = Session.Application.GetService<ILoanEditor>();
      string[] ids;
      if (this.loanData != null && (this.loanData.LinkSyncType == LinkSyncType.ConstructionPrimary || this.loanData.LinkSyncType == LinkSyncType.ConstructionLinked))
      {
        ids = service.SelectLinkAndSyncTemplate();
        if (ids == null)
          return;
      }
      else
        ids = Session.LoanDataMgr.SystemConfiguration.PiggybackSyncFields.GetSyncFields();
      if (ids == null || ids.Length == 0)
      {
        runPostSyncOnly = true;
        if (this.loanData != null && (this.loanData.LinkSyncType == LinkSyncType.ConstructionPrimary || this.loanData.LinkSyncType == LinkSyncType.ConstructionLinked))
        {
          int num = (int) Utils.Dialog((IWin32Window) Session.MainForm, "Synchronization Field List is empty. Both loans won't be synchronized.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return;
        }
        int num1 = (int) Utils.Dialog((IWin32Window) Session.MainForm, "Synchronization Field List is empty. Only some default fields will be synchronized.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      if (!runPostSyncOnly && Utils.Dialog((IWin32Window) Session.MainForm, "Are you sure you want to synchronize data between two loans?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        runPostSyncOnly = true;
      if (((this.loanData == null ? 0 : (this.loanData.LinkSyncType == LinkSyncType.ConstructionPrimary ? 1 : (this.loanData.LinkSyncType == LinkSyncType.ConstructionLinked ? 1 : 0))) & (runPostSyncOnly ? 1 : 0)) != 0)
        return;
      Cursor.Current = Cursors.WaitCursor;
      ArrayList arrayList = this.loanData.SyncPiggyBackFiles(ids, runPostSyncOnly, true, (string) null, (string) null, false);
      Cursor.Current = Cursors.Default;
      service.RefreshContents();
      if (runPostSyncOnly)
        return;
      if (arrayList.Count == 0)
      {
        int num2 = (int) Utils.Dialog((IWin32Window) null, "Both loans have been synchronized.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        string str = "";
        for (int index = 0; index < arrayList.Count; ++index)
          str = !(str == "") ? str + ", " + (string) arrayList[index] : (string) arrayList[index];
        int num3 = (int) Utils.Dialog((IWin32Window) null, "The following fields cannot be synchronized because those fields are locked fields:\r\n\r\n" + str, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
    }

    private ToolStripMenuItem createActionMenuItem(
      string text,
      EventHandler clickHandler,
      bool enabled)
    {
      ToolStripMenuItemEx actionMenuItem = new ToolStripMenuItemEx((object) text, ToolStripMenuItemEx.ToolStripItemType.Clickable);
      actionMenuItem.Click += clickHandler;
      if (!enabled)
      {
        actionMenuItem.Enabled = false;
        actionMenuItem.ItemType = ToolStripMenuItemEx.ToolStripItemType.Label;
      }
      return (ToolStripMenuItem) actionMenuItem;
    }

    private string getLinkedLoanNumber()
    {
      using (CursorActivator.Wait())
      {
        using (ILoan loan = Session.LoanManager.OpenLoan(this.data.PipelineInfo.LinkGuid))
        {
          string text = "The linked loan has been deleted or is no longer accessible!";
          if (loan == null)
          {
            int num = (int) Utils.Dialog((IWin32Window) this.parentControl, text, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return (string) null;
          }
          PipelineInfo pipelineInfo = loan.GetPipelineInfo(false);
          if (pipelineInfo != null)
            return pipelineInfo.LoanNumber;
          int num1 = (int) Utils.Dialog((IWin32Window) this.parentControl, text, MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return (string) null;
        }
      }
    }
  }
}
