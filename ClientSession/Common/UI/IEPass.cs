// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.IEPass
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

using EllieMae.EMLite.DataEngine.Log;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public interface IEPass
  {
    string UserPassword { get; }

    void Navigate(string url);

    void Navigate(string url, bool checkAccess);

    bool ProcessURL(string url);

    bool ProcessURL(string url, bool checkAccess);

    bool ProcessURL(string url, bool checkAccess, bool saveLoan);

    bool Retrieve(DocumentLog doc);

    bool SendMessage(string msgFile);

    void View(string docTitle);

    object ProcessURL2(string url);

    object ProcessURL2(string url, bool checkAccess);

    bool Retrieve(DocumentLog doc, bool showDocumentDetailsDialog);
  }
}
