CREATE DATABASE sales_db;

DROP TABLE "AspNetRoleClaims";
DROP TABLE "AspNetUserClaims";
DROP TABLE "AspNetRoles" CASCADE;
DROP TABLE "AspNetUserLogins";
DROP TABLE "AspNetUserRoles";
DROP TABLE "AspNetUsers" CASCADE;
DROP TABLE "AspNetUserTokens";


-- Creating an AspNetRoles table if it doesn't exist
CREATE TABLE IF NOT EXISTS AspNetRoles
(
    Id               UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    Name             VARCHAR(255) NOT NULL,
    NormalizedName   VARCHAR(255) NOT NULL,
    ConcurrencyStamp UUID             DEFAULT uuid_generate_v4()
);

-- Create administrator role
INSERT INTO public."AspNetRoles" ("Id", "Name", "NormalizedName", "ConcurrencyStamp")
VALUES ('1', 'Admin', 'ADMIN', null);


-- Assign administrator role user
INSERT INTO public."AspNetUserRoles" ("UserId", "RoleId")
VALUES ('eeffa3db-d36a-40c5-a0ba-ca2b7cacb646', '1');
