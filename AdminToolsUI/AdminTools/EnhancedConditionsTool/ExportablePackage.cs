// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.AdminTools.EnhancedConditionsTool.ExportablePackage
// Assembly: AdminToolsUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: BCE9F231-878C-4206-826C-76CFCB8C9167
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\AdminToolsUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable disable
namespace EllieMae.EMLite.AdminTools.EnhancedConditionsTool
{
  public sealed class ExportablePackage
  {
    private static List<string> s_defaultTrackingNames = Utils.GetEnhanceConditionsDefaultTrackingOptions();
    private Dictionary<string, string> _allDocuments;
    private Dictionary<string, string> _allRoles;
    private Sessions.Session _session;

    public ExportablePackage()
    {
    }

    private ExportablePackage(Sessions.Session session)
    {
      this._session = session;
      (this._allRoles, this._allDocuments) = this.GetAllRolesAndDocuments();
    }

    public IDictionary<string, string> Documents { get; set; }

    public IDictionary<string, string> Roles { get; set; }

    public IDictionary<string, string> Owner { get; set; }

    public IEnumerable<EnhancedConditionTemplate> Templates { get; set; }

    public IEnumerable<EnhancedConditionType> Types { get; set; }

    public EnhancedConditionTemplate MatchingTemplate(EnhancedConditionTemplate toMatch)
    {
      IEnumerable<EnhancedConditionTemplate> templates = this.Templates;
      return templates == null ? (EnhancedConditionTemplate) null : templates.FirstOrDefault<EnhancedConditionTemplate>((Func<EnhancedConditionTemplate, bool>) (t => t.Matches(toMatch)));
    }

    public static async Task<ExportablePackage> BuildAsync(
      Sessions.Session session,
      bool filterRolesAndDocuments = true)
    {
      ExportablePackage package = new ExportablePackage(session);
      RestApiHelper helper = new RestApiHelper(session);
      ExportablePackage exportablePackage = package;
      exportablePackage.Types = await helper.GetEnhancedConditionTypes();
      exportablePackage = (ExportablePackage) null;
      exportablePackage = package;
      exportablePackage.Templates = await helper.GetEnhancedConditionTemplates();
      exportablePackage = (ExportablePackage) null;
      package.Roles = (IDictionary<string, string>) package._allRoles;
      package.Documents = (IDictionary<string, string>) package._allDocuments;
      Action<ConditionDefinitionContract> action = (Action<ConditionDefinitionContract>) (definitions =>
      {
        if (definitions?.trackingDefinitions == null)
          return;
        foreach (TrackingDefinitionContract trackingDefinition in definitions.trackingDefinitions)
        {
          if (trackingDefinition.roles == null)
            trackingDefinition.roles = new EntityReferenceContract[0];
        }
      });
      foreach (EnhancedConditionType type in package.Types)
        action(type.definitions);
      foreach (EnhancedConditionTemplate template in package.Templates)
      {
        if (template.ConnectSettings.DocumentTemplate.entityId == null)
          template.ConnectSettings.DocumentTemplate = (EntityReferenceContract) null;
        TrackingDefinitionContract[] trackingDefinitions = template.customDefinitions?.trackingDefinitions;
        if (trackingDefinitions != null)
        {
          template.customDefinitions.trackingDefinitions = ((IEnumerable<TrackingDefinitionContract>) trackingDefinitions).Where<TrackingDefinitionContract>((Func<TrackingDefinitionContract, bool>) (t => !ExportablePackage.s_defaultTrackingNames.Contains(t.name))).ToArray<TrackingDefinitionContract>();
          action(template.customDefinitions);
        }
        if (template.Category == null)
          template.Category = string.Empty;
        if (template.PriorTo == null)
          template.PriorTo = string.Empty;
        if (template.Recipient == null)
          template.Recipient = string.Empty;
        if (template.Source == null)
          template.Source = string.Empty;
        if (template.ExternalDescription == null)
          template.ExternalDescription = string.Empty;
        if (template.ExternalId == null)
          template.ExternalId = string.Empty;
        if (template.InternalDescription == null)
          template.InternalDescription = string.Empty;
        if (template.InternalId == null)
          template.InternalId = string.Empty;
      }
      if (filterRolesAndDocuments)
      {
        package.Owner = package.GetTypeRoles(true);
        package.Roles = package.GetTypeRoles();
        package.Documents = package.GetTemplateDocuments();
      }
      return package;
    }

    public void RemapIdentifiers(ExportablePackage target)
    {
      ExportablePackage source = this;
      if (source.Templates == null || source.Roles == null || source.Documents == null)
        throw new ArgumentException("Source properties invalid");
      if (target.Templates == null || target.Roles == null || target.Documents == null)
        throw new ArgumentException("Target properties invalid");
      StringComparison comparison = PackageValidator.Comparison;
      Func<IDictionary<string, string>, IDictionary<string, string>, string, string> func = (Func<IDictionary<string, string>, IDictionary<string, string>, string, string>) ((tgt, src, id) => tgt.FirstOrDefault<KeyValuePair<string, string>>((Func<KeyValuePair<string, string>, bool>) (kvp => kvp.Value.Equals(src[id], PackageValidator.Comparison))).Key);
      foreach (EnhancedConditionTemplate template1 in source.Templates)
      {
        EnhancedConditionTemplate template = template1;
        template.Id = (Guid?) target.Templates.FirstOrDefault<EnhancedConditionTemplate>((Func<EnhancedConditionTemplate, bool>) (t => t.Title.Equals(template.Title, comparison) && t.ConditionType.Equals(template.ConditionType, comparison)))?.Id;
        string entityId1 = template.ConnectSettings?.DocumentTemplate?.entityId;
        if (entityId1 != null && !target.Documents.ContainsKey(entityId1))
          template.ConnectSettings.DocumentTemplate.entityId = func(target.Documents, source.Documents, entityId1);
        template.AssignedTo = template.AssignedTo ?? new List<EntityReferenceContract>();
        foreach (EntityReferenceContract referenceContract in template.AssignedTo)
        {
          string entityId2 = referenceContract.entityId;
          if (entityId2 != null && !target.Documents.ContainsKey(entityId2))
            referenceContract.entityId = func(target.Documents, source.Documents, entityId2);
        }
        if (template.Owner != null && target.Roles != null && source.Owner != null)
        {
          List<KeyValuePair<string, string>> list = target.Roles.Where<KeyValuePair<string, string>>((Func<KeyValuePair<string, string>, bool>) (x => x.Value == (source.Owner.ContainsKey(template.Owner.entityId) ? source.Owner[template.Owner.entityId] : string.Empty))).ToList<KeyValuePair<string, string>>();
          if (list.Count > 0)
            template.Owner.entityId = list[0].Key;
        }
        if (template.CustomizeTypeDefinition.Value && template.customDefinitions?.trackingDefinitions != null)
        {
          foreach (TrackingDefinitionContract trackingDefinition in template.customDefinitions.trackingDefinitions)
          {
            foreach (EntityReferenceContract role in trackingDefinition.roles)
            {
              string entityId3 = role.entityId;
              if (entityId3 != null && !target.Roles.ContainsKey(entityId3))
                role.entityId = func(target.Roles, source.Roles, entityId3);
            }
          }
        }
      }
    }

    private (Dictionary<string, string>, Dictionary<string, string>) GetAllRolesAndDocuments()
    {
      WorkflowManager bpmManager = (WorkflowManager) this._session.BPM.GetBpmManager(BpmCategory.Workflow);
      Dictionary<string, string> dictionary1 = bpmManager != null ? ((IEnumerable<RoleInfo>) bpmManager.GetAllRoleFunctions()).ToDictionary<RoleInfo, string, string>((Func<RoleInfo, string>) (r => r.ID.ToString()), (Func<RoleInfo, string>) (r => r.Name)) : (Dictionary<string, string>) null;
      Sessions.Session session = this._session;
      Dictionary<string, string> dictionary2;
      if (session == null)
      {
        dictionary2 = (Dictionary<string, string>) null;
      }
      else
      {
        IConfigurationManager configurationManager = session.ConfigurationManager;
        if (configurationManager == null)
        {
          dictionary2 = (Dictionary<string, string>) null;
        }
        else
        {
          DocumentTrackingSetup documentTrackingSetup = configurationManager.GetDocumentTrackingSetup();
          dictionary2 = documentTrackingSetup != null ? documentTrackingSetup.ToDictionary<DocumentTemplate, string, string>((Func<DocumentTemplate, string>) (d => d.Guid), (Func<DocumentTemplate, string>) (d => d.Name)) : (Dictionary<string, string>) null;
        }
      }
      Dictionary<string, string> dictionary3 = dictionary2;
      return (dictionary1, dictionary3);
    }

    private IDictionary<string, string> GetTemplateDocuments()
    {
      IEnumerable<EnhancedConditionTemplate> templates1 = this.Templates;
      if (templates1 == null)
        return (IDictionary<string, string>) null;
      IEnumerable<EnhancedConditionTemplate> source = templates1.Where<EnhancedConditionTemplate>((Func<EnhancedConditionTemplate, bool>) (p => p.AssignedTo != null));
      if (source == null)
        return (IDictionary<string, string>) null;
      IEnumerable<string> first = source.SelectMany<EnhancedConditionTemplate, string>((Func<EnhancedConditionTemplate, IEnumerable<string>>) (t =>
      {
        List<EntityReferenceContract> assignedTo = t.AssignedTo;
        return assignedTo == null ? (IEnumerable<string>) null : assignedTo.Select<EntityReferenceContract, string>((Func<EntityReferenceContract, string>) (doc => doc?.entityId));
      }));
      IEnumerable<EnhancedConditionTemplate> templates2 = this.Templates;
      IEnumerable<string> second = templates2 != null ? templates2.Select<EnhancedConditionTemplate, string>((Func<EnhancedConditionTemplate, string>) (t => t.ConnectSettings?.DocumentTemplate?.entityId)) : (IEnumerable<string>) null;
      return (IDictionary<string, string>) first.Union<string>(second).Where<string>((Func<string, bool>) (id => id != null && this._allDocuments?.ContainsKey(id).Value)).Distinct<string>().ToDictionary<string, string, string>((Func<string, string>) (id => id), (Func<string, string>) (id => this._allDocuments[id]));
    }

    private IDictionary<string, string> GetTypeRoles(bool isOwner = false)
    {
      Dictionary<string, string> typeRoles = new Dictionary<string, string>();
      if (isOwner)
      {
        IEnumerable<EnhancedConditionTemplate> templates = this.Templates;
        foreach (EntityReferenceContract referenceContract in templates != null ? templates.Where<EnhancedConditionTemplate>((Func<EnhancedConditionTemplate, bool>) (t => t.Owner != null)).Select<EnhancedConditionTemplate, EntityReferenceContract>((Func<EnhancedConditionTemplate, EntityReferenceContract>) (c => c.Owner)) : (IEnumerable<EntityReferenceContract>) null)
        {
          string entityId = referenceContract.entityId;
          string str;
          if (!string.IsNullOrEmpty(entityId) && !typeRoles.ContainsKey(entityId) && this._allRoles.TryGetValue(entityId, out str))
            typeRoles.Add(entityId, str);
        }
      }
      else
      {
        IEnumerable<EnhancedConditionTemplate> templates = this.Templates;
        IEnumerable<ConditionDefinitionContract> definitionContracts;
        if (templates == null)
        {
          definitionContracts = (IEnumerable<ConditionDefinitionContract>) null;
        }
        else
        {
          IEnumerable<ConditionDefinitionContract> first = templates.Where<EnhancedConditionTemplate>((Func<EnhancedConditionTemplate, bool>) (t => t.CustomizeTypeDefinition.Value && t.customDefinitions != null)).Select<EnhancedConditionTemplate, ConditionDefinitionContract>((Func<EnhancedConditionTemplate, ConditionDefinitionContract>) (c => c.customDefinitions));
          IEnumerable<EnhancedConditionType> types = this.Types;
          IEnumerable<ConditionDefinitionContract> second = types != null ? types.Select<EnhancedConditionType, ConditionDefinitionContract>((Func<EnhancedConditionType, ConditionDefinitionContract>) (t => t.definitions)) : (IEnumerable<ConditionDefinitionContract>) null;
          definitionContracts = first.Union<ConditionDefinitionContract>(second);
        }
        foreach (ConditionDefinitionContract definitionContract in definitionContracts)
        {
          foreach (TrackingDefinitionContract trackingDefinition in definitionContract.trackingDefinitions)
          {
            foreach (EntityReferenceContract role in trackingDefinition.roles)
            {
              string entityId = role.entityId;
              string str;
              if (!string.IsNullOrEmpty(entityId) && !typeRoles.ContainsKey(entityId) && this._allRoles.TryGetValue(entityId, out str))
                typeRoles.Add(entityId, str);
            }
          }
        }
      }
      return (IDictionary<string, string>) typeRoles;
    }
  }
}
