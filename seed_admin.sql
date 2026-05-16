-- Seed de Super Admin para OrdemServico
-- Email: superadmin@sistema.com
-- Senha: Admin@123456

INSERT INTO AspNetUsers (
    Id, 
    UserName, 
    NormalizedUserName, 
    Email, 
    NormalizedEmail, 
    EmailConfirmed, 
    PasswordHash, 
    SecurityStamp, 
    ConcurrencyStamp, 
    PhoneNumber, 
    PhoneNumberConfirmed, 
    TwoFactorEnabled, 
    LockoutEnabled, 
    AccessFailedCount
) VALUES (
    '550e8400-e29b-41d4-a716-446655440000',
    'superadmin@sistema.com',
    'SUPERADMIN@SISTEMA.COM',
    'superadmin@sistema.com',
    'SUPERADMIN@SISTEMA.COM',
    1,
    'AQAAAAIAAYagAAAAEKs5w6Q8r8J0dj2XY2XcjF3PaFUQxLdJnkM7pQDvxL9B+n1r7V+gYh9v3v7YvEQXUQ==',
    'TESTINGSECURITYSTAMP',
    'TESTINGCONCURRENCYSTAMP',
    NULL,
    0,
    0,
    1,
    0
) ON DUPLICATE KEY UPDATE Email=Email;

-- Associar role SuperAdmin existente ao novo usuário
INSERT INTO AspNetUserRoles (UserId, RoleId)
VALUES (
    '550e8400-e29b-41d4-a716-446655440000',
    '1b53dfff-83b7-4be3-b295-8f027572f0d9'
) ON DUPLICATE KEY UPDATE UserId=UserId;

SELECT 'Super Admin Seed Completo!' AS Status;
SELECT Email, UserName FROM AspNetUsers;
