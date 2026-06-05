# UIFramework

UIFramework 是一套面向 **Unity 2021.3 LTS** 的 uGUI 框架，目标是提供可商用的 UI 核心骨架。

设计灵感来自：
- 原神 `MoleMole.UIManager`（Context 体系、分层、生命周期调度）
- GameFramework（分组深度管理、资源句柄思路）
- LoxodonFramework（后续阶段 MVVM / 绑定）

> 当前 PR 实现 **P1 核心骨架**：可在指定层异步创建并显隐 UI，并完整驱动生命周期。后续功能将通过独立 PR 逐步接入。

## 架构总览

```text
Application
   |
HelloUIBootstrap
   |
UIManager (注册/打开/关闭/生命周期调度)
   |-- UILayerManager (层管理 + 同层排序)
   |-- IResourceLoader (资源抽象)
   |-- Context Registry (Type -> UIConfig)
   |
UIRoot (全局 Canvas + EventSystem + 各 UILayer 子 Canvas)
   |
BaseContext (Page/Widget/Dialog)
   |
UIView (View 与 Context 桥接)
```

## UILayer 分层说明

| 层级 | 值 | 用途 |
|---|---:|---|
| Scene | 0 | 场景贴附 UI |
| Bottom | 100 | 底图/背景 |
| Normal | 200 | 普通全屏页面（导航主层） |
| Fixed | 300 | 常驻 HUD / 固定挂件 |
| Popup | 400 | 弹窗 |
| Guide | 500 | 引导遮罩 |
| Top | 600 | 全局提示 |
| System | 700 | Loading / 断线重连等最高优先级 |

## 生命周期

`UIManager` 严格按以下顺序驱动：

```text
OnInit -> OnShow -> OnHide -> OnClose -> OnDestroy
```

- `OnInit`：实例化后仅一次（绑定组件、注册事件）
- `OnShow`：每次打开（传参与刷新）
- `OnHide`：每次关闭前隐藏逻辑
- `OnClose`：关闭收尾
- `OnDestroy`：销毁前最终清理（未缓存时触发）

## 快速开始

1. 在场景中创建一个空物体，挂载 `HelloUIBootstrap`。
2. 运行场景，脚本会自动初始化 `UIManager`、注册配置并打开 `SampleHelloPage`。

最小示例代码：

```csharp
private static async Task BootstrapAsync()
{
    var uiManager = UIManager.Instance;
    uiManager.Init(new CodeViewLoader());
    uiManager.Register<SampleHelloPage>(new UIConfig
    {
        Id = "HelloPage",
        PrefabKey = "SampleHelloPage",
        Layer = UILayer.Normal,
        CacheOnClose = false,
        FullScreen = true,
    });

    await uiManager.OpenAsync<SampleHelloPage>("Hello UIFramework!");
}
```

> 示例使用 `CodeViewLoader` 纯代码构建占位视图，避免提交二进制 prefab。实际项目请替换为 `ResourcesLoader` 或后续 `AddressablesLoader`。

## 路线图

| 阶段 | 内容 | 状态 |
|---|---|---|
| P1 | 核心骨架（本 PR） | ✅ |
| P2 | 栈式导航 UINavigator | ⏳ |
| P3 | 资源系统扩展（Addressables） | ⏳ |
| P4 | 对象池 IUIPool | ⏳ |
| P5 | 消息中心 IMessageCenter | ⏳ |
| P6 | 虚拟列表 | ⏳ |
| P7 | 转场动画 IUITransition | ⏳ |
| P8 | MVVM / 数据绑定 | ⏳ |
| P9 | Editor 工具（绑定/生成） | ⏳ |
| P10 | 示例与测试完善 | ⏳ |

---

本仓库当前仅实现 P1，后续阶段将以独立 PR 继续推进。
