// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Automation.LoansScreen
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.InputEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.BusinessObjects.Loans;
using EllieMae.Encompass.BusinessObjects.Loans.Templates;
using EllieMae.Encompass.Forms;
using System;

#nullable disable
namespace EllieMae.Encompass.Automation
{
  public class LoansScreen : Screen
  {
    private ScopedEventHandler<FormChangeEventArgs> formLoaded;
    private ScopedEventHandler<FormChangeEventArgs> formUnloading;

    public event FormChangeEventHandler FormLoaded
    {
      add
      {
        if (value == null)
          return;
        this.formLoaded.Add(new ScopedEventHandler<FormChangeEventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
      remove
      {
        if (value == null)
          return;
        this.formLoaded.Remove(new ScopedEventHandler<FormChangeEventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
    }

    public event FormChangeEventHandler FormUnloading
    {
      add
      {
        if (value == null)
          return;
        this.formUnloading.Add(new ScopedEventHandler<FormChangeEventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
      remove
      {
        if (value == null)
          return;
        this.formUnloading.Remove(new ScopedEventHandler<FormChangeEventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
    }

    internal LoansScreen()
      : base(EncompassScreen.Loans)
    {
      this.formLoaded = new ScopedEventHandler<FormChangeEventArgs>(nameof (LoansScreen), "FormLoaded");
      this.formUnloading = new ScopedEventHandler<FormChangeEventArgs>(nameof (LoansScreen), "FormUnloading");
      Session.FormLoaded += new EventHandler(this.onFormLoaded);
      Session.FormUnloading += new EventHandler(this.onFormUnloading);
    }

    public Form CurrentForm
    {
      get
      {
        ILoanEditor service = Session.Application.GetService<ILoanEditor>();
        if (service == null)
          return (Form) null;
        IFormScreen formScreen = (IFormScreen) service.GetFormScreen();
        return formScreen == null ? (Form) null : (Form) formScreen.GetAutomationFormObject();
      }
    }

    public void GoToForm(string formName)
    {
      Session.Application.GetService<ILoanEditor>().CurrentForm = formName;
    }

    public Loan Open(string guid)
    {
      ILoanConsole service = Session.Application.GetService<ILoanConsole>();
      if (service == null)
        throw new AutomationException("The current user does not have access to this feature.");
      if (service.HasOpenLoan)
        throw new AutomationException("A loan is currently open in the editor");
      service.OpenLoan(guid);
      return EncompassApplication.CurrentLoan;
    }

    public Loan OpenNew(LoanFolder targetFolder, LoanTemplate template)
    {
      ILoanConsole service = Session.Application.GetService<ILoanConsole>();
      if (service == null)
        throw new AutomationException("The current user does not have access to this feature.");
      if (service.HasOpenLoan)
        throw new AutomationException("A loan is currently open in the editor");
      if (targetFolder == null)
        throw new ArgumentNullException(nameof (targetFolder), "Specified folder cannot be null");
      FileSystemEntry fileSystemEntry = (FileSystemEntry) null;
      if (template != null)
        fileSystemEntry = FileSystemEntry.Parse(template.Path, EncompassApplication.Session.UserID);
      service.OpenNewLoan(targetFolder.Name, (string) null, template == null ? (LoanTemplateSelection) null : new LoanTemplateSelection(fileSystemEntry, false), true);
      return EncompassApplication.CurrentLoan;
    }

    public bool IsOpen()
    {
      ILoanConsole service = Session.Application.GetService<ILoanConsole>();
      return service != null && service.HasOpenLoan;
    }

    public bool Close(bool allowCancel)
    {
      return Session.Application.GetService<ILoanConsole>().CloseLoan(allowCancel);
    }

    public void AttachCodebaseToForm(string formName, Type formType)
    {
      if (!Session.IsBankerEdition())
        throw new NotSupportedException("The current operation is not supported in the current edition of Encompass.");
      if ((formName ?? "") == "")
        throw new ArgumentException(nameof (formName), "Form name cannot be blank or null");
      if (formType == (Type) null)
        throw new ArgumentNullException(nameof (formType));
      Session.StandardFormCodebaseTypes[(object) formName] = typeof (Form).IsAssignableFrom(formType) ? (object) formType : throw new ArgumentException("Specified type does not derive from " + typeof (Form).FullName);
    }

    public void Print(params string[] formNames)
    {
      if (formNames.Length == 0)
        throw new ArgumentException(nameof (formNames), "One or more forms must be specified");
      (Session.Application.GetService<ILoanEditor>() ?? throw new InvalidOperationException("The loan editor is not currently open.")).Print(formNames);
    }

    private void onFormLoaded(object sender, EventArgs e)
    {
      if (!(sender is Form form))
        return;
      if (form.Name == null)
        return;
      try
      {
        this.formLoaded((object) this, new FormChangeEventArgs(form));
      }
      catch (Exception ex)
      {
        throw new AutomationException("Automation error in LoansScreen.FormLoaded", ex);
      }
    }

    private void onFormUnloading(object sender, EventArgs e)
    {
      if (!(sender is Form form))
        return;
      if (form.Name == null)
        return;
      try
      {
        this.formUnloading((object) this, new FormChangeEventArgs(form));
      }
      catch (Exception ex)
      {
        throw new AutomationException("Automation error in LoansScreen.FormUnloading", ex);
      }
    }
  }
}
