// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataAccess.DbVariable
// Assembly: DataAccess, Version=6.5.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: A079574B-67E2-4BE9-A7E2-5764B684A9D9
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DataAccess.dll

using System.Data;

#nullable disable
namespace EllieMae.EMLite.DataAccess
{
  public class DbVariable
  {
    public string RootName { get; set; }

    public DbType DbType { get; set; }

    public int MaxLength { get; set; }

    public string InitialValue { get; set; }

    public DbVariable(string rootName, DbType dbType, string initialValue)
      : this(rootName, dbType, 0, initialValue)
    {
    }

    public DbVariable(string rootName, DbType dbType, int maxLength = 0, string initialValue = null)
    {
      this.RootName = rootName;
      this.DbType = dbType;
      this.MaxLength = maxLength;
      this.InitialValue = initialValue;
    }
  }
}
