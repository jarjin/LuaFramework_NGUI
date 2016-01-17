using UnityEngine;
using LuaInterface;
using System.Collections;
using System.Collections.Generic;
using System;

namespace LuaFramework {
    public class LuaBehaviour : Base {
        protected static bool initialize = false;

        private string data = null;
        private AssetBundle bundle = null;
        private List<LuaFunction> buttons = new List<LuaFunction>();

        protected void Awake() {
            CallMethod("Awake", gameObject);
        }

        protected void Start() {
            CallMethod("Start");
        }

        protected void OnClick() {
            CallMethod("OnClick");
        }

        protected void OnClickEvent(GameObject go) {
            CallMethod("OnClick", go);
        }

        /// <summary>
        /// 初始化面板
        /// </summary>
        public void OnInit(AssetBundle bundle, string text = null) {
            this.data = text;   //初始化附加参数
            this.bundle = bundle; //初始化
            Debug.LogWarning("OnInit---->>>" + name + " text:>" + text);
        }

        /// <summary>
        /// 获取一个GameObject资源
        /// </summary>
        /// <param name="name"></param>
        public GameObject LoadAsset(string name) {
            if (bundle == null) return null;
            return Util.LoadAsset(bundle, name);
        }

        /// <summary>
        /// 添加单击事件
        /// </summary>
        public void AddClick(GameObject go, LuaFunction luafunc) {
            if (go == null) return;
            UIEventListener.Get(go).onClick = delegate(GameObject o) {
                luafunc.Call(go);
                buttons.Add(luafunc);
            };
        }

        /// <summary>
        /// 清除单击事件
        /// </summary>
        public void ClearClick() {
            for (int i = 0; i < buttons.Count; i++ ) {
                if (buttons[i] != null) {
                    buttons[i].Dispose();
                    buttons[i] = null;
                }
            }
        }
        
        /// <summary>
        /// 执行Lua方法
        /// </summary>
        protected object[] CallMethod(string func, params object[] args) {
            if (!initialize) return null;
            return Util.CallMethod(name, func, args);
        }

        //-----------------------------------------------------------------
        protected void OnDestroy() {
            if (bundle) {
                bundle.Unload(true);
                bundle = null;  //销毁素材
            }
            ClearClick();
            Util.ClearMemory();
            Debug.Log("~" + name + " was destroy!");
        }
    }
}