// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Query.BooleanLogicCriterion
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer.Query;
using System;

#nullable disable
namespace EllieMae.Encompass.Query
{
  internal class BooleanLogicCriterion : QueryCriterion
  {
    private BinaryOperator op;
    private QueryCriterion lhs;
    private QueryCriterion rhs;

    public BooleanLogicCriterion(BinaryOperator op, QueryCriterion lhs, QueryCriterion rhs)
    {
      if (lhs == null)
        throw new ArgumentNullException(nameof (lhs));
      if (rhs == null)
        throw new ArgumentNullException(nameof (rhs));
      this.op = op;
      this.lhs = lhs;
      this.rhs = rhs;
    }

    public override QueryCriterion Clone()
    {
      return (QueryCriterion) new BooleanLogicCriterion(this.op, this.lhs.Clone(), this.rhs.Clone());
    }

    public override QueryCriterion Unwrap()
    {
      return (QueryCriterion) new BinaryOperation(this.op, this.lhs.Unwrap(), this.rhs.Unwrap());
    }
  }
}
