// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.IStageLoanHistoryManager
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using System.Collections;
using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public interface IStageLoanHistoryManager
  {
    IEnumerable HistoryEntries { get; }

    void TrackChange(
      string objectID,
      string objectType,
      string details,
      string userId,
      string linkedObjectID = null,
      string linkedObjectType = null);

    void TrackChange(string xmlHistoryString);

    void TrackChange(XmlElement xmlHistoryElement);

    void TrackChange(List<XmlElement> xmlHistoryElements);

    void ClearHistoryEntrie();
  }
}
