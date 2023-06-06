namespace JingNeng_MES.Model
{
    public enum TesterCommand
    {
        None,
        /// <summary>
        /// 连接socket服务
        /// </summary>
        REG_APP_NAME,
        /// <summary>
        /// 测试服务
        /// </summary>
        Test_Connection,
        /// <summary>
        /// MES定时向测试机获取各bin数量
        /// </summary>
        Each_Bin_TestCount_REQ,
        /// <summary>
        /// 第一步:校正分光机
        /// 校正成功则响应,校正失败响应
        /// </summary>
        STANDARD_DATA,
        /// <summary>
        /// 第二步:样品校准
        /// 成功则响应,失败则响应
        /// </summary>
        LOT_START,
        /// <summary>
        /// 第三步:结束作业
        /// 成功则响应,失败则响应
        /// </summary>
        LOT_END
    }
}