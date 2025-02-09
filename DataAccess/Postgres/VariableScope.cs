// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataAccess.Postgres.VariableScope
// Assembly: DataAccess, Version=6.5.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: A079574B-67E2-4BE9-A7E2-5764B684A9D9
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DataAccess.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.DataAccess.Postgres
{
  public class VariableScope : AnonymousBlock
  {
    protected const string TempTablePrefix = "tmp_v_";
    protected bool _hasVariables;
    protected bool _hasAnyValues;
    private IList<DbVariable> _variables;

    protected string TableName => "tmp_v_" + this.BlockName;

    public IList<DbVariable> Variables
    {
      get => this._variables;
      set
      {
        this._variables = value;
        this._hasVariables = value != null && value.Any<DbVariable>();
        this._hasAnyValues = this._hasVariables && value.Any<DbVariable>((Func<DbVariable, bool>) (v => !string.IsNullOrEmpty(v.InitialValue)));
      }
    }

    public VariableScope(PgDbQueryBuilder idbqb, IList<DbVariable> variables, string blockName = "")
      : base(idbqb, blockName)
    {
      this.Variables = variables;
    }

    public override void EmitOpenScope()
    {
      this.CreateTempTable();
      base.EmitOpenScope();
    }

    protected override void BeforeBegin() => this.EmitDeclarations();

    protected override void AfterBegin() => this.ReceiveValuesIn();

    private void CreateTempTable()
    {
      if (!this._hasAnyValues)
        return;
      this.CreateTempTable((Func<DbVariable, string>) (v => this.ValueOf(v)));
    }

    protected void CreateTempTable(Func<DbVariable, string> valueSelector)
    {
      PgQueryHelpers.CreateTempTable(this._idbqb, this.TableName, (IEnumerable<DbVariable>) this.Variables, valueSelector);
    }

    private void EmitDeclarations()
    {
      if (!this._hasVariables)
        return;
      this._idbqb.AppendLine("DECLARE");
      this._idbqb.AppendLine(string.Join("\r\n", this.Variables.Select<DbVariable, string>((Func<DbVariable, string>) (v => "    " + this.VarNameOf(v) + " " + this.TypeOf(v) + ";"))));
    }

    private void ReceiveValuesIn()
    {
      if (!this._hasAnyValues)
        return;
      this._idbqb.AppendLine("SELECT " + string.Join(", ", this.Variables.Select<DbVariable, string>((Func<DbVariable, string>) (v => this.ColumnNameOf(v)))));
      this._idbqb.AppendLine("  INTO " + string.Join(", ", this.Variables.Select<DbVariable, string>((Func<DbVariable, string>) (v => this.VarNameOf(v)))));
      this._idbqb.AppendLine("FROM " + this.TableName + ";");
    }

    protected string VarNameOf(DbVariable variable) => PgQueryHelpers.VarName(variable.RootName);

    protected string ColumnNameOf(DbVariable variable)
    {
      return PgQueryHelpers.RootName(variable.RootName);
    }

    protected string TypeOf(DbVariable variable)
    {
      return PgQueryHelpers.DbTypeName(variable.DbType, variable.MaxLength);
    }

    protected string ValueOf(DbVariable variable)
    {
      return string.IsNullOrEmpty(variable.InitialValue) ? "NULL" : variable.InitialValue;
    }
  }
}
