namespace CommitteeManagement.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class New : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BogusObjects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Committees",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Criteria",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Subject = c.String(nullable: false),
                        Coefficient = c.Double(nullable: false),
                        IsDeleted = c.Boolean(),
                        CommitteeId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Committees", t => t.CommitteeId)
                .Index(t => t.CommitteeId);
            
            CreateTable(
                "dbo.CriterionModifications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Time = c.DateTime(),
                        Update = c.Boolean(),
                        Add = c.Boolean(),
                        Delete = c.Boolean(),
                        UserId = c.String(maxLength: 128),
                        CriterionId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Criteria", t => t.CriterionId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.CriterionId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Gender = c.Int(nullable: false),
                        Name = c.String(),
                        LastName = c.String(),
                        CreatedTime = c.DateTime(),
                        IsActive = c.Boolean(),
                        LastModificationDate = c.DateTime(),
                        CommitteeRefId = c.Int(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Committees", t => t.CommitteeRefId)
                .Index(t => t.CommitteeRefId)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.ContactInfoes",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        City = c.String(),
                        Region = c.String(),
                        Address1 = c.String(nullable: false),
                        Address2 = c.String(),
                        PhotoLink = c.String(),
                        ModifiedTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.IndicatorModifications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Time = c.DateTime(),
                        AddIndicator = c.Boolean(),
                        UpdateIndicator = c.Boolean(),
                        DeleteIndicator = c.Boolean(),
                        AddRealValue = c.Boolean(),
                        UpdateRealValue = c.Boolean(),
                        AddIdealValue = c.Boolean(),
                        UpdateIdealValue = c.Boolean(),
                        UserId = c.String(maxLength: 128),
                        IndicatorId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Indicators", t => t.IndicatorId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.IndicatorId);
            
            CreateTable(
                "dbo.Indicators",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Subject = c.String(nullable: false),
                        Coefficient = c.Double(nullable: false),
                        DeadlinePeriod = c.Int(nullable: false),
                        IsDeleted = c.Boolean(),
                        SubCriterionId = c.Int(),
                        CommitteeId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Committees", t => t.CommitteeId)
                .ForeignKey("dbo.SubCriterions", t => t.SubCriterionId)
                .Index(t => t.SubCriterionId)
                .Index(t => t.CommitteeId);
            
            CreateTable(
                "dbo.IndicatorIdealValues",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Value = c.Double(nullable: false),
                        Time = c.DateTime(),
                        LowerThan = c.Boolean(),
                        MoreThan = c.Boolean(),
                        IndicatorId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Indicators", t => t.IndicatorId)
                .Index(t => t.IndicatorId);
            
            CreateTable(
                "dbo.IndicatorRealValues",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Value = c.Double(nullable: false),
                        Time = c.DateTime(),
                        IndicatorId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Indicators", t => t.IndicatorId)
                .Index(t => t.IndicatorId);
            
            CreateTable(
                "dbo.Permissions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IndicatorDeadlineAdjust = c.Boolean(),
                        Criterion = c.Boolean(),
                        SubCriterion = c.Boolean(),
                        Indicator = c.Boolean(),
                        RealIndicator = c.Boolean(),
                        Add = c.Boolean(),
                        Delete = c.Boolean(),
                        Update = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex")
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.SubCriterions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Subject = c.String(),
                        Coefficient = c.Double(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CriterionId = c.Int(nullable: false),
                        CommitteeId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Committees", t => t.CommitteeId)
                .ForeignKey("dbo.Criteria", t => t.CriterionId, cascadeDelete: true)
                .Index(t => t.CriterionId)
                .Index(t => t.CommitteeId);
            
            CreateTable(
                "dbo.SubCriterionModifications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Time = c.DateTime(),
                        Update = c.Boolean(),
                        Add = c.Boolean(),
                        Delete = c.Boolean(),
                        UserId = c.String(maxLength: 128),
                        SubCriterionId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SubCriterions", t => t.SubCriterionId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.SubCriterionId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Sessions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HostAddress = c.String(),
                        SessionKey = c.String(),
                        LoginTime = c.DateTime(),
                        LogoutTime = c.DateTime(),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Visitors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IpAddress = c.String(),
                        Browser = c.String(),
                        BrowserVersion = c.String(),
                        Time = c.DateTime(),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.PermissionCriterions",
                c => new
                    {
                        Permission_Id = c.Int(nullable: false),
                        Criterion_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Permission_Id, t.Criterion_Id })
                .ForeignKey("dbo.Permissions", t => t.Permission_Id, cascadeDelete: true)
                .ForeignKey("dbo.Criteria", t => t.Criterion_Id, cascadeDelete: true)
                .Index(t => t.Permission_Id)
                .Index(t => t.Criterion_Id);
            
            CreateTable(
                "dbo.PermissionIndicators",
                c => new
                    {
                        Permission_Id = c.Int(nullable: false),
                        Indicator_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Permission_Id, t.Indicator_Id })
                .ForeignKey("dbo.Permissions", t => t.Permission_Id, cascadeDelete: true)
                .ForeignKey("dbo.Indicators", t => t.Indicator_Id, cascadeDelete: true)
                .Index(t => t.Permission_Id)
                .Index(t => t.Indicator_Id);
            
            CreateTable(
                "dbo.RolePermissions",
                c => new
                    {
                        Role_Id = c.String(nullable: false, maxLength: 128),
                        Permission_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Role_Id, t.Permission_Id })
                .ForeignKey("dbo.AspNetRoles", t => t.Role_Id, cascadeDelete: true)
                .ForeignKey("dbo.Permissions", t => t.Permission_Id, cascadeDelete: true)
                .Index(t => t.Role_Id)
                .Index(t => t.Permission_Id);
            
            CreateTable(
                "dbo.SubCriterionPermissions",
                c => new
                    {
                        SubCriterion_Id = c.Int(nullable: false),
                        Permission_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.SubCriterion_Id, t.Permission_Id })
                .ForeignKey("dbo.SubCriterions", t => t.SubCriterion_Id, cascadeDelete: true)
                .ForeignKey("dbo.Permissions", t => t.Permission_Id, cascadeDelete: true)
                .Index(t => t.SubCriterion_Id)
                .Index(t => t.Permission_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Visitors", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Sessions", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetRoles", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.IndicatorModifications", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.SubCriterionModifications", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.SubCriterionModifications", "SubCriterionId", "dbo.SubCriterions");
            DropForeignKey("dbo.SubCriterionPermissions", "Permission_Id", "dbo.Permissions");
            DropForeignKey("dbo.SubCriterionPermissions", "SubCriterion_Id", "dbo.SubCriterions");
            DropForeignKey("dbo.Indicators", "SubCriterionId", "dbo.SubCriterions");
            DropForeignKey("dbo.SubCriterions", "CriterionId", "dbo.Criteria");
            DropForeignKey("dbo.SubCriterions", "CommitteeId", "dbo.Committees");
            DropForeignKey("dbo.RolePermissions", "Permission_Id", "dbo.Permissions");
            DropForeignKey("dbo.RolePermissions", "Role_Id", "dbo.AspNetRoles");
            DropForeignKey("dbo.PermissionIndicators", "Indicator_Id", "dbo.Indicators");
            DropForeignKey("dbo.PermissionIndicators", "Permission_Id", "dbo.Permissions");
            DropForeignKey("dbo.PermissionCriterions", "Criterion_Id", "dbo.Criteria");
            DropForeignKey("dbo.PermissionCriterions", "Permission_Id", "dbo.Permissions");
            DropForeignKey("dbo.IndicatorRealValues", "IndicatorId", "dbo.Indicators");
            DropForeignKey("dbo.IndicatorModifications", "IndicatorId", "dbo.Indicators");
            DropForeignKey("dbo.IndicatorIdealValues", "IndicatorId", "dbo.Indicators");
            DropForeignKey("dbo.Indicators", "CommitteeId", "dbo.Committees");
            DropForeignKey("dbo.CriterionModifications", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ContactInfoes", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "CommitteeRefId", "dbo.Committees");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.CriterionModifications", "CriterionId", "dbo.Criteria");
            DropForeignKey("dbo.Criteria", "CommitteeId", "dbo.Committees");
            DropIndex("dbo.SubCriterionPermissions", new[] { "Permission_Id" });
            DropIndex("dbo.SubCriterionPermissions", new[] { "SubCriterion_Id" });
            DropIndex("dbo.RolePermissions", new[] { "Permission_Id" });
            DropIndex("dbo.RolePermissions", new[] { "Role_Id" });
            DropIndex("dbo.PermissionIndicators", new[] { "Indicator_Id" });
            DropIndex("dbo.PermissionIndicators", new[] { "Permission_Id" });
            DropIndex("dbo.PermissionCriterions", new[] { "Criterion_Id" });
            DropIndex("dbo.PermissionCriterions", new[] { "Permission_Id" });
            DropIndex("dbo.Visitors", new[] { "UserId" });
            DropIndex("dbo.Sessions", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.SubCriterionModifications", new[] { "SubCriterionId" });
            DropIndex("dbo.SubCriterionModifications", new[] { "UserId" });
            DropIndex("dbo.SubCriterions", new[] { "CommitteeId" });
            DropIndex("dbo.SubCriterions", new[] { "CriterionId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", new[] { "User_Id" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.IndicatorRealValues", new[] { "IndicatorId" });
            DropIndex("dbo.IndicatorIdealValues", new[] { "IndicatorId" });
            DropIndex("dbo.Indicators", new[] { "CommitteeId" });
            DropIndex("dbo.Indicators", new[] { "SubCriterionId" });
            DropIndex("dbo.IndicatorModifications", new[] { "IndicatorId" });
            DropIndex("dbo.IndicatorModifications", new[] { "UserId" });
            DropIndex("dbo.ContactInfoes", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUsers", new[] { "CommitteeRefId" });
            DropIndex("dbo.CriterionModifications", new[] { "CriterionId" });
            DropIndex("dbo.CriterionModifications", new[] { "UserId" });
            DropIndex("dbo.Criteria", new[] { "CommitteeId" });
            DropTable("dbo.SubCriterionPermissions");
            DropTable("dbo.RolePermissions");
            DropTable("dbo.PermissionIndicators");
            DropTable("dbo.PermissionCriterions");
            DropTable("dbo.Visitors");
            DropTable("dbo.Sessions");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.SubCriterionModifications");
            DropTable("dbo.SubCriterions");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Permissions");
            DropTable("dbo.IndicatorRealValues");
            DropTable("dbo.IndicatorIdealValues");
            DropTable("dbo.Indicators");
            DropTable("dbo.IndicatorModifications");
            DropTable("dbo.ContactInfoes");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.CriterionModifications");
            DropTable("dbo.Criteria");
            DropTable("dbo.Committees");
            DropTable("dbo.BogusObjects");
        }
    }
}
