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
    /// <summary>
    /// 登录
    /// </summary>
    public class Step1010 : CaseStep
    {
        public Response1010Pack responsePack = null;
        Request1010Pack req;
        private string name = "";
        private string pwd = "";
        protected override void SetUrlElement()
        {
            readAuthory();
            req = new Request1010Pack();
            req.the3rdUserID = 1;
            req.actionID = (int)Request1010Pack.EnumOptType.use_enterNum;
            req.num = 1;
            req.strThe3rdUserID = "";
            req.typeUser = "YYS_CP360";
            req.version = "1.09";
            if (isUseConfigData())
            {
                setConfigData(req);
            }
            byte[] data = ProtoBufUtils.Serialize(req);
            netWriter.SetBodyData(data);
        }

        protected override bool DecodePacket(MessageStructure reader, MessageHead head)
        {
            responsePack = ProtoBufUtils.Deserialize<Response1010Pack>(netReader.Buffer);
            string responseDataInfo = "";
            responseDataInfo = "request :" + Game.Utils.JsonHelper.prettyJson<Request1010Pack>(req) + "\n";
            responseDataInfo += "response:" + Game.Utils.JsonHelper.prettyJson<Response1010Pack>(responsePack) + "\n";
            DecodePacketInfo = responseDataInfo;
            int childStepId = getChild(1010);
            System.Console.WriteLine("childStepID:" + childStepId);
            if (childStepId > 0)
            {
                System.Collections.Generic.Dictionary<string, string> dic = new System.Collections.Generic.Dictionary<string, string>();
                /*
                  req.UserID = GetParamsData("UserID", req.UserID);
                req.identify = GetParamsData("identify", req.identify);
                req.version = GetParamsData("version", req.version);
                req.the3rdUserID = GetParamsData("the3rdUserID", req.the3rdUserID);
                req.happyPoint = GetParamsData("happyPoint", req.happyPoint);
                req.Rate = GetParamsData("Rate", req.Rate);
                req.index = GetParamsData("index", req.index);
                req.strThe3rdUserID = GetParamsData("strThe3rdUserID", req.strThe3rdUserID);
                req.typeUser = GetParamsData("typeUser", req.typeUser);
                 */
                dic.Add("UserID", req.UserID.ToString());
                dic.Add("index", responsePack.index.ToString());
                dic.Add("the3rdUserID", req.the3rdUserID.ToString());
                dic.Add("strThe3rdUserID", req.strThe3rdUserID);
               SetChildStep(childStepId.ToString(), _setting,dic);
            }
            return true;
        }
        void createAuthory()
        {

        }
        void readAuthory()
        {
            try
            {
                System.IO.StreamReader stream = new System.IO.StreamReader(".//authory.txt");
                string line = "";
                while ((line = stream.ReadLine()) != null)
                {
                    // TODO
                    string[] words = line.Split(',');
                    name = words[0];
                    pwd = words[1];
                }
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine("Step2000:" + e.Message);
            }

            System.Console.WriteLine("name:" + name + "\npwd:" + pwd);
        }
    }
}