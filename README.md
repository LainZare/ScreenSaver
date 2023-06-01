# C# 泡泡屏保

## 关键点

### 双缓存

Panel本身无法设置双缓存，需要自建一个类继承Panel，设置双缓存。

用 `Invalidate()` 强制重绘，触发 `OnPaint()` 事件。

### 透明窗体

在主窗体上添加一个Panel，在Panel上绘制泡泡。

底层窗体设置 `TransparencyKey` 为 Panel的 `BackColor`，并设置 `FormBorderStyle` 为 `None`，即可实现透明窗体。
