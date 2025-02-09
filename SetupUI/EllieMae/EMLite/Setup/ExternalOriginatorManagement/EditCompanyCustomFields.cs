// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ExternalOriginatorManagement.EditCompanyCustomFields
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ContactUI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.Setup.ExternalOriginatorManagement
{
  public class EditCompanyCustomFields : CustomFieldsControl
  {
    private Sessions.Session session;
    private int parentContactId = -1;
    private IContainer components;

    public EditCompanyCustomFields(Sessions.Session session, int orgID, int parentContactId)
      : base(EllieMae.EMLite.ContactUI.ContactType.TPO)
    {
      this.session = session;
      this.contactId = orgID;
      this.parentContactId = parentContactId;
      this.InitializeComponent();
      this.Dock = DockStyle.Fill;
    }

    protected override void updateCustomFields(int contactId, ContactCustomField[] fields)
    {
      this.session.ConfigurationManager.UpdateCustomFieldValues(contactId, fields);
      this.customFieldValues = fields;
    }

    public void populateTPOFieldValues(bool useParentInfo)
    {
      if (useParentInfo)
      {
        this.getContactInformation(this.parentContactId);
      }
      else
      {
        this.getContactInformation(this.contactId);
        if (this.customFieldValues.Length == 0)
          this.getContactInformation(this.parentContactId);
      }
      base.populateFieldValues();
    }

    public new void populateFieldValues()
    {
      this.setControlState(true, true);
      base.populateFieldValues();
    }

    protected override void getContactInformation(int contactId)
    {
      if (contactId > -1)
        this.customFieldValues = Session.ConfigurationManager.GetCustomFieldValues(contactId);
      else
        this.customFieldValues = new ContactCustomField[0];
    }

    public void DisableControls() => this.disableControl(this.Controls);

    private void disableControl(Control.ControlCollection controls)
    {
      foreach (Control control in (ArrangedElementCollection) controls)
      {
        switch (control)
        {
          case TextBox _:
          case CheckBox _:
          case ComboBox _:
          case DatePicker _:
            control.Enabled = false;
            break;
        }
        if (control.Controls != null && control.Controls.Count > 0)
          this.disableControl(control.Controls);
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
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Name = nameof (EditCompanyCustomFields);
      this.Size = new Size(925, 608);
      this.ResumeLayout(false);
    }
  }
}
