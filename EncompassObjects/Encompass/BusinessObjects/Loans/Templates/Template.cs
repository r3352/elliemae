// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Templates.Template
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.Common;
using EllieMae.Encompass.Client;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Templates
{
  /// <summary>
  /// Provides the base class for all template objects in Encompass.
  /// </summary>
  public abstract class Template : SessionBoundObject, ITemplate
  {
    private TemplateEntry tEntry;

    /// <summary>Constructor</summary>
    /// <param name="session"></param>
    /// <param name="tEntry"></param>
    protected internal Template(Session session, TemplateEntry tEntry)
      : base(session)
    {
      this.tEntry = tEntry;
    }

    /// <summary>Gets the name of the template</summary>
    public string Name => this.tEntry.Name;

    /// <summary>Gets the path of the template.</summary>
    /// <remarks>The path will have the form "public:\folder\templatename" or
    /// "personal:\folder\templatename".</remarks>
    public string Path => this.tEntry.Path;

    /// <summary>
    /// Gets the <see cref="P:EllieMae.Encompass.BusinessObjects.Loans.Templates.Template.TemplateEntry" /> for the Template.
    /// </summary>
    public TemplateEntry TemplateEntry => this.tEntry;

    /// <summary>Gets the type of template represented by the object.</summary>
    public abstract TemplateType TemplateType { get; }

    /// <summary>Gets the description of the Template object.</summary>
    public abstract string Description { get; }

    /// <summary>
    /// Unwraps the template object to return the internal data type.
    /// </summary>
    /// <returns></returns>
    /// <exclude />
    internal abstract object Unwrap();

    /// <summary>
    /// Provides access to the low-level FileSystemEntry for the template.
    /// </summary>
    /// <remarks>This property is meant to be used internally by Encompass only.</remarks>
    internal FileSystemEntry FileSystemEntry => this.tEntry.Unwrap();
  }
}
