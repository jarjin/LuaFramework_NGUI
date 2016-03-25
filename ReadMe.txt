项目开源免费，求上面点星支持(star ^o^)

本框架工程基于Unity 5.0/4.6.2 + NGUI 3.8.2 + tolua构建
服务器端基于VS2012及其以上版本。

有问题请加：ulua技术交流群 434341400

支持平台：PC/MAC/Android(armv7-a + Intel x86)/iOS(armv7 + arm64)/
	  WP8(SimpleFramework_WP_v0.1.1 (nlua))/

框架文档地址 http://doc.ulua.org/
网盘下载地址 http://pan.baidu.com/s/1bcP9qY
tolua#地址： https://github.com/topameng/tolua
框架底层库:  https://github.com/jarjin/tolua_rumtime
服务器框架:  https://github.com/jarjin/ServerFramework

//-------------2016-03-25-------------
(1)清理meta文件等问题。
(2)更新tolua#到1.0.4.109版

//-------------2016-03-22-------------
(1)更新tolua#到1.0.4.102版

//-------------2016-03-21-------------
(1)更新tolua#到1.04版

//-------------2016-03-15-------------
(1)添加-fembed-bitcode标记支持BITCODE_MODE

//-------------2016-03-12-------------
(1)修复LuaLoop协同功能。
(2)修复IOS上面加载luabundle大小写问题。

//-------------2016-03-06-------------
(1)更新tolua #到1.03版本

//-------------2016-02-28-------------
(1)修复ByteBuffer.cs的WriteBuffer函数

//-------------2016-02-21-------------
(1)修复Load lua file failed: tolua.lua

//-------------2016-01-31-------------
(1)简化框架加载流程。
(2)集成第三方库pblua\pbc\cjson\sproto等功能。
(3)整理部分框架代码。

//-------------2016-01-30-------------
(1)添加luajit2.1版本在ios下的32、64位编码器。
(2)修复加载Lua文件BUG。

//-------------2016-01-29-------------
(1)同步tolua #1.0.2版本。

//-------------2016-01-25-------------
(1)修复资源管理器扩展名BUG。
(2)修复LuaBundle模式下面在Unity5下面无法加载bug。

//-------------2016-01-24-------------
(1)修复逻辑小bug，添加移除单击监听。

//-------------2016-01-23-------------
(1)完善了Lua的字节码模式AppConst.LuaByteMode、Lua的AssetBundle模式AppConst.LuaBundleMode的交叉使用。
(2)同步tolua #1.0.1版本。

//-------------2016-01-18-------------
(1)框架直接基于tolua#提供的luabundle功能，开关在AppConst.LuaBundleMode。