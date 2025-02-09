// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Templates.PostClosingConditionTemplates
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
  public class PostClosingConditionTemplates : 
    SessionBoundObject,
    IPostClosingConditionTemplates,
    IEnumerable
  {
    private List<PostClosingConditionTemplate> templates;

    internal PostClosingConditionTemplates(Session session)
      : base(session)
    {
      this.Refresh();
    }

    public int Count => this.templates.Count;

    public PostClosingConditionTemplate this[int index] => this.templates[index];

    public PostClosingConditionTemplate GetTemplateByID(string templateId)
    {
      foreach (PostClosingConditionTemplate templateById in this)
      {
        if (templateById.ID == templateId)
          return templateById;
      }
      return (PostClosingConditionTemplate) null;
    }

    public PostClosingConditionTemplate GetTemplateByTitle(string title)
    {
      foreach (PostClosingConditionTemplate templateByTitle in this)
      {
        if (string.Compare(templateByTitle.Title, title, true) == 0)
          return templateByTitle;
      }
      return (PostClosingConditionTemplate) null;
    }

    public void Refresh()
    {
      lock (this)
      {
        ConditionTrackingSetup conditionTrackingSetup = this.Session.SessionObjects.ConfigurationManager.GetConditionTrackingSetup((ConditionType) 2);
        this.templates = new List<PostClosingConditionTemplate>();
        foreach (PostClosingConditionTemplate template in (CollectionBase) conditionTrackingSetup)
          this.templates.Add(new PostClosingConditionTemplate(this.Session, template));
      }
    }

    public IEnumerator GetEnumerator() => (IEnumerator) this.templates.GetEnumerator();
  }
}
