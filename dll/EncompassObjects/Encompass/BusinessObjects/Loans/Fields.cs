// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Fields
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  public abstract class Fields : IFields
  {
    private Hashtable fields = new Hashtable((IEqualityComparer) StringComparer.CurrentCultureIgnoreCase);

    internal Fields()
    {
    }

    public Field this[string fieldId]
    {
      get
      {
        Field field1 = (Field) this.fields[(object) (fieldId ?? "")];
        if (field1 != null)
          return field1;
        Field field2 = this.CreateField(fieldId);
        if (field2 == null)
          throw new ArgumentException("The value '" + fieldId + "' is not a valid Field ID in this context.", nameof (fieldId));
        this.fields.Add((object) fieldId, (object) field2);
        return field2;
      }
    }

    internal abstract Field CreateField(string fieldId);
  }
}
