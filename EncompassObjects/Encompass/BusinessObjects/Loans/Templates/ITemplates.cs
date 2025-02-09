// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Templates.ITemplates
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.Encompass.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Templates
{
  /// <summary>Interface for the Templates class.</summary>
  /// <exclude />
  [Guid("3BAF4DCE-FB1F-4f38-A47F-580D73AB1420")]
  public interface ITemplates
  {
    Template GetTemplate(TemplateType templateType, string path);

    TemplateEntryList GetTemplateFolderContents(
      TemplateType templateType,
      TemplateEntry folderEntry);

    DocumentTemplates Documents { get; }

    UnderwritingConditionTemplates UnderwritingConditions { get; }

    PostClosingConditionTemplates PostClosingConditions { get; }

    TaskTemplates Tasks { get; }
  }
}
