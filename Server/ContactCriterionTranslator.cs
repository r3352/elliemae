// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ContactCriterionTranslator
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataAccess;
using System.Collections;
using System.Collections.Specialized;

#nullable disable
namespace EllieMae.EMLite.Server
{
  internal class ContactCriterionTranslator : ICriterionTranslator
  {
    private const string customFieldPrefix = "custom.�";
    private DbTableInfo customFieldTable;
    private Hashtable fieldIdMap = CollectionsUtil.CreateCaseInsensitiveHashtable();
    private Hashtable fieldLabelToType = new Hashtable();

    public ContactCriterionTranslator(
      string customFieldTableName,
      ContactCustomFieldInfoCollection customFields)
    {
      this.customFieldTable = DbAccessManager.GetTable(customFieldTableName);
      for (int index = 0; index < customFields.Items.Length; ++index)
      {
        if (!this.fieldIdMap.ContainsKey((object) customFields.Items[index].Label))
        {
          this.fieldIdMap.Add((object) customFields.Items[index].Label, (object) customFields.Items[index].LabelID);
          this.fieldLabelToType.Add((object) customFields.Items[index].Label, (object) customFields.Items[index].FieldType);
        }
      }
    }

    public ICriterionNameFormatter CriterionNameFormatter { get; set; }

    public CriterionName TranslateName(string fieldName)
    {
      if (!fieldName.ToLower().StartsWith("custom."))
        return CriterionName.Parse(fieldName);
      string key = fieldName.Substring("custom.".Length);
      if (!this.fieldIdMap.ContainsKey((object) key))
        return (CriterionName) null;
      bool flag1 = false;
      bool flag2 = false;
      if (this.fieldLabelToType.ContainsKey((object) key))
      {
        if ((FieldFormat) this.fieldLabelToType[(object) key] == FieldFormat.X)
          flag1 = true;
        else if ((FieldFormat) this.fieldLabelToType[(object) key] == FieldFormat.MONTHDAY)
          flag2 = true;
      }
      int fieldId = (int) this.fieldIdMap[(object) key];
      return flag1 ? new CriterionName((string) null, "isnull((select FieldValue from " + this.customFieldTable.Name + " where ContactID = Contact.ContactID and FieldID = " + (object) fieldId + "), '')") : (flag2 ? new CriterionName((string) null, "convert(datetime, '2004/' + (select FieldValue from " + this.customFieldTable.Name + " where ContactID = Contact.ContactID and FieldID = " + (object) fieldId + " and FieldValue <> ''))") : new CriterionName((string) null, "(select FieldValue from " + this.customFieldTable.Name + " where ContactID = Contact.ContactID and FieldID = " + (object) fieldId + ")"));
    }

    public QueryCriterion TranslateCriterion(QueryCriterion cri) => cri;
  }
}
