// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.eFolder.EnhancedConditionTemplate
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.DataEngine.eFolder
{
  [Serializable]
  public class EnhancedConditionTemplate
  {
    public EnhancedConditionTemplate()
    {
    }

    public EnhancedConditionTemplate(
      string conditionType,
      bool? active,
      string internalId,
      string externalId,
      bool? allowDuplicate,
      string title,
      string internalDesc,
      string externalDesc,
      List<EntityReferenceContract> assignedTo,
      string category,
      string source,
      string recipient,
      string priorTo,
      string startDate,
      string endDate,
      int? daysToRecieve,
      List<string> printDefinitions,
      ConnectSettingsContract connectSettings,
      bool? customizeTypeDefinition,
      ConditionDefinitionContract definitions,
      EntityReferenceContract owner)
    {
      this.ConditionType = conditionType;
      this.Active = active;
      this.InternalId = internalId;
      this.ExternalId = externalId;
      this.AllowDuplicate = allowDuplicate;
      this.Title = title;
      this.InternalDescription = internalDesc;
      this.ExternalDescription = externalDesc;
      this.AssignedTo = assignedTo;
      this.Category = category;
      this.Source = source;
      this.Recipient = recipient;
      this.PriorTo = priorTo;
      this.StartDate = startDate;
      this.EndDate = endDate;
      this.DaysToReceive = daysToRecieve;
      this.PrintDefinitions = printDefinitions;
      this.ConnectSettings = connectSettings;
      this.CustomizeTypeDefinition = customizeTypeDefinition;
      this.customDefinitions = definitions;
      this.Owner = owner;
    }

    public virtual Guid? Id { get; set; }

    public virtual bool? Active { get; set; }

    public virtual bool? AllowDuplicate { get; set; }

    public virtual List<EntityReferenceContract> AssignedTo { get; set; }

    public virtual string Category { get; set; }

    public virtual ConnectSettingsContract ConnectSettings { get; set; }

    public virtual string ConditionType { get; set; }

    public virtual string ExternalId { get; set; }

    public virtual string ExternalDescription { get; set; }

    public virtual bool? CustomizeTypeDefinition { get; set; }

    public virtual string InternalDescription { get; set; }

    public virtual string InternalId { get; set; }

    public virtual string StartDate { get; set; }

    public virtual string EndDate { get; set; }

    public int? DaysToReceive { get; set; }

    public EntityReferenceContract Owner { get; set; }

    public virtual List<string> PrintDefinitions { get; set; }

    public virtual EntityReferenceContract CreatedBy { get; set; }

    public virtual DateTime? CreatedDate { get; set; }

    public virtual EntityReferenceContract LastModifiedBy { get; set; }

    public virtual DateTime? LastModifiedDate { get; set; }

    public virtual string PriorTo { get; set; }

    public virtual string Recipient { get; set; }

    public virtual string Source { get; set; }

    public virtual string Title { get; set; }

    public override string ToString() => this.Title;

    [JsonIgnore]
    public bool IsInternalPrint
    {
      get
      {
        return this.PrintDefinitions != null && this.PrintDefinitions.Contains(PrintDefinitionContract.InternalPrint.ToString());
      }
      set
      {
        if (this.PrintDefinitions == null)
          this.PrintDefinitions = new List<string>();
        if (value)
        {
          if (this.PrintDefinitions.Contains(PrintDefinitionContract.InternalPrint.ToString()))
            return;
          this.PrintDefinitions.Add(PrintDefinitionContract.InternalPrint.ToString());
        }
        else
        {
          if (!this.PrintDefinitions.Contains(PrintDefinitionContract.InternalPrint.ToString()))
            return;
          this.PrintDefinitions.Remove(PrintDefinitionContract.InternalPrint.ToString());
        }
      }
    }

    [JsonIgnore]
    public bool IsExternalPrint
    {
      get
      {
        return this.PrintDefinitions != null && this.PrintDefinitions.Contains(PrintDefinitionContract.ExternalPrint.ToString());
      }
      set
      {
        if (this.PrintDefinitions == null)
          this.PrintDefinitions = new List<string>();
        if (value)
        {
          if (this.PrintDefinitions.Contains(PrintDefinitionContract.ExternalPrint.ToString()))
            return;
          this.PrintDefinitions.Add(PrintDefinitionContract.ExternalPrint.ToString());
        }
        else
        {
          if (!this.PrintDefinitions.Contains(PrintDefinitionContract.ExternalPrint.ToString()))
            return;
          this.PrintDefinitions.Remove(PrintDefinitionContract.ExternalPrint.ToString());
        }
      }
    }

    [JsonIgnore]
    public string UniqueKey => this.ConditionType + "-*-" + this.Title;

    public virtual ConditionDefinitionContract customDefinitions { get; set; }

    public virtual object Clone()
    {
      return (object) new EnhancedConditionTemplate(this.ConditionType, this.Active, this.InternalId, this.ExternalId, this.AllowDuplicate, this.Title, this.InternalDescription, this.ExternalDescription, this.AssignedTo, this.Category, this.Source, this.Recipient, this.PriorTo, this.StartDate, this.EndDate, this.DaysToReceive, this.PrintDefinitions, this.ConnectSettings, this.CustomizeTypeDefinition, this.customDefinitions, this.Owner);
    }

    public virtual object DeepClone(bool copyGuid = false)
    {
      List<EntityReferenceContract> assignedTo1 = new List<EntityReferenceContract>();
      List<EntityReferenceContract> assignedTo2 = this.AssignedTo;
      // ISSUE: explicit non-virtual call
      if ((assignedTo2 != null ? (__nonvirtual (assignedTo2.Count) > 0 ? 1 : 0) : 0) != 0)
        assignedTo1 = new List<EntityReferenceContract>(this.AssignedTo.Select<EntityReferenceContract, EntityReferenceContract>((Func<EntityReferenceContract, EntityReferenceContract>) (e => new EntityReferenceContract()
        {
          entityId = e.entityId
        })));
      List<string> printDefinitions = this.PrintDefinitions != null ? new List<string>(this.PrintDefinitions.Select<string, string>((Func<string, string>) (e => e))) : (List<string>) null;
      ConnectSettingsContract settingsContract = new ConnectSettingsContract();
      settingsContract.DocumentOption = this.ConnectSettings?.DocumentOption;
      EntityReferenceContract referenceContract;
      if (this.ConnectSettings?.DocumentTemplate?.entityId != null)
        referenceContract = new EntityReferenceContract()
        {
          entityId = this.ConnectSettings?.DocumentTemplate?.entityId
        };
      else
        referenceContract = (EntityReferenceContract) null;
      settingsContract.DocumentTemplate = referenceContract;
      ConnectSettingsContract connectSettings = settingsContract;
      ConditionDefinitionContract definitionContract1;
      if (this.CustomizeTypeDefinition.Value)
      {
        ConditionDefinitionContract definitionContract2 = new ConditionDefinitionContract();
        ConditionDefinitionContract customDefinitions1 = this.customDefinitions;
        OptionDefinitionContract[] definitionContractArray1;
        if (customDefinitions1 == null)
        {
          definitionContractArray1 = (OptionDefinitionContract[]) null;
        }
        else
        {
          OptionDefinitionContract[] categoryDefinitions = customDefinitions1.categoryDefinitions;
          if (categoryDefinitions == null)
          {
            definitionContractArray1 = (OptionDefinitionContract[]) null;
          }
          else
          {
            IEnumerable<OptionDefinitionContract> source = ((IEnumerable<OptionDefinitionContract>) categoryDefinitions).Select<OptionDefinitionContract, OptionDefinitionContract>((Func<OptionDefinitionContract, OptionDefinitionContract>) (e => new OptionDefinitionContract()
            {
              name = e.name
            }));
            definitionContractArray1 = source != null ? source.ToArray<OptionDefinitionContract>() : (OptionDefinitionContract[]) null;
          }
        }
        definitionContract2.categoryDefinitions = definitionContractArray1;
        ConditionDefinitionContract customDefinitions2 = this.customDefinitions;
        OptionDefinitionContract[] definitionContractArray2;
        if (customDefinitions2 == null)
        {
          definitionContractArray2 = (OptionDefinitionContract[]) null;
        }
        else
        {
          OptionDefinitionContract[] priorToDefinitions = customDefinitions2.priorToDefinitions;
          if (priorToDefinitions == null)
          {
            definitionContractArray2 = (OptionDefinitionContract[]) null;
          }
          else
          {
            IEnumerable<OptionDefinitionContract> source = ((IEnumerable<OptionDefinitionContract>) priorToDefinitions).Select<OptionDefinitionContract, OptionDefinitionContract>((Func<OptionDefinitionContract, OptionDefinitionContract>) (e => new OptionDefinitionContract()
            {
              name = e.name
            }));
            definitionContractArray2 = source != null ? source.ToArray<OptionDefinitionContract>() : (OptionDefinitionContract[]) null;
          }
        }
        definitionContract2.priorToDefinitions = definitionContractArray2;
        ConditionDefinitionContract customDefinitions3 = this.customDefinitions;
        OptionDefinitionContract[] definitionContractArray3;
        if (customDefinitions3 == null)
        {
          definitionContractArray3 = (OptionDefinitionContract[]) null;
        }
        else
        {
          OptionDefinitionContract[] recipientDefinitions = customDefinitions3.recipientDefinitions;
          if (recipientDefinitions == null)
          {
            definitionContractArray3 = (OptionDefinitionContract[]) null;
          }
          else
          {
            IEnumerable<OptionDefinitionContract> source = ((IEnumerable<OptionDefinitionContract>) recipientDefinitions).Select<OptionDefinitionContract, OptionDefinitionContract>((Func<OptionDefinitionContract, OptionDefinitionContract>) (e => new OptionDefinitionContract()
            {
              name = e.name
            }));
            definitionContractArray3 = source != null ? source.ToArray<OptionDefinitionContract>() : (OptionDefinitionContract[]) null;
          }
        }
        definitionContract2.recipientDefinitions = definitionContractArray3;
        ConditionDefinitionContract customDefinitions4 = this.customDefinitions;
        OptionDefinitionContract[] definitionContractArray4;
        if (customDefinitions4 == null)
        {
          definitionContractArray4 = (OptionDefinitionContract[]) null;
        }
        else
        {
          OptionDefinitionContract[] sourceDefinitions = customDefinitions4.sourceDefinitions;
          if (sourceDefinitions == null)
          {
            definitionContractArray4 = (OptionDefinitionContract[]) null;
          }
          else
          {
            IEnumerable<OptionDefinitionContract> source = ((IEnumerable<OptionDefinitionContract>) sourceDefinitions).Select<OptionDefinitionContract, OptionDefinitionContract>((Func<OptionDefinitionContract, OptionDefinitionContract>) (e => new OptionDefinitionContract()
            {
              name = e.name
            }));
            definitionContractArray4 = source != null ? source.ToArray<OptionDefinitionContract>() : (OptionDefinitionContract[]) null;
          }
        }
        definitionContract2.sourceDefinitions = definitionContractArray4;
        ConditionDefinitionContract customDefinitions5 = this.customDefinitions;
        TrackingDefinitionContract[] definitionContractArray5;
        if (customDefinitions5 == null)
        {
          definitionContractArray5 = (TrackingDefinitionContract[]) null;
        }
        else
        {
          TrackingDefinitionContract[] trackingDefinitions = customDefinitions5.trackingDefinitions;
          if (trackingDefinitions == null)
          {
            definitionContractArray5 = (TrackingDefinitionContract[]) null;
          }
          else
          {
            IEnumerable<TrackingDefinitionContract> source1 = ((IEnumerable<TrackingDefinitionContract>) trackingDefinitions).Select<TrackingDefinitionContract, TrackingDefinitionContract>((Func<TrackingDefinitionContract, TrackingDefinitionContract>) (t =>
            {
              TrackingDefinitionContract definitionContract3 = new TrackingDefinitionContract();
              definitionContract3.name = t.name;
              definitionContract3.open = t.open;
              EntityReferenceContract[] roles = t.roles;
              EntityReferenceContract[] referenceContractArray;
              if (roles == null)
              {
                referenceContractArray = (EntityReferenceContract[]) null;
              }
              else
              {
                IEnumerable<EntityReferenceContract> source2 = ((IEnumerable<EntityReferenceContract>) roles).Select<EntityReferenceContract, EntityReferenceContract>((Func<EntityReferenceContract, EntityReferenceContract>) (r => new EntityReferenceContract()
                {
                  entityId = r.entityId
                }));
                referenceContractArray = source2 != null ? source2.ToArray<EntityReferenceContract>() : (EntityReferenceContract[]) null;
              }
              definitionContract3.roles = referenceContractArray;
              return definitionContract3;
            }));
            definitionContractArray5 = source1 != null ? source1.ToArray<TrackingDefinitionContract>() : (TrackingDefinitionContract[]) null;
          }
        }
        definitionContract2.trackingDefinitions = definitionContractArray5;
        definitionContract1 = definitionContract2;
      }
      else
        definitionContract1 = (ConditionDefinitionContract) null;
      ConditionDefinitionContract definitions = definitionContract1;
      return (object) new EnhancedConditionTemplate(this.ConditionType, this.Active, this.InternalId, this.ExternalId, this.AllowDuplicate, this.Title, this.InternalDescription, this.ExternalDescription, assignedTo1, this.Category, this.Source, this.Recipient, this.PriorTo, this.StartDate, this.EndDate, this.DaysToReceive, printDefinitions, connectSettings, this.CustomizeTypeDefinition, definitions, this.Owner)
      {
        Id = (copyGuid ? this.Id : new Guid?(Guid.NewGuid()))
      };
    }

    public bool Matches(EnhancedConditionTemplate test)
    {
      StringComparison comparisonType = StringComparison.InvariantCultureIgnoreCase;
      return this.Title.Equals(test?.Title, comparisonType) && this.ConditionType.Equals(test?.ConditionType, comparisonType);
    }
  }
}
