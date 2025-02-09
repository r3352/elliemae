// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Query.BooleanLogicCriterion
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer.Query;
using System;

#nullable disable
namespace EllieMae.Encompass.Query
{
  /// <summary>
  /// Represents a Query Criterion which is composed of two subcriteria joined by either
  /// an AND or OR logic.
  /// </summary>
  internal class BooleanLogicCriterion : QueryCriterion
  {
    private BinaryOperator op;
    private QueryCriterion lhs;
    private QueryCriterion rhs;

    /// <summary>
    /// Constructs a new BooleanLogicCriterion object by combining the specified subcriteria
    /// with the specified boolean operation.
    /// </summary>
    /// <param name="op">The operation (AND or OR) to be applied to the subcriteria.</param>
    /// <param name="lhs">The query criterion that represents the left hand side of
    /// the boolean operation.</param>
    /// <param name="rhs">The query criterion that represents the right hand side of
    /// the boolean operation.</param>
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

    /// <summary>
    /// Creates an exact duplicate of the object by performing a deep copy.
    /// </summary>
    /// <returns>Returns a deep copy of the current criterion object.</returns>
    public override QueryCriterion Clone()
    {
      return (QueryCriterion) new BooleanLogicCriterion(this.op, this.lhs.Clone(), this.rhs.Clone());
    }

    /// <summary>Not intended for use outside of the Encompass API.</summary>
    /// <returns></returns>
    public override EllieMae.EMLite.ClientServer.Query.QueryCriterion Unwrap()
    {
      return (EllieMae.EMLite.ClientServer.Query.QueryCriterion) new BinaryOperation(this.op, this.lhs.Unwrap(), this.rhs.Unwrap());
    }
  }
}
