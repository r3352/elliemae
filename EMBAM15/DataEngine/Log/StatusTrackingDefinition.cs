// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.StatusTrackingDefinition
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Xml;
using System;
using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  [Serializable]
  public class StatusTrackingDefinition
  {
    private readonly string _name;
    private readonly bool _open;
    private readonly int[] _roles;

    public StatusTrackingDefinition(string name, bool open, int[] allowedRoles)
    {
      this._name = name;
      this._open = open;
      this._roles = allowedRoles;
    }

    public StatusTrackingDefinition(XmlElement e)
    {
      AttributeReader attributeReader = new AttributeReader(e);
      this._name = attributeReader.GetString(nameof (Name));
      this._open = attributeReader.GetBoolean(nameof (Open));
      List<int> intList = new List<int>();
      foreach (XmlElement selectNode in e.SelectNodes("AllowedRoles/ref"))
      {
        if (selectNode.HasAttribute("id"))
          intList.Add(int.Parse(selectNode.GetAttribute("id")));
      }
      this._roles = intList.ToArray();
    }

    public string Name => this._name;

    public bool Open => this._open;

    public int[] AllowedRoles => this._roles;

    public void ToXml(XmlElement e)
    {
      AttributeWriter attributeWriter = new AttributeWriter(e);
      attributeWriter.Write("Name", (object) this._name);
      attributeWriter.Write("Open", (object) this._open);
      if (this._roles == null || this._roles.Length == 0)
        return;
      XmlElement element1 = e.OwnerDocument.CreateElement("AllowedRoles");
      e.AppendChild((XmlNode) element1);
      foreach (int role in this._roles)
      {
        XmlElement element2 = e.OwnerDocument.CreateElement("ref");
        new AttributeWriter(element2).Write("id", (object) role);
        element1.AppendChild((XmlNode) element2);
      }
    }
  }
}
