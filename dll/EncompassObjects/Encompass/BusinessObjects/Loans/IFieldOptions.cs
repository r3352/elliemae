// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.IFieldOptions
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.Collections;
using System.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  [Guid("AD246DAE-B384-4c17-80F6-7E335CEC908F")]
  public interface IFieldOptions
  {
    bool RequireValueFromList { get; }

    int Count { get; }

    FieldOption this[int index] { get; }

    bool ContainsValue(string value);

    bool IsValueAllowed(string value);

    StringList GetValues();

    IEnumerator GetEnumerator();
  }
}
