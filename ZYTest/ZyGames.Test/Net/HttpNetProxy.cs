/****************************************************************************
Copyright (c) 2013-2015 scutgame.com

http://www.scutgame.com

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
****************************************************************************/

using System;
using System.IO;
using System.Net;
using ZyGames.Framework.RPC.IO;

namespace ZyGames.Test.Net
{
    internal class HttpNetProxy : NetProxy
    {
        private readonly string _url;

        public HttpNetProxy(string url)
        {
            _url = url;
        }

        public override void SendAsync(byte[] data, Func<byte[], bool> callback)
        {
            var request = (HttpWebRequest)WebRequest.Create(_url);
            request.Method = "POST";
            request.ContentType = "application/octetstream"; 
            request.ContentLength = data.Length;
            request.Timeout = 1000*10;
            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
                stream.Close();
            }
            byte[] result = new byte[0];
            WebResponse resp = request.GetResponse();
            using (Stream stream = resp.GetResponseStream())
            {
                if (stream != null)
                {
                    var reader = new BinaryReader(stream);
                    result = ReadStream(reader);
                }
            }

            if ((result.Length > 3 && result[0] == 0x1f && result[0 + 1] == 0x8b && result[0 + 2] == 0x08 && result[0 + 3] == 0x00))
            {
                result = GzipUtils.DeCompress(result, 0, result.Length);
            }

            callback(result);
        }

    }
}