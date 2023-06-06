using ControlzEx.Standard;

namespace JingNeng_MES.ViewModel
{
    public class BinContInfo
    {
        private string v1;
        private string v2;

        public BinContInfo(int bin, int count)
        {
            BinNo = bin.ToString();
            BinCount = count.ToString();

        }

        public BinContInfo(string bin, string count)
        {
            BinNo = bin;
            BinCount = count;

        }

        public string BinNo { get; set; }
        public string BinCount { get; set; }

    }
}