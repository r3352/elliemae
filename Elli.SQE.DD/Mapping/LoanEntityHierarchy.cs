// Decompiled with JetBrains decompiler
// Type: Elli.SQE.DD.Mapping.LoanEntityHierarchy
// Assembly: Elli.SQE.DD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: AD1B11DD-560E-4083-B59A-D629AF47C790
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.SQE.DD.dll

using Elli.Common.Fields;
using Elli.Common.ModelFields;
using Elli.Domain.ModelFields;
using Elli.Domain.Mortgage;
using Elli.SQE.Mapping;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

#nullable disable
namespace Elli.SQE.DD.Mapping
{
  public class LoanEntityHierarchy : 
    LoanHierarchy,
    IModelEntityHierarchy<LoanHierarchy>,
    IModelHierarchy<LoanHierarchy>
  {
    public LoanEntityHierarchy(LoanHierarchy parent)
      : base(parent, (Loan) null, (EncompassField) null, (ModelFieldPath) null)
    {
    }

    public LoanEntityHierarchy(
      LoanHierarchy parent,
      Loan loan,
      EncompassField field,
      ModelFieldPath modelPath)
      : base(parent, loan, field, modelPath)
    {
      Type propertyType = ModelTraverser.GetModel((object) loan, modelPath.CurrentPath).GetType().GetProperty(modelPath.CurrentPathExpression).PropertyType;
      this.Key = LoanEntityHierarchy.GetKey(modelPath);
      this.Type = propertyType;
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
  }
}
