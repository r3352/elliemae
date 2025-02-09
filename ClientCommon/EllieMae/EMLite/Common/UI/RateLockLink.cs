// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.RateLockLink
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common.ProductPricing;
using EllieMae.EMLite.Common.Properties;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class RateLockLink : PipelineElement, IMouseListener
  {
    private const int imageSpacing = 5;
    private static int alertDaysBefore = -1;
    private ImageLink imageLink;
    private string lockReqStatus;
    private int days = -1;

    static RateLockLink()
    {
      foreach (AlertConfig alertConfig in Session.StartupInfo.AlertConfigs)
      {
        if (alertConfig.AlertID == 10 && alertConfig.AlertEnabled)
          RateLockLink.alertDaysBefore = alertConfig.DaysBefore;
      }
    }

    public RateLockLink(Control parentControl, PipelineElementData data)
      : base(parentControl, data)
    {
      this.lockReqStatus = string.Concat(data.PipelineInfo.GetField("Loan.LockAndRequestStatus"));
      this.imageLink = RateLockLink.GetLockImageForStatus(this.lockReqStatus);
      this.imageLink.Click += new EventHandler(this.imageLink_Click);
      if (!(this.lockReqStatus == "Locked-NoRequest") && !(this.lockReqStatus == "Locked-Request"))
        return;
      object field = data.PipelineInfo.GetField("Loan.LockExpirationDate");
      if (object.Equals(data.PipelineInfo.GetField("Loan.loanChannel"), (object) "4") && Utils.ParseBoolean(data.PipelineInfo.GetField("Loan.MostRecentCommitmentEnabled")) && Utils.ParseDate(data.PipelineInfo.GetField("Loan.CommitmentExpirationDate"), false, DateTime.MinValue) != DateTime.MinValue)
        field = data.PipelineInfo.GetField("Loan.CommitmentExpirationDate");
      if (field == null)
        return;
      this.days = (int) (Convert.ToDateTime(field) - DateTime.Today).TotalDays;
    }

    public RateLockLink(Control parentControl)
      : base(parentControl, (PipelineElementData) null)
    {
      this.lockReqStatus = Session.LoanData.GetField("LOCKRATE.RATEREQUESTSTATUS");
      this.imageLink = RateLockLink.GetLockImageForStatus(this.lockReqStatus);
      this.imageLink.Click += new EventHandler(this.imageLink_Click);
      if (!(this.lockReqStatus == "Locked-NoRequest") || !(this.lockReqStatus != "Locked-Request"))
        return;
      LockConfirmLog confirmForCurrentLock = Session.LoanData.GetLogList().GetMostRecentConfirmForCurrentLock();
      bool flag = false;
      if (confirmForCurrentLock != null)
        flag = confirmForCurrentLock.CommitmentTermEnabled;
      DateTime date = Utils.ParseDate((object) Session.LoanData.GetField("762"));
      if (string.Equals(Session.LoanData.GetField("2626"), "Correspondent") & flag)
        date = Utils.ParseDate((object) Session.LoanData.GetField("4529"));
      if (!(date != DateTime.MinValue))
        return;
      this.days = (int) (date.Date - DateTime.Today).TotalDays;
    }

    public override string ToString() => this.GetRateLockDescription();

    public void DisplayContextMenu()
    {
      Point position = Cursor.Position;
      this.createContextMenu(this.GetLoanData())?.Show(position);
    }

    private ContextMenuStrip createContextMenu(LoanData loanData)
    {
      ContextMenuStrip contextMenu = new ContextMenuStrip();
      contextMenu.ShowImageMargin = false;
      IDictionary serverSettings = Session.ServerManager.GetServerSettings("Policies");
      ObjectWithImage dataSource = new ObjectWithImage(this.GetRateLockDescription(), this.imageLink.NormalImage);
      contextMenu.Items.Add((ToolStripItem) new ToolStripMenuItemEx((object) dataSource, ToolStripMenuItemEx.ToolStripItemType.Header));
      bool flag1 = Session.IsBankerEdition();
      bool flag2 = loanData.GetField("2400") == "Y";
      bool flag3 = loanData.GetField("4532") == "Y";
      bool flag4 = loanData.GetField("LOCKRATE.REQUESTED") == "Y";
      bool flag5 = string.Equals(loanData.GetField("2626"), "correspondent", StringComparison.InvariantCultureIgnoreCase) && !string.IsNullOrEmpty(loanData.GetField("TPO.X14"));
      bool flag6 = (bool) serverSettings[(object) "Policies.EnableCommitmentTermFields"];
      LockConfirmLog confirmForCurrentLock = loanData.GetLogList().GetMostRecentConfirmForCurrentLock();
      bool flag7 = false;
      if (confirmForCurrentLock != null)
        flag7 = confirmForCurrentLock.CommitmentTermEnabled;
      if (flag5 && loanData.GetField("4532") == "Y")
      {
        if (flag7)
        {
          contextMenu.Items.Add((ToolStripItem) new ToolStripMenuItemEx((object) ("Commitment Date: " + loanData.GetField("4527")), ToolStripMenuItemEx.ToolStripItemType.Label));
          contextMenu.Items.Add((ToolStripItem) new ToolStripMenuItemEx((object) ("Commitment Expiration Date: " + loanData.GetField("4529")), ToolStripMenuItemEx.ToolStripItemType.Label));
        }
        else
        {
          contextMenu.Items.Add((ToolStripItem) new ToolStripMenuItemEx((object) ("Lock Date: " + loanData.GetField("761")), ToolStripMenuItemEx.ToolStripItemType.Label));
          contextMenu.Items.Add((ToolStripItem) new ToolStripMenuItemEx((object) ("Lock Expiration Date: " + loanData.GetField("762")), ToolStripMenuItemEx.ToolStripItemType.Label));
        }
      }
      else if (flag2)
      {
        if (flag7)
        {
          contextMenu.Items.Add((ToolStripItem) new ToolStripMenuItemEx((object) ("Commitment Date: " + loanData.GetField("4527")), ToolStripMenuItemEx.ToolStripItemType.Label));
          contextMenu.Items.Add((ToolStripItem) new ToolStripMenuItemEx((object) ("Commitment Expiration Date: " + loanData.GetField("4529")), ToolStripMenuItemEx.ToolStripItemType.Label));
        }
        else
        {
          contextMenu.Items.Add((ToolStripItem) new ToolStripMenuItemEx((object) ("Lock Date: " + loanData.GetField("761")), ToolStripMenuItemEx.ToolStripItemType.Label));
          contextMenu.Items.Add((ToolStripItem) new ToolStripMenuItemEx((object) ("Lock Expiration Date: " + loanData.GetField("762")), ToolStripMenuItemEx.ToolStripItemType.Label));
        }
        contextMenu.Items.Add((ToolStripItem) new ToolStripMenuItemEx((object) ("Expected Closing Date: " + loanData.GetField("763")), ToolStripMenuItemEx.ToolStripItemType.Label));
      }
      else if (flag4)
      {
        LockRequestLog currentLockRequest = loanData.GetLogList().GetCurrentLockRequest();
        if (currentLockRequest != null)
        {
          contextMenu.Items.Add((ToolStripItem) new ToolStripMenuItemEx((object) ("Request Date and Time: " + currentLockRequest.Date.ToString("MM/dd/yy") + " " + currentLockRequest.TimeRequested), ToolStripMenuItemEx.ToolStripItemType.Label));
          contextMenu.Items.Add((ToolStripItem) new ToolStripMenuItemEx((object) ("Requested By: " + currentLockRequest.RequestedFullName), ToolStripMenuItemEx.ToolStripItemType.Label));
          contextMenu.Items.Add((ToolStripItem) new ToolStripMenuItemEx((object) ("Expected Closing Date: " + loanData.GetField("763")), ToolStripMenuItemEx.ToolStripItemType.Label));
        }
      }
      if (contextMenu.Items.Count > 1)
        contextMenu.Items.Add((ToolStripItem) new ToolStripSeparatorEx());
      contextMenu.Items.Add((ToolStripItem) this.createActionMenuItem("Search for Product and Pricing", new EventHandler(this.onSearchForProductAndPricing)));
      contextMenu.Items.Add((ToolStripItem) this.createActionMenuItem("Submit to Lender", new EventHandler(this.onSubmitToLender)));
      if (flag1)
      {
        if (Session.ACL.IsAuthorizedForFeature(AclFeature.ToolsTab_LockRequestForm))
          contextMenu.Items.Add((ToolStripItem) this.createActionMenuItem("Lock Request Form", new EventHandler(this.onLockRequestForm)));
        if (Session.ACL.IsAuthorizedForFeature(AclFeature.ToolsTab_LockComparisonTool))
          contextMenu.Items.Add((ToolStripItem) this.createActionMenuItem("Lock Comparison Tool", new EventHandler(this.onLockComparisonTool)));
        if (loanData.GetLogList().GetCurrentLockConfirmation() != null)
          contextMenu.Items.Add((ToolStripItem) this.createActionMenuItem("Lock Confirmation", new EventHandler(this.onLockConfirmation)));
        if (Session.ACL.IsAuthorizedForFeature(AclFeature.ToolsTab_SecondaryRegistration))
        {
          contextMenu.Items.Add((ToolStripItem) this.createActionMenuItem("Secondary Registration", new EventHandler(this.onSecondaryRegistration)));
          if (Session.SessionObjects.StartupInfo.ProductPricingPartner != null && Session.SessionObjects.StartupInfo.ProductPricingPartner.IsEPPS && ProductPricingUtils.IsHistoricalPricingEnabled)
            contextMenu.Items.Add((ToolStripItem) this.createActionMenuItem("Worst Case Pricing", new EventHandler(this.onWorstCasePricing)));
        }
      }
      return contextMenu;
    }

    private ToolStripMenuItem createActionMenuItem(string text, EventHandler clickHandler)
    {
      ToolStripMenuItemEx actionMenuItem = new ToolStripMenuItemEx((object) text, ToolStripMenuItemEx.ToolStripItemType.Clickable);
      actionMenuItem.Click += clickHandler;
      return (ToolStripMenuItem) actionMenuItem;
    }

    private void onSearchForProductAndPricing(object sender, EventArgs e)
    {
      if (!this.OpenLoan())
        return;
      Session.LoanData.SetField("OPTIMAL.REQUEST", "");
      Session.LoanData.SetField("OPTIMAL.RESPONSE", "");
      if (Session.StartupInfo.ProductPricingPartner != null && Session.StartupInfo.ProductPricingPartner.IsEPPS && LoanLockUtils.GetAllowedRequestType(Session.SessionObjects, Session.LoanDataMgr) == LoanLockUtils.AllowedRequestType.ReLockOnly && !LoanLockUtils.IsAllowGetPricingForReLock(Session.SessionObjects, Session.LoanDataMgr))
      {
        int num = (int) Utils.Dialog((IWin32Window) this.ParentControl, "The pricing request violates your administrative settings and cannot be processed.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
        Session.Application.GetService<IEPass>().ProcessURL("_EPASS_SIGNATURE;EPASSAI;2;Product+and+Pricing");
    }

    private void onSubmitToLender(object sender, EventArgs e)
    {
      if (!this.OpenLoan())
        return;
      Session.Application.GetService<IEPass>().ProcessURL("_EPASS_SIGNATURE;EPASSAI;2;Lenders");
    }

    private void onLockRequestForm(object sender, EventArgs e)
    {
      if (!this.OpenLoan())
        return;
      Session.Application.GetService<ILoanEditor>().OpenForm("Lock Request Form");
    }

    private void onLockComparisonTool(object sender, EventArgs e)
    {
      if (!this.OpenLoan())
        return;
      Session.Application.GetService<ILoanEditor>().OpenForm("Lock Comparison Tool");
    }

    private void onLockConfirmation(object sender, EventArgs e)
    {
      if (!this.OpenLoan())
        return;
      LockConfirmLog recentConfirmedLock = this.GetLoanData().GetLogList().GetMostRecentConfirmedLock();
      Session.Application.GetService<ILoanEditor>().OpenLogRecord((LogRecordBase) recentConfirmedLock);
    }

    private void onSecondaryRegistration(object sender, EventArgs e)
    {
      if (!this.OpenLoan())
        return;
      Session.Application.GetService<ILoanEditor>().OpenForm("Secondary Registration");
    }

    public string GetRateLockDescription()
    {
      switch (this.lockReqStatus)
      {
        case "Cancelled":
          return "Lock Cancelled";
        case "Expired-Extension-Request":
          return "Expired! Extension requested";
        case "Expired-NoRequest":
        case "Expired-Request":
          if (this.days == 1)
            return "Expired 1 day ago";
          return this.days > 0 ? "Expired " + (object) this.days + " days ago" : "Expired";
        case "Locked-Cancellation-Request":
        case "Locked-Extension-Request":
        case "Locked-NoRequest":
        case "Locked-Request":
          string rateLockDescription = this.days != 0 ? (this.days != 1 ? (this.days <= 1 ? "Rate Locked" : "Expires in " + (object) this.days + " days") : "Expires tomorrow") : "Expires today";
          if (this.lockReqStatus == "Locked-Extension-Request")
            rateLockDescription += " ( Lock extension requested )";
          else if (this.lockReqStatus == "Locked-Cancellation-Request")
            rateLockDescription += " ( Lock cancellation requested )";
          return rateLockDescription;
        case "NotLocked-NoRequest":
          return "Not Locked";
        case "NotLocked-Request":
          return "Lock Requested";
        case "Voided":
          return "Lock Voided";
        default:
          return "";
      }
    }

    private void imageLink_Click(object sender, EventArgs e) => this.DisplayContextMenu();

    public override Rectangle Draw(ItemDrawArgs e)
    {
      Rectangle rectangle = this.imageLink.Draw(e);
      if (this.days >= 0)
      {
        Rectangle bounds = ControlDraw.DiffRectangles(e.Bounds, rectangle, 5, HorizontalAlignment.Left);
        e = e.ChangeBounds(bounds);
        if (RateLockLink.alertDaysBefore >= 0 && this.days >= 0 && this.days <= RateLockLink.alertDaysBefore && e.ForeColor == SystemColors.ControlText)
          e = e.ChangeForeColor(EncompassColors.Alert2);
        Rectangle b = ControlDraw.DrawText("(" + (object) this.days + ")", e);
        rectangle = Rectangle.Union(rectangle, b);
      }
      return rectangle;
    }

    public override Size Measure(ItemDrawArgs drawArgs)
    {
      Size size1 = this.imageLink.Measure(drawArgs);
      if (this.days >= 0)
      {
        Size size2 = ControlDraw.Measure((object) ("(" + (object) this.days + ")"), drawArgs);
        size1 = new Size(size1.Width + size2.Width + 5, Math.Max(size1.Height, size2.Height));
      }
      return size1;
    }

    public static ImageLink GetLockImageForStatus(string lockAndReqStatus)
    {
      switch (lockAndReqStatus)
      {
        case "Cancelled":
          return new ImageLink((Element) null, (Image) Resources.rate_lock_cancelled, (Image) Resources.rate_lock_cancelled_over);
        case "Expired-Extension-Request":
          return new ImageLink((Element) null, (Image) Resources.rate_expired__extension_request, (Image) Resources.rate_expired_extension_request_over);
        case "Expired-NoRequest":
          return new ImageLink((Element) null, (Image) Resources.rate_expired, (Image) Resources.rate_expired_over);
        case "Expired-Request":
          return new ImageLink((Element) null, (Image) Resources.rate_expired_request, (Image) Resources.rate_expired_request_over);
        case "Locked-Cancellation-Request":
          return new ImageLink((Element) null, (Image) Resources.rate_lock_cancel_request, (Image) Resources.rate_lock_cancel_request_over);
        case "Locked-Extension-Request":
          return new ImageLink((Element) null, (Image) Resources.rate_locked_extension, (Image) Resources.rate_locked_extension_over);
        case "Locked-NoRequest":
          return new ImageLink((Element) null, (Image) Resources.rate_locked, (Image) Resources.rate_locked_over);
        case "Locked-Request":
          return new ImageLink((Element) null, (Image) Resources.rate_locked_request, (Image) Resources.rate_locked_request_over);
        case "NotLocked-NoRequest":
          return new ImageLink((Element) null, (Image) Resources.rate_unlocked, (Image) Resources.rate_unlocked_over);
        case "NotLocked-Request":
          return new ImageLink((Element) null, (Image) Resources.rate_unlocked_request, (Image) Resources.rate_unlocked_request_over);
        default:
          return new ImageLink((Element) null, (Image) Resources.rate_unlocked, (Image) Resources.rate_unlocked_over);
      }
    }

    private void onWorstCasePricing(object sender, EventArgs e)
    {
      if (!this.OpenLoan())
        return;
      Session.Application.GetService<ILoanEditor>().OpenForm("Worst Case Pricing");
    }

    public bool OnMouseEnter() => this.imageLink.OnMouseEnter();

    public bool OnMouseLeave() => this.imageLink.OnMouseLeave();

    public bool OnMouseMove(Point pt) => this.imageLink.OnMouseMove(pt);

    public bool OnClick(Point pt) => this.imageLink.OnClick(pt);
  }
}
