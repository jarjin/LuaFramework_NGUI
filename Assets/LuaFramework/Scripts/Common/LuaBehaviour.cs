using UnityEngine;
using LuaInterface;
using System.Collections;
using System.Collections.Generic;
using System;

namespace LuaFramework {
    public class LuaBehaviour : Base {
        private string data = null;
        private AssetBundle bundle = null;
        private Dictionary<string, LuaFunction> buttons = new Dictionary<string, LuaFunction>();

        protected void Awake() {
            Util.CallMethod(name, "Awake", gameObject);
        }

        protected void Start() {
            Util.CallMethod(name, "Start");
        }

        protected void OnClick() {
            Util.CallMethod(name, "OnClick");
        }

        protected void OnClickEvent(GameObject go) {
            Util.CallMethod(name, "OnClick", go);
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
#if UNITY_5
            return bundle.LoadAsset(name, typeof(GameObject)) as GameObject;
#else
            return bundle.Load(name, typeof(GameObject)) as GameObject;
#endif
        }

        /// <summary>
        /// 添加单击事件
        /// </summary>
        public void AddClick(GameObject go, LuaFunction luafunc) {
            if (go == null || luafunc == null) return;
            buttons.Add(go.name, luafunc);
            UIEventListener.Get(go).onClick = delegate(GameObject o) {
                luafunc.Call(go);
            };
        }

        /// <summary>
        /// 删除单击事件
        /// </summary>
        /// <param name="go"></param>
        public void RemoveClick(GameObject go) {
            if (go == null) return;
            LuaFunction luafunc = null;
            if (buttons.TryGetValue(go.name, out luafunc)) {
                buttons.Remove(go.name);
                luafunc.Dispose();
                luafunc = null;
            }
        }

        /// <summary>
        /// 清除单击事件
        /// </summary>
        public void ClearClick() {
            foreach (var de in buttons) {
                if (de.Value != null) {
                    de.Value.Dispose();
                }
            }
            buttons.Clear();
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