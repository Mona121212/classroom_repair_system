# 修复登录和权限问题指南

## 问题1: 管理员登录失败

### 原因分析
从错误信息 `"Invalid username or password!"` 看，可能是：
1. OpenIddict客户端配置缺失
2. 用户密码不正确
3. 用户未正确创建

### 修复步骤

#### 步骤1: 检查并更新 appsettings.json
已更新 `src/Alberta.ServiceDesk.Web/appsettings.json`，添加了 OpenIddict 配置。

#### 步骤2: 重新运行数据库迁移
```bash
cd src/Alberta.ServiceDesk.DbMigrator
dotnet run
```

这会：
- 创建/更新 OpenIddict 客户端配置
- 确保所有用户和角色存在
- 重新分配权限

#### 步骤3: 验证用户是否存在
在数据库中运行：
```sql
SELECT "UserName", "Email", "Name" 
FROM "AbpUsers" 
WHERE "UserName" IN ('S1001', 'T1001', 'A1001');
```

#### 步骤4: 验证角色分配
```sql
SELECT u."UserName", r."Name" as RoleName
FROM "AbpUsers" u
JOIN "AbpUserRoles" ur ON u."Id" = ur."UserId"
JOIN "AbpRoles" r ON ur."RoleId" = r."Id"
WHERE u."UserName" IN ('S1001', 'T1001', 'A1001')
ORDER BY u."UserName", r."Name";
```

应该看到：
- S1001 -> Student
- T1001 -> Teacher  
- A1001 -> Admin

#### 步骤5: 验证权限配置
```sql
SELECT "ProviderKey" as RoleName, "Name" as Permission
FROM "AbpPermissionGrants"
WHERE "ProviderName" = 'R' 
  AND "ProviderKey" IN ('Student', 'Teacher', 'Admin')
ORDER BY "ProviderKey", "Name";
```

应该看到每个角色都有相应的权限。

## 问题2: 学生和教师看不到预约

### 已修复
已更新 `BookingAppService.GetListAsync()` 方法：
- **Admin**: 可以看到所有预约
- **Student/Teacher**: 只能看到自己创建的预约

### 验证方法
1. 使用 Student 账号登录
2. 创建一个预约
3. 在"我的预约"页面应该能看到这个预约
4. 使用 Teacher 账号登录，不应该看到 Student 创建的预约
5. 使用 Admin 账号登录，应该能看到所有预约

## 问题3: 403 Forbidden 错误

### 可能原因
1. 权限未正确授予
2. Token 中缺少角色信息
3. 权限检查逻辑问题

### 修复步骤

#### 步骤1: 重新运行权限种子
```bash
cd src/Alberta.ServiceDesk.DbMigrator
dotnet run
```

#### 步骤2: 检查权限表
```sql
-- 检查 Student 权限
SELECT * FROM "AbpPermissionGrants" 
WHERE "ProviderName" = 'R' AND "ProviderKey" = 'Student';

-- 检查 Teacher 权限  
SELECT * FROM "AbpPermissionGrants" 
WHERE "ProviderName" = 'R' AND "ProviderKey" = 'Teacher';

-- 检查 Admin 权限
SELECT * FROM "AbpPermissionGrants" 
WHERE "ProviderName" = 'R' AND "ProviderKey" = 'Admin';
```

#### 步骤3: 清除并重新登录
1. 清除浏览器 localStorage
2. 重新登录
3. 检查 Token 中是否包含角色信息

## 快速修复命令

```bash
# 1. 重新运行数据库迁移（这会重新创建所有种子数据）
cd src/Alberta.ServiceDesk.DbMigrator
dotnet run

# 2. 重启后端
cd ../Alberta.ServiceDesk.Web
dotnet run

# 3. 重启前端
cd ../../frontend/apps/portal
npm run dev
```

## 测试清单

- [ ] Admin (A1001) 可以登录
- [ ] Student (S1001) 可以登录
- [ ] Teacher (T1001) 可以登录
- [ ] Student 可以看到自己创建的预约
- [ ] Teacher 可以看到自己创建的预约
- [ ] Admin 可以看到所有预约
- [ ] Student 不能看到其他用户的预约
- [ ] 所有角色都可以浏览场地列表

