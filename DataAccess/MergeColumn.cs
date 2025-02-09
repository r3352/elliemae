// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataAccess.MergeColumn
// Assembly: DataAccess, Version=6.5.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: A079574B-67E2-4BE9-A7E2-5764B684A9D9
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DataAccess.dll

#nullable disable
namespace EllieMae.EMLite.DataAccess
{
  public class MergeColumn
  {
    public DbColumnType DataType { get; }

    public int MaxLength { get; }

    public MergeIntent Intent { get; }

    public string Name { get; }

    public MergeColumn(string name, MergeIntent intent, DbColumnType dataType, int maxLength)
    {
      this.Name = name;
      this.Intent = intent;
      this.MaxLength = maxLength;
      this.DataType = dataType;
    }
  }
}
