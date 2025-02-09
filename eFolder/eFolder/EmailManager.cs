// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.EmailManager
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

#nullable disable
namespace EllieMae.EMLite.eFolder
{
  public class EmailManager
  {
    public const string CcEmailTemplates = "CCEmailTemplates";
    public const string DefaultCcEmailTemplates = "DefaultCCEmailTemplates";

    public enum EmailType
    {
      AuthenticationCode,
      Consent,
      Disclosures,
      PreClosing,
      Request,
      Send,
    }
  }
}
