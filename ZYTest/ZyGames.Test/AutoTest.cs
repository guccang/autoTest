
/****************************************************************************
    File:		AutoTest.cs
    Desc:		Automated testing
    Date:		2015-7-27
    Author:		guccang
    URL:		http://guccang.github.io
    Email:		guccang@126.com
****************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using ZyGames.Framework.Common.Log;
using System.IO;

namespace ZyGames.Test
{
    /*
     * Class:   AutoTest
     * Desc:   	自动化测试类
     *          读取测试用例(*.txt) 运行。
     * Author：	guccang
     * Date：	2015-7-21 11:11:11
     */
    /// <summary>
    /// AutoTest Document
    /// </summary>


    public class autoTest
    {
        string FilePath = ".\\AutoTask\\";
        public autoTest() { }
        public void Menu()
        {
            string uiStr =
@"///////////////////////////////////////////////////////////////////////////
        cmd01: menu
        cmd02: run
        cmd03: runAll
        cmd04: q/exit
        cmd05: clear
///////////////////////////////////////////////////////////////////////////";
            Console.WriteLine(uiStr);
        }

        class autoTaskData
        {
            public TaskSetting setting { get; set; }
            public string taskName { get; set; }
            public string taskDes { get; set; }
            public int dely { get; set; }
            public autoTaskData()
            {
                setting = new TaskSetting();
            }
        }

        void parseTaskName(string val,autoTaskData atd)
        {
            atd.taskName = val;
            atd.setting.TaskName = val;
        }

        void parseTaskDes(string val,autoTaskData atd)
        {
            atd.taskDes = val;
            atd.setting.TaskDes = val;
        }

        void parseDely(string val,autoTaskData atd)
        {
            atd.dely = int.Parse(val);
        }

        void parseThreadNum(string val,autoTaskData atd)
        {
            atd.setting.ThreadNum = int.Parse(val); 
        }

        void parseRunTimes(string val,autoTaskData atd)
        {
            atd.setting.Runtimes = int.Parse(val);
        }

        void parseChild(string val,autoTaskData atd)
        {
            string[] valSplits = val.Split('&');
            for (int i = 0; i < valSplits.Length; ++i)
            {
                if (valSplits[i] == "") continue;
                string[] words = valSplits[i].Split(':');
                atd.setting.childStepDic.Add(int.Parse(words[0]), int.Parse(words[1]));
            }
        }

        void parseCase(string val,autoTaskData atd)
        {
            atd.setting.CaseStepList = new List<string>(val.Split(','));

        }

        void parseParms(string line,autoTaskData atd)
        {
            atd.setting.Parms.Add(line);
            string[] parmsSplits = line.Split('=');
            string key = parmsSplits[0];
            string val = parmsSplits[1];
            string[] valSplits = val.Split('&');
            Dictionary<string, string> data = new Dictionary<string, string>();
            for (int i = 0; i < valSplits.Length; ++i)
            {
                if (valSplits[i] == "") continue;
                string[] words = valSplits[i].Split(':');
                data.Add(words[0], words[1]);
            }
            if(line.Contains("Parms"))
            {
                string[] keyworld = key.Split('_');
                string id  = keyworld[1];
                atd.setting.StepParms.Add(id,data);
            }
        }
        void ParseLine(string line,autoTaskData atd)
        {
            string[] worlds = line.Split('=');
            string key = worlds[0];
            string val = worlds[1];
            switch (key)
            {
                case "taskName": parseTaskName(val,atd); break;
                case "taskDes":  parseTaskDes(val, atd); break;
                case "dely":     parseDely(val,atd); break;
                case "ThreadNum": parseThreadNum(val, atd); break;
                case "RunTimes": parseRunTimes(val, atd); break;
                case "child": parseChild(val,atd); break;
                case "case": parseCase(val, atd); break;
                default: parseParms(line, atd); break;
            }
        }

        List<autoTaskData> getTasks(string str)
        {
                string begin = "[begin]";
                string end = "[end]";
                System.IO.StreamReader stream = new System.IO.StreamReader(str);
                string line = "";
                autoTaskData atd = null;
                List<autoTaskData> atdLST = new List<autoTaskData>();
                while ((line = stream.ReadLine()) != null)
                {
                    // TODO
                    if (begin == line)
                    {
                        atd = new autoTaskData();
                        continue;
                    }
                    if (end == line)
                    {
                        atdLST.Add(atd);
                        continue;
                    }
                    if (line == "")
                    {
                        continue;
                    }
                    ParseLine(line,atd);
                }
                stream.Close();
                return atdLST;
        }

        void runAll()
        {
            DirectoryInfo folder = new DirectoryInfo(FilePath);

            List<string> names = new List<string>();
            foreach (FileInfo file in folder.GetFiles())
            {
                string name = file.FullName;
                names.Add(name);
            }

            foreach(var f in names)
            {
                Console.WriteLine(f + " begin");
                doRun(f);
                Console.WriteLine(f + " end");
            }
        }

        void doRun(string fileName)
        {
            try
            {
                List<autoTaskData> tasks = getTasks(fileName);
                string result = "";
                foreach (var v in tasks)
                {
                    result += ThreadManager.RunTest(v.setting);
                    Console.WriteLine("sleep" + v.dely);
                    Thread.Sleep(v.dely);
                }
                Console.WriteLine(result);
                writeLog(fileName, result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message+":"+fileName);
            }
          
        }

        void writeLog(string name, string info)
        {
            name = name.Substring(name.LastIndexOf("\\") + 1);
            name = name.Remove(name.LastIndexOf('.'));
            string fileName = ".//AutoTask//log//" + name + "-" +DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fff") + ".txt";
            StreamWriter sw = new StreamWriter(fileName);
            sw.Write(info);
            sw.Close();
        }
        void run(string parm)
        {
            string fileName = parm;
            if(string.IsNullOrEmpty(parm))
            {
                Console.Write("input task filename :");
                fileName = Console.ReadLine();
            }
            fileName = FilePath + fileName + ".atd";
            doRun(fileName);
        }
        void clear()
        {
            Console.Clear();
        }

        public void RunTasks()
        {
            try
            {
                while (true)
                {
                    Menu();
                    Console.Write("input cmd:");
                    string cmd = Console.ReadLine();
                    string[] parsCmd = cmd.Split(' ');
                    string parm = "";
                    if (parsCmd.Length == 2)
                    {
                        cmd  = parsCmd[0];
                        parm = parsCmd[1];
                    }
                    switch (cmd)
                    {
                        case "meun": Menu(); break;
                        case "run": run(parm); break;
                        case "runAll": runAll(); break;
                        case "clear": clear(); break;
                        case "exit": break;
                        case "q": break;
                    }
                    if (cmd == "q" || cmd == "exit")
                    {
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception:" + e.Message);
            }
            Console.WriteLine("Press any Key to Exit");
            Console.ReadKey();
        }
    }
}
