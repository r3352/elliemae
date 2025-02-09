// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Templates.IDocumentTemplate
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Templates
{
  /// <summary>
  /// Represents the interface for the DocumentTemplate object.
  /// </summary>
  /// <exclude />
  [Guid("F0A4852D-399A-4d8c-A39B-E37F9F90E6CD")]
  public interface IDocumentTemplate
  {
    string ID { get; }

    string Title { get; }

    bool IncludeInEDisclosurePackage { get; }

    bool IncludeInClosingPackage { get; }

    int DaysToReceive { get; }

    int DaysToExpire { get; }

    string Source { get; }

    DocumentTemplateType Type { get; }
  }
}
