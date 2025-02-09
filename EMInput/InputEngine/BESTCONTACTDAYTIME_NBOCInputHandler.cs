// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.BESTCONTACTDAYTIME_NBOCInputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.Forms;
using mshtml;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  internal class BESTCONTACTDAYTIME_NBOCInputHandler(
    Sessions.Session session,
    IMainScreen mainScreen,
    HTMLDocument htmldoc,
    Form form,
    object property) : VOLInputHandler(session, mainScreen, htmldoc, form, property)
  {
    protected override void InitHeader() => this.header = "NBOC";

    public BESTCONTACTDAYTIME_NBOCInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }
  }
}
