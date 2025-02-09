// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.UpdateEmailAddress
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class UpdateEmailAddress : Form
  {
    private LoanData loanData;
    private IContainer components;
    private Label lblInfo;
    private Button btnUpdate;
    private Button btnSkip;
    private GridView gvContacts;
    private Label lblEmail;
    private TextBox txtEmail;

    public UpdateEmailAddress(LoanData loanData)
    {
      this.InitializeComponent();
      this.loanData = loanData;
    }

    public void CheckEmailRecipients(string emailList)
    {
      List<string> stringList = new List<string>();
      string str1 = emailList;
      char[] chArray = new char[1]{ ';' };
      foreach (string str2 in str1.Split(chArray))
      {
        string str3 = str2.Trim();
        if (str3 != string.Empty && !stringList.Contains(str3))
          stringList.Add(str3);
      }
      foreach (string strB in stringList)
      {
        this.loadContactList();
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvContacts.Items)
        {
          if (string.Compare(gvItem.SubItems[1].Text, strB, StringComparison.OrdinalIgnoreCase) == 0)
            return;
        }
        this.txtEmail.Text = strB;
        int num = (int) this.ShowDialog((IWin32Window) Form.ActiveForm);
      }
    }

    public void CheckEmailRecipients(string to, string cc)
    {
      if (!string.IsNullOrEmpty(cc))
        this.CheckEmailRecipients(to + ";" + cc);
      else
        this.CheckEmailRecipients(to);
    }

    public void loadContactList()
    {
      this.gvContacts.Items.Clear();
      for (int index = 1; index <= 34; ++index)
      {
        string id1 = (string) null;
        string id2 = (string) null;
        string id3 = (string) null;
        switch (index)
        {
          case 1:
            id1 = "1240";
            id2 = "Borrower";
            id3 = "36";
            break;
          case 2:
            id1 = "1178";
            id2 = "Borrower";
            id3 = "36";
            break;
          case 3:
            id1 = "1268";
            id2 = "Coborrower";
            id3 = "68";
            break;
          case 4:
            id1 = "1179";
            id2 = "Coborrower";
            id3 = "68";
            break;
          case 5:
            id1 = "1407";
            id2 = "Loan Officer";
            id3 = "317";
            break;
          case 6:
            id1 = "1409";
            id2 = "Loan Processor";
            id3 = "362";
            break;
          case 7:
            id1 = "95";
            id2 = "Lender";
            id3 = "1256";
            break;
          case 8:
            id1 = "89";
            id2 = "Appraiser";
            id3 = "618";
            break;
          case 9:
            id1 = "87";
            id2 = "Escrow Company";
            id3 = "611";
            break;
          case 10:
            id1 = "88";
            id2 = "Title Insurance";
            id3 = "416";
            break;
          case 11:
            id1 = "VEND.X119";
            id2 = "Buyer's Attorney";
            id3 = "VEND.X117";
            break;
          case 12:
            id1 = "VEND.X130";
            id2 = "Seller's Attorney";
            id3 = "VEND.X128";
            break;
          case 13:
            id1 = "VEND.X141";
            id2 = "Buyer's Agent";
            id3 = "VEND.X139";
            break;
          case 14:
            id1 = "VEND.X152";
            id2 = "Seller's Agent";
            id3 = "VEND.X150";
            break;
          case 15:
            id1 = "92";
            id2 = "Seller";
            id3 = "638";
            break;
          case 16:
            id1 = "94";
            id2 = "Builder";
            id3 = "714";
            break;
          case 17:
            id1 = "VEND.X164";
            id2 = "Hazard Insurance";
            id3 = "VEND.X162";
            break;
          case 18:
            id1 = "93";
            id2 = "Mortgage Insurance";
            id3 = "707";
            break;
          case 19:
            id1 = "VEND.X43";
            id2 = "Surveyor";
            id3 = "VEND.X35";
            break;
          case 20:
            id1 = "VEND.X21";
            id2 = "Flood Insurance";
            id3 = "VEND.X13";
            break;
          case 21:
            id1 = "90";
            id2 = "Credit Company";
            id3 = "625";
            break;
          case 22:
            id1 = "1411";
            id2 = "Underwriter";
            id3 = "984";
            break;
          case 23:
            id1 = "VEND.X186";
            id2 = "Servicing";
            id3 = "VEND.X184";
            break;
          case 24:
            id1 = "VEND.X197";
            id2 = "Doc Signing";
            id3 = "VEND.X195";
            break;
          case 25:
            id1 = "VEND.X208";
            id2 = "Warehouse";
            id3 = "VEND.X206";
            break;
          case 26:
            id1 = "VEND.X53";
            id2 = "Financial Planner";
            id3 = "VEND.X45";
            break;
          case 27:
            id1 = "VEND.X273";
            id2 = "Investor";
            id3 = "VEND.X271";
            break;
          case 28:
            id1 = "VEND.X288";
            id2 = "Assign To";
            id3 = "VEND.X286";
            break;
          case 29:
            id1 = "VEND.X305";
            id2 = "Broker";
            id3 = "VEND.X302";
            break;
          case 30:
            id1 = "VEND.X319";
            id2 = "Docs Prepared By";
            id3 = "VEND.X317";
            break;
          case 31:
            id1 = "VEND.X63";
            id2 = "VEND.X84";
            id3 = "VEND.X55";
            break;
          case 32:
            id1 = "VEND.X73";
            id2 = "VEND.X85";
            id3 = "VEND.X65";
            break;
          case 33:
            id1 = "VEND.X83";
            id2 = "VEND.X86";
            id3 = "VEND.X75";
            break;
          case 34:
            id1 = "VEND.X10";
            id2 = "VEND.X11";
            id3 = "VEND.X2";
            break;
        }
        string itemText = this.loanData.GetSimpleField(id3);
        if (index == 1)
          itemText = itemText + " " + this.loanData.GetSimpleField("37");
        else if (index == 2)
          itemText = itemText + " " + this.loanData.GetSimpleField("69");
        if (!string.IsNullOrEmpty(itemText.Trim()))
        {
          string simpleField = this.loanData.GetSimpleField(id1);
          if (index >= 29)
          {
            id2 = this.loanData.GetSimpleField(id2);
            if (id2 == string.Empty || id2 == null)
              id2 = "Custom Category #" + Convert.ToString(index - 24);
          }
          GVItem gvItem = this.gvContacts.Items.Add(itemText);
          gvItem.SubItems.Add((object) simpleField);
          gvItem.SubItems.Add((object) id2);
          gvItem.Tag = (object) id1;
        }
      }
    }

    private void gvContacts_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnUpdate.Enabled = this.gvContacts.SelectedItems.Count > 0;
    }

    private void btnUpdate_Click(object sender, EventArgs e)
    {
      string tag = (string) this.gvContacts.SelectedItems[0].Tag;
      if (this.loanData.IsFieldReadOnly(tag))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You cannot update the email address because the field is read-only.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        this.loanData.SetField(tag, this.txtEmail.Text);
        Session.Application.GetService<ILoanEditor>().RefreshContents(tag);
        this.DialogResult = DialogResult.OK;
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
      GVColumn gvColumn3 = new GVColumn();
      this.lblInfo = new Label();
      this.btnUpdate = new Button();
      this.btnSkip = new Button();
      this.gvContacts = new GridView();
      this.lblEmail = new Label();
      this.txtEmail = new TextBox();
      this.SuspendLayout();
      this.lblInfo.Location = new Point(8, 8);
      this.lblInfo.Name = "lblInfo";
      this.lblInfo.Size = new Size(580, 32);
      this.lblInfo.TabIndex = 0;
      this.lblInfo.Text = "Select the File Contact associated with the following email address.  If this email address does not apply to any of the contacts listed below, click the Skip button.";
      this.btnUpdate.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnUpdate.Enabled = false;
      this.btnUpdate.Location = new Point(438, 350);
      this.btnUpdate.Name = "btnUpdate";
      this.btnUpdate.Size = new Size(75, 22);
      this.btnUpdate.TabIndex = 4;
      this.btnUpdate.Text = "Update";
      this.btnUpdate.UseVisualStyleBackColor = true;
      this.btnUpdate.Click += new EventHandler(this.btnUpdate_Click);
      this.btnSkip.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnSkip.DialogResult = DialogResult.Cancel;
      this.btnSkip.Location = new Point(514, 350);
      this.btnSkip.Name = "btnSkip";
      this.btnSkip.Size = new Size(75, 22);
      this.btnSkip.TabIndex = 5;
      this.btnSkip.Text = "Skip";
      this.btnSkip.UseVisualStyleBackColor = true;
      this.gvContacts.AllowMultiselect = false;
      this.gvContacts.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "colName";
      gvColumn1.Text = "Contact Name";
      gvColumn1.Width = 120;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "colEmail";
      gvColumn2.Text = "Email Address";
      gvColumn2.Width = 260;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "colCategory";
      gvColumn3.SpringToFit = true;
      gvColumn3.Text = "Category";
      gvColumn3.Width = 198;
      this.gvContacts.Columns.AddRange(new GVColumn[3]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3
      });
      this.gvContacts.Location = new Point(8, 72);
      this.gvContacts.Name = "gvContacts";
      this.gvContacts.Size = new Size(580, 270);
      this.gvContacts.TabIndex = 3;
      this.gvContacts.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gvContacts.SelectedIndexChanged += new EventHandler(this.gvContacts_SelectedIndexChanged);
      this.lblEmail.AutoSize = true;
      this.lblEmail.Location = new Point(8, 47);
      this.lblEmail.Name = "lblEmail";
      this.lblEmail.Size = new Size(76, 14);
      this.lblEmail.TabIndex = 1;
      this.lblEmail.Text = "Email Address";
      this.txtEmail.Location = new Point(84, 44);
      this.txtEmail.Name = "txtEmail";
      this.txtEmail.ReadOnly = true;
      this.txtEmail.Size = new Size(504, 20);
      this.txtEmail.TabIndex = 2;
      this.txtEmail.TabStop = false;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnSkip;
      this.ClientSize = new Size(598, 381);
      this.Controls.Add((Control) this.txtEmail);
      this.Controls.Add((Control) this.lblEmail);
      this.Controls.Add((Control) this.gvContacts);
      this.Controls.Add((Control) this.btnSkip);
      this.Controls.Add((Control) this.btnUpdate);
      this.Controls.Add((Control) this.lblInfo);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (UpdateEmailAddress);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Update Contact Info";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
