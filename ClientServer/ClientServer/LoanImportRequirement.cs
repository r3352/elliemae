// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.LoanImportRequirement
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class LoanImportRequirement
  {
    private static string NoTemplateRequired = "No template required";
    private static string UserTemplateRequired = "Template required (user will select at import)";
    private static string ThisTemplateRequired = "This template is required";
    private LoanImportRequirement.LoanImportRequirementType fannieMaeImportRequirementType;
    private string templateForFannieMaeImport = string.Empty;
    private LoanImportRequirement.LoanImportRequirementType webCenterImportRequirementType;
    private string templateForWebCenterImport = string.Empty;

    public LoanImportRequirement()
    {
    }

    public LoanImportRequirement(
      LoanImportRequirement.LoanImportRequirementType fannieMaeImportRequirementType,
      string templateForFannieMaeImport,
      LoanImportRequirement.LoanImportRequirementType webCenterImportRequirementType,
      string templateForWebCenterImport)
    {
      this.fannieMaeImportRequirementType = fannieMaeImportRequirementType;
      this.templateForFannieMaeImport = templateForFannieMaeImport;
      this.webCenterImportRequirementType = webCenterImportRequirementType;
      this.templateForWebCenterImport = templateForWebCenterImport;
    }

    public LoanImportRequirement.LoanImportRequirementType FannieMaeImportRequirementType
    {
      get => this.fannieMaeImportRequirementType;
      set => this.fannieMaeImportRequirementType = value;
    }

    public string TemplateForFannieMaeImport
    {
      get => this.templateForFannieMaeImport;
      set => this.templateForFannieMaeImport = value;
    }

    public LoanImportRequirement.LoanImportRequirementType WebCenterImportRequirementType
    {
      get => this.webCenterImportRequirementType;
      set => this.webCenterImportRequirementType = value;
    }

    public string TemplateForWebCenterImport
    {
      get => this.templateForWebCenterImport;
      set => this.templateForWebCenterImport = value;
    }

    public string FannieMaeImportRequirementTypeToString
    {
      get
      {
        switch (this.fannieMaeImportRequirementType)
        {
          case LoanImportRequirement.LoanImportRequirementType.TemplateIsRequiredByUser:
            return LoanImportRequirement.UserTemplateRequired;
          case LoanImportRequirement.LoanImportRequirementType.TemplateIsRequiredByCompany:
            return LoanImportRequirement.ThisTemplateRequired;
          default:
            return LoanImportRequirement.NoTemplateRequired;
        }
      }
    }

    public void FannieMaeImportRequirementTypeToEnum(string requirementTypeString)
    {
      if (string.Compare(requirementTypeString, LoanImportRequirement.UserTemplateRequired, true) == 0)
        this.fannieMaeImportRequirementType = LoanImportRequirement.LoanImportRequirementType.TemplateIsRequiredByUser;
      else if (string.Compare(requirementTypeString, LoanImportRequirement.ThisTemplateRequired, true) == 0)
        this.fannieMaeImportRequirementType = LoanImportRequirement.LoanImportRequirementType.TemplateIsRequiredByCompany;
      else
        this.fannieMaeImportRequirementType = LoanImportRequirement.LoanImportRequirementType.TemplateIsNotRequired;
    }

    public string WebCenterImportRequirementTypeToString
    {
      get
      {
        switch (this.webCenterImportRequirementType)
        {
          case LoanImportRequirement.LoanImportRequirementType.TemplateIsRequiredByUser:
            return LoanImportRequirement.UserTemplateRequired;
          case LoanImportRequirement.LoanImportRequirementType.TemplateIsRequiredByCompany:
            return LoanImportRequirement.ThisTemplateRequired;
          default:
            return LoanImportRequirement.NoTemplateRequired;
        }
      }
    }

    public void WebCenterImportRequirementTypeToEnum(string requirementTypeString)
    {
      if (string.Compare(requirementTypeString, LoanImportRequirement.UserTemplateRequired, true) == 0)
        this.webCenterImportRequirementType = LoanImportRequirement.LoanImportRequirementType.TemplateIsRequiredByUser;
      else if (string.Compare(requirementTypeString, LoanImportRequirement.ThisTemplateRequired, true) == 0)
        this.webCenterImportRequirementType = LoanImportRequirement.LoanImportRequirementType.TemplateIsRequiredByCompany;
      else
        this.webCenterImportRequirementType = LoanImportRequirement.LoanImportRequirementType.TemplateIsNotRequired;
    }

    public static string[] GetImportOptions()
    {
      return new string[3]
      {
        LoanImportRequirement.NoTemplateRequired,
        LoanImportRequirement.UserTemplateRequired,
        LoanImportRequirement.ThisTemplateRequired
      };
    }

    public enum LoanImportRequirementType
    {
      TemplateIsNotRequired,
      TemplateIsRequiredByUser,
      TemplateIsRequiredByCompany,
    }
  }
}
