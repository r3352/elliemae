// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.LockRequestFields
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.CalculationEngine;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  public class LockRequestFields : ILockRequestFields
  {
    private static FieldDescriptors snapshotFields = new FieldDescriptors();
    private LockRequest request;
    private Hashtable fieldValues;
    private LockRequestCalculator calculator;
    private Hashtable fields = new Hashtable((IEqualityComparer) StringComparer.CurrentCultureIgnoreCase);

    static LockRequestFields()
    {
      foreach (string snapshotField in LockRequestLog.SnapshotFields)
      {
        if (LockRequestFields.snapshotFields[snapshotField] == null)
          LockRequestFields.snapshotFields.Add(FieldDescriptors.StandardFields[snapshotField]);
      }
      foreach (string requestField in LockRequestLog.RequestFields)
      {
        if (LockRequestFields.snapshotFields[requestField] == null)
          LockRequestFields.snapshotFields.Add(FieldDescriptors.StandardFields[requestField]);
      }
      foreach (string lockExtensionField in LockRequestLog.LockExtensionFields)
      {
        if (LockRequestFields.snapshotFields[lockExtensionField] == null)
          LockRequestFields.snapshotFields.Add(FieldDescriptors.StandardFields[lockExtensionField]);
      }
    }

    internal LockRequestFields(LockRequest request, Hashtable fieldValues)
    {
      this.request = request;
      this.fieldValues = fieldValues;
      this.calculator = new LockRequestCalculator(request.Loan.Session.SessionObjects, request.Loan.LoanData, (CalculationObjects) null);
    }

    public FieldDescriptors Descriptors => LockRequestFields.snapshotFields;

    public LockRequestField this[string fieldId]
    {
      get
      {
        LockRequestField field = (LockRequestField) this.fields[(object) (fieldId ?? "")];
        if (field != null)
          return field;
        LockRequestField lockRequestField = new LockRequestField(this.fieldValues, this.request.Loan.Session.Loans.FieldDescriptors[fieldId] ?? throw new ArgumentException("The specified field ID is invalid", nameof (fieldId)));
        this.fields.Add((object) fieldId, (object) lockRequestField);
        return lockRequestField;
      }
    }

    public void Recalculate() => this.calculator.PerformSnapshotCalculations(this.fieldValues);

    public void CommitChanges()
    {
      this.Recalculate();
      this.request.LockRequestLog.AddLockRequestSnapshot(this.fieldValues);
    }

    internal Hashtable FieldTable => this.fieldValues;
  }
}
