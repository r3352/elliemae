// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.AsmResolver.Utils.HashUtil
// Assembly: EllieMae.Encompass.AsmResolver, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF61C82F-0411-4587-80AB-293D90FB58E7
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\EllieMae.Encompass.AsmResolver.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

#nullable disable
namespace EllieMae.Encompass.AsmResolver.Utils
{
  public class HashUtil
  {
    private static MD5CryptoServiceProvider _md5;
    private static SHA1CryptoServiceProvider _sha1;

    private static MD5CryptoServiceProvider md5
    {
      get
      {
        if (HashUtil._md5 == null)
          HashUtil._md5 = new MD5CryptoServiceProvider();
        return HashUtil._md5;
      }
    }

    private static SHA1CryptoServiceProvider sha1
    {
      get
      {
        if (HashUtil._sha1 == null)
          HashUtil._sha1 = new SHA1CryptoServiceProvider();
        return HashUtil._sha1;
      }
    }

    public static byte[] ComputeHash(HashAlgorithm algorithm, byte[] buffer)
    {
      if (algorithm == HashAlgorithm.MD5)
        return HashUtil.md5.ComputeHash(buffer);
      return algorithm == HashAlgorithm.SHA1 ? HashUtil.sha1.ComputeHash(buffer) : (byte[]) null;
    }

    public static byte[] ComputeHash(HashAlgorithm algorithm, string filePath)
    {
      byte[] buffer = File.ReadAllBytes(filePath);
      return HashUtil.ComputeHash(algorithm, buffer);
    }

    public static string ComputeHashB64(HashAlgorithm algorithm, byte[] buffer)
    {
      return Convert.ToBase64String(HashUtil.ComputeHash(algorithm, buffer));
    }

    public static string ComputeHashB64(HashAlgorithm algorithm, string filePath)
    {
      byte[] buffer = File.ReadAllBytes(filePath);
      return HashUtil.ComputeHashB64(algorithm, buffer);
    }

    public static string ComputeHashB64FilePath(HashAlgorithm algorithm, string pathString)
    {
      byte[] bytes = Encoding.Default.GetBytes(pathString.Trim().ToLower());
      return HashUtil.ComputeHashB64(algorithm, bytes).Replace("/", "#");
    }

    public static Dictionary<string, byte[]> ComputeHash(
      HashAlgorithm algorithm,
      string folder,
      string basePath,
      bool recursive,
      string[] exclusion,
      string[] inclusion)
    {
      DirectoryInfo directoryInfo1 = (DirectoryInfo) null;
      if (basePath != null)
      {
        DirectoryInfo directoryInfo2 = new DirectoryInfo(folder);
        directoryInfo1 = new DirectoryInfo(basePath);
        if (!directoryInfo2.FullName.ToLower().StartsWith(directoryInfo1.FullName.ToLower()))
          throw new Exception("Base path is not a prefix of the files folder path");
      }
      Dictionary<string, byte[]> hash = new Dictionary<string, byte[]>();
      string[] files = Directory.GetFiles(folder, "*", SearchOption.AllDirectories);
      if (files == null)
        return hash;
      List<string> stringList = new List<string>();
      stringList.AddRange((IEnumerable<string>) files);
      if (exclusion != null)
      {
        foreach (string str in files)
        {
          string input = str.Substring(folder.Length);
          while (input.Length > 0 && input[0] == '\\')
            input = input.Substring(1);
          if (BasicUtils.PathMatch(input, exclusion) && (inclusion == null || !BasicUtils.PathMatch(input, inclusion)))
            stringList.Remove(str);
        }
      }
      foreach (string filePath in stringList)
      {
        string key = filePath;
        if (directoryInfo1 != null)
        {
          key = key.Substring(directoryInfo1.FullName.Length);
          while (key.Length > 0 && key[0] == '\\')
            key = key.Substring(1);
        }
        hash.Add(key, HashUtil.ComputeHash(algorithm, filePath));
      }
      return hash;
    }

    public static Dictionary<string, string> ComputeHashB64(
      HashAlgorithm algorithm,
      string folder,
      string basePath,
      bool recursive,
      string[] exclusion,
      string[] inclusion)
    {
      Dictionary<string, string> hashB64 = new Dictionary<string, string>();
      Dictionary<string, byte[]> hash = HashUtil.ComputeHash(algorithm, folder, basePath, recursive, exclusion, inclusion);
      foreach (string key in hash.Keys)
        hashB64.Add(key, Convert.ToBase64String(hash[key]));
      return hashB64;
    }
  }
}
