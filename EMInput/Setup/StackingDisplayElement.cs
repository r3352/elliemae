// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.StackingDisplayElement
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.DocEngine;
using EllieMae.EMLite.UI;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class StackingDisplayElement : MultiValueElement
  {
    private StackingElement element;

    public StackingDisplayElement(StackingElement element)
      : base(5, LayoutDirection.Horizontal)
    {
      this.element = element;
      if (element.Type == StackingElementType.DocumentType)
      {
        this.AddElement((Element) new TextElement("set"));
        this.AddElement((Element) new TextElement(element.Name));
      }
      else
      {
        this.AddElement((Element) new SpacerElement(16, 16));
        this.AddElement((Element) new TextElement(element.Name));
      }
    }

    public StackingElement StackingElement => this.element;

    public override string ToString() => this.element.Name;
  }
}
