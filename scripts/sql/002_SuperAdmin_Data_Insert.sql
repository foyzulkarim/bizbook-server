USE [BizBookDb]
GO

INSERT INTO [dbo].[AspNetUsers]
           ([Id]
           ,[FirstName]
           ,[LastName]
           ,[IsActive]
           ,[PhoneNumber]
           ,[ShopId]
           ,[RoleName]
           ,[Email]
           ,[EmailConfirmed]
           ,[PasswordHash]
           ,[SecurityStamp]
           ,[PhoneNumberConfirmed]
           ,[TwoFactorEnabled]
           ,[LockoutEndDateUtc]
           ,[LockoutEnabled]
           ,[AccessFailedCount]
           ,[UserName])
     VALUES
           ('8f9dc73a-f065-47c6-ac83-4c0db1a74a06'
           ,'Super'
           ,'Admin'
           ,1
           ,'+8801968569097'
           ,'00000000-0000-0000-0000-000000000000'
           ,'SuperAdmin'
           ,'foyzulkarim@gmail.com'
           ,1
           ,'AGJNOoecZasTJYZZFImChiYCg4U3KtqL2adkLuLGmVeX14jhVG9FTsQgLujDY4r8kw=='
           ,'b22709f1-3ab9-4b50-a606-93ae682749f3'
           ,1
           ,0
           ,NULL 
           ,0
           ,0
           ,'foyzulkarim@gmail.com')

INSERT INTO [dbo].[AspNetUserRoles]
           ([UserId]
           ,[RoleId])
     VALUES
           ('8f9dc73a-f065-47c6-ac83-4c0db1a74a06'
           ,'3360c5a0-6526-4450-9a3d-f9b91e1ccf92')
GO