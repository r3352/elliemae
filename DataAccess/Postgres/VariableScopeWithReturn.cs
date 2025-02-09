// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataAccess.Postgres.VariableScopeWithReturn
// Assembly: DataAccess, Version=6.5.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: A079574B-67E2-4BE9-A7E2-5764B684A9D9
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DataAccess.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.DataAccess.Postgres
{
  public class VariableScopeWithReturn : VariableScope
  {
    public VariableScopeWithReturn(
      PgDbQueryBuilder idbqb,
      IList<DbVariable> variables,
      string blockName = "")
      : base(idbqb, variables, blockName)
    {
      if (variables == null || !variables.Any<DbVariable>())
        throw new ArgumentException("VariableScopeWithReturn requires a list of variables whose ending values will be returned");
    }

    public override void EmitCloseScope()
    {
      this.TransferValuesOut();
      base.EmitCloseScope();
      this.ReceiveValuesOut();
    }

    protected void TransferValuesOut()
    {
      if (!this._hasAnyValues)
        this.CreateTempTable((Func<DbVariable, string>) (v => this.VarNameOf(v)));
      else
        this.UpdateTempTable();
    }

    protected void UpdateTempTable()
    {
      this._idbqb.AppendLine("UPDATE " + this.TableName);
      this._idbqb.AppendLine("SET " + string.Join(", ", this.Variables.Select<DbVariable, string>((Func<DbVariable, string>) (v => this.TableName + "." + this.ColumnNameOf(v) + " = " + this.VarNameOf(v)))) + ";");
    }

    protected void ReceiveValuesOut()
    {
      this._idbqb.AppendLine("SELECT " + string.Join(", ", this.Variables.Select<DbVariable, string>((Func<DbVariable, string>) (v => this.ColumnNameOf(v)))) + " FROM " + this.TableName + ";");
    }
  }
}
