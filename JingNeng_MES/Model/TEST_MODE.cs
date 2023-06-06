using System;

namespace JingNeng_MES.Model
{
    [Flags]
    public enum TEST_MODE
    {
        None = 0,
        /// <summary>
        /// (正常生产)
        /// </summary>
        NormalRun,
        /// <summary>
        /// 样品校准）
        /// </summary>
        MasterTest,
        /// <summary>
        ///（基准测试）
        /// </summary>
        BenchmarkTest,
        
    }
}