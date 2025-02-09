// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.AdminTools.EnhancedConditionsTool.Exerciser
// Assembly: AdminToolsUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: BCE9F231-878C-4206-826C-76CFCB8C9167
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\AdminToolsUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.eFolder;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.AdminTools.EnhancedConditionsTool
{
  public sealed class Exerciser
  {
    private Random random = new Random();
    private readonly StringComparison s_comparison = StringComparison.InvariantCultureIgnoreCase;

    private int NextID { get; set; } = 1;

    public ExportablePackage CreatePackage()
    {
      DocumentTrackingSetup allDocuments = new DocumentTrackingSetup();
      Enumerable.Range(0, 50).ToList<int>().ForEach((Action<int>) (i => allDocuments.Add(new DocumentTemplate(string.Format("Document {0}", (object) i)))));
      Dictionary<string, string> roles = Enumerable.Range(1, 5).ToDictionary<int, string, string>((Func<int, string>) (id => "Role_" + this.GetNextID()), (Func<int, string>) (id => string.Format("Role {0}", (object) id)));
      List<EnhancedConditionType> types = Enumerable.Range(1, 15).Select<int, EnhancedConditionType>((Func<int, EnhancedConditionType>) (id => this.CreateEnhancedConditionType(string.Format("{0}", (object) id), (IDictionary<string, string>) roles))).ToList<EnhancedConditionType>();
      List<EnhancedConditionTemplate> list = Enumerable.Range(1, 50).Select<int, EnhancedConditionTemplate>((Func<int, EnhancedConditionTemplate>) (id => this.CreateEnhancedCondition(string.Format("{0}", (object) id), (IDictionary<string, string>) roles, (IEnumerable<EnhancedConditionType>) types, allDocuments))).ToList<EnhancedConditionTemplate>();
      string defaultDocOption = ConnectSettingsDocumentOptions.DefaultDocument.ToString().ToLower();
      IOrderedEnumerable<string> source = list.Where<EnhancedConditionTemplate>((Func<EnhancedConditionTemplate, bool>) (x => x.AssignedTo != null)).SelectMany<EnhancedConditionTemplate, string>((Func<EnhancedConditionTemplate, IEnumerable<string>>) (tmp => tmp.AssignedTo.Select<EntityReferenceContract, string>((Func<EntityReferenceContract, string>) (doc => doc.entityId)))).Concat<string>(list.Where<EnhancedConditionTemplate>((Func<EnhancedConditionTemplate, bool>) (tmp => defaultDocOption.Equals(tmp.ConnectSettings.DocumentOption, this.s_comparison))).Select<EnhancedConditionTemplate, string>((Func<EnhancedConditionTemplate, string>) (tmp => tmp.ConnectSettings.DocumentTemplate.entityId))).Distinct<string>().OrderBy<string, string>((Func<string, string>) (id => id));
      return new ExportablePackage()
      {
        Roles = (IDictionary<string, string>) roles,
        Types = (IEnumerable<EnhancedConditionType>) types.ToArray(),
        Templates = (IEnumerable<EnhancedConditionTemplate>) list.ToArray(),
        Documents = (IDictionary<string, string>) source.ToDictionary<string, string, string>((Func<string, string>) (id => id), (Func<string, string>) (id => allDocuments.GetByID(id)?.Name))
      };
    }

    private EnhancedConditionTemplate CreateEnhancedCondition(
      string ident,
      IDictionary<string, string> roles,
      IEnumerable<EnhancedConditionType> types,
      DocumentTrackingSetup _allDocuments)
    {
      DateTime utcNow = DateTime.UtcNow;
      int num = _allDocuments.Count<DocumentTemplate>();
      EnhancedConditionType enhancedConditionType = this.CreateEnhancedConditionType(ident, roles);
      Tuple<int, int> nextRandom = this.GetNextRandom(num, this.GetNextRandom(Math.Min(6, Math.Max(1, num / 3))));
      return new EnhancedConditionTemplate()
      {
        Active = new bool?(true),
        AllowDuplicate = new bool?(true),
        AssignedTo = _allDocuments.Skip<DocumentTemplate>(nextRandom.Item1).Take<DocumentTemplate>(nextRandom.Item2).Select<DocumentTemplate, EntityReferenceContract>((Func<DocumentTemplate, EntityReferenceContract>) (doc => this.NewEntity(doc.Guid))).ToList<EntityReferenceContract>(),
        Category = "Category " + ident,
        ConditionType = types.Skip<EnhancedConditionType>(this.GetNextRandom(types.Count<EnhancedConditionType>())).First<EnhancedConditionType>().title,
        ConnectSettings = new ConnectSettingsContract()
        {
          DocumentOption = "DefaultDocument",
          DocumentTemplate = this.NewEntity(_allDocuments.Skip<DocumentTemplate>(this.GetNextRandom(num)).First<DocumentTemplate>().Guid)
        },
        CreatedBy = this.NewEntity(),
        CreatedDate = new DateTime?(utcNow),
        customDefinitions = enhancedConditionType?.definitions,
        CustomizeTypeDefinition = new bool?(enhancedConditionType != null),
        DaysToReceive = new int?(10),
        EndDate = utcNow.AddDays(180.0).ToShortDateString(),
        ExternalDescription = "External description - " + ident,
        ExternalId = "Ext_" + this.GetNextID(),
        Id = new Guid?(Guid.NewGuid()),
        InternalDescription = "Internal description - " + ident,
        InternalId = "Int_" + this.GetNextID(),
        IsExternalPrint = false,
        IsInternalPrint = true,
        LastModifiedBy = this.NewEntity(),
        LastModifiedDate = new DateTime?(utcNow),
        PrintDefinitions = (List<string>) null,
        PriorTo = "Prior to " + ident,
        Recipient = "Recipient " + ident,
        Source = "Source " + ident,
        StartDate = utcNow.AddDays(1.0).ToShortDateString(),
        Title = "Title " + ident
      };
    }

    private int GetNextRandom(int lessThan) => this.random.Next(0, lessThan);

    private Tuple<int, int> GetNextRandom(int maxValue, int maxCount = 1)
    {
      int num = maxCount > 1 ? 1 + this.GetNextRandom(maxCount) : 1;
      return new Tuple<int, int>(this.GetNextRandom(maxValue - num + 1), num);
    }

    private EntityReferenceContract NewEntity(string id = null)
    {
      return new EntityReferenceContract()
      {
        entityId = id ?? Guid.NewGuid().ToString()
      };
    }

    private string GetNextID() => this.NextID++.ToString();

    private EnhancedConditionType CreateEnhancedConditionType(
      string ident,
      IDictionary<string, string> roles)
    {
      return new EnhancedConditionType()
      {
        id = "Type_" + this.GetNextID(),
        active = true,
        title = "Test type " + ident,
        definitions = new ConditionDefinitionContract()
        {
          categoryDefinitions = this.CreateOptionDefinitions("Category"),
          sourceDefinitions = this.CreateOptionDefinitions("Source"),
          recipientDefinitions = this.CreateOptionDefinitions("Recipient"),
          priorToDefinitions = this.CreateOptionDefinitions("Prior To"),
          trackingDefinitions = this.CreateTrackingDefinitions(roles)
        }
      };
    }

    private OptionDefinitionContract[] CreateOptionDefinitions(string ident)
    {
      return new OptionDefinitionContract[1]
      {
        new OptionDefinitionContract() { name = "Default " + ident }
      };
    }

    private TrackingDefinitionContract[] CreateTrackingDefinitions(IDictionary<string, string> roles)
    {
      return Utils.GetEnhanceConditionsTrackingOptions().Select<string, TrackingDefinitionContract>((Func<string, TrackingDefinitionContract>) (name => new TrackingDefinitionContract()
      {
        name = name,
        open = true,
        roles = roles.Where<KeyValuePair<string, string>>((Func<KeyValuePair<string, string>, bool>) (_ => this.GetNextRandom(3) > 0)).Select<KeyValuePair<string, string>, EntityReferenceContract>((Func<KeyValuePair<string, string>, EntityReferenceContract>) (kvp => this.NewEntity(kvp.Key))).ToArray<EntityReferenceContract>()
      })).ToArray<TrackingDefinitionContract>();
    }
  }
}
