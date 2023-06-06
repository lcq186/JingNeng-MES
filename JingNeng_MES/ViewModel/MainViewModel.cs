using JingNeng_MES.Model;
using JingNeng_MES.Service;

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
 
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.Net;
using System.Net.Sockets;

namespace JingNeng_MES.ViewModel
{
    public class MainViewModel : ObservableObject
    {
        private IEnumerable<BinContInfo> _binContInfo;
        private CommunicationModel _communicationModel;

        private JingNengServer _server;
        private TEST_MODE _testMode;

        public MainViewModel()
        {

            _server = new JingNengServer();
         

            string Port = ConfigurationManager.AppSettings["Port"] ?? "8848";

            var host = Dns.GetHostEntry(Dns.GetHostName()).AddressList.FirstOrDefault(a=>a.AddressFamily== AddressFamily.InterNetwork).ToString();
            string ServerIP = ConfigurationManager.AppSettings["ServerIP"] ?? host;
            var xx = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
            _server.Bind(ServerIP,  int.Parse(Port));
            _server.Listen(10);
            _server.StartAsync();
            _server.OnRequestHandle += _server_OnRequestHandle;
            _communicationModel = new CommunicationModel();
            _binContInfo = new List<BinContInfo>();
            //for (int i = 0; i < 100; i++)
            //{

            //    _binContInfo.Add(new BinContInfo(i, i));
            //}
        }

        public IEnumerable<BinContInfo> BinContInfo
        {
            get => _binContInfo;
            set => SetProperty(ref _binContInfo, value);
        }

        public ICommand CountSynchronization => OnCountSynchronizationCommand();
        public ICommand StartWork => OnStartWorkCommand();

        public int BinNo { get; set; }
        public bool IsSingleBin { get; set; }
        public bool BinClear { get; set; }


        public TEST_MODE TestMode
        {
            get => _testMode;
            set => SetProperty(ref _testMode, value);
        }

        private void _server_OnRequestHandle(object sender, MesEventArgs e)
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
                    var x = _communicationModel.SplitBin(e.StringData);
                    BinContInfo = x;

                    break;

                case TesterCommand.STANDARD_DATA:

                    break;

                case TesterCommand.LOT_START:

                    break;

                case TesterCommand.LOT_END:

                    break;
            }
        }

        private ICommand OnCountSynchronizationCommand()
        {
            return new RelayCommand(() =>
            {
                try
                {
                    var cmd = _communicationModel.CountSynchronization();

                    _server.Send(cmd);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }

            });
        }
        private ICommand OnStartWorkCommand()
        {
            return new AsyncCommand(async () =>
            {
                await Task.Run(() =>
                {
                    var x = $"LOT_START;TEST_MODE={(int)TestMode}";


                    switch (TestMode)
                    {
                        case TEST_MODE.None:
                            break;
                        case TEST_MODE.NormalRun:


                            break;
                        case TEST_MODE.MasterTest:
                            x = "STANDARD_DATA;" +
                                      "PPID=50102.TCOB;" +
                                      "FOLDER=D:\\eTester\\Program\\JNENG.PRG;" +
                                      "VF1=2,3.205,3.24,3.18,3.23,3.168,3.178,3.187,3.188,3.19,3.245;" +
                                      "KY1=1,380.3,385,388,381.1,385.8,380.3,380.3,377.2,374.1,389.4;" +
                                      "LMX1=2,0.3227,0.3212,0.3238,0.3211,0.3235,0.3208,0.3215,0.3192,0.3227,0.3228";
                            _server.Send(x);
                            //x = "LOT_START;" +
                            //    "PPID=50102.TCOB;" +
                            //    "FOLDER=D:\\eTester\\Program\\JNENG.PRG;" +
                            //    "LOTID=G-230510001-006;" +
                            //    "OPERATORID=EGZ100888;" +
                            //    "SAVE_PATH=" +
                            //    "D:\\DATA\\FGJZ\\G-230510001\\CTS007_FGJZ_G-230510001-006_236D5EE8;" +
                            //    "TEST_MODE=2;" +
                            //    "INTERFACE=OFF;" +
                            //    "TEST_TOTALNUMBER_CLEAR=ON;";
                            //_server.Send(x);

                            break;
                        case TEST_MODE.BenchmarkTest:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    x = $"LOT_START;" +
                        "PPID=50102.TCOB;" +
                        "FOLDER=D:\\eTester\\Program\\JNENG.PRG;" +
                        "LOTID=G-230510001-006;" +
                        "OPERATORID=EGZ100888;" +
                        "SAVE_PATH=" +
                        "D:\\DATA\\FGJZ\\G-230510001\\CTS007_FGJZ_G-230510001-006_236D5EE8;" +
                        $"TEST_MODE={(int)TestMode};" +
                        $"INTERFACE={(IsSingleBin? "ON" :"OFF")};" +
                        $"BIN_OUT_SET={BinNo};" +
                        $"TEST_TOTALNUMBER_CLEAR={(BinClear ? "ON" : "OFF")};;";
                    _server.Send(x);







                });


            });
        }

  

        public ICommand EndWork => new RelayCommand(PerformEndWork);

        private void PerformEndWork()
        {
            _server.Send("LOT_END");
        }




      

        public ICommand ResetWork => new RelayCommand(PerformResetWork);

        private void PerformResetWork()
        {
        }
    }
}
