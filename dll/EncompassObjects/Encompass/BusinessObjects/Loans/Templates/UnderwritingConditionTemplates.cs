// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Templates.UnderwritingConditionTemplates
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.Encompass.Client;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Templates
{
  public class UnderwritingConditionTemplates : 
    SessionBoundObject,
    IUnderwritingConditionTemplates,
    IEnumerable
  {
    private List<UnderwritingConditionTemplate> templates;

    internal UnderwritingConditionTemplates(Session session)
      : base(session)
    {
      this.Refresh();
    }

    public int Count => this.templates.Count;

    public UnderwritingConditionTemplate this[int index] => this.templates[index];

    public UnderwritingConditionTemplate GetTemplateByID(string templateId)
    {
      foreach (UnderwritingConditionTemplate templateById in this)
      {
        if (templateById.ID == templateId)
          return templateById;
      }
      return (UnderwritingConditionTemplate) null;
    }

    public UnderwritingConditionTemplate GetTemplateByTitle(string title)
    {
      foreach (UnderwritingConditionTemplate templateByTitle in this)
      {
        if (string.Compare(templateByTitle.Title, title, true) == 0)
          return templateByTitle;
      }
      return (UnderwritingConditionTemplate) null;
    }

    public void Refresh()
    {
      lock (this)
      {
        ConditionTrackingSetup conditionTrackingSetup = this.Session.SessionObjects.ConfigurationManager.GetConditionTrackingSetup((ConditionType) 1);
        this.templates = new List<UnderwritingConditionTemplate>();
        foreach (UnderwritingConditionTemplate template in (CollectionBase) conditionTrackingSetup)
          this.templates.Add(new UnderwritingConditionTemplate(this.Session, template));
      }
    }

    public IEnumerator GetEnumerator() => (IEnumerator) this.templates.GetEnumerator();
  }
}
