-- ============================================
-- Role Migration Script: DepartmentAdmin + SchoolAdmin -> Admin
-- ============================================
-- This script migrates users from the deprecated DepartmentAdmin and SchoolAdmin roles
-- to the new unified Admin role.
--
-- Execution Instructions:
-- 1. Backup your database before running this script
-- 2. Connect to your PostgreSQL database
-- 3. Run this script in a transaction (BEGIN; ... COMMIT;)
-- 4. Verify the results using the verification queries at the end
--
-- ============================================

BEGIN;

-- Step 1: Ensure Admin role exists (should already exist from seed data)
-- If Admin role doesn't exist, create it
DO $$
DECLARE
    admin_role_id UUID;
BEGIN
    SELECT "Id" INTO admin_role_id FROM "AbpRoles" WHERE "Name" = 'Admin';
    
    IF admin_role_id IS NULL THEN
        INSERT INTO "AbpRoles" (
            "Id", "TenantId", "Name", "NormalizedName", "IsDefault", "IsStatic", "IsPublic",
            "EntityVersion", "CreationTime", "ExtraProperties", "ConcurrencyStamp"
        )
        VALUES (
            gen_random_uuid(), NULL, 'Admin', 'ADMIN', false, true, true,
            1, NOW(), '{}', gen_random_uuid()::text
        )
        RETURNING "Id" INTO admin_role_id;
    END IF;
END $$;

-- Step 2: Migrate users from DepartmentAdmin to Admin
-- Add users to Admin role if they are in DepartmentAdmin
INSERT INTO "AbpUserRoles" ("UserId", "RoleId", "TenantId")
SELECT DISTINCT ur."UserId", admin_role."Id", ur."TenantId"
FROM "AbpUserRoles" ur
INNER JOIN "AbpRoles" dept_role ON ur."RoleId" = dept_role."Id" AND dept_role."Name" = 'DepartmentAdmin'
INNER JOIN "AbpRoles" admin_role ON admin_role."Name" = 'Admin'
WHERE NOT EXISTS (
    SELECT 1 FROM "AbpUserRoles" ur2
    INNER JOIN "AbpRoles" r2 ON ur2."RoleId" = r2."Id"
    WHERE ur2."UserId" = ur."UserId" AND r2."Name" = 'Admin'
);

-- Step 3: Migrate users from SchoolAdmin to Admin
-- Add users to Admin role if they are in SchoolAdmin
INSERT INTO "AbpUserRoles" ("UserId", "RoleId", "TenantId")
SELECT DISTINCT ur."UserId", admin_role."Id", ur."TenantId"
FROM "AbpUserRoles" ur
INNER JOIN "AbpRoles" school_role ON ur."RoleId" = school_role."Id" AND school_role."Name" = 'SchoolAdmin'
INNER JOIN "AbpRoles" admin_role ON admin_role."Name" = 'Admin'
WHERE NOT EXISTS (
    SELECT 1 FROM "AbpUserRoles" ur2
    INNER JOIN "AbpRoles" r2 ON ur2."RoleId" = r2."Id"
    WHERE ur2."UserId" = ur."UserId" AND r2."Name" = 'Admin'
);

-- Step 4: Remove users from old roles (optional - uncomment if you want to remove old role assignments)
-- DELETE FROM "AbpUserRoles" ur
-- USING "AbpRoles" r
-- WHERE ur."RoleId" = r."Id" AND r."Name" IN ('DepartmentAdmin', 'SchoolAdmin');

-- Step 5: Migrate permissions (Admin role should already have all permissions from seed)
-- Permissions are handled by AppPermissionDataSeedContributor, but we can verify:
-- Old role permissions will remain in AbpPermissionGrants but won't affect users
-- as they are now in Admin role which has all permissions

COMMIT;

-- ============================================
-- Verification Queries
-- ============================================

-- Check users in Admin role
SELECT u."UserName", u."Email", r."Name" as RoleName
FROM "AbpUsers" u
INNER JOIN "AbpUserRoles" ur ON u."Id" = ur."UserId"
INNER JOIN "AbpRoles" r ON ur."RoleId" = r."Id"
WHERE r."Name" = 'Admin'
ORDER BY u."UserName";

-- Check if any users are still in old roles (should return 0 rows after migration)
SELECT u."UserName", u."Email", r."Name" as RoleName
FROM "AbpUsers" u
INNER JOIN "AbpUserRoles" ur ON u."Id" = ur."UserId"
INNER JOIN "AbpRoles" r ON ur."RoleId" = r."Id"
WHERE r."Name" IN ('DepartmentAdmin', 'SchoolAdmin')
ORDER BY u."UserName", r."Name";

-- Check Admin role permissions
SELECT "ProviderKey" as RoleName, "Name" as Permission
FROM "AbpPermissionGrants"
WHERE "ProviderName" = 'R' AND "ProviderKey" = 'Admin'
ORDER BY "Name";

