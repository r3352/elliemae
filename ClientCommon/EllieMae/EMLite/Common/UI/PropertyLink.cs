// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.PropertyLink
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.Common.Properties;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Web;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class PropertyLink : PipelineImageLink
  {
    private EllieMae.EMLite.ClientServer.Address propertyAddress;

    public PropertyLink(Control parentControl, PipelineElementData data)
      : base(parentControl, data, (Element) new TextElement(string.Concat(data.GetValue())), (Image) Resources.house, (Image) Resources.house_over)
    {
    }

    public PropertyLink(Control parentControl)
      : base(parentControl, (Image) Resources.house, (Image) Resources.house_over)
    {
    }

    protected override void OnLinkClicked(object sender, EventArgs e)
    {
      Point position = Cursor.Position;
      LoanData loanData = this.GetLoanData();
      if (loanData == null)
        return;
      this.propertyAddress = PropertyLink.createPropertyAddress(loanData);
      this.createContextMenu(loanData)?.Show(position);
    }

    private static EllieMae.EMLite.ClientServer.Address createPropertyAddress(LoanData loanData)
    {
      return new EllieMae.EMLite.ClientServer.Address()
      {
        Street1 = loanData.GetField("11"),
        City = loanData.GetField("12"),
        State = loanData.GetField("14"),
        Zip = loanData.GetField("15")
      };
    }

    private ContextMenuStrip createContextMenu(LoanData loanData)
    {
      ContextMenuStrip contextMenu = new ContextMenuStrip();
      contextMenu.ShowImageMargin = false;
      ObjectWithImage dataSource = new ObjectWithImage(loanData.GetField("11"), (Image) Resources.house_icon_menu);
      contextMenu.Items.Add((ToolStripItem) new ToolStripMenuItemEx((object) dataSource, ToolStripMenuItemEx.ToolStripItemType.Header));
      Decimal num1 = Utils.ParseDecimal((object) loanData.GetSimpleField("136"));
      Decimal num2 = Utils.ParseDecimal((object) loanData.GetSimpleField("356"));
      if (num1 > 0M)
        contextMenu.Items.Add((ToolStripItem) new ToolStripMenuItemEx((object) ("Purchase Price: $" + num1.ToString("#,##0")), ToolStripMenuItemEx.ToolStripItemType.Label));
      if (num2 > 0M)
        contextMenu.Items.Add((ToolStripItem) new ToolStripMenuItemEx((object) ("Appraised Value: $" + num2.ToString("#,##0")), ToolStripMenuItemEx.ToolStripItemType.Label));
      if (contextMenu.Items.Count > 1)
        contextMenu.Items.Add((ToolStripItem) new ToolStripSeparatorEx());
      contextMenu.Items.Add((ToolStripItem) this.createActionMenuItem("View Map", new EventHandler(this.onViewMap)));
      DocumentLog appraisalDocumentLog = this.getAppraisalDocumentLog(loanData);
      if (appraisalDocumentLog == null)
        contextMenu.Items.Add((ToolStripItem) this.createActionMenuItem("Order Appraisal", new EventHandler(this.onOrderAppraisal)));
      else if ((appraisalDocumentLog.Guid ?? "") != "")
        contextMenu.Items.Add((ToolStripItem) this.createActionMenuItem("View Appraisal", new EventHandler(this.onViewAppraisal)));
      DocumentLog reportDocumentLog = this.getTitleReportDocumentLog(loanData);
      if (reportDocumentLog == null)
        contextMenu.Items.Add((ToolStripItem) this.createActionMenuItem("Order Title", new EventHandler(this.onOrderTitle)));
      else if ((reportDocumentLog.Guid ?? "") != "")
        contextMenu.Items.Add((ToolStripItem) this.createActionMenuItem("View Title", new EventHandler(this.onViewTitle)));
      return contextMenu;
    }

    private DocumentLog getAppraisalDocumentLog(LoanData loanData)
    {
      foreach (DocumentLog appraisalDocumentLog in loanData.GetLogList().GetDocumentsByTitle("Appraisal"))
      {
        if (appraisalDocumentLog.IsePASS)
          return appraisalDocumentLog;
      }
      return (DocumentLog) null;
    }

    private DocumentLog getTitleReportDocumentLog(LoanData loanData)
    {
      foreach (DocumentLog reportDocumentLog in loanData.GetLogList().GetDocumentsByTitle("Title Report"))
      {
        if (reportDocumentLog.IsePASS)
          return reportDocumentLog;
      }
      return (DocumentLog) null;
    }

    private ToolStripMenuItem createActionMenuItem(string text, EventHandler clickHandler)
    {
      ToolStripMenuItemEx actionMenuItem = new ToolStripMenuItemEx((object) text, ToolStripMenuItemEx.ToolStripItemType.Clickable);
      actionMenuItem.Click += clickHandler;
      return (ToolStripMenuItem) actionMenuItem;
    }

    private void onOrderTitle(object sender, EventArgs e)
    {
      if (!this.OpenLoan())
        return;
      Session.Application.GetService<IEPass>().ProcessURL("_EPASS_SIGNATURE;EPASSAI;2;Title+%26+Closing");
    }

    private void onViewTitle(object sender, EventArgs e)
    {
      if (!this.OpenLoan())
        return;
      DocumentLog reportDocumentLog = this.getTitleReportDocumentLog(Session.LoanData);
      if (reportDocumentLog == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) this.ParentControl, "The Title Report for this loan cannot be found or is inaccessible.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
        Session.Application.GetService<IEFolder>().View(Session.LoanDataMgr, reportDocumentLog);
    }

    private void onOrderAppraisal(object sender, EventArgs e)
    {
      if (!this.OpenLoan())
        return;
      Session.Application.GetService<IEPass>().ProcessURL("_EPASS_SIGNATURE;EPASSAI;2;Appraisal");
    }

    private void onViewAppraisal(object sender, EventArgs e)
    {
      if (!this.OpenLoan())
        return;
      DocumentLog appraisalDocumentLog = this.getAppraisalDocumentLog(Session.LoanData);
      if (appraisalDocumentLog == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) this.ParentControl, "The Appraisal for this loan cannot be found or is inaccessible.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
        Session.Application.GetService<IEFolder>().View(Session.LoanDataMgr, appraisalDocumentLog);
    }

    private void onViewMap(object sender, EventArgs e)
    {
      Process.Start("http://maps.google.com/maps?q=" + HttpUtility.UrlEncode(this.propertyAddress.Street1 + ", " + this.propertyAddress.City + ", " + this.propertyAddress.State + ", " + this.propertyAddress.Zip));
    }
  }
}
