using JingNeng_MES.Model;
using JingNeng_MES.Service;
using JingNeng_MES.ViewModel;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JingNeng_MES.Tester
{
    internal class MesHelper
    {
        /// <summary>
        /// 分光程序名
        /// </summary>
        public const string PPID = "PPID=";
        /// <summary>
        /// 程序名完整路径
        /// </summary>
        public const string FOLDER = "FOLDER=";

        /// <summary>
        /// 作业批次ID/流程卡号
        /// </summary>
        public const string LOTID = "LOTID=";
        /// <summary>
        /// 文件名 组合包含前缀D
        /// </summary>
        public const string FILENAME = "FILENAME=D";



 

        private void  OnRequestHandle(object sender, MesEventArgs e)
        {
            LoggerHelper._.Info($"Handle:{e.Handle}:{e.TesterCommand}=>{e.StringData}");
            var jns = sender as JingNengServer;
            if (jns == null) throw new ArgumentNullException(nameof(jns));

            switch (e.TesterCommand)
            {
                case TesterCommand.REG_APP_NAME:
                    jns.Send(e.Handle, System.Text.Encoding.Default.GetBytes("REG_APP_NAME:RC=OK"));
                    break;

                case TesterCommand.Test_Connection:
                    jns.Send(e.Handle, System.Text.Encoding.Default.GetBytes("Test=OK;"));
                    break;

                case TesterCommand.Each_Bin_TestCount_REQ:
                 

                    break;

                case TesterCommand.STANDARD_DATA:
                    
                    break;

                case TesterCommand.LOT_START:
                   
                    break;

                case TesterCommand.LOT_END:
                  
                    break;
            }
        }
    }
}
