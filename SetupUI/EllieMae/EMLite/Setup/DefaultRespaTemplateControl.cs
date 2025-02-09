// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.DefaultRespaTemplateControl
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class DefaultRespaTemplateControl(SetUpContainer setupContainer) : DefaultTemplateControl(setupContainer, (FormDataBase) new LoanDefaultRESPA(Session.SessionObjects))
  {
    public override string Title => "RESPA";

    protected override InputFormInfo GetInputFormInfo()
    {
      return new InputFormInfo("SERVICINGDISCLOSURE", "Servicing Disclosure");
    }

    public override void Reset()
    {
      ((LoanDefaultRESPA) this.TemplateData).Reset();
      base.Reset();
    }

    public override void Save()
    {
      ((LoanDefaultRESPA) this.TemplateData).CommitChanges(Session.SessionObjects);
      base.Save();
    }
  }
}
