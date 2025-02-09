// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.EnhancedConditionDefinition
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  [Serializable]
  public class EnhancedConditionDefinition
  {
    private readonly IList<OptionDefinition> _categoryDefinitions;
    private readonly IList<OptionDefinition> _priorToDefinitions;
    private readonly IList<OptionDefinition> _recipientDefinitions;
    private readonly IList<OptionDefinition> _sourceDefinitions;
    private readonly IList<StatusTrackingDefinition> _trackingDefinitions;

    public EnhancedConditionDefinition(
      IList<OptionDefinition> categoryDefinitions = null,
      IList<OptionDefinition> priorToDefinitions = null,
      IList<OptionDefinition> recipientDefinitions = null,
      IList<OptionDefinition> sourceDefinitions = null,
      IList<StatusTrackingDefinition> trackingDefinitions = null)
    {
      this._categoryDefinitions = categoryDefinitions ?? (IList<OptionDefinition>) new List<OptionDefinition>();
      this._priorToDefinitions = priorToDefinitions ?? (IList<OptionDefinition>) new List<OptionDefinition>();
      this._recipientDefinitions = recipientDefinitions ?? (IList<OptionDefinition>) new List<OptionDefinition>();
      this._sourceDefinitions = sourceDefinitions ?? (IList<OptionDefinition>) new List<OptionDefinition>();
      this._trackingDefinitions = trackingDefinitions ?? (IList<StatusTrackingDefinition>) new List<StatusTrackingDefinition>();
    }

    public EnhancedConditionDefinition(XmlElement e, string rootName)
    {
      XmlElement xmlElement = (XmlElement) e.SelectSingleNode(rootName);
      if (xmlElement == null)
        return;
      this._categoryDefinitions = (IList<OptionDefinition>) new List<OptionDefinition>();
      this._priorToDefinitions = (IList<OptionDefinition>) new List<OptionDefinition>();
      this._recipientDefinitions = (IList<OptionDefinition>) new List<OptionDefinition>();
      this._sourceDefinitions = (IList<OptionDefinition>) new List<OptionDefinition>();
      this._trackingDefinitions = (IList<StatusTrackingDefinition>) new List<StatusTrackingDefinition>();
      foreach (XmlElement selectNode in xmlElement.SelectNodes("CategoryDefinitions/Definition"))
        this._categoryDefinitions.Add(new OptionDefinition(selectNode));
      foreach (XmlElement selectNode in xmlElement.SelectNodes("PriorToDefinitions/Definition"))
        this._priorToDefinitions.Add(new OptionDefinition(selectNode));
      foreach (XmlElement selectNode in xmlElement.SelectNodes("RecipientDefinitions/Definition"))
        this._recipientDefinitions.Add(new OptionDefinition(selectNode));
      foreach (XmlElement selectNode in xmlElement.SelectNodes("SourceDefinitions/Definition"))
        this._sourceDefinitions.Add(new OptionDefinition(selectNode));
      foreach (XmlElement selectNode in xmlElement.SelectNodes("TrackingDefinitions/Definition"))
        this._trackingDefinitions.Add(new StatusTrackingDefinition(selectNode));
    }

    public IList<OptionDefinition> CategoryDefinitions => this._categoryDefinitions;

    public IList<OptionDefinition> PriorToDefinitions => this._priorToDefinitions;

    public IList<OptionDefinition> RecipientDefinitions => this._recipientDefinitions;

    public IList<OptionDefinition> SourceDefinitions => this._sourceDefinitions;

    public IList<StatusTrackingDefinition> TrackingDefinitions => this._trackingDefinitions;

    public void ToXml(XmlElement definitionElement, string rootName)
    {
      XmlElement element1 = definitionElement.OwnerDocument.CreateElement(rootName);
      definitionElement.AppendChild((XmlNode) element1);
      this.CreateOptionDefinitionsXml(definitionElement, element1, this._categoryDefinitions, "CategoryDefinitions");
      this.CreateOptionDefinitionsXml(definitionElement, element1, this._priorToDefinitions, "PriorToDefinitions");
      this.CreateOptionDefinitionsXml(definitionElement, element1, this._recipientDefinitions, "RecipientDefinitions");
      this.CreateOptionDefinitionsXml(definitionElement, element1, this._sourceDefinitions, "SourceDefinitions");
      if (this._trackingDefinitions == null || !this._trackingDefinitions.Any<StatusTrackingDefinition>())
        return;
      XmlElement element2 = definitionElement.OwnerDocument.CreateElement("TrackingDefinitions");
      element1.AppendChild((XmlNode) element2);
      foreach (StatusTrackingDefinition trackingDefinition in (IEnumerable<StatusTrackingDefinition>) this._trackingDefinitions)
      {
        XmlElement element3 = definitionElement.OwnerDocument.CreateElement("Definition");
        element2.AppendChild((XmlNode) element3);
        trackingDefinition.ToXml(element3);
      }
    }

    private void CreateOptionDefinitionsXml(
      XmlElement e,
      XmlElement rootElement,
      IList<OptionDefinition> optionDefinitions,
      string groupName)
    {
      if (optionDefinitions == null || !optionDefinitions.Any<OptionDefinition>())
        return;
      XmlElement element1 = e.OwnerDocument.CreateElement(groupName);
      rootElement.AppendChild((XmlNode) element1);
      foreach (OptionDefinition optionDefinition in (IEnumerable<OptionDefinition>) optionDefinitions)
      {
        XmlElement element2 = e.OwnerDocument.CreateElement("Definition");
        element1.AppendChild((XmlNode) element2);
        optionDefinition.ToXml(element2);
      }
    }

    public virtual bool AddOptionDefinition(
      OptionDefinition newEntity,
      OptionDefinitionType definitionType)
    {
      if (newEntity == null)
        return false;
      switch (definitionType)
      {
        case OptionDefinitionType.Category:
          this._categoryDefinitions.Add(newEntity);
          break;
        case OptionDefinitionType.PriorTo:
          this._priorToDefinitions.Add(newEntity);
          break;
        case OptionDefinitionType.Recipient:
          this._recipientDefinitions.Add(newEntity);
          break;
        case OptionDefinitionType.Source:
          this._sourceDefinitions.Add(newEntity);
          break;
        default:
          return false;
      }
      return true;
    }

    public virtual bool RemoveOptionDefinition(
      OptionDefinition existingEntity,
      OptionDefinitionType definitionType)
    {
      if (existingEntity == null)
        return false;
      switch (definitionType)
      {
        case OptionDefinitionType.Category:
          return this._categoryDefinitions.Remove(existingEntity);
        case OptionDefinitionType.PriorTo:
          return this._priorToDefinitions.Remove(existingEntity);
        case OptionDefinitionType.Recipient:
          return this._recipientDefinitions.Remove(existingEntity);
        case OptionDefinitionType.Source:
          return this._sourceDefinitions.Remove(existingEntity);
        default:
          return false;
      }
    }

    public virtual void ClearOptionDefinition(OptionDefinitionType definitionType)
    {
      switch (definitionType)
      {
        case OptionDefinitionType.Category:
          this._categoryDefinitions.Clear();
          break;
        case OptionDefinitionType.PriorTo:
          this._priorToDefinitions.Clear();
          break;
        case OptionDefinitionType.Recipient:
          this._recipientDefinitions.Clear();
          break;
        case OptionDefinitionType.Source:
          this._sourceDefinitions.Clear();
          break;
      }
    }
  }
}
