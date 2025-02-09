// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.LockValidationStatusLink
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common.ProductPricing;
using EllieMae.EMLite.Common.Properties;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.LoanUtils.Services;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class LockValidationStatusLink(Control parentControl, PipelineElementData data) : 
    PipelineImageLink(parentControl, data, LockValidationStatusLink.getDisplayValue(data), LockValidationStatusLink.getLockvalidationStatusImage(data, LockValidationStatusLink.ImageType.NormalImage), LockValidationStatusLink.getLockvalidationStatusImage(data, LockValidationStatusLink.ImageType.HotImage))
  {
    public const string NeedValidation = "needs validation";
    public const string PriceChangeStillQualifies = "price changed, still qualifies";
    public const string LoanNoLongerQualifies = "loan no longer qualifies";
    public const string NoPriceChangeStillQualifies = "no price change, still qualifies";
    private string ProviderId;

    private static Image getLockvalidationStatusImage(
      PipelineElementData pdata,
      LockValidationStatusLink.ImageType imageType)
    {
      string lockValidationStatus = Convert.ToString(pdata.PipelineInfo.GetField("Fields.4788"));
      Image image = (Image) null;
      return !string.IsNullOrEmpty(lockValidationStatus) ? LockValidationStatusLink.getValidationStatusImageAsPerType(imageType, lockValidationStatus, ref image) : (Image) null;
    }

    private static Image getValidationStatusImageAsPerType(
      LockValidationStatusLink.ImageType imageType,
      string lockValidationStatus,
      ref Image image)
    {
      if (imageType == LockValidationStatusLink.ImageType.NormalImage)
      {
        switch (lockValidationStatus.ToLower())
        {
          case "needs validation":
          case "loan no longer qualifies":
            image = (Image) Resources.status_alert;
            break;
          case "price changed, still qualifies":
            image = (Image) Resources.status_warning;
            break;
          case "no price change, still qualifies":
            image = (Image) Resources.status_info;
            break;
          default:
            return (Image) null;
        }
      }
      else
      {
        switch (lockValidationStatus.ToLower())
        {
          case "needs validation":
          case "loan no longer qualifies":
            image = (Image) Resources.status_alert_over;
            break;
          case "price changed, still qualifies":
            image = (Image) Resources.status_warning_over;
            break;
          case "no price change, still qualifies":
            image = (Image) Resources.status_info_over;
            break;
          default:
            return (Image) null;
        }
      }
      return image;
    }

    private static Element getDisplayValue(PipelineElementData pdata)
    {
      string text = (string) null;
      if (string.Compare(pdata.FieldName, "Fields.4788", true) == 0)
        text = (string) pdata.PipelineInfo.Info[(object) "Fields.4788"];
      switch (text)
      {
        case null:
          return (Element) new TextElement(string.Concat(pdata.GetValue()));
        case "":
          return (Element) null;
        default:
          return (Element) new FormattedText(text);
      }
    }

    protected override void OnLinkClicked(object sender, EventArgs e)
    {
      Point position = Cursor.Position;
      if (!Session.ACL.IsAuthorizedForFeature(AclFeature.ToolsTab_LockComparisonTool))
        return;
      this.createContextMenu()?.Show(position);
    }

    private ContextMenuStrip createContextMenu()
    {
      LoanData loanData = this.GetLoanData();
      ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
      contextMenuStrip.ShowImageMargin = false;
      ContextMenuStrip contextMenu = contextMenuStrip;
      contextMenu.Items.Add((ToolStripItem) this.createActionMenuItem("Compare Lock Fields", new EventHandler(this.onCompareLockFields), true));
      this.ProviderId = loanData.GetProviderId();
      if (Session.ACL.IsAuthorizedForFeature(AclFeature.ToolsTab_ValidatePricing) && !string.IsNullOrWhiteSpace(this.ProviderId))
        contextMenu.Items.Add((ToolStripItem) this.createActionMenuItem("Validate Pricing", new EventHandler(this.onValidatePricing), this.IsValidatePricingEnabled(loanData)));
      return contextMenu;
    }

    private bool IsValidatePricingEnabled(LoanData loanData)
    {
      return !new List<string>()
      {
        "NotLocked",
        "Cancelled",
        "Expired",
        "Voided"
      }.Contains<string>(loanData.GetField("LOCKRATE.RATESTATUS"), (IEqualityComparer<string>) StringComparer.InvariantCultureIgnoreCase) && ProductPricingUtils.IsEpc2Provider(this.ProviderId) && !this.IsLoanInBulkTrade(loanData);
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
        actionMenuItem.ItemType = ToolStripMenuItemEx.ToolStripItemType.Label;
        actionMenuItem.Enabled = false;
      }
      return (ToolStripMenuItem) actionMenuItem;
    }

    private void onCompareLockFields(object sender, EventArgs e)
    {
      if (!this.OpenLoan())
        return;
      Session.Application.GetService<ILoanEditor>().OpenForm("Lock Comparison Tool");
    }

    private void onValidatePricing(object sender, EventArgs e)
    {
      if (!this.OpenLoan())
        return;
      ILoanEditor service = Session.Application.GetService<ILoanEditor>();
      service.OpenForm("Lock Comparison Tool");
      string partnerName;
      string epC2EpassUrl = Epc2ServiceClient.GetEPC2EPassURL(Session.SessionObjects, Session.LoanData.GUID, this.ProviderId, "urn:elli:services:form:secondarylock:validatelock", out partnerName);
      if (string.IsNullOrWhiteSpace(epC2EpassUrl))
      {
        int num = (int) Utils.Dialog((IWin32Window) null, string.Format("Please contact your administrator for \"{0}\" access.", (object) partnerName), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        Session.Application.GetService<IEPass>().ProcessURL(epC2EpassUrl);
        service.OpenForm("Lock Comparison Tool");
      }
    }

    private bool IsLoanInBulkTrade(LoanData loanData)
    {
      string field1 = loanData.GetField("2626");
      string field2 = loanData.GetField("3967");
      List<string> source = new List<string>()
      {
        "Bulk",
        "Bulk AOT"
      };
      return string.Compare(field1, "Correspondent", true) == 0 && source.Contains<string>(field2, (IEqualityComparer<string>) StringComparer.InvariantCultureIgnoreCase);
    }

    private enum ImageType
    {
      NormalImage,
      HotImage,
    }
  }
}
