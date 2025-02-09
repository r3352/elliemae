// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataAccess.Postgres.IfThenElseBlock
// Assembly: DataAccess, Version=6.5.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: A079574B-67E2-4BE9-A7E2-5764B684A9D9
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DataAccess.dll

using System;

#nullable disable
namespace EllieMae.EMLite.DataAccess.Postgres
{
  public sealed class IfThenElseBlock : Scope
  {
    public string Predicate { get; set; }

    public IfThenElseBlock(Scope container, string predicate)
      : base(container._idbqb)
    {
      this.Predicate = predicate;
      if (!this.IsSameOrSubclass(typeof (AnonymousBlock), container.GetType()))
        throw new ArgumentException("A container of type (or type derived from) AnonymousBlock is required.");
    }

    public bool IsSameOrSubclass(Type baseType, Type testType)
    {
      return testType.IsSubclassOf(baseType) || testType == baseType;
    }

    public override void EmitOpenScope()
    {
      this._idbqb.AppendLine("IF " + this.Predicate + " THEN");
    }

    public void Else() => this._idbqb.AppendLine("ELSE");

    public override void EmitCloseScope() => this._idbqb.AppendLine("END IF;");
  }
}
