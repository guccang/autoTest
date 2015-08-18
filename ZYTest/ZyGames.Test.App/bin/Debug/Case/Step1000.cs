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

using ZyGames.Framework.Common;
using ZyGames.Framework.RPC.IO;
using ZyGames.Test;
using GameRanking.Pack;
using ZyGames.Framework.Common.Serialization;
using ZyGames.Framework.Common.Configuration;

namespace ZyGames.Quanmin.Test.Case
{


    /*
     * Class:   Step1000
     * Desc:    上传排行榜数据
     * Author：	guccang
     * Date：	2015-07-21 15:54:00
     */
    /// <summary>
    /// Step1000 Document
    /// </summary>
    
    public class Step1000 : CaseStep
    {
        public ResponsePack responsePack = null;
        Request1000Pack req = null;
        protected override void SetUrlElement()
        {
          req = new Request1000Pack();
          req.UserName = "Computer_" + indentify;
          req.Score = RandomUtils.GetRandom(1, 1000);
          req.UserID = -10;// RandomUtils.GetRandom(1, 1000);
          req.version = "1.08";
          //System.Console.WriteLine(" setp urlelement 1000 ");
          req.Identify = req.UserName;
          if(isUseConfigData())
          {
              setConfigData(req);
          }
          byte[] data = ProtoBufUtils.Serialize(req);
          netWriter.SetBodyData(data);
        }

        protected override bool DecodePacket(MessageStructure reader, MessageHead head)
        {
             responsePack = ProtoBufUtils.Deserialize<ResponsePack>(netReader.Buffer);
             string responseDataInfo = "";
             responseDataInfo  = "request :" + Game.Utils.JsonHelper.prettyJson<Request1000Pack>(req) + "\n";
             responseDataInfo += "response:" + Game.Utils.JsonHelper.prettyJson<ResponsePack>(responsePack) + "\n";
             DecodePacketInfo = responseDataInfo;
             int childStepId = getChild(1000);
             if(childStepId>0)
             {
                System.Collections.Generic.Dictionary<string,string> dic = new System.Collections.Generic.Dictionary<string,string>();
                dic.Add("UserID",responsePack.UserID.ToString()) ;
                SetChildStep(childStepId.ToString(),_setting,dic);
             }
             return true;
        }

    }
}