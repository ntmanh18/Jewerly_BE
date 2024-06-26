USE [master]
GO
/****** Object:  Database [Jewerly_v6]    Script Date: 5/29/2024 1:03:10 AM ******/
CREATE DATABASE [Jewerly_v6]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Jewerly_v6', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\Jewerly_v6.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Jewerly_v6_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\Jewerly_v6_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [Jewerly_v6] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Jewerly_v6].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Jewerly_v6] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Jewerly_v6] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Jewerly_v6] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Jewerly_v6] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Jewerly_v6] SET ARITHABORT OFF 
GO
ALTER DATABASE [Jewerly_v6] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Jewerly_v6] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Jewerly_v6] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Jewerly_v6] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Jewerly_v6] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Jewerly_v6] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Jewerly_v6] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Jewerly_v6] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Jewerly_v6] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Jewerly_v6] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Jewerly_v6] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Jewerly_v6] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Jewerly_v6] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Jewerly_v6] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Jewerly_v6] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Jewerly_v6] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Jewerly_v6] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Jewerly_v6] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [Jewerly_v6] SET  MULTI_USER 
GO
ALTER DATABASE [Jewerly_v6] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Jewerly_v6] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Jewerly_v6] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Jewerly_v6] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Jewerly_v6] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [Jewerly_v6] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [Jewerly_v6] SET QUERY_STORE = ON
GO
ALTER DATABASE [Jewerly_v6] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [Jewerly_v6]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 5/29/2024 1:03:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Bill]    Script Date: 5/29/2024 1:03:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Bill](
	[BillId] [varchar](10) NOT NULL,
	[TotalCost] [bigint] NOT NULL,
	[PublishDay] [datetime] NOT NULL,
	[CustomerCustomerID] [varchar](10) NULL,
	[VoucherVoucherId] [varchar](10) NULL,
	[CashierID] [varchar](10) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[BillId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Cashier]    Script Date: 5/29/2024 1:03:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cashier](
	[CashId] [varchar](10) NOT NULL,
	[StartCash] [datetime] NOT NULL,
	[EndCash] [datetime] NOT NULL,
	[Income] [bigint] NOT NULL,
	[CashNumber] [int] NOT NULL,
	[UserId] [varchar](10) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CashId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Customer]    Script Date: 5/29/2024 1:03:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customer](
	[CustomerID] [varchar](10) NOT NULL,
	[FullName] [varchar](255) NOT NULL,
	[DoB] [date] NOT NULL,
	[Address] [varchar](255) NOT NULL,
	[Email] [varchar](255) NOT NULL,
	[Phone] [varchar](10) NOT NULL,
	[Point] [int] NOT NULL,
	[Rate] [varchar](255) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CustomerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Discount]    Script Date: 5/29/2024 1:03:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Discount](
	[DiscountId] [varchar](10) NOT NULL,
	[CreatedBy] [varchar](10) NOT NULL,
	[ExpiredDay] [date] NOT NULL,
	[PublishDay] [date] NOT NULL,
	[Amount] [int] NOT NULL,
	[Cost] [bigint] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[DiscountId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Discount_Product]    Script Date: 5/29/2024 1:03:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Discount_Product](
	[DiscountDiscountId] [varchar](10) NOT NULL,
	[ProductProductID] [varchar](10) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[DiscountDiscountId] ASC,
	[ProductProductID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Gem]    Script Date: 5/29/2024 1:03:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Gem](
	[GemId] [varchar](10) NOT NULL,
	[Name] [varchar](255) NOT NULL,
	[Type] [int] NOT NULL,
	[Price] [bigint] NOT NULL,
	[Desc] [text] NULL,
PRIMARY KEY CLUSTERED 
(
	[GemId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Gold]    Script Date: 5/29/2024 1:03:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Gold](
	[GoldId] [varchar](10) NOT NULL,
	[GoldName] [varchar](255) NULL,
	[PurchasePrice] [bigint] NOT NULL,
	[SalePrice] [bigint] NOT NULL,
	[ModifiedBy] [varchar](10) NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[GoldId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OldProduct]    Script Date: 5/29/2024 1:03:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OldProduct](
	[OProductID] [varchar](10) NOT NULL,
	[ProductProductID] [varchar](10) NOT NULL,
	[Desc] [varchar](255) NULL,
	[BillBillId] [varchar](10) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[OProductID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Product]    Script Date: 5/29/2024 1:03:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Product](
	[ProductID] [varchar](10) NOT NULL,
	[ProductName] [varchar](255) NOT NULL,
	[Category] [varchar](255) NOT NULL,
	[Material] [varchar](10) NOT NULL,
	[Weight] [real] NOT NULL,
	[MachiningCost] [bigint] NOT NULL,
	[Size] [real] NOT NULL,
	[Amount] [int] NOT NULL,
	[Desc] [text] NULL,
	[Image] [varchar](255) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ProductID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Product_Bill]    Script Date: 5/29/2024 1:03:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Product_Bill](
	[ProductProductID] [varchar](10) NOT NULL,
	[BillBillId] [varchar](10) NOT NULL,
	[Amount] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ProductProductID] ASC,
	[BillBillId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Product_Gem]    Script Date: 5/29/2024 1:03:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Product_Gem](
	[ProductProductID] [varchar](10) NOT NULL,
	[GemGemId] [varchar](10) NOT NULL,
	[Amount] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ProductProductID] ASC,
	[GemGemId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 5/29/2024 1:03:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[UserID] [varchar](10) NOT NULL,
	[Username] [varchar](50) NOT NULL,
	[Password] [varchar](255) NOT NULL,
	[Role] [int] NOT NULL,
	[FullName] [varchar](100) NOT NULL,
	[DoB] [date] NOT NULL,
	[Phone] [varchar](10) NOT NULL,
	[Address] [varchar](255) NOT NULL,
	[Status] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Voucher]    Script Date: 5/29/2024 1:03:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Voucher](
	[VoucherId] [varchar](10) NOT NULL,
	[CreatedBy] [varchar](10) NOT NULL,
	[ExpiredDay] [date] NOT NULL,
	[PublishedDay] [date] NOT NULL,
	[Cost] [bigint] NOT NULL,
	[CustomerCustomerID] [varchar](10) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[VoucherId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Warranty]    Script Date: 5/29/2024 1:03:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Warranty](
	[WarrantyID] [varchar](10) NOT NULL,
	[CustomerCustomerID] [varchar](10) NOT NULL,
	[StartDate] [date] NOT NULL,
	[ExpiredDate] [date] NOT NULL,
	[Desc] [varchar](255) NULL,
	[ProductID] [varchar](10) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[WarrantyID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[Bill] ([BillId], [TotalCost], [PublishDay], [CustomerCustomerID], [VoucherVoucherId], [CashierID]) VALUES (N'B001', 3000000, CAST(N'2024-05-06T08:00:00.000' AS DateTime), N'C001', N'V001', N'CH001')
INSERT [dbo].[Bill] ([BillId], [TotalCost], [PublishDay], [CustomerCustomerID], [VoucherVoucherId], [CashierID]) VALUES (N'B002', 2000000, CAST(N'2024-05-07T08:00:00.000' AS DateTime), N'C002', N'V002', N'CH002')
INSERT [dbo].[Bill] ([BillId], [TotalCost], [PublishDay], [CustomerCustomerID], [VoucherVoucherId], [CashierID]) VALUES (N'B003', 3000000, CAST(N'2024-05-06T08:00:00.000' AS DateTime), N'C003', NULL, N'CH003')
INSERT [dbo].[Bill] ([BillId], [TotalCost], [PublishDay], [CustomerCustomerID], [VoucherVoucherId], [CashierID]) VALUES (N'B004', 3000000, CAST(N'2024-05-06T08:00:00.000' AS DateTime), N'C004', N'V003', N'CH003')
INSERT [dbo].[Bill] ([BillId], [TotalCost], [PublishDay], [CustomerCustomerID], [VoucherVoucherId], [CashierID]) VALUES (N'B005', 7500000, CAST(N'2024-05-06T08:00:00.000' AS DateTime), N'C002', NULL, N'CH004')
GO
INSERT [dbo].[Cashier] ([CashId], [StartCash], [EndCash], [Income], [CashNumber], [UserId]) VALUES (N'CH001', CAST(N'2024-05-03T08:00:00.000' AS DateTime), CAST(N'2024-05-03T18:00:00.000' AS DateTime), 3000000, 1, N'USR001')
INSERT [dbo].[Cashier] ([CashId], [StartCash], [EndCash], [Income], [CashNumber], [UserId]) VALUES (N'CH002', CAST(N'2024-05-03T08:00:00.000' AS DateTime), CAST(N'2024-05-03T18:00:00.000' AS DateTime), 2000000, 2, N'USR002')
INSERT [dbo].[Cashier] ([CashId], [StartCash], [EndCash], [Income], [CashNumber], [UserId]) VALUES (N'CH003', CAST(N'2024-05-03T08:00:00.000' AS DateTime), CAST(N'2024-05-03T18:00:00.000' AS DateTime), 6000000, 3, N'USR003')
INSERT [dbo].[Cashier] ([CashId], [StartCash], [EndCash], [Income], [CashNumber], [UserId]) VALUES (N'CH004', CAST(N'2024-05-04T08:00:00.000' AS DateTime), CAST(N'2024-05-04T18:00:00.000' AS DateTime), 7500000, 4, N'USR004')
GO
INSERT [dbo].[Customer] ([CustomerID], [FullName], [DoB], [Address], [Email], [Phone], [Point], [Rate]) VALUES (N'C001', N'John Doe', CAST(N'1990-05-20' AS Date), N'123 Main St, City, Country', N'john.doe@example.com', N'0234567890', 60, N'Ð?ng')
INSERT [dbo].[Customer] ([CustomerID], [FullName], [DoB], [Address], [Email], [Phone], [Point], [Rate]) VALUES (N'C002', N'Jane Smith', CAST(N'1995-10-15' AS Date), N'456 Elm St, City, Country', N'jane.smith@example.com', N'0987654321', 100, N'B?c')
INSERT [dbo].[Customer] ([CustomerID], [FullName], [DoB], [Address], [Email], [Phone], [Point], [Rate]) VALUES (N'C003', N'Bob Jones', CAST(N'1985-03-10' AS Date), N'789 Oak St, City, Country', N'bob.jones@example.com', N'0579246802', 150, N'Vàng')
INSERT [dbo].[Customer] ([CustomerID], [FullName], [DoB], [Address], [Email], [Phone], [Point], [Rate]) VALUES (N'C004', N'Alice Walker', CAST(N'1988-07-25' AS Date), N'321 Pine St, City, Country', N'alice.walker@example.com', N'0765432108', 220, N'Kim cuong')
GO
INSERT [dbo].[Discount] ([DiscountId], [CreatedBy], [ExpiredDay], [PublishDay], [Amount], [Cost]) VALUES (N'D002', N'USR002', CAST(N'2024-07-31' AS Date), CAST(N'2024-05-15' AS Date), 15, 750000)
INSERT [dbo].[Discount] ([DiscountId], [CreatedBy], [ExpiredDay], [PublishDay], [Amount], [Cost]) VALUES (N'D003', N'USR003', CAST(N'2024-08-31' AS Date), CAST(N'2024-06-01' AS Date), 20, 1000000)
INSERT [dbo].[Discount] ([DiscountId], [CreatedBy], [ExpiredDay], [PublishDay], [Amount], [Cost]) VALUES (N'D004', N'USR004', CAST(N'2024-09-30' AS Date), CAST(N'2024-07-01' AS Date), 25, 1250000)
INSERT [dbo].[Discount] ([DiscountId], [CreatedBy], [ExpiredDay], [PublishDay], [Amount], [Cost]) VALUES (N'D005', N'USR005', CAST(N'2024-10-31' AS Date), CAST(N'2024-08-01' AS Date), 30, 1500000)
GO
INSERT [dbo].[Discount_Product] ([DiscountDiscountId], [ProductProductID]) VALUES (N'D002', N'P002')
INSERT [dbo].[Discount_Product] ([DiscountDiscountId], [ProductProductID]) VALUES (N'D003', N'P003')
GO
INSERT [dbo].[Gem] ([GemId], [Name], [Type], [Price], [Desc]) VALUES (N'G001', N'Diamond', 2, 1000000, NULL)
INSERT [dbo].[Gem] ([GemId], [Name], [Type], [Price], [Desc]) VALUES (N'G002', N'Ruby', 1, 750000, NULL)
INSERT [dbo].[Gem] ([GemId], [Name], [Type], [Price], [Desc]) VALUES (N'G003', N'Emerald', 1, 900000, NULL)
INSERT [dbo].[Gem] ([GemId], [Name], [Type], [Price], [Desc]) VALUES (N'G004', N'Sapphire', 1, 800000, NULL)
INSERT [dbo].[Gem] ([GemId], [Name], [Type], [Price], [Desc]) VALUES (N'G005', N'Topaz', 2, 600000, NULL)
GO
INSERT [dbo].[Gold] ([GoldId], [GoldName], [PurchasePrice], [SalePrice], [ModifiedBy], [ModifiedDate]) VALUES (N'1', N'Vàng SJC 1L - 10L - 1KG', 88800, 90800, N'USR005', CAST(N'2024-05-20T11:19:39.000' AS DateTime))
INSERT [dbo].[Gold] ([GoldId], [GoldName], [PurchasePrice], [SalePrice], [ModifiedBy], [ModifiedDate]) VALUES (N'2', N'Vàng nh?n SJC 99,99 1 ch?, 2 ch?, 5 ch?', 75700, 77400, N'USR005', CAST(N'2024-05-20T11:19:39.000' AS DateTime))
INSERT [dbo].[Gold] ([GoldId], [GoldName], [PurchasePrice], [SalePrice], [ModifiedBy], [ModifiedDate]) VALUES (N'3', N'Vàng nh?n SJC 99,99 0,3 ch?, 0,5 ch?', 75700, 77500, N'USR005', CAST(N'2024-05-20T11:19:39.000' AS DateTime))
INSERT [dbo].[Gold] ([GoldId], [GoldName], [PurchasePrice], [SalePrice], [ModifiedBy], [ModifiedDate]) VALUES (N'4', N'Vàng n? trang 99,99%', 75600, 76600, N'USR005', CAST(N'2024-05-20T11:19:39.000' AS DateTime))
INSERT [dbo].[Gold] ([GoldId], [GoldName], [PurchasePrice], [SalePrice], [ModifiedBy], [ModifiedDate]) VALUES (N'5', N'Vàng n? trang 99%', 73842, 75842, N'USR005', CAST(N'2024-05-20T11:19:39.000' AS DateTime))
INSERT [dbo].[Gold] ([GoldId], [GoldName], [PurchasePrice], [SalePrice], [ModifiedBy], [ModifiedDate]) VALUES (N'6', N'Vàng n? trang 75%', 55106, 57606, N'USR005', CAST(N'2024-05-20T11:19:39.000' AS DateTime))
INSERT [dbo].[Gold] ([GoldId], [GoldName], [PurchasePrice], [SalePrice], [ModifiedBy], [ModifiedDate]) VALUES (N'7', N'Vàng n? trang 58,3%', 42312, 44812, N'USR005', CAST(N'2024-05-20T11:19:39.000' AS DateTime))
INSERT [dbo].[Gold] ([GoldId], [GoldName], [PurchasePrice], [SalePrice], [ModifiedBy], [ModifiedDate]) VALUES (N'8', N'Vàng n? trang 41,7%', 29595, 32095, N'USR005', CAST(N'2024-05-20T11:19:39.000' AS DateTime))
GO
INSERT [dbo].[OldProduct] ([OProductID], [ProductProductID], [Desc], [BillBillId]) VALUES (N'OP001', N'P001', N'Old version of Product P001', N'B001')
INSERT [dbo].[OldProduct] ([OProductID], [ProductProductID], [Desc], [BillBillId]) VALUES (N'OP002', N'P002', N'Older model of Product P002', N'B001')
GO
INSERT [dbo].[Product] ([ProductID], [ProductName], [Category], [Material], [Weight], [MachiningCost], [Size], [Amount], [Desc], [Image]) VALUES (N'P001', N'Gold Ring 1', N'Nh?n', N'1', 3.5, 350000, 2, 20, N'18k Gold Ring with diamond', N'ring1.jpg')
INSERT [dbo].[Product] ([ProductID], [ProductName], [Category], [Material], [Weight], [MachiningCost], [Size], [Amount], [Desc], [Image]) VALUES (N'P002', N'Gold Bracelet 1', N'Vòng tay', N'2', 4.2, 450000, 3.5, 15, N'Gold bracelet with intricate design', N'bracelet1.jpg')
INSERT [dbo].[Product] ([ProductID], [ProductName], [Category], [Material], [Weight], [MachiningCost], [Size], [Amount], [Desc], [Image]) VALUES (N'P003', N'Gold Necklace 1', N'Dây chuy?n', N'3', 5, 500000, 4, 10, N'Gold necklace with pendant', N'necklace1.jpg')
INSERT [dbo].[Product] ([ProductID], [ProductName], [Category], [Material], [Weight], [MachiningCost], [Size], [Amount], [Desc], [Image]) VALUES (N'P004', N'Gold Pendant 1', N'M?t dây chuy?n', N'4', 2.3, 320000, 1.5, 25, N'Gold pendant with gemstone', N'pendant1.jpg')
INSERT [dbo].[Product] ([ProductID], [ProductName], [Category], [Material], [Weight], [MachiningCost], [Size], [Amount], [Desc], [Image]) VALUES (N'P005', N'Gold Anklet 1', N'L?c chân', N'5', 3.8, 400000, 2.5, 12, N'Gold anklet with charm', N'anklet1.jpg')
INSERT [dbo].[Product] ([ProductID], [ProductName], [Category], [Material], [Weight], [MachiningCost], [Size], [Amount], [Desc], [Image]) VALUES (N'P006', N'Gold Charm 1', N'Charm', N'6', 1.9, 310000, 1.2, 28, N'Gold charm for bracelet', N'charm1.jpg')
INSERT [dbo].[Product] ([ProductID], [ProductName], [Category], [Material], [Weight], [MachiningCost], [Size], [Amount], [Desc], [Image]) VALUES (N'P007', N'Gold Ring 2', N'Nh?n', N'7', 3.6, 360000, 2.1, 21, N'Gold Ring with intricate design', N'ring2.jpg')
INSERT [dbo].[Product] ([ProductID], [ProductName], [Category], [Material], [Weight], [MachiningCost], [Size], [Amount], [Desc], [Image]) VALUES (N'P008', N'Gold Bracelet 2', N'Vòng tay', N'8', 4.3, 460000, 3.6, 16, N'Elegant gold bracelet', N'bracelet2.jpg')
INSERT [dbo].[Product] ([ProductID], [ProductName], [Category], [Material], [Weight], [MachiningCost], [Size], [Amount], [Desc], [Image]) VALUES (N'P009', N'Gold Necklace 2', N'Dây chuy?n', N'1', 5.1, 510000, 4.1, 11, N'Gold necklace with pearls', N'necklace2.jpg')
INSERT [dbo].[Product] ([ProductID], [ProductName], [Category], [Material], [Weight], [MachiningCost], [Size], [Amount], [Desc], [Image]) VALUES (N'P010', N'Gold Pendant 2', N'M?t dây chuy?n', N'2', 2.4, 330000, 1.6, 26, N'Stylish gold pendant', N'pendant2.jpg')
INSERT [dbo].[Product] ([ProductID], [ProductName], [Category], [Material], [Weight], [MachiningCost], [Size], [Amount], [Desc], [Image]) VALUES (N'P011', N'Gold Anklet 2', N'L?c chân', N'3', 3.9, 410000, 2.6, 13, N'Beautiful gold anklet', N'anklet2.jpg')
INSERT [dbo].[Product] ([ProductID], [ProductName], [Category], [Material], [Weight], [MachiningCost], [Size], [Amount], [Desc], [Image]) VALUES (N'P012', N'Gold Charm 2', N'Charm', N'4', 2, 320000, 1.3, 29, N'Gold charm with unique design', N'charm2.jpg')
INSERT [dbo].[Product] ([ProductID], [ProductName], [Category], [Material], [Weight], [MachiningCost], [Size], [Amount], [Desc], [Image]) VALUES (N'P013', N'Gold Ring 3', N'Nh?n', N'5', 3.7, 370000, 2.2, 22, N'Gold ring with sapphire', N'ring3.jpg')
INSERT [dbo].[Product] ([ProductID], [ProductName], [Category], [Material], [Weight], [MachiningCost], [Size], [Amount], [Desc], [Image]) VALUES (N'P014', N'Gold Bracelet 3', N'Vòng tay', N'6', 4.4, 470000, 3.7, 17, N'Gold bracelet with diamonds', N'bracelet3.jpg')
INSERT [dbo].[Product] ([ProductID], [ProductName], [Category], [Material], [Weight], [MachiningCost], [Size], [Amount], [Desc], [Image]) VALUES (N'P015', N'Gold Necklace 3', N'Dây chuy?n', N'7', 5.2, 520000, 4.2, 12, N'Elegant gold necklace', N'necklace3.jpg')
INSERT [dbo].[Product] ([ProductID], [ProductName], [Category], [Material], [Weight], [MachiningCost], [Size], [Amount], [Desc], [Image]) VALUES (N'P016', N'Gold Pendant 3', N'M?t dây chuy?n', N'8', 2.5, 340000, 1.7, 27, N'Gold pendant with emerald', N'pendant3.jpg')
INSERT [dbo].[Product] ([ProductID], [ProductName], [Category], [Material], [Weight], [MachiningCost], [Size], [Amount], [Desc], [Image]) VALUES (N'P017', N'Gold Anklet 3', N'L?c chân', N'1', 4, 420000, 2.7, 14, N'Gold anklet with bells', N'anklet3.jpg')
INSERT [dbo].[Product] ([ProductID], [ProductName], [Category], [Material], [Weight], [MachiningCost], [Size], [Amount], [Desc], [Image]) VALUES (N'P018', N'Gold Charm 3', N'Charm', N'2', 2.1, 330000, 1.4, 30, N'Gold charm with intricate patterns', N'charm3.jpg')
INSERT [dbo].[Product] ([ProductID], [ProductName], [Category], [Material], [Weight], [MachiningCost], [Size], [Amount], [Desc], [Image]) VALUES (N'P019', N'Gold Ring 4', N'Nh?n', N'3', 3.8, 380000, 2.3, 23, N'Gold ring with ruby', N'ring4.jpg')
INSERT [dbo].[Product] ([ProductID], [ProductName], [Category], [Material], [Weight], [MachiningCost], [Size], [Amount], [Desc], [Image]) VALUES (N'P020', N'Gold Bracelet 4', N'Vòng tay', N'4', 4.5, 480000, 3.8, 18, N'Gold bracelet with engraved design', N'bracelet4.jpg')
INSERT [dbo].[Product] ([ProductID], [ProductName], [Category], [Material], [Weight], [MachiningCost], [Size], [Amount], [Desc], [Image]) VALUES (N'P021', N'Gold Necklace 4', N'Dây chuy?n', N'5', 5.3, 530000, 4.3, 13, N'Gold necklace with amethyst', N'necklace4.jpg')
INSERT [dbo].[Product] ([ProductID], [ProductName], [Category], [Material], [Weight], [MachiningCost], [Size], [Amount], [Desc], [Image]) VALUES (N'P022', N'Gold Pendant 4', N'M?t dây chuy?n', N'6', 2.6, 350000, 1.8, 28, N'Gold pendant with topaz', N'pendant4.jpg')
INSERT [dbo].[Product] ([ProductID], [ProductName], [Category], [Material], [Weight], [MachiningCost], [Size], [Amount], [Desc], [Image]) VALUES (N'P023', N'Gold Anklet 4', N'L?c chân', N'7', 4.1, 430000, 2.8, 15, N'Gold anklet with pearls', N'anklet4.jpg')
INSERT [dbo].[Product] ([ProductID], [ProductName], [Category], [Material], [Weight], [MachiningCost], [Size], [Amount], [Desc], [Image]) VALUES (N'P024', N'Gold Charm 4', N'Charm', N'8', 2.2, 340000, 1.5, 31, N'Gold charm with animal motifs', N'charm4.jpg')
INSERT [dbo].[Product] ([ProductID], [ProductName], [Category], [Material], [Weight], [MachiningCost], [Size], [Amount], [Desc], [Image]) VALUES (N'P025', N'Gold Ring 5', N'Nh?n', N'1', 3.9, 390000, 2.4, 24, N'Gold ring with intricate carvings', N'ring5.jpg')
INSERT [dbo].[Product] ([ProductID], [ProductName], [Category], [Material], [Weight], [MachiningCost], [Size], [Amount], [Desc], [Image]) VALUES (N'P026', N'Gold Bracelet 5', N'Vòng tay', N'2', 4.6, 490000, 3.9, 19, N'Gold bracelet with floral patterns', N'bracelet5.jpg')
INSERT [dbo].[Product] ([ProductID], [ProductName], [Category], [Material], [Weight], [MachiningCost], [Size], [Amount], [Desc], [Image]) VALUES (N'P027', N'Gold Necklace 5', N'Dây chuy?n', N'3', 5.4, 540000, 4.4, 14, N'Gold necklace with heart pendant', N'necklace5.jpg')
INSERT [dbo].[Product] ([ProductID], [ProductName], [Category], [Material], [Weight], [MachiningCost], [Size], [Amount], [Desc], [Image]) VALUES (N'P028', N'Gold Pendant 5', N'M?t dây chuy?n', N'4', 2.7, 360000, 1.9, 29, N'Gold pendant with onyx', N'pendant5.jpg')
INSERT [dbo].[Product] ([ProductID], [ProductName], [Category], [Material], [Weight], [MachiningCost], [Size], [Amount], [Desc], [Image]) VALUES (N'P029', N'Gold Anklet 5', N'L?c chân', N'5', 4.2, 440000, 2.9, 16, N'Gold anklet with diamond', N'anklet5.jpg')
INSERT [dbo].[Product] ([ProductID], [ProductName], [Category], [Material], [Weight], [MachiningCost], [Size], [Amount], [Desc], [Image]) VALUES (N'P030', N'Gold Charm 5', N'Charm', N'6', 2.3, 350000, 1.6, 32, N'Gold charm with initials', N'charm5.jpg')
GO
INSERT [dbo].[Product_Bill] ([ProductProductID], [BillBillId], [Amount]) VALUES (N'P001', N'B001', 1)
INSERT [dbo].[Product_Bill] ([ProductProductID], [BillBillId], [Amount]) VALUES (N'P001', N'B003', 1)
INSERT [dbo].[Product_Bill] ([ProductProductID], [BillBillId], [Amount]) VALUES (N'P002', N'B001', 2)
INSERT [dbo].[Product_Bill] ([ProductProductID], [BillBillId], [Amount]) VALUES (N'P002', N'B003', 3)
INSERT [dbo].[Product_Bill] ([ProductProductID], [BillBillId], [Amount]) VALUES (N'P003', N'B002', 3)
INSERT [dbo].[Product_Bill] ([ProductProductID], [BillBillId], [Amount]) VALUES (N'P003', N'B004', 1)
INSERT [dbo].[Product_Bill] ([ProductProductID], [BillBillId], [Amount]) VALUES (N'P004', N'B002', 2)
INSERT [dbo].[Product_Bill] ([ProductProductID], [BillBillId], [Amount]) VALUES (N'P004', N'B004', 2)
INSERT [dbo].[Product_Bill] ([ProductProductID], [BillBillId], [Amount]) VALUES (N'P005', N'B005', 3)
INSERT [dbo].[Product_Bill] ([ProductProductID], [BillBillId], [Amount]) VALUES (N'P006', N'B005', 1)
GO
INSERT [dbo].[Product_Gem] ([ProductProductID], [GemGemId], [Amount]) VALUES (N'P001', N'G001', 2)
INSERT [dbo].[Product_Gem] ([ProductProductID], [GemGemId], [Amount]) VALUES (N'P002', N'G002', 3)
INSERT [dbo].[Product_Gem] ([ProductProductID], [GemGemId], [Amount]) VALUES (N'P003', N'G003', 1)
INSERT [dbo].[Product_Gem] ([ProductProductID], [GemGemId], [Amount]) VALUES (N'P004', N'G004', 4)
INSERT [dbo].[Product_Gem] ([ProductProductID], [GemGemId], [Amount]) VALUES (N'P005', N'G005', 2)
GO
INSERT [dbo].[User] ([UserID], [Username], [Password], [Role], [FullName], [DoB], [Phone], [Address], [Status]) VALUES (N'USR001', N'jame', N'secpass', 1, N'Jane Smith', CAST(N'1995-10-15' AS Date), N'0937654321', N'456 Elm St, City, Country', 1)
INSERT [dbo].[User] ([UserID], [Username], [Password], [Role], [FullName], [DoB], [Phone], [Address], [Status]) VALUES (N'USR002', N'jane_smith', N'securepass', 1, N'Jane Smith', CAST(N'1995-10-15' AS Date), N'0987654721', N'456 Elm St, City, Country', 1)
INSERT [dbo].[User] ([UserID], [Username], [Password], [Role], [FullName], [DoB], [Phone], [Address], [Status]) VALUES (N'USR003', N'bob_jones', N'pass1234', 1, N'Bob Jones', CAST(N'1985-03-10' AS Date), N'0135792468', N'789 Oak St, City, Country', 1)
INSERT [dbo].[User] ([UserID], [Username], [Password], [Role], [FullName], [DoB], [Phone], [Address], [Status]) VALUES (N'USR004', N'alice_walker', N'123456', 2, N'Alice Walker', CAST(N'1988-07-25' AS Date), N'0987654421', N'321 Pine St, City, Country', 1)
INSERT [dbo].[User] ([UserID], [Username], [Password], [Role], [FullName], [DoB], [Phone], [Address], [Status]) VALUES (N'USR005', N'sam_brown', N'brown123', 2, N'Sam Brown', CAST(N'1992-12-05' AS Date), N'0246813679', N'654 Cedar St, City, Country', 1)
INSERT [dbo].[User] ([UserID], [Username], [Password], [Role], [FullName], [DoB], [Phone], [Address], [Status]) VALUES (N'USR10', N'shbfUSR10', N'251bed121151d3b35da993ea22daa1bdebf494ca6d71f3d7498dd31b05d59931', 1, N'fdjsf shbf', CAST(N'2001-05-27' AS Date), N'0119235333', N'TPHCM', 1)
INSERT [dbo].[User] ([UserID], [Username], [Password], [Role], [FullName], [DoB], [Phone], [Address], [Status]) VALUES (N'USR11', N'MinhUSR11', N'e8d6efdbe282ce5b6776f1fe39b65e3fbce7ddeb2e3f5617d738a111aac3a2a3', 3, N'Le Hoang Minh', CAST(N'2000-05-28' AS Date), N'0911922312', N'Quan 9, TPHCM', 1)
INSERT [dbo].[User] ([UserID], [Username], [Password], [Role], [FullName], [DoB], [Phone], [Address], [Status]) VALUES (N'USR5', N'AnhUSR5', N'7902699be42c8a8e46fbbb4501726517e86b22c56a189f7625a6da49081b2451', 2, N'Nguyen Ngoc Anh', CAST(N'2004-05-23' AS Date), N'0918267345', N'Quan 1, HCM', 1)
INSERT [dbo].[User] ([UserID], [Username], [Password], [Role], [FullName], [DoB], [Phone], [Address], [Status]) VALUES (N'USR7', N'NhatUSR7', N'837016e8729dcdc1f412ec90bf9c195966f5828e5ebb0e5852ee9439fe881756', 2, N'Nguyen Minh Nhat', CAST(N'2000-09-20' AS Date), N'0918337345', N'Quan 2, HCM', 1)
INSERT [dbo].[User] ([UserID], [Username], [Password], [Role], [FullName], [DoB], [Phone], [Address], [Status]) VALUES (N'USR8', N'NhatUSR8', N'56a529103704878c7412371192eda65881a648bded2048e8b63425488bf126a5', 2, N'Nguyen Minh Nhat', CAST(N'2000-09-20' AS Date), N'0123666888', N'Quan 2, HCM', 1)
INSERT [dbo].[User] ([UserID], [Username], [Password], [Role], [FullName], [DoB], [Phone], [Address], [Status]) VALUES (N'USR9', N'LinhUSR9', N'f5aa3c5db27fadaa12b77509754d43eab43deb89e552da3714bf4f92939f5ff0', 3, N'Tran Hoang Linh', CAST(N'1995-01-20' AS Date), N'0911789345', N'Quan 2, TPHCM', 1)
GO
INSERT [dbo].[Voucher] ([VoucherId], [CreatedBy], [ExpiredDay], [PublishedDay], [Cost], [CustomerCustomerID]) VALUES (N'V001', N'USR002', CAST(N'2024-06-30' AS Date), CAST(N'2024-05-01' AS Date), 500000, N'C001')
INSERT [dbo].[Voucher] ([VoucherId], [CreatedBy], [ExpiredDay], [PublishedDay], [Cost], [CustomerCustomerID]) VALUES (N'V002', N'USR002', CAST(N'2024-07-31' AS Date), CAST(N'2024-05-15' AS Date), 750000, N'C002')
INSERT [dbo].[Voucher] ([VoucherId], [CreatedBy], [ExpiredDay], [PublishedDay], [Cost], [CustomerCustomerID]) VALUES (N'V003', N'USR003', CAST(N'2024-08-31' AS Date), CAST(N'2024-06-01' AS Date), 1000000, N'C003')
GO
INSERT [dbo].[Warranty] ([WarrantyID], [CustomerCustomerID], [StartDate], [ExpiredDate], [Desc], [ProductID]) VALUES (N'W001', N'C001', CAST(N'2024-01-01' AS Date), CAST(N'2024-12-31' AS Date), N'One-year warranty', N'P001')
INSERT [dbo].[Warranty] ([WarrantyID], [CustomerCustomerID], [StartDate], [ExpiredDate], [Desc], [ProductID]) VALUES (N'W002', N'C002', CAST(N'2024-03-15' AS Date), CAST(N'2025-03-14' AS Date), N'One-year warranty', N'P002')
INSERT [dbo].[Warranty] ([WarrantyID], [CustomerCustomerID], [StartDate], [ExpiredDate], [Desc], [ProductID]) VALUES (N'W003', N'C003', CAST(N'2024-02-20' AS Date), CAST(N'2025-02-19' AS Date), N'One-year warranty', N'P003')
INSERT [dbo].[Warranty] ([WarrantyID], [CustomerCustomerID], [StartDate], [ExpiredDate], [Desc], [ProductID]) VALUES (N'W004', N'C004', CAST(N'2024-04-10' AS Date), CAST(N'2025-04-09' AS Date), N'One-year warranty', N'P004')
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Customer__5C7E359E07EAE794]    Script Date: 5/29/2024 1:03:10 AM ******/
ALTER TABLE [dbo].[Customer] ADD UNIQUE NONCLUSTERED 
(
	[Phone] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Customer__A9D10534BB2CB339]    Script Date: 5/29/2024 1:03:10 AM ******/
ALTER TABLE [dbo].[Customer] ADD UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__User__536C85E40AB6F2E8]    Script Date: 5/29/2024 1:03:10 AM ******/
ALTER TABLE [dbo].[User] ADD UNIQUE NONCLUSTERED 
(
	[Username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__User__5C7E359E8B1D431F]    Script Date: 5/29/2024 1:03:10 AM ******/
ALTER TABLE [dbo].[User] ADD UNIQUE NONCLUSTERED 
(
	[Phone] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Bill]  WITH CHECK ADD  CONSTRAINT [FK_Cashier_Bill] FOREIGN KEY([CashierID])
REFERENCES [dbo].[Cashier] ([CashId])
GO
ALTER TABLE [dbo].[Bill] CHECK CONSTRAINT [FK_Cashier_Bill]
GO
ALTER TABLE [dbo].[Bill]  WITH CHECK ADD  CONSTRAINT [FKBill513577] FOREIGN KEY([VoucherVoucherId])
REFERENCES [dbo].[Voucher] ([VoucherId])
GO
ALTER TABLE [dbo].[Bill] CHECK CONSTRAINT [FKBill513577]
GO
ALTER TABLE [dbo].[Cashier]  WITH CHECK ADD  CONSTRAINT [FKCash_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserID])
GO
ALTER TABLE [dbo].[Cashier] CHECK CONSTRAINT [FKCash_User]
GO
ALTER TABLE [dbo].[Discount]  WITH CHECK ADD  CONSTRAINT [FKDiscount651255] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[User] ([UserID])
GO
ALTER TABLE [dbo].[Discount] CHECK CONSTRAINT [FKDiscount651255]
GO
ALTER TABLE [dbo].[Discount_Product]  WITH CHECK ADD  CONSTRAINT [FKDiscount_P628904] FOREIGN KEY([DiscountDiscountId])
REFERENCES [dbo].[Discount] ([DiscountId])
GO
ALTER TABLE [dbo].[Discount_Product] CHECK CONSTRAINT [FKDiscount_P628904]
GO
ALTER TABLE [dbo].[Discount_Product]  WITH CHECK ADD  CONSTRAINT [FKDiscount_P804639] FOREIGN KEY([ProductProductID])
REFERENCES [dbo].[Product] ([ProductID])
GO
ALTER TABLE [dbo].[Discount_Product] CHECK CONSTRAINT [FKDiscount_P804639]
GO
ALTER TABLE [dbo].[Gold]  WITH CHECK ADD  CONSTRAINT [FK_Gold_User] FOREIGN KEY([ModifiedBy])
REFERENCES [dbo].[User] ([UserID])
GO
ALTER TABLE [dbo].[Gold] CHECK CONSTRAINT [FK_Gold_User]
GO
ALTER TABLE [dbo].[OldProduct]  WITH CHECK ADD  CONSTRAINT [FK_Oldproduct_Bill] FOREIGN KEY([BillBillId])
REFERENCES [dbo].[Bill] ([BillId])
GO
ALTER TABLE [dbo].[OldProduct] CHECK CONSTRAINT [FK_Oldproduct_Bill]
GO
ALTER TABLE [dbo].[OldProduct]  WITH CHECK ADD  CONSTRAINT [FKOldProduct36216] FOREIGN KEY([BillBillId])
REFERENCES [dbo].[Bill] ([BillId])
GO
ALTER TABLE [dbo].[OldProduct] CHECK CONSTRAINT [FKOldProduct36216]
GO
ALTER TABLE [dbo].[OldProduct]  WITH CHECK ADD  CONSTRAINT [FKOldProduct499685] FOREIGN KEY([ProductProductID])
REFERENCES [dbo].[Product] ([ProductID])
GO
ALTER TABLE [dbo].[OldProduct] CHECK CONSTRAINT [FKOldProduct499685]
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [FK_Product_Gold] FOREIGN KEY([Material])
REFERENCES [dbo].[Gold] ([GoldId])
GO
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [FK_Product_Gold]
GO
ALTER TABLE [dbo].[Product_Bill]  WITH CHECK ADD  CONSTRAINT [FKProduct_Bi56046] FOREIGN KEY([BillBillId])
REFERENCES [dbo].[Bill] ([BillId])
GO
ALTER TABLE [dbo].[Product_Bill] CHECK CONSTRAINT [FKProduct_Bi56046]
GO
ALTER TABLE [dbo].[Product_Bill]  WITH CHECK ADD  CONSTRAINT [FKProduct_Bi592576] FOREIGN KEY([ProductProductID])
REFERENCES [dbo].[Product] ([ProductID])
GO
ALTER TABLE [dbo].[Product_Bill] CHECK CONSTRAINT [FKProduct_Bi592576]
GO
ALTER TABLE [dbo].[Product_Gem]  WITH CHECK ADD  CONSTRAINT [FKProduct_Ge596162] FOREIGN KEY([GemGemId])
REFERENCES [dbo].[Gem] ([GemId])
GO
ALTER TABLE [dbo].[Product_Gem] CHECK CONSTRAINT [FKProduct_Ge596162]
GO
ALTER TABLE [dbo].[Product_Gem]  WITH CHECK ADD  CONSTRAINT [FKProduct_Ge731455] FOREIGN KEY([ProductProductID])
REFERENCES [dbo].[Product] ([ProductID])
GO
ALTER TABLE [dbo].[Product_Gem] CHECK CONSTRAINT [FKProduct_Ge731455]
GO
ALTER TABLE [dbo].[Voucher]  WITH CHECK ADD  CONSTRAINT [FKVoucher301107] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[User] ([UserID])
GO
ALTER TABLE [dbo].[Voucher] CHECK CONSTRAINT [FKVoucher301107]
GO
ALTER TABLE [dbo].[Voucher]  WITH CHECK ADD  CONSTRAINT [FKVoucher633196] FOREIGN KEY([CustomerCustomerID])
REFERENCES [dbo].[Customer] ([CustomerID])
GO
ALTER TABLE [dbo].[Voucher] CHECK CONSTRAINT [FKVoucher633196]
GO
ALTER TABLE [dbo].[Warranty]  WITH CHECK ADD  CONSTRAINT [FKProduct700894] FOREIGN KEY([ProductID])
REFERENCES [dbo].[Product] ([ProductID])
GO
ALTER TABLE [dbo].[Warranty] CHECK CONSTRAINT [FKProduct700894]
GO
ALTER TABLE [dbo].[Warranty]  WITH CHECK ADD  CONSTRAINT [FKWarranty22608] FOREIGN KEY([CustomerCustomerID])
REFERENCES [dbo].[Customer] ([CustomerID])
GO
ALTER TABLE [dbo].[Warranty] CHECK CONSTRAINT [FKWarranty22608]
GO
USE [master]
GO
ALTER DATABASE [Jewerly_v6] SET  READ_WRITE 
GO
