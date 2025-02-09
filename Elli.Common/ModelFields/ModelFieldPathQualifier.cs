// Decompiled with JetBrains decompiler
// Type: Elli.Common.ModelFields.ModelFieldPathQualifier
// Assembly: Elli.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 5A516607-8D77-4351-85BB-54751A6A69B4
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Common.dll

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace Elli.Common.ModelFields
{
  public class ModelFieldPathQualifier
  {
    private int hashCode;
    private Dictionary<Type, ModelFieldPathQualifier.QualifierEvaluator> evaluators = new Dictionary<Type, ModelFieldPathQualifier.QualifierEvaluator>();

    public ModelFieldPathQualifier(string name, string value, bool isValueQuoted)
    {
      this.Name = name;
      this.Value = value;
      this.IsValueQuoted = isValueQuoted;
      if (value == null)
        this.hashCode = StringComparer.OrdinalIgnoreCase.GetHashCode(name);
      else
        this.hashCode = StringComparer.OrdinalIgnoreCase.GetHashCode(name) ^ StringComparer.OrdinalIgnoreCase.GetHashCode(value);
    }

    public ModelFieldPathQualifier(string name, string value)
      : this(name, value, true)
    {
    }

    public string Name { get; private set; }

    public string Value { get; private set; }

    public bool IsValueQuoted { get; private set; }

    public bool Match(object target)
    {
      ModelFieldPathQualifier.QualifierEvaluator qualifierEvaluator = (ModelFieldPathQualifier.QualifierEvaluator) null;
      lock (this.evaluators)
        this.evaluators.TryGetValue(target.GetType(), out qualifierEvaluator);
      if (qualifierEvaluator == null)
      {
        qualifierEvaluator = ModelFieldPathQualifier.QualifierEvaluator.Create(target.GetType(), this.Name, this.Value);
        lock (this.evaluators)
          this.evaluators[target.GetType()] = qualifierEvaluator;
      }
      return qualifierEvaluator.Match(target);
    }

    public override int GetHashCode() => this.hashCode;

    public override bool Equals(object obj)
    {
      return obj is ModelFieldPathQualifier fieldPathQualifier && (this.Value != null || fieldPathQualifier.Value == null) && (this.Value == null || fieldPathQualifier.Value != null) && StringComparer.OrdinalIgnoreCase.Compare(this.Name, fieldPathQualifier.Name) == 0 && StringComparer.OrdinalIgnoreCase.Compare(this.Value, fieldPathQualifier.Value) == 0;
    }

    private class QualifierEvaluator
    {
      private Func<object, bool> evaluator;

      private QualifierEvaluator(Func<object, bool> evaluator) => this.evaluator = evaluator;

      public bool Match(object target) => this.evaluator(target);

      public static ModelFieldPathQualifier.QualifierEvaluator Create(
        Type type,
        string propName,
        string propValueText)
      {
        Type propertyType = type.GetProperty(propName).PropertyType;
        Type type1 = propertyType;
        object obj = (object) propValueText;
        if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof (Nullable<>))
          type1 = propertyType.GetGenericArguments()[0];
        if (type1.IsEnum)
          obj = Enum.Parse(type1, propValueText, true);
        else if (type1 != typeof (string))
          obj = Convert.ChangeType((object) propValueText, type1);
        ParameterExpression left = Expression.Parameter(propertyType, "val");
        ParameterExpression parameterExpression1 = Expression.Parameter(typeof (object), "target");
        ParameterExpression[] variables = new ParameterExpression[1]
        {
          left
        };
        Expression[] expressionArray = new Expression[2]
        {
          (Expression) Expression.Assign((Expression) left, (Expression) Expression.Property((Expression) Expression.Convert((Expression) parameterExpression1, type), type.GetProperty(propName))),
          null
        };
        BinaryExpression binaryExpression;
        if (!(propertyType == typeof (string)))
          binaryExpression = Expression.Equal((Expression) left, (Expression) Expression.Constant(obj, propertyType));
        else
          binaryExpression = Expression.Equal((Expression) Expression.Call(typeof (string).GetMethod("Compare", BindingFlags.Static | BindingFlags.Public, (Binder) null, new Type[3]
          {
            typeof (string),
            typeof (string),
            typeof (StringComparison)
          }, (ParameterModifier[]) null), (Expression) left, (Expression) Expression.Constant((object) propValueText), (Expression) Expression.Constant((object) StringComparison.OrdinalIgnoreCase)), (Expression) Expression.Constant((object) 0));
        expressionArray[1] = (Expression) binaryExpression;
        return new ModelFieldPathQualifier.QualifierEvaluator(((Expression<Func<object, bool>>) (parameterExpression => Expression.Block((IEnumerable<ParameterExpression>) variables, expressionArray))).Compile());
      }
    }
  }
}
