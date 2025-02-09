// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.DynamicDataManagement.ImportExport.DDMRuleValidator
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Setup.DynamicDataManagement.ImportExport
{
  public class DDMRuleValidator
  {
    private SessionObjects sessionObjects;
    private BizRuleType ddmRuleType;
    private List<DDMFieldValueValidation> fieldValidationList = new List<DDMFieldValueValidation>();
    private List<string> errorFragmentsForExceptionLog = new List<string>();
    private IImportErrorProvider _dummyErrorProvider = (IImportErrorProvider) new DummyImportErrorProvider();
    private XmlDocument parsedXml;
    private ExternalEntities exEntities;
    private Dictionary<string, string> refIdToStatusMap;
    private Dictionary<string, string> refIdToDependencyMap;
    private Dictionary<string, string> refIdToTypeMap;
    private FieldSettings fieldSettings;

    public DDMRuleValidator(
      SessionObjects sessionObjects,
      BizRuleType ddmRuleType,
      XmlDocument parsedXml,
      ExternalEntities exEntities)
    {
      this.sessionObjects = sessionObjects;
      this.ddmRuleType = ddmRuleType;
      this.fieldSettings = Session.LoanManager.GetFieldSettings();
      this.parsedXml = parsedXml;
      this.exEntities = exEntities;
    }

    public DDMRuleValidationResult mapExEntitiesToUI()
    {
      this.validateFields(this.exEntities);
      return new DDMRuleValidationResult(this.fieldValidationList, this.errorFragmentsForExceptionLog.Count == 0, string.Join("\"\r\n\r\n=========================================================================================================================\r\n\r\n\"", (IEnumerable<string>) this.errorFragmentsForExceptionLog));
    }

    private void validateFields(ExternalEntities externalEntities)
    {
      this.refIdToStatusMap = externalEntities.externalEntities.ToDictionary<XrefEntity, string, string>((Func<XrefEntity, string>) (item => item.XRefId), (Func<XrefEntity, string>) (item => item.IsResolved));
      this.refIdToDependencyMap = externalEntities.externalEntities.ToDictionary<XrefEntity, string, string>((Func<XrefEntity, string>) (item => item.XRefId), (Func<XrefEntity, string>) (item => item.IsHardDependency));
      this.refIdToTypeMap = externalEntities.externalEntities.ToDictionary<XrefEntity, string, string>((Func<XrefEntity, string>) (item => item.XRefId), (Func<XrefEntity, string>) (item => item.EntityType));
      foreach (XmlNode selectNode1 in this.parsedXml.SelectNodes("//GlobalSettings"))
      {
        string name = "Global rule Settings";
        foreach (XmlNode selectNode2 in selectNode1.SelectNodes(".//AdvancedCodeDependencies"))
          this.addToValidationList(selectNode2, name, "");
        foreach (XmlNode selectNode3 in selectNode1.SelectNodes(".//FieldValue"))
        {
          string valueType = selectNode3.Attributes.GetNamedItem("fieldValueType").Value;
          this.addToValidationList(selectNode3, name, valueType);
        }
      }
      foreach (XmlNode selectNode4 in this.parsedXml.SelectNodes("//Scenario"))
      {
        string name = selectNode4.Attributes.GetNamedItem("Name").Value;
        foreach (XmlNode selectNode5 in selectNode4.SelectNodes(".//AdvancedCodeDependencies"))
          this.addToValidationList(selectNode5, name, "");
        foreach (XmlNode selectNode6 in selectNode4.SelectNodes(".//EffectiveDateInfo"))
          this.addToValidationList(selectNode6, name, "");
        foreach (XmlNode selectNode7 in selectNode4.SelectNodes(".//Condition"))
          this.addToValidationList(selectNode7, name, "");
        foreach (XmlNode selectNode8 in selectNode4.SelectNodes(".//FieldValue"))
        {
          string valueType = selectNode8.Attributes.GetNamedItem("fieldValueType").Value;
          this.addToValidationList(selectNode8, name, valueType);
        }
      }
    }

    private void addToValidationList(XmlNode node, string name, string valueType)
    {
      try
      {
        List<XmlAttributeCollection> attributeCollectionList = new List<XmlAttributeCollection>();
        foreach (XmlNode selectNode in node.SelectNodes("AffectedField/XRef | AffectedMilestone/XRef | AffectedRole/XRef | AffectedDataTable/XRef | AffectedSystemTable/XRef"))
          attributeCollectionList.Add(selectNode.Attributes);
        foreach (XmlAttributeCollection attributeCollection in attributeCollectionList)
        {
          string key = attributeCollection.GetNamedItem("RefID").Value;
          string fieldID = attributeCollection.GetNamedItem("EntityID").Value;
          string fieldDescription = attributeCollection.GetNamedItem("EntityUID").Value;
          if (this.refIdToDependencyMap.ContainsKey(key))
          {
            string refIdToType = this.refIdToTypeMap[key];
            bool isRequired = this.refIdToDependencyMap[key].ToLower() == "true";
            bool validatedCorrectly = this.refIdToStatusMap[key].ToLower() == "true";
            if (validatedCorrectly && (valueType == DDMRuleValidator.fieldValueType.ValueNotSet.ToString() || valueType == DDMRuleValidator.fieldValueType.ClearValueInLoanFile.ToString() || valueType == DDMRuleValidator.fieldValueType.UseCalculatedValue.ToString()))
              break;
            string sourceXmlPortion = "";
            if (!validatedCorrectly)
            {
              sourceXmlPortion = node.OuterXml;
              this.errorFragmentsForExceptionLog.Add(sourceXmlPortion);
            }
            this.fieldValidationList.Add(new DDMFieldValueValidation(name, refIdToType, fieldID, fieldDescription, isRequired, validatedCorrectly, sourceXmlPortion));
          }
        }
      }
      catch (Exception ex)
      {
      }
    }

    public enum fieldValueType
    {
      ValueNotSet,
      SpecificValue,
      Calculation,
      UseCalculatedValue,
      ClearValueInLoanFile,
      Table,
      SystemTable,
    }
  }
}
