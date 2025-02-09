// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.LoanXDBAuditField
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class LoanXDBAuditField
  {
    public const string AuditFieldPrefix = "AuditTrail�";
    private LoanXDBField dbField;
    private AuditTrailDataElement dataElement;
    private string description;
    private string columnName;

    public LoanXDBAuditField(LoanXDBField dbField, AuditTrailDataElement dataElement)
    {
      this.dbField = dbField;
      this.dataElement = dataElement;
      switch (this.dbField.ComortgagorPair)
      {
        case 2:
          this.description = dbField.Description + " " + LoanXDBAuditField.GetDescriptionForDataElement(dataElement) + " - 2nd";
          break;
        case 3:
          this.description = dbField.Description + " " + LoanXDBAuditField.GetDescriptionForDataElement(dataElement) + " - 3rd";
          break;
        case 4:
          this.description = dbField.Description + " " + LoanXDBAuditField.GetDescriptionForDataElement(dataElement) + " - 4th";
          break;
        default:
          this.description = dbField.Description + " " + LoanXDBAuditField.GetDescriptionForDataElement(dataElement);
          break;
      }
      this.columnName = LoanXDBAuditField.GetColumnNameForDataElement(this.dataElement);
    }

    public LoanXDBField DatabaseField => this.dbField;

    public AuditTrailDataElement DataElement => this.dataElement;

    public string ReportingCriterionName
    {
      get => "AuditTrail." + this.dbField.FieldIDWithCoMortgagor + "." + (object) this.dataElement;
    }

    public string Description => this.description;

    public string ColumnName => this.columnName;

    public LoanXDBTableList.TableTypes FieldType
    {
      get
      {
        if (this.dataElement == AuditTrailDataElement.ModifiedDate)
          return LoanXDBTableList.TableTypes.IsDate;
        return this.dataElement == AuditTrailDataElement.ModifiedValue || this.dataElement == AuditTrailDataElement.PreviousValue ? this.dbField.FieldType : LoanXDBTableList.TableTypes.IsString;
      }
    }

    public static string GetLegacyAuditTableName(string uiFieldID)
    {
      return "AuditTrail_" + LoanXDBField.GetDbColumnName(uiFieldID);
    }

    public static string GetColumnNameForDataElement(AuditTrailDataElement dataElement)
    {
      switch (dataElement)
      {
        case AuditTrailDataElement.ModifiedBy:
          return "UserID";
        case AuditTrailDataElement.ModifiedByFirstName:
          return "FirstName";
        case AuditTrailDataElement.ModifiedByLastName:
          return "LastName";
        case AuditTrailDataElement.ModifiedDate:
          return "ModifiedDTTM";
        case AuditTrailDataElement.ModifiedValue:
          return "Data";
        case AuditTrailDataElement.PreviousValue:
          return "PreviousData";
        default:
          throw new ArgumentException("fieldName", "The value '" + (object) dataElement + "' is not a recognized audit field type");
      }
    }

    public static string GetDescriptionForDataElement(AuditTrailDataElement dataElement)
    {
      switch (dataElement)
      {
        case AuditTrailDataElement.ModifiedBy:
          return "Last Modified By";
        case AuditTrailDataElement.ModifiedByFirstName:
          return "Last Modified By (First Name)";
        case AuditTrailDataElement.ModifiedByLastName:
          return "Last Modified By (Last Name)";
        case AuditTrailDataElement.ModifiedDate:
          return "Last Modified Date";
        case AuditTrailDataElement.ModifiedValue:
          return "Last Modified Value";
        case AuditTrailDataElement.PreviousValue:
          return "Previous Value";
        default:
          throw new ArgumentException("fieldName", "The value '" + (object) dataElement + "' is not a recognized audit field type");
      }
    }

    public static bool IsAuditFieldID(string fieldId)
    {
      return fieldId.ToLower().StartsWith("AuditTrail".ToLower() + ".");
    }
  }
}
