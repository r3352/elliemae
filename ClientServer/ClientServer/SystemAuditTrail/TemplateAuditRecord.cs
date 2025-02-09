// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.SystemAuditTrail.TemplateAuditRecord
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.SystemAuditTrail
{
  [Serializable]
  public class TemplateAuditRecord : SystemAuditRecord
  {
    private string templateName = "";
    private string templatePath = "";

    public TemplateAuditRecord(
      string loginUserID,
      string loginUserFullName,
      ActionType actionType,
      DateTime dateTime,
      string templateName,
      string templatePath)
      : base(loginUserID, loginUserFullName, actionType, dateTime, AuditObjectType.Template)
    {
      this.templateName = templateName;
      this.templatePath = templatePath;
    }

    public string TemplateName
    {
      get => this.templateName;
      set => this.templateName = value;
    }

    public string TemplatePath
    {
      get => this.templatePath;
      set => this.templatePath = value;
    }

    public override string[] ColumnHeaders
    {
      get
      {
        return new string[5]
        {
          "Performed by",
          "Action Type",
          "DateTime",
          "Template Name",
          "Template Path"
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
          this.TemplateName,
          this.TemplatePath
        };
      }
    }

    public override string ContentToCSV
    {
      get
      {
        return string.Join(",", this.CSVEncode(this.userID), this.CSVEncode(this.actionType.ToString()), this.CSVEncode(this.dateTime.ToString("MM/dd/yyyy HH:mm:ss")), this.CSVEncode(this.TemplateName), this.CSVEncode(this.TemplatePath));
      }
    }
  }
}
