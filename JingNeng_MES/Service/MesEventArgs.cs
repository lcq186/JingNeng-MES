using System;
using JingNeng_MES.Model;

namespace JingNeng_MES.Service
{
    public class MesEventArgs : EventArgs
    {
        public MesEventArgs(IntPtr handle, string receiveStr)
        {

            this.StringData = receiveStr;
            Handle = handle;
            TesterCommand = receiveStr.ToCmd();
        }

        public IntPtr Handle { get; }
        public TesterCommand TesterCommand { get; set; }
        public string StringData { get; set; }
        public string PPID { get; set; }
        public string FOLDER { get; set; }

        public string FILENAME { get; set; }

        public string REASON { get; }

        public void Analysis(string str)
        {
          


        }
     


    }
}