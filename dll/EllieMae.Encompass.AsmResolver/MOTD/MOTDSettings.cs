// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.AsmResolver.MOTD.MOTDSettings
// Assembly: EllieMae.Encompass.AsmResolver, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF61C82F-0411-4587-80AB-293D90FB58E7
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\EllieMae.Encompass.AsmResolver.dll

using System;

#nullable disable
namespace EllieMae.Encompass.AsmResolver.MOTD
{
  public class MOTDSettings
  {
    public readonly int MessageID;
    public readonly string Title;
    public readonly string Description;
    public readonly string MessageURL;
    public readonly int NumberOfDisplays;
    public readonly int DisplayInterval;
    public readonly MOTDSettings.DisplayIntervalUnit IntervalUnit;
    public readonly DateTime StartTime = DateTime.MinValue;
    public readonly DateTime EndTime = DateTime.MaxValue;
    public readonly int WinWidth;
    public readonly int WinHeight;

    public MOTDSettings(
      int messageID,
      string title,
      string description,
      string messageURL,
      int numberOfDisplays,
      int displayInterval,
      MOTDSettings.DisplayIntervalUnit intervalUnit,
      DateTime startTime,
      DateTime endTime,
      int winWidth,
      int winHeight)
    {
      this.MessageID = messageID;
      this.Title = title;
      this.Description = description;
      this.MessageURL = messageURL;
      this.NumberOfDisplays = numberOfDisplays;
      this.DisplayInterval = displayInterval;
      this.IntervalUnit = intervalUnit;
      this.StartTime = startTime;
      this.EndTime = endTime;
      this.WinWidth = winWidth;
      this.WinHeight = winHeight;
    }

    public enum DisplayIntervalUnit
    {
      Login,
      Day,
      Hour,
      Minute,
    }
  }
}
