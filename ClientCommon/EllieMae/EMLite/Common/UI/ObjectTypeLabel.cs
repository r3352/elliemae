// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.ObjectTypeLabel
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.Common.Properties;
using EllieMae.EMLite.UI;
using System;
using System.Drawing;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class ObjectTypeLabel : Element, IComparable
  {
    private ObjectTypeEnum objType;
    private Element objElement;

    public ObjectTypeLabel(ObjectTypeEnum objType)
    {
      this.objType = objType;
      switch (objType)
      {
        case ObjectTypeEnum.FileAttachment:
          this.objElement = (Element) new ImageElement((Image) Resources.paper_clip);
          break;
        case ObjectTypeEnum.Document:
          this.objElement = (Element) new ImageElement((Image) Resources.document);
          break;
        case ObjectTypeEnum.Condition:
          this.objElement = (Element) new ImageElement((Image) Resources.condition);
          break;
        case ObjectTypeEnum.ImageAttachment:
          this.objElement = (Element) new ImageElement((Image) Resources.image);
          break;
        case ObjectTypeEnum.PageImage:
          this.objElement = (Element) new ImageElement((Image) Resources.page);
          break;
        case ObjectTypeEnum.BackgroundAttachment:
          this.objElement = (Element) new ImageElement((Image) Resources.background_image);
          break;
        case ObjectTypeEnum.CloudAttachment:
          this.objElement = (Element) new ImageElement((Image) Resources.cloud);
          break;
      }
    }

    public ObjectTypeEnum ObjectType => this.objType;

    public override Size Measure(ItemDrawArgs drawArgs) => this.objElement.Measure(drawArgs);

    public override Rectangle Draw(ItemDrawArgs drawArgs) => this.objElement.Draw(drawArgs);

    public override string ToString()
    {
      switch (this.objType)
      {
        case ObjectTypeEnum.FileAttachment:
          return "File";
        case ObjectTypeEnum.Document:
          return "Document";
        case ObjectTypeEnum.Condition:
          return "Condition";
        case ObjectTypeEnum.ImageAttachment:
          return "Image";
        case ObjectTypeEnum.PageImage:
          return "Page";
        default:
          return (string) null;
      }
    }

    public int CompareTo(object obj)
    {
      return obj is ObjectTypeLabel ? this.objType.CompareTo((object) ((ObjectTypeLabel) obj).objType) : -1;
    }
  }
}
