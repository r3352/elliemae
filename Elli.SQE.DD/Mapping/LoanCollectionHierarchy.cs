// Decompiled with JetBrains decompiler
// Type: Elli.SQE.DD.Mapping.LoanCollectionHierarchy
// Assembly: Elli.SQE.DD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: AD1B11DD-560E-4083-B59A-D629AF47C790
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.SQE.DD.dll

using Elli.Common.Fields;
using Elli.Common.ModelFields;
using Elli.Domain.DomainModelSchema;
using Elli.Domain.ModelFields;
using Elli.Domain.Mortgage;
using Elli.SQE.Mapping;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

#nullable disable
namespace Elli.SQE.DD.Mapping
{
  public class LoanCollectionHierarchy : 
    LoanHierarchy,
    IModelCollectionHierarchy<LoanHierarchy>,
    IModelHierarchy<LoanHierarchy>
  {
    public LoanCollectionHierarchy(
      LoanHierarchy parent,
      Loan loan,
      EncompassField field,
      ModelFieldPath modelPath)
      : base(parent, loan, field, modelPath)
    {
      Type type = ModelTraverser.GetModel((object) loan, modelPath.CollectionExpression).GetType();
      ModelFieldPathExpression fieldPathExpression = ModelFieldPathExpression.Parse(modelPath.CurrentPathExpression);
      string collectionName = fieldPathExpression.CollectionName;
      PropertyInfo property = type.GetProperty(collectionName);
      Type genericArgument = property.PropertyType.GetGenericArguments()[0];
      this.Key = LoanCollectionHierarchy.GetKey(modelPath);
      this.Type = genericArgument;
      this.IsRepeatable = fieldPathExpression.IsRepeatableCollection;
      this.Index = fieldPathExpression.Index;
      if (modelPath.CurrentPathExpression == "Applications[0]")
        this.Index = 0;
      if (!this.IsRepeatable)
        return;
      this.InstancePattern = property.GetCustomAttribute<InstancePatternAttribute>();
    }

    public bool IsRepeatable { get; private set; }

    public int Index { get; private set; }

    public InstancePatternAttribute InstancePattern { get; set; }

    public bool IsValidRepeatableCollection() => this.IsRepeatable && this.InstancePattern != null;

    public static string GetKey(ModelFieldPath modelPath) => modelPath.CollectionExpression;

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
