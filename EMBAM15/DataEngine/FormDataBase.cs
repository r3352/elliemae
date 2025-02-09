// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.FormDataBase
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  [Serializable]
  public class FormDataBase : BinaryConvertibleObject, IHtmlInput, IEnumerable
  {
    private const string className = "FormDataBase";
    private static string sw = Tracing.SwDataEngine;
    private XmlStringTable fieldData = new XmlStringTable();
    private XmlStringTable lockedFields = new XmlStringTable();
    private XmlStringTable fieldDatawithValues = new XmlStringTable();
    public const string DDM_DT_VERSION = "18.2.0";
    public const string DDM_DT_VERSION_KEY = "ddmdtversion";
    public const char DT_DELIMITER = '|';
    public const string DT_DELIMITER_1 = "|1";
    public const string DT_DELIMITER_0 = "|0";
    [NonSerialized]
    private Hashtable dirtyFields = new Hashtable();
    [NonSerialized]
    private bool dirty;

    protected FormDataBase()
    {
    }

    protected FormDataBase(XmlSerializationInfo info)
    {
      if (this.GetTemplateType() == FormDataBase.TemplateType.DataTemplate)
        this.GetUserModified(info.GetValue<XmlStringTable>("tbl"), info);
      else
        this.fieldData = info.GetValue<XmlStringTable>("tbl");
      this.lockedFields = info.GetValue<XmlStringTable>("lock", (XmlStringTable) null);
      if (this.lockedFields != null)
        return;
      this.lockedFields = new XmlStringTable();
    }

    public virtual FormDataBase.TemplateType GetTemplateType() => FormDataBase.TemplateType.Unknown;

    private void GetUserModified(XmlStringTable collection, XmlSerializationInfo info)
    {
      if (this.GetDataTemplateVersion(info) == "18.2.0")
      {
        foreach (KeyValuePair<string, object> keyValuePair in (Dictionary<string, object>) collection)
        {
          if (keyValuePair.Value.ToString().EndsWith("|0") || keyValuePair.Value.ToString().EndsWith("|1"))
          {
            string[] strArray = keyValuePair.Value.ToString().Split('|');
            if (strArray[1] == "1")
              this.fieldDatawithValues.Add(keyValuePair.Key, (object) strArray[0]);
            this.fieldData.Add(keyValuePair.Key, (object) strArray[0]);
          }
          else
            this.fieldData.Add(keyValuePair.Key, keyValuePair.Value);
        }
      }
      else
      {
        foreach (KeyValuePair<string, object> keyValuePair in (Dictionary<string, object>) collection)
          this.fieldData.Add(keyValuePair.Key, keyValuePair.Value);
      }
    }

    protected FormDataBase(DataRow[] arrDr)
    {
      XmlStringTable xmlStringTable = new XmlStringTable();
      foreach (DataRow dataRow in arrDr)
        xmlStringTable.Add(Convert.ToString(dataRow["FieldKey"]), (object) "Y");
      this.fieldData = xmlStringTable;
      this.lockedFields = new XmlStringTable();
    }

    protected XmlStringTable FieldData => this.fieldData;

    public XmlStringTable FieldDataWithValues
    {
      get
      {
        return this.GetDataTemplateVersion((XmlSerializationInfo) null) == "18.2.0" ? this.fieldDatawithValues : this.fieldData;
      }
    }

    public string[] GetFieldIDsWithValue()
    {
      List<string> stringList = new List<string>();
      if (this.GetDataTemplateVersion((XmlSerializationInfo) null) == "18.2.0")
        stringList.AddRange((IEnumerable<string>) this.fieldDatawithValues.Keys);
      else
        stringList.AddRange((IEnumerable<string>) this.fieldData.Keys);
      return stringList.ToArray();
    }

    public string[] GetLockedFieldIDs()
    {
      return new List<string>((IEnumerable<string>) this.lockedFields.Keys).ToArray();
    }

    public string[] GetAssignedFieldIDs()
    {
      return new List<string>((IEnumerable<string>) this.FieldData.Keys).ToArray();
    }

    public virtual string[] GetAllowedFieldIDs()
    {
      throw new NotSupportedException("The specified object does not provide a fixed list of allowed fields");
    }

    public string FormatValue(string val, FieldFormat format)
    {
      return Utils.ApplyFieldFormatting(val, format);
    }

    public FieldFormat GetFormat(string id)
    {
      FieldDefinition fieldDefinition = this.GetFieldDefinition(id);
      return fieldDefinition == null ? FieldFormat.NONE : fieldDefinition.Format;
    }

    public virtual FieldDefinition GetFieldDefinition(string id)
    {
      return (FieldDefinition) StandardFields.GetField(id, true);
    }

    public virtual string GetField(string id)
    {
      return this.FormatValue(this.GetSimpleField(id), this.GetFormat(id));
    }

    public virtual string GetSimpleField(string id) => string.Concat(this.fieldData[id]);

    public virtual void SetField(string id, string val)
    {
      this.fieldData[id] = (object) val;
      if (this.dirtyFields == null)
        this.dirtyFields = new Hashtable();
      this.dirtyFields[(object) id] = (object) true;
    }

    public void RemoveField(string id)
    {
      if (!this.fieldData.ContainsKey(id))
        return;
      this.fieldData.Remove(id);
    }

    public virtual void SetCurrentField(string id, string val) => this.SetField(id, val);

    public bool IsLocked(string id) => this.lockedFields.ContainsKey(id);

    public virtual void RemoveLock(string id) => this.lockedFields.Remove(id);

    public virtual void AddLock(string id) => this.lockedFields[id] = (object) true;

    public void AddLocks(string[] fieldIds)
    {
      foreach (string fieldId in fieldIds)
        this.AddLock(fieldId);
    }

    public void AddLockValue(string id, string val)
    {
      if (!this.lockedFields.ContainsKey(id))
        return;
      this.lockedFields[id] = (object) val;
    }

    public virtual string GetOrgField(string id)
    {
      return this.IsLocked(id) && this.lockedFields[id] != null ? (string) this.lockedFields[id] : this.GetField(id);
    }

    public void ClearDirtyTable() => this.dirtyFields = new Hashtable();

    public bool IsDirty() => this.dirty || this.dirtyFields != null && this.dirtyFields.Count > 0;

    public bool IsDirty(string id)
    {
      return this.dirtyFields != null && this.dirtyFields.ContainsKey((object) id);
    }

    protected virtual void MarkAsDirty() => this.dirty = true;

    public void MarkAsClean()
    {
      this.dirty = false;
      this.ClearDirtyTable();
    }

    public void CleanField(string id) => this.dirtyFields.Remove((object) id);

    public void ClearAllFields()
    {
      this.fieldData.Clear();
      this.lockedFields.Clear();
      this.dirtyFields = new Hashtable();
    }

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      if (this.GetTemplateType() == FormDataBase.TemplateType.DataTemplate)
        this.UpdateUserModified();
      info.AddValue("tbl", (object) this.fieldData);
      info.AddValue("lock", (object) this.lockedFields);
    }

    public virtual string GetDataTemplateVersion(XmlSerializationInfo info) => "0.0.0";

    private void UpdateUserModified()
    {
      XmlStringTable xmlStringTable = new XmlStringTable();
      foreach (KeyValuePair<string, object> keyValuePair in (Dictionary<string, object>) this.fieldData)
      {
        if (!keyValuePair.Value.ToString().EndsWith("|0") && !keyValuePair.Value.ToString().EndsWith("|1"))
          xmlStringTable.Add(keyValuePair.Key, (object) (keyValuePair.Value.ToString() + "|0"));
        else
          xmlStringTable.Add(keyValuePair.Key, keyValuePair.Value);
      }
      this.fieldData = xmlStringTable;
      foreach (KeyValuePair<string, object> fieldDatawithValue in (Dictionary<string, object>) this.fieldDatawithValues)
      {
        if (this.fieldData.ContainsKey(fieldDatawithValue.Key))
        {
          string str = this.fieldData[fieldDatawithValue.Key].ToString();
          if (str.EndsWith("|0") || str.EndsWith("|1"))
          {
            string[] strArray = str.Split('|');
            if (strArray[1] != "1")
              this.fieldData[fieldDatawithValue.Key] = (object) (strArray[0] + "|1");
          }
          else
            this.fieldData[fieldDatawithValue.Key] = (object) (this.fieldData[fieldDatawithValue.Key].ToString() + "|1");
        }
      }
    }

    public IEnumerator GetEnumerator() => (IEnumerator) this.fieldData.Keys.GetEnumerator();

    public void SetField(string id, string val, bool isUserModified) => this.SetField(id, val);

    public enum TemplateType
    {
      Unknown,
      DataTemplate,
      ClosingTemplate,
    }
  }
}
