// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Automation.Macro
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassAutomation.xml

using EllieMae.EMLite.CalculationEngine;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Compiler;
using EllieMae.EMLite.Customization;
using EllieMae.EMLite.InputEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.ComponentModel;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.Encompass.Automation
{
  /// <summary>
  /// Represents the collection of Macros available in the Form Builder's Macro window.
  /// </summary>
  /// <remarks>Macros are staticly defined methods which provide shortcuts for functions
  /// that would otherwise require more significant understanding of the EncompassAutomation API.
  /// Although primarilly intended for use within the Input Form Builder's event editor,
  /// they can be used in any automation-enabled development environment.</remarks>
  public class Macro
  {
    private const string className = "Macro";
    private static string sw = Tracing.SwInputEngine;

    private Macro()
    {
    }

    /// <summary>Copies the value of one loan field to another.</summary>
    /// <param name="sourceFieldID">The Field ID of the source field.</param>
    /// <param name="targetFieldID">The Field ID of the target field.</param>
    [Macro]
    [Description("Copies the value of one loan field to another. You must specify the Field IDs of the source and target fields.")]
    public static void CopyField([Description("Copy From"), Required] string sourceFieldID, [Description("Copy To"), Required] string targetFieldID)
    {
      if (EncompassApplication.CurrentLoan == null)
        return;
      EncompassApplication.CurrentLoan.Fields[targetFieldID].Value = EncompassApplication.CurrentLoan.Fields[sourceFieldID].Value;
    }

    /// <summary>
    /// Macro for opening a URL with parameters in the user's default web browser application.
    /// </summary>
    /// <param name="url">The URL to open.</param>
    /// <param name="parameters">The parameters used with URL.</param>
    [Macro]
    [Description("Opens a URL with parameters in the user's default Web Browser.")]
    public static void OpenURL([Description("URL"), Required] string url, [Description("Parameters"), Required] string parameters)
    {
      if (!Macro.IsLegalUrl(url))
        throw new ApplicationException("url contains potentially unsafe path: " + url);
      Process.Start(url + parameters);
    }

    /// <summary>
    /// Macro for opening a URL in the user's default web browser application.
    /// </summary>
    /// <param name="url">The URL to open.</param>
    [Macro]
    [Description("Opens a URL in the user's default Web Browser.")]
    public static void OpenURL([Description("URL"), Required] string url)
    {
      if (!Macro.IsLegalUrl(url))
        throw new ApplicationException("url contains potentially unsafe path: " + url);
      Process.Start(url);
    }

    /// <summary>Validate the url address to make sure it is safe</summary>
    /// <param name="urlInput"></param>
    /// <returns></returns>
    private static bool IsLegalUrl(string urlInput)
    {
      Uri uri = new Uri(urlInput, UriKind.Absolute);
      return (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps) && Uri.IsWellFormedUriString(urlInput, UriKind.RelativeOrAbsolute);
    }

    /// <summary>
    /// Opens a new mail message in the user's default mail application.
    /// </summary>
    /// <param name="addressees">The addressee to whom the message is to be sent.</param>
    /// <param name="subject">The subject of the message.</param>
    [Description("Opens a new email message in the user's default mail application. You may specify multiple addressees by seperating them with commas.")]
    public static void OpenEMail([Description("Addressees"), Required] string addressees, [Description("Subject")] string subject)
    {
      Macro.OpenEMail(addressees, subject, "");
    }

    /// <summary>
    /// Opens a new mail message in the user's default mail application.
    /// </summary>
    /// <param name="addressees">The addressee to whom the message is to be sent.</param>
    /// <param name="subject">The subject of the message.</param>
    /// <param name="body">The body of the email message. Only plain text is allowed, no HTML markup.</param>
    [Macro]
    [Description("Opens a new email message in the user's default mail application. You may specify multiple addressees by seperating them with commas.")]
    public static void OpenEMail([Description("Addressees"), Required] string addressees, [Description("Subject")] string subject, [Description("Body")] string body)
    {
      if (!Macro.IsLegalEmailAddress(addressees))
        throw new ApplicationException("Email Address contains potentially unsafe information or invalid format: " + addressees);
      Process.Start("mailto:" + HttpUtility.UrlEncode(addressees).Replace('+', ' ') + "?Subject=" + HttpUtility.UrlEncode(subject).Replace('+', ' ') + "&body=" + HttpUtility.UrlEncode(body).Replace('+', ' '));
    }

    /// <summary>Validate the email addressees to make sure it is safe</summary>
    /// <param name="addressees"></param>
    /// <returns></returns>
    private static bool IsLegalEmailAddress(string addressees)
    {
      return addressees.Length != 0 && addressees.IndexOf("@") > -1 && addressees.IndexOf(".", addressees.IndexOf("@")) > addressees.IndexOf("@");
    }

    /// <summary>Launches an external application.</summary>
    /// <param name="fileName">The full path to the application to be launched.</param>
    /// <param name="arguments">The command line arguments to be passed to the program.</param>
    /// <param name="waitForExit">Indicates if the Encompass UI should wait until the program
    /// terminates before responding to user input.</param>
    [Macro]
    [Description("Launches an external application.")]
    public static void Run([Description("Program File/Path"), Required] string fileName, [Description("Command Line")] string arguments, [Description("Wait for Program to Exit")] bool waitForExit)
    {
      Process process = Macro.IsLegalProgramName(fileName) ? Process.Start(fileName, arguments) : throw new ApplicationException("path of the application contains potentially unsafe path , or invlaid filename or path: " + fileName);
      if (!waitForExit)
        return;
      process.WaitForExit();
    }

    /// <summary>
    /// Validate the program name path to make sure it is a full valid physical path
    /// </summary>
    /// <param name="fileNameWithPath"></param>
    /// <returns></returns>
    private static bool IsLegalProgramName(string fileNameWithPath)
    {
      return !new Regex("[" + Regex.Escape(new string(Path.GetInvalidPathChars())) + "]").IsMatch(fileNameWithPath) && Path.IsPathRooted(fileNameWithPath) && new FileInfo(fileNameWithPath).Exists;
    }

    /// <summary>
    /// Sends keystrokes to the application window to be executed.
    /// </summary>
    /// <param name="keySequence">The sequence of keystrokes to send.</param>
    [Macro]
    [Description("Sends a sequence of keystrokes to the application. Use % to designate ALT, ^ for CONTROL and + for SHIFT.")]
    public static void SendKeys([Description("Key Sequence"), Required] string keySequence)
    {
      System.Windows.Forms.SendKeys.Send(keySequence);
    }

    /// <summary>Opens the ePASS Services Window.</summary>
    /// <param name="serviceType">The service type to be displayed.</param>
    [Macro]
    [Description("Opens the Ellie Mae Network Services Window. Use the Service Type parameter to specify which list of services should be displayed, e.g. \"Credit Report\".)")]
    public static void DisplayServices([Description("Service Type"), Required] string serviceType)
    {
      EncompassApplication.EPass.DisplayServices(serviceType);
    }

    /// <summary>
    /// Provides a pop-up confirmation window and return a bool indicating if the user
    /// pressed the OK button.
    /// </summary>
    /// <param name="message">The message to be displayed.</param>
    /// <returns><c>True</c> if the user clicked OK, <c>false</c> otherwise.</returns>
    [Macro]
    [Description("Provides a pop-up confirmation message to the user, who will be permitted to select OK or Cancel. This function returns a TRUE value if the user selects OK, FALSE otherwise.")]
    public static bool Confirm([Description("Message"), Required] string message)
    {
      return MessageBox.Show((IWin32Window) Session.MainScreen, message, "Encompass", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK;
    }

    /// <summary>Provides a pop-up alert window to the user.</summary>
    /// <param name="message">The message to be displayed.</param>
    [Macro]
    [Description("Provides a pop-up alert message to the user.")]
    public static void Alert([Description("Message"), Required] string message)
    {
      int num = (int) MessageBox.Show((IWin32Window) Session.MainScreen, message, "Encompass", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
    }

    /// <summary>
    /// Performs a ZIP Code lookup and populates the specified fields with the
    /// City, County and State values for it.
    /// </summary>
    /// <param name="expression">The expression to be evaluated (using the Eval syntax) to a ZIP Code.</param>
    /// <param name="cityFieldID">The Field ID of the associated City field, or blank for none.</param>
    /// <param name="countyFieldID">The Field ID of the associated County field, or blank for none.</param>
    /// <param name="stateFieldID">The Field ID of the associated State field, or blank for none.</param>
    [Macro]
    [Description("Populates city, county and state field data by looking up a ZIP Code.")]
    public static void ResolveZipCode(
      [Description("Zip Expression")] string expression,
      [Description("City Field ID")] string cityFieldID,
      [Description("County Field ID")] string countyFieldID,
      [Description("State Field ID")] string stateFieldID)
    {
      if (EncompassApplication.CurrentLoan == null)
        return;
      string zip = string.Concat(Macro.Eval(expression));
      if (zip == "")
        return;
      ZipCodeInfo zipInfoAt = ZipCodeUtils.GetZipInfoAt(zip);
      if (zipInfoAt == null)
        return;
      if ((cityFieldID ?? "") != "")
        EncompassApplication.CurrentLoan.Fields[cityFieldID].Value = (object) zipInfoAt.City;
      if ((countyFieldID ?? "") != "")
        EncompassApplication.CurrentLoan.Fields[countyFieldID].Value = (object) zipInfoAt.County;
      if (!((stateFieldID ?? "") != ""))
        return;
      EncompassApplication.CurrentLoan.Fields[stateFieldID].Value = (object) zipInfoAt.State;
    }

    /// <summary>
    /// Changes the currently visible screen in EncompassApplication.
    /// </summary>
    /// <param name="screen"></param>
    [Macro]
    [Description("Changes the currently visible screen in EncompassApplication.")]
    public static void GoToScreen([Description("Screen"), Required] EncompassScreen screen)
    {
      EncompassApplication.Screens[screen].MakeCurrent();
    }

    /// <summary>
    /// Changes the currently visible input form in EncompassApplication.
    /// </summary>
    /// <param name="formName">The name of the form to make current, as it appears in the
    /// Forms list.</param>
    [Macro]
    [Description("Changes the currently visible input form in the Encompass loan editor.")]
    public static void GoToForm([Description("Form Name"), Required] string formName)
    {
      ((LoansScreen) EncompassApplication.Screens[EncompassScreen.Loans]).GoToForm(formName);
    }

    /// <summary>
    /// Executes a standard action in the Encompass loan editor.
    /// </summary>
    /// <param name="action">The action to be performed</param>
    [Macro]
    [Description("Executes a standard action in the Encompass loan editor.")]
    public static void ExecAction([Description("Action"), Required] string action)
    {
      ((LoansScreen) EncompassApplication.Screens[EncompassScreen.Loans]).CurrentForm.ExecAction(action);
    }

    /// <summary>
    /// Executes an Ellie Mae Network Signature to invoke a partner service.
    /// </summary>
    /// <param name="emnSignature">The signature to be executes</param>
    [Macro]
    [Description("Executes an Ellie Mae Network (EMN) Signature to invoke a partner service.")]
    public static void ExecSignature([Description("EMN Signature"), Required] string emnSignature)
    {
      ((EPassScreen) EncompassApplication.Screens[EncompassScreen.ePASS]).Execute(emnSignature);
    }

    /// <summary>Gets a single field from the loan.</summary>
    /// <param name="fieldId">The ID of the field to be updated</param>
    [Macro]
    [Description("Gets the value of a loan field.")]
    public static string GetField([Description("Field ID"), Required] string fieldId)
    {
      return EncompassApplication.CurrentLoan != null ? EncompassApplication.CurrentLoan.Fields[fieldId].FormattedValue : "";
    }

    /// <summary>Sets a single field in the loan.</summary>
    /// <param name="fieldId">The ID of the field to be updated</param>
    /// <param name="value">The value to store into the specified field</param>
    [Macro]
    [Description("Sets the value of a loan field.")]
    public static void SetField([Description("Field ID"), Required] string fieldId, [Description("Value")] string value)
    {
      if (EncompassApplication.CurrentLoan == null)
        return;
      EncompassApplication.CurrentLoan.Fields[fieldId].Value = (object) value;
    }

    /// <summary>
    /// Sets a single field in the loan, ignoring any business rules that may prevent this action.
    /// </summary>
    /// <param name="fieldId">The ID of the field to be updated</param>
    /// <param name="value">The value to store into the specified field</param>
    [Macro]
    [Description("Sets the value of a loan field, ignoring any Field or Field Access Rules which would otherwise prevent this action.")]
    public static void SetFieldNoRules([Description("Field ID"), Required] string fieldId, [Description("Value")] string value)
    {
      if (EncompassApplication.CurrentLoan == null)
        return;
      bool businessRulesEnabled = EncompassApplication.CurrentLoan.BusinessRulesEnabled;
      if (businessRulesEnabled)
        EncompassApplication.CurrentLoan.BusinessRulesEnabled = false;
      EncompassApplication.CurrentLoan.Fields[fieldId].Value = (object) value;
      if (!businessRulesEnabled)
        return;
      EncompassApplication.CurrentLoan.BusinessRulesEnabled = true;
    }

    /// <summary>
    /// Sets a single field in the loan by evaluating an expression.
    /// </summary>
    /// <param name="fieldId">The ID of the field to be updated</param>
    /// <param name="expr">The expression to evaluate and save into the specified field</param>
    [Macro]
    [Description("Sets the value of a loan field by evaluating the given expression.")]
    public static void SetFieldEval([Description("Field ID"), Required] string fieldId, [Description("Expression")] string expr)
    {
      Macro.SetField(fieldId, string.Concat(Macro.Eval(expr)));
    }

    /// <summary>
    /// Evaluates an expression using the custom field calculation engine.
    /// </summary>
    /// <param name="expr">The expression to be evaluated.</param>
    /// <returns>Returns the result of the evaluation of the given expression.</returns>
    /// <remarks>The expression syntax is the same as that used for the Encompass
    /// Loan Custom Field Calculations.</remarks>
    [Macro]
    [Description("Evaluates an expression using the custom field calculation syntax.")]
    public static object Eval([Description("Expression"), Required] string expr)
    {
      try
      {
        using (RuntimeContext context1 = RuntimeContext.Create())
        {
          using (CustomCalculationContext context2 = new CustomCalculationContext(Session.UserInfo, Session.LoanData, (IServerDataProvider) new CustomCodeSessionDataProvider(Session.SessionObjects)))
            return new CalculationBuilder().CreateImplementation(new CustomCalculation(expr), context1).Calculate((ICalculationContext) context2);
        }
      }
      catch (CompileException ex)
      {
        if (EnConfigurationSettings.GlobalSettings.Debug)
        {
          throw;
        }
        else
        {
          CompilerError error = ex.Errors[0];
          string str = "Eval failed for expression '" + expr + "': ";
          throw new ApplicationException(error.CharIndexOfRegion < 0 || error.CharIndexOfRegion >= expr.Length ? str + "Expression contains errors or is not a valid formula." : str + error.Message, (Exception) ex);
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(Macro.sw, nameof (Macro), TraceLevel.Warning, "Macro.Eval failed for expression '" + expr + "': " + (object) ex);
        return (object) "";
      }
    }

    /// <summary>Displays an input form in a modal pop-up window.</summary>
    /// <param name="formName">The name of the form to be displayed.</param>
    /// <param name="title">The title of the Window to be displayed.</param>
    /// <param name="width">The width of the pop-up window, in pixels.</param>
    /// <param name="height">The height of the pop-up window, in pixels.</param>
    [Macro]
    [Description("Displays an input form in a modal pop-up window.")]
    public static void Popup([Description("Form Name"), Required] string formName, [Description("Title"), Required] string title, [Description("Width"), Required] int width, [Description("Height"), Required] int height)
    {
      ILoanConsole service1 = Session.Application.GetService<ILoanConsole>();
      ILoanEditor service2 = Session.Application.GetService<ILoanEditor>();
      if (!service1.HasOpenLoan)
        throw new Exception("The loan screen must be open in order to display in input form");
      ((IFormScreen) service2.GetFormScreen()).Popup(formName, title, width, height);
    }

    /// <summary>Prints one or more output forms.</summary>
    /// <param name="form1">First form to print</param>
    /// <param name="form2">Second form to print</param>
    /// <param name="form3">Third form to print</param>
    [Macro]
    [Description("Prints one or more forms for the current loan. For Standard Print Forms, enter the form's name, e.g. \"1003 Page 1\". For Custom Print Forms, enter the full path to the form, e.g. \"public:\\My Forms\\Appraisal Notification.doc\".")]
    public static void Print([Description("Form 1"), Required] string form1, [Description("Form 2")] string form2, [Description("Form 3")] string form3)
    {
      Macro.Print(new string[3]{ form1, form2, form3 });
    }

    /// <summary>Prints one or more output forms.</summary>
    /// <param name="formNames">The name of the form to be displayed.</param>
    [Description("Prints one or more forms for the current loan.")]
    public static void Print(params string[] formNames)
    {
      ILoanConsole service1 = Session.Application.GetService<ILoanConsole>();
      ILoanEditor service2 = Session.Application.GetService<ILoanEditor>();
      if (!service1.HasOpenLoan)
        throw new Exception("The loan screen must be open in order to print");
      service2.Print(formNames);
    }

    /// <summary>Apply Field Access Business Rule.</summary>
    [Macro]
    [Description("Apply Input Form Business Rule, Loan Access Rule, Field Access Rule and Field Rule.")]
    public static void ApplyBusinessRule()
    {
      Session.Application.GetService<ILoanEditor>()?.ApplyBusinessRules();
    }
  }
}
