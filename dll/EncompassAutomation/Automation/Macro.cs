// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Automation.Macro
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll

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
  public class Macro
  {
    private const string className = "Macro";
    private static string sw = Tracing.SwInputEngine;

    private Macro()
    {
    }

    [Macro]
    [Description("Copies the value of one loan field to another. You must specify the Field IDs of the source and target fields.")]
    public static void CopyField([Description("Copy From"), Required] string sourceFieldID, [Description("Copy To"), Required] string targetFieldID)
    {
      if (EncompassApplication.CurrentLoan == null)
        return;
      EncompassApplication.CurrentLoan.Fields[targetFieldID].Value = EncompassApplication.CurrentLoan.Fields[sourceFieldID].Value;
    }

    [Macro]
    [Description("Opens a URL with parameters in the user's default Web Browser.")]
    public static void OpenURL([Description("URL"), Required] string url, [Description("Parameters"), Required] string parameters)
    {
      if (!Macro.IsLegalUrl(url))
        throw new ApplicationException("url contains potentially unsafe path: " + url);
      Process.Start(url + parameters);
    }

    [Macro]
    [Description("Opens a URL in the user's default Web Browser.")]
    public static void OpenURL([Description("URL"), Required] string url)
    {
      if (!Macro.IsLegalUrl(url))
        throw new ApplicationException("url contains potentially unsafe path: " + url);
      Process.Start(url);
    }

    private static bool IsLegalUrl(string urlInput)
    {
      Uri uri = new Uri(urlInput, UriKind.Absolute);
      return (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps) && Uri.IsWellFormedUriString(urlInput, UriKind.RelativeOrAbsolute);
    }

    [Description("Opens a new email message in the user's default mail application. You may specify multiple addressees by seperating them with commas.")]
    public static void OpenEMail([Description("Addressees"), Required] string addressees, [Description("Subject")] string subject)
    {
      Macro.OpenEMail(addressees, subject, "");
    }

    [Macro]
    [Description("Opens a new email message in the user's default mail application. You may specify multiple addressees by seperating them with commas.")]
    public static void OpenEMail([Description("Addressees"), Required] string addressees, [Description("Subject")] string subject, [Description("Body")] string body)
    {
      if (!Macro.IsLegalEmailAddress(addressees))
        throw new ApplicationException("Email Address contains potentially unsafe information or invalid format: " + addressees);
      Process.Start("mailto:" + HttpUtility.UrlEncode(addressees).Replace('+', ' ') + "?Subject=" + HttpUtility.UrlEncode(subject).Replace('+', ' ') + "&body=" + HttpUtility.UrlEncode(body).Replace('+', ' '));
    }

    private static bool IsLegalEmailAddress(string addressees)
    {
      return addressees.Length != 0 && addressees.IndexOf("@") > -1 && addressees.IndexOf(".", addressees.IndexOf("@")) > addressees.IndexOf("@");
    }

    [Macro]
    [Description("Launches an external application.")]
    public static void Run([Description("Program File/Path"), Required] string fileName, [Description("Command Line")] string arguments, [Description("Wait for Program to Exit")] bool waitForExit)
    {
      Process process = Macro.IsLegalProgramName(fileName) ? Process.Start(fileName, arguments) : throw new ApplicationException("path of the application contains potentially unsafe path , or invlaid filename or path: " + fileName);
      if (!waitForExit)
        return;
      process.WaitForExit();
    }

    private static bool IsLegalProgramName(string fileNameWithPath)
    {
      return !new Regex("[" + Regex.Escape(new string(Path.GetInvalidPathChars())) + "]").IsMatch(fileNameWithPath) && Path.IsPathRooted(fileNameWithPath) && new FileInfo(fileNameWithPath).Exists;
    }

    [Macro]
    [Description("Sends a sequence of keystrokes to the application. Use % to designate ALT, ^ for CONTROL and + for SHIFT.")]
    public static void SendKeys([Description("Key Sequence"), Required] string keySequence)
    {
      System.Windows.Forms.SendKeys.Send(keySequence);
    }

    [Macro]
    [Description("Opens the Ellie Mae Network Services Window. Use the Service Type parameter to specify which list of services should be displayed, e.g. \"Credit Report\".)")]
    public static void DisplayServices([Description("Service Type"), Required] string serviceType)
    {
      EncompassApplication.EPass.DisplayServices(serviceType);
    }

    [Macro]
    [Description("Provides a pop-up confirmation message to the user, who will be permitted to select OK or Cancel. This function returns a TRUE value if the user selects OK, FALSE otherwise.")]
    public static bool Confirm([Description("Message"), Required] string message)
    {
      return MessageBox.Show((IWin32Window) Session.MainScreen, message, "Encompass", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK;
    }

    [Macro]
    [Description("Provides a pop-up alert message to the user.")]
    public static void Alert([Description("Message"), Required] string message)
    {
      int num = (int) MessageBox.Show((IWin32Window) Session.MainScreen, message, "Encompass", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
    }

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
      string str = string.Concat(Macro.Eval(expression));
      if (str == "")
        return;
      ZipCodeInfo zipInfoAt = ZipCodeUtils.GetZipInfoAt(str);
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

    [Macro]
    [Description("Changes the currently visible screen in EncompassApplication.")]
    public static void GoToScreen([Description("Screen"), Required] EncompassScreen screen)
    {
      EncompassApplication.Screens[screen].MakeCurrent();
    }

    [Macro]
    [Description("Changes the currently visible input form in the Encompass loan editor.")]
    public static void GoToForm([Description("Form Name"), Required] string formName)
    {
      ((LoansScreen) EncompassApplication.Screens[EncompassScreen.Loans]).GoToForm(formName);
    }

    [Macro]
    [Description("Executes a standard action in the Encompass loan editor.")]
    public static void ExecAction([Description("Action"), Required] string action)
    {
      ((LoansScreen) EncompassApplication.Screens[EncompassScreen.Loans]).CurrentForm.ExecAction(action);
    }

    [Macro]
    [Description("Executes an Ellie Mae Network (EMN) Signature to invoke a partner service.")]
    public static void ExecSignature([Description("EMN Signature"), Required] string emnSignature)
    {
      ((EPassScreen) EncompassApplication.Screens[EncompassScreen.ePASS]).Execute(emnSignature);
    }

    [Macro]
    [Description("Gets the value of a loan field.")]
    public static string GetField([Description("Field ID"), Required] string fieldId)
    {
      return EncompassApplication.CurrentLoan != null ? EncompassApplication.CurrentLoan.Fields[fieldId].FormattedValue : "";
    }

    [Macro]
    [Description("Sets the value of a loan field.")]
    public static void SetField([Description("Field ID"), Required] string fieldId, [Description("Value")] string value)
    {
      if (EncompassApplication.CurrentLoan == null)
        return;
      EncompassApplication.CurrentLoan.Fields[fieldId].Value = (object) value;
    }

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

    [Macro]
    [Description("Sets the value of a loan field by evaluating the given expression.")]
    public static void SetFieldEval([Description("Field ID"), Required] string fieldId, [Description("Expression")] string expr)
    {
      Macro.SetField(fieldId, string.Concat(Macro.Eval(expr)));
    }

    [Macro]
    [Description("Evaluates an expression using the custom field calculation syntax.")]
    public static object Eval([Description("Expression"), Required] string expr)
    {
      try
      {
        using (RuntimeContext runtimeContext = RuntimeContext.Create())
        {
          using (CustomCalculationContext calculationContext = new CustomCalculationContext(Session.UserInfo, Session.LoanData, (IServerDataProvider) new CustomCodeSessionDataProvider(Session.SessionObjects)))
            return new CalculationBuilder().CreateImplementation(new CustomCalculation(expr), runtimeContext).Calculate((ICalculationContext) calculationContext);
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

    [Macro]
    [Description("Prints one or more forms for the current loan. For Standard Print Forms, enter the form's name, e.g. \"1003 Page 1\". For Custom Print Forms, enter the full path to the form, e.g. \"public:\\My Forms\\Appraisal Notification.doc\".")]
    public static void Print([Description("Form 1"), Required] string form1, [Description("Form 2")] string form2, [Description("Form 3")] string form3)
    {
      Macro.Print(new string[3]{ form1, form2, form3 });
    }

    [Description("Prints one or more forms for the current loan.")]
    public static void Print(params string[] formNames)
    {
      ILoanConsole service1 = Session.Application.GetService<ILoanConsole>();
      ILoanEditor service2 = Session.Application.GetService<ILoanEditor>();
      if (!service1.HasOpenLoan)
        throw new Exception("The loan screen must be open in order to print");
      service2.Print(formNames);
    }

    [Macro]
    [Description("Apply Input Form Business Rule, Loan Access Rule, Field Access Rule and Field Rule.")]
    public static void ApplyBusinessRule()
    {
      Session.Application.GetService<ILoanEditor>()?.ApplyBusinessRules();
    }
  }
}
