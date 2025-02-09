// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Viewers.ViewerMessage
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

#nullable disable
namespace EllieMae.EMLite.eFolder.Viewers
{
  internal class ViewerMessage
  {
    public string action { get; set; }

    public string[] attachments { get; set; }

    public DragDropAttachment[] dragDropAttachments { get; set; }

    public string dragDropJobId { get; set; }

    public string[] documents { get; set; }

    public url[] urls { get; set; }
  }
}
