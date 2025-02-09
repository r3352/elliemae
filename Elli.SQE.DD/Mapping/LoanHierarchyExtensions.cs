// Decompiled with JetBrains decompiler
// Type: Elli.SQE.DD.Mapping.LoanHierarchyExtensions
// Assembly: Elli.SQE.DD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: AD1B11DD-560E-4083-B59A-D629AF47C790
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.SQE.DD.dll

using Elli.Common.Fields;
using Elli.Common.ModelFields;
using Elli.Domain.Mortgage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#nullable disable
namespace Elli.SQE.DD.Mapping
{
  public static class LoanHierarchyExtensions
  {
    public static void Insert(
      this LoanHierarchy current,
      Loan loan,
      EncompassField field,
      ModelFieldPath modelFieldPath)
    {
      if (modelFieldPath.IsField)
      {
        LoanHierarchy loanHierarchy = current.GetChildrenOfType<LoanPropertyHierarchy>().FirstOrDefault<LoanHierarchy>((Func<LoanHierarchy, bool>) (x => x.Path.FieldName == modelFieldPath.FieldName));
        if (loanHierarchy.IsNotNull<LoanHierarchy>())
          loanHierarchy.CastTo<LoanPropertyHierarchy>().AddOfTheSameKind(field);
        else
          current.InsertProperty(loan, field, modelFieldPath);
      }
      else
      {
        LoanHierarchy current1 = modelFieldPath.IsCollectionExpression ? current.InsertCollection(loan, field, modelFieldPath) : current.InsertEntity(loan, field, modelFieldPath);
        if (!(modelFieldPath.RemainingPath != ""))
          return;
        modelFieldPath.MoveToNextPath();
        current1.Insert(loan, field, modelFieldPath);
      }
    }

    private static LoanHierarchy InsertProperty(
      this LoanHierarchy current,
      Loan loan,
      EncompassField field,
      ModelFieldPath modelFieldPath)
    {
      if (current.Type.GetProperty(modelFieldPath.FieldName) == (PropertyInfo) null)
        return (LoanHierarchy) null;
      return (LoanHierarchy) new LoanPropertyHierarchy(current, loan, field, modelFieldPath)
      {
        RepeatableFieldTag = RepeatableFieldTag.CreateIfRepeatable(current, field)
      };
    }

    private static LoanHierarchy InsertCollection(
      this LoanHierarchy current,
      Loan loan,
      EncompassField field,
      ModelFieldPath modelFieldPath)
    {
      return current.Find((Predicate<LoanHierarchy>) (p => p.Key == LoanCollectionHierarchy.GetKey(modelFieldPath))) ?? (LoanHierarchy) new LoanCollectionHierarchy(current, loan, field, modelFieldPath);
    }

    private static LoanHierarchy InsertEntity(
      this LoanHierarchy current,
      Loan loan,
      EncompassField field,
      ModelFieldPath modelFieldPath)
    {
      return current.Find((Predicate<LoanHierarchy>) (p => p.Key == LoanEntityHierarchy.GetKey(modelFieldPath))) ?? (LoanHierarchy) new LoanEntityHierarchy(current, loan, field, modelFieldPath);
    }

    public static IEnumerable<LoanHierarchy> GetChildrenOfType<T>(
      this LoanHierarchy current,
      HierarchyLevel level = HierarchyLevel.Immediate)
      where T : class
    {
      if (current.Children == null)
        return (IEnumerable<LoanHierarchy>) new List<LoanHierarchy>();
      return level != HierarchyLevel.Immediate ? current.FlattenUnsafe().Where<LoanHierarchy>((Func<LoanHierarchy, bool>) (x => x.GetType() == typeof (T))) : current.Children.Where<LoanHierarchy>((Func<LoanHierarchy, bool>) (x => x.GetType() == typeof (T)));
    }

    public static List<QueryableField> GenerateQueryableFieldsOfTheSameKind(
      this LoanPropertyHierarchy current)
    {
      List<QueryableField> list = new List<QueryableField>();
      if (current.Parent.IsNull<LoanHierarchy>())
        return list;
      LoanCollectionHierarchy collectionHierarchy = current.Parent.CastAs<LoanCollectionHierarchy>();
      if (!collectionHierarchy.IsNull<LoanCollectionHierarchy>())
      {
        if (collectionHierarchy.IsValidRepeatableCollection())
        {
          for (int indexOffset = collectionHierarchy.InstancePattern.IndexOffset; indexOffset <= collectionHierarchy.InstancePattern.MaxIndex; ++indexOffset)
          {
            EncompassField field = current.Field.Clone().CastTo<EncompassField>();
            field.ID = current.Field.GetRepeatableFieldIdForIndex(indexOffset);
            field.ModelPath = current.Field.GetRepeatableFieldModelPath(indexOffset, current.Field.ID);
            list.Add(field.ToQueryableField(current.Type).SetIndex(indexOffset).SetIsRepeatabe(current.RepeatableFieldTag.IsNotNull<RepeatableFieldTag>()));
          }
          return list;
        }
        current.GetOfTheSameKinds().ForEach((Action<LoanPropertyHierarchy.OfTheSameKind>) (x => list.Add(x.Field.ToQueryableField(current.Type))));
      }
      if (!current.Parent.CastAs<LoanEntityHierarchy>().IsNull<LoanEntityHierarchy>())
        current.GetOfTheSameKinds().ForEach((Action<LoanPropertyHierarchy.OfTheSameKind>) (x => list.Add(x.Field.ToQueryableField(current.Type))));
      return list;
    }
  }
}
