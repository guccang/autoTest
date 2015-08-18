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
    public class Step1005 : CaseStep
    {

        Response1005Pack responsePack;
        Request1005Pack req;
        protected override void SetUrlElement()
        {
            req = new Request1005Pack();
            req.token = "06433cd6e21d45f79a95c8e2ac9027c1-9d56aacb28bda0b9457bf9079bc715d9-20141222181929-ce629633cfaaf2bb1681719fc9b1f1ac-3b2927c8419cda02063bb7180deeb67a-f7ee2d74e5adcb026d457c2f7caee153";
            req.typeUser = "YYS_CP360";
            req.version = "1.08";
            req.UserID = 1160518;
            if(isUseConfigData())
            {
                setConfigData(req);
            }
            byte[] data = ProtoBufUtils.Serialize(req);
            netWriter.SetBodyData(data);
        }

        protected override bool DecodePacket(MessageStructure reader, MessageHead head)
        {
            responsePack = ProtoBufUtils.Deserialize<Response1005Pack>(netReader.Buffer);
            string responseDataInfo = "";
            responseDataInfo = "request :" + Game.Utils.JsonHelper.prettyJson<Request1005Pack>(req) + "\n";
            responseDataInfo += "response:" + Game.Utils.JsonHelper.prettyJson<Response1005Pack>(responsePack) + "\n";
            DecodePacketInfo = responseDataInfo;
            int childStepId = getChild(1005);
            if (childStepId > 0)
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("the3rdUserID", responsePack.the3rdUserId.ToString());
                dic.Add("strThe3rdUserID", dic["the3rdUserID"]);
                SetChildStep(childStepId.ToString(), _setting,dic);
            }
            return true;
        }

    }
}