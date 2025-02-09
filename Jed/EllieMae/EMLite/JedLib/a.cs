// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.JedLib.a
// Assembly: Jed, Version=1.0.1234.56789, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D965D698-A97D-45D6-911B-975853D5C21D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Jed.dll

using Microsoft.Win32;
using System;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.JedLib
{
  public class a
  {
    private const string a = "Software\\JedServer\\1.0\\Debug";
    private const string b = "Software\\Jed\\1.0\\Debug";
    private const string c = "Software\\Classes\\";
    private static readonly RegistryKey d = Registry.CurrentUser;
    private static readonly string[] e = new string[10]
    {
      \u003CModule\u003E.a("ꪘ겒꺛낝늁뒂뚇뢅"),
      \u003CModule\u003E.a("ꪑ겜꺛낚늅뒀뚇뢆"),
      \u003CModule\u003E.a("ꪘ겒꺔나늄뒋뚃뢏"),
      \u003CModule\u003E.a("ꪘ겙꺞낛늄뒅뚂뢏"),
      \u003CModule\u003E.a("ꪘ겘꺘나늃뒇뚃뢏"),
      \u003CModule\u003E.a("ꪚ겟꺘낗늆뒊뚅뢄"),
      \u003CModule\u003E.a("ꪘ겒꺕낗늃뒃뚆뢄"),
      \u003CModule\u003E.a("ꪘ겒꺛난늉뒄뚁뢂"),
      \u003CModule\u003E.a("ꪘ겒꺔낙늁뒊뚆뢇"),
      \u003CModule\u003E.a("ꪘ겒꺔낚늁뒄뚄뢅")
    };
    private static int f;

    public static void b() => EllieMae.EMLite.JedLib.a.b(false);

    public static void b(bool A_0)
    {
      if (EllieMae.EMLite.JedLib.b.a() != null)
        return;
      string name = \u003CModule\u003E.a("\uAAFA계껋냛닆듒뛇룒뫥볱뻘샛슝쓲웫죷쪕첏캨킭튤풴");
      if (A_0)
        name = \u003CModule\u003E.a("\uAAFA계껋냛닆듒뛇룒뫥볱뻘샛슒쒦욷좱쪬첹캑탾틿퓣횉\uD893\uDABC\uDCB9\uDEA8\uE0B8");
      RegistryKey registryKey1 = EllieMae.EMLite.JedLib.a.d.OpenSubKey(name);
      string str1 = (string) registryKey1.GetValue(\u003CModule\u003E.a("ꫪ곢껩"));
      string str2 = (string) registryKey1.GetValue(\u003CModule\u003E.a("\uAAFF곎껟냜님드뛛"));
      string str3 = \u003CModule\u003E.a("\uAAFA계껋냛닆듒뛇룒뫥본뻑샞슲쒰욠좴쪕") + str1;
      try
      {
        EllieMae.EMLite.JedLib.a.d.DeleteSubKey(str3);
      }
      catch (Exception ex)
      {
      }
      EllieMae.EMLite.JedLib.a.d.CreateSubKey(str3);
      RegistryKey registryKey2 = EllieMae.EMLite.JedLib.a.d.OpenSubKey(str3, true);
      registryKey2.SetValue(\u003CModule\u003E.a("\uAAFF곎껟냜님드뛛"), (object) (str2.Substring(0, str2.LastIndexOf(\u003CModule\u003E.a("ꪇ"))) + \u003CModule\u003E.a("ꪐ")));
      registryKey2.SetValue(\u003CModule\u003E.a("\uAAFD곂껀냊"), (object) DateTime.Now.ToString());
    }

    public static void a() => EllieMae.EMLite.JedLib.a.a(false);

    public static void a(bool A_0)
    {
      if (EllieMae.EMLite.JedLib.b.a() != null)
        return;
      string name = \u003CModule\u003E.a("\uAAFA계껋냛닆듒뛇룒뫥볱뻘샛슝쓲웫죷쪕첏캨킭튤풴");
      if (A_0)
        name = \u003CModule\u003E.a("\uAAFA계껋냛닆듒뛇룒뫥볱뻘샛슒쒦욷좱쪬첹캑탾틿퓣횉\uD893\uDABC\uDCB9\uDEA8\uE0B8");
      string str1 = (string) EllieMae.EMLite.JedLib.a.d.OpenSubKey(name).GetValue(\u003CModule\u003E.a("ꫪ곢껩"));
      string str2 = (string) EllieMae.EMLite.JedLib.a.d.OpenSubKey(\u003CModule\u003E.a("\uAAFA계껋냛닆듒뛇룒뫥본뻑샞슲쒰욠좴쪕") + str1).GetValue(\u003CModule\u003E.a("\uAAFF곎껟냜님드뛛"));
      EllieMae.EMLite.JedLib.a.d.DeleteSubKeyTree(\u003CModule\u003E.a("\uAAFA계껋냛닆듒뛇룒뫥본뻑샞슲쒰욠좴쪕") + str1);
      string str3 = "";
      if (A_0)
        str3 = \u003CModule\u003E.a("\uAAFA곎껟냙닔듁");
      EllieMae.EMLite.JedLib.a.d.DeleteSubKeyTree(\u003CModule\u003E.a("\uAAFA곤껫냻닦듲뛧룲뫥볱뻘샛") + str3);
      int index = 0;
      if (str2.Equals(\u003CModule\u003E.a("ꫣ곎껉낏늀뒝뚅")))
        index = 0;
      else if (!str2.Equals(\u003CModule\u003E.a("ꫣ곎껉낏늀뒝뚄")))
      {
        if (str2.Equals(\u003CModule\u003E.a("ꫣ곎껉낏늀뒝뚇")))
          index = 2;
        else if (!str2.Equals(\u003CModule\u003E.a("ꫣ곎껉낏늀뒝뚆")))
        {
          if (!str2.Equals(\u003CModule\u003E.a("ꫣ곎껉낏늀뒝뚁")))
          {
            if (!str2.Equals(\u003CModule\u003E.a("ꫣ곎껉낏늀뒝뚀")))
            {
              if (str2.Equals(\u003CModule\u003E.a("ꫣ곎껉낏늀뒝뚃")))
                index = 6;
              else if (str2.Equals(\u003CModule\u003E.a("ꫣ곎껉낏늀뒝뚂")))
                index = 7;
              else if (!str2.Equals(\u003CModule\u003E.a("ꫣ곎껉낏늀뒝뚍")))
              {
                if (str2.Equals(\u003CModule\u003E.a("ꫣ곎껉낏늀뒝뚌")))
                  index = 9;
              }
              else
                index = 8;
            }
            else
              index = 5;
          }
          else
            index = 4;
        }
        else
          index = 3;
      }
      else
        index = 1;
      byte[] bytes = Encoding.ASCII.GetBytes(EllieMae.EMLite.JedLib.a.e[index]);
      EllieMae.EMLite.JedLib.b.a(new EllieMae.EMLite.JedLib.b(EllieMae.EMLite.JedLib.a.e[0]).b(bytes, 0, bytes.Length));
    }

    public static EllieMae.EMLite.JedLib.b b(string A_0)
    {
      byte[] bytes = Encoding.ASCII.GetBytes(EllieMae.EMLite.JedLib.a.e[EllieMae.EMLite.JedLib.a.f]);
      EllieMae.EMLite.JedLib.a.f = Convert.ToInt32(A_0.Substring(A_0.Length - 1, 1));
      return new EllieMae.EMLite.JedLib.b(new EllieMae.EMLite.JedLib.b(EllieMae.EMLite.JedLib.a.e[EllieMae.EMLite.JedLib.a.f]).b(bytes, 0, bytes.Length));
    }

    public static void a(string A_0) => EllieMae.EMLite.JedLib.a.f = Convert.ToInt32(A_0.Substring(3, 1));
  }
}
