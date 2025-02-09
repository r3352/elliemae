// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.AdminTools.EnhancedConditionsTool.PackageValidator
// Assembly: AdminToolsUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: BCE9F231-878C-4206-826C-76CFCB8C9167
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\AdminToolsUI.dll

using EllieMae.EMLite.DataEngine.eFolder;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.AdminTools.EnhancedConditionsTool
{
  public class PackageValidator
  {
    public static readonly StringComparison Comparison = StringComparison.InvariantCultureIgnoreCase;

    public ExportablePackage Target { get; set; }

    public PackageValidator(ExportablePackage target)
    {
      this.Target = target;
      if (target == null)
        throw new ArgumentException("Target may not be null");
      if (target.Roles == null)
        throw new ArgumentException("Target Roles may not be null");
      if (target.Documents == null)
        throw new ArgumentException("Target Documents may not be null");
      if (target.Types == null)
        throw new ArgumentException("Target Types may not be null");
      if (target.Templates == null)
        throw new ArgumentException("Target Templates may not be null");
    }

    public List<ValidationError> ValidateImport(ExportablePackage source)
    {
      List<ValidationError> errors = new List<ValidationError>();
      ExportablePackage target = this.Target;
      Action<ValidationError> addError = (Action<ValidationError>) (error =>
      {
        if (errors.Any<ValidationError>((Func<ValidationError, bool>) (e =>
        {
          if (!(e.Message == error.Message))
            return false;
          Guid? templateId3 = e.TemplateID;
          Guid? templateId4 = error.TemplateID;
          if (templateId3.HasValue != templateId4.HasValue)
            return false;
          return !templateId3.HasValue || templateId3.GetValueOrDefault() == templateId4.GetValueOrDefault();
        })))
          return;
        errors.Add(error);
      });
      if (source != target)
      {
        foreach (EnhancedConditionType type1 in source.Types)
        {
          EnhancedConditionType type = type1;
          if (!target.Types.Any<EnhancedConditionType>((Func<EnhancedConditionType, bool>) (t => t.id.Equals(type.id, PackageValidator.Comparison) || t.title.Equals(type.title, PackageValidator.Comparison))))
            addError(new ValidationError("Invalid Type " + type.id + " ('" + type.title + "')"));
          if (type.definitions?.trackingDefinitions != null)
          {
            foreach (TrackingDefinitionContract trackingDefinition in type.definitions.trackingDefinitions)
            {
              foreach (EntityReferenceContract role in trackingDefinition.roles)
              {
                EntityReferenceContract optionRole = role;
                if (!source.Roles.Any<KeyValuePair<string, string>>((Func<KeyValuePair<string, string>, bool>) (kvp => kvp.Key.Equals(optionRole.entityId))))
                  addError(new ValidationError("Invalid role " + optionRole.entityId + " in tracking options for Type " + type.id));
              }
            }
          }
        }
        foreach (KeyValuePair<string, string> role1 in (IEnumerable<KeyValuePair<string, string>>) source.Roles)
        {
          KeyValuePair<string, string> role = role1;
          IDictionary<string, string> roles = target.Roles;
          if ((roles != null ? (!roles.Any<KeyValuePair<string, string>>((Func<KeyValuePair<string, string>, bool>) (kvp => kvp.Key.Equals(role.Key, PackageValidator.Comparison) || kvp.Value.Equals(role.Value, PackageValidator.Comparison))) ? 1 : 0) : 1) != 0)
            addError(new ValidationError("Invalid Role " + role.Key + " ('" + role.Value + "')"));
        }
        foreach (KeyValuePair<string, string> document in (IEnumerable<KeyValuePair<string, string>>) source.Documents)
        {
          KeyValuePair<string, string> doc = document;
          IDictionary<string, string> documents = target.Documents;
          if ((documents != null ? (!documents.Any<KeyValuePair<string, string>>((Func<KeyValuePair<string, string>, bool>) (kvp => kvp.Key.Equals(doc.Key, PackageValidator.Comparison) || kvp.Value.Equals(doc.Value, PackageValidator.Comparison))) ? 1 : 0) : 1) != 0)
            addError(new ValidationError("Invalid Document " + doc.Key + " ('" + doc.Value + "')"));
        }
      }
      foreach (EnhancedConditionTemplate template1 in source.Templates)
      {
        EnhancedConditionTemplate template = template1;
        EnhancedConditionType enhancedConditionType = target.Types.FirstOrDefault<EnhancedConditionType>((Func<EnhancedConditionType, bool>) (t => t.title.Equals(template.ConditionType, PackageValidator.Comparison)));
        if (enhancedConditionType == null)
        {
          addError(new ValidationError("Invalid template ConditionType '" + template.ConditionType + "'", template.Id));
        }
        else
        {
          ConditionDefinitionContract definitionContract = template.CustomizeTypeDefinition.Value ? template.customDefinitions : enhancedConditionType.definitions;
          Action<string, string, OptionDefinitionContract[]> action = (Action<string, string, OptionDefinitionContract[]>) ((name, value, options) =>
          {
            if (string.IsNullOrEmpty(value) || (options != null ? (!((IEnumerable<OptionDefinitionContract>) options).Any<OptionDefinitionContract>((Func<OptionDefinitionContract, bool>) (o => o.name.Equals(value, PackageValidator.Comparison))) ? 1 : 0) : 1) == 0)
              return;
            addError(new ValidationError("Invalid template " + name + " '" + value + "'", template.Id));
          });
          action("Category", template.Category, definitionContract.categoryDefinitions);
          action("PriorTo", template.PriorTo, definitionContract.priorToDefinitions);
          action("Recipient", template.Recipient, definitionContract.recipientDefinitions);
          action("Source", template.Source, definitionContract.sourceDefinitions);
        }
        Func<IDictionary<string, string>, string, string> func = (Func<IDictionary<string, string>, string, string>) ((dict, id) => !dict.ContainsKey(id) ? id : "'" + dict[id] + "'");
        if (template.Owner != null && target.Roles != null && source.Owner != null)
        {
          IDictionary<string, string> roles = target.Roles;
          if ((roles != null ? (!roles.Any<KeyValuePair<string, string>>((Func<KeyValuePair<string, string>, bool>) (kvp => source.Owner.ContainsKey(template.Owner.entityId) && source.Owner[template.Owner.entityId].Contains(kvp.Value))) ? 1 : 0) : 1) != 0)
            addError(new ValidationError("Invalid template Owner Role " + func(source.Owner, template.Owner.entityId), template.Id));
        }
        if (template.CustomizeTypeDefinition.Value && template.customDefinitions != null)
        {
          foreach (TrackingDefinitionContract trackingDefinition in template.customDefinitions.trackingDefinitions)
          {
            foreach (EntityReferenceContract role2 in trackingDefinition.roles)
            {
              EntityReferenceContract role = role2;
              IDictionary<string, string> roles = target.Roles;
              if ((roles != null ? (!roles.Any<KeyValuePair<string, string>>((Func<KeyValuePair<string, string>, bool>) (kvp => source.Roles.ContainsKey(role.entityId) && kvp.Value.Equals(source.Roles[role.entityId], PackageValidator.Comparison))) ? 1 : 0) : 1) != 0)
                addError(new ValidationError("Invalid template tracking Role " + func(source.Roles, role.entityId), template.Id));
            }
          }
        }
        if (template.AssignedTo != null)
        {
          foreach (EntityReferenceContract referenceContract in template.AssignedTo)
          {
            EntityReferenceContract doc = referenceContract;
            IDictionary<string, string> documents = target.Documents;
            if ((documents != null ? (!documents.Any<KeyValuePair<string, string>>((Func<KeyValuePair<string, string>, bool>) (t => t.Value.Equals(source.Documents[doc.entityId], PackageValidator.Comparison))) ? 1 : 0) : 1) != 0)
              addError(new ValidationError("Invalid template AssignedTo Document " + func(source.Documents, doc.entityId), template.Id));
          }
        }
        if (!Enum.TryParse<ConnectSettingsDocumentOptions>(template.ConnectSettings?.DocumentOption, out ConnectSettingsDocumentOptions _))
          addError(new ValidationError("Invalid template ConnectSettings DocumentOption '" + template.ConnectSettings.DocumentOption + "'", template.Id));
        string connectDocumentTemplateID = template.ConnectSettings?.DocumentTemplate?.entityId;
        if (connectDocumentTemplateID != null)
        {
          if (source.Documents.ContainsKey(connectDocumentTemplateID))
          {
            IDictionary<string, string> documents = target.Documents;
            if ((documents != null ? (!documents.Any<KeyValuePair<string, string>>((Func<KeyValuePair<string, string>, bool>) (kvp => source.Documents.ContainsKey(connectDocumentTemplateID) && kvp.Value.Equals(source.Documents[connectDocumentTemplateID], PackageValidator.Comparison))) ? 1 : 0) : 1) == 0)
              goto label_64;
          }
          addError(new ValidationError("Invalid template ConnectSettings DocumentTemplate '" + func(source.Documents, connectDocumentTemplateID) + "'", template.Id));
        }
label_64:
        DateTime result;
        if (!string.IsNullOrEmpty(template.StartDate) && !DateTime.TryParse(template.StartDate, out result))
          addError(new ValidationError("Invalid template StartDate '" + template.StartDate + "'", template.Id));
        if (!string.IsNullOrEmpty(template.EndDate) && !DateTime.TryParse(template.EndDate, out result))
          addError(new ValidationError("Invalid template EndDate '" + template.EndDate + "'", template.Id));
      }
      return errors;
    }
  }
}
