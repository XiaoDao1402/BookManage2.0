    use db_bookmanage


--管理员表
if Exists(select top 1 * from sysObjects where Id=OBJECT_ID(N't_admin') and xtype='U')
    drop table t_admin;
create table t_admin(
	AdminId int primary key identity(10001,1),	--管理员id
	Name nvarchar(32) not null,				--名称
	Account varchar(16) not null,			--账号
	Password varchar(32) not null,			--密码
	CreateDate datetime default getdate(), --创建时间
	ModifyDate datetime default getdate()  --修改时间
);

--用户表
if Exists(select top 1 * from sysObjects where Id=OBJECT_ID(N't_user') and xtype='U')
    drop table t_user;
create table t_user(
	UserId int primary key identity(10001,1),	--用户id
	Name nvarchar(32) not null,				--用户名
	Account varchar(32) not null,			--账号
	[Password] varchar(32) not null,		--密码
	TotalCount int not null,				--总借书记录
	CreateDate datetime default getdate(), --创建时间
	ModifyDate datetime default getdate()  --修改时间
);

--图书表
if Exists(select top 1 * from sysObjects where Id=OBJECT_ID(N't_book') and xtype='U')
    drop table t_book;
create table t_book(
	BookId int primary key identity(10001,1),	--图书id
	Name nvarchar(32) not null,				--图书名称
	BookCategoryId int not null,			--图书分类id
	CoverImage varchar(128) not null,		--图书封面
	Price decimal(4,1) not null,			--图书价格
	BorrowNum int default 0,				--借书次数
	TotalStockCount int not null,			--总库存
	SurplusStockCount int not null,			--剩余库存
	AdminId int not null,					--管理员id
	CreateDate datetime default getdate(), --创建时间
	ModifyDate datetime default getdate()  --修改时间
);

--图书分类表
if Exists(select top 1 * from sysObjects where Id=OBJECT_ID(N't_book_category') and xtype='U')
    drop table t_book_category;
create table t_book_category(
	BookCategoryId int primary key identity(10001,1),	--图书分类id
	Name nvarchar(32) not null,				--图书分类名称
	ParentId int default 0,					--父id
	CreateDate datetime default getdate(), --创建时间
	ModifyDate datetime default getdate()  --修改时间
);

--用户借书记录表
if Exists(select top 1 * from sysObjects where Id=OBJECT_ID(N't_user_book') and xtype='U')
    drop table t_user_book;
create table t_user_book(
	UserBookId int primary key identity(10001,1),	--用户借书id
	UserId int not null,							--用户id
	BookId int not null,							--图书id
	[State] int not null,							--状态 1.借书中 2.已还书
	CreateDate datetime default getdate(),	--创建时间
	ModifyDate datetime default getdate(),  --修改时间
	ReturnDate varchar(30)					--还书时间
);

--用户还书记录表
if Exists(select top 1 * from sysObjects where Id=OBJECT_ID(N't_user_book_return') and xtype='U')
    drop table t_user_book_return;
create table t_user_book_return(
	ReturnId int primary key identity(10001,1),		--用户还书id
	UserId int not null,							--用户id
	BookId int not null,							--图书id
	CreateDate datetime default getdate(), --创建时间
	ModifyDate datetime default getdate()  --修改时间
);

