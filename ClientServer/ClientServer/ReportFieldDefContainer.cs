// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.ReportFieldDefContainer
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public class ReportFieldDefContainer : IEnumerable<ReportFieldDef>, IEnumerable
  {
    protected List<ReportFieldDef> fieldDefs = new List<ReportFieldDef>();
    protected Dictionary<string, List<ReportFieldDef>> categorizedFields = new Dictionary<string, List<ReportFieldDef>>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
    protected Dictionary<string, ReportFieldDef> fieldIdLookup = new Dictionary<string, ReportFieldDef>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
    protected Dictionary<string, ReportFieldDef> dbnameLookup = new Dictionary<string, ReportFieldDef>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
    protected Dictionary<string, string> fieldId2DbnameLookup = new Dictionary<string, string>();

    public ReportFieldDefContainer()
    {
    }

    public ReportFieldDefContainer(ReportFieldDefContainer source)
    {
      this.fieldDefs = new List<ReportFieldDef>((IEnumerable<ReportFieldDef>) source.fieldDefs);
      this.categorizedFields = new Dictionary<string, List<ReportFieldDef>>((IDictionary<string, List<ReportFieldDef>>) source.categorizedFields);
      this.fieldIdLookup = new Dictionary<string, ReportFieldDef>((IDictionary<string, ReportFieldDef>) source.fieldIdLookup);
      this.dbnameLookup = new Dictionary<string, ReportFieldDef>((IDictionary<string, ReportFieldDef>) source.dbnameLookup);
    }

    public ReportFieldDefContainer(ReportFieldDefContainer source, HashSet<string> validFieldIds)
    {
      foreach (string validFieldId in validFieldIds)
      {
        ReportFieldDef fieldById = source.GetFieldByID(validFieldId);
        this.fieldDefs.Add(fieldById);
        this.fieldIdLookup[validFieldId] = fieldById;
        if (source.fieldId2DbnameLookup.ContainsKey(validFieldId))
        {
          this.fieldId2DbnameLookup[validFieldId] = source.fieldId2DbnameLookup[validFieldId];
          this.dbnameLookup[this.fieldId2DbnameLookup[validFieldId]] = fieldById;
        }
      }
      this.categorizedFields = new Dictionary<string, List<ReportFieldDef>>((IDictionary<string, List<ReportFieldDef>>) source.categorizedFields);
      foreach (string key in source.categorizedFields.Keys)
      {
        List<ReportFieldDef> reportFieldDefList = new List<ReportFieldDef>();
        foreach (ReportFieldDef reportFieldDef in this.categorizedFields[key])
        {
          if (validFieldIds.Contains(reportFieldDef.FieldID))
            reportFieldDefList.Add(reportFieldDef);
        }
        if (reportFieldDefList.Count < this.categorizedFields[key].Count)
          this.categorizedFields[key] = reportFieldDefList;
      }
    }

    public void Add(ReportFieldDef fieldDef)
    {
      if ((fieldDef.FieldID ?? "") == "")
        throw new Exception("Report field definition has missing FieldID");
      if (!this.categorizedFields.ContainsKey(fieldDef.Category))
        this.categorizedFields.Add(fieldDef.Category, new List<ReportFieldDef>());
      List<ReportFieldDef> categorizedField = this.categorizedFields[fieldDef.Category];
      bool flag = true;
      ReportFieldDef fieldById = this.GetFieldByID(fieldDef.FieldID);
      if (fieldById != null)
      {
        flag = false;
        if (string.IsNullOrEmpty(fieldById.CriterionFieldName) && !string.IsNullOrEmpty(fieldDef.CriterionFieldName))
        {
          this.fieldDefs.Remove(fieldById);
          categorizedField = this.categorizedFields[fieldById.Category];
          categorizedField.Remove(fieldById);
          fieldDef.Category = fieldById.Category;
          flag = true;
        }
        else if (fieldDef.CriterionFieldName.ToLower().StartsWith("fields.") && fieldById.CriterionFieldName.ToLower().StartsWith("loan."))
        {
          this.fieldDefs.Remove(fieldById);
          categorizedField = this.categorizedFields[fieldById.Category];
          categorizedField.Remove(fieldById);
          fieldDef.Category = fieldById.Category;
          fieldDef.Description = fieldById.Description;
          fieldDef.DisplayType = fieldById.DisplayType;
          fieldDef.Name = fieldById.Name;
          flag = true;
          this.fieldId2DbnameLookup[fieldDef.CriterionFieldName] = fieldById.CriterionFieldName;
        }
        else if (!string.IsNullOrEmpty(fieldById.CriterionFieldName))
          fieldById.Description = fieldDef.Description;
      }
      if (flag)
      {
        this.PopulateDynamicOptionList(fieldDef);
        categorizedField.Add(fieldDef);
        this.fieldDefs.Add(fieldDef);
        this.fieldIdLookup[fieldDef.FieldID] = fieldDef;
      }
      if (!(fieldDef.CriterionFieldName != ""))
        return;
      this.dbnameLookup[fieldDef.CriterionFieldName] = flag ? fieldDef : fieldById;
    }

    public void Remove(ReportFieldDef fieldDef)
    {
      this.fieldDefs.Remove(fieldDef);
      this.fieldIdLookup.Remove(fieldDef.FieldID);
      this.dbnameLookup.Remove(fieldDef.CriterionFieldName);
    }

    public ReportFieldDef this[int index] => this.fieldDefs[index];

    public ReportFieldDef GetFieldByID(string fieldId)
    {
      return this.fieldIdLookup.ContainsKey(fieldId) ? this.fieldIdLookup[fieldId] : (ReportFieldDef) null;
    }

    public ReportFieldDef GetDBFieldByID(ReportFieldDef fieldDef)
    {
      return this.fieldId2DbnameLookup.ContainsKey(fieldDef.CriterionFieldName) ? this.GetFieldByCriterionName(this.fieldId2DbnameLookup[fieldDef.CriterionFieldName]) : (ReportFieldDef) null;
    }

    public ReportFieldDef GetFieldByCriterionName(string dbname)
    {
      return this.dbnameLookup.ContainsKey(dbname) ? this.dbnameLookup[dbname] : (ReportFieldDef) null;
    }

    public IEnumerator<ReportFieldDef> GetEnumerator()
    {
      return (IEnumerator<ReportFieldDef>) this.fieldDefs.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable) this.fieldDefs).GetEnumerator();

    public IEnumerable<string> Categories => (IEnumerable<string>) this.categorizedFields.Keys;

    public bool ContainsCategory(string categoryName)
    {
      return this.categorizedFields.ContainsKey(categoryName);
    }

    public IEnumerable<ReportFieldDef> GetCategoryFields(string name)
    {
      return (IEnumerable<ReportFieldDef>) this.categorizedFields[name];
    }

    public int Count => this.fieldDefs.Count;

    public virtual void PopulateDynamicOptionList(ReportFieldDef fieldDef)
    {
    }
  }
}
