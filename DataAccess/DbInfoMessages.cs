// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataAccess.DbInfoMessages
// Assembly: DataAccess, Version=6.5.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: A079574B-67E2-4BE9-A7E2-5764B684A9D9
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DataAccess.dll

using System;
using System.Data.SqlClient;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.DataAccess
{
  public class DbInfoMessages : IDisposable
  {
    private SqlConnection conn;
    private StringBuilder messages = new StringBuilder();

    public DbInfoMessages(SqlConnection conn)
    {
      this.conn = conn;
      conn.InfoMessage += new SqlInfoMessageEventHandler(this.conn_InfoMessage);
    }

    private void conn_InfoMessage(object sender, SqlInfoMessageEventArgs e)
    {
      if (this.messages == null)
        return;
      this.messages.Append(e.Message);
    }

    public override string ToString() => this.messages.ToString();

    public void Dispose()
    {
      if (this.messages == null)
        return;
      this.conn.InfoMessage -= new SqlInfoMessageEventHandler(this.conn_InfoMessage);
      this.messages = (StringBuilder) null;
    }
  }
}
