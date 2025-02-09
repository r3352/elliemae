// Decompiled with JetBrains decompiler
// Type: Elli.CalculationEngine.Core.CalculationLibrary.CalculationSet
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
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

#nullable disable
namespace Elli.CalculationEngine.Core.CalculationLibrary
{
  public class CalculationSet : LibraryElement, IDisposable
  {
    private const string className = "CalculationSet";
    private static readonly object _calcSetLock = new object();
    private HashSet<string> addedReferenceList = new HashSet<string>();
    private HashSet<FieldDescriptor> addedFieldsList = new HashSet<FieldDescriptor>();
    private ConcurrentDictionary<Guid, ICalculationImpl> calcImpl = new ConcurrentDictionary<Guid, ICalculationImpl>();

    public string Name { get; set; }

    public EntityDescriptor RootEntityType { get; set; }

    public string RootRelationship { get; set; }

    public string ObjectModelRoot { get; set; }

    public string ObjectModelNamespace { get; set; }

    public string ObjectModelAssemblyName { get; set; }

    public ConcurrentDictionary<EntityDescriptor, Dictionary<string, string>> EntityTypeDictionary { get; set; }

    public List<EntityDescriptor> EntityTypes
    {
      get => this.EntityTypeDictionary.Keys.ToList<EntityDescriptor>();
      private set
      {
      }
    }

    public List<CalculationSetElement> Elements { get; set; }

    public Dictionary<string, FieldExpressionCalculation> CalculationIndex { get; set; }

    public string Version { get; set; }

    public string CalculationEngineAssemblyName
    {
      get
      {
        return new Regex("[^a-zA-Z0-9]").Replace(string.Format("{0}CalculationEngine", (object) this.Name), "");
      }
    }

    public string CalculationEngineDllName
    {
      get => string.Format("{0}.dll", (object) this.CalculationEngineAssemblyName);
    }

    public string CalculationSetAssemblyName
    {
      get
      {
        return new Regex("[^a-zA-Z0-9]").Replace(string.Format("{0}CalculationSet", (object) this.Name), "");
      }
    }

    public string CalculationSetDllName
    {
      get => string.Format("{0}.dll", (object) this.CalculationSetAssemblyName);
    }

    public CalculationSet()
    {
      this.Identity = new ElementIdentity();
      this.Identity.Type = LibraryElementType.CalculationSet;
      this.Elements = new List<CalculationSetElement>();
      this.CalculationIndex = new Dictionary<string, FieldExpressionCalculation>();
      this.EntityTypeDictionary = new ConcurrentDictionary<EntityDescriptor, Dictionary<string, string>>();
    }

    public CalculationSet(Guid id)
    {
      this.Identity = new ElementIdentity();
      this.Identity.Id = id;
      this.Identity.Type = LibraryElementType.CalculationSet;
      this.Elements = new List<CalculationSetElement>();
      this.CalculationIndex = new Dictionary<string, FieldExpressionCalculation>();
    }

    public void AddElement(CalculationSetElement element)
    {
      if (this.Elements.Find((Predicate<CalculationSetElement>) (elem => elem.Name == element.Name && elem.ParentEntityType == element.ParentEntityType)) == null)
      {
        if (element.Identity.Type == LibraryElementType.FieldExpressionCalculation)
          this.CalculationIndex.Add(element.ParentEntityType + element.Name, (FieldExpressionCalculation) element);
        this.Elements.Add(element);
      }
      else
      {
        string message = string.Format("{0}->{1} already exists in the CalculationSet.", (object) element.ParentEntityType, (object) element.Name);
        Tracing.Log(TraceLevel.Error, nameof (CalculationSet), string.Format("AddElement - {0}", (object) message));
        throw new Exception(message);
      }
    }

    public void AppendFunctions(
      string expressionText,
      List<Function> functionsAvailable,
      bool enableWarning = false)
    {
      foreach (Function function1 in this.GetFunctions(expressionText, functionsAvailable))
      {
        Function function = function1;
        if (this.Elements.Find((Predicate<CalculationSetElement>) (calc => calc.Identity.Id == function.Identity.Id)) == null)
        {
          this.AppendFunctions(function.Expression.Text, functionsAvailable, enableWarning);
          this.AddElement((CalculationSetElement) function);
        }
      }
    }

    public void AppendTransientDataObjects(
      string expressionText,
      List<TransientDataObject> transientDataObjectsAvailable,
      bool enableWarning = false)
    {
      foreach (TransientDataObject transientDataObject1 in this.GetTransientDataObjects(expressionText, transientDataObjectsAvailable))
      {
        TransientDataObject transientDataObject = transientDataObject1;
        if (this.Elements.Find((Predicate<CalculationSetElement>) (calc => calc.Identity.Id == transientDataObject.Identity.Id)) == null)
        {
          this.AppendTransientDataObjects(transientDataObject.Expression.Text, transientDataObjectsAvailable, enableWarning);
          this.AddElement((CalculationSetElement) transientDataObject);
        }
      }
    }

    public ValidationStatus DuplicateCalculationCheck(
      Guid guid,
      string fieldId,
      string descriptiveName,
      string expressionType,
      string parentEntityType)
    {
      string str1 = expressionType == ExpressionType.Function.ToString() || expressionType == ExpressionType.TransientDataObject.ToString() ? descriptiveName : fieldId;
      Tracing.Log(TraceLevel.Info, nameof (CalculationSet), string.Format("Check for Duplicate Calculations for {0}", (object) str1));
      bool flag = false;
      ValidationStatus validationStatus = new ValidationStatus();
      foreach (CalculationSetElement element in this.Elements)
      {
        string str2 = expressionType == ExpressionType.Function.ToString() || expressionType == ExpressionType.TransientDataObject.ToString() ? (string.IsNullOrEmpty(element.DescriptiveName) ? string.Empty : element.DescriptiveName) : element.Name;
        if (str1.ToUpper().Trim() == str2.ToUpper().Trim() && string.Compare(parentEntityType.Trim(), element.ParentEntityType.Trim(), true) == 0 && guid != element.Identity.Id)
          flag = true;
      }
      if (flag)
      {
        validationStatus.Success = false;
        validationStatus.Message = string.Format("A Calculation Definition for {0}->{1} already exists.", (object) parentEntityType, (object) str1);
      }
      return validationStatus;
    }

    public string ExpandCalculation(string expressionText, bool mustBeEnabled = false)
    {
      return this.ReplaceTemplates(expressionText, mustBeEnabled);
    }

    public FieldExpressionCalculation GetCalculation(string fieldId)
    {
      return this.Elements.OfType<FieldExpressionCalculation>().FirstOrDefault<FieldExpressionCalculation>((Func<FieldExpressionCalculation, bool>) (f => f.Name == fieldId));
    }

    public FieldExpressionCalculation GetCalculation(string fieldId, string parentEntityType)
    {
      if (this.CalculationIndex.Count < 1 && this.Elements.Count > 0)
      {
        lock (CalculationSet._calcSetLock)
        {
          if (this.CalculationIndex.Count < 1)
            this.Elements.Where<CalculationSetElement>((Func<CalculationSetElement, bool>) (element => element.Identity.Type == LibraryElementType.FieldExpressionCalculation)).ToList<CalculationSetElement>().ForEach((Action<CalculationSetElement>) (p => this.CalculationIndex.Add(p.ParentEntityType + p.Name, (FieldExpressionCalculation) p)));
        }
      }
      FieldExpressionCalculation calculation = (FieldExpressionCalculation) null;
      this.CalculationIndex.TryGetValue(parentEntityType + fieldId, out calculation);
      return calculation;
    }

    public List<Function> GetAvailableFunctions()
    {
      return this.Elements.OfType<Function>().ToList<Function>();
    }

    public List<TransientDataObject> GetAvailableTransientDataObjects()
    {
      return this.Elements.OfType<TransientDataObject>().ToList<TransientDataObject>();
    }

    public IEnumerable<FieldExpressionCalculation> GetAllFieldExpressionCalculations()
    {
      return this.Elements.OfType<FieldExpressionCalculation>();
    }

    public LibraryElement GetElement(IElementIdentity id) => (LibraryElement) this;

    public CalculationSetElement GetCalculationSetElementFromId(string id)
    {
      CalculationSetElement setElementFromId = (CalculationSetElement) null;
      Guid guid;
      if (Guid.TryParse(id, out guid))
        setElementFromId = this.Elements.SingleOrDefault<CalculationSetElement>((Func<CalculationSetElement, bool>) (element => element.Identity.Id == guid));
      return setElementFromId;
    }

    public string GetElementDescriptor(string fieldId)
    {
      return this.Elements.Find((Predicate<CalculationSetElement>) (elem => elem.Name == fieldId)).ToString();
    }

    public string[] ListElements()
    {
      List<string> stringList = new List<string>();
      foreach (CalculationSetElement element in this.Elements)
        stringList.Add(element.ToString());
      return stringList.ToArray();
    }

    public string GetEnabledCalcInfo()
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (FieldExpressionCalculation expressionCalculation in this.Elements.OfType<FieldExpressionCalculation>().Where<FieldExpressionCalculation>((Func<FieldExpressionCalculation, bool>) (element => element.Enabled)))
      {
        if (!expressionCalculation.IsTransient)
          stringBuilder.AppendLine(string.Format("{0}\t{{{1}}}.[{2}]", (object) expressionCalculation.DescriptiveName, (object) expressionCalculation.ParentEntityType, (object) expressionCalculation.Name));
      }
      return stringBuilder.ToString();
    }

    public CalculationSet GetEnabled()
    {
      CalculationSet calculationSet1 = new CalculationSet();
      calculationSet1.Name = this.Name;
      calculationSet1.Version = this.Version;
      calculationSet1.RootEntityType = this.RootEntityType;
      calculationSet1.RootRelationship = this.RootRelationship;
      calculationSet1.ObjectModelRoot = this.ObjectModelRoot;
      calculationSet1.ObjectModelNamespace = this.ObjectModelNamespace;
      calculationSet1.ObjectModelAssemblyName = this.ObjectModelAssemblyName;
      calculationSet1.Identity = new ElementIdentity();
      calculationSet1.Identity.Id = this.Identity.Id;
      calculationSet1.Identity.Type = LibraryElementType.CalculationSet;
      calculationSet1.Elements = this.Elements.Where<CalculationSetElement>((Func<CalculationSetElement, bool>) (calcSet => calcSet.Enabled)).ToList<CalculationSetElement>();
      calculationSet1.EntityTypeDictionary = this.EntityTypeDictionary;
      CalculationSet calculationSet2 = calculationSet1.ExpandCalculations(calculationSet1);
      CalculationSet.AddParameterizedReferences(ref calculationSet2);
      return calculationSet2;
    }

    public void RemoveElement(Guid id)
    {
      if (this.Elements == null)
        return;
      CalculationSetElement calculationSetElement = this.Elements.Find((Predicate<CalculationSetElement>) (calc => calc.Identity.Id == id));
      this.Elements.Remove(calculationSetElement);
      if (calculationSetElement == null || calculationSetElement.Identity.Type != LibraryElementType.FieldExpressionCalculation)
        return;
      this.CalculationIndex.Remove(calculationSetElement.ParentEntityType + calculationSetElement.Name);
    }

    public string ReplaceTemplates(string expressionText, bool mustBeEnabled = false)
    {
      foreach (string template in TemplateReplacementRegex.ParseTemplates(expressionText))
      {
        int length = template.IndexOf("(");
        string name = string.Empty;
        if (length > -1)
          name = template.Substring(0, length);
        CalculationSetElement calculationSetElement = this.Elements.Find((Predicate<CalculationSetElement>) (element => element.Name == name));
        if (calculationSetElement != null && (!mustBeEnabled || mustBeEnabled && calculationSetElement.Enabled))
        {
          string sourceCode = this.ReplaceTemplates(((CalculationTemplate) calculationSetElement).Expression.Text, mustBeEnabled);
          string newValue = TemplateReplacementRegex.Replace(template, sourceCode);
          expressionText = expressionText.Replace(template, newValue);
        }
      }
      return expressionText;
    }

    public void ResetVerifiedFlags()
    {
      foreach (CalculationSetElement element in this.Elements)
      {
        element.IsTemplateVerified = false;
        element.IsTransientVerified = false;
      }
    }

    public bool ValidateEntityType(EntityDescriptor entityType)
    {
      if (entityType.EntityType.Contains("%"))
        return true;
      if (this.EntityTypes == null)
        return !(entityType != (EntityDescriptor) null);
      foreach (EntityDescriptor entityType1 in this.EntityTypes)
      {
        if (entityType.EntityType == entityType1.EntityType)
          return true;
      }
      return false;
    }

    public ValidationStatus VerifyReferencedCalculationElements(
      string expressionText,
      ValidationStatus status,
      bool enableWarning)
    {
      status = this.VerifyEntityTypes(expressionText, status);
      status = this.VerifyTemplates(expressionText, status, enableWarning);
      status = this.VerifyTransientsEnabled(expressionText, status, enableWarning);
      status = this.VerifyFunctionsEnabled(expressionText, status, enableWarning);
      status = this.VerifyTransientDataObjectsEnabled(expressionText, status, enableWarning);
      return status;
    }

    public static void AddParameterizedReferences(ref CalculationSet calculationSet)
    {
      Dictionary<string, Dictionary<string, List<EntityDescriptor>>> parameterizedElementsDictionary = CalculationSet.ParameterizedElements(calculationSet);
      List<CalculationSetElement> calculationSetElementList = new List<CalculationSetElement>();
      foreach (FieldExpressionCalculation expressionCalculation in calculationSet.Elements.OfType<FieldExpressionCalculation>())
        CalculationSet.AddReferencedCalculations(expressionCalculation.Expression.ReferenceRoot, parameterizedElementsDictionary, calculationSet, calculationSetElementList);
      calculationSet.Elements.AddRange((IEnumerable<CalculationSetElement>) calculationSetElementList);
      foreach (FieldExpressionCalculation expressionCalculation in calculationSet.Elements.OfType<FieldExpressionCalculation>())
      {
        try
        {
          CalculationSet.CheckForParameterizedElements(expressionCalculation.Expression.ReferenceRoot, expressionCalculation.Expression.ReferenceRoot, parameterizedElementsDictionary, calculationSet);
        }
        catch (Exception ex)
        {
          throw new Exception(string.Format("Calculation for {{{0}}}.[{1}] contains {2}; Weak references must be parameterized if their is a base definition for the field calculation.", (object) expressionCalculation.ParentEntityType, (object) expressionCalculation.Name, (object) ex.Message), ex);
        }
      }
    }

    public CalculationSet ExpandCalculations(CalculationSet calculationSet)
    {
      foreach (LibraryElement element in calculationSet.Elements)
      {
        if (element.Identity.Type == LibraryElementType.FieldExpressionCalculation)
          ((FieldExpressionCalculation) element).Expression.Text = this.ExpandCalculation(((FieldExpressionCalculation) element).Expression.Text, true);
        else if (element.Identity.Type == LibraryElementType.CalculationTemplate)
          ((CalculationTemplate) element).Expression.Text = this.ExpandCalculation(((CalculationTemplate) element).Expression.Text, true);
        else if (element.Identity.Type == LibraryElementType.Function)
          ((Function) element).Expression.Text = this.ExpandCalculation(((Function) element).Expression.Text, true);
        else if (element.Identity.Type == LibraryElementType.TransientDataObject)
          ((TransientDataObject) element).Expression.Text = this.ExpandCalculation(((TransientDataObject) element).Expression.Text, true);
      }
      return calculationSet;
    }

    public void ExpandCalculations()
    {
      foreach (LibraryElement element in this.Elements)
      {
        if (element.Identity.Type == LibraryElementType.FieldExpressionCalculation)
          ((FieldExpressionCalculation) element).Expression.Text = this.ExpandCalculation(((FieldExpressionCalculation) element).Expression.Text, true);
        else if (element.Identity.Type == LibraryElementType.CalculationTemplate)
          ((CalculationTemplate) element).Expression.Text = this.ExpandCalculation(((CalculationTemplate) element).Expression.Text, true);
        else if (element.Identity.Type == LibraryElementType.Function)
          ((Function) element).Expression.Text = this.ExpandCalculation(((Function) element).Expression.Text, true);
        else if (element.Identity.Type == LibraryElementType.TransientDataObject)
          ((TransientDataObject) element).Expression.Text = this.ExpandCalculation(((TransientDataObject) element).Expression.Text, true);
      }
    }

    public void AddReadOnlyCalcs()
    {
      Stopwatch stopwatch = new Stopwatch();
      stopwatch.Start();
      this.addedReferenceList.Clear();
      this.addedFieldsList.Clear();
      foreach (FieldExpressionCalculation calc in new List<FieldExpressionCalculation>(this.GetAllFieldExpressionCalculations()))
        this.AddReferencedFields(calc);
      stopwatch.Stop();
      Tracing.Log(TraceLevel.Info, nameof (CalculationSet), string.Format("AddReadOnlyCalcs duration: {0}", (object) stopwatch.Elapsed));
    }

    public ICalculationImpl GetCalcImpl(ElementIdentity Identity)
    {
      ICalculationImpl calcImpl;
      if (this.calcImpl.TryGetValue(Identity.Id, out calcImpl))
        return calcImpl;
      ICalculationImpl instance = (ICalculationImpl) RuntimeContext.Current.CreateInstance((IElementIdentity) Identity, typeof (ICalculationImpl));
      return this.calcImpl.GetOrAdd(Identity.Id, instance);
    }

    public void Dispose() => this.Dispose(true);

    protected void Dispose(bool disposing)
    {
      if (this.EntityTypeDictionary != null)
        this.EntityTypeDictionary.Clear();
      if (this.Elements != null)
        this.Elements.Clear();
      if (this.CalculationIndex == null)
        return;
      this.CalculationIndex.Clear();
    }

    private static void AddReferencedCalculations(
      ReferencedElement reference,
      Dictionary<string, Dictionary<string, List<EntityDescriptor>>> parameterizedElementsDictionary,
      CalculationSet set,
      List<CalculationSetElement> referencedCalculations)
    {
      for (int index = reference.ReferencedElements.Count<ReferencedElement>() - 1; index >= 0; --index)
      {
        ReferencedElement referencedElement = reference.ReferencedElements.ElementAt<ReferencedElement>(index);
        if (referencedElement.DataElementType == DataElementType.DataFieldType)
        {
          if (CalculationSet.IsFieldParameterized(referencedElement.Name, referencedElement.ParentElement.EntityType, parameterizedElementsDictionary))
          {
            EntityDescriptor entityType = referencedElement.ParentElement == null ? (EntityDescriptor) null : referencedElement.ParentElement.EntityType;
            string refParentTypeString = entityType == (EntityDescriptor) null ? string.Empty : entityType.ToString();
            if (!referencedCalculations.Where<CalculationSetElement>((Func<CalculationSetElement, bool>) (p => p.Name == referencedElement.Name && p.ParentEntityType == refParentTypeString)).Any<CalculationSetElement>() && !set.Elements.Where<CalculationSetElement>((Func<CalculationSetElement, bool>) (p => p.Name == referencedElement.Name && p.ParentEntityType == refParentTypeString)).Any<CalculationSetElement>())
              referencedCalculations.Add((CalculationSetElement) new FieldExpressionCalculation(Guid.NewGuid(), referencedElement.GetFieldName(), "Read Only Calculation", "", false, string.Format("[{0}]", (object) referencedElement.GetFieldName()), Guid.Empty, true, refParentTypeString)
              {
                IsReadOnly = true
              });
            foreach (EntityDescriptor entityDescriptor in CalculationSet.GetParameterizedElementsForField(referencedElement.Name, referencedElement.ParentElement.EntityType, parameterizedElementsDictionary))
            {
              if (!referencedCalculations.Where<CalculationSetElement>((Func<CalculationSetElement, bool>) (p => p.Name == referencedElement.Name && p.ParentEntityType == refParentTypeString)).Any<CalculationSetElement>() && !set.Elements.Where<CalculationSetElement>((Func<CalculationSetElement, bool>) (p => p.Name == referencedElement.Name && p.ParentEntityType == refParentTypeString)).Any<CalculationSetElement>())
                referencedCalculations.Add((CalculationSetElement) new FieldExpressionCalculation(Guid.NewGuid(), referencedElement.GetFieldName(), "Read Only Calculation", "", false, string.Format("[{0}]", (object) referencedElement.GetFieldName()), Guid.Empty, true, entityDescriptor.ToString())
                {
                  IsReadOnly = true
                });
            }
          }
        }
        else
          CalculationSet.AddReferencedCalculations(referencedElement, parameterizedElementsDictionary, set, referencedCalculations);
      }
    }

    private static void CheckForParameterizedElements(
      ReferencedElement root,
      ReferencedElement reference,
      Dictionary<string, Dictionary<string, List<EntityDescriptor>>> parameterizedElementsDictionary,
      CalculationSet set)
    {
      for (int index = reference.ReferencedElements.Count<ReferencedElement>() - 1; index >= 0; --index)
      {
        ReferencedElement referencedElement = reference.ReferencedElements.ElementAt<ReferencedElement>(index);
        if (referencedElement.DataElementType == DataElementType.DataFieldType)
        {
          if (referencedElement.ParentElement.EntityType.IsBaseType() && CalculationSet.IsFieldParameterized(referencedElement.Name, referencedElement.ParentElement.EntityType, parameterizedElementsDictionary))
          {
            if (referencedElement.IsWeak)
              throw new Exception(string.Format("Invalid Weak Reference to Base Parent Element {{{0}}}.[{1}].", (object) referencedElement.ParentElement.EntityType, (object) referencedElement.Name, (object) reference.Name));
            FieldExpressionCalculation calculation = set.GetCalculation(referencedElement.Name, referencedElement.ParentElement.EntityType.ToString());
            set.Elements.Where<CalculationSetElement>((Func<CalculationSetElement, bool>) (p => p.Name == referencedElement.Name));
            foreach (EntityDescriptor type in CalculationSet.GetParameterizedElementsForField(referencedElement.Name, referencedElement.ParentElement.EntityType, parameterizedElementsDictionary))
            {
              ReferencedElement referencedElement1 = new ReferencedElement(CalculationUtility.ALL_ENTITIES, type, referencedElement.ParentElement.DataElementType, (ReferencedElement) null, false);
              ReferencedElement element1 = new ReferencedElement(referencedElement.Name, referencedElement.EntityType, referencedElement.DataElementType, referencedElement1, false);
              calculation.Expression.ReferenceRoot.AddReferencedElement(referencedElement1);
              referencedElement1.AddReferencedElement(element1);
              ReferencedElement referencedElement2 = new ReferencedElement(CalculationUtility.ALL_ENTITIES, type, referencedElement.ParentElement.DataElementType, (ReferencedElement) null, false);
              ReferencedElement element2 = new ReferencedElement(referencedElement.Name, referencedElement.EntityType, referencedElement.DataElementType, referencedElement2, false);
              root.AddReferencedElement(referencedElement2);
              referencedElement2.AddReferencedElement(element2);
            }
          }
        }
        else
          CalculationSet.CheckForParameterizedElements(root, referencedElement, parameterizedElementsDictionary, set);
      }
    }

    private bool IsParameterizedElementDefined(
      string name,
      EntityDescriptor descriptor,
      Dictionary<string, Dictionary<string, List<EntityDescriptor>>> parameterizedElementsDictionary)
    {
      bool flag = false;
      if (parameterizedElementsDictionary.ContainsKey(descriptor.EntityType) && parameterizedElementsDictionary[descriptor.EntityType].ContainsKey(name))
      {
        foreach (EntityDescriptor entityDescriptor in parameterizedElementsDictionary[descriptor.EntityType][name])
        {
          if (descriptor.ToString() == entityDescriptor.ToString())
          {
            flag = true;
            break;
          }
        }
      }
      return flag;
    }

    private static bool IsFieldParameterized(
      string name,
      EntityDescriptor descriptor,
      Dictionary<string, Dictionary<string, List<EntityDescriptor>>> parameterizedElementsDictionary)
    {
      bool flag = false;
      if (parameterizedElementsDictionary.ContainsKey(descriptor.EntityType) && parameterizedElementsDictionary[descriptor.EntityType].ContainsKey(name))
        flag = true;
      return flag;
    }

    private static List<EntityDescriptor> GetParameterizedElementsForField(
      string name,
      EntityDescriptor descriptor,
      Dictionary<string, Dictionary<string, List<EntityDescriptor>>> parameterizedElementsDictionary)
    {
      List<EntityDescriptor> elementsForField = new List<EntityDescriptor>();
      if (parameterizedElementsDictionary.ContainsKey(descriptor.EntityType) && parameterizedElementsDictionary[descriptor.EntityType].ContainsKey(name))
        elementsForField = parameterizedElementsDictionary[descriptor.EntityType][name];
      return elementsForField;
    }

    private static Dictionary<string, Dictionary<string, List<EntityDescriptor>>> ParameterizedElements(
      CalculationSet calculationSet)
    {
      Dictionary<string, Dictionary<string, List<EntityDescriptor>>> dictionary = new Dictionary<string, Dictionary<string, List<EntityDescriptor>>>();
      foreach (CalculationSetElement calculationSetElement in calculationSet.Elements.Where<CalculationSetElement>((Func<CalculationSetElement, bool>) (element => element.Identity.Type == LibraryElementType.FieldExpressionCalculation)))
      {
        EntityDescriptor entityDescriptor = FieldReplacementRegex.ParseEntityDescriptor(calculationSetElement.ParentEntityType);
        if (!entityDescriptor.IsBaseType())
        {
          if (!dictionary.ContainsKey(entityDescriptor.EntityType))
            dictionary.Add(entityDescriptor.EntityType, new Dictionary<string, List<EntityDescriptor>>());
          if (!dictionary[entityDescriptor.EntityType].ContainsKey(calculationSetElement.Name))
            dictionary[entityDescriptor.EntityType].Add(calculationSetElement.Name, new List<EntityDescriptor>());
          dictionary[entityDescriptor.EntityType][calculationSetElement.Name].Add(entityDescriptor);
        }
      }
      return dictionary;
    }

    private List<Function> GetFunctions(string expressionText, List<Function> functionsAvailable = null)
    {
      string str = FunctionReplacementRegex.FunctionName(expressionText);
      List<Function> functions = new List<Function>();
      char[] separator = new char[2]{ ' ', '(' };
      string[] source = expressionText.Split(separator, StringSplitOptions.RemoveEmptyEntries);
      foreach (Function function1 in functionsAvailable)
      {
        Function function = function1;
        if (((IEnumerable<string>) source).Contains<string>(function.DescriptiveName) && str != function.DescriptiveName && functions.Find((Predicate<Function>) (element => element.DescriptiveName == function.DescriptiveName)) == null)
          functions.Add(function);
      }
      return functions;
    }

    private List<TransientDataObject> GetTransientDataObjects(
      string expressionText,
      List<TransientDataObject> transientDataObjectsAvailable = null)
    {
      string str = TransientDataObjectReplacementRegex.TransientDataObjectName(expressionText);
      List<TransientDataObject> transientDataObjects = new List<TransientDataObject>();
      char[] separator = new char[4]{ ' ', '(', ')', ',' };
      string[] source = expressionText.Split(separator, StringSplitOptions.RemoveEmptyEntries);
      foreach (TransientDataObject transientDataObject1 in transientDataObjectsAvailable)
      {
        TransientDataObject transientDataObject = transientDataObject1;
        if (((IEnumerable<string>) source).Contains<string>(transientDataObject.DescriptiveName) && str != transientDataObject.DescriptiveName && transientDataObjects.Find((Predicate<TransientDataObject>) (element => element.DescriptiveName == transientDataObject.DescriptiveName)) == null)
          transientDataObjects.Add(transientDataObject);
      }
      return transientDataObjects;
    }

    private ValidationStatus VerifyEntityTypes(string expressionText, ValidationStatus status)
    {
      string empty = string.Empty;
      foreach (ReferencedElement referencedElement in FieldReplacementRegex.ParseReferencedElements(expressionText, string.Empty).ReferencedElements)
      {
        if (referencedElement.DataElementType != DataElementType.DataFieldType && !this.ValidateEntityType(referencedElement.EntityType))
        {
          status.Success = false;
          status.Message += string.Format("ERROR: {0} is not a valid Entity Type.\r\n", referencedElement.EntityType == (EntityDescriptor) null ? (object) string.Empty : (object) referencedElement.EntityType.ToString());
        }
        status = this.VerifyEntityTypes(referencedElement.ReferencedElements, status);
      }
      if (!string.IsNullOrEmpty(status.Message))
        Tracing.Log(TraceLevel.Warning, nameof (CalculationSet), string.Format("VerifyEntityTypes: {0}", (object) status.Message));
      return status;
    }

    private ValidationStatus VerifyEntityTypes(
      List<ReferencedElement> referencedElements,
      ValidationStatus status)
    {
      if (referencedElements == null)
        return status;
      foreach (ReferencedElement referencedElement in referencedElements)
      {
        if (referencedElement.DataElementType != DataElementType.DataFieldType && !this.ValidateEntityType(referencedElement.EntityType))
        {
          status.Success = false;
          status.Message += string.Format("ERROR: {0} is not a valid Entity Type.\r\n", referencedElement.EntityType == (EntityDescriptor) null ? (object) string.Empty : (object) referencedElement.EntityType.ToString());
        }
        status = this.VerifyEntityTypes(referencedElement.ReferencedElements, status);
      }
      return status;
    }

    private ValidationStatus VerifyFunctionsEnabled(
      string expressionText,
      ValidationStatus status,
      bool enableWarning)
    {
      foreach (Function function in this.GetFunctions(expressionText, this.Elements.OfType<Function>().ToList<Function>()))
      {
        if (enableWarning && !function.Enabled)
          status.Message += string.Format("WARNING: {0} is not enabled.\r\n", (object) function.DescriptiveName);
      }
      if (!string.IsNullOrEmpty(status.Message))
        Tracing.Log(TraceLevel.Warning, nameof (CalculationSet), string.Format("VerifyFunctionsEnabled: {0}", (object) status.Message));
      return status;
    }

    private ValidationStatus VerifyTransientDataObjectsEnabled(
      string expressionText,
      ValidationStatus status,
      bool enableWarning)
    {
      foreach (TransientDataObject transientDataObject in this.GetTransientDataObjects(expressionText, this.Elements.OfType<TransientDataObject>().ToList<TransientDataObject>()))
      {
        if (enableWarning && !transientDataObject.Enabled)
          status.Message += string.Format("WARNING: {0} is not enabled.\r\n", (object) transientDataObject.DescriptiveName);
      }
      if (!string.IsNullOrEmpty(status.Message))
        Tracing.Log(TraceLevel.Warning, nameof (CalculationSet), string.Format("VerifyTransientDataObjectsEnabled: {0}", (object) status.Message));
      return status;
    }

    private ValidationStatus VerifyTemplates(
      string expressionText,
      ValidationStatus status,
      bool enableWarning)
    {
      string empty = string.Empty;
      foreach (string template in TemplateReplacementRegex.ParseTemplates(expressionText))
      {
        string name = TemplateReplacementRegex.GetTemplateName(template);
        CalculationSetElement calculationSetElement = this.Elements.Find((Predicate<CalculationSetElement>) (element => element.Name == name));
        if (enableWarning && (calculationSetElement == null || !calculationSetElement.Enabled))
          status.Message += string.Format("WARNING: {0} is not enabled.\r\n", (object) name);
        if (calculationSetElement != null && !calculationSetElement.IsTemplateVerified)
        {
          calculationSetElement.IsTemplateVerified = true;
          string text = ((CalculationTemplate) calculationSetElement).Expression.Text;
          status = this.VerifyReferencedCalculationElements(text, status, enableWarning);
          string str = TemplateReplacementRegex.Verify(name, template, text);
          if (!string.IsNullOrEmpty(str))
          {
            status.Message += str;
            status.Success = false;
          }
        }
      }
      if (!string.IsNullOrEmpty(status.Message))
        Tracing.Log(TraceLevel.Warning, nameof (CalculationSet), string.Format("VerifyTemplates: {0}", (object) status.Message));
      return status;
    }

    private ValidationStatus VerifyTransientsEnabled(
      string expressionText,
      ValidationStatus status,
      bool enableWarning)
    {
      string empty = string.Empty;
      foreach (string transientField in FieldReplacementRegex.ParseTransientFields(expressionText))
      {
        string transient = transientField;
        CalculationSetElement calculationSetElement = this.Elements.Find((Predicate<CalculationSetElement>) (element => element.Name == transient));
        if (enableWarning && (calculationSetElement == null || !calculationSetElement.Enabled))
        {
          status.Success = false;
          status.Message += string.Format("WARNING: {0} does not exist or is not enabled.\r\n", (object) transient);
        }
        if (calculationSetElement != null && !calculationSetElement.IsTransientVerified)
        {
          calculationSetElement.IsTransientVerified = true;
          status = this.VerifyReferencedCalculationElements(((FieldExpressionCalculation) calculationSetElement).Expression.Text, status, enableWarning);
        }
      }
      if (!string.IsNullOrEmpty(status.Message))
        Tracing.Log(TraceLevel.Warning, nameof (CalculationSet), string.Format("VerifyTransientsEnabled: {0}", (object) status.Message));
      return status;
    }

    private void AddReferencedElements(IEnumerable<ReferencedElement> elements)
    {
      foreach (ReferencedElement element in elements)
      {
        if (element.DataElementType == DataElementType.DataFieldType)
          this.AddReferencedField(element);
        else
          this.AddReferencedElements((IEnumerable<ReferencedElement>) element.ReferencedElements);
      }
    }

    private void AddReferencedField(ReferencedElement element)
    {
      string typeFormattedName = element.TypeFormattedName;
      EntityDescriptor entityDescriptor = element.ParentElement != null ? element.ParentElement.EntityType : this.RootEntityType;
      if (element.IsWeak || this.addedReferenceList.Contains(typeFormattedName))
        return;
      this.addedReferenceList.Add(typeFormattedName);
      FieldExpressionCalculation calculation = this.GetCalculation(element.Name, entityDescriptor.ToString());
      if (calculation == null)
        this.AddElement((CalculationSetElement) new FieldExpressionCalculation(Guid.NewGuid(), element.Name, "Referenced Field - No Calculation", string.Empty, false, string.Format("[{0}]", (object) element.Name), this.Identity.Id, true, entityDescriptor.ToString())
        {
          IsReadOnly = true
        });
      else if (!calculation.IsReadOnly)
        this.AddReferencedFields(calculation);
      FieldDescriptor fieldDescriptor = new FieldDescriptor(element.ParentElement.EntityType, element.Name);
      if (this.addedFieldsList.Contains(fieldDescriptor))
        return;
      this.addedFieldsList.Add(fieldDescriptor);
    }

    private void AddReferencedFields(FieldExpressionCalculation calc)
    {
      if (calc == null)
        return;
      this.AddReferencedElements(calc.GetReferencedElements());
    }
  }
}
