// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.IMChatMessage
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;
using System.Drawing;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class IMChatMessage : IMMessage
  {
    private IMChatMessage.MyFont font;
    public readonly Color Color;
    public readonly string ReceiverSessionID;

    public IMChatMessage(string text, Font font, Color color, string receiverSessionID)
      : base(text)
    {
      this.font = new IMChatMessage.MyFont(font);
      this.Color = color;
      this.ReceiverSessionID = receiverSessionID;
    }

    public IMChatMessage(
      string text,
      Font font,
      Color color,
      SessionInfo info,
      string receiverSessionID)
      : base(new IMMessage(text), info)
    {
      this.font = new IMChatMessage.MyFont(font);
      this.Color = color;
      this.ReceiverSessionID = receiverSessionID;
    }

    protected IMChatMessage(IMChatMessage source, SessionInfo info)
      : base((IMMessage) source, info)
    {
      this.font = new IMChatMessage.MyFont(source.Font);
      this.Color = source.Color;
      this.ReceiverSessionID = source.ReceiverSessionID;
    }

    public override Message Clone(SessionInfo info) => (Message) new IMChatMessage(this, info);

    public Font Font
    {
      get
      {
        try
        {
          return new Font(this.font.Name, this.font.Size, this.font.Style);
        }
        catch
        {
          return new Font(FontFamily.GenericSansSerif, this.font.Size, this.font.Style);
        }
      }
    }

    [Serializable]
    private class MyFont
    {
      public readonly string Name;
      public readonly float Size;
      public readonly FontStyle Style;

      public MyFont(Font font)
      {
        this.Name = font.Name;
        this.Size = font.Size;
        this.Style = font.Style;
      }
    }
  }
}
