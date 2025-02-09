// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.InvestorAddressEditorH
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class InvestorAddressEditorH : UserControl
  {
    private Investor currentInvestor;
    private ContactInformation currentDealer;
    private ContactInformation currentAssignee;
    private bool readOnly;
    private IContainer components;
    private TabControl tabsMain;
    private TabPage tabPage1;
    private TabPage tabPage3;
    private TabPage tabPage2;
    private TabPage tabPage4;
    private ContactInfoEditorH ciContact;
    private ContactInfoEditorH ciShipTo;
    private ContactInfoEditorH ciCustomerService;
    private ContactInfoEditorH ciTrailingDocs;
    private TabPage tpDealer;
    private ContactInfoEditorH ciDealer;
    private ContactInfoEditorH ciAssignee;
    private TabPage tpAssignee;

    public event EventHandler DataChange;

    public event InvestorAddressEditorH.AssigneeNameChangeEventHandlerH AssigneeNameChanged;

    public InvestorAddressEditorH()
      : this(false)
    {
    }

    public InvestorAddressEditorH(bool showAssigneeTab)
    {
      this.InitializeComponent();
      if (!showAssigneeTab)
        this.tabsMain.TabPages.Remove(this.tpAssignee);
      this.tabsMain.TabPages.Remove(this.tpDealer);
      this.ciAssignee.EntityNameTextChanged += new ContactInfoEditorH.EntityNameTextChangeEventHandlerH(this.ciAssignee_EntityNameTextChanged);
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
      this.ciContact.Clear();
      this.ciCustomerService.Clear();
      this.ciShipTo.Clear();
      this.ciTrailingDocs.Clear();
      if (this.currentDealer != null)
        this.ciDealer.Clear();
      if (this.currentAssignee == null)
        return;
      this.ciAssignee.Clear();
    }

    private void loadCurrentInvestor()
    {
      this.ciContact.ContactInformation = this.currentInvestor.ContactInformation;
      this.ciCustomerService.ContactInformation = this.currentInvestor.CustomerServiceInformation;
      this.ciShipTo.ContactInformation = this.currentInvestor.ShippingInformation;
      this.ciTrailingDocs.ContactInformation = this.currentInvestor.TrailingDocumentsInformation;
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
      this.ciContact.CommitChanges();
      this.ciCustomerService.CommitChanges();
      this.ciShipTo.CommitChanges();
      this.ciTrailingDocs.CommitChanges();
      if (this.currentDealer != null)
        this.ciDealer.CommitChanges();
      if (this.currentAssignee == null)
        return;
      this.ciAssignee.CommitChanges();
    }

    public bool DataModified
    {
      get
      {
        if (this.ciContact.DataModified || this.ciCustomerService.DataModified || this.ciShipTo.DataModified || this.ciTrailingDocs.DataModified || this.currentDealer != null && this.ciDealer.DataModified)
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
      this.ciContact.ReadOnly = this.readOnly;
      this.ciCustomerService.ReadOnly = this.readOnly;
      this.ciShipTo.ReadOnly = this.readOnly;
      this.ciTrailingDocs.ReadOnly = this.readOnly;
      this.ciDealer.ReadOnly = this.readOnly;
      this.ciAssignee.ReadOnly = this.readOnly;
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
      this.ciContact = new ContactInfoEditorH();
      this.tabPage3 = new TabPage();
      this.ciShipTo = new ContactInfoEditorH();
      this.tpAssignee = new TabPage();
      this.ciAssignee = new ContactInfoEditorH();
      this.tabPage2 = new TabPage();
      this.ciCustomerService = new ContactInfoEditorH();
      this.tabPage4 = new TabPage();
      this.ciTrailingDocs = new ContactInfoEditorH();
      this.tpDealer = new TabPage();
      this.ciDealer = new ContactInfoEditorH();
      this.tabsMain.SuspendLayout();
      this.tabPage1.SuspendLayout();
      this.tabPage3.SuspendLayout();
      this.tpAssignee.SuspendLayout();
      this.tabPage2.SuspendLayout();
      this.tabPage4.SuspendLayout();
      this.tpDealer.SuspendLayout();
      this.SuspendLayout();
      this.tabsMain.Controls.Add((Control) this.tabPage1);
      this.tabsMain.Controls.Add((Control) this.tabPage3);
      this.tabsMain.Controls.Add((Control) this.tpAssignee);
      this.tabsMain.Controls.Add((Control) this.tabPage2);
      this.tabsMain.Controls.Add((Control) this.tabPage4);
      this.tabsMain.Controls.Add((Control) this.tpDealer);
      this.tabsMain.Dock = DockStyle.Fill;
      this.tabsMain.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.tabsMain.ItemSize = new Size(73, 20);
      this.tabsMain.Location = new Point(0, 0);
      this.tabsMain.Name = "tabsMain";
      this.tabsMain.Padding = new Point(11, 3);
      this.tabsMain.SelectedIndex = 0;
      this.tabsMain.Size = new Size(715, 164);
      this.tabsMain.TabIndex = 0;
      this.tabPage1.Controls.Add((Control) this.ciContact);
      this.tabPage1.Location = new Point(4, 24);
      this.tabPage1.Name = "tabPage1";
      this.tabPage1.Padding = new Padding(0, 8, 0, 3);
      this.tabPage1.Size = new Size(707, 136);
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
      this.ciContact.Size = new Size(707, 125);
      this.ciContact.TabIndex = 0;
      this.ciContact.DataChange += new EventHandler(this.onContactDataChange);
      this.tabPage3.Controls.Add((Control) this.ciShipTo);
      this.tabPage3.Location = new Point(4, 24);
      this.tabPage3.Name = "tabPage3";
      this.tabPage3.Padding = new Padding(0, 8, 0, 3);
      this.tabPage3.Size = new Size(707, 136);
      this.tabPage3.TabIndex = 2;
      this.tabPage3.Text = "Ship To";
      this.tabPage3.UseVisualStyleBackColor = true;
      this.ciShipTo.AutoScroll = true;
      this.ciShipTo.ContactInformation = (ContactInformation) null;
      this.ciShipTo.DisplayAddress = true;
      this.ciShipTo.DisplayContactName = true;
      this.ciShipTo.DisplayEmailAddress = true;
      this.ciShipTo.DisplayEntityName = true;
      this.ciShipTo.DisplayFaxNumber = true;
      this.ciShipTo.DisplayPhoneNumber = true;
      this.ciShipTo.DisplayWebSite = false;
      this.ciShipTo.Dock = DockStyle.Fill;
      this.ciShipTo.EntityNameTextBoxText = "";
      this.ciShipTo.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.ciShipTo.Location = new Point(0, 8);
      this.ciShipTo.Name = "ciShipTo";
      this.ciShipTo.ReadOnly = false;
      this.ciShipTo.RolodexCategory = "Investor";
      this.ciShipTo.Size = new Size(707, 125);
      this.ciShipTo.TabIndex = 1;
      this.ciShipTo.DataChange += new EventHandler(this.onContactDataChange);
      this.tpAssignee.Controls.Add((Control) this.ciAssignee);
      this.tpAssignee.Location = new Point(4, 24);
      this.tpAssignee.Name = "tpAssignee";
      this.tpAssignee.Padding = new Padding(0, 8, 0, 3);
      this.tpAssignee.Size = new Size(707, 136);
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
      this.ciAssignee.Size = new Size(707, 125);
      this.ciAssignee.TabIndex = 2;
      this.ciAssignee.DataChange += new EventHandler(this.onContactDataChange);
      this.tabPage2.Controls.Add((Control) this.ciCustomerService);
      this.tabPage2.Location = new Point(4, 24);
      this.tabPage2.Name = "tabPage2";
      this.tabPage2.Padding = new Padding(0, 8, 0, 3);
      this.tabPage2.Size = new Size(707, 136);
      this.tabPage2.TabIndex = 1;
      this.tabPage2.Text = "Customer Service";
      this.tabPage2.UseVisualStyleBackColor = true;
      this.ciCustomerService.AutoScroll = true;
      this.ciCustomerService.ContactInformation = (ContactInformation) null;
      this.ciCustomerService.DisplayAddress = true;
      this.ciCustomerService.DisplayContactName = true;
      this.ciCustomerService.DisplayEmailAddress = true;
      this.ciCustomerService.DisplayEntityName = true;
      this.ciCustomerService.DisplayFaxNumber = true;
      this.ciCustomerService.DisplayPhoneNumber = true;
      this.ciCustomerService.DisplayWebSite = false;
      this.ciCustomerService.Dock = DockStyle.Fill;
      this.ciCustomerService.EntityNameTextBoxText = "";
      this.ciCustomerService.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.ciCustomerService.Location = new Point(0, 8);
      this.ciCustomerService.Name = "ciCustomerService";
      this.ciCustomerService.ReadOnly = false;
      this.ciCustomerService.RolodexCategory = "Investor";
      this.ciCustomerService.Size = new Size(707, 125);
      this.ciCustomerService.TabIndex = 1;
      this.ciCustomerService.DataChange += new EventHandler(this.onContactDataChange);
      this.tabPage4.Controls.Add((Control) this.ciTrailingDocs);
      this.tabPage4.Location = new Point(4, 24);
      this.tabPage4.Name = "tabPage4";
      this.tabPage4.Padding = new Padding(0, 8, 0, 3);
      this.tabPage4.Size = new Size(707, 136);
      this.tabPage4.TabIndex = 3;
      this.tabPage4.Text = "Trailing Docs";
      this.tabPage4.UseVisualStyleBackColor = true;
      this.ciTrailingDocs.AutoScroll = true;
      this.ciTrailingDocs.ContactInformation = (ContactInformation) null;
      this.ciTrailingDocs.DisplayAddress = true;
      this.ciTrailingDocs.DisplayContactName = true;
      this.ciTrailingDocs.DisplayEmailAddress = true;
      this.ciTrailingDocs.DisplayEntityName = true;
      this.ciTrailingDocs.DisplayFaxNumber = true;
      this.ciTrailingDocs.DisplayPhoneNumber = true;
      this.ciTrailingDocs.DisplayWebSite = false;
      this.ciTrailingDocs.Dock = DockStyle.Fill;
      this.ciTrailingDocs.EntityNameTextBoxText = "";
      this.ciTrailingDocs.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.ciTrailingDocs.Location = new Point(0, 8);
      this.ciTrailingDocs.Name = "ciTrailingDocs";
      this.ciTrailingDocs.ReadOnly = false;
      this.ciTrailingDocs.RolodexCategory = "Investor";
      this.ciTrailingDocs.Size = new Size(707, 125);
      this.ciTrailingDocs.TabIndex = 1;
      this.ciTrailingDocs.DataChange += new EventHandler(this.onContactDataChange);
      this.tpDealer.Controls.Add((Control) this.ciDealer);
      this.tpDealer.Location = new Point(4, 24);
      this.tpDealer.Name = "tpDealer";
      this.tpDealer.Padding = new Padding(0, 8, 0, 3);
      this.tpDealer.Size = new Size(707, 136);
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
      this.ciDealer.Size = new Size(707, 125);
      this.ciDealer.TabIndex = 2;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.Transparent;
      this.Controls.Add((Control) this.tabsMain);
      this.Name = nameof (InvestorAddressEditorH);
      this.Size = new Size(715, 164);
      this.tabsMain.ResumeLayout(false);
      this.tabPage1.ResumeLayout(false);
      this.tabPage3.ResumeLayout(false);
      this.tpAssignee.ResumeLayout(false);
      this.tabPage2.ResumeLayout(false);
      this.tabPage4.ResumeLayout(false);
      this.tpDealer.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    public delegate void AssigneeNameChangeEventHandlerH(object sender, string newName);
  }
}
