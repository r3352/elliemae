// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.ImageHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.Encompass.Forms;
using mshtml;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  internal class ImageHandler
  {
    private Form currentForm;
    private ImageButton imagePlus;
    private ImageButton imagePlusOver;
    private ImageButton imageMinus;
    private ImageButton imageMinusOver;

    public ImageHandler(Form currentForm)
    {
      this.currentForm = currentForm;
      this.imagePlus = (ImageButton) this.currentForm.FindControl(nameof (imagePlus));
      this.imagePlusOver = (ImageButton) this.currentForm.FindControl(nameof (imagePlusOver));
      this.imageMinus = (ImageButton) this.currentForm.FindControl(nameof (imageMinus));
      this.imageMinusOver = (ImageButton) this.currentForm.FindControl(nameof (imageMinusOver));
    }

    public void OnMouseOut(IHTMLEventObj pEvtObj)
    {
      RuntimeControl controlForElement = this.currentForm.FindControlForElement(pEvtObj.srcElement) as RuntimeControl;
      if (!(controlForElement is ImageButton))
        return;
      ImageButton imageButton = (ImageButton) controlForElement;
      if (imageButton.Source == this.imagePlusOver.Source)
      {
        imageButton.Source = this.imagePlus.Source;
      }
      else
      {
        if (!(imageButton.Source == this.imageMinusOver.Source))
          return;
        imageButton.Source = this.imageMinus.Source;
      }
    }

    public void OnMouseEnter(IHTMLEventObj pEvtObj)
    {
      RuntimeControl controlForElement = this.currentForm.FindControlForElement(pEvtObj.srcElement) as RuntimeControl;
      if (!(controlForElement is ImageButton))
        return;
      ImageButton imageButton = (ImageButton) controlForElement;
      if (imageButton.Source == this.imagePlus.Source)
      {
        imageButton.Source = this.imagePlusOver.Source;
      }
      else
      {
        if (!(imageButton.Source == this.imageMinus.Source))
          return;
        imageButton.Source = this.imageMinusOver.Source;
      }
    }

    public void OnMouseClick(IHTMLEventObj pEvtObj)
    {
      RuntimeControl controlForElement = this.currentForm.FindControlForElement(pEvtObj.srcElement) as RuntimeControl;
      if (!(controlForElement is ImageButton))
        return;
      ImageButton imageButton = (ImageButton) controlForElement;
      if (imageButton.Source == this.imageMinusOver.Source)
      {
        imageButton.Source = this.imagePlusOver.Source;
      }
      else
      {
        if (!(imageButton.Source == this.imagePlusOver.Source))
          return;
        imageButton.Source = this.imageMinusOver.Source;
      }
    }
  }
}
