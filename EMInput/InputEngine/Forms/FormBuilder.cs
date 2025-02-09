// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.Forms.FormBuilder
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.Compiler;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.WebServices;
using EllieMae.Encompass.Forms;
using EllieMae.Encompass.Licensing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Services.Protocols;

#nullable disable
namespace EllieMae.EMLite.InputEngine.Forms
{
  public class FormBuilder
  {
    private const string className = "FormBuilder";
    private static string sw = Tracing.SwInputEngine;
    public static readonly TypeIdentifier AutomationFormTypeID = new TypeIdentifier("EncompassAutomation", "EllieMae.Encompass.Forms.Form");
    private string uniqueId = Guid.NewGuid().ToString("N");

    public TypeIdentifier Compile(FormDescriptor form, RuntimeContext context)
    {
      using (Tracing.StartTimer(FormBuilder.sw, nameof (FormBuilder), TraceLevel.Info, "Compiling input form '" + form.FormName + "'"))
      {
        FormParser formParser = this.createFormParser(form);
        if (!formParser.IsEMFRMFormat())
          throw new Exception("The form '" + form.FormName + "' has an invalid format.");
        if (formParser.RequiresAuthorization())
          this.authorizeForm(form, formParser.GetCodeBase());
        TypeIdentifier parentFormType = this.getParentFormType(formParser);
        TypeDescriptor parentType = (TypeDescriptor) null;
        if (parentFormType != null)
          parentType = this.loadParentTypeIntoContext(parentFormType, context);
        CodeLanguage languageForEvents = formParser.GetCodeLanguageForEvents();
        TypeIdentifier typeIdentifier = (TypeIdentifier) null;
        if (languageForEvents != CodeLanguage.None)
          typeIdentifier = this.generateDerivedPageType(form, formParser, parentType, languageForEvents, context);
        PerformanceMeter performanceMeter = PerformanceMeter.Get("InputForm.Load");
        int num1 = 0;
        int num2 = 0;
        int num3 = 0;
        try
        {
          CodeBase codeBase = formParser.GetCodeBase();
          if (performanceMeter.Active && (typeIdentifier != null || codeBase != null))
          {
            foreach (EventDescriptor eventDescriptor in formParser.GetEvents())
              num1 += this.getNoOfLinesInSourceCode(eventDescriptor.EventSourceCode);
            if (codeBase != null)
            {
              Assembly assembly = context.GetAssembly(formParser.GetCodeBase().AssemblyName);
              if (assembly != (Assembly) null)
              {
                IEnumerable<Type> source = ((IEnumerable<Type>) assembly.GetExportedTypes()).Where<Type>((Func<Type, bool>) (p => p.IsClass));
                num3 = source.Count<Type>();
                foreach (Type type in source)
                {
                  MethodInfo[] methods = type.GetType().GetMethods(BindingFlags.Static | BindingFlags.Public);
                  num2 += methods.Length;
                }
              }
            }
          }
          performanceMeter.AddVariable("# of lines of Adv Code", (object) num1.ToString());
          performanceMeter.AddVariable("# of classes", (object) num3);
          performanceMeter.AddVariable("# of methods", (object) num2);
        }
        catch (Exception ex)
        {
          Tracing.Log(FormBuilder.sw, nameof (FormBuilder), TraceLevel.Warning, "Error recording input form data for " + form.FormName + "'..." + ex.Message);
        }
        performanceMeter.Stop();
        return typeIdentifier ?? parentFormType ?? FormBuilder.AutomationFormTypeID;
      }
    }

    private int getNoOfLinesInSourceCode(string sourceCode)
    {
      string[] strArray = sourceCode.Replace(Environment.NewLine, "\r").Split('\r');
      return strArray.Length != 0 ? strArray.Length : 0;
    }

    private FormParser createFormParser(FormDescriptor des)
    {
      try
      {
        return new FormParser(des);
      }
      catch (Exception ex)
      {
        throw new Exception("The form '" + des.FormName + "' is invalid and cannot be parsed", ex);
      }
    }

    private TypeIdentifier generateDerivedPageType(
      FormDescriptor form,
      FormParser parser,
      TypeDescriptor parentType,
      CodeLanguage language,
      RuntimeContext context)
    {
      string newObjectName1 = this.createNewObjectName();
      string derivedPageScript = this.generateDerivedPageScript(newObjectName1, parser, parentType, language);
      if (string.IsNullOrEmpty(derivedPageScript))
        return (TypeIdentifier) null;
      string newObjectName2 = this.createNewObjectName();
      this.compileAssembly(derivedPageScript, newObjectName2, language, parentType, context);
      return new TypeIdentifier(newObjectName2, newObjectName1);
    }

    private void compileAssembly(
      string sourceCode,
      string assemblyName,
      CodeLanguage language,
      TypeDescriptor parentType,
      RuntimeContext context)
    {
      ArrayList arrayList = new ArrayList((ICollection) new string[5]
      {
        "System.dll",
        "System.Drawing.dll",
        "System.Windows.Forms.dll",
        "EncompassObjects.dll",
        "EncompassAutomation.dll"
      });
      if (parentType != null)
      {
        using (FormCache formCache = new FormCache(Session.DefaultInstance))
          arrayList.Add((object) formCache.FetchCustomFormAssembly(parentType.TypeID.AssemblyName));
      }
      context.CompileAssembly(assemblyName, sourceCode, language, (string[]) arrayList.ToArray(typeof (string)));
    }

    private string generateDerivedPageScript(
      string typeName,
      FormParser parser,
      TypeDescriptor parentType,
      CodeLanguage language)
    {
      ControlDescriptor[] controls = parser.GetControls(true);
      if (controls.Length == 0)
        return (string) null;
      EventDescriptor[] events = parser.GetEvents();
      if (events.Length == 0)
        return (string) null;
      return language == CodeLanguage.CSharp ? this.generateDerivedPageScriptInCSharp(typeName, parentType, controls, events) : this.generateDerivedPageScriptInVB(typeName, parentType, controls, events);
    }

    private string generateDerivedPageScriptInCSharp(
      string typeName,
      TypeDescriptor parentType,
      ControlDescriptor[] controls,
      EventDescriptor[] events)
    {
      CodeWriter codeWriter = new CodeWriter(CodeLanguage.CSharp);
      codeWriter.WriteLine("using System;");
      codeWriter.WriteLine("using System.Collections;");
      codeWriter.WriteLine("using System.Drawing;");
      codeWriter.WriteLine("using EllieMae.Encompass.Forms;");
      codeWriter.WriteLine("using EllieMae.Encompass.Automation;");
      codeWriter.WriteLine("using EllieMae.Encompass.BusinessEnums;");
      codeWriter.WriteLine("using EllieMae.Encompass.Client;");
      codeWriter.WriteLine("using EllieMae.Encompass.Collections;");
      codeWriter.WriteLine("using EllieMae.Encompass.BusinessObjects.Contacts;");
      codeWriter.WriteLine("using EllieMae.Encompass.BusinessObjects.Loans;");
      codeWriter.WriteLine("using EllieMae.Encompass.BusinessObjects.Loans.Logging;");
      codeWriter.WriteLine("using EllieMae.Encompass.BusinessObjects.Loans.Templates;");
      codeWriter.WriteLine("using EllieMae.Encompass.BusinessObjects.Loans.Servicing;");
      codeWriter.WriteLine("using EllieMae.Encompass.BusinessObjects.Users;");
      codeWriter.WriteLine("using EllieMae.Encompass.Configuration;");
      codeWriter.WriteLine("using EllieMae.Encompass.Query;");
      codeWriter.WriteLine("using EllieMae.Encompass.Reporting;");
      codeWriter.WriteLine();
      codeWriter.StartBlock("public class " + typeName + " : " + (parentType == null ? "EllieMae.Encompass.Forms.Form" : parentType.TypeID.ClassName));
      codeWriter.WriteCommentLine("Declaration of controls on the form");
      foreach (ControlDescriptor control in controls)
        codeWriter.WriteLine("private " + control.ControlTypeName + " " + control.ControlID + ";");
      codeWriter.WriteLine();
      codeWriter.WriteCommentLine("Override CreateControls method to build our control list and hook up event handlers");
      codeWriter.StartBlock("public override void CreateControls()");
      codeWriter.WriteLine("base.CreateControls();");
      codeWriter.WriteLine();
      foreach (ControlDescriptor control in controls)
        codeWriter.WriteLine(control.ControlID + " = (" + control.ControlTypeName + ") FindControl(\"" + control.ControlID + "\");");
      codeWriter.WriteLine();
      foreach (EventDescriptor e in events)
      {
        EventPrototype eventPrototype = this.getEventPrototype(e, controls);
        codeWriter.WriteLine(e.ControlID + "." + e.EventType + " += new " + eventPrototype.EventHandlerTypeName + "(__" + typeName + "_" + e.ControlID + "_" + e.EventType + ");");
      }
      codeWriter.EndBlock();
      foreach (EventDescriptor e in events)
      {
        EventPrototype eventPrototype = this.getEventPrototype(e, controls);
        codeWriter.WriteCommentLine("Event handler for " + e.ControlID + "." + e.EventType);
        codeWriter.StartBlock("private void __" + typeName + "_" + e.ControlID + "_" + e.EventType + "(object Sender, " + eventPrototype.EventArgsTypeName + " EventArgs)");
        codeWriter.StartRegion(e.ControlID + "." + e.EventType);
        codeWriter.WriteLine(e.EventSourceCode);
        codeWriter.EndRegion(e.ControlID + "." + e.EventType);
        codeWriter.EndBlock();
      }
      codeWriter.EndBlock();
      return codeWriter.ToString();
    }

    private string generateDerivedPageScriptInVB(
      string typeName,
      TypeDescriptor parentType,
      ControlDescriptor[] controls,
      EventDescriptor[] events)
    {
      CodeWriter codeWriter = new CodeWriter(CodeLanguage.VB);
      codeWriter.WriteLine("Imports System");
      codeWriter.WriteLine("Imports System.Collections");
      codeWriter.WriteLine("Imports System.Drawing");
      codeWriter.WriteLine("Imports Microsoft.VisualBasic");
      codeWriter.WriteLine("Imports EllieMae.Encompass.Forms");
      codeWriter.WriteLine("Imports EllieMae.Encompass.Automation");
      codeWriter.WriteLine("Imports EllieMae.Encompass.BusinessEnums");
      codeWriter.WriteLine("Imports EllieMae.Encompass.Client");
      codeWriter.WriteLine("Imports EllieMae.Encompass.Collections");
      codeWriter.WriteLine("Imports EllieMae.Encompass.BusinessObjects.Contacts");
      codeWriter.WriteLine("Imports EllieMae.Encompass.BusinessObjects.Loans");
      codeWriter.WriteLine("Imports EllieMae.Encompass.BusinessObjects.Loans.Logging");
      codeWriter.WriteLine("Imports EllieMae.Encompass.BusinessObjects.Loans.Templates");
      codeWriter.WriteLine("Imports EllieMae.Encompass.BusinessObjects.Loans.Servicing");
      codeWriter.WriteLine("Imports EllieMae.Encompass.BusinessObjects.Users");
      codeWriter.WriteLine("Imports EllieMae.Encompass.Configuration");
      codeWriter.WriteLine("Imports EllieMae.Encompass.Query");
      codeWriter.WriteLine("Imports EllieMae.Encompass.Reporting");
      codeWriter.WriteLine();
      codeWriter.StartBlock("Public Class " + typeName);
      codeWriter.WriteLine("Inherits " + (parentType == null ? "EllieMae.Encompass.Forms.Form" : parentType.TypeID.ClassName));
      codeWriter.WriteLine();
      codeWriter.WriteCommentLine("Declaration of controls on the form");
      foreach (ControlDescriptor control in controls)
        codeWriter.WriteLine("Private " + control.ControlID + " as " + control.ControlTypeName);
      codeWriter.WriteLine();
      codeWriter.WriteCommentLine("Override CreateControls method to build our control list and hook up event handlers");
      codeWriter.StartBlock("Public Overrides Sub CreateControls()");
      codeWriter.WriteLine("MyBase.CreateControls()");
      codeWriter.WriteLine();
      foreach (ControlDescriptor control in controls)
        codeWriter.WriteLine(control.ControlID + " = CType(FindControl(\"" + control.ControlID + "\"), " + control.ControlTypeName + ")");
      codeWriter.WriteLine();
      foreach (EventDescriptor eventDescriptor in events)
        codeWriter.WriteLine("AddHandler " + eventDescriptor.ControlID + "." + eventDescriptor.EventType + ", AddressOf __" + typeName + "_" + eventDescriptor.ControlID + "_" + eventDescriptor.EventType);
      codeWriter.EndBlock("End Sub");
      foreach (EventDescriptor e in events)
      {
        EventPrototype eventPrototype = this.getEventPrototype(e, controls);
        codeWriter.WriteCommentLine("Event handler for " + e.ControlID + "." + e.EventType);
        codeWriter.StartBlock("Private Sub __" + typeName + "_" + e.ControlID + "_" + e.EventType + "(Sender as Object, EventArgs as " + eventPrototype.EventArgsTypeName + ")");
        codeWriter.StartRegion(e.ControlID + "." + e.EventType);
        codeWriter.WriteLine(e.EventSourceCode);
        codeWriter.EndRegion(e.ControlID + "." + e.EventType);
        codeWriter.EndBlock("End Sub");
      }
      codeWriter.EndBlock("End Class");
      return codeWriter.ToString();
    }

    private EventPrototype getEventPrototype(EventDescriptor e, ControlDescriptor[] controls)
    {
      foreach (ControlDescriptor control in controls)
      {
        if (control.ControlID == e.ControlID)
          return new EventPrototype(control.GetControlType().GetEvent(e.EventType, BindingFlags.Instance | BindingFlags.Public));
      }
      throw new Exception("An event with the name '" + e.EventType + "' does not exist for control '" + e.ControlID + "'");
    }

    private TypeDescriptor loadParentTypeIntoContext(TypeIdentifier typeId, RuntimeContext context)
    {
      string str = (string) null;
      using (FormCache formCache = new FormCache(Session.DefaultInstance))
        str = formCache.FetchCustomFormAssembly(typeId.AssemblyName);
      AssemblyName assemblyName1 = AssemblyName.GetAssemblyName(str);
      if (assemblyName1 == null)
        return (TypeDescriptor) null;
      AssemblyName assemblyName2 = (AssemblyName) null;
      try
      {
        assemblyName2 = context.GetAssemblyName(typeId.AssemblyName);
      }
      catch
      {
      }
      if (!assemblyName1.Equals((object) assemblyName2))
      {
        using (BinaryObject binaryObject = new BinaryObject(str))
          context.LoadAssembly(typeId.AssemblyName, binaryObject.GetBytes());
      }
      try
      {
        return context.GetTypeInformation(typeId);
      }
      catch
      {
        return (TypeDescriptor) null;
      }
    }

    private void authorizeForm(FormDescriptor form, CodeBase formCodeBase)
    {
      try
      {
        Tracing.Log(FormBuilder.sw, nameof (FormBuilder), TraceLevel.Verbose, "Authorizing Encompass form '" + form.FormName + "'...");
        string formSourcePath = FormExtractor.GetFormSourcePath(form.FormID);
        string str1 = formSourcePath != null && File.Exists(formSourcePath) ? AuthenticationUtils.ComputeCRC(formSourcePath) : throw new Exception("Source file for form '" + form.FormID + "' could not be located");
        string str2 = (string) null;
        if (formCodeBase != null)
        {
          using (FormCache formCache = new FormCache(Session.DefaultInstance))
          {
            string path = formCache.FetchCustomFormAssembly(formCodeBase.AssemblyName);
            if ((path ?? "") != "")
              str2 = AuthenticationUtils.ComputeCRC(path);
          }
        }
        if (AuthenticationUtils.IsPreauthorized(str1, PreauthorizedModuleType.CustomForm, Session.StartupInfo.PreauthorizedModules))
        {
          if (str2 == null)
          {
            Tracing.Log(FormBuilder.sw, nameof (FormBuilder), TraceLevel.Verbose, "Encompass form '" + form.FormName + "' is pre-authorized.");
            return;
          }
          if (AuthenticationUtils.IsPreauthorized(str2, PreauthorizedModuleType.CodebaseAssembly, Session.StartupInfo.PreauthorizedModules))
          {
            Tracing.Log(FormBuilder.sw, nameof (FormBuilder), TraceLevel.Verbose, "Encompass form '" + form.FormName + "' and its codebase asembly are pre-authorized.");
            return;
          }
        }
        string passPhrase = Session.SessionObjects.SystemID + "|" + str1 + "|" + str2;
        using (PluginService pluginService = new PluginService(Session.SessionObjects?.StartupInfo?.ServiceUrls?.JedServicesUrl))
        {
          Tracing.Log(FormBuilder.sw, nameof (FormBuilder), TraceLevel.Verbose, "Invoking form authorization service for '" + form.FormName + "'...");
          pluginService.Timeout = 5000;
          pluginService.AuthorizeForm(Session.CompanyInfo.ClientID, Environment.MachineName, Environment.UserName, form.FormName, str1, formCodeBase == null ? (string) null : formCodeBase.AssemblyName, formCodeBase == null ? (string) null : formCodeBase.ClassName, str2, passPhrase);
          Tracing.Log(FormBuilder.sw, nameof (FormBuilder), TraceLevel.Verbose, "Authorization validated successfully for '" + form.FormName + "'.");
        }
      }
      catch (SoapException ex)
      {
        Tracing.Log(FormBuilder.sw, nameof (FormBuilder), TraceLevel.Error, "Custom form authorization failed for '" + form.FormName + "'. Reason resported = '" + ex.Message + "'.");
        if (ex.ToString().IndexOf("Use of this form within Encompass is not permitted") >= 0)
        {
          if (ex.Message.IndexOf("--> ") > 0)
            throw new LicenseException(ex.Message.Substring(ex.Message.IndexOf("--> ") + 4), (Exception) ex);
          throw new LicenseException(ex.Message, (Exception) ex);
        }
        if (ex.Message.ToString().IndexOf("--> System.Security.Cryptography.CryptographicException: CryptoAPI cryptographic service provider (CSP) for this implementation could not be acquired.") < 0 || ex.Message.IndexOf("--> System.Security.Cryptography.CryptographicException: CryptoAPI cryptographic service provider (CSP) for this implementation could not be acquired.") < 0)
          return;
        Tracing.Log(FormBuilder.sw, nameof (FormBuilder), TraceLevel.Warning, "Cryptographic exception: " + ex.Message);
      }
      catch (Exception ex)
      {
        Tracing.Log(FormBuilder.sw, nameof (FormBuilder), TraceLevel.Error, "Custom form authorization failed for '" + form.FormName + "'. Reason resported = '" + ex.Message + "'.");
      }
    }

    private TypeIdentifier getParentFormType(FormParser parser)
    {
      CodeBase codeBase = parser.GetCodeBase();
      return codeBase == null ? (TypeIdentifier) null : new TypeIdentifier(codeBase.AssemblyName, codeBase.ClassName);
    }

    private string createNewObjectName() => "fb" + Guid.NewGuid().ToString("N");
  }
}
