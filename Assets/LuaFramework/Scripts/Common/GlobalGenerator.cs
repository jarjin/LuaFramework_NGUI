using UnityEngine;
using System.Collections;

namespace LuaFramework {
    /// <summary>
    /// 全局构造器，每个场景里都有，所以每个场景都会初始化一遍，也会初始化游戏管理器一次
    /// 如果游戏管理器已经存在了，就跳过了，否则创建游戏管理器，来保证游戏里只有一个GameManager
    /// </summary>
    public class GlobalGenerator : MonoBehaviour {

        void Awake() {
            InitGameMangager();
        }

        /// <summary>
        /// 实例化游戏管理器
        /// </summary>
        public void InitGameMangager() {
            string name = "GameManager";
            GameObject manager = GameObject.Find(name);
            if (manager == null) {
                manager = new GameObject(name);
                manager.name = name;

                AppFacade.Instance.StartUp();   //启动游戏
            }
        }
    }
}