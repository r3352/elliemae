// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Templates.IPostClosingConditionTemplate
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Templates
{
  /// <summary>
  /// Represents the interface for the PostClosingConditionTemplate object.
  /// </summary>
  /// <exclude />
  [Guid("63BA5FD7-4E35-48fa-A060-47512A201F6E")]
  public interface IPostClosingConditionTemplate
  {
    string ID { get; }

    string Title { get; }

    string Description { get; }

    ConditionDocuments Documents { get; }

    string Source { get; }

    string Recipient { get; }

    int DaysToReceive { get; }
  }
}
