// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.AuditTrailEntry
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.Encompass.Client;
using EllieMae.Encompass.Collections;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  public class AuditTrailEntry : IAuditTrailEntry
  {
    private AuditRecord auditRecord;
    private ReadOnlyField field;

    internal AuditTrailEntry(AuditRecord rec, FieldDescriptor descriptor)
    {
      this.auditRecord = rec;
      this.field = new ReadOnlyField(rec.FieldID, string.Concat(rec.DataValue), descriptor);
    }

    public DateTime Timestamp => this.auditRecord.ModifiedDateTime;

    public string UserID => this.auditRecord.UserID;

    public string UserFirstName => this.auditRecord.FirstName;

    public string UserLastName => this.auditRecord.LastName;

    public string UserName => Utils.JoinName(this.auditRecord.FirstName, this.auditRecord.LastName);

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
