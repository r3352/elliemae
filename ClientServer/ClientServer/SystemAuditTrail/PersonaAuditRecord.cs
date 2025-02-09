// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.SystemAuditTrail.PersonaAuditRecord
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.SystemAuditTrail
{
  [Serializable]
  public class PersonaAuditRecord : SystemAuditRecord
  {
    private int personaID;
    private string personaName = "";

    public PersonaAuditRecord(
      string invokeUserID,
      string invokeUserFullName,
      ActionType action,
      DateTime loginTime,
      int personaID,
      string personaName)
      : base(invokeUserID, invokeUserFullName, action, loginTime, AuditObjectType.Persona)
    {
      this.personaID = personaID;
      this.personaName = personaName;
    }

    public int PersonaID
    {
      get => this.personaID;
      set => this.personaID = value;
    }

    public string PersonaName
    {
      get => this.personaName;
      set => this.personaName = value;
    }

    public override string[] ColumnHeaders
    {
      get
      {
        return new string[5]
        {
          "Performed by User",
          "Action Type",
          "DateTime",
          "Persona ID",
          "Persona Name"
        };
      }
    }

    public override string[] ColumnContents
    {
      get
      {
        return new string[5]
        {
          this.userID,
          this.actionType.ToString(),
          this.dateTime.ToString("MM/dd/yyyy HH:mm:ss"),
          this.PersonaID.ToString(),
          this.PersonaName
        };
      }
    }

    public override string ContentToCSV
    {
      get
      {
        return string.Join(",", this.CSVEncode(this.userID), this.CSVEncode(this.actionType.ToString()), this.CSVEncode(this.dateTime.ToString("MM/dd/yyyy HH:mm:ss")), this.CSVEncode(this.PersonaID.ToString()), this.CSVEncode(this.PersonaName));
      }
    }
  }
}
