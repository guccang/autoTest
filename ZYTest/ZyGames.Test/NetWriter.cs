using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class NetWriter
{
    private static ulong s_userID = 0;
    private static string s_strSessionID = "";
    private static string s_strSt = "";

    private  string s_strUrl = "";
    private  string s_strPostData = "";
    private  string s_strUserData = "";
    private  int s_Counter = 1;
    private  string s_md5Key = "";
    private  byte[] _bodyBuffer;
    private  byte[] _headBuffer;
    private readonly NetWriter s_isntance = null;

    public  NetWriter Instance
    {
        get { return s_isntance; }
    }

    public  int MsgId
    {
        get { return s_Counter; }
    }
   
    public  void resetData()
    {
        _headBuffer = null;
        _bodyBuffer = null;
        s_strPostData = "";
        s_strUserData = string.Format("MsgId={0}&Sid={1}&Uid={2}&St={3}", s_Counter, s_strSessionID, s_userID, s_strSt);
        s_Counter++;
    }

    public  void setSessionID(string pszSessionID)
    {
        if (pszSessionID != null)
        {
            s_strSessionID = pszSessionID;
            resetData();
        }
    }

    public  void setUserID(ulong value)
    {
        s_userID = value;
        resetData();
    }

    public  void setStime(string pszTime)
    {
        if (pszTime != null)
        {
            s_strSt = pszTime;
            resetData();
        }
    }

    public void SetHeadBuffer(byte[] buffer)
    {
        _headBuffer = buffer;
    }

    public void SetBodyData(byte[] buffer)
    {
        _bodyBuffer = buffer ?? new byte[0];
    }

    private byte[] GetDataBuffer()
    {
        if (_headBuffer == null || _headBuffer.Length == 0 || _bodyBuffer == null)
        {
            return new byte[0];
        }
        //加头长度,合并body
        byte[] lenBytes = BitConverter.GetBytes(_headBuffer.Length);
        byte[] buffer = new byte[_headBuffer.Length + lenBytes.Length + _bodyBuffer.Length];
        Buffer.BlockCopy(lenBytes, 0, buffer, 0, lenBytes.Length);
        Buffer.BlockCopy(_headBuffer, 0, buffer, lenBytes.Length, _headBuffer.Length);
        Buffer.BlockCopy(_bodyBuffer, 0, buffer, lenBytes.Length + _headBuffer.Length, _bodyBuffer.Length);
        return buffer;
    }


    public NetWriter()
    {
        resetData();
    }

    public byte[] PostData()
    {
            //支持自定义结构
          return  GetDataBuffer();
    }

}