using PilotGaea.Serialize;
using PilotGaea.TMPClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using System.Threading;
using System.Diagnostics;

namespace CheckServerWorking
{
    class Program
    {
        static int second =60;//每x秒檢測一次
        static string url = "http://127.0.0.1:8080/docmd?cmd=";

        static void Main(string[] args)
        {
            Timer t = new Timer(TimerCallback, null, 0, (second * 1000));
            Console.Title = "CheckServerWorking";
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("檢測Server是否啟動...");
            Console.SetWindowSize(30, 5);
            Console.ReadLine();
        }
        private static void TimerCallback(Object o)
        {
            //Console.WriteLine("In TimerCallback: " + DateTime.Now);
            if (!CheckIsAlive()) StartServer();
            GC.Collect();
        }
        private static bool CheckIsAlive()
        {
            CMapDocument cmDoc = new CMapDocument();
            VarStruct inputnum = new VarStruct();
            VarStruct outputnum = new VarStruct();

            bool connect = false;
            string doCmdUrl = url + "isLive";
            connect = cmDoc.DoCommand(doCmdUrl, ref inputnum, ref outputnum);

            return connect;
        }
        private static void StartServer()
        {
            //[Server異常]
            //WerFault.exe 以及PGMapServer.exe

            string PrcocessName = "PGMapServer";
            string target = @"C:\Program Files\PilotGaea\TileMap\PGMapServer.exe";
            Process[] MyProcess = Process.GetProcessesByName(PrcocessName);
            if (MyProcess.Length > 0)
            {
                MyProcess[0].Kill(); //關閉執行中的程式
                MyProcess[0].WaitForExit();
            }
            string SystemErrorName = "WerFault";
            Process[] SystemErrorProcess = Process.GetProcessesByName(SystemErrorName);
            if (SystemErrorProcess.Length > 0)
            {
                SystemErrorProcess[0].Kill(); //關閉執行中的程式
                SystemErrorProcess[0].WaitForExit();
            }

            Process.Start(target, "-s 1");
        }
    }
}
