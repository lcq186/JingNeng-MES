using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using JingNeng_MES.Helper;
using JingNeng_MES.ViewModel;
using MahApps.Metro.Controls;

using NLog.Config;
using NLog.Targets.Wrappers;
using NLog;

namespace JingNeng_MES
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : MetroWindow
    {

        public MainWindow()
        {
          

            InitializeComponent();
           
            this.Loaded += MainWindow_Loaded;
            StartButton.Click += StartButton_Click;
        }

     

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                // target.Layout = "${longdate:useUTC=true}|${level:uppercase=true}|${logger}::${message}";

                var target = new WpfRichTextBoxTarget
                {
                    Name = "RichText",

                    Layout = "${time} ${uppercase:${level}} ${message}",//"[${longdate:useUTC=false}] :: [${level:uppercase=true}] :: ${logger}:${callsite} :: ${message} ${exception:innerFormat=tostring:maxInnerExceptionLevel=10:separator=,:format=tostring}",
                    ControlName = "RichTextBox",
                    FormName = GetType().Name,
                    AutoScroll = true,
                    MaxLines = 1000,
                    UseDefaultRowColoringRules = true,
                };
                var asyncWrapper = new AsyncTargetWrapper { Name = "RichTextAsync", WrappedTarget = target };

                LogManager.Configuration.AddTarget(asyncWrapper.Name, asyncWrapper);
                LogManager.Configuration.LoggingRules.Insert(0, new LoggingRule("*", LogLevel.Trace, asyncWrapper));
                LogManager.ReconfigExistingLoggers();

            });
            this.DataContext = new MainViewModel();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {

             

           
        }
    }
}
