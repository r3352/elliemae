// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DocEngine.DocEngineStackingOrder
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Serialization;
using EllieMae.Encompass.AsmResolver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;

#nullable disable
namespace EllieMae.EMLite.DocEngine
{
  [Serializable]
  public class DocEngineStackingOrder : BinaryConvertible<DocEngineStackingOrder>, ITemplateSetting
  {
    private string guid;
    private string name;
    private string description;
    private DocumentOrderType orderType;
    private bool isDefault;
    private XmlList<StackingElement> elements;

    public DocEngineStackingOrder(string name, DocumentOrderType orderType)
    {
      this.guid = System.Guid.NewGuid().ToString("D");
      this.name = name;
      this.description = "";
      this.orderType = orderType;
      this.elements = new XmlList<StackingElement>();
    }

    public DocEngineStackingOrder(string guid, string name, DocumentOrderType orderType)
    {
      this.guid = guid;
      this.name = name;
      this.description = "";
      this.orderType = orderType;
      this.elements = new XmlList<StackingElement>();
    }

    public DocEngineStackingOrder(
      string guid,
      string name,
      bool isDefault,
      DocumentOrderType orderType)
    {
      this.guid = guid;
      this.name = name;
      this.isDefault = isDefault;
      this.description = "";
      this.orderType = orderType;
      this.elements = new XmlList<StackingElement>();
    }

    public DocEngineStackingOrder(XmlSerializationInfo info)
    {
      this.guid = info.GetString(nameof (Guid));
      this.name = info.GetString(nameof (Name));
      this.description = info.GetString(nameof (Description));
      this.orderType = info.GetEnum<DocumentOrderType>(nameof (Type));
      this.elements = info.GetValue<XmlList<StackingElement>>(nameof (Elements));
    }

    public string Guid => this.guid;

    public string Name
    {
      get => this.name;
      set => this.name = value;
    }

    string ITemplateSetting.TemplateName
    {
      get => this.Name;
      set => this.Name = value;
    }

    public bool IsDefault
    {
      get => this.isDefault;
      set => this.isDefault = value;
    }

    public string Description
    {
      get => this.description;
      set => this.description = value;
    }

    public DocumentOrderType Type
    {
      get => this.orderType;
      set => this.orderType = value;
    }

    public bool AppliesToOrderType(DocumentOrderType type) => (this.orderType & type) != 0;

    public ITemplateSetting Duplicate()
    {
      DocEngineStackingOrder engineStackingOrder = (DocEngineStackingOrder) this.Clone();
      engineStackingOrder.guid = System.Guid.NewGuid().ToString("D");
      engineStackingOrder.name = "";
      return (ITemplateSetting) engineStackingOrder;
    }

    public Hashtable GetProperties()
    {
      Hashtable insensitiveHashtable = CollectionsUtil.CreateCaseInsensitiveHashtable();
      insensitiveHashtable.Add((object) "Guid", (object) this.Guid);
      insensitiveHashtable.Add((object) "Name", (object) this.Name);
      insensitiveHashtable.Add((object) "Description", (object) this.Description);
      insensitiveHashtable.Add((object) "Type", (object) this.orderType);
      return insensitiveHashtable;
    }

    public List<StackingElement> Elements => (List<StackingElement>) this.elements;

    public string[] ElementsToStringList()
    {
      List<string> stringList = new List<string>();
      foreach (StackingElement element in (List<StackingElement>) this.elements)
        stringList.Add(element.ToString());
      return stringList.ToArray();
    }

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("Guid", (object) this.guid);
      info.AddValue("Name", (object) this.name);
      info.AddValue("Description", (object) this.description);
      info.AddValue("Type", (object) this.orderType);
      info.AddValue("Elements", (object) this.elements);
    }

    public static explicit operator DocEngineStackingOrder(BinaryObject obj)
    {
      return BinaryConvertible<DocEngineStackingOrder>.Parse(obj);
    }

    public static DocEngineStackingOrder GetDefaultStackingOrder(DocumentOrderType orderType)
    {
      string str = SystemSettings.DocDirRelPath + "DefaultStackingOrder." + (object) orderType + ".xml";
      using (BinaryObject o = new BinaryObject(!AssemblyResolver.IsSmartClient ? Path.Combine(SystemSettings.LocalAppDir, str) : AssemblyResolver.GetResourceFileFolderPath(str)))
      {
        try
        {
          return BinaryConvertible<DocEngineStackingOrder>.Parse(o);
        }
        catch
        {
          return new DocEngineStackingOrder("", orderType);
        }
      }
    }
  }
}
