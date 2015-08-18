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
using System.Collections.Generic;

namespace ZyGames.Quanmin.Test.Case
{
    /// <summary>
    /// 创角
    /// </summary>
    public class Step1008 : CaseStep
    {
        Response1008Pack responsePack;
        Request1008Pack req;
        protected override void SetUrlElement()
        {
            int id = -357016106;
            req = new Request1008Pack();
            req.UserID = 1;
            req.identify = "";
            req.version = "1.09";
            req.the3rdUserID = 1;
            req.happyPoint = 1;
            req.Rate = 1;
            req.Distance = 1;
            req.index = 1;
            req.strThe3rdUserID = "";
            req.typeUser = "YYS_CP360";
            if (isUseConfigData())
            {
                setConfigData(req);
            }
            byte[] data = ProtoBufUtils.Serialize(req);
            netWriter.SetBodyData(data);
        }

        protected override bool DecodePacket(MessageStructure reader, MessageHead head)
        {
            responsePack = ProtoBufUtils.Deserialize<Response1008Pack>(netReader.Buffer);
            string responseDataInfo = "";
            responseDataInfo = "request :" + Game.Utils.JsonHelper.prettyJson<Request1008Pack>(req) + "\n";
            responseDataInfo += "response:" + Game.Utils.JsonHelper.prettyJson<Response1008Pack>(responsePack) + "\n";
            DecodePacketInfo = responseDataInfo;
            int chidId = getChild(1008);
            if(chidId>0)
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("the3rdUserID",req.the3rdUserID.ToString());
                SetChildStep(chidId.ToString(), _setting, dic);
            }
            return true;
        }

    }
}