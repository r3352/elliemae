// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Version.JedServerVersion
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;

#nullable disable
namespace EllieMae.EMLite.Common.Version
{
  [Serializable]
  public class JedServerVersion : IComparable
  {
    public readonly int Major;
    public readonly int Minor;
    public readonly int Build;
    public readonly int Revision;
    public readonly int Subrevision;
    private bool isSHU = true;

    public JedServerVersion(int major, int minor, int build, int revision, int subrevision)
    {
      this.Major = major;
      this.Minor = minor;
      this.Build = build;
      this.Revision = revision;
      this.Subrevision = subrevision;
    }

    public JedServerVersion(string versionString)
    {
      string[] strArray = versionString != null ? versionString.Trim().Split('.') : throw new ArgumentNullException(nameof (versionString));
      try
      {
        this.Major = int.Parse(strArray[0]);
        this.Minor = int.Parse(strArray[1]);
        if (strArray.Length <= 2)
          return;
        this.Build = int.Parse(strArray[2]);
        if (strArray.Length > 3)
        {
          if (strArray[3].Length <= 2)
          {
            this.Revision = int.Parse(strArray[3]);
            if (strArray.Length <= 4)
              return;
            this.Subrevision = int.Parse(strArray[4]);
          }
          else
          {
            if (strArray.Length > 4 || strArray[3].Length > 4)
              throw new Exception("Too many numbers or number too large");
            string str = strArray[3];
            string s1 = str.Substring(0, str.Length - 2);
            string s2 = str.Substring(str.Length - 2);
            this.Revision = int.Parse(s1);
            this.Subrevision = int.Parse(s2);
          }
        }
        else
          this.isSHU = false;
      }
      catch (Exception ex)
      {
        throw new ArgumentException(versionString + ": invalid version string format: " + ex.Message + "\r\n" + ex.StackTrace, nameof (versionString));
      }
    }

    public int UpdateNumber => this.isSHU ? this.Revision * 100 + this.Subrevision : this.Build;

    public string Version(int numOfParts)
    {
      if (numOfParts < 2 || numOfParts > 5)
        throw new ArgumentException("Argument must be between 2 and 5", nameof (numOfParts));
      int[] numArray = new int[5]
      {
        this.Major,
        this.Minor,
        this.Build,
        this.Revision,
        this.Subrevision
      };
      string str = string.Concat((object) numArray[0]);
      if (numOfParts != 4)
      {
        for (int index = 1; index < numOfParts; ++index)
          str = str + "." + (object) numArray[index];
      }
      else
      {
        for (int index = 1; index < numOfParts - 1; ++index)
          str = str + "." + (object) numArray[index];
        str = str + "." + (object) this.UpdateNumber;
      }
      return str;
    }

    public string FullVersion => this.Version(5);

    public string FullVersion4Parts
    {
      get => this.Version(4) + string.Format("{0:00}", (object) this.Subrevision);
    }

    public string DisplayVersionString => this.isSHU ? this.Version(4) : this.Version(3);

    public override string ToString() => this.FullVersion;

    public int CompareTo(object obj, int numOfParts)
    {
      JedServerVersion jedServerVersion = (object) (obj as JedServerVersion) != null ? (JedServerVersion) obj : throw new ArgumentException("Argument must be of type JedServerVersion", nameof (obj));
      int[] numArray1 = new int[5]
      {
        this.Major,
        this.Minor,
        this.Build,
        this.Revision,
        this.Subrevision
      };
      int[] numArray2 = new int[5]
      {
        jedServerVersion.Major,
        jedServerVersion.Minor,
        jedServerVersion.Build,
        jedServerVersion.Revision,
        jedServerVersion.Subrevision
      };
      for (int index = 0; index < numArray1.Length; ++index)
      {
        if (numArray1[index] != numArray2[index])
          return numArray1[index] - numArray2[index];
      }
      return 0;
    }

    public int CompareTo(object obj) => this.CompareTo(obj, 5);

    public override int GetHashCode() => this.FullVersion.GetHashCode();

    public override bool Equals(object target) => this.CompareTo(target) == 0;

    public static bool operator ==(JedServerVersion a, JedServerVersion b)
    {
      if ((object) a == null && (object) b == null)
        return true;
      return ((object) a == null || (object) b != null) && a.Equals((object) b);
    }

    public static bool operator !=(JedServerVersion a, JedServerVersion b)
    {
      if ((object) a == null && (object) b == null)
        return false;
      return (object) a != null && (object) b == null || !(a == b);
    }

    public static bool operator >(JedServerVersion a, JedServerVersion b)
    {
      return a.CompareTo((object) b) > 0;
    }

    public static bool operator >=(JedServerVersion a, JedServerVersion b)
    {
      return a.CompareTo((object) b) >= 0;
    }

    public static bool operator <(JedServerVersion a, JedServerVersion b)
    {
      return a.CompareTo((object) b) < 0;
    }

    public static bool operator <=(JedServerVersion a, JedServerVersion b)
    {
      return a.CompareTo((object) b) <= 0;
    }
  }
}
