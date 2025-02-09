// Decompiled with JetBrains decompiler
// Type: Elli.SQE.DD.Mapping.LoanPropertyHierarchy
// Assembly: Elli.SQE.DD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: AD1B11DD-560E-4083-B59A-D629AF47C790
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.SQE.DD.dll

using Elli.Common.Fields;
using Elli.Common.ModelFields;
using Elli.Domain.DomainModelSchema;
using Elli.Domain.Mortgage;
using Elli.SQE.Mapping;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

#nullable disable
namespace Elli.SQE.DD.Mapping
{
  public class LoanPropertyHierarchy : 
    LoanHierarchy,
    IModelPropertyHierarchy<LoanHierarchy>,
    IModelHierarchy<LoanHierarchy>
  {
    public LoanPropertyHierarchy(
      LoanHierarchy parent,
      Loan loan,
      EncompassField field,
      ModelFieldPath modelPath)
      : base(parent, loan, field, modelPath)
    {
      this.OfTheSameKinds = new List<LoanPropertyHierarchy.OfTheSameKind>();
      this.Key = LoanPropertyHierarchy.GetKey(modelPath);
      PropertyInfo property = parent.Type.GetProperty(modelPath.FieldName);
      this.Type = !(property == (PropertyInfo) null) ? property.PropertyType : throw new ApplicationException("LoanPropertyHierarchy cannot find domain model property at '" + (object) modelPath + "'");
      this.ExcludeFromMapping = property.GetCustomAttribute<ExcludeElasticMapping>() != null;
    }

    private List<LoanPropertyHierarchy.OfTheSameKind> OfTheSameKinds { get; set; }

    public RepeatableFieldTag RepeatableFieldTag { get; set; }

    public List<LoanPropertyHierarchy.OfTheSameKind> GetOfTheSameKinds() => this.OfTheSameKinds;

    public void AddOfTheSameKind(EncompassField field)
    {
      this.AddOfTheSameKind(new LoanPropertyHierarchy.OfTheSameKind(field));
    }

    public void AddOfTheSameKind(LoanPropertyHierarchy.OfTheSameKind obj)
    {
      this.OfTheSameKinds.Add(obj);
    }

    public static string GetKey(ModelFieldPath modelPath) => modelPath.CurrentPath;

    [SpecialName]
    bool IModelHierarchy<LoanHierarchy>.get_IsRoot() => this.IsRoot;

    [SpecialName]
    List<LoanHierarchy> IModelHierarchy<LoanHierarchy>.get_Children() => this.Children;

    [SpecialName]
    void IModelHierarchy<LoanHierarchy>.set_Children(List<LoanHierarchy> value)
    {
      this.Children = value;
    }

    [SpecialName]
    bool IModelHierarchy<LoanHierarchy>.get_ExcludeFromMapping() => this.ExcludeFromMapping;

    public class OfTheSameKind
    {
      public OfTheSameKind(EncompassField field) => this.Field = field;

      public EncompassField Field { get; private set; }
    }
  }
}
