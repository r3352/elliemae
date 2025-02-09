// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanUtils.Services.DragDropJobStatusResponse
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using System;

#nullable disable
namespace EllieMae.EMLite.LoanUtils.Services
{
  [Serializable]
  public class DragDropJobStatusResponse
  {
    public EOSError error { get; set; }

    public string objectId { get; set; }

    public string attachmentId { get; set; }

    public string jobId { get; set; }

    public string status { get; set; }

    public DragDropPresignDetails drag { get; set; }

    public DragDropPresignDetails drop { get; set; }
  }
}
