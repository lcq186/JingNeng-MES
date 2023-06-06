using JingNeng_MES.Service;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using JingNeng_MES.Model;
using System.Windows.Threading;
using System.Text;

namespace JingNeng_MES
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
  

        public App()
        {

            Console.WriteLine("start");
            LoggerHelper._.TraceMsg("start");

            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
            //UI线程未捕获异常处理事件
            this.DispatcherUnhandledException += new DispatcherUnhandledExceptionEventHandler(App_DispatcherUnhandledException);
            //Task线程内未捕获异常处理事件
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

            //非UI线程未捕获异常处理事件
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);


        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            StringBuilder sbEx = new StringBuilder();
            if (e.IsTerminating)
            {
                sbEx.Append("程序发生致命错误，将终止，请联系运营商！\n");
            }
            sbEx.Append("捕获未处理异常：");
            if (e.ExceptionObject is Exception)
            {
                sbEx.Append(((Exception)e.ExceptionObject).Message);
            }
            else
            {
                sbEx.Append(e.ExceptionObject);
            }
            MessageBox.Show(sbEx.ToString());

        }

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            //task线程内未处理捕获
            MessageBox.Show("捕获线程内未处理异常：" + e.Exception.Message);
            e.SetObserved();//设置该异常已察觉（这样处理后就不会引起程序崩溃）
        }

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            try
            {
                e.Handled = true;
                if (e.Exception.InnerException == null)
                {
                    LoggerHelper._.Error( ("（1）发生了一个错误！请联系开发人员！" + Environment.NewLine
                                                          + "（2）错误源：" + e.Exception.Source + Environment.NewLine
                                                          + "（3）详细信息：" + e.Exception.Message + Environment.NewLine));
                    //+ "（4）报错区域：" + e.Exception.StackTrace);
                }
                else
                {
                    LoggerHelper._.Error("（1）发生了一个错误！请联系开发人员！" + Environment.NewLine
                                                               + "（2）错误源：" + e.Exception.InnerException.Source + Environment.NewLine
                                                               + "（3）错误信息：" + e.Exception.Message + Environment.NewLine
                                                               + "（4）详细信息：" + e.Exception.InnerException.Message + Environment.NewLine
                                                               + "（5）报错区域：" + e.Exception.InnerException.StackTrace);
                }

            }
            catch (Exception e2)
            {
                //此时程序出现严重异常，将强制结束退出
                MessageBox.Show("程序发生致命错误，将终止，请联系运营商！");
            }
        }
    }
}
