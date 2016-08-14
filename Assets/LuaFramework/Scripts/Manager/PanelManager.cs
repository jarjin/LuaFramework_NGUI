using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LuaInterface;

namespace LuaFramework {
    public class PanelManager : Manager {
        private Transform parent;

        Transform Parent {
            get {
                if (parent == null) {
                    GameObject go = GameObject.FindWithTag("GuiCamera");
                    if (go != null) parent = go.transform;
                }
                return parent;
            }
        }

        /// <summary>
        /// 创建面板，请求资源管理器
        /// </summary>
        /// <param name="type"></param>
        public void CreatePanel(string name, LuaFunction func = null) {
            StartCoroutine(StartCreatePanel(name, func));
        }

        /// <summary>
        /// 创建面板
        /// </summary>
        IEnumerator StartCreatePanel(string name, LuaFunction func = null) {
            AssetBundle bundle = ResManager.LoadBundle(name);

            name += "Panel";
            GameObject prefab = null;
#if UNITY_5
            prefab = bundle.LoadAsset(name, typeof(GameObject)) as GameObject;
#else
            prefab = bundle.Load(name, typeof(GameObject)) as GameObject;
#endif
            yield return new WaitForEndOfFrame();

            if (Parent.FindChild(name) != null || prefab == null) {
                yield break;
            }
            GameObject go = Instantiate(prefab) as GameObject;
            go.name = name;
            go.layer = LayerMask.NameToLayer("Default");
            go.transform.parent = Parent;
            go.transform.localScale = Vector3.one;
            go.transform.localPosition = Vector3.zero;

            yield return new WaitForEndOfFrame();
            go.AddComponent<LuaBehaviour>().OnInit(bundle);

            if (func != null) func.Call(go);
            Debug.Log("StartCreatePanel------>>>>" + name);
        }

        /// <summary>
        /// 关闭面板
        /// </summary>
        /// <param name="name"></param>
        public void ClosePanel(string name) {
            var panelName = name + "Panel";
            var panelObj = Parent.FindChild(panelName);
            if (panelObj == null) return;
            Destroy(panelObj.gameObject);
        }
    }
}