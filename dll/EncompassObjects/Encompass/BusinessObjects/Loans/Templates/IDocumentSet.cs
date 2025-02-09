// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Templates.IDocumentSet
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.BusinessEnums;
using EllieMae.Encompass.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Templates
{
  [Guid("D6D831EF-CF1F-473c-9FC6-3967AD4883CE")]
  public interface IDocumentSet
  {
    string Name { get; }

    string Path { get; }

    TemplateType TemplateType { get; }

    string Description { get; }

    DocumentTemplateList GetDocumentsForMilestone(Milestone ms);

    DocumentTemplateList GetAllDocuments();
  }
}
