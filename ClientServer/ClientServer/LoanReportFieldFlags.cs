// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.LoanReportFieldFlags
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Flags]
  public enum LoanReportFieldFlags
  {
    IncludeBasicFields = 1,
    IncludeCustomFields = 2,
    IncludeReportingFields = 4,
    IncludeAuditFields = 8,
    LoanDataFieldsOnly = 16, // 0x00000010
    DatabaseFieldsOnly = 32, // 0x00000020
    AllFields = IncludeAuditFields | IncludeReportingFields | IncludeCustomFields | IncludeBasicFields, // 0x0000000F
    BasicLoanDataFields = LoanDataFieldsOnly | IncludeCustomFields | IncludeBasicFields, // 0x00000013
    AllLoanDataFields = BasicLoanDataFields | IncludeReportingFields, // 0x00000017
    DatabaseFieldsNoAudit = DatabaseFieldsOnly | IncludeReportingFields | IncludeCustomFields | IncludeBasicFields, // 0x00000027
    AllDatabaseFields = DatabaseFieldsNoAudit | IncludeAuditFields, // 0x0000002F
    LoanDataFieldsInDatabase = DatabaseFieldsNoAudit | LoanDataFieldsOnly, // 0x00000037
  }
}
