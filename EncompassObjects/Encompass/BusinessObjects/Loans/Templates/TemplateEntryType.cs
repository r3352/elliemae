// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Templates.TemplateEntryType
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Templates
{
  /// <summary>
  /// Defines the different types of nodes within the template file structure.
  /// </summary>
  [Flags]
  [Guid("459B271E-F71D-462f-BB0D-2DD52992B0D0")]
  public enum TemplateEntryType
  {
    /// <summary>No template type information</summary>
    None = 0,
    /// <summary>Object represents a folder in the template hierarchy</summary>
    Folder = 1,
    /// <summary>Object represents a template</summary>
    Template = 2,
    /// <summary>Used to specify an action on both folders and templates</summary>
    All = Template | Folder, // 0x00000003
  }
}
