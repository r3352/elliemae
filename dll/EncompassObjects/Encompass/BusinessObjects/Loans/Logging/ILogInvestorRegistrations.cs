// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.ILogInvestorRegistrations
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System;
using System.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  [Guid("E62F3F48-F381-4b3a-90E2-763732FF28BC")]
  public interface ILogInvestorRegistrations
  {
    int Count { get; }

    InvestorRegistration this[int index] { get; }

    InvestorRegistration Add(DateTime registrationDate);

    InvestorRegistration GetCurrent();

    IEnumerator GetEnumerator();
  }
}
