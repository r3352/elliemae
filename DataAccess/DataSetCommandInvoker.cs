// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataAccess.DataSetCommandInvoker
// Assembly: DataAccess, Version=6.5.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: A079574B-67E2-4BE9-A7E2-5764B684A9D9
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DataAccess.dll

using System.Data;

#nullable disable
namespace EllieMae.EMLite.DataAccess
{
  public class DataSetCommandInvoker : DbCommandInvoker
  {
    private readonly IDbDataAdapter _adapter;

    public DataSetCommandInvoker(IDbCommand cmd, IDbDataAdapter adapter)
      : base(cmd)
    {
      this._adapter = adapter;
    }

    public override object Execute()
    {
      DataSet dataSet = new DataSet();
      this._adapter.SelectCommand = this.CommandToInvoke;
      this._adapter.Fill(dataSet);
      return (object) dataSet;
    }
  }
}
