// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.DisclosureFields
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  public class DisclosureFields : IDisclosureFields
  {
    private DisclosureBase disclosure;
    private Hashtable disclosedLoanItems = new Hashtable((IEqualityComparer) StringComparer.CurrentCultureIgnoreCase);
    private FieldDescriptors fieldDescriptors;
    private Hashtable disclosureFieldCache = new Hashtable((IEqualityComparer) StringComparer.CurrentCultureIgnoreCase);

    internal DisclosureFields(DisclosureBase disclosure)
    {
      this.disclosure = disclosure;
      foreach (DisclosedLoanItem disclosedLoanItem in ((DisclosureTrackingBase) disclosure.Unwrap()).DisclosedData)
        this.disclosedLoanItems[(object) disclosedLoanItem.FieldID] = (object) disclosedLoanItem;
    }

    public FieldDescriptors Descriptors
    {
      get
      {
        if (this.fieldDescriptors == null)
        {
          this.fieldDescriptors = new FieldDescriptors();
          foreach (string key in (IEnumerable) this.disclosedLoanItems.Keys)
          {
            FieldDefinition field = (FieldDefinition) StandardFields.GetField(key);
            if (field != null)
              this.fieldDescriptors.Add(new FieldDescriptor(field));
          }
        }
        return this.fieldDescriptors;
      }
    }

    public DisclosureField this[string fieldId]
    {
      get
      {
        DisclosureField disclosureField1 = (DisclosureField) this.disclosureFieldCache[(object) (fieldId ?? "")];
        if (disclosureField1 != null)
          return disclosureField1;
        string str = string.Empty;
        DisclosedLoanItem disclosedLoanItem = (DisclosedLoanItem) this.disclosedLoanItems[(object) (fieldId ?? "")];
        if (disclosedLoanItem != null)
          str = disclosedLoanItem.FieldValue;
        DisclosureField disclosureField2 = new DisclosureField(str, new FieldDescriptor((FieldDefinition) StandardFields.GetField(fieldId) ?? throw new ArgumentException("Unknown field ID " + fieldId)));
        this.disclosureFieldCache.Add((object) fieldId, (object) disclosureField2);
        return disclosureField2;
      }
    }
  }
}
