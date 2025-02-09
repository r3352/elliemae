// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataAccess.NonQueryCommandInvoker
// Assembly: DataAccess, Version=6.5.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: A079574B-67E2-4BE9-A7E2-5764B684A9D9
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DataAccess.dll

using System.Data;

#nullable disable
namespace EllieMae.EMLite.DataAccess
{
  public class NonQueryCommandInvoker(IDbCommand cmd) : DbCommandInvoker(cmd)
  {
    public override object Execute() => (object) this.CommandToInvoke.ExecuteNonQuery();
  }
}
