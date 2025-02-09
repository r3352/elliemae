// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.InvestorAddressEditor
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.RemotingServices;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class InvestorAddressEditor : UserControl
  {
    private Investor currentInvestor;
    private ContactInformation currentDealer;
    private ContactInformation currentAssignee;
    private bool readOnly;
    private Sessions.Session session;
    private IContainer components;
    private TabControl tabsMain;
    private TabPage tabPage1;
    private TabPage tabPage3;
    private TabPage tabPage2;
    private TabPage tabPage4;
    private ContactInfoEditor ciContact;
    private ContactInfoEditor ciShipping;
    private ContactInfoEditor ciServicing;
    private ContactInfoEditor ciDocs;
    private TabPage tpDealer;
    private ContactInfoEditor ciAssignee;
    private TabPage tpAssignee;
    private ContactInfoEditor ciDealer;
    private TabPage tpPayment;
    private ContactInfoEditor ciPayment;
    private TabPage tpInsurance;
    private ContactInfoEditor ciInsurance;
    private TabPage tpNoteDelivery;
    private ContactInfoEditor ciNoteDelivery;
    private TabPage tpTaxNotice;
    private ContactInfoEditor ciTaxNotice;
    private TabPage tpMortgageInsurance;
    private TabPage tpLoanDelivery;
    private TabPage tpAssignment;
    private TabPage tpCorrespondence;
    private ContactInfoEditor ciMortgageInsurance;
    private ContactInfoEditor ciLoanDelivery;
    private ContactInfoEditor ciAssignment;
    private ContactInfoEditor ciCorrespondence;
    private TabPage tpGeneric1;
    private TabPage tpGeneric2;
    private TabPage tpGeneric3;
    private TabPage tpGeneric4;
    private ContactInfoEditor ciGeneric1;
    private ContactInfoEditor ciGeneric2;
    private ContactInfoEditor ciGeneric3;
    private ContactInfoEditor ciGeneric4;

    public event EventHandler DataChange;

    public event InvestorAddressEditor.AssigneeNameChangeEventHandler AssigneeNameChanged;

    public InvestorAddressEditor()
      : this(Session.DefaultInstance)
    {
    }

    public InvestorAddressEditor(Sessions.Session session, LayoutOrientation orientation = LayoutOrientation.portrait)
      : this(session, false)
    {
      if (orientation != LayoutOrientation.landscape)
        return;
      this.tabsMain.Size = new Size(715, 164);
      foreach (TabPage control1 in (ArrangedElementCollection) this.tabsMain.Controls)
      {
        control1.Size = new Size(707, 136);
        foreach (UserControl control2 in (ArrangedElementCollection) control1.Controls)
        {
          if (control2 is ContactInfoEditor)
          {
            control2.Size = new Size(707, 125);
            (control2 as ContactInfoEditor).ResizeToLandscape();
          }
        }
      }
    }

    public InvestorAddressEditor(Sessions.Session session, bool showAssigneeTab)
    {
      this.session = session;
      this.ciContact = new ContactInfoEditor(this.session);
      this.ciShipping = new ContactInfoEditor(this.session);
      this.ciAssignee = new ContactInfoEditor(this.session);
      this.ciServicing = new ContactInfoEditor(this.session);
      this.ciDocs = new ContactInfoEditor(this.session);
      this.ciDealer = new ContactInfoEditor(this.session);
      this.ciPayment = new ContactInfoEditor(this.session);
      this.ciInsurance = new ContactInfoEditor(this.session);
      this.ciNoteDelivery = new ContactInfoEditor(this.session);
      this.ciTaxNotice = new ContactInfoEditor(this.session);
      this.ciMortgageInsurance = new ContactInfoEditor(this.session);
      this.ciLoanDelivery = new ContactInfoEditor(this.session);
      this.ciAssignment = new ContactInfoEditor(this.session);
      this.ciCorrespondence = new ContactInfoEditor(this.session);
      this.ciGeneric1 = new ContactInfoEditor(this.session);
      this.ciGeneric2 = new ContactInfoEditor(this.session);
      this.ciGeneric3 = new ContactInfoEditor(this.session);
      this.ciGeneric4 = new ContactInfoEditor(this.session);
      this.InitializeComponent();
      if (!showAssigneeTab)
        this.tabsMain.TabPages.Remove(this.tpAssignee);
      this.tabsMain.TabPages.Remove(this.tpDealer);
      this.ciAssignee.EntityNameTextChanged += new ContactInfoEditor.EntityNameTextChangeEventHandler(this.ciAssignee_EntityNameTextChanged);
    }

    private void ciAssignee_EntityNameTextChanged(object sender, string newName)
    {
      if (this.AssigneeNameChanged == null)
        return;
      this.AssigneeNameChanged((object) this, newName);
    }

    public Investor CurrentInvestor
    {
      get => this.currentInvestor;
      set
      {
        this.currentInvestor = value;
        if (value == null)
          return;
        this.loadCurrentInvestor();
      }
    }

    public ContactInformation CurrentDealer
    {
      get => this.currentDealer;
      set
      {
        this.currentDealer = value;
        if (value == null)
          return;
        this.loadCurrentDealer();
      }
    }

    public ContactInformation CurrentAssignee
    {
      get => this.currentAssignee;
      set
      {
        this.currentAssignee = value;
        if (value == null)
          return;
        this.loadCurrentAssignee();
      }
    }

    public void SetAssigneeTextBoxValues(ContactInformation contactInfo)
    {
      if (this.currentAssignee == null)
        return;
      this.ciAssignee.SetTextBoxValues(contactInfo);
    }

    public void Clear()
    {
      foreach (Control control1 in (ArrangedElementCollection) this.tabsMain.Controls)
      {
        foreach (UserControl control2 in (ArrangedElementCollection) control1.Controls)
        {
          if (control2 is ContactInfoEditor && (control2.Name == "ciDealer" && this.currentDealer != null || control2.Name == "ciAssignee" && this.currentAssignee != null || control2.Name != "ciDealer" && control2.Name != "ciAssignee"))
            (control2 as ContactInfoEditor).Clear();
        }
      }
    }

    private void loadCurrentInvestor()
    {
      this.Clear();
      foreach (Control control1 in (ArrangedElementCollection) this.tabsMain.Controls)
      {
        foreach (UserControl control2 in (ArrangedElementCollection) control1.Controls)
        {
          if (control2 is ContactInfoEditor && !control2.Name.Equals("ciAssignee") && !control2.Name.Equals("ciDealer"))
            (control2 as ContactInfoEditor).ContactInformation = this.currentInvestor.GetContactInformation(control2.Name.Substring(2));
        }
      }
    }

    private void loadCurrentDealer()
    {
      this.ciDealer.ContactInformation = this.currentDealer;
      if (this.tabsMain.TabPages.Contains(this.tpDealer))
        return;
      this.tabsMain.TabPages.Add(this.tpDealer);
      this.tpDealer.Padding = new Padding(0, 9, 0, 3);
    }

    private void loadCurrentAssignee() => this.ciAssignee.ContactInformation = this.currentAssignee;

    public void CommitChanges()
    {
      foreach (Control control1 in (ArrangedElementCollection) this.tabsMain.Controls)
      {
        foreach (UserControl control2 in (ArrangedElementCollection) control1.Controls)
        {
          if (control2 is ContactInfoEditor && (control2.Name == "ciDealer" && this.currentDealer != null || control2.Name == "ciAssignee" && this.currentAssignee != null || control2.Name != "ciDealer" && control2.Name != "ciAssignee"))
            (control2 as ContactInfoEditor).CommitChanges();
        }
      }
    }

    public bool DataModified
    {
      get
      {
        bool flag = false;
        foreach (Control control1 in (ArrangedElementCollection) this.tabsMain.Controls)
        {
          foreach (UserControl control2 in (ArrangedElementCollection) control1.Controls)
          {
            if (control2 is ContactInfoEditor && (control2 as ContactInfoEditor).DataModified)
            {
              flag = true;
              break;
            }
          }
        }
        if (flag || this.currentDealer != null && this.ciDealer.DataModified)
          return true;
        return this.currentAssignee != null && this.ciAssignee.DataModified;
      }
    }

    public bool ReadOnly
    {
      get => this.readOnly;
      set
      {
        if (value == this.readOnly)
          return;
        this.readOnly = value;
        this.setReadOnly();
      }
    }

    private void setReadOnly()
    {
      foreach (Control control1 in (ArrangedElementCollection) this.tabsMain.Controls)
      {
        foreach (UserControl control2 in (ArrangedElementCollection) control1.Controls)
        {
          if (control2 is ContactInfoEditor)
            (control2 as ContactInfoEditor).ReadOnly = this.readOnly;
        }
      }
    }

    private void onContactDataChange(object sender, EventArgs e)
    {
      if (this.DataChange == null)
        return;
      this.DataChange((object) this, EventArgs.Empty);
    }

    public string AssigneeNameTextBoxText
    {
      get => this.ciAssignee.EntityNameTextBoxText;
      set => this.ciAssignee.EntityNameTextBoxText = value;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.tabsMain = new TabControl();
      this.tabPage1 = new TabPage();
      this.tabPage3 = new TabPage();
      this.tpAssignee = new TabPage();
      this.tabPage2 = new TabPage();
      this.tabPage4 = new TabPage();
      this.tpDealer = new TabPage();
      this.tpPayment = new TabPage();
      this.tpInsurance = new TabPage();
      this.tpNoteDelivery = new TabPage();
      this.tpTaxNotice = new TabPage();
      this.tpMortgageInsurance = new TabPage();
      this.tpLoanDelivery = new TabPage();
      this.tpAssignment = new TabPage();
      this.tpCorrespondence = new TabPage();
      this.tpGeneric1 = new TabPage();
      this.tpGeneric2 = new TabPage();
      this.tpGeneric3 = new TabPage();
      this.tpGeneric4 = new TabPage();
      this.tabsMain.SuspendLayout();
      this.tabPage1.SuspendLayout();
      this.tabPage3.SuspendLayout();
      this.tpAssignee.SuspendLayout();
      this.tabPage2.SuspendLayout();
      this.tabPage4.SuspendLayout();
      this.tpDealer.SuspendLayout();
      this.tpPayment.SuspendLayout();
      this.tpInsurance.SuspendLayout();
      this.tpNoteDelivery.SuspendLayout();
      this.tpTaxNotice.SuspendLayout();
      this.tpMortgageInsurance.SuspendLayout();
      this.tpLoanDelivery.SuspendLayout();
      this.tpAssignment.SuspendLayout();
      this.tpCorrespondence.SuspendLayout();
      this.tpGeneric1.SuspendLayout();
      this.tpGeneric2.SuspendLayout();
      this.tpGeneric3.SuspendLayout();
      this.tpGeneric4.SuspendLayout();
      this.SuspendLayout();
      this.tabsMain.Controls.Add((Control) this.tabPage1);
      this.tabsMain.Controls.Add((Control) this.tabPage3);
      this.tabsMain.Controls.Add((Control) this.tpAssignee);
      this.tabsMain.Controls.Add((Control) this.tabPage2);
      this.tabsMain.Controls.Add((Control) this.tabPage4);
      this.tabsMain.Controls.Add((Control) this.tpDealer);
      this.tabsMain.Controls.Add((Control) this.tpPayment);
      this.tabsMain.Controls.Add((Control) this.tpInsurance);
      this.tabsMain.Controls.Add((Control) this.tpNoteDelivery);
      this.tabsMain.Controls.Add((Control) this.tpTaxNotice);
      this.tabsMain.Controls.Add((Control) this.tpMortgageInsurance);
      this.tabsMain.Controls.Add((Control) this.tpLoanDelivery);
      this.tabsMain.Controls.Add((Control) this.tpAssignment);
      this.tabsMain.Controls.Add((Control) this.tpCorrespondence);
      this.tabsMain.Controls.Add((Control) this.tpGeneric1);
      this.tabsMain.Controls.Add((Control) this.tpGeneric2);
      this.tabsMain.Controls.Add((Control) this.tpGeneric3);
      this.tabsMain.Controls.Add((Control) this.tpGeneric4);
      this.tabsMain.Dock = DockStyle.Fill;
      this.tabsMain.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.tabsMain.ItemSize = new Size(73, 20);
      this.tabsMain.Location = new Point(0, 0);
      this.tabsMain.Name = "tabsMain";
      this.tabsMain.Padding = new Point(11, 3);
      this.tabsMain.SelectedIndex = 0;
      this.tabsMain.Size = new Size(349, 333);
      this.tabsMain.TabIndex = 0;
      this.tabPage1.Controls.Add((Control) this.ciContact);
      this.tabPage1.Location = new Point(4, 24);
      this.tabPage1.Name = "tabPage1";
      this.tabPage1.Padding = new Padding(0, 8, 0, 3);
      this.tabPage1.Size = new Size(341, 305);
      this.tabPage1.TabIndex = 0;
      this.tabPage1.Text = "Secondary";
      this.tabPage1.UseVisualStyleBackColor = true;
      this.ciContact.AutoScroll = true;
      this.ciContact.ContactInformation = (ContactInformation) null;
      this.ciContact.DisplayAddress = true;
      this.ciContact.DisplayContactName = true;
      this.ciContact.DisplayEmailAddress = true;
      this.ciContact.DisplayEntityName = true;
      this.ciContact.DisplayFaxNumber = true;
      this.ciContact.DisplayPhoneNumber = true;
      this.ciContact.DisplayWebSite = true;
      this.ciContact.Dock = DockStyle.Fill;
      this.ciContact.EntityNameTextBoxText = "";
      this.ciContact.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.ciContact.Location = new Point(0, 8);
      this.ciContact.Name = "ciContact";
      this.ciContact.ReadOnly = false;
      this.ciContact.RolodexCategory = "Investor";
      this.ciContact.Size = new Size(341, 294);
      this.ciContact.TabIndex = 0;
      this.ciContact.DataChange += new EventHandler(this.onContactDataChange);
      this.tabPage3.Controls.Add((Control) this.ciShipping);
      this.tabPage3.Location = new Point(4, 24);
      this.tabPage3.Name = "tabPage3";
      this.tabPage3.Padding = new Padding(0, 8, 0, 3);
      this.tabPage3.Size = new Size(341, 305);
      this.tabPage3.TabIndex = 2;
      this.tabPage3.Text = "Ship To";
      this.tabPage3.UseVisualStyleBackColor = true;
      this.ciShipping.AutoScroll = true;
      this.ciShipping.ContactInformation = (ContactInformation) null;
      this.ciShipping.DisplayAddress = true;
      this.ciShipping.DisplayContactName = true;
      this.ciShipping.DisplayEmailAddress = true;
      this.ciShipping.DisplayEntityName = true;
      this.ciShipping.DisplayFaxNumber = true;
      this.ciShipping.DisplayPhoneNumber = true;
      this.ciShipping.DisplayWebSite = false;
      this.ciShipping.Dock = DockStyle.Fill;
      this.ciShipping.EntityNameTextBoxText = "";
      this.ciShipping.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.ciShipping.Location = new Point(0, 8);
      this.ciShipping.Name = "ciShipping";
      this.ciShipping.ReadOnly = false;
      this.ciShipping.RolodexCategory = "Investor";
      this.ciShipping.Size = new Size(341, 294);
      this.ciShipping.TabIndex = 1;
      this.ciShipping.DataChange += new EventHandler(this.onContactDataChange);
      this.tpAssignee.Controls.Add((Control) this.ciAssignee);
      this.tpAssignee.Location = new Point(4, 24);
      this.tpAssignee.Name = "tpAssignee";
      this.tpAssignee.Padding = new Padding(0, 8, 0, 3);
      this.tpAssignee.Size = new Size(341, 305);
      this.tpAssignee.TabIndex = 5;
      this.tpAssignee.Text = "Assignee";
      this.tpAssignee.UseVisualStyleBackColor = true;
      this.ciAssignee.AutoScroll = true;
      this.ciAssignee.ContactInformation = (ContactInformation) null;
      this.ciAssignee.DisplayAddress = true;
      this.ciAssignee.DisplayContactName = true;
      this.ciAssignee.DisplayEmailAddress = true;
      this.ciAssignee.DisplayEntityName = true;
      this.ciAssignee.DisplayFaxNumber = true;
      this.ciAssignee.DisplayPhoneNumber = true;
      this.ciAssignee.DisplayWebSite = false;
      this.ciAssignee.Dock = DockStyle.Fill;
      this.ciAssignee.EntityNameTextBoxText = "";
      this.ciAssignee.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.ciAssignee.Location = new Point(0, 8);
      this.ciAssignee.Name = "ciAssignee";
      this.ciAssignee.ReadOnly = false;
      this.ciAssignee.RolodexCategory = "TradeAssignee";
      this.ciAssignee.Size = new Size(341, 294);
      this.ciAssignee.TabIndex = 2;
      this.ciAssignee.DataChange += new EventHandler(this.onContactDataChange);
      this.tabPage2.Controls.Add((Control) this.ciServicing);
      this.tabPage2.Location = new Point(4, 24);
      this.tabPage2.Name = "tabPage2";
      this.tabPage2.Padding = new Padding(0, 8, 0, 3);
      this.tabPage2.Size = new Size(341, 305);
      this.tabPage2.TabIndex = 1;
      this.tabPage2.Text = "Customer Service";
      this.tabPage2.UseVisualStyleBackColor = true;
      this.ciServicing.AutoScroll = true;
      this.ciServicing.ContactInformation = (ContactInformation) null;
      this.ciServicing.DisplayAddress = true;
      this.ciServicing.DisplayContactName = true;
      this.ciServicing.DisplayEmailAddress = true;
      this.ciServicing.DisplayEntityName = true;
      this.ciServicing.DisplayFaxNumber = true;
      this.ciServicing.DisplayPhoneNumber = true;
      this.ciServicing.DisplayWebSite = false;
      this.ciServicing.Dock = DockStyle.Fill;
      this.ciServicing.EntityNameTextBoxText = "";
      this.ciServicing.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.ciServicing.Location = new Point(0, 8);
      this.ciServicing.Name = "ciServicing";
      this.ciServicing.ReadOnly = false;
      this.ciServicing.RolodexCategory = "Investor";
      this.ciServicing.Size = new Size(341, 294);
      this.ciServicing.TabIndex = 1;
      this.ciServicing.DataChange += new EventHandler(this.onContactDataChange);
      this.tabPage4.Controls.Add((Control) this.ciDocs);
      this.tabPage4.Location = new Point(4, 24);
      this.tabPage4.Name = "tabPage4";
      this.tabPage4.Padding = new Padding(0, 8, 0, 3);
      this.tabPage4.Size = new Size(341, 305);
      this.tabPage4.TabIndex = 3;
      this.tabPage4.Text = "Trailing Docs";
      this.tabPage4.UseVisualStyleBackColor = true;
      this.ciDocs.AutoScroll = true;
      this.ciDocs.ContactInformation = (ContactInformation) null;
      this.ciDocs.DisplayAddress = true;
      this.ciDocs.DisplayContactName = true;
      this.ciDocs.DisplayEmailAddress = true;
      this.ciDocs.DisplayEntityName = true;
      this.ciDocs.DisplayFaxNumber = true;
      this.ciDocs.DisplayPhoneNumber = true;
      this.ciDocs.DisplayWebSite = false;
      this.ciDocs.Dock = DockStyle.Fill;
      this.ciDocs.EntityNameTextBoxText = "";
      this.ciDocs.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.ciDocs.Location = new Point(0, 8);
      this.ciDocs.Name = "ciDocs";
      this.ciDocs.ReadOnly = false;
      this.ciDocs.RolodexCategory = "Investor";
      this.ciDocs.Size = new Size(341, 294);
      this.ciDocs.TabIndex = 1;
      this.ciDocs.DataChange += new EventHandler(this.onContactDataChange);
      this.tpDealer.Controls.Add((Control) this.ciDealer);
      this.tpDealer.Location = new Point(4, 24);
      this.tpDealer.Name = "tpDealer";
      this.tpDealer.Padding = new Padding(0, 8, 0, 3);
      this.tpDealer.Size = new Size(341, 305);
      this.tpDealer.TabIndex = 4;
      this.tpDealer.Text = "Dealer";
      this.tpDealer.UseVisualStyleBackColor = true;
      this.ciDealer.AutoScroll = true;
      this.ciDealer.ContactInformation = (ContactInformation) null;
      this.ciDealer.DisplayAddress = true;
      this.ciDealer.DisplayContactName = true;
      this.ciDealer.DisplayEmailAddress = true;
      this.ciDealer.DisplayEntityName = true;
      this.ciDealer.DisplayFaxNumber = true;
      this.ciDealer.DisplayPhoneNumber = true;
      this.ciDealer.DisplayWebSite = false;
      this.ciDealer.Dock = DockStyle.Fill;
      this.ciDealer.EntityNameTextBoxText = "";
      this.ciDealer.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.ciDealer.Location = new Point(0, 8);
      this.ciDealer.Name = "ciDealer";
      this.ciDealer.ReadOnly = false;
      this.ciDealer.RolodexCategory = "Dealer";
      this.ciDealer.Size = new Size(341, 294);
      this.ciDealer.TabIndex = 3;
      this.tpPayment.Controls.Add((Control) this.ciPayment);
      this.tpPayment.Location = new Point(4, 24);
      this.tpPayment.Name = "tpPayment";
      this.tpPayment.Padding = new Padding(0, 8, 0, 3);
      this.tpPayment.Size = new Size(341, 305);
      this.tpPayment.TabIndex = 6;
      this.tpPayment.Text = "Payment";
      this.tpPayment.UseVisualStyleBackColor = true;
      this.ciPayment.AutoScroll = true;
      this.ciPayment.ContactInformation = (ContactInformation) null;
      this.ciPayment.DisplayAddress = true;
      this.ciPayment.DisplayContactName = true;
      this.ciPayment.DisplayEmailAddress = true;
      this.ciPayment.DisplayEntityName = true;
      this.ciPayment.DisplayFaxNumber = true;
      this.ciPayment.DisplayPhoneNumber = true;
      this.ciPayment.DisplayWebSite = false;
      this.ciPayment.Dock = DockStyle.Fill;
      this.ciPayment.EntityNameTextBoxText = "";
      this.ciPayment.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.ciPayment.Location = new Point(0, 8);
      this.ciPayment.Name = "ciPayment";
      this.ciPayment.ReadOnly = false;
      this.ciPayment.RolodexCategory = "Investor";
      this.ciPayment.Size = new Size(341, 294);
      this.ciPayment.TabIndex = 0;
      this.tpInsurance.Controls.Add((Control) this.ciInsurance);
      this.tpInsurance.Location = new Point(4, 24);
      this.tpInsurance.Name = "tpInsurance";
      this.tpInsurance.Padding = new Padding(0, 8, 0, 3);
      this.tpInsurance.Size = new Size(341, 305);
      this.tpInsurance.TabIndex = 7;
      this.tpInsurance.Text = "Insurance";
      this.tpInsurance.UseVisualStyleBackColor = true;
      this.ciInsurance.AutoScroll = true;
      this.ciInsurance.ContactInformation = (ContactInformation) null;
      this.ciInsurance.DisplayAddress = true;
      this.ciInsurance.DisplayContactName = true;
      this.ciInsurance.DisplayEmailAddress = true;
      this.ciInsurance.DisplayEntityName = true;
      this.ciInsurance.DisplayFaxNumber = true;
      this.ciInsurance.DisplayPhoneNumber = true;
      this.ciInsurance.DisplayWebSite = false;
      this.ciInsurance.Dock = DockStyle.Fill;
      this.ciInsurance.EntityNameTextBoxText = "";
      this.ciInsurance.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.ciInsurance.Location = new Point(0, 8);
      this.ciInsurance.Name = "ciInsurance";
      this.ciInsurance.ReadOnly = false;
      this.ciInsurance.RolodexCategory = "Investor";
      this.ciInsurance.Size = new Size(341, 294);
      this.ciInsurance.TabIndex = 0;
      this.tpNoteDelivery.Controls.Add((Control) this.ciNoteDelivery);
      this.tpNoteDelivery.Location = new Point(4, 24);
      this.tpNoteDelivery.Name = "tpNoteDelivery";
      this.tpNoteDelivery.Padding = new Padding(0, 8, 0, 3);
      this.tpNoteDelivery.Size = new Size(341, 305);
      this.tpNoteDelivery.TabIndex = 8;
      this.tpNoteDelivery.Text = "Note Delivery";
      this.tpNoteDelivery.UseVisualStyleBackColor = true;
      this.ciNoteDelivery.AutoScroll = true;
      this.ciNoteDelivery.ContactInformation = (ContactInformation) null;
      this.ciNoteDelivery.DisplayAddress = true;
      this.ciNoteDelivery.DisplayContactName = true;
      this.ciNoteDelivery.DisplayEmailAddress = true;
      this.ciNoteDelivery.DisplayEntityName = true;
      this.ciNoteDelivery.DisplayFaxNumber = true;
      this.ciNoteDelivery.DisplayPhoneNumber = true;
      this.ciNoteDelivery.DisplayWebSite = false;
      this.ciNoteDelivery.Dock = DockStyle.Fill;
      this.ciNoteDelivery.EntityNameTextBoxText = "";
      this.ciNoteDelivery.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.ciNoteDelivery.Location = new Point(0, 8);
      this.ciNoteDelivery.Name = "ciNoteDelivery";
      this.ciNoteDelivery.ReadOnly = false;
      this.ciNoteDelivery.RolodexCategory = "Investor";
      this.ciNoteDelivery.Size = new Size(341, 294);
      this.ciNoteDelivery.TabIndex = 0;
      this.tpTaxNotice.Controls.Add((Control) this.ciTaxNotice);
      this.tpTaxNotice.Location = new Point(4, 24);
      this.tpTaxNotice.Name = "tpTaxNotice";
      this.tpTaxNotice.Padding = new Padding(0, 8, 0, 3);
      this.tpTaxNotice.Size = new Size(341, 305);
      this.tpTaxNotice.TabIndex = 9;
      this.tpTaxNotice.Text = "Tax Notice";
      this.tpTaxNotice.UseVisualStyleBackColor = true;
      this.ciTaxNotice.AutoScroll = true;
      this.ciTaxNotice.ContactInformation = (ContactInformation) null;
      this.ciTaxNotice.DisplayAddress = true;
      this.ciTaxNotice.DisplayContactName = true;
      this.ciTaxNotice.DisplayEmailAddress = true;
      this.ciTaxNotice.DisplayEntityName = true;
      this.ciTaxNotice.DisplayFaxNumber = true;
      this.ciTaxNotice.DisplayPhoneNumber = true;
      this.ciTaxNotice.DisplayWebSite = false;
      this.ciTaxNotice.Dock = DockStyle.Fill;
      this.ciTaxNotice.EntityNameTextBoxText = "";
      this.ciTaxNotice.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.ciTaxNotice.Location = new Point(0, 8);
      this.ciTaxNotice.Name = "ciTaxNotice";
      this.ciTaxNotice.ReadOnly = false;
      this.ciTaxNotice.RolodexCategory = "Investor";
      this.ciTaxNotice.Size = new Size(341, 294);
      this.ciTaxNotice.TabIndex = 0;
      this.tpMortgageInsurance.Controls.Add((Control) this.ciMortgageInsurance);
      this.tpMortgageInsurance.Location = new Point(4, 24);
      this.tpMortgageInsurance.Name = "tpMortgageInsurance";
      this.tpMortgageInsurance.Padding = new Padding(0, 8, 0, 3);
      this.tpMortgageInsurance.Size = new Size(341, 305);
      this.tpMortgageInsurance.TabIndex = 10;
      this.tpMortgageInsurance.Text = "Mortgage Insurance";
      this.tpMortgageInsurance.UseVisualStyleBackColor = true;
      this.ciMortgageInsurance.AutoScroll = true;
      this.ciMortgageInsurance.ContactInformation = (ContactInformation) null;
      this.ciMortgageInsurance.DisplayAddress = true;
      this.ciMortgageInsurance.DisplayContactName = true;
      this.ciMortgageInsurance.DisplayEmailAddress = true;
      this.ciMortgageInsurance.DisplayEntityName = true;
      this.ciMortgageInsurance.DisplayFaxNumber = true;
      this.ciMortgageInsurance.DisplayPhoneNumber = true;
      this.ciMortgageInsurance.DisplayWebSite = false;
      this.ciMortgageInsurance.Dock = DockStyle.Fill;
      this.ciMortgageInsurance.EntityNameTextBoxText = "";
      this.ciMortgageInsurance.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.ciMortgageInsurance.Location = new Point(0, 8);
      this.ciMortgageInsurance.Name = "ciMortgageInsurance";
      this.ciMortgageInsurance.ReadOnly = false;
      this.ciMortgageInsurance.RolodexCategory = "Investor";
      this.ciMortgageInsurance.Size = new Size(341, 294);
      this.ciMortgageInsurance.TabIndex = 0;
      this.tpLoanDelivery.Controls.Add((Control) this.ciLoanDelivery);
      this.tpLoanDelivery.Location = new Point(4, 24);
      this.tpLoanDelivery.Name = "tpLoanDelivery";
      this.tpLoanDelivery.Padding = new Padding(0, 8, 0, 3);
      this.tpLoanDelivery.Size = new Size(341, 305);
      this.tpLoanDelivery.TabIndex = 11;
      this.tpLoanDelivery.Text = "Loan Delivery";
      this.tpLoanDelivery.UseVisualStyleBackColor = true;
      this.ciLoanDelivery.AutoScroll = true;
      this.ciLoanDelivery.ContactInformation = (ContactInformation) null;
      this.ciLoanDelivery.DisplayAddress = true;
      this.ciLoanDelivery.DisplayContactName = true;
      this.ciLoanDelivery.DisplayEmailAddress = true;
      this.ciLoanDelivery.DisplayEntityName = true;
      this.ciLoanDelivery.DisplayFaxNumber = true;
      this.ciLoanDelivery.DisplayPhoneNumber = true;
      this.ciLoanDelivery.DisplayWebSite = false;
      this.ciLoanDelivery.Dock = DockStyle.Fill;
      this.ciLoanDelivery.EntityNameTextBoxText = "";
      this.ciLoanDelivery.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.ciLoanDelivery.Location = new Point(0, 8);
      this.ciLoanDelivery.Name = "ciLoanDelivery";
      this.ciLoanDelivery.ReadOnly = false;
      this.ciLoanDelivery.RolodexCategory = "Investor";
      this.ciLoanDelivery.Size = new Size(341, 294);
      this.ciLoanDelivery.TabIndex = 0;
      this.tpAssignment.Controls.Add((Control) this.ciAssignment);
      this.tpAssignment.Location = new Point(4, 24);
      this.tpAssignment.Name = "tpAssignment";
      this.tpAssignment.Padding = new Padding(0, 8, 0, 3);
      this.tpAssignment.Size = new Size(341, 305);
      this.tpAssignment.TabIndex = 12;
      this.tpAssignment.Text = "Assignment";
      this.tpAssignment.UseVisualStyleBackColor = true;
      this.ciAssignment.AutoScroll = true;
      this.ciAssignment.ContactInformation = (ContactInformation) null;
      this.ciAssignment.DisplayAddress = true;
      this.ciAssignment.DisplayContactName = true;
      this.ciAssignment.DisplayEmailAddress = true;
      this.ciAssignment.DisplayEntityName = true;
      this.ciAssignment.DisplayFaxNumber = true;
      this.ciAssignment.DisplayPhoneNumber = true;
      this.ciAssignment.DisplayWebSite = false;
      this.ciAssignment.Dock = DockStyle.Fill;
      this.ciAssignment.EntityNameTextBoxText = "";
      this.ciAssignment.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.ciAssignment.Location = new Point(0, 8);
      this.ciAssignment.Name = "ciAssignment";
      this.ciAssignment.ReadOnly = false;
      this.ciAssignment.RolodexCategory = "Investor";
      this.ciAssignment.Size = new Size(341, 294);
      this.ciAssignment.TabIndex = 0;
      this.tpCorrespondence.Controls.Add((Control) this.ciCorrespondence);
      this.tpCorrespondence.Location = new Point(4, 24);
      this.tpCorrespondence.Name = "tpCorrespondence";
      this.tpCorrespondence.Padding = new Padding(0, 8, 0, 3);
      this.tpCorrespondence.Size = new Size(341, 305);
      this.tpCorrespondence.TabIndex = 13;
      this.tpCorrespondence.Text = "Correspondence";
      this.tpCorrespondence.UseVisualStyleBackColor = true;
      this.ciCorrespondence.AutoScroll = true;
      this.ciCorrespondence.ContactInformation = (ContactInformation) null;
      this.ciCorrespondence.DisplayAddress = true;
      this.ciCorrespondence.DisplayContactName = true;
      this.ciCorrespondence.DisplayEmailAddress = true;
      this.ciCorrespondence.DisplayEntityName = true;
      this.ciCorrespondence.DisplayFaxNumber = true;
      this.ciCorrespondence.DisplayPhoneNumber = true;
      this.ciCorrespondence.DisplayWebSite = false;
      this.ciCorrespondence.Dock = DockStyle.Fill;
      this.ciCorrespondence.EntityNameTextBoxText = "";
      this.ciCorrespondence.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.ciCorrespondence.Location = new Point(0, 8);
      this.ciCorrespondence.Name = "ciCorrespondence";
      this.ciCorrespondence.ReadOnly = false;
      this.ciCorrespondence.RolodexCategory = "Investor";
      this.ciCorrespondence.Size = new Size(341, 294);
      this.ciCorrespondence.TabIndex = 0;
      this.tpGeneric1.Controls.Add((Control) this.ciGeneric1);
      this.tpGeneric1.Location = new Point(4, 24);
      this.tpGeneric1.Name = "tpGeneric1";
      this.tpGeneric1.Padding = new Padding(0, 8, 0, 3);
      this.tpGeneric1.Size = new Size(341, 305);
      this.tpGeneric1.TabIndex = 14;
      this.tpGeneric1.Text = "Generic 1";
      this.tpGeneric1.UseVisualStyleBackColor = true;
      this.tpGeneric2.Controls.Add((Control) this.ciGeneric2);
      this.tpGeneric2.Location = new Point(4, 24);
      this.tpGeneric2.Name = "tpGeneric2";
      this.tpGeneric2.Padding = new Padding(0, 8, 0, 3);
      this.tpGeneric2.Size = new Size(341, 305);
      this.tpGeneric2.TabIndex = 15;
      this.tpGeneric2.Text = "Generic 2";
      this.tpGeneric2.UseVisualStyleBackColor = true;
      this.tpGeneric3.Controls.Add((Control) this.ciGeneric3);
      this.tpGeneric3.Location = new Point(4, 24);
      this.tpGeneric3.Name = "tpGeneric3";
      this.tpGeneric3.Padding = new Padding(0, 8, 0, 3);
      this.tpGeneric3.Size = new Size(341, 305);
      this.tpGeneric3.TabIndex = 16;
      this.tpGeneric3.Text = "Generic 3";
      this.tpGeneric3.UseVisualStyleBackColor = true;
      this.tpGeneric4.Controls.Add((Control) this.ciGeneric4);
      this.tpGeneric4.Location = new Point(4, 24);
      this.tpGeneric4.Name = "tpGeneric4";
      this.tpGeneric4.Padding = new Padding(0, 8, 0, 3);
      this.tpGeneric4.Size = new Size(341, 305);
      this.tpGeneric4.TabIndex = 17;
      this.tpGeneric4.Text = "Generic 4";
      this.tpGeneric4.UseVisualStyleBackColor = true;
      this.ciGeneric1.AutoScroll = true;
      this.ciGeneric1.ContactInformation = (ContactInformation) null;
      this.ciGeneric1.DisplayAddress = true;
      this.ciGeneric1.DisplayContactName = true;
      this.ciGeneric1.DisplayEmailAddress = true;
      this.ciGeneric1.DisplayEntityName = true;
      this.ciGeneric1.DisplayFaxNumber = true;
      this.ciGeneric1.DisplayPhoneNumber = true;
      this.ciGeneric1.DisplayWebSite = false;
      this.ciGeneric1.Dock = DockStyle.Fill;
      this.ciGeneric1.EntityNameTextBoxText = "";
      this.ciGeneric1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.ciGeneric1.Location = new Point(0, 8);
      this.ciGeneric1.Name = "ciGeneric1";
      this.ciGeneric1.ReadOnly = false;
      this.ciGeneric1.RolodexCategory = "Investor";
      this.ciGeneric1.Size = new Size(341, 294);
      this.ciGeneric1.TabIndex = 1;
      this.ciGeneric2.AutoScroll = true;
      this.ciGeneric2.ContactInformation = (ContactInformation) null;
      this.ciGeneric2.DisplayAddress = true;
      this.ciGeneric2.DisplayContactName = true;
      this.ciGeneric2.DisplayEmailAddress = true;
      this.ciGeneric2.DisplayEntityName = true;
      this.ciGeneric2.DisplayFaxNumber = true;
      this.ciGeneric2.DisplayPhoneNumber = true;
      this.ciGeneric2.DisplayWebSite = false;
      this.ciGeneric2.Dock = DockStyle.Fill;
      this.ciGeneric2.EntityNameTextBoxText = "";
      this.ciGeneric2.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.ciGeneric2.Location = new Point(0, 8);
      this.ciGeneric2.Name = "ciGeneric2";
      this.ciGeneric2.ReadOnly = false;
      this.ciGeneric2.RolodexCategory = "Investor";
      this.ciGeneric2.Size = new Size(341, 294);
      this.ciGeneric2.TabIndex = 2;
      this.ciGeneric3.AutoScroll = true;
      this.ciGeneric3.ContactInformation = (ContactInformation) null;
      this.ciGeneric3.DisplayAddress = true;
      this.ciGeneric3.DisplayContactName = true;
      this.ciGeneric3.DisplayEmailAddress = true;
      this.ciGeneric3.DisplayEntityName = true;
      this.ciGeneric3.DisplayFaxNumber = true;
      this.ciGeneric3.DisplayPhoneNumber = true;
      this.ciGeneric3.DisplayWebSite = false;
      this.ciGeneric3.Dock = DockStyle.Fill;
      this.ciGeneric3.EntityNameTextBoxText = "";
      this.ciGeneric3.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.ciGeneric3.Location = new Point(0, 8);
      this.ciGeneric3.Name = "ciGeneric3";
      this.ciGeneric3.ReadOnly = false;
      this.ciGeneric3.RolodexCategory = "Investor";
      this.ciGeneric3.Size = new Size(341, 294);
      this.ciGeneric3.TabIndex = 2;
      this.ciGeneric4.AutoScroll = true;
      this.ciGeneric4.ContactInformation = (ContactInformation) null;
      this.ciGeneric4.DisplayAddress = true;
      this.ciGeneric4.DisplayContactName = true;
      this.ciGeneric4.DisplayEmailAddress = true;
      this.ciGeneric4.DisplayEntityName = true;
      this.ciGeneric4.DisplayFaxNumber = true;
      this.ciGeneric4.DisplayPhoneNumber = true;
      this.ciGeneric4.DisplayWebSite = false;
      this.ciGeneric4.Dock = DockStyle.Fill;
      this.ciGeneric4.EntityNameTextBoxText = "";
      this.ciGeneric4.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.ciGeneric4.Location = new Point(0, 8);
      this.ciGeneric4.Name = "ciGeneric4";
      this.ciGeneric4.ReadOnly = false;
      this.ciGeneric4.RolodexCategory = "Investor";
      this.ciGeneric4.Size = new Size(341, 294);
      this.ciGeneric4.TabIndex = 2;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.Transparent;
      this.Controls.Add((Control) this.tabsMain);
      this.Name = nameof (InvestorAddressEditor);
      this.Size = new Size(349, 333);
      this.tabsMain.ResumeLayout(false);
      this.tabPage1.ResumeLayout(false);
      this.tabPage3.ResumeLayout(false);
      this.tpAssignee.ResumeLayout(false);
      this.tabPage2.ResumeLayout(false);
      this.tabPage4.ResumeLayout(false);
      this.tpDealer.ResumeLayout(false);
      this.tpPayment.ResumeLayout(false);
      this.tpInsurance.ResumeLayout(false);
      this.tpNoteDelivery.ResumeLayout(false);
      this.tpTaxNotice.ResumeLayout(false);
      this.tpMortgageInsurance.ResumeLayout(false);
      this.tpLoanDelivery.ResumeLayout(false);
      this.tpAssignment.ResumeLayout(false);
      this.tpCorrespondence.ResumeLayout(false);
      this.tpGeneric1.ResumeLayout(false);
      this.tpGeneric2.ResumeLayout(false);
      this.tpGeneric3.ResumeLayout(false);
      this.tpGeneric4.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    public delegate void AssigneeNameChangeEventHandler(object sender, string newName);
  }
}
