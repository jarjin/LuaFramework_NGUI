using UnityEngine;
using System.Collections;
using System.IO;
using System;

namespace LuaFramework {
    public class ResourceManager : Manager {
        private AssetBundle shared;

        /// <summary>
        /// 初始化
        /// </summary>
        public void initialize(Action func) {
            if (AppConst.ExampleMode) {
                byte[] stream;
                string uri = string.Empty;
                //------------------------------------Shared--------------------------------------
                uri = Util.DataPath + "shared" + AppConst.ExtName;
                Debug.LogWarning("LoadFile::>> " + uri);

                stream = File.ReadAllBytes(uri);
                shared = AssetBundle.CreateFromMemoryImmediate(stream);
#if UNITY_5
        shared.LoadAsset("Dialog", typeof(GameObject));
#else
                shared.Load("Dialog", typeof(GameObject));
#endif
            }
            if (func != null) func();    //资源初始化完成，回调游戏管理器，执行后续操作 
        }

        /// <summary>
        /// 载入素材
        /// </summary>
        public AssetBundle LoadBundle(string name) {
            byte[] stream = null;
            AssetBundle bundle = null;
            string uri = Util.DataPath + name.ToLower() + AppConst.ExtName;
            stream = File.ReadAllBytes(uri);
            bundle = AssetBundle.CreateFromMemoryImmediate(stream); //关联数据的素材绑定
            return bundle;
        }

        /// <summary>
        /// 销毁资源
        /// </summary>
        void OnDestroy() {
            if (shared != null) shared.Unload(true);
            Debug.Log("~ResourceManager was destroy!");
        }
    }
}