// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Templates.PostClosingConditionTemplate
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.Encompass.Client;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Templates
{
  public class PostClosingConditionTemplate : ConditionTemplate, IPostClosingConditionTemplate
  {
    private PostClosingConditionTemplate pcTemplate;

    internal PostClosingConditionTemplate(Session session, PostClosingConditionTemplate template)
      : base(session, (ConditionTemplate) template)
    {
      this.pcTemplate = template;
    }

    public string Source => this.pcTemplate.Source;

    public string Recipient => this.pcTemplate.Recipient;

    public int DaysToReceive => ((ConditionTemplate) this.pcTemplate).DaysTillDue;
  }
}
