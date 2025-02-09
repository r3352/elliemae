// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Templates.ITemplateEntry
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Templates
{
  [Guid("59C19A47-D9AA-406a-B2CE-26E5FC5FF196")]
  public interface ITemplateEntry
  {
    string Name { get; }

    string Path { get; }

    string DomainPath { get; }

    TemplateEntryType EntryType { get; }

    bool IsPublic { get; }

    DateTime LastModified { get; }

    TemplateEntryProperties Properties { get; }

    TemplateEntry ParentFolder { get; }

    string Owner { get; }

    string ToString();

    string UserQualifiedPath { get; }
  }
}
