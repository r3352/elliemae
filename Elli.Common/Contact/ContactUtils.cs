// Decompiled with JetBrains decompiler
// Type: Elli.Common.Contact.ContactUtils
// Assembly: Elli.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 5A516607-8D77-4351-85BB-54751A6A69B4
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Common.dll

using PostSharp.Aspects;
using PostSharp.Aspects.Internals;
using PostSharp.ImplementationDetails_e8afc894;
using System;
using System.Net.Mail;
using System.Net.Sockets;
using System.Text;

#nullable disable
namespace Elli.Common.Contact
{
  public class ContactUtils
  {
    public static void Send(MailMessage mailMessage)
    {
      MethodExecutionArgs args = new MethodExecutionArgs((object) null, (Arguments) new Arguments<MailMessage>()
      {
        Arg0 = mailMessage
      });
      // ISSUE: reference to a compiler-generated field
      args.Method = \u003C\u003Ez__a_1._3;
      // ISSUE: reference to a compiler-generated field
      \u003C\u003Ez__a_1.a0.OnEntry(args);
      try
      {
        new SmtpClient().Send(mailMessage);
      }
      finally
      {
        // ISSUE: reference to a compiler-generated field
        \u003C\u003Ez__a_1.a0.OnExit(args);
      }
    }

    public static bool CheckEmail(string fromEmail, string toEmail)
    {
      MethodExecutionArgs args = new MethodExecutionArgs((object) null, (Arguments) new Arguments<string, string>()
      {
        Arg0 = fromEmail,
        Arg1 = toEmail
      });
      // ISSUE: reference to a compiler-generated field
      args.Method = \u003C\u003Ez__a_1._4;
      // ISSUE: reference to a compiler-generated field
      \u003C\u003Ez__a_1.a1.OnEntry(args);
      try
      {
        SmtpClient smtpClient = new SmtpClient();
        bool flag = false;
        TcpClient tcpClient = (TcpClient) null;
        try
        {
          tcpClient = new TcpClient(smtpClient.Host, smtpClient.Port);
          string str = "\r\n";
          byte[] numArray = new byte[1024];
          NetworkStream stream = tcpClient.GetStream();
          stream.Read(numArray, 0, numArray.Length);
          Encoding.ASCII.GetString(numArray);
          byte[] bytes1 = Encoding.ASCII.GetBytes("HELO KirtanHere" + str);
          stream.Write(bytes1, 0, bytes1.Length);
          numArray.Initialize();
          stream.Read(numArray, 0, numArray.Length);
          Encoding.ASCII.GetString(numArray);
          byte[] bytes2 = Encoding.ASCII.GetBytes("MAIL FROM:" + fromEmail + str);
          stream.Write(bytes2, 0, bytes2.Length);
          numArray.Initialize();
          stream.Read(numArray, 0, numArray.Length);
          Encoding.ASCII.GetString(numArray);
          byte[] bytes3 = Encoding.ASCII.GetBytes("RCPT TO:<" + toEmail + ">" + str);
          stream.Write(bytes3, 0, bytes3.Length);
          numArray.Initialize();
          stream.Read(numArray, 0, numArray.Length);
          flag = int.Parse(Encoding.ASCII.GetString(numArray).Substring(0, 3)) == 250;
        }
        catch (Exception ex)
        {
        }
        tcpClient?.Close();
        return flag;
      }
      finally
      {
        // ISSUE: reference to a compiler-generated field
        \u003C\u003Ez__a_1.a1.OnExit(args);
      }
    }
  }
}
