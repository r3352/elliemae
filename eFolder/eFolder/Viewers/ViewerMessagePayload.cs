// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Viewers.ViewerMessagePayload
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.eFolder.Viewers
{
  internal class ViewerMessagePayload
  {
    public string title { get; set; }

    public string attachmentId { get; set; }

    public string documentId { get; set; }

    public Dictionary<string, object> parameters { get; set; }

    public ViewerErrorMessage error { get; set; }
  }
}
