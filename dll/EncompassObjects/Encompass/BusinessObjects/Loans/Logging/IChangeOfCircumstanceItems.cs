// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.IChangeOfCircumstanceItems
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System.Collections.Generic;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  [Guid("DE8C3050-001A-4CF2-BB37-DA120BB37F1C")]
  public interface IChangeOfCircumstanceItems
  {
    List<COCCollection> COCCollections { get; }

    string ChangedCircumstances { get; }

    string FeeLevelIndicator { get; }
  }
}
