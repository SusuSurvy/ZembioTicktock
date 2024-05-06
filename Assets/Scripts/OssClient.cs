using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aliyun.OSS.Common;
using System.IO;
using Aliyun.OSS;
using UnityEngine;


namespace SparkServer
{
    class Program
    {
        static void Main(string[] args)
        {
          
        }
    }
}

public class Oss
{
    static string accessKeyId = "******";
    static string accessKeySecret = "******";
    static string endpoint = "******";
    static string bucket = "******";
    static OssClient client  = new OssClient(endpoint, accessKeyId, accessKeySecret);
    public static void PutObjectFromFile(string key, string fileName, Action<string> callback = null)
    {
        try
        {
            client.PutObject(bucket, key, fileName);
            Debug.Log($"Put object:{key} succeeded");
            if (callback != null)
            {
                callback("注册成功，请重新登录");
            }
        }
        catch (OssException ex)  
        {
            Debug.Log(string.Format("Failed with error code: {0}; Error info: {1}. \nRequestID:{2}\tHostID:{3}",
                ex.ErrorCode, ex.Message, ex.RequestId, ex.HostId));
            callback("注册失败:" + ex.Message);
        }
        catch (Exception ex)
        {
            Debug.Log(string.Format("Failed with error info: {0}", ex.Message));
            callback("注册失败:" + ex.Message);
        }
    }

    public static void GetObject(string key, string dirToDownload,  Action<string> callback = null)
    {

        client = new OssClient(endpoint, accessKeyId, accessKeySecret);

        try
        {

            var result = client.GetObject(bucket, key);

            using (var requestStream = result.Content)
            {
                string directoryPath = Path.GetDirectoryName(dirToDownload);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
                using (var fs = File.Open(dirToDownload,  FileMode.OpenOrCreate))
                {
                    int length = 4 * 1024;
                    var buf = new byte[length];
                    do
                    {
                        length = requestStream.Read(buf, 0, length);
                        fs.Write(buf, 0, length);
                    } while (length != 0);
                }
            }

            if (callback != null)
            {
                callback(dirToDownload);
            }

            Debug.Log("Get object succeeded");
        }
        catch (OssException ex)
        {
            Debug.Log(string.Format("Failed with error code: {0}; Error info: {1}. \nRequestID:{2}\tHostID:{3}",
                ex.ErrorCode, ex.Message, ex.RequestId, ex.HostId));
            Debug.LogError(key);
            callback(null);
        }
        catch (Exception ex)
        {
            callback(null);
            Debug.Log( string.Format("Failed with error info: {0}", ex.Message));
        }
    }

    public static bool CheckObjectExist(string key)
    {
        try
        {
            bool exist = client.DoesObjectExist(bucket, key);
            return exist;
        }
        catch (OssException ex)
        {
            Debug.Log(string.Format("CheckObjectExist failed, msg:{0}", ex.Message));
        }
        return false;
    }
}
