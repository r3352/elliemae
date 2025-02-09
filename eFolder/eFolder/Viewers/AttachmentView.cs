// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Viewers.AttachmentView
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using Newtonsoft.Json;

#nullable disable
namespace EllieMae.EMLite.eFolder.Viewers
{
  public class AttachmentView
  {
    public string attachmentId { get; set; }

    public string url { get; set; }

    public string token { get; set; }

    public string fileName { get; set; }

    public string dragDropJobStatus { get; set; }

    public bool isDragDropJobRunning { get; set; }

    [JsonIgnore]
    public string objectID { get; set; }
  }
}
