// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.DefaultPrivacyPolicyTemplateControl
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class DefaultPrivacyPolicyTemplateControl(SetUpContainer setupContainer) : 
    DefaultTemplateControl(setupContainer, (FormDataBase) new LoanDefaultPrivacyPolicy(Session.SessionObjects))
  {
    public override string Title => "Privacy Policy";

    protected override InputFormInfo GetInputFormInfo()
    {
      return new InputFormInfo("PRIVACYPOLICY", "Privacy Policy");
    }

    public override void Reset()
    {
      ((LoanDefaultPrivacyPolicy) this.TemplateData).Reset();
      base.Reset();
    }

    public override void Save()
    {
      LoanDefaultPrivacyPolicy templateData = (LoanDefaultPrivacyPolicy) this.TemplateData;
      if (string.Compare(templateData.GetField("NOTICES.X83"), "We share personal information with nonaffiliated third parties", true) == 0 && templateData.GetField("NOTICES.X84") == string.Empty)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please enter non-affiliated third parties.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (string.Compare(templateData.GetField("NOTICES.X85"), "We share personal information with joint marketing partners", true) == 0 && templateData.GetField("NOTICES.X86") == string.Empty)
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "Please enter joint marketing partners.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        templateData.CommitChanges(Session.SessionObjects);
        base.Save();
      }
    }
  }
}
