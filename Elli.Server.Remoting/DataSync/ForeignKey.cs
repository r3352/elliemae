// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.DataSync.ForeignKey
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

#nullable disable
namespace Elli.Server.Remoting.DataSync
{
  public struct ForeignKey(
    string foreignKeyColumn,
    string primaryKeyTableName,
    string primaryKeyTableColumn,
    string isEntryColumn)
  {
    public string ForeignKeyColumn = foreignKeyColumn;
    public string PrimaryKeyTableName = primaryKeyTableName;
    public string PrimaryKeyTableColumn = primaryKeyTableColumn;
    public bool IsEntryColumn = !(isEntryColumn == "False");
  }
}
