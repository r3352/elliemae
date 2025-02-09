// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ePass.EmailListDialog
// Assembly: EMePass, Version=3.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A610697F-A1EC-4CC3-A30A-403E37B2B276
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMePass.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ePass
{
  [ComVisible(false)]
  public class EmailListDialog : Form, IHelp
  {
    private const string className = "EmailListDialog";
    private static readonly string sw = Tracing.SwEpass;
    private ColumnHeader contactHdr;
    private ColumnHeader emailHdr;
    private ColumnHeader categoryHdr;
    private Button okBtn;
    private Button cancelBtn;
    private ListView emailLvw;
    private System.ComponentModel.Container components;
    private ListViewSortManager sortMngr;
    private bool includeAllPairs;
    public List<string> selectedContacts = new List<string>();
    public List<NonBorrowingOwner> selectedNbo = new List<NonBorrowingOwner>();
    private string emailList = string.Empty;

    public EmailListDialog(LoanData loanData, bool includeAllPairs = false)
    {
      this.includeAllPairs = includeAllPairs;
      this.InitializeComponent();
      this.initializeEmailList(loanData);
      this.sortMngr = new ListViewSortManager(this.emailLvw, new System.Type[3]
      {
        typeof (ListViewTextSort),
        typeof (ListViewTextSort),
        typeof (ListViewTextSort)
      });
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.okBtn = new Button();
      this.cancelBtn = new Button();
      this.emailLvw = new ListView();
      this.contactHdr = new ColumnHeader();
      this.emailHdr = new ColumnHeader();
      this.categoryHdr = new ColumnHeader();
      this.SuspendLayout();
      this.okBtn.Anchor = AnchorStyles.Bottom;
      this.okBtn.Location = new Point(170, 280);
      this.okBtn.Name = "okBtn";
      this.okBtn.Size = new Size(84, 24);
      this.okBtn.TabIndex = 2;
      this.okBtn.Text = "&Select";
      this.okBtn.Click += new EventHandler(this.okBtn_Click);
      this.cancelBtn.Anchor = AnchorStyles.Bottom;
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(262, 280);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(84, 24);
      this.cancelBtn.TabIndex = 3;
      this.cancelBtn.Text = "&Cancel";
      this.emailLvw.Columns.AddRange(new ColumnHeader[3]
      {
        this.contactHdr,
        this.emailHdr,
        this.categoryHdr
      });
      this.emailLvw.FullRowSelect = true;
      this.emailLvw.HideSelection = false;
      this.emailLvw.Location = new Point(12, 12);
      this.emailLvw.Name = "emailLvw";
      this.emailLvw.Size = new Size(500, 256);
      this.emailLvw.Sorting = SortOrder.Ascending;
      this.emailLvw.TabIndex = 1;
      this.emailLvw.UseCompatibleStateImageBehavior = false;
      this.emailLvw.View = View.Details;
      this.emailLvw.DoubleClick += new EventHandler(this.emailLvw_DoubleClick);
      this.contactHdr.Text = "Contact Name";
      this.contactHdr.Width = 120;
      this.emailHdr.Text = "Email Address";
      this.emailHdr.Width = 260;
      this.categoryHdr.Text = "Category";
      this.categoryHdr.Width = 100;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.CancelButton = (IButtonControl) this.cancelBtn;
      this.ClientSize = new Size(526, 323);
      this.Controls.Add((Control) this.emailLvw);
      this.Controls.Add((Control) this.cancelBtn);
      this.Controls.Add((Control) this.okBtn);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (EmailListDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "File Contacts";
      this.KeyDown += new KeyEventHandler(this.Help_KeyDown);
      this.ResumeLayout(false);
    }

    public string EmailList => this.emailList;

    private void initializeEmailList(LoanData loanData)
    {
      if (this.includeAllPairs)
      {
        int pairIndex1 = loanData.GetPairIndex(loanData.PairId);
        int pairIndex2 = -1;
        BorrowerPair[] borrowerPairs = loanData.GetBorrowerPairs();
        try
        {
          foreach (BorrowerPair borrowerPair in borrowerPairs)
          {
            pairIndex2 = loanData.GetPairIndex(borrowerPair.Id);
            loanData.SetBorrowerPair(pairIndex2);
            for (int index = 1; index <= 4; ++index)
            {
              string id1 = (string) null;
              string str = (string) null;
              string id2 = (string) null;
              switch (index)
              {
                case 1:
                  id1 = "1240";
                  str = "Borrower";
                  id2 = "36";
                  break;
                case 2:
                  id1 = "1178";
                  str = "Borrower";
                  id2 = "36";
                  break;
                case 3:
                  id1 = "1268";
                  str = "Coborrower";
                  id2 = "68";
                  break;
                case 4:
                  id1 = "1179";
                  str = "Coborrower";
                  id2 = "68";
                  break;
              }
              string simpleField = loanData.GetSimpleField(id1);
              if (!string.IsNullOrEmpty(simpleField))
              {
                string text = loanData.GetSimpleField(id2);
                switch (index)
                {
                  case 1:
                    text = text + " " + loanData.GetSimpleField("37");
                    break;
                  case 3:
                    text = text + " " + loanData.GetSimpleField("69");
                    break;
                }
                this.emailLvw.Items.Add(new ListViewItem(text)
                {
                  SubItems = {
                    simpleField,
                    str
                  }
                });
              }
            }
          }
        }
        finally
        {
          if (pairIndex1 != pairIndex2)
            loanData.SetBorrowerPair(pairIndex1);
        }
      }
      for (int index = this.includeAllPairs ? 5 : 1; index <= 34; ++index)
      {
        string id3 = (string) null;
        string id4 = (string) null;
        string id5 = (string) null;
        switch (index)
        {
          case 1:
            id3 = "1240";
            id4 = "Borrower";
            id5 = "36";
            break;
          case 2:
            id3 = "1178";
            id4 = "Borrower";
            id5 = "36";
            break;
          case 3:
            id3 = "1268";
            id4 = "Coborrower";
            id5 = "68";
            break;
          case 4:
            id3 = "1179";
            id4 = "Coborrower";
            id5 = "68";
            break;
          case 5:
            id3 = "1407";
            id4 = "Loan Officer";
            id5 = "317";
            break;
          case 6:
            id3 = "1409";
            id4 = "Loan Processor";
            id5 = "362";
            break;
          case 7:
            id3 = "95";
            id4 = "Lender";
            id5 = "1256";
            break;
          case 8:
            id3 = "89";
            id4 = "Appraiser";
            id5 = "618";
            break;
          case 9:
            id3 = "87";
            id4 = "Escrow Company";
            id5 = "611";
            break;
          case 10:
            id3 = "88";
            id4 = "Title Insurance";
            id5 = "416";
            break;
          case 11:
            id3 = "VEND.X119";
            id4 = "Buyer's Attorney";
            id5 = "VEND.X117";
            break;
          case 12:
            id3 = "VEND.X130";
            id4 = "Seller's Attorney";
            id5 = "VEND.X128";
            break;
          case 13:
            id3 = "VEND.X141";
            id4 = "Buyer's Agent";
            id5 = "VEND.X139";
            break;
          case 14:
            id3 = "VEND.X152";
            id4 = "Seller's Agent";
            id5 = "VEND.X150";
            break;
          case 15:
            id3 = "92";
            id4 = "Seller";
            id5 = "638";
            break;
          case 16:
            id3 = "94";
            id4 = "Builder";
            id5 = "714";
            break;
          case 17:
            id3 = "VEND.X164";
            id4 = "Hazard Insurance";
            id5 = "VEND.X162";
            break;
          case 18:
            id3 = "93";
            id4 = "Mortgage Insurance";
            id5 = "707";
            break;
          case 19:
            id3 = "VEND.X43";
            id4 = "Surveyor";
            id5 = "VEND.X35";
            break;
          case 20:
            id3 = "VEND.X21";
            id4 = "Flood Insurance";
            id5 = "VEND.X13";
            break;
          case 21:
            id3 = "90";
            id4 = "Credit Company";
            id5 = "625";
            break;
          case 22:
            id3 = "1411";
            id4 = "Underwriter";
            id5 = "984";
            break;
          case 23:
            id3 = "VEND.X186";
            id4 = "Servicing";
            id5 = "VEND.X184";
            break;
          case 24:
            id3 = "VEND.X197";
            id4 = "Doc Signing";
            id5 = "VEND.X195";
            break;
          case 25:
            id3 = "VEND.X208";
            id4 = "Warehouse";
            id5 = "VEND.X206";
            break;
          case 26:
            id3 = "VEND.X53";
            id4 = "Financial Planner";
            id5 = "VEND.X45";
            break;
          case 27:
            id3 = "VEND.X273";
            id4 = "Investor";
            id5 = "VEND.X271";
            break;
          case 28:
            id3 = "VEND.X288";
            id4 = "Assign To";
            id5 = "VEND.X286";
            break;
          case 29:
            id3 = "VEND.X305";
            id4 = "Broker";
            id5 = "VEND.X302";
            break;
          case 30:
            id3 = "VEND.X319";
            id4 = "Docs Prepared By";
            id5 = "VEND.X317";
            break;
          case 31:
            id3 = "VEND.X63";
            id4 = "VEND.X84";
            id5 = "VEND.X55";
            break;
          case 32:
            id3 = "VEND.X73";
            id4 = "VEND.X85";
            id5 = "VEND.X65";
            break;
          case 33:
            id3 = "VEND.X83";
            id4 = "VEND.X86";
            id5 = "VEND.X75";
            break;
          case 34:
            id3 = "VEND.X10";
            id4 = "VEND.X11";
            id5 = "VEND.X2";
            break;
        }
        string simpleField = loanData.GetSimpleField(id3);
        if (!string.IsNullOrEmpty(simpleField))
        {
          string text = loanData.GetSimpleField(id5);
          if (index == 1)
            text = text + " " + loanData.GetSimpleField("37");
          else if (index == 3)
            text = text + " " + loanData.GetSimpleField("69");
          if (index >= 29)
          {
            id4 = loanData.GetSimpleField(id4);
            if (string.IsNullOrEmpty(id4))
              id4 = "Custom Category #" + Convert.ToString(index - 24);
          }
          this.emailLvw.Items.Add(new ListViewItem(text)
          {
            SubItems = {
              simpleField,
              id4
            }
          });
        }
      }
      foreach (NonBorrowingOwner nonBorrowingOwner in this.includeAllPairs ? loanData.GetNboByBorrowerPairId("") : loanData.GetNboByBorrowerPairId(loanData.CurrentBorrowerPair.Id))
      {
        if (!string.IsNullOrWhiteSpace(nonBorrowingOwner.LastName) && !string.IsNullOrWhiteSpace(nonBorrowingOwner.FirstName) && !string.IsNullOrWhiteSpace(nonBorrowingOwner.EmailAddress))
          this.emailLvw.Items.Add(new ListViewItem(string.Format("{1} {0}", (object) nonBorrowingOwner.LastName, (object) nonBorrowingOwner.FirstName))
          {
            SubItems = {
              nonBorrowingOwner.EmailAddress,
              "Non-Borrowing Owner"
            },
            Tag = (object) nonBorrowingOwner
          });
      }
    }

    private void okBtn_Click(object sender, EventArgs e)
    {
      if (this.emailLvw.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select at least one e-mail.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        foreach (ListViewItem selectedItem in this.emailLvw.SelectedItems)
        {
          if (selectedItem.Tag is NonBorrowingOwner)
            this.selectedNbo.Add(selectedItem.Tag as NonBorrowingOwner);
          else
            this.selectedContacts.Add(string.Join(";", selectedItem.SubItems[0].Text, selectedItem.SubItems[1].Text, selectedItem.SubItems[2].Text));
          string text = selectedItem.SubItems[1].Text;
          this.emailList = !(this.emailList == string.Empty) ? this.emailList + "; " + text : text;
        }
        this.DialogResult = DialogResult.OK;
      }
    }

    private void emailLvw_DoubleClick(object sender, EventArgs e)
    {
      if (this.emailLvw.SelectedItems.Count == 0)
        return;
      this.emailList = this.emailLvw.SelectedItems[0].SubItems[1].Text;
      this.DialogResult = DialogResult.OK;
    }

    private void Help_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F1)
        return;
      this.ShowHelp();
    }

    public void ShowHelp()
    {
      JedHelp.ShowHelp((Control) this, SystemSettings.HelpFile, nameof (EmailListDialog));
    }
  }
}
