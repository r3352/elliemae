// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.DynamicDataManagement.ImportExport.DDMFieldValueValidation
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

#nullable disable
namespace EllieMae.EMLite.Setup.DynamicDataManagement.ImportExport
{
  public class DDMFieldValueValidation
  {
    public string scenarioName;
    public string dependency;
    public string fieldID;
    public string fieldDescription;
    public bool isRequired;
    public bool validatedCorrectly;
    public string sourceXmlPortion;

    public DDMFieldValueValidation(
      string scenarioName,
      string dependency,
      string fieldID,
      string fieldDescription,
      bool isRequired,
      bool validatedCorrectly,
      string sourceXmlPortion)
    {
      this.scenarioName = scenarioName;
      this.dependency = dependency;
      this.fieldID = fieldID;
      this.fieldDescription = fieldDescription;
      this.isRequired = isRequired;
      this.validatedCorrectly = validatedCorrectly;
      this.sourceXmlPortion = sourceXmlPortion;
    }
  }
}
