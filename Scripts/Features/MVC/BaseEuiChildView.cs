using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ClientCore
{
    /**
     * 
     * ViewChild基类
     */
    class BaseEuiChildView : BaseEuiView
    {
        /**
         * 构造函
         * @param $controller 所属模块
         * @param $parent 父级
         */
        public BaseEuiChildView(BaseController controller, LayoutUI lay, GameObject obj, ViewDef key)
        {
            this.key = key;

            //this.percentHeight = 100;
            //this.percentWidth = 100;
        }

        /**
         * 打开子页签
         */
        public void onChildGroupShow()
        {

        }

        /**
        * 关闭子页签
        */
        public void onChildGroupHide()
        {

        }
    }
}