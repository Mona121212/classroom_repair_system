# ServiceDesk 系统架构说明

## 整体架构图

```
┌─────────────────┐
│   React 前端    │  (Vite + React 18 + TypeScript + Ant Design)
│  localhost:5173 │
└────────┬────────┘
         │ HTTP/HTTPS (API调用)
         │ Bearer Token (认证)
         ▼
┌─────────────────┐
│  ABP 后端 API   │  (ASP.NET Core + ABP Framework)
│ localhost:44302 │
└────────┬────────┘
         │ Entity Framework Core
         │ LINQ查询
         ▼
┌─────────────────┐
│  PostgreSQL     │  (数据库)
│  localhost:5432 │
└─────────────────┘
```

## 数据流向

### 1. 用户操作流程
```
用户在前端点击按钮
    ↓
React组件调用 services/api.ts
    ↓
axios发送HTTP请求到后端 (https://localhost:44302/api/app/...)
    ↓
ABP后端接收请求，验证权限
    ↓
Application Service处理业务逻辑
    ↓
Repository查询数据库
    ↓
返回数据给前端
    ↓
React组件更新UI
```

### 2. 认证流程
```
用户登录
    ↓
前端发送用户名密码到 /connect/token
    ↓
后端验证并返回 Access Token
    ↓
前端保存Token到localStorage
    ↓
后续所有API请求自动携带Token (Bearer Token)
    ↓
后端验证Token并检查权限
```

## 文件作用说明

### 前端文件 (frontend/apps/portal/src/)

#### 1. services/ - API服务层
- **api.ts**: HTTP客户端，处理所有API请求，自动添加Token
- **auth.ts**: 认证服务，处理登录/登出
- **facilities.ts**: 场地相关的API调用
- **bookings.ts**: 预约相关的API调用

#### 2. pages/ - 页面组件
- **facilities/FacilityList.tsx**: 场地列表页面（显示所有场地，支持筛选）
- **facilities/FacilityDetail.tsx**: 场地详情页面（显示单个场地信息）
- **bookings/CreateBooking.tsx**: 创建预约页面（表单提交预约）
- **layout/MainLayout.tsx**: 主布局（导航栏、侧边栏等）

#### 3. types/ - TypeScript类型定义
- **facility.ts**: 场地相关的类型（FacilityDto等）
- **booking.ts**: 预约相关的类型（BookingDto等）
- **common.ts**: 通用类型（PagedResultDto等）

### 后端文件

#### 1. Application层
- **FacilityAppService.cs**: 场地业务逻辑（列表、详情）
- **BookingAppService.cs**: 预约业务逻辑（创建、查询、业务规则验证）

#### 2. Application.Contracts层
- **IFacilityAppService.cs**: 场地服务接口
- **IBookingAppService.cs**: 预约服务接口
- **DTOs**: 数据传输对象（FacilityDto, BookingDto等）

#### 3. Domain层
- **Facility.cs**: 场地实体
- **Booking.cs**: 预约实体

#### 4. EntityFrameworkCore层
- **ServiceDeskDbContext.cs**: 数据库上下文，连接PostgreSQL

## 如何连接

### 步骤1: 启动数据库
PostgreSQL应该已经在运行（localhost:5432）

### 步骤2: 运行数据库迁移
```bash
cd src/Alberta.ServiceDesk.DbMigrator
dotnet run
```

### 步骤3: 启动后端
```bash
cd src/Alberta.ServiceDesk.Web
dotnet run
```
后端运行在: https://localhost:44302

### 步骤4: 启动前端
```bash
cd frontend/apps/portal
npm install  # 首次运行需要安装依赖
npm run dev
```
前端运行在: http://localhost:5173

### 步骤5: 访问应用
打开浏览器访问: http://localhost:5173

## API端点

后端自动生成的API端点（通过ABP的Conventional Controllers）：
- GET /api/app/facility - 获取场地列表
- GET /api/app/facility/{id} - 获取场地详情
- GET /api/app/booking - 获取预约列表
- POST /api/app/booking - 创建预约
- GET /api/app/booking/{id} - 获取预约详情

## 认证端点

- POST /connect/token - 获取访问令牌（登录）

