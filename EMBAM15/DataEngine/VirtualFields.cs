// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.VirtualFields
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public static class VirtualFields
  {
    public static FieldDefinitionCollection All = new FieldDefinitionCollection();
    public static IDictionary<VirtualFieldType, FieldDefinitionCollection> VirtualFieldDictionary = (IDictionary<VirtualFieldType, FieldDefinitionCollection>) new Dictionary<VirtualFieldType, FieldDefinitionCollection>();

    static VirtualFields()
    {
      FieldDefinitionCollection[] definitionCollectionArray = new FieldDefinitionCollection[26]
      {
        BorrowerFields.All,
        CoMortgagerFields.All,
        ComplianceTestFields.All,
        DocumentTrackingFields.All,
        LoanAssociateFields.All,
        MilestoneFields.All,
        PostClosingConditionFields.All,
        RateLockFields.All,
        UnderwritingConditionFields.All,
        PreliminaryConditionFields.All,
        MilestoneTaskFields.All,
        InterimServicingFields.All,
        LastDisclosedGFEFields.All,
        LastDisclosedLEFields.All,
        LastDisclosedCDFields.All,
        LastDisclosedTILFields.All,
        EDisclosureTrackingFields.All,
        EDisclosure2015TrackingFields.All,
        AlertFields.All,
        DocEngineFieldList.All,
        MilestoneTemplateFields.All,
        AUSTrackingFields.All,
        BidTapePricingFields.PricingFields.All,
        EnhancedConditionFields.All,
        EnhancedConditionField.All,
        EnhancedConditionFieldByOption.All
      };
      foreach (FieldDefinitionCollection definitionCollection in definitionCollectionArray)
      {
        foreach (VirtualField field in definitionCollection)
        {
          VirtualFields.All.Add((FieldDefinition) field);
          if (!VirtualFields.VirtualFieldDictionary.ContainsKey(field.VirtualFieldType))
            VirtualFields.VirtualFieldDictionary.Add(field.VirtualFieldType, new FieldDefinitionCollection());
          VirtualFields.VirtualFieldDictionary[field.VirtualFieldType].Add((FieldDefinition) field);
        }
      }
    }

    public static void Initialize()
    {
    }

    public static VirtualField GetField(string fieldId) => VirtualFields.GetField(fieldId, true);

    public static VirtualField GetField(string fieldId, bool resolveInstance)
    {
      FieldDefinition field = VirtualFields.All[fieldId];
      if (field != null)
        return (VirtualField) field;
      if (!resolveInstance)
        return (VirtualField) null;
      string instanceParentId = VirtualField.GetMultiInstanceParentID(fieldId);
      if (instanceParentId == null)
        return (VirtualField) null;
      FieldDefinition fieldDefinition = VirtualFields.All[instanceParentId];
      return fieldDefinition == null ? (VirtualField) null : (VirtualField) fieldDefinition.CreateInstanceWithID(fieldId);
    }

    public static bool Contains(string fieldId) => VirtualFields.Contains(fieldId, true);

    public static bool Contains(string fieldId, bool resolveInstances)
    {
      return VirtualFields.GetField(fieldId, resolveInstances) != null;
    }

    private static void addField(VirtualField field)
    {
      if (VirtualFields.Contains(field.FieldID))
        return;
      VirtualFields.All.Add((FieldDefinition) field);
    }
  }
}
