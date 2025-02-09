// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Templates.PostClosingConditionTemplate
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.Encompass.Client;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Templates
{
  /// <summary>
  /// Represents a configured condition template which can be used to create a
  /// <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.PostClosingCondition" />.
  /// </summary>
  public class PostClosingConditionTemplate : ConditionTemplate, IPostClosingConditionTemplate
  {
    private EllieMae.EMLite.DataEngine.eFolder.PostClosingConditionTemplate pcTemplate;

    internal PostClosingConditionTemplate(Session session, EllieMae.EMLite.DataEngine.eFolder.PostClosingConditionTemplate template)
      : base(session, (EllieMae.EMLite.DataEngine.eFolder.ConditionTemplate) template)
    {
      this.pcTemplate = template;
    }

    /// <summary>Gets the source for the template.</summary>
    public string Source => this.pcTemplate.Source;

    /// <summary>Gets the intended recipient of the condition.</summary>
    public string Recipient => this.pcTemplate.Recipient;

    /// <summary>
    /// Returns the number of days for the condition to be received once ordered.
    /// </summary>
    public int DaysToReceive => this.pcTemplate.DaysTillDue;
  }
}
