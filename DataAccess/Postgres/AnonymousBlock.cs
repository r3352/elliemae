// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataAccess.Postgres.AnonymousBlock
// Assembly: DataAccess, Version=6.5.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: A079574B-67E2-4BE9-A7E2-5764B684A9D9
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DataAccess.dll

#nullable disable
namespace EllieMae.EMLite.DataAccess.Postgres
{
  public class AnonymousBlock : Scope
  {
    public string BlockName { get; set; }

    protected AnonymousBlock(PgDbQueryBuilder idbqb, string blockName = "")
      : base(idbqb)
    {
      this.BlockName = blockName;
    }

    public override void EmitOpenScope()
    {
      this._idbqb.AppendLine("DO $" + this.BlockName + "$");
      this.BeforeBegin();
      this._idbqb.AppendLine("BEGIN");
      this.AfterBegin();
    }

    public override void EmitCloseScope() => this._idbqb.AppendLine("END$" + this.BlockName + "$;");

    protected virtual void BeforeBegin()
    {
    }

    protected virtual void AfterBegin()
    {
    }
  }
}
