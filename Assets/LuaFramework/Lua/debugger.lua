--for mac--
--[[
ZBS = "/Applications/ZeroBraneStudio.app/Contents/ZeroBraneStudio/";
LuaPath = "/Users/jarjin/Desktop/ulua_debugger_demo/Assets/uLua/Lua/"
package.path = package.path..";./?.lua;"..ZBS.."lualibs/?/?.lua;"..ZBS.."lualibs/?.lua;"..LuaPath.."?.lua;"
]]

--for win--
ZBS = "D:/ZeroBraneStudio/";
LuaPath = "E:/workspace/SimpleFramework_NGUI/Assets/Lua/"
package.path = package.path..";./?.lua;"..ZBS.."lualibs/?/?.lua;"..ZBS.."lualibs/?.lua;"..LuaPath.."?.lua;"

require("mobdebug").start()

debugger = {};
local this = debugger;

require "Logic/LuaClass"
require "Common/functions"

function debugger.Awake()
  log('Awake--->>>');
end

--启动事件--
function debugger.Start()
   warn('Start--->>>');
end

debugger.Awake();
