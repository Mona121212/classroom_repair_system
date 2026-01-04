# 🔧 前端页面空白问题排查指南

## 常见原因和解决方法

### 1. 缺少依赖包

**症状：** 页面空白，浏览器控制台显示模块未找到错误

**解决方法：**
```bash
cd frontend/apps/portal
npm install
```

**需要安装的依赖：**
- axios
- antd
- react-router-dom
- @ant-design/icons
- dayjs

### 2. JavaScript 运行时错误

**症状：** 页面空白，浏览器控制台（F12）显示红色错误信息

**检查步骤：**
1. 打开浏览器开发者工具（按 F12）
2. 查看 "Console" 标签页
3. 查找红色错误信息

**常见错误：**
- `Cannot find module 'xxx'` - 缺少依赖，运行 `npm install`
- `xxx is not defined` - 代码错误，检查导入语句
- `Cannot read property 'xxx' of undefined` - 空值检查问题

### 3. 路由配置问题

**症状：** 页面空白，但控制台没有错误

**检查：**
- 确认 `App.tsx` 中的路由配置正确
- 确认所有页面组件都已正确导入
- 检查 URL 路径是否正确

### 4. 认证检查导致重定向循环

**症状：** 页面空白，网络请求显示多次重定向

**解决方法：**
- 检查 `MainLayout.tsx` 中的认证逻辑
- 确认 `authService.isAuthenticated()` 不会导致循环

### 5. CSS 样式问题

**症状：** 页面有内容但不可见（白色文字在白色背景上）

**解决方法：**
- 检查 `index.css` 中的颜色设置
- 确认 Ant Design 样式已正确加载

## 🔍 详细排查步骤

### 步骤 1: 检查浏览器控制台

1. 打开浏览器（Chrome/Edge）
2. 按 `F12` 打开开发者工具
3. 点击 "Console" 标签页
4. 查看是否有红色错误信息
5. **截图或复制错误信息**

### 步骤 2: 检查网络请求

1. 在开发者工具中点击 "Network" 标签页
2. 刷新页面（F5）
3. 查看是否有请求失败（红色状态码）
4. 检查主要资源（JS、CSS）是否成功加载

### 步骤 3: 检查依赖安装

```bash
cd frontend/apps/portal
npm list --depth=0
```

应该看到：
- react
- react-dom
- axios
- antd
- react-router-dom
- @ant-design/icons
- dayjs

### 步骤 4: 重新安装依赖

如果依赖有问题：

```bash
cd frontend/apps/portal
rm -rf node_modules package-lock.json  # Windows: 删除这两个文件夹/文件
npm install
```

### 步骤 5: 检查 Vite 开发服务器

确认 Vite 服务器正常运行：

```bash
cd frontend/apps/portal
npm run dev
```

应该看到：
```
  VITE v7.x.x  ready in xxx ms

  ➜  Local:   http://localhost:5173/
  ➜  Network: use --host to expose
```

### 步骤 6: 检查代码语法

运行 TypeScript 检查：

```bash
cd frontend/apps/portal
npm run build
```

如果有语法错误，会显示出来。

## 🐛 常见错误和解决方案

### 错误 1: "Cannot find module 'dayjs'"

**解决：**
```bash
npm install dayjs
```

### 错误 2: "Cannot find module 'antd'"

**解决：**
```bash
npm install antd @ant-design/icons
```

### 错误 3: "Cannot find module 'react-router-dom'"

**解决：**
```bash
npm install react-router-dom
```

### 错误 4: "Failed to fetch dynamically imported module"

**原因：** 模块路径错误或文件不存在

**解决：** 检查导入路径是否正确

### 错误 5: "Uncaught TypeError: Cannot read property 'xxx' of undefined"

**原因：** 访问了未定义的属性

**解决：** 添加空值检查

## 📋 快速检查清单

- [ ] 已运行 `npm install`
- [ ] 浏览器控制台没有红色错误
- [ ] Vite 开发服务器正在运行
- [ ] 访问的 URL 正确（http://localhost:5173）
- [ ] 所有依赖包都已安装
- [ ] 代码没有语法错误

## 💡 如果以上都不行

1. **清除浏览器缓存**
   - 按 `Ctrl+Shift+Delete`
   - 选择"缓存的图片和文件"
   - 清除数据

2. **重启开发服务器**
   - 停止 Vite 服务器（Ctrl+C）
   - 重新运行 `npm run dev`

3. **检查端口占用**
   - 确认 5173 端口没有被其他程序占用

4. **查看完整错误信息**
   - 打开浏览器控制台
   - 查看完整的错误堆栈
   - 根据错误信息定位问题

## 📞 需要帮助时提供的信息

如果问题仍然存在，请提供：

1. **浏览器控制台的完整错误信息**（截图或复制）
2. **Network 标签页的请求状态**（哪些请求失败了）
3. **运行 `npm list --depth=0` 的输出**
4. **Vite 开发服务器的输出信息**

