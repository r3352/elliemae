// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Automation.LoansScreen
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassAutomation.xml

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.InputEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.BusinessObjects.Loans;
using EllieMae.Encompass.Forms;
using System;

#nullable disable
namespace EllieMae.Encompass.Automation
{
  /// <summary>
  /// Represents the Loans screen within Encompass, which is the loan editor.
  /// </summary>
  public class LoansScreen : Screen
  {
    private ScopedEventHandler<FormChangeEventArgs> formLoaded;
    private ScopedEventHandler<FormChangeEventArgs> formUnloading;

    /// <summary>
    /// Event indicating that a new input form is being loaded into the Form Editor.
    /// </summary>
    public event FormChangeEventHandler FormLoaded
    {
      add
      {
        if (value == null)
          return;
        this.formLoaded.Add(new ScopedEventHandler<FormChangeEventArgs>.EventHandlerT(value.Invoke));
      }
      remove
      {
        if (value == null)
          return;
        this.formLoaded.Remove(new ScopedEventHandler<FormChangeEventArgs>.EventHandlerT(value.Invoke));
      }
    }

    /// <summary>
    /// Event indicating that an input form is being unloaded into the Form Editor.
    /// </summary>
    public event FormChangeEventHandler FormUnloading
    {
      add
      {
        if (value == null)
          return;
        this.formUnloading.Add(new ScopedEventHandler<FormChangeEventArgs>.EventHandlerT(value.Invoke));
      }
      remove
      {
        if (value == null)
          return;
        this.formUnloading.Remove(new ScopedEventHandler<FormChangeEventArgs>.EventHandlerT(value.Invoke));
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

    /// <summary>
    /// Gets the currently active input form from the Loan Editor.
    /// </summary>
    /// <remarks>If no form is currently active or the currently active form does not support
    /// the <see cref="T:EllieMae.Encompass.Forms.Form" /> object model, this method will return <c>null</c>.</remarks>
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

    /// <summary>
    /// Sets the specified form as the currently visible form.
    /// </summary>
    /// <param name="formName">The name of the form to set as current.</param>
    /// <remarks>The specified form name must be in the user's form list or this call will be ignored.
    /// When this method is invoked, the specified form is loaded asynchronously, meaning that you cannot
    /// assume that when this call is complete that the specified form has been completely loaded into
    /// the loan editor.
    /// </remarks>
    public void GoToForm(string formName)
    {
      Session.Application.GetService<ILoanEditor>().CurrentForm = formName;
    }

    /// <summary>Opens a loan into the loan editor screen.</summary>
    /// <param name="guid">The unique identifier of the loan to be opened.</param>
    /// <remarks>If a loan is already open in the Loans screen, this will will throw an exception. You should
    /// <see cref="M:EllieMae.Encompass.Automation.LoansScreen.Close(System.Boolean)" /> the existing loan prior to opening a new one.
    /// <p>Additionally, invoking this method starts the process of opening a loan in the loan editor, but the
    /// load occurs asynchronously. As a result, you cannot assume that the loan has been completely opened when this
    /// method returns. To perform an action when the loan has completed loading using the EncompassApplication.LoanOpened
    /// event instead.</p>
    /// </remarks>
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

    /// <summary>Creates a new loan and opens it in the loan editor.</summary>
    /// <param name="targetFolder">The <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.LoanFolder" /> in which the new loan file will be placed.</param>
    /// <param name="template">The <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Templates.LoanTemplate" /> to apply to the new loan, or <c>null</c> to apply no template.</param>
    /// <remarks>If a loan is already open in the Loans screen, this will will throw an exception. You should
    /// <see cref="M:EllieMae.Encompass.Automation.LoansScreen.Close(System.Boolean)" /> the existing loan prior to opening a new one.
    /// <p>Additionally, invoking this method starts the process of opening a loan in the loan editor, but the
    /// load occurs asynchronously. As a result, you cannot assume that the loan has been completely opened when this
    /// method returns. To perform an action when the loan has completed loading using the EncompassApplication.LoanOpened
    /// event instead.</p>
    /// </remarks>
    public Loan OpenNew(LoanFolder targetFolder, EllieMae.Encompass.BusinessObjects.Loans.Templates.LoanTemplate template)
    {
      ILoanConsole service = Session.Application.GetService<ILoanConsole>();
      if (service == null)
        throw new AutomationException("The current user does not have access to this feature.");
      if (service.HasOpenLoan)
        throw new AutomationException("A loan is currently open in the editor");
      if (targetFolder == null)
        throw new ArgumentNullException(nameof (targetFolder), "Specified folder cannot be null");
      FileSystemEntry template1 = (FileSystemEntry) null;
      if (template != null)
        template1 = FileSystemEntry.Parse(template.Path, EncompassApplication.Session.UserID);
      service.OpenNewLoan(targetFolder.Name, (string) null, template == null ? (LoanTemplateSelection) null : new LoanTemplateSelection(template1, false), true);
      return EncompassApplication.CurrentLoan;
    }

    /// <summary>
    /// Indicates if the editor is currently open with a loan.
    /// </summary>
    /// <returns>Returns <c>true</c> if the editor is open, <c>false</c> otherwise.</returns>
    public bool IsOpen()
    {
      ILoanConsole service = Session.Application.GetService<ILoanConsole>();
      return service != null && service.HasOpenLoan;
    }

    /// <summary>Closes the Loan Editor.</summary>
    /// <param name="allowCancel">Indicates if the user should be allowed to cancel this operation.</param>
    /// <returns>Returns <c>true</c> if the Loan screen has been closed, <c>false</c> otherwise.</returns>
    /// <remarks>If the user has modified the loan they will be offered the opportunity to save their changes.
    /// If the allowCancel parameter is <c>true</c>, the user will also be allowed to cancel the operation
    /// and continue working in the loan. In this case, the method will return <c>false</c> to indicate the
    /// loan editor remains open.</remarks>
    public bool Close(bool allowCancel)
    {
      return Session.Application.GetService<ILoanConsole>().CloseLoan(allowCancel);
    }

    /// <summary>Sets a codebase type for an Encompass standard form.</summary>
    /// <param name="formName">The name of the form as it appears in Encompass.</param>
    /// <param name="formType">The Type which represents the codebase for the form.</param>
    /// <remarks>The specified formType must derive from the base
    /// <see cref="T:EllieMae.Encompass.Forms.Form" /> type and must conform to all rules
    /// applicable to custom form codebase assemblies (e.g. must include a zero-parameter
    /// constructor).
    /// <p>Whenever the specified standard form is loaded, an instance of the specified
    /// formType will be instantiated. You can then dynamically modify the content or behavior
    /// of the standard form. Note that some modifications may break the default behavior of the
    /// form so extensive testing should be used when modifying existing behavior.</p>
    /// </remarks>
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

    /// <summary>
    /// Opens the Print dialog and prints one or more standard or custom forms.
    /// </summary>
    /// <param name="formNames">A list of the form names to be printed. For standard forms,
    /// the value should simply be the form name. For custom forms, the value should be the
    /// fully qualified path to the form, e.g. "public:\Approval Forms\Standard Approval Form.doc"</param>
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
        this.formLoaded.Invoke((object) this, new FormChangeEventArgs(form));
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
        this.formUnloading.Invoke((object) this, new FormChangeEventArgs(form));
      }
      catch (Exception ex)
      {
        throw new AutomationException("Automation error in LoansScreen.FormUnloading", ex);
      }
    }
  }
}
