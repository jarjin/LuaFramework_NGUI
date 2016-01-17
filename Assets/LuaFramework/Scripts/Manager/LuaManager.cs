using UnityEngine;
using System.Collections;
using LuaInterface;

namespace LuaFramework {
    public class LuaManager : Manager {
        public LuaState lua;

        // Use this for initialization
        void Awake() {
            lua = new LuaState();
            this.InitLuaPath();
            LuaBinder.Bind(lua);
        }

        void InitLuaPath() {
            string rootPath = AppConst.FrameworkRoot;
            lua.AddSearchPath(rootPath + "/Lua");
            lua.AddSearchPath(rootPath + "/ToLua/Lua");
        }

        public object[] DoFile(string filename) {
            return lua.DoFile(filename);
        }

        // Update is called once per frame
        public object[] CallFunction(string funcName, params object[] args) {
            LuaFunction func = lua.GetFunction(funcName);
            if (func != null) {
                return func.Call(args);
            }
            return null;
        }

        public void LuaGC() {
            lua.LuaGC(LuaGCOptions.LUA_GCCOLLECT);
        }

        public void Close() {
            lua.Dispose();
        }
    }
}