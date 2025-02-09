// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.eFolder.EnhancedConditionSetAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using Elli.Domain.EnhancedCondition;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.DataEngine.eFolder;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.eFolder
{
  public class EnhancedConditionSetAccessor
  {
    public static void UpdateConditionSets(EnhancedConditionSet condset)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("IF NOT EXISTS(SELECT 1 FROM EnhancedConditionSets WHERE Id = '" + (object) condset.Id + "') ");
      dbQueryBuilder.AppendLine("BEGIN ");
      dbQueryBuilder.AppendLine(" INSERT INTO EnhancedConditionSets(Id, SetName, [Description], [CreatedDate], [CreatedBy], [LastModifiedBy], [LastModifiedDate]) ");
      dbQueryBuilder.AppendLine(" VALUES('" + (object) condset.Id + "', '" + condset.SetName + "', '" + condset.Description + "', getdate(), '" + condset.CreatedBy + "', '" + condset.LastModifiedBy + "', getdate())");
      if (condset.ConditionTemplates != null)
      {
        foreach (EllieMae.EMLite.DataEngine.eFolder.EnhancedConditionTemplate conditionTemplate in condset.ConditionTemplates)
        {
          dbQueryBuilder.AppendLine(" INSERT INTO EnhancedConditionSetTemplates(Id, SetId, TemplateId, CreatedDate, CreatedBy) ");
          dbQueryBuilder.AppendLine(" VALUES('" + (object) Guid.NewGuid() + "', '" + (object) condset.Id + "', '" + (object) conditionTemplate.Id + "', getdate(), '" + condset.CreatedBy + "')");
        }
      }
      dbQueryBuilder.AppendLine(" END ");
      dbQueryBuilder.AppendLine(" ELSE ");
      dbQueryBuilder.AppendLine(" BEGIN ");
      if (condset.Deleted)
      {
        dbQueryBuilder.AppendLine(" DELETE FROM EnhancedConditionSetTemplates ");
        dbQueryBuilder.AppendLine(" WHERE SetId = '" + (object) condset.Id + "' ");
        dbQueryBuilder.AppendLine(" DELETE FROM EnhancedConditionSets ");
        dbQueryBuilder.AppendLine(" WHERE Id = '" + (object) condset.Id + "' ");
      }
      else
      {
        dbQueryBuilder.AppendLine(" UPDATE EnhancedConditionSets ");
        dbQueryBuilder.AppendLine(" SET SetName='" + condset.SetName + "', [Description]='" + condset.Description + "', LastModifiedBy ='" + condset.LastModifiedBy + "', LastModifiedDate= getdate() ");
        dbQueryBuilder.AppendLine(" WHERE Id = '" + (object) condset.Id + "' ");
      }
      if (!condset.Deleted)
      {
        if (condset.ConditionTemplates != null)
        {
          dbQueryBuilder.AppendLine(" DECLARE @tmpids AS TABLE (templateid uniqueidentifier) ");
          dbQueryBuilder.AppendLine(" INSERT INTO @tmpids (templateid) select templateid from EnhancedConditionSetTemplates ");
          dbQueryBuilder.AppendLine(" where setid='" + (object) condset.Id + "' ");
          foreach (EllieMae.EMLite.DataEngine.eFolder.EnhancedConditionTemplate conditionTemplate in condset.ConditionTemplates)
          {
            dbQueryBuilder.AppendLine("IF NOT EXISTS(SELECT 1 FROM @tmpids WHERE templateid = '" + (object) conditionTemplate.Id + "') ");
            dbQueryBuilder.AppendLine(" BEGIN ");
            dbQueryBuilder.AppendLine(" INSERT INTO EnhancedConditionSetTemplates(Id, SetId, TemplateId, CreatedBy, CreatedDate) ");
            dbQueryBuilder.AppendLine(" VALUES('" + (object) Guid.NewGuid() + "', '" + (object) condset.Id + "', '" + (object) conditionTemplate.Id + "', '" + condset.LastModifiedBy + "', getdate()) ");
            dbQueryBuilder.AppendLine(" END");
            dbQueryBuilder.AppendLine(" DELETE FROM @tmpids WHERE templateid='" + (object) conditionTemplate.Id + "' ");
          }
          dbQueryBuilder.AppendLine(" DELETE FROM EnhancedConditionSetTemplates");
          dbQueryBuilder.AppendLine(" WHERE setid='" + (object) condset.Id + "' and templateid in(select templateid from @tmpids)");
        }
        else
        {
          dbQueryBuilder.AppendLine(" DELETE FROM EnhancedConditionSetTemplates ");
          dbQueryBuilder.AppendLine(" WHERE SetId = '" + (object) condset.Id + "' ");
        }
      }
      dbQueryBuilder.AppendLine(" END ");
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static List<EnhancedConditionSet> GetConditionSets()
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("select id, setname, description ");
      dbQueryBuilder.AppendLine(" from enhancedconditionsets ");
      List<EnhancedConditionSet> conditionSets = new List<EnhancedConditionSet>();
      DataTable dataTable = dbQueryBuilder.ExecuteTableQuery();
      try
      {
        conditionSets = dataTable.Rows.Cast<DataRow>().Select<DataRow, EnhancedConditionSet>((System.Func<DataRow, EnhancedConditionSet>) (dr => new EnhancedConditionSet()
        {
          Id = Guid.Parse(dr["id"].ToString()),
          SetName = dr["setname"].ToString(),
          Description = dr["description"].ToString()
        })).ToList<EnhancedConditionSet>();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (EnhancedConditionSetAccessor), ex);
      }
      return conditionSets;
    }

    public static EnhancedConditionSet GetEnhancedConditionSetDetail(
      Guid setid,
      bool activeTemplatesOnly,
      bool includeDetails,
      List<string> conditionTypes = null,
      bool activeTypesOnly = false)
    {
      EnhancedConditionSet conditionSetDetail = (EnhancedConditionSet) null;
      try
      {
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        dbQueryBuilder.AppendLine("select cs.id as setid, cs.setname as setname, cs.Description as setdesc");
        dbQueryBuilder.AppendLine("from EnhancedConditionSets cs ");
        dbQueryBuilder.AppendLine(string.Format("where cs.id = {0}", (object) SQL.EncodeString(setid.ToString())));
        if (includeDetails)
          dbQueryBuilder.AppendLine("select ct.Data ");
        else
          dbQueryBuilder.AppendLine("select ct.id as templateid, ct.title as templatetitle, ct.conditionType as templateconditiontype, ct.active as templateactive, ec.Active as typeactive ");
        dbQueryBuilder.AppendLine(" from EnhancedConditionSetTemplates st ");
        dbQueryBuilder.AppendLine(" inner join EnhancedConditionTemplates ct on st.TemplateId = ct.id ");
        dbQueryBuilder.AppendLine(" inner join EnhancedConditionTypes ec on ec.Title=ct.ConditionType");
        dbQueryBuilder.AppendLine(string.Format("where st.setid = {0}", (object) SQL.Encode((object) setid.ToString())));
        if (activeTemplatesOnly)
          dbQueryBuilder.AppendLine(" and ct.active = 1 ");
        if (activeTypesOnly)
          dbQueryBuilder.AppendLine(" and  ec.active = 1 ");
        if (conditionTypes != null && conditionTypes.Any<string>())
          dbQueryBuilder.AppendLine(" and ct.conditionType in (" + string.Join(",", conditionTypes.Select<string, string>((System.Func<string, string>) (x => string.Format("'{0}'", (object) x)))) + ")");
        DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
        if (dataSet.Tables.Count > 0)
        {
          DataTable table1 = dataSet.Tables[0];
          if (table1.Rows != null && table1.Rows.Count > 0)
          {
            conditionSetDetail = new EnhancedConditionSet();
            DataRow row = table1.Rows[0];
            conditionSetDetail.Id = Guid.Parse(row[nameof (setid)].ToString());
            conditionSetDetail.SetName = row["setname"].ToString();
            conditionSetDetail.Description = row["setdesc"].ToString();
          }
          DataTable table2 = dataSet.Tables[1];
          List<EllieMae.EMLite.DataEngine.eFolder.EnhancedConditionTemplate> conditionTemplateList = new List<EllieMae.EMLite.DataEngine.eFolder.EnhancedConditionTemplate>();
          if (table2 != null)
          {
            if (table2.Rows.Count > 0)
            {
              foreach (DataRow row in (InternalDataCollectionBase) table2.Rows)
              {
                EllieMae.EMLite.DataEngine.eFolder.EnhancedConditionTemplate conditionTemplate = new EllieMae.EMLite.DataEngine.eFolder.EnhancedConditionTemplate();
                if (includeDetails)
                {
                  conditionTemplate = EnhancedConditionSetAccessor.MapTemplate(row["data"].ToString());
                }
                else
                {
                  conditionTemplate.Id = new Guid?(Guid.Parse(row["templateid"].ToString()));
                  conditionTemplate.Title = row["templatetitle"].ToString();
                  conditionTemplate.ConditionType = row["templateconditiontype"].ToString();
                  conditionTemplate.Active = new bool?(SQL.DecodeBoolean(row["templateactive"]));
                }
                conditionTemplateList.Add(conditionTemplate);
              }
              conditionSetDetail.ConditionTemplates = conditionTemplateList;
            }
          }
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (EnhancedConditionSetAccessor), ex);
      }
      return conditionSetDetail;
    }

    public static bool IsUniqueSetName(string setname, Guid id)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("select id ");
      dbQueryBuilder.AppendLine(" from EnhancedConditionSets ");
      dbQueryBuilder.AppendLine(" where id != '" + (object) id + "' and SetName = '" + setname + "' ");
      DataTable dataTable = dbQueryBuilder.ExecuteTableQuery();
      return dataTable == null || dataTable.Rows.Count <= 0;
    }

    public static EllieMae.EMLite.DataEngine.eFolder.EnhancedConditionTemplate MapTemplate(
      string serializedObject)
    {
      Elli.Domain.EnhancedCondition.EnhancedConditionTemplate conditionTemplate1 = JsonConvert.DeserializeObject<Elli.Domain.EnhancedCondition.EnhancedConditionTemplate>(serializedObject);
      EllieMae.EMLite.DataEngine.eFolder.EnhancedConditionTemplate conditionTemplate2 = new EllieMae.EMLite.DataEngine.eFolder.EnhancedConditionTemplate();
      if (conditionTemplate1 != null)
      {
        conditionTemplate2.ConditionType = conditionTemplate1.ConditionType;
        conditionTemplate2.Active = new bool?(conditionTemplate1.Active);
        conditionTemplate2.InternalId = conditionTemplate1.InternalId;
        conditionTemplate2.ExternalId = conditionTemplate1.ExternalId;
        conditionTemplate2.AllowDuplicate = new bool?(conditionTemplate1.AllowDuplicate);
        conditionTemplate2.Title = conditionTemplate1.Title;
        conditionTemplate2.InternalDescription = conditionTemplate1.InternalDescription;
        conditionTemplate2.ExternalDescription = conditionTemplate1.ExternalDescription;
        List<EntityReferenceContract> referenceContractList = new List<EntityReferenceContract>();
        if (conditionTemplate1.AssignedTo != null)
        {
          foreach (string str in conditionTemplate1.AssignedTo)
            referenceContractList.Add(new EntityReferenceContract()
            {
              entityId = str
            });
        }
        conditionTemplate2.AssignedTo = referenceContractList;
        conditionTemplate2.Category = conditionTemplate1.Category;
        conditionTemplate2.Source = conditionTemplate1.Source;
        conditionTemplate2.Recipient = conditionTemplate1.Recipient;
        conditionTemplate2.PriorTo = conditionTemplate1.PriorTo;
        EllieMae.EMLite.DataEngine.eFolder.EnhancedConditionTemplate conditionTemplate3 = conditionTemplate2;
        DateTime? nullable = conditionTemplate1.StartDate;
        string str1 = nullable.ToString();
        conditionTemplate3.StartDate = str1;
        EllieMae.EMLite.DataEngine.eFolder.EnhancedConditionTemplate conditionTemplate4 = conditionTemplate2;
        nullable = conditionTemplate1.EndDate;
        string str2 = nullable.ToString();
        conditionTemplate4.EndDate = str2;
        conditionTemplate2.DaysToReceive = conditionTemplate1.DaysToReceive;
        int? owner = conditionTemplate1.Owner;
        if (owner.HasValue)
        {
          EllieMae.EMLite.DataEngine.eFolder.EnhancedConditionTemplate conditionTemplate5 = conditionTemplate2;
          EntityReferenceContract referenceContract = new EntityReferenceContract();
          owner = conditionTemplate1.Owner;
          referenceContract.entityId = owner.ToString();
          conditionTemplate5.Owner = referenceContract;
        }
        List<string> stringList = new List<string>();
        if (conditionTemplate1.PrintDefinitions != null)
        {
          foreach (PrintDefinition printDefinition in conditionTemplate1.PrintDefinitions)
          {
            string str3 = printDefinition.ToString();
            stringList.Add(str3);
          }
        }
        conditionTemplate2.PrintDefinitions = stringList;
        ConnectSettingsContract settingsContract = new ConnectSettingsContract()
        {
          DocumentOption = conditionTemplate1.ConnectSettings.DocumentOption.ToString(),
          DocumentTemplate = new EntityReferenceContract()
        };
        settingsContract.DocumentTemplate.entityId = conditionTemplate1.ConnectSettings.DocumentTemplate == null ? (string) null : conditionTemplate1.ConnectSettings.DocumentTemplate.RefId;
        conditionTemplate2.ConnectSettings = settingsContract;
        conditionTemplate2.CustomizeTypeDefinition = new bool?(conditionTemplate1.CustomizeTypeDefinition);
        if (conditionTemplate1.CustomDefinitions != null)
        {
          ConditionDefinitionContract definitionContract1 = new ConditionDefinitionContract();
          if (conditionTemplate1.CustomDefinitions.CategoryDefinitions != null)
          {
            OptionDefinitionContract[] definitionContractArray = new OptionDefinitionContract[conditionTemplate1.CustomDefinitions.CategoryDefinitions.Count];
            for (int index = 0; index < conditionTemplate1.CustomDefinitions.CategoryDefinitions.Count; ++index)
              definitionContractArray[index] = new OptionDefinitionContract()
              {
                name = index.ToString()
              };
            definitionContract1.categoryDefinitions = definitionContractArray;
          }
          if (conditionTemplate1.CustomDefinitions.PriorToDefinitions != null)
          {
            OptionDefinitionContract[] definitionContractArray = new OptionDefinitionContract[conditionTemplate1.CustomDefinitions.PriorToDefinitions.Count];
            for (int index = 0; index < conditionTemplate1.CustomDefinitions.PriorToDefinitions.Count; ++index)
              definitionContractArray[index] = new OptionDefinitionContract()
              {
                name = index.ToString()
              };
            definitionContract1.priorToDefinitions = definitionContractArray;
          }
          if (conditionTemplate1.CustomDefinitions.RecipientDefinitions != null)
          {
            OptionDefinitionContract[] definitionContractArray = new OptionDefinitionContract[conditionTemplate1.CustomDefinitions.RecipientDefinitions.Count];
            for (int index = 0; index < conditionTemplate1.CustomDefinitions.RecipientDefinitions.Count; ++index)
              definitionContractArray[index] = new OptionDefinitionContract()
              {
                name = index.ToString()
              };
            definitionContract1.recipientDefinitions = definitionContractArray;
          }
          if (conditionTemplate1.CustomDefinitions.SourceDefinitions != null)
          {
            OptionDefinitionContract[] definitionContractArray = new OptionDefinitionContract[conditionTemplate1.CustomDefinitions.SourceDefinitions.Count];
            for (int index = 0; index < conditionTemplate1.CustomDefinitions.SourceDefinitions.Count; ++index)
              definitionContractArray[index] = new OptionDefinitionContract()
              {
                name = index.ToString()
              };
            definitionContract1.sourceDefinitions = definitionContractArray;
          }
          if (conditionTemplate1.CustomDefinitions.TrackingDefinitions != null)
          {
            TrackingDefinitionContract[] definitionContractArray = new TrackingDefinitionContract[conditionTemplate1.CustomDefinitions.TrackingDefinitions.Count];
            for (int index1 = 0; index1 < conditionTemplate1.CustomDefinitions.TrackingDefinitions.Count; ++index1)
            {
              TrackingDefinitionContract definitionContract2 = new TrackingDefinitionContract();
              definitionContract2.name = conditionTemplate1.CustomDefinitions.TrackingDefinitions[index1].Name;
              definitionContract2.open = conditionTemplate1.CustomDefinitions.TrackingDefinitions[index1].Open;
              if (conditionTemplate1.CustomDefinitions.TrackingDefinitions[index1].Roles != null)
              {
                EntityReferenceContract[] referenceContractArray = new EntityReferenceContract[conditionTemplate1.CustomDefinitions.TrackingDefinitions[index1].Roles.Count];
                for (int index2 = 0; index2 < conditionTemplate1.CustomDefinitions.TrackingDefinitions[index1].Roles.Count; ++index2)
                  referenceContractArray[index2] = new EntityReferenceContract()
                  {
                    entityId = conditionTemplate1.CustomDefinitions.TrackingDefinitions[index1].Roles[index2]
                  };
                definitionContract2.roles = referenceContractArray;
              }
              definitionContractArray[index1] = definitionContract2;
            }
            definitionContract1.trackingDefinitions = definitionContractArray;
          }
          conditionTemplate2.customDefinitions = definitionContract1;
        }
        conditionTemplate2.Id = new Guid?(conditionTemplate1.Id);
        conditionTemplate2.CreatedBy = new EntityReferenceContract()
        {
          entityId = conditionTemplate1.CreatedBy
        };
        conditionTemplate2.CreatedDate = new DateTime?(conditionTemplate1.CreatedDate);
        conditionTemplate2.LastModifiedBy = new EntityReferenceContract()
        {
          entityId = conditionTemplate1.LastModifiedBy
        };
        conditionTemplate2.LastModifiedDate = new DateTime?(conditionTemplate1.LastModifiedDate);
      }
      return conditionTemplate2;
    }
  }
}
