// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataAccess.Postgres.Scope
// Assembly: DataAccess, Version=6.5.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: A079574B-67E2-4BE9-A7E2-5764B684A9D9
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DataAccess.dll

#nullable disable
namespace EllieMae.EMLite.DataAccess.Postgres
{
  public abstract class Scope
  {
    internal PgDbQueryBuilder _idbqb;

    protected Scope(PgDbQueryBuilder idbqb) => this._idbqb = idbqb;

    public abstract void EmitOpenScope();

    public abstract void EmitCloseScope();
  }
}
