// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.ContactTabForm
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.Common;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.ContactUI
{
  public class ContactTabForm : Form, IBindingForm
  {
    private System.ComponentModel.Container components;
    protected int currentContactId = -1;
    protected object currentContact;
    protected TabControl tabs;
    protected bool fieldEnabled;

    public event ContactDeletedEventHandler ContactDeleted;

    public ContactTabForm() => this.InitializeComponent();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.SuspendLayout();
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.White;
      this.ClientSize = new Size(511, 260);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.None;
      this.Name = nameof (ContactTabForm);
      this.Text = "BorrowerTabForm";
      this.Closing += new CancelEventHandler(this.ContactTabForm_Closing);
      this.ResumeLayout(false);
    }

    protected TabControl TabControl
    {
      get => this.tabs;
      set
      {
        this.tabs = value;
        this.tabs.SelectedIndexChanged += new EventHandler(this.tabChanged);
      }
    }

    public virtual int CurrentContactID
    {
      get => this.currentContactId;
      set
      {
        this.currentContactId = value;
        foreach (Control tabPage in this.tabs.TabPages)
        {
          foreach (Control control in (ArrangedElementCollection) tabPage.Controls)
          {
            if (control is IBindingForm)
            {
              IBindingForm bindingForm = (IBindingForm) control;
              try
              {
                bindingForm.CurrentContactID = this.currentContactId;
              }
              catch (ObjectNotFoundException ex)
              {
                int num = (int) Utils.Dialog((IWin32Window) this, ex.Message + " The contact has been deleted by another user.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.OnContactDeleted(this.currentContactId);
                return;
              }
            }
          }
        }
      }
    }

    public virtual object CurrentContact
    {
      get => this.currentContact;
      set
      {
        this.currentContact = value;
        this.currentContactId = value != null ? (!(value is BizPartnerInfo) ? ((BorrowerInfo) value).ContactID : ((BizPartnerInfo) value).ContactID) : -1;
        foreach (TabPage tabPage in this.tabs.TabPages)
        {
          foreach (Control control in (ArrangedElementCollection) tabPage.Controls)
          {
            if (control is IBindingForm)
            {
              IBindingForm bindingForm = (IBindingForm) control;
              try
              {
                if (this.tabs.SelectedTab == tabPage)
                  bindingForm.CurrentContact = this.currentContact;
                else if (bindingForm.CurrentContact != this.currentContact)
                  bindingForm.CurrentContact = (object) null;
              }
              catch (ObjectNotFoundException ex)
              {
                int num = (int) Utils.Dialog((IWin32Window) this, ex.Message + " The contact has been deleted by another user.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.OnContactDeleted(this.currentContactId);
                return;
              }
            }
          }
        }
      }
    }

    public virtual bool SaveChanges()
    {
      bool flag = false;
      try
      {
        foreach (Control tabPage in this.tabs.TabPages)
        {
          foreach (Control control in (ArrangedElementCollection) tabPage.Controls)
          {
            if (control is IBindingForm)
            {
              IBindingForm bindingForm = (IBindingForm) control;
              if (bindingForm.CurrentContact == this.currentContact)
                flag |= bindingForm.SaveChanges();
            }
          }
        }
      }
      catch (ObjectNotFoundException ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, ex.Message + " The contact has been deleted by another user.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        this.OnContactDeleted(this.currentContactId);
      }
      return flag;
    }

    public virtual bool isDirty()
    {
      bool flag = false;
      try
      {
        foreach (Control tabPage in this.tabs.TabPages)
        {
          foreach (Control control in (ArrangedElementCollection) tabPage.Controls)
          {
            if (control is IBindingForm)
            {
              IBindingForm bindingForm = (IBindingForm) control;
              if (bindingForm.CurrentContact == this.currentContact && bindingForm.isDirty())
              {
                flag = true;
                break;
              }
            }
          }
          if (flag)
            break;
        }
      }
      catch (ObjectNotFoundException ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, ex.Message + " The contact has been deleted by another user.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        this.OnContactDeleted(this.currentContactId);
      }
      return flag;
    }

    public virtual void ClearChanges() => this.CurrentContact = (object) null;

    protected IBindingForm getVisibleForm()
    {
      foreach (Control control in (ArrangedElementCollection) this.tabs.SelectedTab.Controls)
      {
        if (control is IBindingForm)
          return (IBindingForm) control;
      }
      return (IBindingForm) null;
    }

    private void tabChanged(object sender, EventArgs e)
    {
      IBindingForm visibleForm = this.getVisibleForm();
      if (visibleForm == null)
        return;
      try
      {
        visibleForm.CurrentContact = this.currentContact;
      }
      catch (ObjectNotFoundException ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, ex.Message + " The contact has been deleted by another user.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        this.OnContactDeleted(this.currentContactId);
      }
    }

    private void ContactTabForm_Closing(object sender, CancelEventArgs e)
    {
      try
      {
        this.SaveChanges();
      }
      catch
      {
      }
    }

    protected void OnContactDeleted(int contactId)
    {
      if (this.ContactDeleted == null)
        return;
      this.ContactDeleted(contactId);
    }
  }
}
