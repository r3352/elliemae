// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Settings.SettingsService
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer;
using EllieMae.Encompass.Client;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Settings
{
  /// <summary>SettingsService</summary>
  public class SettingsService : SessionBoundObject, ISettingsService
  {
    internal SettingsService(Session session)
      : base(session)
    {
    }

    /// <summary>Service to return EPPS Loan Program from Settings</summary>
    /// <returns></returns>
    public List<EppsLoanProgram> GetEppsLoanProgram()
    {
      this.ValidateEppsLoanProgram();
      return ((IConfigurationManager) this.Session.GetObject("ConfigurationManager")).GetEPPSLoanProgramsSettings().Select<EPPSLoanProgram, EppsLoanProgram>((Func<EPPSLoanProgram, EppsLoanProgram>) (p => new EppsLoanProgram(p.ProgramID, p.ProgramName))).ToList<EppsLoanProgram>();
    }

    private void ValidateEppsLoanProgram()
    {
      ProductPricingSetting productPricingPartner = this.Session.SessionObjects.StartupInfo.ProductPricingPartner;
      if (productPricingPartner == null || !productPricingPartner.IsEPPS)
        throw new Exception("Access to the ICE PPE Loan Programs table is only allowed if ICE PPE is the selected Product and Pricing supplier.");
    }
  }
}
