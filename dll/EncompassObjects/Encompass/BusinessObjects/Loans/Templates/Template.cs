// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Templates.Template
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.Common;
using EllieMae.Encompass.Client;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Templates
{
  public abstract class Template : SessionBoundObject, ITemplate
  {
    private TemplateEntry tEntry;

    protected internal Template(Session session, TemplateEntry tEntry)
      : base(session)
    {
      this.tEntry = tEntry;
    }

    public string Name => this.tEntry.Name;

    public string Path => this.tEntry.Path;

    public TemplateEntry TemplateEntry => this.tEntry;

    public abstract TemplateType TemplateType { get; }

    public abstract string Description { get; }

    internal abstract object Unwrap();

    internal FileSystemEntry FileSystemEntry => this.tEntry.Unwrap();
  }
}
