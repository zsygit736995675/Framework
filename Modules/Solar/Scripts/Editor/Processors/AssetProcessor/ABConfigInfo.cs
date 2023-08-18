/***************************************************************
 * (c) copyright 2021 - 2025, Solar.Game
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  ABConfigInfo.cs
 * author:    taoye
 * created:   2021/2/17
 * descrip:   ABConfig信息
 ***************************************************************/
namespace Solar.Editor
{
    public class ABConfigInfo
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="path">路径</param>
        /// <param name="packageMeasureType">AB打包方式</param>
        /// <param name="rename">AB重命名</param>
        /// <param name="groupName">分组名称</param>
        /// <param name="isIncreaserGroup">是否为增量分组</param>
        public ABConfigInfo(int id, string path, int packageMeasureType, string rename, string groupName, bool isIncreaserGroup)
        {
            ID = id;
            Path = path;
            PackageMeasureType = packageMeasureType;
            Rename = rename;
            GroupName = groupName;
            IsIncreaserGroup = isIncreaserGroup;
        }

        /// <summary>
        /// ID编码
        /// </summary>
        public int ID;

        /// <summary>
        /// 路径
        /// </summary>
        public string Path;

        /// <summary>
        /// AB打包方式
        /// </summary>
        public int PackageMeasureType;

        /// <summary>
        /// AB重命名
        /// </summary>
        public string Rename;

        /// <summary>
        /// 分组名称
        /// </summary>
        public string GroupName;

        /// <summary>
        /// 是否为增量分组
        /// </summary>
        public bool IsIncreaserGroup;

    }
}
