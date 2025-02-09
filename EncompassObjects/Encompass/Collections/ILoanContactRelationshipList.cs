// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Collections.ILoanContactRelationshipList
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.Encompass.BusinessObjects.Loans;
using System.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.Collections
{
  /// <summary>Interface for LoanContactRelationshipList class.</summary>
  /// <exclude />
  [Guid("5CEFBEFF-F07E-4249-9CBB-C58E5109D30D")]
  public interface ILoanContactRelationshipList
  {
    LoanContactRelationship this[int index] { get; set; }

    int Count { get; }

    void Clear();

    void Add(LoanContactRelationship value);

    bool Contains(LoanContactRelationship value);

    int IndexOf(LoanContactRelationship value);

    void Insert(int index, LoanContactRelationship value);

    void Remove(LoanContactRelationship value);

    LoanContactRelationship[] ToArray();

    IEnumerator GetEnumerator();
  }
}
