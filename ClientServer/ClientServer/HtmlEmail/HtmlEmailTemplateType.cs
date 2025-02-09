// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.HtmlEmail.HtmlEmailTemplateType
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.HtmlEmail
{
  [Flags]
  public enum HtmlEmailTemplateType
  {
    Unknown = 0,
    StatusOnline = 1,
    RequestDocuments = 2,
    SendDocuments = 4,
    InitialDisclosures = 8,
    SecureFormTransfer = 16, // 0x00000010
    LoanLevelConsent = 32, // 0x00000020
    PreClosing = 64, // 0x00000040
    ConsumerConnectPreClosing = 128, // 0x00000080
    ConsumerConnectRequestDocuments = 256, // 0x00000100
    ConsumerConnectSendDocuments = 512, // 0x00000200
    ConsumerConnectInitialDisclosures = 1024, // 0x00000400
    ConsumerConnectLoanLevelConsent = 2048, // 0x00000800
    ConsumerConnectStatusOnline = 4096, // 0x00001000
  }
}
