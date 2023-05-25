using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JingNeng_MES.Model;
using static JingNeng_MES.Model.CommunicationModel;

namespace JingNeng_MES
{
    public static class CommandHelper
    {
        public static TesterCommand ToCmd(this string command)
        {

            foreach (var item in Enum.GetNames(typeof(TesterCommand)))
            {
                if (command.StartsWith(item))
                {
                    Enum.TryParse<TesterCommand>(item, true, out var result);

                    return result;
                }
            }
            return default;
        }
    }
}
