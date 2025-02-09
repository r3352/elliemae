// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.CustomFields.CustomFieldsDefinition
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.BizLayer;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.CustomFields;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.ContactUI.CustomFields
{
  [Serializable]
  public class CustomFieldsDefinition : BusinessBase, IDisposable
  {
    private CustomFieldsDefinitionInfo customFieldsDefinitionInfo;
    [NotUndoable]
    private CustomFieldDefinitionCollection customFieldDefinitions = CustomFieldDefinitionCollection.NewCustomFieldDefinitionCollection();
    [NotUndoable]
    private SessionObjects sessionObjects;

    public CustomFieldsType CustomFieldsType => this.customFieldsDefinitionInfo.CustomFieldsType;

    public int RecordId => this.customFieldsDefinitionInfo.RecordId;

    public CustomFieldDefinitionCollection CustomFieldDefinitions => this.customFieldDefinitions;

    public bool PossibleDataLoss() => this.customFieldDefinitions.PossibleDataLoss();

    internal CustomFieldsDefinitionInfo GetDirtyInfo()
    {
      CustomFieldsDefinitionInfo dirtyInfo = new CustomFieldsDefinitionInfo();
      dirtyInfo.CustomFieldsType = this.CustomFieldsType;
      dirtyInfo.RecordId = this.RecordId;
      dirtyInfo.IsNew = this.IsNew;
      dirtyInfo.IsDirty = this.IsDirty;
      dirtyInfo.IsDeleted = this.IsDeleted;
      CustomFieldDefinitionCollection dirtyMembers = this.customFieldDefinitions.GetDirtyMembers();
      dirtyInfo.CustomFieldDefinitions = new CustomFieldDefinitionInfo[dirtyMembers.Count];
      int num = 0;
      foreach (CustomFieldDefinition customFieldDefinition in (CollectionBase) dirtyMembers)
        dirtyInfo.CustomFieldDefinitions[num++] = customFieldDefinition.GetDirtyInfo();
      return dirtyInfo;
    }

    internal void SetInfo(
      CustomFieldsDefinitionInfo customFieldsDefinitionInfo)
    {
      if (customFieldsDefinitionInfo == null)
        return;
      this.customFieldsDefinitionInfo = customFieldsDefinitionInfo;
      this.customFieldDefinitions = CustomFieldDefinitionCollection.NewCustomFieldDefinitionCollection();
      if (customFieldsDefinitionInfo.CustomFieldDefinitions == null)
        return;
      foreach (CustomFieldDefinitionInfo customFieldDefinition in customFieldsDefinitionInfo.CustomFieldDefinitions)
        this.customFieldDefinitions.Add(CustomFieldDefinition.NewCustomFieldDefinition(customFieldDefinition));
    }

    public override string ToString()
    {
      return string.Format("CustomFields[{0}:{1}]", (object) this.CustomFieldsType, (object) this.RecordId);
    }

    public bool Equals(CustomFieldsDefinition customFieldsDefinition)
    {
      return this.CustomFieldsType.Equals((object) customFieldsDefinition.CustomFieldsType) && this.RecordId.Equals(customFieldsDefinition.RecordId);
    }

    public new static bool Equals(object objA, object objB)
    {
      return objA is CustomFieldsDefinition && objB is CustomFieldsDefinition && ((CustomFieldsDefinition) objA).Equals((CustomFieldsDefinition) objB);
    }

    public override bool Equals(object obj)
    {
      return obj is CustomFieldsDefinition && this.Equals((CustomFieldsDefinition) obj);
    }

    public override int GetHashCode()
    {
      return string.Format("{0}-{1}", (object) this.CustomFieldsType, (object) this.RecordId).GetHashCode();
    }

    public override bool IsValid => base.IsValid && this.customFieldDefinitions.IsValid;

    public override bool IsDirty => base.IsDirty || this.customFieldDefinitions.IsDirty;

    public override BusinessBase Save()
    {
      if (this.IsNew && this.customFieldDefinitions.Count == 0)
        return (BusinessBase) this;
      if (this.IsDeleted)
      {
        int num = this.IsNew ? 1 : 0;
        this.MarkNew();
      }
      else if (this.IsDirty)
      {
        this.SetInfo(this.sessionObjects.ContactManager.UpdateCustomFieldsDefinition(this.GetDirtyInfo()));
        this.MarkOld();
      }
      return (BusinessBase) this;
    }

    public override string GetBrokenRulesString()
    {
      foreach (BusinessBase customFieldDefinition in (CollectionBase) this.customFieldDefinitions)
      {
        string brokenRulesString = customFieldDefinition.GetBrokenRulesString();
        if (string.Empty != brokenRulesString)
          return brokenRulesString;
      }
      return string.Empty;
    }

    public static CustomFieldsDefinition NewCustomFieldsDefinition(
      SessionObjects sessionObjects,
      CustomFieldsType customFieldsType,
      int recordId)
    {
      return new CustomFieldsDefinition(sessionObjects, customFieldsType, recordId);
    }

    public static CustomFieldsDefinition NewCustomFieldsDefinition(
      SessionObjects sessionObjects,
      CustomFieldsDefinitionInfo customFieldsDefinitionInfo)
    {
      return new CustomFieldsDefinition(sessionObjects, customFieldsDefinitionInfo);
    }

    public static CustomFieldsDefinition GetCustomFieldsDefinition(
      SessionObjects sessionObjects,
      CustomFieldsType customFieldsType,
      int recordId)
    {
      CustomFieldsDefinitionInfo fieldsDefinition = sessionObjects.ContactManager.GetCustomFieldsDefinition(customFieldsType, recordId);
      return fieldsDefinition == null ? new CustomFieldsDefinition(sessionObjects, customFieldsType, recordId) : new CustomFieldsDefinition(sessionObjects, fieldsDefinition);
    }

    private CustomFieldsDefinition(
      SessionObjects sessionObjects,
      CustomFieldsType customFieldsType,
      int recordId)
    {
      this.MarkNew();
      this.sessionObjects = sessionObjects;
      this.customFieldsDefinitionInfo = new CustomFieldsDefinitionInfo(customFieldsType, recordId, (CustomFieldDefinitionInfo[]) null);
    }

    private CustomFieldsDefinition(
      SessionObjects sessionObjects,
      CustomFieldsDefinitionInfo customFieldsDefinitionInfo)
    {
      this.MarkOld();
      this.sessionObjects = sessionObjects;
      this.SetInfo(customFieldsDefinitionInfo);
    }

    public void Dispose()
    {
    }
  }
}
