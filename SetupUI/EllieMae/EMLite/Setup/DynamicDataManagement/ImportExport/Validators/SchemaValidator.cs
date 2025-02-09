// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.DynamicDataManagement.ImportExport.Validators.SchemaValidator
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.Encompass.AsmResolver;
using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

#nullable disable
namespace EllieMae.EMLite.Setup.DynamicDataManagement.ImportExport.Validators
{
  public class SchemaValidator
  {
    public bool ValidateFeeFieldRules(BizRuleType bizRuleType, string packageData)
    {
      if (string.IsNullOrEmpty(packageData))
        return false;
      XDocument source = XDocument.Parse(packageData);
      string path1 = string.IsNullOrEmpty(AppDomain.CurrentDomain.RelativeSearchPath) ? AppDomain.CurrentDomain.BaseDirectory : AppDomain.CurrentDomain.RelativeSearchPath;
      string empty = string.Empty;
      string path = !AssemblyResolver.IsSmartClient ? Path.Combine(path1, "XMLSchema") : AssemblyResolver.GetResourceFileFolderPath("XMLSchema");
      bool validated = true;
      XmlSchemaSet schemas = new XmlSchemaSet();
      foreach (string file in Directory.GetFiles(path, "*.xsd"))
      {
        using (FileStream input = new FileStream(file, FileMode.Open, FileAccess.Read))
        {
          if (this.IsCommonXsdFile(file))
          {
            schemas.Add("", XmlReader.Create((Stream) input));
          }
          else
          {
            if (bizRuleType != BizRuleType.DDMFeeRules && bizRuleType != BizRuleType.DDMFieldRules)
              throw new ArgumentOutOfRangeException(nameof (bizRuleType), (object) bizRuleType, (string) null);
            if (this.GetFileName(file).Equals("DDMFeeAndFieldRule.xsd", StringComparison.OrdinalIgnoreCase))
              schemas.Add("", XmlReader.Create((Stream) input));
          }
        }
      }
      source.Validate(schemas, (ValidationEventHandler) ((o, e) => validated = false));
      return validated;
    }

    private string GetFileName(string filePath)
    {
      if (string.IsNullOrEmpty(filePath))
        return (string) null;
      string[] strArray = filePath.Split('\\');
      return strArray[strArray.Length - 1];
    }

    private bool IsCommonXsdFile(string filePath)
    {
      if (string.IsNullOrEmpty(filePath))
        return false;
      string[] strArray = filePath.Split('\\');
      string str = strArray[strArray.Length - 1];
      return str.Equals("Enum.xsd", StringComparison.OrdinalIgnoreCase) || str.Equals("CommonElements.xsd", StringComparison.OrdinalIgnoreCase) || str.Equals("BusinessRulesConditions.xsd", StringComparison.OrdinalIgnoreCase) || str.Equals("BusinessRuleBase.xsd", StringComparison.OrdinalIgnoreCase) || str.Equals("GlobalSettings.xsd", StringComparison.OrdinalIgnoreCase) || str.Equals("ScenarioBase.xsd", StringComparison.OrdinalIgnoreCase);
    }
  }
}
