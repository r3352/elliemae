// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.BorrowerHistoryForm
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.Common.Contact;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI
{
  public class BorrowerHistoryForm : Form, IBindingForm
  {
    private IContainer components;
    private int currentContactId = -1;
    private System.Windows.Forms.LinkLabel linkLblMailMerge;
    private GroupContainer gcEvent;
    private CollapsibleSplitter collapsibleSplitter1;
    private GroupContainer gcDetail;
    private Panel panel1;
    private GridView lvwHistory;
    private ToolTip toolTip1;
    private TextBox txtDetails;
    private StandardIconButton btnDelete;
    private BorrowerInfo currentContact;
    public bool IsReadOnly;

    public BorrowerHistoryForm()
    {
      this.InitializeComponent();
      this.disableForm();
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
      this.linkLblMailMerge = new System.Windows.Forms.LinkLabel();
      this.gcEvent = new GroupContainer();
      this.btnDelete = new StandardIconButton();
      this.lvwHistory = new GridView();
      this.collapsibleSplitter1 = new CollapsibleSplitter();
      this.gcDetail = new GroupContainer();
      this.panel1 = new Panel();
      this.txtDetails = new TextBox();
      this.toolTip1 = new ToolTip(this.components);
      this.gcEvent.SuspendLayout();
      ((ISupportInitialize) this.btnDelete).BeginInit();
      this.gcDetail.SuspendLayout();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      this.linkLblMailMerge.Location = new Point(16, 244);
      this.linkLblMailMerge.Name = "linkLblMailMerge";
      this.linkLblMailMerge.Size = new Size(232, 16);
      this.linkLblMailMerge.TabIndex = 7;
      this.linkLblMailMerge.Visible = false;
      this.linkLblMailMerge.LinkClicked += new LinkLabelLinkClickedEventHandler(this.linkLblMailMerge_LinkClicked);
      this.gcEvent.Controls.Add((Control) this.btnDelete);
      this.gcEvent.Controls.Add((Control) this.lvwHistory);
      this.gcEvent.Dock = DockStyle.Left;
      this.gcEvent.Location = new Point(1, 1);
      this.gcEvent.Name = "gcEvent";
      this.gcEvent.Size = new Size(207, 266);
      this.gcEvent.TabIndex = 9;
      this.gcEvent.Text = "Events";
      this.btnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDelete.BackColor = Color.Transparent;
      this.btnDelete.Location = new Point(185, 4);
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new Size(16, 16);
      this.btnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnDelete.TabIndex = 8;
      this.btnDelete.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnDelete, "Delete Event");
      this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
      this.lvwHistory.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Event Time";
      gvColumn1.Width = 100;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Event";
      gvColumn2.Width = 100;
      this.lvwHistory.Columns.AddRange(new GVColumn[2]
      {
        gvColumn1,
        gvColumn2
      });
      this.lvwHistory.Dock = DockStyle.Fill;
      this.lvwHistory.Location = new Point(1, 26);
      this.lvwHistory.Name = "lvwHistory";
      this.lvwHistory.Size = new Size(205, 239);
      this.lvwHistory.TabIndex = 7;
      this.lvwHistory.SelectedIndexChanged += new EventHandler(this.lvwHistory_SelectedIndexChanged);
      this.collapsibleSplitter1.AnimationDelay = 20;
      this.collapsibleSplitter1.AnimationStep = 20;
      this.collapsibleSplitter1.BorderStyle3D = Border3DStyle.Flat;
      this.collapsibleSplitter1.ControlToHide = (Control) this.gcEvent;
      this.collapsibleSplitter1.ExpandParentForm = false;
      this.collapsibleSplitter1.Location = new Point(208, 1);
      this.collapsibleSplitter1.Name = "collapsibleSplitter1";
      this.collapsibleSplitter1.TabIndex = 10;
      this.collapsibleSplitter1.TabStop = false;
      this.collapsibleSplitter1.UseAnimations = false;
      this.collapsibleSplitter1.VisualStyle = VisualStyles.Encompass;
      this.gcDetail.Controls.Add((Control) this.panel1);
      this.gcDetail.Dock = DockStyle.Fill;
      this.gcDetail.Location = new Point(215, 1);
      this.gcDetail.Name = "gcDetail";
      this.gcDetail.Size = new Size(308, 266);
      this.gcDetail.TabIndex = 11;
      this.gcDetail.Text = "Event Details";
      this.panel1.BackColor = Color.Transparent;
      this.panel1.Controls.Add((Control) this.txtDetails);
      this.panel1.Dock = DockStyle.Fill;
      this.panel1.Location = new Point(1, 26);
      this.panel1.Name = "panel1";
      this.panel1.Padding = new Padding(3);
      this.panel1.Size = new Size(306, 239);
      this.panel1.TabIndex = 9;
      this.txtDetails.Dock = DockStyle.Fill;
      this.txtDetails.Location = new Point(3, 3);
      this.txtDetails.Multiline = true;
      this.txtDetails.Name = "txtDetails";
      this.txtDetails.ReadOnly = true;
      this.txtDetails.ScrollBars = ScrollBars.Both;
      this.txtDetails.Size = new Size(300, 233);
      this.txtDetails.TabIndex = 9;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.White;
      this.ClientSize = new Size(524, 268);
      this.Controls.Add((Control) this.gcDetail);
      this.Controls.Add((Control) this.collapsibleSplitter1);
      this.Controls.Add((Control) this.gcEvent);
      this.Controls.Add((Control) this.linkLblMailMerge);
      this.Font = new Font("Arial", 11f, FontStyle.Regular, GraphicsUnit.Pixel, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.None;
      this.Name = nameof (BorrowerHistoryForm);
      this.Padding = new Padding(1);
      this.Text = "ContactHistoryForm";
      this.gcEvent.ResumeLayout(false);
      ((ISupportInitialize) this.btnDelete).EndInit();
      this.gcDetail.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.ResumeLayout(false);
    }

    public bool isDirty() => false;

    public int CurrentContactID
    {
      get => this.currentContactId;
      set
      {
        this.currentContactId = -1;
        this.lvwHistory.Items.Clear();
        this.clearHistory();
        if (value >= 0)
        {
          ContactHistoryItem[] historyForContact = Session.ContactManager.GetHistoryForContact(value, ContactType.Borrower);
          for (int index = 0; index < historyForContact.Length; ++index)
            this.lvwHistory.Items.Add(new GVItem(new string[2]
            {
              historyForContact[index].Timestamp.ToShortDateString() + " " + historyForContact[index].Timestamp.ToShortTimeString(),
              this.translateEventType(historyForContact[index].EventType)
            })
            {
              Tag = (object) historyForContact[index]
            });
          this.currentContactId = value;
        }
        this.gcEvent.Text = "Events (" + (object) this.lvwHistory.Items.Count + ")";
      }
    }

    public object CurrentContact
    {
      get => (object) this.currentContact;
      set
      {
        this.currentContactId = -1;
        this.currentContact = (BorrowerInfo) null;
        this.lvwHistory.Items.Clear();
        this.clearHistory();
        if (value != null)
        {
          this.currentContact = (BorrowerInfo) value;
          this.currentContactId = this.currentContact.ContactID;
          ContactHistoryItem[] historyForContact = Session.ContactManager.GetHistoryForContact(this.currentContact.ContactID, ContactType.Borrower);
          for (int index = 0; index < historyForContact.Length; ++index)
          {
            string[] items = new string[2];
            DateTime timestamp = historyForContact[index].Timestamp;
            string shortDateString = timestamp.ToShortDateString();
            timestamp = historyForContact[index].Timestamp;
            string shortTimeString = timestamp.ToShortTimeString();
            items[0] = shortDateString + " " + shortTimeString;
            items[1] = this.translateEventType(historyForContact[index].EventType);
            this.lvwHistory.Items.Add(new GVItem(items)
            {
              Tag = (object) historyForContact[index]
            });
          }
        }
        this.gcEvent.Text = "Events (" + (object) this.lvwHistory.Items.Count + ")";
      }
    }

    private string translateEventType(string originalEvent)
    {
      switch (originalEvent)
      {
        case "Called":
          return "Call Received";
        case "Campaign Add":
          return "Added to Campaign";
        case "Campaign Complete":
          return "Campaign Completed";
        case "Campaign Email":
          return "Campaign Email Received";
        case "Campaign Fax":
          return "Campaign Fax Received";
        case "Campaign Letter":
          return "Campaign Letter Received";
        case "Campaign Phone Call":
          return "Campaign Call Received";
        case "Campaign Reminder":
          return "Campaign Reminder Completed";
        case "Emailed":
          return "Email Received";
        case "Faxed":
          return "Fax Received";
        case "First Contact":
          return "Contact Created";
        case "Loan Origination":
          return "Loan Originated";
        default:
          return originalEvent;
      }
    }

    public bool SaveChanges() => false;

    private void lvwHistory_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.lvwHistory.SelectedItems.Count > 0)
        this.showHistory((ContactHistoryItem) this.lvwHistory.SelectedItems[0].Tag);
      else
        this.clearHistory();
    }

    private void clearHistory()
    {
      this.txtDetails.Text = "";
      this.linkLblMailMerge.Visible = false;
      this.btnDelete.Enabled = false;
    }

    public void disableForm() => this.btnDelete.Enabled = false;

    private void showLoanOriginationHistoryWithLoanInfo(ContactHistoryItem item)
    {
      try
      {
        ContactLoanInfo contactLoan = Session.ContactManager.GetContactLoan(item.LoanID);
        if (contactLoan == null)
          return;
        this.txtDetails.Lines = new List<string>()
        {
          "Origination Date: " + item.Timestamp.ToString("d", (IFormatProvider) null),
          "Originated By: " + item.Sender,
          "Loan status: " + contactLoan.LoanStatus,
          "Appraised value: " + contactLoan.AppraisedValue.ToString("c", (IFormatProvider) ContactFormat.NFI_AV),
          "Loan Amount: " + contactLoan.LoanAmount.ToString("c", (IFormatProvider) ContactFormat.NFI_LA),
          "Interest Rate: " + contactLoan.InterestRate.ToString("c", (IFormatProvider) ContactFormat.NFI_IR) + "%",
          "Term: " + contactLoan.Term.ToString(),
          "Purpose: " + LoanPurposeEnumUtil.ValueToName(contactLoan.Purpose),
          "Down Payment: " + contactLoan.DownPayment.ToString("c", (IFormatProvider) ContactFormat.NFI_LA),
          "Amortization: " + AmortizationTypeEnumUtil.ValueToName(contactLoan.Amortization)
        }.ToArray();
      }
      catch
      {
        this.txtDetails.Lines = new string[1]
        {
          "No event details available."
        };
      }
    }

    private void showLoanOriginationHistory(ContactHistoryItem item)
    {
      if (item.LoanID < 0)
        this.txtDetails.Lines = new List<string>()
        {
          "Origination Date: " + item.Timestamp.ToString("d", (IFormatProvider) null),
          "Originated By: " + item.Sender
        }.ToArray();
      else
        this.showLoanOriginationHistoryWithLoanInfo(item);
    }

    private void showCreditScoresHistory(ContactHistoryItem item)
    {
      ContactCreditScores[] scoresForHistoryItem = Session.ContactManager.GetCreditScoresForHistoryItem(this.currentContactId, item.HistoryItemID);
      StringBuilder stringBuilder = new StringBuilder("");
      List<string> stringList = new List<string>();
      stringList.Add("Credit report received on: " + item.Timestamp.ToString("d", (IFormatProvider) null));
      stringList.Add("User ID of person who ordered the report: " + item.Sender);
      for (int index1 = 0; index1 < scoresForHistoryItem.Length; ++index1)
      {
        ContactCreditScores contactCreditScores = scoresForHistoryItem[index1];
        stringList.Add("Credit report for " + contactCreditScores.FirstName + " " + contactCreditScores.LastName);
        for (int index2 = 0; index2 < contactCreditScores.Scores.Length; ++index2)
          stringList.Add(contactCreditScores.Scores[index2].Source + "  " + contactCreditScores.Scores[index2].Score);
      }
      this.txtDetails.Lines = stringList.ToArray();
    }

    private void showClosedLoanHistory(ContactHistoryItem item)
    {
      this.linkLblMailMerge.Visible = false;
      if (item.LoanID < 0)
      {
        this.txtDetails.Lines = new string[1]
        {
          "No event details available."
        };
      }
      else
      {
        try
        {
          ContactLoanInfo contactLoan = Session.ContactManager.GetContactLoan(item.LoanID);
          if (contactLoan == null)
            return;
          this.txtDetails.Lines = new List<string>()
          {
            "Loan status: " + contactLoan.LoanStatus,
            "Appraised value: " + contactLoan.AppraisedValue.ToString("c", (IFormatProvider) ContactFormat.NFI_AV),
            "Loan Amount: " + contactLoan.LoanAmount.ToString("c", (IFormatProvider) ContactFormat.NFI_LA),
            "Interest Rate: " + contactLoan.InterestRate.ToString("c", (IFormatProvider) ContactFormat.NFI_IR) + "%",
            "Term: " + contactLoan.Term.ToString(),
            "Purpose: " + LoanPurposeEnumUtil.ValueToName(contactLoan.Purpose),
            "Down Payment: " + contactLoan.DownPayment.ToString("c", (IFormatProvider) ContactFormat.NFI_DP) + "%",
            "LTV: " + contactLoan.LTV.ToString(),
            "Amortization: " + AmortizationTypeEnumUtil.ValueToName(contactLoan.Amortization),
            "Completion date: " + contactLoan.DateCompleted.ToString("d", (IFormatProvider) null),
            "Loan type: " + LoanTypeEnumUtil.ValueToName(contactLoan.LoanType),
            "Lien position: " + LienEnumUtil.ValueToName(contactLoan.LienPosition)
          }.ToArray();
        }
        catch
        {
          this.txtDetails.Lines = new string[1]
          {
            "No event details available."
          };
        }
      }
    }

    private void showMailMergeHistory(ContactHistoryItem item)
    {
      this.txtDetails.Lines = new List<string>()
      {
        "Document Name: " + item.LetterName,
        "Letter Printed Date: " + item.Timestamp.ToString("d", (IFormatProvider) null),
        "Letter Printed By: " + item.Sender
      }.ToArray();
      this.linkLblMailMerge.Text = "View " + item.LetterName;
    }

    private void showEmailMergeHistory(ContactHistoryItem item)
    {
      this.txtDetails.Lines = new List<string>()
      {
        "Document Name: " + item.LetterName,
        "Subject: " + item.Subject,
        "Email Received Date: " + item.Timestamp.ToString("d", (IFormatProvider) null),
        "Email Sent By: " + item.Sender
      }.ToArray();
      this.linkLblMailMerge.Text = "View " + item.LetterName;
    }

    private void showCallHistory(ContactHistoryItem item)
    {
      this.linkLblMailMerge.Visible = false;
      if (item.LoanID < 0)
      {
        this.txtDetails.Lines = new string[1]
        {
          "No event details available."
        };
      }
      else
      {
        try
        {
          ContactNote contactNote = Session.ContactManager.GetContactNote(item.LoanID, ContactType.Borrower);
          if (contactNote != null)
            this.txtDetails.Lines = new List<string>()
            {
              "Call Received Date: " + item.Timestamp.ToString("d", (IFormatProvider) null),
              "Call Made By: " + item.Sender,
              "Subject: " + contactNote.Subject,
              "Details: " + contactNote.Details
            }.ToArray();
          else
            this.txtDetails.Lines = new string[1]
            {
              "No event details available."
            };
        }
        catch
        {
          this.txtDetails.Lines = new string[1]
          {
            "No event details available."
          };
        }
      }
    }

    private void showEmailHistory(ContactHistoryItem item)
    {
      this.linkLblMailMerge.Visible = false;
      if (item.LoanID < 0)
      {
        this.txtDetails.Lines = new string[1]
        {
          "No event details available."
        };
      }
      else
      {
        try
        {
          ContactNote contactNote = Session.ContactManager.GetContactNote(item.LoanID, ContactType.Borrower);
          if (contactNote != null)
            this.txtDetails.Lines = new List<string>()
            {
              "Email Received Date: " + item.Timestamp.ToString("d", (IFormatProvider) null),
              "Email Sent By: " + item.Sender,
              "Subject: " + contactNote.Subject,
              "Details: " + contactNote.Details
            }.ToArray();
          else
            this.txtDetails.Lines = new string[1]
            {
              "No event details available."
            };
        }
        catch
        {
          this.txtDetails.Lines = new string[1]
          {
            "No event details available."
          };
        }
      }
    }

    private void showFaxHistory(ContactHistoryItem item)
    {
      this.linkLblMailMerge.Visible = false;
      if (item.LoanID < 0)
      {
        this.txtDetails.Lines = new string[1]
        {
          "No event details available."
        };
      }
      else
      {
        try
        {
          ContactNote contactNote = Session.ContactManager.GetContactNote(item.LoanID, ContactType.Borrower);
          if (contactNote != null)
            this.txtDetails.Lines = new List<string>()
            {
              "Fax Received Date: " + item.Timestamp.ToString("d", (IFormatProvider) null),
              "Fax Sent By: " + item.Sender,
              "Subject: " + contactNote.Subject,
              "Details: " + contactNote.Details
            }.ToArray();
          else
            this.txtDetails.Lines = new string[1]
            {
              "No event details available."
            };
        }
        catch
        {
          this.txtDetails.Lines = new string[1]
          {
            "No event details available."
          };
        }
      }
    }

    private void showFirstContactHistory(ContactHistoryItem item)
    {
      this.txtDetails.Lines = new List<string>()
      {
        "Data Source: " + ContactSourceEnumUtil.ValueToName(item.Source),
        "Creation Date: " + item.Timestamp.ToString("d", (IFormatProvider) null)
      }.ToArray();
    }

    private void showCampaignActivityHistory(ContactHistoryItem item)
    {
      List<string> stringList = new List<string>();
      stringList.Add("Campaign Owner: " + item.Sender);
      stringList.Add("Campaign Name: " + item.CampaignName);
      if (item.CampaignStepNumber != 0)
      {
        stringList.Add("Campaign Step: " + item.CampaignStepName);
        if ("Campaign Email" == item.EventType || "Campaign Letter" == item.EventType)
        {
          stringList.Add("Document Name: " + this.stripPath(item.LetterName));
          if ("Campaign Email" == item.EventType)
            stringList.Add("Subject: " + item.Subject);
        }
        stringList.Add("Status: " + item.CampaignActivityStatus);
        stringList.Add("Scheduled Date: " + item.CampaignScheduledDate.ToShortDateString());
        stringList.Add("Completion Date: " + item.Timestamp.ToShortDateString());
      }
      else if (item.EventType.ToLower() == "campaign complete")
        stringList.Add("Completion Date: " + item.Timestamp.ToShortDateString());
      else if (item.EventType.ToLower() == "campaign add")
        stringList.Add("Added Date: " + item.Timestamp.ToShortDateString());
      else if (item.EventType.ToLower() == "campaign delete")
        stringList.Add("Deleted Date: " + item.Timestamp.ToShortDateString());
      this.txtDetails.Lines = stringList.ToArray();
    }

    private string stripPath(string fileName)
    {
      int startIndex = fileName.LastIndexOf("\\") + 1;
      return startIndex != 0 ? fileName.Substring(startIndex) : fileName;
    }

    private void showHistory(ContactHistoryItem item)
    {
      if (!this.IsReadOnly)
        this.btnDelete.Enabled = true;
      switch (item.EventType)
      {
        case "Called":
          this.showCallHistory(item);
          break;
        case "Campaign Add":
        case "Campaign Complete":
        case "Campaign Delete":
        case "Campaign Email":
        case "Campaign Fax":
        case "Campaign Insert":
        case "Campaign Letter":
        case "Campaign Phone Call":
        case "Campaign Reminder":
        case "Campaign Remove":
          this.showCampaignActivityHistory(item);
          break;
        case "Completed Loan":
          this.showClosedLoanHistory(item);
          break;
        case "Credit Received":
          this.showCreditScoresHistory(item);
          break;
        case "Email Merge":
          this.showEmailMergeHistory(item);
          break;
        case "Emailed":
          this.showEmailHistory(item);
          break;
        case "Faxed":
          this.showFaxHistory(item);
          break;
        case "First Contact":
          this.showFirstContactHistory(item);
          break;
        case "Loan Origination":
          this.showLoanOriginationHistory(item);
          break;
        case "Mail Merge":
          this.showMailMergeHistory(item);
          break;
        default:
          this.linkLblMailMerge.Visible = false;
          this.txtDetails.Lines = new string[1]
          {
            "No event details available."
          };
          break;
      }
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
      if (this.lvwHistory.SelectedItems.Count > 0)
      {
        if (DialogResult.Cancel == MessageBox.Show("Are you sure you want to delete the selected history?", "Delete History", MessageBoxButtons.OKCancel))
          return;
        Session.ContactManager.DeleteHistoryItemForContact(this.currentContactId, ContactType.Borrower, ((ContactHistoryItem) this.lvwHistory.SelectedItems[0].Tag).HistoryItemID);
        this.lvwHistory.Items.Remove(this.lvwHistory.SelectedItems[0]);
      }
      this.gcEvent.Text = "Events (" + (object) this.lvwHistory.Items.Count + ")";
    }

    private void linkLblMailMerge_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
    }
  }
}
