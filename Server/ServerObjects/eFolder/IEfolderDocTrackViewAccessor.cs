// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.eFolder.IEfolderDocTrackViewAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.eFolder
{
  internal interface IEfolderDocTrackViewAccessor
  {
    List<ViewSummary> GetViewsSummary(string userId);

    DocumentTrackingView GetView(string userId, string viewId);

    DocumentTrackingView CreateView(string userId, DocumentTrackingView view);

    DocumentTrackingView UpdateView(string userId, DocumentTrackingView view);

    bool DeleteView(string userId, string viewId);
  }
}
