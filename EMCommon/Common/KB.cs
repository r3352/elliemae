// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.KB
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

#nullable disable
namespace EllieMae.EMLite.Common
{
  public class KB
  {
    private static readonly string localization = "l10n";
    private static readonly string internationalization = "i18n";
    public static string KB64 = XT.ESB64Legacy(KB.internationalization + "@" + KB.localization, KB.localization + "@" + KB.internationalization);
    public static string SC64 = XT.ESB64Legacy(KB.localization + "@" + KB.internationalization, KB.internationalization + "@" + KB.localization);
  }
}
