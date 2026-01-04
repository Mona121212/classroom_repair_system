# 🚀 ServiceDesk 快速启动指南

## 📖 系统架构说明

### 前端网页的作用

前端（React）是**用户界面层**，负责：
1. **展示数据** - 将后端API返回的数据以友好的方式展示给用户
2. **用户交互** - 处理用户点击、输入等操作
3. **调用后端API** - 通过HTTP请求与后端通信
4. **管理认证** - 处理登录、Token存储等

### 数据流向

```
用户操作（点击按钮）
    ↓
React组件（FacilityList.tsx）
    ↓
调用 services/facilities.ts
    ↓
services/api.ts 发送HTTP请求
    ↓
Vite Proxy 转发到后端 (https://localhost:44302)
    ↓
ABP后端接收请求
    ↓
Application Service 处理业务逻辑
    ↓
Repository 查询数据库
    ↓
返回数据给前端
    ↓
React组件更新UI显示
```

## 🔧 完整连接步骤

### 第一步：启动数据库（PostgreSQL）

确保PostgreSQL服务正在运行：
- 默认端口：5432
- 数据库名：ServiceDesk
- 用户名：postgres
- 密码：MonaDatabase3355

### 第二步：初始化数据库

打开**第一个终端窗口**：

```bash
cd src/Alberta.ServiceDesk.DbMigrator
dotnet run
```

**这一步会：**
- ✅ 创建所有数据库表
- ✅ 创建角色（Student, Teacher, Admin）
- ✅ 分配权限
- ✅ 创建测试用户（S1001, T1001, A1001）
- ✅ 创建测试场地数据

看到 "Successfully completed all database migrations" 表示成功。

### 第三步：启动后端服务

打开**第二个终端窗口**：

```bash
cd src/Alberta.ServiceDesk.Web
dotnet run
```

**后端将运行在：** `https://localhost:44302`

**验证后端：**
- 打开浏览器访问：`https://localhost:44302/swagger`
- 应该能看到所有API端点（Facility, Booking等）

### 第四步：安装前端依赖（仅首次）

打开**第三个终端窗口**：

```bash
cd frontend/apps/portal
npm install
```

**这会安装：**
- axios（HTTP客户端）
- antd（UI组件库）
- react-router-dom（路由）
- dayjs（日期处理）

### 第五步：启动前端开发服务器

在**同一个终端**（frontend/apps/portal目录）：

```bash
npm run dev
```

**前端将运行在：** `http://localhost:5173`

### 第六步：访问应用

打开浏览器访问：`http://localhost:5173`

## 🎯 完整测试流程

### 1. 登录
1. 访问 `http://localhost:5173`
2. 自动跳转到登录页
3. 输入：
   - 用户名：`S1001`
   - 密码：`Student123!`
4. 点击"登录"
5. 登录成功后跳转到首页

### 2. 浏览场地
1. 点击左侧菜单"场地"
2. 看到场地列表（4个测试场地）
3. 可以：
   - 按类型筛选（Lab, Auditorium等）
   - 按所属单位筛选
   - 点击"查看详情"查看单个场地

### 3. 创建预约
1. 在场地详情页点击"预约此场地"
2. 或从菜单"我的预约" → "新建预约"
3. 填写表单：
   - 选择时间范围
   - 输入参与人数（不能超过场地容量）
   - 填写用途
4. 点击"提交预约"
5. 看到成功提示和预约号（如：BK-20260104-0001）

### 4. 查看我的预约
1. 点击左侧菜单"我的预约"
2. 看到刚才创建的预约
3. 显示预约号、场地、时间、状态等信息

### 5. 测试业务规则
1. 使用Student账号（S1001）登录
2. 尝试预约一个"Lab"类型的场地
3. 应该看到错误："Students cannot book laboratory facilities"
4. 使用Teacher账号（T1001）登录
5. 可以成功预约Lab场地

## 📁 文件作用详解

### 前端文件结构

```
frontend/apps/portal/src/
├── services/           # API服务层（连接后端）
│   ├── api.ts         # HTTP客户端，处理所有API请求
│   ├── auth.ts        # 认证服务（登录/登出）
│   ├── facilities.ts  # 场地API调用
│   └── bookings.ts    # 预约API调用
│
├── pages/             # 页面组件（用户看到的界面）
│   ├── Login.tsx      # 登录页面
│   ├── facilities/
│   │   ├── FacilityList.tsx    # 场地列表页
│   │   └── FacilityDetail.tsx   # 场地详情页
│   ├── bookings/
│   │   ├── BookingList.tsx     # 预约列表页
│   │   └── CreateBooking.tsx   # 创建预约页
│   └── layout/
│       └── MainLayout.tsx       # 主布局（导航栏等）
│
├── types/             # TypeScript类型定义
│   ├── facility.ts    # 场地类型
│   ├── booking.ts     # 预约类型
│   └── common.ts     # 通用类型
│
└── App.tsx            # 主应用组件（路由配置）
```

### 后端文件结构

```
src/
├── Alberta.ServiceDesk.Domain/          # 领域层（实体）
│   ├── Facilities/Facility.cs           # 场地实体
│   └── Bookings/Booking.cs             # 预约实体
│
├── Alberta.ServiceDesk.Application/      # 应用层（业务逻辑）
│   ├── Facilities/FacilityAppService.cs # 场地服务
│   └── Bookings/BookingAppService.cs    # 预约服务
│
├── Alberta.ServiceDesk.Application.Contracts/  # 接口和DTO
│   ├── Facilities/
│   │   ├── IFacilityAppService.cs      # 场地服务接口
│   │   └── FacilityDto.cs              # 场地DTO
│   └── Bookings/
│       ├── IBookingAppService.cs       # 预约服务接口
│       └── BookingDto.cs               # 预约DTO
│
└── Alberta.ServiceDesk.EntityFrameworkCore/    # 数据访问层
    └── EntityFrameworkCore/
        └── ServiceDeskDbContext.cs      # 数据库上下文
```

## 🔗 连接原理

### 1. 前端如何连接后端？

**Vite Proxy配置** (`vite.config.ts`)：
```typescript
proxy: {
  '/api': {
    target: 'https://localhost:44302',  // 后端地址
    changeOrigin: true,
    secure: false
  }
}
```

**工作原理：**
- 前端运行在 `http://localhost:5173`
- 当前端调用 `/api/app/facility` 时
- Vite自动转发到 `https://localhost:44302/api/app/facility`
- 解决了跨域问题（CORS）

### 2. 后端如何连接数据库？

**连接字符串** (`appsettings.json`)：
```json
"ConnectionStrings": {
  "Default": "Host=localhost;Port=5432;Database=ServiceDesk;..."
}
```

**Entity Framework Core：**
- `ServiceDeskDbContext` 使用连接字符串连接PostgreSQL
- LINQ查询自动转换为SQL
- 数据映射到实体对象

### 3. 认证流程

```
1. 用户输入用户名密码
   ↓
2. 前端调用 authService.login()
   ↓
3. 发送POST请求到 /connect/token
   ↓
4. 后端验证并返回Access Token
   ↓
5. 前端保存Token到localStorage
   ↓
6. 后续所有API请求自动携带Token
   ↓
7. 后端验证Token并检查权限
```

## 🎨 前端页面功能

### Login.tsx（登录页）
- **作用**：用户登录界面
- **功能**：输入用户名密码，获取Token

### FacilityList.tsx（场地列表）
- **作用**：显示所有场地
- **功能**：
  - 调用 `facilitiesApi.getList()` 获取数据
  - 支持筛选（类型、所属单位）
  - 支持分页
  - 点击查看详情

### FacilityDetail.tsx（场地详情）
- **作用**：显示单个场地信息
- **功能**：
  - 调用 `facilitiesApi.getById()` 获取详情
  - 显示场地所有信息
  - 提供"预约此场地"按钮

### CreateBooking.tsx（创建预约）
- **作用**：创建新预约的表单
- **功能**：
  - 选择时间范围
  - 输入参与人数（验证容量）
  - 填写用途
  - 调用 `bookingsApi.create()` 提交

### BookingList.tsx（我的预约）
- **作用**：显示当前用户的所有预约
- **功能**：
  - 调用 `bookingsApi.getList()` 获取数据
  - 显示预约列表
  - 显示预约状态

## 🚨 常见问题

### Q1: 前端无法连接后端？
**检查：**
1. 后端是否运行在 `https://localhost:44302`
2. 打开浏览器控制台（F12）查看错误
3. 检查 `vite.config.ts` 的proxy配置

### Q2: 登录失败？
**检查：**
1. 数据库迁移是否已运行
2. 测试用户是否已创建
3. 后端日志中的错误信息

### Q3: CORS错误？
**解决：**
- 检查 `appsettings.json` 中的 `CorsOrigins`
- 确认包含 `http://localhost:5173`

### Q4: 页面空白？
**检查：**
1. 浏览器控制台错误
2. 前端依赖是否安装（`npm install`）
3. 路由配置是否正确

## 📝 开发建议

1. **修改后端代码** → 停止后端 → 重新运行
2. **修改前端代码** → Vite自动热重载（无需重启）
3. **修改数据库** → 创建迁移 → 运行DbMigrator

## ✅ 验证清单

- [ ] PostgreSQL运行中
- [ ] 数据库迁移完成
- [ ] 后端运行在 https://localhost:44302
- [ ] 前端运行在 http://localhost:5173
- [ ] 可以登录（S1001/Student123!）
- [ ] 可以浏览场地列表
- [ ] 可以查看场地详情
- [ ] 可以创建预约
- [ ] Student不能预约Lab场地

现在你可以按照这个指南运行整个系统了！

