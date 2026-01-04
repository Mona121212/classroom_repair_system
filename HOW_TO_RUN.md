# ServiceDesk 完整运行指南

## 🏗️ 系统架构

```
┌─────────────────────────────────────────────────────────┐
│                    用户浏览器                            │
│              http://localhost:5173                      │
└────────────────────┬────────────────────────────────────┘
                     │
                     │ HTTP请求 (带Bearer Token)
                     ▼
┌─────────────────────────────────────────────────────────┐
│              React 前端 (Vite Dev Server)               │
│              frontend/apps/portal                       │
│  - 用户界面 (Ant Design)                                │
│  - API调用 (axios)                                       │
│  - 路由管理 (React Router)                              │
└────────────────────┬────────────────────────────────────┘
                     │
                     │ Proxy转发到后端
                     ▼
┌─────────────────────────────────────────────────────────┐
│            ABP 后端 API (ASP.NET Core)                  │
│            src/Alberta.ServiceDesk.Web                  │
│  - RESTful API                                          │
│  - 权限验证                                             │
│  - 业务逻辑                                             │
└────────────────────┬────────────────────────────────────┘
                     │
                     │ Entity Framework Core
                     │ LINQ查询
                     ▼
┌─────────────────────────────────────────────────────────┐
│              PostgreSQL 数据库                           │
│              localhost:5432                             │
│  - AppFacilities (场地表)                                │
│  - AppBookings (预约表)                                  │
│  - AbpUsers (用户表)                                    │
│  - AbpRoles (角色表)                                    │
└─────────────────────────────────────────────────────────┘
```

## 📋 运行步骤（按顺序）

### 步骤 1: 确保 PostgreSQL 运行
```bash
# 检查PostgreSQL是否运行
# Windows: 检查服务管理器
# 或使用 pgAdmin 连接测试
```

### 步骤 2: 初始化数据库
```bash
cd src/Alberta.ServiceDesk.DbMigrator
dotnet run
```

**这一步会：**
- 创建数据库表结构
- 创建角色（Student, Teacher, Admin）
- 分配权限
- 创建测试用户（S1001, T1001, A1001）
- 创建测试场地数据

### 步骤 3: 启动后端服务
```bash
cd src/Alberta.ServiceDesk.Web
dotnet run
```

**后端将运行在：** `https://localhost:44302`

**验证后端：**
- 打开浏览器访问：`https://localhost:44302/swagger`
- 应该能看到所有API端点

### 步骤 4: 安装前端依赖（首次运行）
```bash
cd frontend/apps/portal
npm install
```

**这会安装：**
- react, react-dom
- axios (HTTP客户端)
- antd (UI组件库)
- react-router-dom (路由)
- dayjs (日期处理)

### 步骤 5: 启动前端开发服务器
```bash
cd frontend/apps/portal
npm run dev
```

**前端将运行在：** `http://localhost:5173`

### 步骤 6: 访问应用
打开浏览器访问：`http://localhost:5173`

## 🔐 测试账号

| 角色 | 用户名 | 密码 | 权限说明 |
|------|--------|------|----------|
| **学生** | `S1001` | `Student123!` | 可以浏览场地、创建预约（不能预约实验室） |
| **教师** | `T1001` | `Teacher123!` | 可以浏览场地、创建预约（包括实验室） |
| **管理员** | `A1001` | `Admin123!` | 拥有所有权限，包括管理功能 |

## 🎯 完整测试流程

### 1. 登录测试
1. 访问 `http://localhost:5173`
2. 自动跳转到登录页
3. 输入 `S1001` / `Student123!`
4. 点击登录

### 2. 浏览场地
1. 登录后，点击左侧菜单 "场地"
2. 应该看到场地列表
3. 可以按类型、所属单位筛选
4. 点击 "查看详情" 查看单个场地

### 3. 创建预约
1. 在场地详情页点击 "预约此场地"
2. 或从菜单进入 "我的预约" → "新建预约"
3. 填写预约信息：
   - 选择时间范围
   - 输入参与人数
   - 填写用途
4. 点击 "提交预约"
5. 应该看到成功提示和预约号

### 4. 测试业务规则
1. 使用 Student 账号登录
2. 尝试预约一个 Type="Lab" 的场地
3. 应该看到错误提示："Students cannot book laboratory facilities"

## 🔧 配置文件说明

### 后端配置
- `src/Alberta.ServiceDesk.Web/appsettings.json`
  - `ConnectionStrings:Default` - 数据库连接字符串
  - `App:CorsOrigins` - 允许的前端地址

### 前端配置
- `frontend/apps/portal/vite.config.ts`
  - `server.proxy` - API代理配置（转发到后端）

## 🐛 常见问题

### 问题1: 前端无法连接后端
**解决：**
- 检查后端是否运行在 `https://localhost:44302`
- 检查 `vite.config.ts` 中的 proxy 配置
- 检查浏览器控制台的错误信息

### 问题2: 登录失败
**解决：**
- 确认数据库迁移已运行
- 检查测试用户是否已创建（运行 DbMigrator）
- 检查后端日志

### 问题3: CORS 错误
**解决：**
- 检查 `appsettings.json` 中的 `CorsOrigins` 配置
- 确认前端地址在允许列表中

### 问题4: 权限错误
**解决：**
- 确认用户角色已正确分配
- 检查 `AppPermissionDataSeedContributor` 是否已运行
- 重新运行 DbMigrator

## 📝 开发工作流

1. **修改后端代码** → 停止后端 → 重新运行 `dotnet run`
2. **修改前端代码** → Vite 会自动热重载（无需重启）
3. **修改数据库结构** → 创建迁移 → 运行 DbMigrator

## 🎨 前端页面说明

- **登录页** (`/login`) - 用户登录
- **首页** (`/`) - 欢迎页面，显示用户角色信息
- **场地列表** (`/facilities`) - 显示所有场地，支持筛选
- **场地详情** (`/facilities/:id`) - 显示单个场地详情，可以预约
- **我的预约** (`/bookings`) - 显示当前用户的所有预约
- **创建预约** (`/bookings/create`) - 创建新预约的表单

## 🔗 API端点

所有API端点都在 `/api/app/` 下：
- `GET /api/app/facility` - 获取场地列表
- `GET /api/app/facility/{id}` - 获取场地详情
- `GET /api/app/booking` - 获取预约列表
- `POST /api/app/booking` - 创建预约

认证端点：
- `POST /connect/token` - 获取访问令牌

