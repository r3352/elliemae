// Decompiled with JetBrains decompiler
// Type: Elli.CalculationEngine.Core.ExpressionParser.ReferencedElement
// Assembly: Elli.CalculationEngine.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E7988E98-462C-4B95-BC53-687EC5965B19
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.CalculationEngine.Core.dll

using Elli.CalculationEngine.Core.DataSource;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

#nullable disable
namespace Elli.CalculationEngine.Core.ExpressionParser
{
  [DataContract(IsReference = true)]
  public class ReferencedElement : IDisposable
  {
    private string typeFormattedName = string.Empty;
    private bool fieldElementInitialized;
    private ReferencedElement fieldElement;
    private string formattedName;
    private string qualifiedName;
    private List<string> referencedFields;
    private List<string> weakReferenceList;
    private List<string> strongReferenceList;
    [DataMember]
    private EntityDescriptor entityType;
    [DataMember]
    private List<ReferencedElement> referencedElements = new List<ReferencedElement>();

    [DataMember]
    public string Name { get; private set; }

    public EntityDescriptor EntityType
    {
      get => this.entityType;
      set
      {
        this.entityType = value;
        this.typeFormattedName = string.Empty;
        this.formattedName = (string) null;
        this.qualifiedName = (string) null;
      }
    }

    [DataMember]
    public ReferencedElement ParentElement { get; set; }

    [DataMember]
    public bool IsWeak { get; private set; }

    [DataMember]
    public DataElementType DataElementType { get; private set; }

    public List<ReferencedElement> ReferencedElements
    {
      get => this.referencedElements;
      private set
      {
      }
    }

    public ReferencedElement(
      string name,
      EntityDescriptor type,
      DataElementType dataElementType,
      ReferencedElement parentElement,
      bool isWeak)
    {
      this.Name = name;
      this.DataElementType = dataElementType;
      this.ParentElement = parentElement;
      this.IsWeak = isWeak;
      if (dataElementType == DataElementType.DataFieldType)
      {
        this.EntityType = (EntityDescriptor) null;
      }
      else
      {
        if (isWeak)
          throw new Exception("Non field is set to weak reference: " + name);
        if ((object) type == null)
          this.EntityType = new EntityDescriptor(name, (IEnumerable<EntityParameter>) null);
        else
          this.EntityType = type;
      }
    }

    public void AddReferencedElement(ReferencedElement element)
    {
      this.referencedElements.Add(element);
    }

    public void RemoveReferencedElement(ReferencedElement element)
    {
      this.referencedElements.Remove(element);
    }

    public string TypeFormattedName
    {
      get
      {
        if (string.IsNullOrEmpty(this.typeFormattedName))
        {
          int num = this.ParentElement == null ? 1 : 0;
          string str1 = num == 0 ? this.ParentElement.EntityType.ToString() : CalculationUtility.GetRootEntityType();
          string str2 = num == 0 ? this.ParentElement.Name : CalculationUtility.GetRootRelationship();
          this.typeFormattedName = string.IsNullOrEmpty(str1) ? this.Name : string.Format("{{{0}:{1}}}.{2}", (object) str2, (object) str1, (object) this.Name);
        }
        return this.typeFormattedName;
      }
      private set
      {
      }
    }

    public string FormattedName
    {
      get
      {
        if (string.IsNullOrEmpty(this.formattedName))
        {
          string str = this.Name;
          if (!EntityDescriptor.IsNullOrEmpty(this.EntityType))
            str = string.Format("{0}:{1}", string.IsNullOrEmpty(this.Name) ? (object) CalculationUtility.ALL_ENTITIES : (object) this.Name, (object) this.EntityType.ToString());
          switch (this.DataElementType)
          {
            case DataElementType.DataEntityCollectionType:
              this.formattedName = string.Format("{{{{{0}}}}}", (object) str);
              break;
            case DataElementType.DataEntityType:
              this.formattedName = string.Format("{{{0}}}", (object) str);
              break;
            case DataElementType.DataFieldType:
              this.formattedName = !this.IsWeak ? string.Format("[{0}]", (object) str) : string.Format("<{0}>", (object) str);
              break;
          }
        }
        return this.formattedName;
      }
      private set
      {
      }
    }

    public string QualifiedName
    {
      get
      {
        if (string.IsNullOrEmpty(this.qualifiedName))
          this.qualifiedName = this.GetQualifiedName();
        return this.qualifiedName;
      }
      private set
      {
      }
    }

    private string GetQualifiedName()
    {
      string qualifiedName = this.FormattedName;
      if (this.ParentElement != null)
      {
        for (ReferencedElement parentElement = this.ParentElement; parentElement != null; parentElement = parentElement.ParentElement)
          qualifiedName = string.Format("{0}.{1}", (object) parentElement.FormattedName, (object) qualifiedName);
      }
      else
      {
        ReferencedElement referencedElement = this;
        while (referencedElement.ReferencedElements.Count > 0)
        {
          referencedElement = referencedElement.ReferencedElements[0];
          qualifiedName = string.Format("{0}.{1}", (object) qualifiedName, (object) referencedElement.FormattedName);
        }
      }
      return qualifiedName;
    }

    public ReferencedElement FieldElement
    {
      get
      {
        if (!this.fieldElementInitialized)
        {
          this.fieldElement = (ReferencedElement) null;
          ReferencedElement referencedElement = this;
          if (referencedElement.DataElementType == DataElementType.DataFieldType)
          {
            this.fieldElement = referencedElement;
          }
          else
          {
            while (referencedElement.ReferencedElements.Count > 0)
            {
              referencedElement = referencedElement.ReferencedElements[0];
              if (referencedElement.DataElementType == DataElementType.DataFieldType)
                this.fieldElement = referencedElement;
            }
          }
          this.fieldElementInitialized = true;
        }
        return this.fieldElement;
      }
      private set
      {
      }
    }

    public EntityDescriptor GetFieldParentType()
    {
      return this.FieldElement != null && this.FieldElement.ParentElement != null ? this.FieldElement.ParentElement.EntityType : (EntityDescriptor) null;
    }

    public string GetFieldName()
    {
      return this.FieldElement != null ? this.FieldElement.Name : string.Empty;
    }

    public string GetQualifiedNameWithoutRoot(EntityDescriptor rootParentEntityType, bool showTypes = true)
    {
      string qualifiedNameWithoutRoot = this.FormattedName;
      if (this.ParentElement != null)
      {
        ReferencedElement referencedElement = this.ParentElement;
        while (referencedElement != null)
        {
          if (referencedElement.EntityType != rootParentEntityType)
          {
            qualifiedNameWithoutRoot = string.Format("{0}.{1}", (object) referencedElement.FormattedName, (object) qualifiedNameWithoutRoot);
            referencedElement = referencedElement.ParentElement == null || !(referencedElement.ParentElement.EntityType == rootParentEntityType) ? referencedElement.ParentElement : (ReferencedElement) null;
          }
          else
            referencedElement = (ReferencedElement) null;
        }
      }
      return qualifiedNameWithoutRoot;
    }

    public IEnumerable<string> ReferencedFields
    {
      get
      {
        if (this.referencedFields == null)
        {
          this.referencedFields = new List<string>();
          if (this.DataElementType == DataElementType.DataFieldType)
          {
            string qualifiedName = this.QualifiedName;
            if (!this.referencedFields.Contains(qualifiedName))
              this.referencedFields.Add(qualifiedName);
          }
          foreach (ReferencedElement referencedElement in this.ReferencedElements)
            this.referencedFields.AddRange(referencedElement.ReferencedFields);
        }
        return (IEnumerable<string>) this.referencedFields;
      }
      private set
      {
      }
    }

    public IEnumerable<string> WeakReferencedFields
    {
      get
      {
        if (this.weakReferenceList == null)
        {
          this.weakReferenceList = new List<string>();
          if (this.DataElementType == DataElementType.DataFieldType && this.IsWeak)
          {
            string qualifiedName = this.QualifiedName;
            if (!this.weakReferenceList.Contains(qualifiedName))
              this.weakReferenceList.Add(qualifiedName);
          }
          foreach (ReferencedElement referencedElement in this.ReferencedElements)
            this.weakReferenceList.AddRange(referencedElement.WeakReferencedFields);
        }
        return (IEnumerable<string>) this.weakReferenceList;
      }
      private set
      {
      }
    }

    public IEnumerable<string> StrongReferencedFields
    {
      get
      {
        if (this.strongReferenceList == null)
        {
          this.strongReferenceList = new List<string>();
          if (this.DataElementType == DataElementType.DataFieldType && !this.IsWeak)
          {
            string qualifiedName = this.QualifiedName;
            if (!this.strongReferenceList.Contains(qualifiedName))
              this.strongReferenceList.Add(qualifiedName);
          }
          foreach (ReferencedElement referencedElement in this.ReferencedElements)
            this.strongReferenceList.AddRange(referencedElement.StrongReferencedFields);
        }
        return (IEnumerable<string>) this.strongReferenceList;
      }
      private set
      {
      }
    }

    public void Dispose() => this.Dispose(true);

    protected void Dispose(bool disposing)
    {
      if (this.referencedElements == null)
        return;
      foreach (ReferencedElement referencedElement in this.referencedElements)
        referencedElement.Dispose();
      this.referencedElements.Clear();
    }
  }
}
