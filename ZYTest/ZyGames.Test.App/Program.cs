﻿/****************************************************************************
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
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZyGames.Framework.Common.Log;
using ZyGames.Framework.Script;
using ZyGames.Test.Net;

namespace ZyGames.Test.App
{
  
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                autoTest tasks = new autoTest();
                ScriptEngines.AddReferencedAssembly("ZyGames.Test.dll");
                ScriptEngines.AddReferencedAssembly("GameRanking.Pack.dll");
                ScriptEngines.AddReferencedAssembly("Protocol.dll");
                ScriptEngines.AddReferencedAssembly("Utils.dll");
                ScriptEngines.Initialize();
                tasks.RunTasks();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                TraceLog.WriteError("Main error:{0}", ex);
                Console.ReadKey();
            }
        }

    }
}
