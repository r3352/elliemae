// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.AuditTrailEntry
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.Encompass.Client;
using EllieMae.Encompass.Collections;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  /// <summary>Represents a single entry in an audit trail.</summary>
  public class AuditTrailEntry : IAuditTrailEntry
  {
    private AuditRecord auditRecord;
    private ReadOnlyField field;

    internal AuditTrailEntry(AuditRecord rec, FieldDescriptor descriptor)
    {
      this.auditRecord = rec;
      this.field = new ReadOnlyField(rec.FieldID, string.Concat(rec.DataValue), descriptor);
    }

    /// <summary>Gets the date and time of the change to the field.</summary>
    public DateTime Timestamp => this.auditRecord.ModifiedDateTime;

    /// <summary>Gets the User ID of the user who modified this field.</summary>
    public string UserID => this.auditRecord.UserID;

    /// <summary>
    /// Gets the first name of the user who modified this field.
    /// </summary>
    /// <remarks>This value will represent the user's name at the time the audit record
    /// was recorded.</remarks>
    public string UserFirstName => this.auditRecord.FirstName;

    /// <summary>
    /// Gets the first name of the user who modified this field.
    /// </summary>
    /// <remarks>This value will represent the user's name at the time the audit record
    /// was recorded.</remarks>
    public string UserLastName => this.auditRecord.LastName;

    /// <summary>
    /// Gets the first name of the user who modified this field.
    /// </summary>
    /// <remarks>This value will represent the user's name at the time the audit record
    /// was recorded.</remarks>
    public string UserName => Utils.JoinName(this.auditRecord.FirstName, this.auditRecord.LastName);

    /// <summary>
    /// Returns the Field information for the specified audit record.
    /// </summary>
    public ReadOnlyField Field => this.field;

    internal static AuditTrailEntryList ToList(Session session, AuditRecord[] auditRecords)
    {
      AuditTrailEntryList list = new AuditTrailEntryList();
      for (int index = 0; index < auditRecords.Length; ++index)
      {
        FieldPairInfo fieldPairInfo = FieldPairParser.ParseFieldPairInfo(auditRecords[index].FieldID);
        list.Add(new AuditTrailEntry(auditRecords[index], session.Loans.FieldDescriptors.GetOrCreate(fieldPairInfo.FieldID)));
      }
      return list;
    }
  }
}
