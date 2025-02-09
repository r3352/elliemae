// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Templates.IUnderwritingConditionTemplate
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Templates
{
  /// <summary>
  /// Represents the interface for the UnderwritingConditionTemplate object.
  /// </summary>
  /// <exclude />
  [Guid("CB0809D5-4F55-46f4-B8FA-CECA2C37F17C")]
  public interface IUnderwritingConditionTemplate
  {
    string ID { get; }

    string Title { get; }

    string Description { get; }

    ConditionDocuments Documents { get; }

    bool ForInternalUse { get; }

    bool ForExternalUse { get; }

    string Category { get; }

    string PriorTo { get; }

    Role ForRole { get; }

    bool AllowToClear { get; }
  }
}
