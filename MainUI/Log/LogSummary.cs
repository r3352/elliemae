// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.LogSummary
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.Common.TimeZones;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Log
{
  public class LogSummary : Form
  {
    private EnhancedDisclosureTracking2015Log enhancedDT2015Log;
    private IDisclosureManager disclosureManager;
    private List<string> disclosureDetailItems = new List<string>()
    {
      "Role",
      "Sent Date",
      "By",
      "Sent Method",
      "Received Method",
      "Presumed Received Date",
      "Actual Received Date",
      "Borrower Type",
      "Mailing Address"
    };
    private List<string> disclosureTrackingItems = new List<string>()
    {
      "Role",
      "eDisclosures Sent",
      "ePackage ID",
      "Email address",
      "Consent when eDisclosure was sent",
      "Message Viewed",
      "Package Consent Form Accepted",
      "Package Consent Form Accepted from IP",
      "Package Consent Form Rejected",
      "Package Consent Form Rejected from IP",
      "Authenticated",
      "Authenticated from IP Address",
      "eSigned Viewed Date",
      "WetSigned Viewed Date",
      "eSigned Disclosures",
      "eSigned Disclosures from IP Address",
      "Informational Viewed Date",
      "Informational Viewed from IP Address",
      "Informational Completed Date",
      "Informational Completed from IP Address",
      "Fulfilled by",
      "Date/Time Fulfilled",
      "Fulfillment Method",
      "Fulfillment Presumed Received Date",
      "Fulfillment Actual Received Date"
    };
    private IContainer components;
    private Button btnClose;
    private Panel panel1;
    private Panel panel2;
    private GridView grdVwDisclosureDetails;
    private GradientPanel gradientPanel1;
    private Label label1;
    private GridView grdVwEDisclosureTracking;
    private GradientPanel gradientPanel2;
    private Label label2;
    private Panel panel3;

    public LogSummary(IDisclosureManager disclosureManager)
    {
      this.InitializeComponent();
      this.disclosureManager = disclosureManager;
      this.enhancedDT2015Log = (EnhancedDisclosureTracking2015Log) disclosureManager.DisclosureTrackingLog;
      List<EnhancedDisclosureTracking2015Log.DisclosureRecipient> list = this.enhancedDT2015Log.DisclosureRecipients.Where<EnhancedDisclosureTracking2015Log.DisclosureRecipient>((Func<EnhancedDisclosureTracking2015Log.DisclosureRecipient, bool>) (x => !string.IsNullOrEmpty(x.Name))).ToList<EnhancedDisclosureTracking2015Log.DisclosureRecipient>();
      this.populateGridColumns(list);
      this.populateDisclosureDetails(list);
      this.populateDisclosureTrackingDetails(list);
      if (disclosureManager.DisclosureTrackingLog.DisclosureMethod != DisclosureTrackingBase.DisclosedMethod.ClosingDocsOrder)
        return;
      this.gradientPanel2.Visible = false;
      this.grdVwEDisclosureTracking.Visible = false;
    }

    private void populateDisclosureDetails(
      List<EnhancedDisclosureTracking2015Log.DisclosureRecipient> recipients)
    {
      foreach (string disclosureDetailItem in this.disclosureDetailItems)
      {
        GVItem gvItem = new GVItem();
        if (disclosureDetailItem == "Role")
          gvItem.SubItems.Add((object) "");
        else if (this.enhancedDT2015Log.DisclosedMethod != DisclosureTrackingBase.DisclosedMethod.eClose || !(disclosureDetailItem == "Presumed Received Date") && !(disclosureDetailItem == "Actual Received Date"))
          gvItem.SubItems.Add((object) disclosureDetailItem);
        else
          continue;
        foreach (EnhancedDisclosureTracking2015Log.DisclosureRecipient recipient in recipients)
        {
          EnhancedDisclosureTracking2015Log.Address address = (EnhancedDisclosureTracking2015Log.Address) null;
          string str;
          if (recipient.Role == EnhancedDisclosureTracking2015Log.DisclosureRecipientType.Borrower)
          {
            address = ((EnhancedDisclosureTracking2015Log.BorrowerRecipient) recipient).MailingAddress;
            str = "Borrower";
          }
          else if (recipient.Role == EnhancedDisclosureTracking2015Log.DisclosureRecipientType.CoBorrower)
          {
            address = ((EnhancedDisclosureTracking2015Log.CoborrowerRecipient) recipient).MailingAddress;
            str = "Co-Borrower";
          }
          else
            str = recipient.Role != EnhancedDisclosureTracking2015Log.DisclosureRecipientType.NonBorrowingOwner ? (recipient.Role != EnhancedDisclosureTracking2015Log.DisclosureRecipientType.LoanAssociate ? "Other" : "Loan Associate") : "Non-Borrowing Owner";
          switch (disclosureDetailItem)
          {
            case "Actual Received Date":
              gvItem.SubItems.Add(recipient.ActualReceivedDate == DateTime.MinValue ? (object) "" : (object) recipient.ActualReceivedDate.ToString("MM/dd/yyyy"));
              continue;
            case "Borrower Type":
              if (recipient.BorrowerType.UseUserValue)
              {
                gvItem.SubItems.Add((object) recipient.BorrowerType.UserValue);
                continue;
              }
              gvItem.SubItems.Add((object) recipient.BorrowerType.ComputedValue);
              continue;
            case "By":
              if (this.disclosureManager.DisclosureTrackingLog.IsDisclosedByLocked)
              {
                gvItem.SubItems.Add((object) this.disclosureManager.DisclosureTrackingLog.LockedDisclosedByField);
                continue;
              }
              gvItem.SubItems.Add((object) (this.disclosureManager.DisclosureTrackingLog.DisclosedByFullName + "(" + this.disclosureManager.DisclosureTrackingLog.DisclosedBy + ")"));
              continue;
            case "Mailing Address":
              if (address != null)
              {
                gvItem.SubItems.Add((object) address.AddressFullName);
                continue;
              }
              continue;
            case "Presumed Received Date":
              if (this.disclosureManager.DisclosureTrackingLog.DisclosureMethod != DisclosureTrackingBase.DisclosedMethod.ClosingDocsOrder && this.disclosureManager.DisclosureTrackingLog.DisclosureMethod != DisclosureTrackingBase.DisclosedMethod.eClose)
              {
                if (recipient.PresumedReceivedDate.UseUserValue)
                {
                  gvItem.SubItems.Add(recipient.PresumedReceivedDate.UserValue == DateTime.MinValue ? (object) "" : (object) recipient.PresumedReceivedDate.UserValue.ToString("MM/dd/yyyy"));
                  continue;
                }
                gvItem.SubItems.Add(recipient.PresumedReceivedDate.ComputedValue == DateTime.MinValue ? (object) "" : (object) recipient.PresumedReceivedDate.ComputedValue.ToString("MM/dd/yyyy"));
                continue;
              }
              continue;
            case "Received Method":
              gvItem.SubItems.Add((object) this.populateReceivedMethod(recipient.DisclosedMethod));
              continue;
            case "Role":
              gvItem.SubItems.Add((object) str);
              continue;
            case "Sent Date":
              if (this.disclosureManager.DisclosureTrackingLog.DisclosureMethod == DisclosureTrackingBase.DisclosedMethod.eDisclosure && this.disclosureManager.DisclosureTrackingLog.eDisclosurePackageCreatedDate != DateTime.MinValue && !this.disclosureManager.DisclosureTrackingLog.IsLocked)
              {
                gvItem.SubItems.Add((object) this.disclosureManager.DisclosureTrackingLog.eDisclosurePackageCreatedDate);
                continue;
              }
              gvItem.SubItems.Add((object) this.disclosureManager.DisclosedDate.ToString("MM/dd/yyyy"));
              continue;
            case "Sent Method":
              gvItem.SubItems.Add((object) this.disclosureManager.DisclosureTrackingLog.DisclosureMethod);
              continue;
            default:
              continue;
          }
        }
        if (gvItem.SubItems.Count > 0)
          this.grdVwDisclosureDetails.Items.Add(gvItem);
      }
    }

    private string populateReceivedMethod(
      DisclosureTrackingBase.DisclosedMethod disclosedMethod)
    {
      string[] strArray = new string[8]
      {
        "U.S. Mail",
        "In Person",
        "Email",
        "eFolder eDisclosures",
        "Fax",
        "Other",
        "Closing Docs Order",
        "eClose"
      };
      switch (disclosedMethod)
      {
        case DisclosureTrackingBase.DisclosedMethod.ByMail:
          return strArray[0];
        case DisclosureTrackingBase.DisclosedMethod.eDisclosure:
          return strArray[3];
        case DisclosureTrackingBase.DisclosedMethod.Fax:
          return strArray[4];
        case DisclosureTrackingBase.DisclosedMethod.InPerson:
          return strArray[1];
        case DisclosureTrackingBase.DisclosedMethod.Other:
          return strArray[5];
        case DisclosureTrackingBase.DisclosedMethod.Email:
          return strArray[2];
        case DisclosureTrackingBase.DisclosedMethod.ClosingDocsOrder:
        case DisclosureTrackingBase.DisclosedMethod.eClose:
          return string.Empty;
        default:
          return string.Empty;
      }
    }

    private void populateDisclosureTrackingDetails(
      List<EnhancedDisclosureTracking2015Log.DisclosureRecipient> recipients)
    {
      foreach (string disclosureTrackingItem in this.disclosureTrackingItems)
      {
        GVItem gvItem = new GVItem();
        if (disclosureTrackingItem == "Role")
          gvItem.SubItems.Add((object) "");
        else
          gvItem.SubItems.Add((object) disclosureTrackingItem);
        foreach (EnhancedDisclosureTracking2015Log.DisclosureRecipient recipient1 in recipients)
        {
          EnhancedDisclosureTracking2015Log.DisclosureRecipient recipient = recipient1;
          IList<EnhancedDisclosureTracking2015Log.FulfillmentFields> fulfillments = this.enhancedDT2015Log.Fulfillments;
          EnhancedDisclosureTracking2015Log.FulfillmentFields fulfillment = fulfillments != null ? fulfillments.FirstOrDefault<EnhancedDisclosureTracking2015Log.FulfillmentFields>() : (EnhancedDisclosureTracking2015Log.FulfillmentFields) null;
          EnhancedDisclosureTracking2015Log.FulfillmentRecipient fulfillmentRecipient;
          if (fulfillment == null || !Enum.IsDefined(typeof (EnhancedDisclosureTracking2015Log.FulfillmentRecipientType), (object) recipient.Role.ToString()))
          {
            fulfillment = new EnhancedDisclosureTracking2015Log.FulfillmentFields(this.enhancedDT2015Log);
            fulfillmentRecipient = new EnhancedDisclosureTracking2015Log.FulfillmentRecipient(this.enhancedDT2015Log);
          }
          else
            fulfillmentRecipient = fulfillment.Recipients.FirstOrDefault<EnhancedDisclosureTracking2015Log.FulfillmentRecipient>((Func<EnhancedDisclosureTracking2015Log.FulfillmentRecipient, bool>) (fr => fr.Id == recipient.Id));
          string str1 = recipient.Role != EnhancedDisclosureTracking2015Log.DisclosureRecipientType.Borrower ? (recipient.Role != EnhancedDisclosureTracking2015Log.DisclosureRecipientType.CoBorrower ? (recipient.Role != EnhancedDisclosureTracking2015Log.DisclosureRecipientType.NonBorrowingOwner ? (recipient.Role != EnhancedDisclosureTracking2015Log.DisclosureRecipientType.LoanAssociate ? "Other" : "Loan Associate") : "Non-Borrowing Owner") : "Co-Borrower") : "Borrower";
          switch (disclosureTrackingItem)
          {
            case "Authenticated":
              GVSubItemCollection subItems1 = gvItem.SubItems;
              DateTimeWithZone authenticatedDate = recipient.Tracking.AuthenticatedDate;
              string str2;
              if (!(authenticatedDate.DateTime == DateTime.MinValue))
              {
                authenticatedDate = recipient.Tracking.AuthenticatedDate;
                str2 = authenticatedDate.DateTime.ToString();
              }
              else
                str2 = "";
              subItems1.Add((object) str2);
              continue;
            case "Authenticated from IP Address":
              gvItem.SubItems.Add((object) recipient.Tracking.AuthenticatedIP);
              continue;
            case "Consent when eDisclosure was sent":
              gvItem.SubItems.Add((object) recipient.Tracking.LoanLevelConsent);
              continue;
            case "Date/Time Fulfilled":
              gvItem.SubItems.Add(fulfillment.ProcessedDate.DateTime == DateTime.MinValue ? (object) "" : (object) fulfillment.ProcessedDate.DateTime.ToString("MM/dd/yyyy hh:mm tt"));
              continue;
            case "Email address":
              gvItem.SubItems.Add((object) recipient.Email);
              continue;
            case "Fulfilled by":
              gvItem.SubItems.Add((object) fulfillment.OrderedBy);
              continue;
            case "Fulfillment Actual Received Date":
              gvItem.SubItems.Add(fulfillmentRecipient.ActualDate.DateTime == DateTime.MinValue ? (object) "" : (object) fulfillmentRecipient.ActualDate.DateTime.ToString());
              continue;
            case "Fulfillment Method":
              if (Enum.IsDefined(typeof (EnhancedDisclosureTracking2015Log.FulfillmentRecipientType), (object) recipient.Role.ToString()))
              {
                gvItem.SubItems.Add((object) this.GetFulfillmentMethod(fulfillment));
                continue;
              }
              continue;
            case "Fulfillment Presumed Received Date":
              gvItem.SubItems.Add(fulfillmentRecipient.PresumedDate.DateTime == DateTime.MinValue ? (object) "" : (object) fulfillmentRecipient.PresumedDate.DateTime.ToString());
              continue;
            case "Fulfillment Tracking Number":
              gvItem.SubItems.Add((object) fulfillment.TrackingNumber);
              continue;
            case "Informational Completed Date":
              GVSubItemCollection subItems2 = gvItem.SubItems;
              DateTimeWithZone informationalCompletedDate = recipient.Tracking.InformationalCompletedDate;
              string str3;
              if (!(informationalCompletedDate.DateTime == DateTime.MinValue))
              {
                informationalCompletedDate = recipient.Tracking.InformationalCompletedDate;
                str3 = informationalCompletedDate.DateTime.ToString();
              }
              else
                str3 = "";
              subItems2.Add((object) str3);
              continue;
            case "Informational Completed from IP Address":
              gvItem.SubItems.Add((object) recipient.Tracking.InformationalCompletedIP);
              continue;
            case "Informational Viewed Date":
              GVSubItemCollection subItems3 = gvItem.SubItems;
              DateTimeWithZone informationalViewedDate = recipient.Tracking.InformationalViewedDate;
              string str4;
              if (!(informationalViewedDate.DateTime == DateTime.MinValue))
              {
                informationalViewedDate = recipient.Tracking.InformationalViewedDate;
                str4 = informationalViewedDate.DateTime.ToString();
              }
              else
                str4 = "";
              subItems3.Add((object) str4);
              continue;
            case "Informational Viewed from IP Address":
              gvItem.SubItems.Add((object) recipient.Tracking.InformationalViewedIP);
              continue;
            case "Message Viewed":
              GVSubItemCollection subItems4 = gvItem.SubItems;
              DateTimeWithZone viewMessageDate = recipient.Tracking.ViewMessageDate;
              string str5;
              if (!(viewMessageDate.DateTime == DateTime.MinValue))
              {
                viewMessageDate = recipient.Tracking.ViewMessageDate;
                str5 = viewMessageDate.DateTime.ToString();
              }
              else
                str5 = "";
              subItems4.Add((object) str5);
              continue;
            case "Package Consent Form Accepted":
              GVSubItemCollection subItems5 = gvItem.SubItems;
              DateTimeWithZone acceptConsentDate = recipient.Tracking.AcceptConsentDate;
              string str6;
              if (!(acceptConsentDate.DateTime == DateTime.MinValue))
              {
                acceptConsentDate = recipient.Tracking.AcceptConsentDate;
                str6 = acceptConsentDate.DateTime.ToString();
              }
              else
                str6 = "";
              subItems5.Add((object) str6);
              continue;
            case "Package Consent Form Accepted from IP":
              gvItem.SubItems.Add((object) recipient.Tracking.AcceptConsentIP);
              continue;
            case "Package Consent Form Rejected":
              GVSubItemCollection subItems6 = gvItem.SubItems;
              DateTimeWithZone rejectConsentDate = recipient.Tracking.RejectConsentDate;
              string str7;
              if (!(rejectConsentDate.DateTime == DateTime.MinValue))
              {
                rejectConsentDate = recipient.Tracking.RejectConsentDate;
                str7 = rejectConsentDate.DateTime.ToString();
              }
              else
                str7 = "";
              subItems6.Add((object) str7);
              continue;
            case "Package Consent Form Rejected from IP":
              gvItem.SubItems.Add((object) recipient.Tracking.RejectConsentIP);
              continue;
            case "Role":
              gvItem.SubItems.Add((object) str1);
              continue;
            case "WetSigned Viewed Date":
              GVSubItemCollection subItems7 = gvItem.SubItems;
              DateTimeWithZone viewWetSignedDate = recipient.Tracking.ViewWetSignedDate;
              string str8;
              if (!(viewWetSignedDate.DateTime == DateTime.MinValue))
              {
                viewWetSignedDate = recipient.Tracking.ViewWetSignedDate;
                str8 = viewWetSignedDate.DateTime.ToString();
              }
              else
                str8 = "";
              subItems7.Add((object) str8);
              continue;
            case "eDisclosures Sent":
              gvItem.SubItems.Add(this.disclosureManager.DisclosureTrackingLog.eDisclosurePackageCreatedDate != DateTime.MinValue ? (object) this.disclosureManager.DisclosureTrackingLog.eDisclosurePackageCreatedDate.ToString("MM/dd/yyyy hh:mm tt") : (object) "");
              continue;
            case "ePackage ID":
              gvItem.SubItems.Add(string.IsNullOrWhiteSpace(((EnhancedDisclosureTracking2015Log) this.disclosureManager.DisclosureTrackingLog).Tracking.PackageId) ? (object) "" : (object) ((EnhancedDisclosureTracking2015Log) this.disclosureManager.DisclosureTrackingLog).Tracking.PackageId);
              continue;
            case "eSigned Disclosures":
              GVSubItemCollection subItems8 = gvItem.SubItems;
              DateTimeWithZone esignedDate = recipient.Tracking.ESignedDate;
              string str9;
              if (!(esignedDate.DateTime == DateTime.MinValue))
              {
                esignedDate = recipient.Tracking.ESignedDate;
                str9 = esignedDate.DateTime.ToString();
              }
              else
                str9 = "";
              subItems8.Add((object) str9);
              continue;
            case "eSigned Disclosures from IP Address":
              gvItem.SubItems.Add((object) recipient.Tracking.ESignedIP);
              continue;
            case "eSigned Viewed Date":
              GVSubItemCollection subItems9 = gvItem.SubItems;
              DateTimeWithZone viewEsignedDate = recipient.Tracking.ViewESignedDate;
              string str10;
              if (!(viewEsignedDate.DateTime == DateTime.MinValue))
              {
                viewEsignedDate = recipient.Tracking.ViewESignedDate;
                str10 = viewEsignedDate.DateTime.ToString();
              }
              else
                str10 = "";
              subItems9.Add((object) str10);
              continue;
            default:
              continue;
          }
        }
        if (gvItem.SubItems.Count > 0)
          this.grdVwEDisclosureTracking.Items.Add(gvItem);
      }
    }

    private string GetFulfillmentMethod(
      EnhancedDisclosureTracking2015Log.FulfillmentFields fulfillment)
    {
      if (!fulfillment.IsManual)
      {
        string fulfillmentMethod = "";
        if (this.disclosureManager.DisclosureTrackingLog.FullfillmentProcessedDate != DateTime.MinValue)
          fulfillmentMethod = fulfillment.DisclosedMethod != DisclosureTrackingBase.DisclosedMethod.OvernightShipping ? this.disclosureManager.AutomaticFullfillmentServiceName : "Overnight Shipping";
        return fulfillmentMethod;
      }
      switch (fulfillment.DisclosedMethod)
      {
        case DisclosureTrackingBase.DisclosedMethod.ByMail:
          return "U.S. Mail";
        case DisclosureTrackingBase.DisclosedMethod.InPerson:
          return "In Person";
        default:
          return "";
      }
    }

    private void populateGridColumns(
      List<EnhancedDisclosureTracking2015Log.DisclosureRecipient> recipients)
    {
      foreach (EnhancedDisclosureTracking2015Log.DisclosureRecipient recipient in recipients)
      {
        GVColumn newColumn1 = new GVColumn(recipient.Name);
        GVColumn newColumn2 = new GVColumn(recipient.Name);
        newColumn1.SortMethod = newColumn2.SortMethod = GVSortMethod.Text;
        newColumn1.Width = newColumn2.Width = 150;
        this.grdVwDisclosureDetails.Columns.Add(newColumn1);
        this.grdVwEDisclosureTracking.Columns.Add(newColumn2);
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
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      this.btnClose = new Button();
      this.panel1 = new Panel();
      this.grdVwDisclosureDetails = new GridView();
      this.gradientPanel1 = new GradientPanel();
      this.label1 = new Label();
      this.panel2 = new Panel();
      this.grdVwEDisclosureTracking = new GridView();
      this.gradientPanel2 = new GradientPanel();
      this.label2 = new Label();
      this.panel3 = new Panel();
      this.panel1.SuspendLayout();
      this.gradientPanel1.SuspendLayout();
      this.panel2.SuspendLayout();
      this.gradientPanel2.SuspendLayout();
      this.panel3.SuspendLayout();
      this.SuspendLayout();
      this.btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnClose.DialogResult = DialogResult.OK;
      this.btnClose.Location = new Point(611, 6);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new Size(75, 23);
      this.btnClose.TabIndex = 1;
      this.btnClose.Text = "Close";
      this.btnClose.UseVisualStyleBackColor = true;
      this.panel1.AutoScroll = true;
      this.panel1.BackColor = Color.Transparent;
      this.panel1.Controls.Add((Control) this.grdVwDisclosureDetails);
      this.panel1.Controls.Add((Control) this.gradientPanel1);
      this.panel1.Dock = DockStyle.Top;
      this.panel1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.panel1.Location = new Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(698, 212);
      this.panel1.TabIndex = 2;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.SortMethod = GVSortMethod.None;
      gvColumn1.Text = "Recipient";
      gvColumn1.Width = 230;
      this.grdVwDisclosureDetails.Columns.AddRange(new GVColumn[1]
      {
        gvColumn1
      });
      this.grdVwDisclosureDetails.Dock = DockStyle.Fill;
      this.grdVwDisclosureDetails.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.grdVwDisclosureDetails.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.grdVwDisclosureDetails.Location = new Point(0, 25);
      this.grdVwDisclosureDetails.Name = "grdVwDisclosureDetails";
      this.grdVwDisclosureDetails.Size = new Size(698, 187);
      this.grdVwDisclosureDetails.TabIndex = 0;
      this.gradientPanel1.Controls.Add((Control) this.label1);
      this.gradientPanel1.Dock = DockStyle.Top;
      this.gradientPanel1.Location = new Point(0, 0);
      this.gradientPanel1.Name = "gradientPanel1";
      this.gradientPanel1.Size = new Size(698, 25);
      this.gradientPanel1.TabIndex = 1;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(13, 6);
      this.label1.Name = "label1";
      this.label1.Size = new Size(109, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Disclosure Details";
      this.panel2.AutoScroll = true;
      this.panel2.AutoScrollMinSize = new Size(650, 0);
      this.panel2.Controls.Add((Control) this.grdVwEDisclosureTracking);
      this.panel2.Controls.Add((Control) this.gradientPanel2);
      this.panel2.Dock = DockStyle.Top;
      this.panel2.Location = new Point(0, 212);
      this.panel2.MaximumSize = new Size(0, 500);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(698, 248);
      this.panel2.TabIndex = 3;
      this.grdVwEDisclosureTracking.AllowMultiselect = false;
      this.grdVwEDisclosureTracking.AutoHeight = true;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column1";
      gvColumn2.SortMethod = GVSortMethod.None;
      gvColumn2.Text = "Recipient";
      gvColumn2.Width = 230;
      this.grdVwEDisclosureTracking.Columns.AddRange(new GVColumn[1]
      {
        gvColumn2
      });
      this.grdVwEDisclosureTracking.Dock = DockStyle.Fill;
      this.grdVwEDisclosureTracking.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.grdVwEDisclosureTracking.Location = new Point(0, 26);
      this.grdVwEDisclosureTracking.Name = "grdVwEDisclosureTracking";
      this.grdVwEDisclosureTracking.Size = new Size(698, 222);
      this.grdVwEDisclosureTracking.TabIndex = 3;
      this.gradientPanel2.Controls.Add((Control) this.label2);
      this.gradientPanel2.Dock = DockStyle.Top;
      this.gradientPanel2.Location = new Point(0, 0);
      this.gradientPanel2.Name = "gradientPanel2";
      this.gradientPanel2.Size = new Size(698, 26);
      this.gradientPanel2.TabIndex = 2;
      this.label2.AutoSize = true;
      this.label2.BackColor = Color.Transparent;
      this.label2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(13, 6);
      this.label2.Name = "label2";
      this.label2.Size = new Size((int) sbyte.MaxValue, 13);
      this.label2.TabIndex = 0;
      this.label2.Text = "eDisclosure Tracking";
      this.panel3.Controls.Add((Control) this.btnClose);
      this.panel3.Dock = DockStyle.Fill;
      this.panel3.Location = new Point(0, 460);
      this.panel3.Name = "panel3";
      this.panel3.Size = new Size(698, 35);
      this.panel3.TabIndex = 4;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.ClientSize = new Size(698, 495);
      this.Controls.Add((Control) this.panel3);
      this.Controls.Add((Control) this.panel2);
      this.Controls.Add((Control) this.panel1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MinimizeBox = false;
      this.Name = nameof (LogSummary);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Summary Details";
      this.panel1.ResumeLayout(false);
      this.gradientPanel1.ResumeLayout(false);
      this.gradientPanel1.PerformLayout();
      this.panel2.ResumeLayout(false);
      this.gradientPanel2.ResumeLayout(false);
      this.gradientPanel2.PerformLayout();
      this.panel3.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
