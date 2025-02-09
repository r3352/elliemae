// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.FHAConsumerTemplate
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.LoanUtils.DataEngine;
using EllieMae.EMLite.RemotingServices;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class FHAConsumerTemplate(SetUpContainer setupContainer) : DefaultTemplateControl(setupContainer, (FormDataBase) new LoanDefaultFHAConsumerChoice(Session.SessionObjects))
  {
    public override string Title => "FHA Informed Consumer Choice Disclosure";

    protected override InputFormInfo GetInputFormInfo()
    {
      return new InputFormInfo("FHAInformedConsumerChoiceDisclosure", "FHA Informed Consumer Choice Disclosure");
    }

    public override void Reset()
    {
      ((LoanDefaultFHAConsumerChoice) this.TemplateData).Reset();
      base.Reset();
    }

    public override void Save()
    {
      ((LoanDefaultFHAConsumerChoice) this.TemplateData).CommitChanges(Session.SessionObjects);
      base.Save();
    }
  }
}
