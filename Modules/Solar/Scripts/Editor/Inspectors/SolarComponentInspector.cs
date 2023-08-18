/***************************************************************
 * (c) copyright 2021 - 2025, Solar.Game
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  SolarComponentInspector.cs
 * author:    taoye
 * created:   2020/12/16
 * descrip:   编辑器面板定制基础类
 ***************************************************************/

using UnityEditor;

namespace Solar.Editor
{
    public abstract class SolarComponentInspector : UnityEditor.Editor
    {
        private bool m_IsCompiling = false;

        /// <summary>
        /// 绘制事件
        /// </summary>
        public override void OnInspectorGUI()
        {
            if (m_IsCompiling && !EditorApplication.isCompiling)
            {
                m_IsCompiling = false;
                OnCompileComplete();
            }
            else if (!m_IsCompiling && EditorApplication.isCompiling)
            {
                m_IsCompiling = true;
                OnCompileStart();
            }

            Repaint();
        }

        /// <summary>
        /// 编译开始事件
        /// </summary>
        protected virtual void OnCompileStart()
        {
        }

        /// <summary>
        /// 编译完成事件
        /// </summary>
        protected virtual void OnCompileComplete()
        {
        }

        protected bool IsPrefabInHierarchy(UnityEngine.Object obj)
        {
            if (obj == null)
            {
                return false;
            }

            return PrefabUtility.GetPrefabAssetType(obj) != PrefabAssetType.Regular;
        }
    }
}
