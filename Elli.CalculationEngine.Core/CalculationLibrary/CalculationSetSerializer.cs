// Decompiled with JetBrains decompiler
// Type: Elli.CalculationEngine.Core.CalculationLibrary.CalculationSetSerializer
// Assembly: Elli.CalculationEngine.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E7988E98-462C-4B95-BC53-687EC5965B19
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.CalculationEngine.Core.dll

using Elli.CalculationEngine.Common;
using Elli.CalculationEngine.Core.DataSource;
using Elli.CalculationEngine.Core.ExpressionParser;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Linq;

#nullable disable
namespace Elli.CalculationEngine.Core.CalculationLibrary
{
  public sealed class CalculationSetSerializer
  {
    private const string className = "CalculationSetSerializer";
    private DataContractSerializer elementSerializer = new DataContractSerializer(typeof (CalculationSetElement));
    private CalculationSet calculationSet = new CalculationSet(Guid.NewGuid());
    private string currentExpressionType = "All";
    private string currentEnabledFilter = "All";
    private string currentParentEntityTypeFilter = "All";
    private string currentTransientFilter = "All";
    private string currentLibraryName = string.Empty;
    private string currentAssemblyOutputPath;
    private string currentCalculationLibraryPath;

    public CalculationSetSerializer()
    {
      this.CurrentAssemblyLibraryPath = string.Empty;
      this.CurrentAssemblyOutputPath = string.Empty;
      this.CurrentCalculationLibraryPath = string.Empty;
    }

    public string GetCalculationElementFilePath(Guid id, string elementName)
    {
      return Path.Combine(this.CurrentCalculationLibraryPath, string.Format("{0}.xml", id.ToString() == elementName ? (object) elementName : (object) string.Format("{0}.{1}", (object) elementName, (object) id.ToString())));
    }

    public CalculationSet CalculationSet
    {
      get
      {
        if (this.calculationSet.Elements == null || this.calculationSet.Elements.Count == 0 || this.calculationSet.Name != this.CurrentCalculationLibraryName)
        {
          if (this.calculationSet.Elements == null)
            this.calculationSet.Elements = new List<CalculationSetElement>();
          this.LoadCalculationElements();
          Tracing.Log(TraceLevel.Info, nameof (CalculationSetSerializer), string.Format("CalculationSet created for {0}", (object) this.CurrentCalculationLibraryName));
        }
        return this.calculationSet;
      }
    }

    public string CurrentEnabledFilter => this.currentEnabledFilter;

    public string CurrentExpressionType => this.currentExpressionType;

    public string CurrentCalculationLibraryName => this.currentLibraryName;

    public string CurrentAssemblyLibraryPath { get; set; }

    public string CurrentAssemblyOutputPath
    {
      get => this.currentAssemblyOutputPath;
      set
      {
        this.currentAssemblyOutputPath = value;
        Utility.AddPathToWhiteList(this.currentAssemblyOutputPath);
      }
    }

    public string CurrentCalculationLibraryPath
    {
      get => this.currentCalculationLibraryPath;
      set
      {
        this.currentCalculationLibraryPath = value;
        Utility.AddPathToWhiteList(this.currentCalculationLibraryPath);
      }
    }

    public string CurrentParentEntityTypeFilter => this.currentParentEntityTypeFilter;

    public string CurrentTransientFilter => this.currentTransientFilter;

    public CalculationSetElement GetCalculationElement(string elementId)
    {
      return this.GetCalculationElement(elementId, string.Empty);
    }

    public CalculationSetElement GetCalculationElement(string elementId, bool refreshElement)
    {
      return this.GetCalculationElement(elementId, string.Empty, refreshElement: refreshElement);
    }

    public CalculationSetElement GetCalculationElement(
      string elementId,
      string parentEntityType,
      bool useName = false,
      bool refreshElement = false)
    {
      Tracing.Log(TraceLevel.Info, nameof (CalculationSetSerializer), string.Format("Get Calculation Element for ParentEntityType: {0}, ElementId: {1}", (object) parentEntityType, (object) elementId));
      if (this.calculationSet.Elements == null || this.calculationSet.Elements.Count == 0)
        this.LoadCalculationElements();
      if (refreshElement)
      {
        CalculationSetElement calculationSetElement = useName ? this.calculationSet.Elements.Find((Predicate<CalculationSetElement>) (c => c.Name == elementId && c.ParentEntityType == parentEntityType)) : this.calculationSet.Elements.Find((Predicate<CalculationSetElement>) (c => c.Identity.Id == new Guid(elementId)));
        this.RefreshCalculationElement(calculationSetElement.Identity.Id, calculationSetElement.Name);
      }
      return !useName ? this.calculationSet.Elements.Find((Predicate<CalculationSetElement>) (c => c.Identity.Id == new Guid(elementId))) : this.calculationSet.Elements.Find((Predicate<CalculationSetElement>) (c => c.Name == elementId && c.ParentEntityType == parentEntityType));
    }

    public List<CalculationSetElement> GetCurrentLibrary()
    {
      return this.GetLibrary(this.currentLibraryName, this.currentExpressionType, this.currentEnabledFilter, this.currentParentEntityTypeFilter, this.currentTransientFilter);
    }

    public List<CalculationSetElement> GetLibrary(
      string name,
      string expressionType,
      string enabled = "All",
      string parentEntityType = "All",
      string transient = "All",
      bool replaceTemplates = false)
    {
      Tracing.Log(TraceLevel.Info, nameof (CalculationSetSerializer), string.Format("Get Library for {0}; ExpressionType: {1}; Enabled: {2}; Transient: {3}", (object) name, (object) expressionType, (object) enabled, (object) transient));
      this.currentExpressionType = expressionType;
      this.currentEnabledFilter = enabled;
      this.currentParentEntityTypeFilter = parentEntityType;
      this.currentTransientFilter = transient;
      LibraryElementType enumElementType = LibraryElementType.None;
      if (this.currentExpressionType != "All")
      {
        ExpressionType expressionType1 = string.IsNullOrEmpty(expressionType) ? ExpressionType.None : (ExpressionType) Enum.Parse(typeof (ExpressionType), expressionType);
        int num;
        if (!string.IsNullOrEmpty(expressionType))
        {
          switch (expressionType1)
          {
            case ExpressionType.Calculation:
              num = 2;
              break;
            case ExpressionType.Function:
              num = 4;
              break;
            case ExpressionType.TransientDataObject:
              num = 5;
              break;
            default:
              num = 3;
              break;
          }
        }
        else
          num = 0;
        enumElementType = (LibraryElementType) num;
      }
      this.currentLibraryName = name;
      this.LoadCalculationElements();
      if (this.calculationSet != null)
      {
        if (this.calculationSet.RootEntityType != (EntityDescriptor) null)
          CalculationUtility.SetRootEntityType(this.calculationSet.RootEntityType.ToString());
        CalculationUtility.SetRootRelationship(this.calculationSet.RootRelationship);
        CalculationUtility.SetObjectModelRoot(this.calculationSet.ObjectModelRoot);
        CalculationUtility.SetObjectModelNamespace(this.calculationSet.ObjectModelNamespace);
      }
      if (replaceTemplates)
      {
        this.calculationSet = this.CalculationSet.ExpandCalculations(this.calculationSet);
        CalculationSet.AddParameterizedReferences(ref this.calculationSet);
      }
      if (this.currentExpressionType == "All" && enabled == "All" && parentEntityType == "All" && transient == "All")
        return this.calculationSet.Elements.OrderBy<CalculationSetElement, string>((Func<CalculationSetElement, string>) (calc => calc.Name)).ToList<CalculationSetElement>();
      if (this.currentExpressionType == "All" && enabled != "All" && parentEntityType == "All" && transient == "All")
        return this.calculationSet.Elements.Where<CalculationSetElement>((Func<CalculationSetElement, bool>) (calc => calc.Enabled.ToString() == enabled)).OrderBy<CalculationSetElement, string>((Func<CalculationSetElement, string>) (calc => calc.Name)).ToList<CalculationSetElement>();
      if (this.currentExpressionType == "All" && enabled == "All" && parentEntityType != "All" && transient == "All")
        return this.calculationSet.Elements.Where<CalculationSetElement>((Func<CalculationSetElement, bool>) (calc => calc.ParentEntityType == parentEntityType)).OrderBy<CalculationSetElement, string>((Func<CalculationSetElement, string>) (calc => calc.Name)).ToList<CalculationSetElement>();
      if (this.currentExpressionType == "All" && enabled == "All" && parentEntityType == "All" && transient != "All")
        return transient == bool.TrueString ? this.calculationSet.GetAllFieldExpressionCalculations().ToList<FieldExpressionCalculation>().Where<FieldExpressionCalculation>((Func<FieldExpressionCalculation, bool>) (calc => calc.IsTransient.ToString() == transient)).OrderBy<FieldExpressionCalculation, string>((Func<FieldExpressionCalculation, string>) (calc => calc.Name)).Cast<CalculationSetElement>().ToList<CalculationSetElement>() : this.calculationSet.Elements.Except<CalculationSetElement>((IEnumerable<CalculationSetElement>) this.calculationSet.GetAllFieldExpressionCalculations().ToList<FieldExpressionCalculation>().Where<FieldExpressionCalculation>((Func<FieldExpressionCalculation, bool>) (calc => calc.IsTransient)).OrderBy<FieldExpressionCalculation, string>((Func<FieldExpressionCalculation, string>) (calc => calc.Name)).Cast<CalculationSetElement>().ToList<CalculationSetElement>()).OrderBy<CalculationSetElement, string>((Func<CalculationSetElement, string>) (calc => calc.Name)).ToList<CalculationSetElement>();
      if (this.currentExpressionType == "All" && enabled != "All" && parentEntityType != "All" && transient == "All")
        return this.calculationSet.Elements.Where<CalculationSetElement>((Func<CalculationSetElement, bool>) (calc => calc.Enabled.ToString() == enabled && calc.ParentEntityType == parentEntityType)).OrderBy<CalculationSetElement, string>((Func<CalculationSetElement, string>) (calc => calc.Name)).ToList<CalculationSetElement>();
      if (this.currentExpressionType == "All" && enabled != "All" && parentEntityType == "All" && transient != "All")
      {
        if (transient == bool.TrueString)
          return this.calculationSet.GetAllFieldExpressionCalculations().ToList<FieldExpressionCalculation>().Where<FieldExpressionCalculation>((Func<FieldExpressionCalculation, bool>) (calc => calc.Enabled.ToString() == enabled && calc.IsTransient.ToString() == transient)).OrderBy<FieldExpressionCalculation, string>((Func<FieldExpressionCalculation, string>) (calc => calc.Name)).Cast<CalculationSetElement>().ToList<CalculationSetElement>();
        List<CalculationSetElement> list = this.calculationSet.GetAllFieldExpressionCalculations().ToList<FieldExpressionCalculation>().Where<FieldExpressionCalculation>((Func<FieldExpressionCalculation, bool>) (calc => calc.IsTransient)).OrderBy<FieldExpressionCalculation, string>((Func<FieldExpressionCalculation, string>) (calc => calc.Name)).Cast<CalculationSetElement>().ToList<CalculationSetElement>();
        return this.calculationSet.Elements.Where<CalculationSetElement>((Func<CalculationSetElement, bool>) (calc => calc.Enabled.ToString() == enabled)).Except<CalculationSetElement>((IEnumerable<CalculationSetElement>) list).OrderBy<CalculationSetElement, string>((Func<CalculationSetElement, string>) (calc => calc.Name)).ToList<CalculationSetElement>();
      }
      if (this.currentExpressionType == "All" && enabled == "All" && parentEntityType != "All" && transient != "All")
      {
        if (transient == bool.TrueString)
          return this.calculationSet.GetAllFieldExpressionCalculations().ToList<FieldExpressionCalculation>().Where<FieldExpressionCalculation>((Func<FieldExpressionCalculation, bool>) (calc => calc.ParentEntityType == parentEntityType && calc.IsTransient.ToString() == transient)).OrderBy<FieldExpressionCalculation, string>((Func<FieldExpressionCalculation, string>) (calc => calc.Name)).Cast<CalculationSetElement>().ToList<CalculationSetElement>();
        List<CalculationSetElement> list = this.calculationSet.GetAllFieldExpressionCalculations().ToList<FieldExpressionCalculation>().Where<FieldExpressionCalculation>((Func<FieldExpressionCalculation, bool>) (calc => calc.IsTransient)).OrderBy<FieldExpressionCalculation, string>((Func<FieldExpressionCalculation, string>) (calc => calc.Name)).Cast<CalculationSetElement>().ToList<CalculationSetElement>();
        return this.calculationSet.Elements.Where<CalculationSetElement>((Func<CalculationSetElement, bool>) (calc => calc.ParentEntityType == parentEntityType)).Except<CalculationSetElement>((IEnumerable<CalculationSetElement>) list).OrderBy<CalculationSetElement, string>((Func<CalculationSetElement, string>) (calc => calc.Name)).ToList<CalculationSetElement>();
      }
      if (this.currentExpressionType == "All" && enabled != "All" && parentEntityType != "All" && transient != "All")
      {
        if (transient == bool.TrueString)
          return this.calculationSet.GetAllFieldExpressionCalculations().ToList<FieldExpressionCalculation>().Where<FieldExpressionCalculation>((Func<FieldExpressionCalculation, bool>) (calc => calc.Enabled.ToString() == enabled && calc.ParentEntityType == parentEntityType && calc.IsTransient.ToString() == transient)).OrderBy<FieldExpressionCalculation, string>((Func<FieldExpressionCalculation, string>) (calc => calc.Name)).Cast<CalculationSetElement>().ToList<CalculationSetElement>();
        List<CalculationSetElement> list = this.calculationSet.GetAllFieldExpressionCalculations().ToList<FieldExpressionCalculation>().Where<FieldExpressionCalculation>((Func<FieldExpressionCalculation, bool>) (calc => calc.IsTransient)).OrderBy<FieldExpressionCalculation, string>((Func<FieldExpressionCalculation, string>) (calc => calc.Name)).Cast<CalculationSetElement>().ToList<CalculationSetElement>();
        return this.calculationSet.Elements.Where<CalculationSetElement>((Func<CalculationSetElement, bool>) (calc => calc.Enabled.ToString() == enabled && calc.ParentEntityType == parentEntityType)).Except<CalculationSetElement>((IEnumerable<CalculationSetElement>) list).OrderBy<CalculationSetElement, string>((Func<CalculationSetElement, string>) (calc => calc.Name)).ToList<CalculationSetElement>();
      }
      if (this.currentExpressionType != "All" && enabled == "All" && parentEntityType == "All" && transient == "All")
        return this.calculationSet.Elements.Where<CalculationSetElement>((Func<CalculationSetElement, bool>) (calc => calc.Identity.Type == enumElementType)).OrderBy<CalculationSetElement, string>((Func<CalculationSetElement, string>) (calc => calc.Name)).ToList<CalculationSetElement>();
      if (this.currentExpressionType != "All" && enabled != "All" && parentEntityType == "All" && transient == "All")
        return this.calculationSet.Elements.Where<CalculationSetElement>((Func<CalculationSetElement, bool>) (calc => calc.Identity.Type == enumElementType && calc.Enabled.ToString() == enabled)).OrderBy<CalculationSetElement, string>((Func<CalculationSetElement, string>) (calc => calc.Name)).ToList<CalculationSetElement>();
      if (this.currentExpressionType != "All" && enabled == "All" && parentEntityType != "All" && transient == "All")
        return this.calculationSet.Elements.Where<CalculationSetElement>((Func<CalculationSetElement, bool>) (calc => calc.Identity.Type == enumElementType && calc.ParentEntityType == parentEntityType)).OrderBy<CalculationSetElement, string>((Func<CalculationSetElement, string>) (calc => calc.Name)).ToList<CalculationSetElement>();
      if (this.currentExpressionType != "All" && enabled == "All" && parentEntityType == "All" && transient != "All")
      {
        if (transient == bool.TrueString)
          return this.calculationSet.GetAllFieldExpressionCalculations().ToList<FieldExpressionCalculation>().Where<FieldExpressionCalculation>((Func<FieldExpressionCalculation, bool>) (calc => calc.Identity.Type == enumElementType && calc.IsTransient.ToString() == transient)).OrderBy<FieldExpressionCalculation, string>((Func<FieldExpressionCalculation, string>) (calc => calc.Name)).Cast<CalculationSetElement>().ToList<CalculationSetElement>();
        List<CalculationSetElement> list = this.calculationSet.GetAllFieldExpressionCalculations().ToList<FieldExpressionCalculation>().Where<FieldExpressionCalculation>((Func<FieldExpressionCalculation, bool>) (calc => calc.IsTransient)).OrderBy<FieldExpressionCalculation, string>((Func<FieldExpressionCalculation, string>) (calc => calc.Name)).Cast<CalculationSetElement>().ToList<CalculationSetElement>();
        return this.calculationSet.Elements.Where<CalculationSetElement>((Func<CalculationSetElement, bool>) (calc => calc.Identity.Type == enumElementType)).Except<CalculationSetElement>((IEnumerable<CalculationSetElement>) list).OrderBy<CalculationSetElement, string>((Func<CalculationSetElement, string>) (calc => calc.Name)).ToList<CalculationSetElement>();
      }
      if (this.currentExpressionType != "All" && enabled == "All" && parentEntityType != "All" && transient != "All")
      {
        if (transient == bool.TrueString)
          return this.calculationSet.GetAllFieldExpressionCalculations().ToList<FieldExpressionCalculation>().Where<FieldExpressionCalculation>((Func<FieldExpressionCalculation, bool>) (calc => calc.Identity.Type == enumElementType && calc.ParentEntityType == parentEntityType && calc.IsTransient.ToString() == transient)).OrderBy<FieldExpressionCalculation, string>((Func<FieldExpressionCalculation, string>) (calc => calc.Name)).Cast<CalculationSetElement>().ToList<CalculationSetElement>();
        List<CalculationSetElement> list = this.calculationSet.GetAllFieldExpressionCalculations().ToList<FieldExpressionCalculation>().Where<FieldExpressionCalculation>((Func<FieldExpressionCalculation, bool>) (calc => calc.IsTransient)).OrderBy<FieldExpressionCalculation, string>((Func<FieldExpressionCalculation, string>) (calc => calc.Name)).Cast<CalculationSetElement>().ToList<CalculationSetElement>();
        return this.calculationSet.Elements.Where<CalculationSetElement>((Func<CalculationSetElement, bool>) (calc => calc.Identity.Type == enumElementType && calc.ParentEntityType == parentEntityType)).Except<CalculationSetElement>((IEnumerable<CalculationSetElement>) list).OrderBy<CalculationSetElement, string>((Func<CalculationSetElement, string>) (calc => calc.Name)).ToList<CalculationSetElement>();
      }
      if (this.currentExpressionType != "All" && enabled != "All" && parentEntityType == "All" && transient != "All")
      {
        if (transient == bool.TrueString)
          return this.calculationSet.GetAllFieldExpressionCalculations().ToList<FieldExpressionCalculation>().Where<FieldExpressionCalculation>((Func<FieldExpressionCalculation, bool>) (calc => calc.Identity.Type == enumElementType && calc.Enabled.ToString() == enabled && calc.IsTransient.ToString() == transient)).OrderBy<FieldExpressionCalculation, string>((Func<FieldExpressionCalculation, string>) (calc => calc.Name)).Cast<CalculationSetElement>().ToList<CalculationSetElement>();
        List<CalculationSetElement> list = this.calculationSet.GetAllFieldExpressionCalculations().ToList<FieldExpressionCalculation>().Where<FieldExpressionCalculation>((Func<FieldExpressionCalculation, bool>) (calc => calc.IsTransient)).OrderBy<FieldExpressionCalculation, string>((Func<FieldExpressionCalculation, string>) (calc => calc.Name)).Cast<CalculationSetElement>().ToList<CalculationSetElement>();
        return this.calculationSet.Elements.Where<CalculationSetElement>((Func<CalculationSetElement, bool>) (calc => calc.Identity.Type == enumElementType && calc.Enabled.ToString() == enabled)).Except<CalculationSetElement>((IEnumerable<CalculationSetElement>) list).OrderBy<CalculationSetElement, string>((Func<CalculationSetElement, string>) (calc => calc.Name)).ToList<CalculationSetElement>();
      }
      if (this.currentExpressionType != "All" && enabled != "All" && parentEntityType != "All" && transient == "All")
        return this.calculationSet.GetAllFieldExpressionCalculations().ToList<FieldExpressionCalculation>().Where<FieldExpressionCalculation>((Func<FieldExpressionCalculation, bool>) (calc => calc.Identity.Type == enumElementType && calc.Enabled.ToString() == enabled && calc.ParentEntityType == parentEntityType)).OrderBy<FieldExpressionCalculation, string>((Func<FieldExpressionCalculation, string>) (calc => calc.Name)).Cast<CalculationSetElement>().ToList<CalculationSetElement>();
      if (transient == bool.TrueString)
        return this.calculationSet.GetAllFieldExpressionCalculations().ToList<FieldExpressionCalculation>().Where<FieldExpressionCalculation>((Func<FieldExpressionCalculation, bool>) (calc => calc.Identity.Type == enumElementType && calc.Enabled.ToString() == enabled && calc.ParentEntityType == parentEntityType && calc.IsTransient.ToString() == transient)).OrderBy<FieldExpressionCalculation, string>((Func<FieldExpressionCalculation, string>) (calc => calc.Name)).Cast<CalculationSetElement>().ToList<CalculationSetElement>();
      List<CalculationSetElement> list1 = this.calculationSet.GetAllFieldExpressionCalculations().ToList<FieldExpressionCalculation>().Where<FieldExpressionCalculation>((Func<FieldExpressionCalculation, bool>) (calc => calc.IsTransient)).OrderBy<FieldExpressionCalculation, string>((Func<FieldExpressionCalculation, string>) (calc => calc.Name)).Cast<CalculationSetElement>().ToList<CalculationSetElement>();
      return this.calculationSet.Elements.Where<CalculationSetElement>((Func<CalculationSetElement, bool>) (calc => calc.Identity.Type == enumElementType && calc.Enabled.ToString() == enabled && calc.ParentEntityType == parentEntityType)).Except<CalculationSetElement>((IEnumerable<CalculationSetElement>) list1).OrderBy<CalculationSetElement, string>((Func<CalculationSetElement, string>) (calc => calc.Name)).ToList<CalculationSetElement>();
    }

    public void RemoveCalculationElement(Guid id, string elementName)
    {
      Tracing.Log(TraceLevel.Info, nameof (CalculationSetSerializer), string.Format("Remove Element Id: {0}, Element: {1}", (object) id.ToString(), (object) elementName));
      string calculationElementFilePath = this.GetCalculationElementFilePath(id, elementName);
      if (File.Exists(calculationElementFilePath))
        File.Delete(calculationElementFilePath);
      this.calculationSet.RemoveElement(id);
    }

    public void RemoveTest(string elementId, Guid testId)
    {
      Tracing.Log(TraceLevel.Info, nameof (CalculationSetSerializer), string.Format("Remove Test for {0}; TestId {1}", (object) elementId, (object) testId));
      CalculationSetElement calculationElement = this.GetCalculationElement(elementId, false);
      CalculationTest calculationTest = calculationElement.CalculationTests.Find((Predicate<CalculationTest>) (t => t.Id == testId));
      calculationElement.CalculationTests.Remove(calculationTest);
      this.WriteElement(calculationElement);
    }

    public void WriteElement(CalculationSetElement calculationElement)
    {
      Tracing.Log(TraceLevel.Info, nameof (CalculationSetSerializer), string.Format("Write Calculation Element for Id: {0}, Element: {1}", (object) calculationElement.Identity.Id.ToString(), (object) calculationElement.Name));
      XmlWriterSettings settings = new XmlWriterSettings()
      {
        Indent = true
      };
      XmlWriter writer = XmlWriter.Create(this.GetCalculationElementFilePath(calculationElement.Identity.Id, calculationElement.Name), settings);
      this.elementSerializer.WriteObject(writer, (object) calculationElement);
      writer.Close();
      this.RefreshCalculationElement(calculationElement.Identity.Id, calculationElement.Name);
    }

    private void LoadCalculationElements()
    {
      if (string.IsNullOrWhiteSpace(this.CurrentCalculationLibraryName))
        return;
      this.calculationSet.Elements.Clear();
      this.calculationSet.CalculationIndex.Clear();
      this.calculationSet.Identity.Id = Guid.NewGuid();
      this.calculationSet.Name = this.CurrentCalculationLibraryName;
      string str1 = Path.Combine(this.CurrentCalculationLibraryPath, "..", string.Format("{0}EntityTypes.xml", (object) this.CurrentCalculationLibraryName.Replace(" ", "")));
      XDocument xdocument = File.Exists(str1) ? XDocument.Load(str1) : throw new Exception(string.Format("Entity Type File \"{0}\" does not exist.", (object) str1));
      XElement xelement1 = xdocument.Root.Element((XName) "EntityTypes");
      List<XElement> list = xelement1.Elements((XName) "EntityType").Select<XElement, XElement>((Func<XElement, XElement>) (element => element)).ToList<XElement>();
      this.calculationSet.Version = xdocument.Root.Element((XName) "Version").Value;
      this.calculationSet.RootEntityType = FieldReplacementRegex.ParseEntityDescriptor(xelement1.Elements((XName) "RootEntityType").FirstOrDefault<XElement>().Value.ToString());
      this.calculationSet.RootRelationship = xelement1.Elements((XName) "RootRelationship").FirstOrDefault<XElement>().Value.ToString();
      this.calculationSet.ObjectModelRoot = xelement1.Element((XName) "ObjectModelRoot") == null ? string.Empty : xelement1.Element((XName) "ObjectModelRoot").Value.ToString();
      this.calculationSet.ObjectModelNamespace = xelement1.Element((XName) "ObjectModelNamespace") == null ? string.Empty : xelement1.Element((XName) "ObjectModelNamespace").Value.ToString();
      this.calculationSet.ObjectModelAssemblyName = xelement1.Element((XName) "ObjectModelAssemblyName") == null ? string.Empty : xelement1.Element((XName) "ObjectModelAssemblyName").Value.ToString();
      this.calculationSet.EntityTypeDictionary = new ConcurrentDictionary<EntityDescriptor, Dictionary<string, string>>();
      this.calculationSet.EntityTypeDictionary.TryAdd(this.calculationSet.RootEntityType.ToInterned(), new Dictionary<string, string>());
      foreach (XElement xelement2 in list)
      {
        Dictionary<string, string> dictionary = new Dictionary<string, string>();
        foreach (XAttribute attribute in xelement2.Attributes())
          dictionary.Add(attribute.Name.ToString(), attribute.Value);
        this.calculationSet.EntityTypeDictionary.TryAdd(FieldReplacementRegex.ParseEntityDescriptor(xelement2.Value).ToInterned(), dictionary);
      }
      if (string.IsNullOrEmpty(this.CurrentCalculationLibraryPath))
        return;
      string[] files = Directory.GetFiles(this.CurrentCalculationLibraryPath, "*.xml");
      if (files.Length == 0)
        return;
      foreach (string str2 in files)
      {
        if (File.Exists(str2))
        {
          Path.GetFullPath(str2);
          this.ReadCalculationElement(str2);
        }
      }
    }

    private void ReadCalculationElement(string xmlFile)
    {
      try
      {
        XmlReader reader = XmlReader.Create(Utility.ValidateAssemblyPath(xmlFile));
        CalculationSetElement element = (CalculationSetElement) this.elementSerializer.ReadObject(reader);
        element.Identity.ParentId = this.calculationSet.Identity.Id;
        reader.Close();
        EntityDescriptor entityDescriptor = FieldReplacementRegex.ParseEntityDescriptor(element.ParentEntityType);
        element.IsValidEntityType = this.calculationSet.ValidateEntityType(entityDescriptor);
        this.calculationSet.AddElement(element);
      }
      catch (Exception ex)
      {
        Tracing.Log(TraceLevel.Error, nameof (CalculationSetSerializer), string.Format("ReadCalculationElement failed for {0}", (object) xmlFile));
        throw ex;
      }
    }

    private void RefreshCalculationElement(Guid id, string elementName)
    {
      string calculationElementFilePath = this.GetCalculationElementFilePath(id, elementName);
      if (this.calculationSet.Elements != null)
      {
        this.calculationSet.RemoveElement(id);
        if (!File.Exists(calculationElementFilePath))
          return;
        this.ReadCalculationElement(calculationElementFilePath);
      }
      else
        this.LoadCalculationElements();
    }
  }
}
