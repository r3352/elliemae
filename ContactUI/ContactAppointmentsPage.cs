// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.ContactAppointmentsPage
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.Calendar;
using EllieMae.EMLite.ClientServer.Calendar;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI
{
  public class ContactAppointmentsPage : Form
  {
    private ContactInfo contactInfo;
    private IContainer components;
    private GroupContainer gcAppointment;
    private StandardIconButton btnNewAppt;
    private StandardIconButton btnDeleteAppt;
    private GridView listViewAppt;
    private ToolTip toolTip1;
    public bool IsReadOnly;

    public event EventHandler AppointmentModified;

    public ContactAppointmentsPage() => this.InitializeComponent();

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
      this.gcAppointment = new GroupContainer();
      this.listViewAppt = new GridView();
      this.btnDeleteAppt = new StandardIconButton();
      this.btnNewAppt = new StandardIconButton();
      this.toolTip1 = new ToolTip(this.components);
      this.gcAppointment.SuspendLayout();
      ((ISupportInitialize) this.btnDeleteAppt).BeginInit();
      ((ISupportInitialize) this.btnNewAppt).BeginInit();
      this.SuspendLayout();
      this.gcAppointment.Controls.Add((Control) this.listViewAppt);
      this.gcAppointment.Controls.Add((Control) this.btnDeleteAppt);
      this.gcAppointment.Controls.Add((Control) this.btnNewAppt);
      this.gcAppointment.Dock = DockStyle.Fill;
      this.gcAppointment.Location = new Point(0, 0);
      this.gcAppointment.Name = "gcAppointment";
      this.gcAppointment.Size = new Size(299, 161);
      this.gcAppointment.TabIndex = 6;
      this.gcAppointment.Text = "Appointments";
      this.listViewAppt.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Subject";
      gvColumn1.Width = 160;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Time";
      gvColumn2.Width = 120;
      this.listViewAppt.Columns.AddRange(new GVColumn[2]
      {
        gvColumn1,
        gvColumn2
      });
      this.listViewAppt.Dock = DockStyle.Fill;
      this.listViewAppt.Location = new Point(1, 26);
      this.listViewAppt.Name = "listViewAppt";
      this.listViewAppt.Size = new Size(297, 134);
      this.listViewAppt.TabIndex = 7;
      this.listViewAppt.SelectedIndexChanged += new EventHandler(this.listViewAppt_SelectedIndexChanged);
      this.listViewAppt.DoubleClick += new EventHandler(this.listViewAppt_DoubleClick);
      this.btnDeleteAppt.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDeleteAppt.BackColor = Color.Transparent;
      this.btnDeleteAppt.Location = new Point(278, 4);
      this.btnDeleteAppt.Name = "btnDeleteAppt";
      this.btnDeleteAppt.Size = new Size(16, 16);
      this.btnDeleteAppt.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnDeleteAppt.TabIndex = 6;
      this.btnDeleteAppt.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnDeleteAppt, "Delete Appointment");
      this.btnDeleteAppt.Click += new EventHandler(this.btnDeleteAppt_Click);
      this.btnNewAppt.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnNewAppt.BackColor = Color.Transparent;
      this.btnNewAppt.Location = new Point(258, 4);
      this.btnNewAppt.Name = "btnNewAppt";
      this.btnNewAppt.Size = new Size(16, 16);
      this.btnNewAppt.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnNewAppt.TabIndex = 5;
      this.btnNewAppt.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnNewAppt, "New Appointment");
      this.btnNewAppt.Click += new EventHandler(this.btnNewAppt_Click);
      this.AutoScaleBaseSize = new Size(5, 13);
      this.BackColor = Color.White;
      this.ClientSize = new Size(299, 161);
      this.Controls.Add((Control) this.gcAppointment);
      this.FormBorderStyle = FormBorderStyle.None;
      this.Name = nameof (ContactAppointmentsPage);
      this.ShowInTaskbar = false;
      this.Text = nameof (ContactAppointmentsPage);
      this.gcAppointment.ResumeLayout(false);
      ((ISupportInitialize) this.btnDeleteAppt).EndInit();
      ((ISupportInitialize) this.btnNewAppt).EndInit();
      this.ResumeLayout(false);
    }

    public ContactInfo CurrentContact
    {
      get => this.contactInfo;
      set
      {
        this.contactInfo = (ContactInfo) null;
        this.disableControls();
        if (value != null)
        {
          this.contactInfo = value;
          this.loadForm();
          this.btnNewAppt.Enabled = true;
          this.btnDeleteAppt.Enabled = true;
          if (this.listViewAppt.SelectedItems.Count != 0)
            return;
          this.btnDeleteAppt.Enabled = false;
        }
        else
        {
          this.btnNewAppt.Enabled = false;
          this.btnDeleteAppt.Enabled = false;
        }
      }
    }

    public void disableForm() => this.disableControlsOnly();

    private void disableControlsOnly()
    {
      this.btnDeleteAppt.Enabled = false;
      this.btnNewAppt.Enabled = false;
    }

    private void disableControls()
    {
      this.clearForm();
      this.disableControlsOnly();
    }

    private void enableControls()
    {
      this.btnDeleteAppt.Enabled = true;
      this.btnNewAppt.Enabled = true;
    }

    private void loadForm()
    {
      if (!this.IsReadOnly)
        this.enableControls();
      this.GetAppointmentsForContact();
    }

    private void clearForm()
    {
      this.listViewAppt.Items.Clear();
      this.gcAppointment.Text = "Appointments (0)";
    }

    public bool SaveChanges() => true;

    public void AddAppointment() => this.btnNewAppt_Click((object) null, (EventArgs) null);

    private void btnNewAppt_Click(object sender, EventArgs e)
    {
      if (this.ParentForm is IBindingForm && ((IBindingForm) this.ParentForm).SaveChanges())
      {
        ContactInfo contactInfo = (ContactInfo) null;
        if (this.CurrentContact != null)
        {
          if (CategoryType.BizPartner == this.CurrentContact.ContactType)
          {
            BizPartnerInfo bizPartner = Session.ContactManager.GetBizPartner(int.Parse(this.CurrentContact.ContactID));
            if (bizPartner != null)
              contactInfo = new ContactInfo(bizPartner.FirstName + " " + bizPartner.LastName, bizPartner.ContactID.ToString(), CategoryType.BizPartner);
          }
          else
          {
            BorrowerInfo borrower = Session.ContactManager.GetBorrower(int.Parse(this.CurrentContact.ContactID));
            if (borrower != null)
              contactInfo = new ContactInfo(borrower.FirstName + " " + borrower.LastName, borrower.ContactID.ToString(), CategoryType.Borrower);
          }
        }
        this.CurrentContact = contactInfo;
      }
      frmCalendar userCalendar = frmCalendar.GetUserCalendar(Session.UserInfo.Userid, CSMessage.AccessLevel.Full);
      ContactInfo[] linkedContacts;
      if (this.contactInfo != null)
        linkedContacts = new ContactInfo[1]
        {
          this.contactInfo
        };
      else
        linkedContacts = new ContactInfo[0];
      userCalendar.AddAppointment(linkedContacts);
      this.GetAppointmentsForContact();
      if (this.AppointmentModified == null)
        return;
      this.AppointmentModified((object) null, (EventArgs) null);
    }

    private void GetAppointmentsForContact()
    {
      if (this.contactInfo == null)
        return;
      AppointmentInfo[] appointmentsForContact = Session.CalendarManager.GetAllAppointmentsForContact(this.contactInfo);
      this.listViewAppt.Items.Clear();
      frmCalendar userCalendar = frmCalendar.GetUserCalendar(Session.UserInfo.Userid, CSMessage.AccessLevel.Full);
      for (int index = 0; index < appointmentsForContact.Length; ++index)
      {
        AppointmentInfo appt = appointmentsForContact[index];
        string meetingTime = string.Empty;
        if (!appt.IsRemoved)
        {
          DateTime dateTime;
          if (!userCalendar.CheckAppointmentRecurrenceRoot(appt, ref meetingTime))
          {
            if (appt.AllDayEvent)
            {
              dateTime = appt.StartDateTime;
              meetingTime = "All Day - " + dateTime.ToString("d");
            }
            else
            {
              dateTime = appt.StartDateTime;
              string str1 = dateTime.ToString("g");
              dateTime = appt.EndDateTime;
              string str2 = dateTime.ToString("g");
              meetingTime = str1 + " - " + str2;
            }
          }
          this.listViewAppt.Items.Add(new GVItem(new string[2]
          {
            appt.Subject,
            meetingTime
          })
          {
            Tag = (object) appt
          });
        }
      }
      this.listViewAppt_SelectedIndexChanged((object) null, (EventArgs) null);
      this.gcAppointment.Text = "Appointments (" + (object) this.listViewAppt.Items.Count + ")";
    }

    private void listViewAppt_DoubleClick(object sender, EventArgs e)
    {
      if (this.listViewAppt.SelectedItems.Count <= 0)
        return;
      AppointmentInfo tag = (AppointmentInfo) this.listViewAppt.SelectedItems[0].Tag;
      string dataKey = tag.DataKey;
      frmCalendar.GetUserCalendar(tag.UserID, tag.AccessLevel).EditAppointmentFromContact(dataKey);
      this.GetAppointmentsForContact();
    }

    private void btnDeleteAppt_Click(object sender, EventArgs e)
    {
      if (this.listViewAppt.SelectedItems.Count > 0)
      {
        AppointmentInfo tag = (AppointmentInfo) this.listViewAppt.SelectedItems[0].Tag;
        string dataKey = tag.DataKey;
        frmCalendar.GetUserCalendar(tag.UserID, tag.AccessLevel).DeleteAppointmentFromContact(dataKey);
        this.GetAppointmentsForContact();
        if (this.AppointmentModified == null)
          return;
        this.AppointmentModified((object) null, (EventArgs) null);
      }
      else
      {
        int num = (int) Utils.Dialog((IWin32Window) Session.MainForm, "Please select the appointment you want to delete before clicking the Delete Appointment button.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
    }

    private void listViewAppt_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.contactInfo == null)
        return;
      if (this.listViewAppt.SelectedItems.Count == 0)
        this.btnDeleteAppt.Enabled = false;
      else if (((AppointmentInfo) this.listViewAppt.SelectedItems[0].Tag).AccessLevel == CSMessage.AccessLevel.Full)
        this.btnDeleteAppt.Enabled = true;
      else
        this.btnDeleteAppt.Enabled = false;
    }
  }
}
