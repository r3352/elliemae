// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataAccess.DbEncoding
// Assembly: DataAccess, Version=6.5.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: A079574B-67E2-4BE9-A7E2-5764B684A9D9
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DataAccess.dll

#nullable disable
namespace EllieMae.EMLite.DataAccess
{
  public class DbEncoding
  {
    public static readonly NonEncoder None = new NonEncoder();
    public static readonly ColumnEncoder Default = new ColumnEncoder();
    public static readonly YesNoEncoder YesNo = new YesNoEncoder();
    public static readonly FlagEncoder Flag = new FlagEncoder();
    public static readonly NullEncoder EmptyStringAsNull = new NullEncoder((object) "");
    public static readonly NullEncoder MinusOneAsNull = new NullEncoder((object) -1);
    public static readonly DateTimeEncoder DateTime = new DateTimeEncoder(false);
    public static readonly DateTimeEncoder ShortDateTime = new DateTimeEncoder(true);
    public static readonly SqlVariantEncoder SqlVariant = new SqlVariantEncoder();

    private DbEncoding()
    {
    }
  }
}
