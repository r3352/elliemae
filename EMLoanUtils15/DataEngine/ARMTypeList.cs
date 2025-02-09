// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.ARMTypeList
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.AsmResolver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class ARMTypeList : IEnumerable<ARMType>, IEnumerable
  {
    public static readonly ARMTypeList ARMTypes = new ARMTypeList();
    private List<ARMType> armTypes = new List<ARMType>();

    private ARMTypeList()
    {
      string filename = !AssemblyResolver.IsSmartClient ? Path.Combine(SystemSettings.DocDirAbsPath, "ARMTypeList.xml") : AssemblyResolver.GetResourceFileFullPath(SystemSettings.DocDirRelPath + "ARMTypeList.xml");
      try
      {
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.Load(filename);
        foreach (XmlElement selectNode in xmlDocument.SelectNodes("/LIST/ITEM"))
          this.armTypes.Add(new ARMType(selectNode.GetAttribute("id"), selectNode.GetAttribute("desc")));
      }
      catch (Exception ex)
      {
        throw new Exception("Error loading loading ARM Type List", ex);
      }
    }

    public int Count => this.armTypes.Count;

    public ARMType this[int index] => this.armTypes[index];

    IEnumerator<ARMType> IEnumerable<ARMType>.GetEnumerator()
    {
      return (IEnumerator<ARMType>) this.armTypes.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.armTypes.GetEnumerator();
  }
}
