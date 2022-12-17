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

use [SampleDatabase]

insert into
  [Sweets] ([Name], [Calorie])
values 
  (N'いちごケーキ', 500),
  (N'チョコレートケーキ', 800),
  (N'赤福', 900),
  (N'アイスクリーム', 1500),
  (N'なんとかフラペチーノ', 99999999)

select
  *
from
  [Sweets]
