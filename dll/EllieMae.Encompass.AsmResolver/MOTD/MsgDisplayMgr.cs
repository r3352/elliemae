// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.AsmResolver.MOTD.MsgDisplayMgr
// Assembly: EllieMae.Encompass.AsmResolver, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF61C82F-0411-4587-80AB-293D90FB58E7
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\EllieMae.Encompass.AsmResolver.dll

using Microsoft.Win32;
using System;

#nullable disable
namespace EllieMae.Encompass.AsmResolver.MOTD
{
  internal class MsgDisplayMgr
  {
    private const string regKeyRoot = "Software\\Ellie Mae\\SmartClient";
    private const string regMessageID = "MessageID";
    private const string regDisplayCount = "DisplayCount";
    private const string regLastDisplayTime = "LastDisplayTime";
    private const string regLoginsSinceLastDisplay = "LoginsSinceLastDisplay";
    private const string regDontShow = "DontShow";
    private const string dateTimeFormat = "MM/dd/yyyy HH:mm";

    internal static void DisplayMessage(
      string appStartupPath,
      string authServerURL,
      string userid,
      MOTDSettings motdSettings)
    {
      if (motdSettings == null)
        return;
      string str = "Software\\Ellie Mae\\SmartClient\\" + appStartupPath.Replace("\\", "/") + "\\MOTD\\" + authServerURL.Replace("\\", "/");
      RegistryKey registryKey = (RegistryKey) null;
      try
      {
        registryKey = Registry.CurrentUser.OpenSubKey(str, true);
        if (registryKey == null)
        {
          registryKey = Registry.CurrentUser.CreateSubKey(str);
          registryKey.SetValue("MessageID", (object) motdSettings.MessageID, RegistryValueKind.DWord);
        }
        else if ((int) registryKey.GetValue("MessageID", (object) 0) != motdSettings.MessageID)
        {
          registryKey.SetValue("MessageID", (object) motdSettings.MessageID, RegistryValueKind.DWord);
          registryKey.DeleteValue("DisplayCount", false);
          registryKey.DeleteValue("LastDisplayTime", false);
          registryKey.DeleteValue("LoginsSinceLastDisplay", false);
          registryKey.DeleteValue("DontShow", false);
        }
        int num1 = (int) registryKey.GetValue("LoginsSinceLastDisplay", (object) 0);
        int num2 = (int) registryKey.GetValue("DisplayCount", (object) 0);
        int num3 = num2 != 0 ? num1 + 1 : 0;
        registryKey.SetValue("LoginsSinceLastDisplay", (object) num3, RegistryValueKind.DWord);
        DateTime now = DateTime.Now;
        if (now < motdSettings.StartTime || now > motdSettings.EndTime)
          return;
        if (motdSettings.NumberOfDisplays < 0)
        {
          if ((int) registryKey.GetValue("DontShow", (object) 0) == 1)
            return;
        }
        else if (motdSettings.NumberOfDisplays > 0 && num2 >= motdSettings.NumberOfDisplays)
          return;
        DateTime dateTime = Convert.ToDateTime((string) registryKey.GetValue("LastDisplayTime", (object) DateTime.MinValue.ToString("MM/dd/yyyy HH:mm")));
        switch (motdSettings.IntervalUnit)
        {
          case MOTDSettings.DisplayIntervalUnit.Login:
            if (num2 > 0 && num3 < motdSettings.DisplayInterval)
              return;
            break;
          case MOTDSettings.DisplayIntervalUnit.Day:
            if (DateTime.Now - dateTime < new TimeSpan(motdSettings.DisplayInterval, 0, 0, 0))
              return;
            break;
          case MOTDSettings.DisplayIntervalUnit.Hour:
            if (DateTime.Now - dateTime < new TimeSpan(0, motdSettings.DisplayInterval, 0, 0))
              return;
            break;
          case MOTDSettings.DisplayIntervalUnit.Minute:
            if (DateTime.Now - dateTime < new TimeSpan(0, 0, motdSettings.DisplayInterval, 0))
              return;
            break;
        }
        string url = motdSettings.MessageURL.Replace("%userid%", userid);
        string newTitle = "";
        string scAppCmdArgs1 = MsgDisplayMgr.getScAppCmdArgs(motdSettings.Title, out newTitle);
        int num4 = scAppCmdArgs1 != null ? 1 : 0;
        string scAppCmdArgs2 = scAppCmdArgs1;
        MessageScreen messageScreen = new MessageScreen(url, num4 != 0, scAppCmdArgs2);
        if (!string.IsNullOrWhiteSpace(newTitle))
          messageScreen.Text = newTitle;
        messageScreen.Width = motdSettings.WinWidth;
        messageScreen.Height = motdSettings.WinHeight;
        if (motdSettings.NumberOfDisplays < 0)
          messageScreen.DontShowEnabled = true;
        else if (motdSettings.NumberOfDisplays >= 0)
          messageScreen.DontShowEnabled = false;
        int num5 = (int) messageScreen.ShowDialog();
        int num6;
        registryKey.SetValue("DisplayCount", (object) (num6 = num2 + 1), RegistryValueKind.DWord);
        registryKey.SetValue("LastDisplayTime", (object) DateTime.Now.ToString("MM/dd/yyyy HH:mm"));
        registryKey.SetValue("LoginsSinceLastDisplay", (object) 0, RegistryValueKind.DWord);
        if (messageScreen.DontShowChecked)
          registryKey.SetValue("DontShow", (object) 1, RegistryValueKind.DWord);
        else
          registryKey.SetValue("DontShow", (object) 0, RegistryValueKind.DWord);
      }
      finally
      {
        registryKey?.Close();
      }
    }

    private static string getScAppCmdArgs(string motdTitle, out string newTitle)
    {
      motdTitle = (motdTitle ?? "").Trim();
      int num = motdTitle.IndexOf(")");
      if (!motdTitle.StartsWith("@") || num < 0)
      {
        newTitle = motdTitle;
        return (string) null;
      }
      newTitle = motdTitle.Substring(num + 1);
      return motdTitle.Substring(2, num - 2);
    }
  }
}
