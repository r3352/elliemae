// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.AuditRecord
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class AuditRecord
  {
    private int recordId;
    private DateTime dttm;
    private string userID;
    private string firstName;
    private string lastName;
    private object dataValue;
    private string fieldID;
    private string loanGuid;
    private object previousValue;

    public AuditRecord(
      int recordId,
      string fieldID,
      string loanGuid,
      string userID,
      string firstName,
      string lastName,
      object dataValue,
      object previousValue,
      DateTime modifiedDTTM)
    {
      this.dttm = modifiedDTTM;
      this.userID = userID;
      this.firstName = firstName;
      this.lastName = lastName;
      this.dataValue = dataValue;
      this.fieldID = fieldID;
      this.loanGuid = loanGuid;
      this.recordId = recordId;
      this.previousValue = previousValue;
    }

    public int RecordId => this.recordId;

    public string LoanGuid => this.loanGuid;

    public string FieldID => this.fieldID;

    public object DataValue => this.dataValue;

    public string LastName => this.lastName;

    public string FirstName => this.firstName;

    public string UserID => this.userID;

    public DateTime ModifiedDateTime => this.dttm;

    public object PreviousValue => this.previousValue;
  }
}
