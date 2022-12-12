/* ---------------------------------------------------
-- 
--			サンプルデータベース作成
--
--------------------------------------------------- */

drop database if exists [SampleDatabase]
create database [SampleDatabase]
select
  [name]
from
  [sys].[databases]
go

use [SampleDatabase]
go

drop table if exists [Sweets]
create table [Sweets]
(
  [Id] int identity(1, 1),
  [Name] nvarchar(max),
  [Calorie] int,
)
