// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Export.IClient
// Assembly: EMExport, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D06A4C35-7634-4F74-B132-8DD78784C14A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMExport.dll

#nullable disable
namespace EllieMae.EMLite.Export
{
  public interface IClient
  {
    string GetAccessibleClients();

    string GetAccessibleClientIDs(string encompassVersion);

    IBam Bam { get; set; }
  }
}
