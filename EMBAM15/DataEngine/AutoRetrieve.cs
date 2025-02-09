// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.AutoRetrieve
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public enum AutoRetrieve
  {
    ESignedDocuments = 1,
    DocumentsUploadedToConsumerConnectTask = 2,
    DocumentsUploadedWithoutATask = 3,
    Fax = 4,
    Scan = 5,
    ESignedDocumentsAfterMilestone = 6,
    DocumentsUploadedToConsumerConnectTaskAfterMilestone = 7,
    DocumentsUploadedWithoutATaskAfterMilestone = 8,
    FaxAfterMilestone = 9,
    ScanAfterMilestone = 10, // 0x0000000A
  }
}
