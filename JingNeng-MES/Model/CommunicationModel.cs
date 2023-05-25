using System;
using System.Collections.Generic;
using System.Linq;
using JingNeng_MES.ViewModel;

namespace JingNeng_MES.Model
{
    public class CommunicationModel
    {
       
        public CommunicationModel()
        {
          


        }

        public TesterCommand Request(string command)
        {
            var r = command.ToCmd();
            return r;
        }

        internal  IEnumerable<BinContInfo> SplitBin(string stringData)
        {
            var bins = stringData.Split(':').LastOrDefault().Split(';').Select(a =>
                new BinContInfo(a.Split('=').FirstOrDefault(), a.Split('=').LastOrDefault()));

            return bins;


        }

        public string CountSynchronization()
        {
            return TesterCommand.Each_Bin_TestCount_REQ.ToString();

        }
    }
}