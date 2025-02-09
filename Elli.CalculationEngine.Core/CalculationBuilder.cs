// Decompiled with JetBrains decompiler
// Type: Elli.CalculationEngine.Core.CalculationBuilder
// Assembly: Elli.CalculationEngine.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E7988E98-462C-4B95-BC53-687EC5965B19
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.CalculationEngine.Core.dll

using Elli.CalculationEngine.Core.CalculationLibrary;
using Elli.CalculationEngine.Core.DataSource;
using Elli.CalculationEngine.Core.ExpressionParser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

#nullable disable
namespace Elli.CalculationEngine.Core
{
  public class CalculationBuilder
  {
    private CodeWriter assemblyCode;
    private string assemblyId;
    private string _objectModelRoot = string.Empty;
    private string _objectModelNamespace = string.Empty;
    private string _objectModelAssemblyName = string.Empty;
    private Assembly _objectModelAssembly;
    private Version version;

    public CalculationBuilder(
      string parentId,
      string entityWrapperNamespace,
      Version version,
      string objectModelRoot,
      string objectModelNamespace,
      string assemblyPath,
      string objectModelAssemblyName)
    {
      this._objectModelRoot = objectModelRoot;
      this._objectModelNamespace = objectModelNamespace;
      if (!string.IsNullOrEmpty(assemblyPath) && !string.IsNullOrEmpty(objectModelAssemblyName))
      {
        this._objectModelAssemblyName = objectModelAssemblyName;
        this._objectModelAssembly = Assembly.LoadFrom(Path.Combine(assemblyPath, this._objectModelAssemblyName));
      }
      this.Clear(parentId, entityWrapperNamespace, version);
    }

    public Version Compile(
      RuntimeContext context,
      string assemblyPath,
      string calculationEngineDllName,
      bool saveAssembly = true,
      bool optionStrict = false,
      bool includeDebugInfo = false)
    {
      string path = Assembly.GetExecutingAssembly().CodeBase;
      if (path.IndexOf("file:///") >= 0)
        path = path.Substring(8);
      List<string> stringList = new List<string>();
      stringList.Add(path);
      stringList.Add("System.Core.dll");
      stringList.Add("System.Data.DataSetExtensions.dll");
      stringList.Add("System.dll");
      stringList.Add("System.Data.dll");
      stringList.Add("System.Data.Linq.dll");
      stringList.Add("System.Deployment.dll");
      stringList.Add("System.Xml.dll");
      stringList.Add("System.Xml.Linq.dll");
      stringList.Add("System.Numerics.dll");
      if (this.assemblyId == "EncompassCalculationSet")
        stringList.Add(Path.Combine(assemblyPath, "Elli.CalculationEngineIntegration.dll"));
      if (!string.IsNullOrEmpty(this._objectModelAssemblyName))
      {
        string str = Path.Combine(assemblyPath, this._objectModelAssemblyName);
        stringList.Add(str);
      }
      if (File.Exists(Path.Combine(assemblyPath, calculationEngineDllName)))
        stringList.Add(Path.Combine(assemblyPath, calculationEngineDllName));
      else
        stringList.Add(Path.Combine(Path.GetDirectoryName(path), calculationEngineDllName));
      return context.CompileAssembly(this.assemblyId, this.assemblyCode.ToString(), CodeLanguage.VB, stringList.ToArray(), new string[0], assemblyPath, saveAssembly, includeDebugInfo: includeDebugInfo);
    }

    public void Clear(string parentId, string entityWrapperNamespace, Version version)
    {
      this.assemblyId = parentId;
      this.assemblyCode = new CodeWriter(CodeLanguage.VB);
      this.assemblyCode.WriteLine("Imports System");
      this.assemblyCode.WriteLine("Imports System.Collections");
      this.assemblyCode.WriteLine("Imports System.Collections.Generic");
      this.assemblyCode.WriteLine("Imports System.Diagnostics");
      this.assemblyCode.WriteLine("Imports System.Data");
      this.assemblyCode.WriteLine("Imports System.Dynamic");
      this.assemblyCode.WriteLine("Imports System.Linq");
      this.assemblyCode.WriteLine("Imports System.Text.RegularExpressions");
      this.assemblyCode.WriteLine("Imports System.Threading.Tasks");
      this.assemblyCode.WriteLine("Imports System.Reflection");
      this.assemblyCode.WriteLine("Imports System.Xml.Linq");
      this.assemblyCode.WriteLine("Imports System.Numerics");
      this.assemblyCode.WriteLine("Imports Microsoft.VisualBasic");
      this.assemblyCode.WriteLine("Imports TransientDataObjects");
      if (this.assemblyId == "EncompassCalculationSet")
        this.assemblyCode.WriteLine("Imports Elli.Domain.Extensions");
      if (!string.IsNullOrEmpty(this._objectModelNamespace))
        this.assemblyCode.WriteLine(string.Format("Imports {0}", (object) this._objectModelNamespace));
      this.assemblyCode.WriteLine("Imports Elli.CalculationEngineIntegration");
      this.assemblyCode.WriteLine("Imports Elli.CalculationEngine.Core");
      this.assemblyCode.WriteLine("Imports Elli.CalculationEngine.Core.DataSource");
      this.assemblyCode.WriteLine(string.Format("Imports Elli.CalculationEngine.{0}", (object) entityWrapperNamespace));
      if (version == (Version) null)
      {
        this.version = Assembly.GetExecutingAssembly().GetName().Version;
        this.assemblyCode.WriteLine(string.Format("<assembly: System.Reflection.AssemblyFileVersion(\"{0}.{1}.0.0\")>", (object) this.version.Major, (object) this.version.MajorRevision));
      }
      else
      {
        this.version = version;
        this.assemblyCode.WriteLine(string.Format("<assembly: System.Reflection.AssemblyFileVersion(\"{0}\")>", (object) this.version));
      }
    }

    public void AddCalculation(CalculationSetElement element)
    {
      this.assemblyCode.StartBlock("Public Class " + element.Identity.ClassName);
      this.assemblyCode.WriteLine("Inherits CalculationFunctionImplBase");
      this.assemblyCode.StartBlock("Protected Overrides Function ExecuteCalculation(Of T)(obj As T, ByRef executionContext As ICalculationContext) As Object");
      EntityDescriptor entityDescriptor = FieldReplacementRegex.ParseEntityDescriptor(element.ParentEntityType);
      this.assemblyCode.WriteLine("Dim context As ICalculationContext = executionContext");
      this.assemblyCode.WriteLine(string.Format("Dim {0}Entity As I{0}Entity = CType(context.ContextRootEntity, I{0}Entity)", (object) entityDescriptor.EntityType));
      if (this._objectModelAssembly != (Assembly) null)
      {
        this.assemblyCode.WriteLine(string.Format("Dim {0} As {1} = CTypeDynamic(Of {2}.{1})(obj)", (object) this._objectModelRoot, (object) CalculationUtility.GetRootEntityType(), (object) this._objectModelNamespace));
        if (this.assemblyId == "EncompassCalculationSet" && entityDescriptor.EntityType != "RegulationZPaymentsPopulated" && entityDescriptor.EntityType != "Settings" && entityDescriptor.EntityType != "VerifContact" && entityDescriptor.EntityType != "PaymentScheduleSnapshot" && entityDescriptor.EntityType != "PaymentSchedule" && entityDescriptor.EntityType != "EscrowSchedule" && entityDescriptor.EntityType != "CurrentLockRequest")
          this.assemblyCode.WriteLine(string.Format("Dim {0}ContextRoot as Elli.Domain.Mortgage.{0} = CType(CType(context.ContextRootEntity.Entity, Elli.CalculationEngineIntegration.CalculationDataSource).ModelObject, Elli.Domain.Mortgage.{0})", (object) entityDescriptor.EntityType));
      }
      this.assemblyCode.StartBlock("Try");
      this.assemblyCode.StartRegion(element.Identity.ToString(), 7);
      FieldExpressionCalculation expressionCalculation = (FieldExpressionCalculation) element;
      if (expressionCalculation.IsMultiLineCalculation)
      {
        foreach (string sourceCode in expressionCalculation.Expression.ToSourceCodeArray(expressionCalculation.Name, this._objectModelAssembly))
          this.assemblyCode.WriteLine(sourceCode);
      }
      else
        this.assemblyCode.WriteLine("Return " + expressionCalculation.Expression.ToSourceCode(expressionCalculation.Name, this._objectModelAssembly));
      this.assemblyCode.EndBlock();
      this.assemblyCode.StartBlock("Catch ex As Exception");
      if (this._objectModelAssembly != (Assembly) null && this.assemblyId == "EncompassCalculationSet")
        this.assemblyCode.WriteLine("LogData(context, model)");
      this.assemblyCode.WriteLine("Dim st As StackTrace = New StackTrace(ex, True)");
      this.assemblyCode.WriteLine("Dim sf As StackFrame = st.GetFrame(st.FrameCount - 1)");
      this.assemblyCode.WriteLine("Throw New Exception(\"An exception was thrown by " + element.Name + " Calculation: " + element.Identity.ClassName + ", Line Number \" & sf.GetFileLineNumber(), ex)");
      this.assemblyCode.EndBlock("End Try");
      this.assemblyCode.EndRegion(element.Identity.ToString());
      this.assemblyCode.EndBlock("End Function");
      if (this._objectModelAssembly == (Assembly) null)
      {
        this.assemblyCode.StartBlock(string.Format("Protected ReadOnly Property {0}Entity as I{0}Entity", (object) entityDescriptor.EntityType));
        this.assemblyCode.StartBlock("Get");
        this.assemblyCode.WriteLine(string.Format("Return CType(Context.ContextRootEntity, I{0}Entity)", (object) entityDescriptor.EntityType));
        this.assemblyCode.EndBlock("End Get");
        this.assemblyCode.EndBlock("End Property");
      }
      this.assemblyCode.EndBlock("End Class");
    }

    public void AddFunctions(List<CalculationSetElement> elements)
    {
      this.assemblyCode.StartBlock("Public Class CalculationFunctionImplBase");
      this.assemblyCode.WriteLine("Inherits CalculationImplBase");
      foreach (CalculationSetElement element in elements)
      {
        CalculationSetElement calculationSetElement;
        string[] sourceCodeArray = ((Function) (calculationSetElement = element)).Expression.ToSourceCodeArray(string.Empty, (Assembly) null);
        string descriptiveName = calculationSetElement.DescriptiveName;
        for (int index = 0; index < sourceCodeArray.Length; ++index)
        {
          if (index == 0)
          {
            this.assemblyCode.StartBlock("Public Shared " + sourceCodeArray[index]);
            this.assemblyCode.StartBlock("Try");
          }
          else if (index == sourceCodeArray.Length - 1)
          {
            this.assemblyCode.EndBlock();
            this.assemblyCode.StartBlock("Catch ex As Exception");
            this.assemblyCode.WriteLine("Dim st As StackTrace = New StackTrace(ex)");
            this.assemblyCode.WriteLine("Dim sf As StackFrame = st.GetFrame(st.FrameCount - 1)");
            this.assemblyCode.WriteLine("Throw New Exception(\"An exception was thrown by " + descriptiveName + ", Line: \", ex)");
            this.assemblyCode.EndBlock("End Try");
            this.assemblyCode.EndBlock(sourceCodeArray[index]);
          }
          else
            this.assemblyCode.WriteLine(sourceCodeArray[index]);
        }
      }
      this.assemblyCode.StartBlock("Protected Overrides Function ExecuteCalculation(Of T)(obj As T, ByRef executionContext As ICalculationContext) As Object");
      this.assemblyCode.WriteLine("Return \" \"");
      this.assemblyCode.EndBlock("End Function");
      this.assemblyCode.EndBlock("End Class");
    }

    public void AddTransientDataObjects(List<CalculationSetElement> elements)
    {
      this.assemblyCode.StartBlock("Namespace TransientDataObjects");
      foreach (CalculationSetElement element in elements)
      {
        CalculationSetElement calculationSetElement;
        string[] sourceCodeArray = ((TransientDataObject) (calculationSetElement = element)).Expression.ToSourceCodeArray(string.Empty, this._objectModelAssembly);
        string descriptiveName = calculationSetElement.DescriptiveName;
        for (int index = 0; index < sourceCodeArray.Length; ++index)
        {
          if (index == 0)
            this.assemblyCode.StartBlock(sourceCodeArray[index]);
          else if (index == sourceCodeArray.Length - 1)
            this.assemblyCode.EndBlock(sourceCodeArray[index]);
          else
            this.assemblyCode.WriteLine(sourceCodeArray[index]);
        }
      }
      this.assemblyCode.EndBlock("End Namespace");
    }
  }
}
