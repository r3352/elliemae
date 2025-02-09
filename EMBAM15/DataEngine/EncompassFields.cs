// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.EncompassFields
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public static class EncompassFields
  {
    private const string className = "EncompassFields";
    private static readonly string sw = Tracing.SwDataEngine;

    public static FieldDefinition GetField(string fieldId)
    {
      return EncompassFields.GetField(fieldId, true);
    }

    public static FieldDefinition GetField(string fieldId, bool resolveInstances)
    {
      return EncompassFields.GetField(fieldId, (FieldSettings) null, resolveInstances);
    }

    public static FieldDefinition GetField(string fieldId, FieldSettings fieldConfig)
    {
      return EncompassFields.GetField(fieldId, fieldConfig, true);
    }

    public static FieldDefinition GetField(
      string fieldId,
      FieldSettings fieldConfig,
      bool resolveInstance)
    {
      if (fieldConfig != null)
      {
        CustomFieldInfo field1 = fieldConfig.CustomFields.GetField(fieldId);
        if (field1 != null)
          return (FieldDefinition) new CustomField(field1);
        if (LockRequestCustomField.IsLockRequestCustomField(fieldId))
        {
          string idForCustomField = LockRequestCustomField.GetBaseFieldIDForCustomField(fieldId);
          if (fieldConfig.LockRequestAdditionalFields.IsAdditionalField(idForCustomField, true))
          {
            FieldDefinition field2 = EncompassFields.GetField(idForCustomField, fieldConfig, true);
            if (field2 != null)
              return (FieldDefinition) new LockRequestCustomField(field2);
          }
        }
        if (RateLockField.IsRateLockField(fieldId) && !RateLockFields.All.Contains(fieldId))
        {
          FieldDefinition field3 = EncompassFields.GetField(RateLockField.GetBaseFieldIDForRateLockField(fieldId), fieldConfig, true);
          if (field3 != null)
            return (FieldDefinition) new RateLockField(fieldId, field3);
        }
      }
      return (FieldDefinition) StandardFields.GetField(fieldId, resolveInstance) ?? (FieldDefinition) VirtualFields.GetField(fieldId, resolveInstance);
    }

    public static FieldDefinitionCollection GetAllUserDefinedFieldDefinitions(
      FieldSettings fieldSettings)
    {
      FieldDefinitionCollection fieldDefinitions = new FieldDefinitionCollection();
      foreach (FieldDefinition customFieldDefinition in EncompassFields.GetCustomFieldDefinitions(fieldSettings))
        fieldDefinitions.Add(customFieldDefinition);
      foreach (FieldDefinition lockRequestField in EncompassFields.GetUserDefinedLockRequestFields(fieldSettings))
        fieldDefinitions.Add(lockRequestField);
      return fieldDefinitions;
    }

    public static FieldDefinitionCollection GetCustomFieldDefinitions(FieldSettings fieldSettings)
    {
      FieldDefinitionCollection fieldDefinitions = new FieldDefinitionCollection();
      foreach (CustomFieldInfo customField in fieldSettings.CustomFields)
        fieldDefinitions.Add((FieldDefinition) new CustomField(customField));
      return fieldDefinitions;
    }

    public static FieldDefinitionCollection GetAllLockRequestFields(FieldSettings fieldSettings)
    {
      FieldDefinitionCollection lockRequestFields = new FieldDefinitionCollection();
      foreach (FieldDefinition field in RateLockFields.All)
        lockRequestFields.Add(field);
      foreach (FieldDefinition lockRequestField in EncompassFields.GetUserDefinedLockRequestFields(fieldSettings))
      {
        if (!lockRequestFields.Contains(lockRequestField.FieldID))
          lockRequestFields.Add(lockRequestField);
      }
      return lockRequestFields;
    }

    public static FieldDefinitionCollection GetUserDefinedLockRequestFields(
      FieldSettings fieldSettings)
    {
      return EncompassFields.GetUserDefinedLockRequestFields(fieldSettings, true, true);
    }

    public static FieldDefinitionCollection GetUserDefinedLockRequestFields(
      FieldSettings fieldSettings,
      bool includeRequestFields,
      bool includeSnapshotFields)
    {
      FieldDefinitionCollection lockRequestFields = new FieldDefinitionCollection();
      foreach (string field1 in fieldSettings.LockRequestAdditionalFields.GetFields(true))
      {
        FieldDefinition field2 = EncompassFields.GetField(LockRequestCustomField.GenerateCustomFieldID(field1), fieldSettings, true);
        if (field2 != null)
        {
          if (includeRequestFields)
            lockRequestFields.Add(field2);
          if (includeSnapshotFields)
          {
            lockRequestFields.Add((FieldDefinition) new RateLockField(field2));
            lockRequestFields.Add((FieldDefinition) new RateLockField(field2, RateLockField.RateLockOrder.Previous));
            lockRequestFields.Add((FieldDefinition) new RateLockField(field2, RateLockField.RateLockOrder.Previous2));
            lockRequestFields.Add((FieldDefinition) new RateLockField(field2, RateLockField.RateLockOrder.MostRecentRequest));
          }
        }
      }
      if (includeSnapshotFields)
      {
        foreach (string field3 in fieldSettings.LockRequestAdditionalFields.GetFields(false))
        {
          FieldDefinition field4 = EncompassFields.GetField(field3, fieldSettings, true);
          if (field4 != null)
          {
            lockRequestFields.Add((FieldDefinition) new RateLockField(field4));
            lockRequestFields.Add((FieldDefinition) new RateLockField(field4, RateLockField.RateLockOrder.Previous));
            lockRequestFields.Add((FieldDefinition) new RateLockField(field4, RateLockField.RateLockOrder.Previous2));
            lockRequestFields.Add((FieldDefinition) new RateLockField(field4, RateLockField.RateLockOrder.MostRecentRequest));
          }
        }
      }
      return lockRequestFields;
    }

    public static bool IsNumeric(string fieldId)
    {
      return EncompassFields.IsNumeric(fieldId, (FieldSettings) null);
    }

    public static bool IsNumeric(string fieldId, FieldSettings fieldConfig)
    {
      FieldDefinition field = EncompassFields.GetField(fieldId, fieldConfig);
      return field != null && field.IsNumeric();
    }

    public static FieldFormat GetFormat(string fieldId)
    {
      return EncompassFields.GetFormat(fieldId, (FieldSettings) null);
    }

    public static FieldFormat GetFormat(string fieldId, FieldSettings fieldConfig)
    {
      FieldDefinition field = EncompassFields.GetField(fieldId, fieldConfig);
      return field == null ? FieldFormat.STRING : field.Format;
    }

    public static string GetDescription(string fieldId)
    {
      return EncompassFields.GetDescription(fieldId, (FieldSettings) null);
    }

    public static string GetDescription(string fieldId, FieldSettings fieldConfig)
    {
      FieldDefinition field = EncompassFields.GetField(fieldId, fieldConfig);
      if (field != null)
        return field.Description;
      if (fieldId.ToLower().StartsWith("button_"))
        return "Button";
      return fieldId.ToLower().StartsWith("lockbutton_") ? "Lock Button" : "";
    }

    public static bool IsReportable(string fieldId)
    {
      return EncompassFields.IsReportable(fieldId, (FieldSettings) null);
    }

    public static bool IsReportable(string fieldId, FieldSettings fieldConfig)
    {
      FieldDefinition field = EncompassFields.GetField(fieldId, fieldConfig);
      if (field != null)
        return field.AllowInReportingDatabase;
      return fieldId.ToLower().StartsWith("button_");
    }

    public static bool IsUserDefinedField(string fieldId)
    {
      return CustomFieldInfo.IsCustomFieldID(fieldId) || LockRequestCustomField.IsLockRequestCustomField(fieldId);
    }
  }
}
