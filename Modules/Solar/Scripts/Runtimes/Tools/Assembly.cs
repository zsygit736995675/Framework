/***************************************************************
 * (c) copyright 2021 - 2025, Solar.Game
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  Assembly.cs
 * author:    taoye
 * created:   2020/12/16
 * descrip:   程序集相关的实用函数
 ***************************************************************/
using System;
using System.Collections.Generic;

namespace Solar.Runtime
{
    public static class Assembly
    {
        /// <summary>
        /// 所有的程序集集合
        /// </summary>
        private static readonly System.Reflection.Assembly[] s_Assemblies = null;

        /// <summary>
        /// 缓存类型
        /// <名称, 类型>
        /// 将从程序集中已经获取过的类型缓存一下，方便下次再次获取
        /// </summary>
        private static readonly Dictionary<string, Type> s_CachedTypes = new Dictionary<string, Type>();

        static Assembly()
        {
            s_Assemblies = AppDomain.CurrentDomain.GetAssemblies();
        }

        /// <summary>
        /// 获取已加载的程序集。
        /// </summary>
        /// <returns>已加载的程序集。</returns>
        public static System.Reflection.Assembly[] GetAssemblies()
        {
            return s_Assemblies;
        }

        /// <summary>
        /// 获取已加载的程序集中的所有类型。
        /// </summary>
        /// <returns>已加载的程序集中的所有类型。</returns>
        public static Type[] GetTypes()
        {
            List<Type> results = new List<Type>();
            foreach (System.Reflection.Assembly assembly in s_Assemblies)
            {
                results.AddRange(assembly.GetTypes());
            }

            return results.ToArray();
        }

        /// <summary>
        /// 获取已加载的程序集中的所有类型。
        /// </summary>
        /// <param name="results">已加载的程序集中的所有类型。</param>
        public static void GetTypes(List<Type> results)
        {
            if (results == null)
            {
                throw new Exception("Results 无效。");
            }

            results.Clear();
            foreach (System.Reflection.Assembly assembly in s_Assemblies)
            {
                results.AddRange(assembly.GetTypes());
            }
        }

        /// <summary>
        /// 获取已加载的程序集中的指定类型。
        /// </summary>
        /// <param name="typeFullName">要获取的类型全名（包括完整的名字空间）。</param>
        /// <returns>已加载的程序集中的指定类型。</returns>
        public static Type GetType(string typeFullName)
        {
            if (string.IsNullOrEmpty(typeFullName))
            {
                throw new Exception("Type fullName 无效。");
            }

            Type type = null;
            if (s_CachedTypes.TryGetValue(typeFullName, out type))
            {
                return type;
            }

            type = Type.GetType(typeFullName);
            if (type != null)
            {
                s_CachedTypes.Add(typeFullName, type);
                return type;
            }

            foreach (System.Reflection.Assembly assembly in s_Assemblies)
            {
                type = Type.GetType($"{typeFullName}, {assembly.FullName}");
                if (type != null)
                {
                    s_CachedTypes.Add(typeFullName, type);
                    return type;
                }
            }

            return null;
        }

    }
}


