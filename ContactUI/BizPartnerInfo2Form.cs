// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.BizPartnerInfo2Form
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.ClientServer.Calendar;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI
{
  public class BizPartnerInfo2Form : Form, IBindingForm
  {
    private Panel pnlLeft;
    private Panel pnlRight;
    private bool changed;
    private ContactType sourceCT = ContactType.PublicBiz;
    private BizPartnerInfo contactInfo;
    private ContactAppointmentsPage apptPage;
    private ContactGroupsPage groupPage;
    private bool isReadOnly;
    private BizPartnerTabForm tabForm;

    public event EventHandler RequireContactRefresh;

    public bool IsReadOnly
    {
      get => this.isReadOnly;
      set
      {
        this.isReadOnly = value;
        this.apptPage.IsReadOnly = value;
        this.groupPage.IsReadOnly = value;
      }
    }

    public bool isDirty() => this.changed;

    public BizPartnerInfo2Form(BizPartnerTabForm tabForm, ContactType source)
    {
      this.InitializeComponent();
      this.tabForm = tabForm;
      this.sourceCT = source;
      this.initializeControls();
    }

    private void initializeControls()
    {
      this.apptPage = new ContactAppointmentsPage();
      this.apptPage.TopLevel = false;
      this.apptPage.Dock = DockStyle.Fill;
      this.apptPage.Visible = true;
      this.apptPage.AppointmentModified += new EventHandler(this.apptPage_AppointmentModified);
      this.pnlLeft.Controls.Add((Control) this.apptPage);
      this.groupPage = new ContactGroupsPage(this.tabForm.ContactInfoForm, this.sourceCT);
      this.groupPage.TopLevel = false;
      this.groupPage.Dock = DockStyle.Fill;
      this.groupPage.Visible = true;
      this.groupPage.GroupsModified += new EventHandler(this.groupPage_GroupsModified);
      this.pnlRight.Controls.Add((Control) this.groupPage);
    }

    private void groupPage_GroupsModified(object sender, EventArgs e)
    {
      if (this.RequireContactRefresh == null)
        return;
      this.RequireContactRefresh((object) null, (EventArgs) null);
    }

    private void apptPage_AppointmentModified(object sender, EventArgs e)
    {
      if (this.RequireContactRefresh == null)
        return;
      this.RequireContactRefresh((object) null, (EventArgs) null);
    }

    public int CurrentContactID
    {
      get => this.contactInfo != null ? this.contactInfo.ContactID : -1;
      set
      {
        if (this.CurrentContactID == value)
          return;
        this.contactInfo = (BizPartnerInfo) null;
        this.apptPage.CurrentContact = (ContactInfo) null;
        this.groupPage.CurrentContact = value;
        if (value < 0)
          return;
        this.contactInfo = Session.ContactManager.GetBizPartner(value);
        if (this.contactInfo == null)
          throw new ObjectNotFoundException("Unable to retrieve business contact.", ObjectType.Contact, (object) value);
        this.apptPage.CurrentContact = new ContactInfo(this.contactInfo.FirstName + " " + this.contactInfo.LastName, this.contactInfo.ContactID.ToString(), CategoryType.BizPartner);
        this.groupPage.CurrentContact = this.contactInfo.ContactID;
      }
    }

    public object CurrentContact
    {
      get => (object) this.contactInfo;
      set
      {
        if (this.CurrentContact == value)
          return;
        this.contactInfo = (BizPartnerInfo) null;
        this.apptPage.CurrentContact = (ContactInfo) null;
        this.groupPage.CurrentContact = value == null ? -1 : ((BizPartnerInfo) value).ContactID;
        if (value == null)
          return;
        this.contactInfo = (BizPartnerInfo) value;
        this.apptPage.CurrentContact = new ContactInfo(this.contactInfo.FirstName + " " + this.contactInfo.LastName, this.contactInfo.ContactID.ToString(), CategoryType.BizPartner);
        this.groupPage.CurrentContact = this.contactInfo.ContactID;
      }
    }

    public bool SaveChanges() => true;

    private void InitializeComponent()
    {
      this.pnlLeft = new Panel();
      this.pnlRight = new Panel();
      this.SuspendLayout();
      this.pnlLeft.Dock = DockStyle.Left;
      this.pnlLeft.Location = new Point(1, 1);
      this.pnlLeft.Name = "pnlLeft";
      this.pnlLeft.Size = new Size(256, 231);
      this.pnlLeft.TabIndex = 0;
      this.pnlRight.Dock = DockStyle.Fill;
      this.pnlRight.Location = new Point(257, 1);
      this.pnlRight.Name = "pnlRight";
      this.pnlRight.Padding = new Padding(4, 0, 0, 0);
      this.pnlRight.Size = new Size(295, 231);
      this.pnlRight.TabIndex = 1;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.White;
      this.ClientSize = new Size(553, 233);
      this.Controls.Add((Control) this.pnlRight);
      this.Controls.Add((Control) this.pnlLeft);
      this.Font = new Font("Arial", 11f, FontStyle.Regular, GraphicsUnit.Pixel, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.None;
      this.Name = nameof (BizPartnerInfo2Form);
      this.Padding = new Padding(1);
      this.Text = "BizPartnerInfoForm";
      this.SizeChanged += new EventHandler(this.BizPartnerInfo2Form_SizeChanged);
      this.ResumeLayout(false);
    }

    private void BizPartnerInfo2Form_SizeChanged(object sender, EventArgs e)
    {
      int width = 503;
      if (this.Width > 1010)
        width = (this.Width - 4) / 2;
      Panel pnlRight = this.pnlRight;
      Panel pnlLeft = this.pnlLeft;
      Size size1 = new Size(width, this.Height);
      Size size2 = size1;
      pnlLeft.Size = size2;
      Size size3 = size1;
      pnlRight.Size = size3;
    }
  }
}
