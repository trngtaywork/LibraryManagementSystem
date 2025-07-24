USE [master]
GO
/****** Object:  Database [LibraryManageSystem]    Script Date: 11/9/2024 8:01:53 PM ******/
CREATE DATABASE [LibraryManageSystem]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'LibraryManageSystem', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\LibraryManageSystem.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'LibraryManageSystem_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\LibraryManageSystem_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [LibraryManageSystem] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [LibraryManageSystem].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [LibraryManageSystem] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [LibraryManageSystem] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [LibraryManageSystem] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [LibraryManageSystem] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [LibraryManageSystem] SET ARITHABORT OFF 
GO
ALTER DATABASE [LibraryManageSystem] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [LibraryManageSystem] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [LibraryManageSystem] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [LibraryManageSystem] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [LibraryManageSystem] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [LibraryManageSystem] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [LibraryManageSystem] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [LibraryManageSystem] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [LibraryManageSystem] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [LibraryManageSystem] SET  ENABLE_BROKER 
GO
ALTER DATABASE [LibraryManageSystem] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [LibraryManageSystem] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [LibraryManageSystem] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [LibraryManageSystem] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [LibraryManageSystem] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [LibraryManageSystem] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [LibraryManageSystem] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [LibraryManageSystem] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [LibraryManageSystem] SET  MULTI_USER 
GO
ALTER DATABASE [LibraryManageSystem] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [LibraryManageSystem] SET DB_CHAINING OFF 
GO
ALTER DATABASE [LibraryManageSystem] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [LibraryManageSystem] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [LibraryManageSystem] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [LibraryManageSystem] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [LibraryManageSystem] SET QUERY_STORE = ON
GO
ALTER DATABASE [LibraryManageSystem] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [LibraryManageSystem]
GO
/****** Object:  Table [dbo].[Authors]    Script Date: 11/9/2024 8:01:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Authors](
	[author_id] [int] IDENTITY(1,1) NOT NULL,
	[author_name] [nvarchar](255) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[author_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Books]    Script Date: 11/9/2024 8:01:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Books](
	[book_id] [int] IDENTITY(1,1) NOT NULL,
	[title] [nvarchar](255) NOT NULL,
	[quantity] [int] NOT NULL,
	[publication_date] [date] NULL,
	[image] [nvarchar](255) NULL,
	[created_at] [datetime] NULL,
	[created_by] [nvarchar](50) NULL,
	[category_id] [int] NULL,
 CONSTRAINT [PK__Books__490D1AE1C66CC684] PRIMARY KEY CLUSTERED 
(
	[book_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Books_Authors]    Script Date: 11/9/2024 8:01:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Books_Authors](
	[book_id] [int] NOT NULL,
	[author_id] [int] NOT NULL,
 CONSTRAINT [PK_Books_Authors] PRIMARY KEY CLUSTERED 
(
	[book_id] ASC,
	[author_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BorrowBooks]    Script Date: 11/9/2024 8:01:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BorrowBooks](
	[borrow_id] [int] IDENTITY(1,1) NOT NULL,
	[reservation_date] [date] NULL,
	[borrow_date] [date] NULL,
	[due_date] [date] NULL,
	[return_date] [date] NULL,
	[librarian_in_charge] [nvarchar](50) NULL,
	[status] [int] NULL,
	[book_id] [int] NULL,
	[user_id] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[borrow_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Categories]    Script Date: 11/9/2024 8:01:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categories](
	[category_id] [int] IDENTITY(1,1) NOT NULL,
	[category_name] [nvarchar](255) NOT NULL,
	[created_by] [nvarchar](50) NULL,
	[modified_by] [nvarchar](50) NULL,
	[created_at] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[category_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Fines]    Script Date: 11/9/2024 8:01:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Fines](
	[fine_id] [int] IDENTITY(1,1) NOT NULL,
	[fine_type] [nvarchar](50) NULL,
	[fine_amount] [decimal](18, 2) NULL,
	[status] [int] NULL,
	[paid_date] [date] NULL,
	[borrow_id] [int] NULL,
 CONSTRAINT [PK__Fines__F3C688D1F9FA9756] PRIMARY KEY CLUSTERED 
(
	[fine_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Post]    Script Date: 11/9/2024 8:01:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Post](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[title] [nvarchar](100) NOT NULL,
	[content] [nvarchar](max) NOT NULL,
	[created_at] [datetime] NULL,
 CONSTRAINT [PK_Post] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Reports]    Script Date: 11/9/2024 8:01:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Reports](
	[report_id] [int] IDENTITY(1,1) NOT NULL,
	[report_reason] [nvarchar](200) NULL,
	[user_id] [int] NULL,
	[status] [nvarchar](20) NULL,
	[borrow_id] [int] NULL,
	[created_at] [datetime] NULL,
	[fine_id] [int] NULL,
 CONSTRAINT [PK_Reports] PRIMARY KEY CLUSTERED 
(
	[report_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 11/9/2024 8:01:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](20) NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Thesis]    Script Date: 11/9/2024 8:01:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Thesis](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[title] [nvarchar](150) NULL,
	[author] [nvarchar](255) NULL,
	[file_doc] [text] NULL,
	[create_at] [date] NULL,
 CONSTRAINT [PK_Thesis] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 11/9/2024 8:01:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[user_id] [int] IDENTITY(1,1) NOT NULL,
	[email] [nvarchar](50) NOT NULL,
	[password] [nvarchar](80) NOT NULL,
	[student_code] [nvarchar](10) NULL,
	[fullname] [nvarchar](50) NULL,
	[phone] [nvarchar](10) NULL,
	[role] [nvarchar](10) NULL,
	[status] [int] NULL,
	[create_at] [date] NULL,
	[verify_code] [nvarchar](6) NULL,
	[expiration_code] [date] NULL,
	[image] [text] NULL,
 CONSTRAINT [PK__Users__B9BE370FF290676B] PRIMARY KEY CLUSTERED 
(
	[user_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Authors] ON 
GO
INSERT [dbo].[Authors] ([author_id], [author_name]) VALUES (1, N'J.K. Rowling')
GO
INSERT [dbo].[Authors] ([author_id], [author_name]) VALUES (2, N'George R.R. Martin')
GO
INSERT [dbo].[Authors] ([author_id], [author_name]) VALUES (3, N'Isaac Asimov')
GO
INSERT [dbo].[Authors] ([author_id], [author_name]) VALUES (4, N'Yuval Noah Harari')
GO
INSERT [dbo].[Authors] ([author_id], [author_name]) VALUES (5, N'Malala Yousafzai')
GO
INSERT [dbo].[Authors] ([author_id], [author_name]) VALUES (6, N'Dale Carnegie')
GO
INSERT [dbo].[Authors] ([author_id], [author_name]) VALUES (7, N'JR Downey')
GO
INSERT [dbo].[Authors] ([author_id], [author_name]) VALUES (8, N'test')
GO
INSERT [dbo].[Authors] ([author_id], [author_name]) VALUES (9, N'test123')
GO
INSERT [dbo].[Authors] ([author_id], [author_name]) VALUES (10, N'test321')
GO
INSERT [dbo].[Authors] ([author_id], [author_name]) VALUES (11, N'123')
GO
SET IDENTITY_INSERT [dbo].[Authors] OFF
GO
SET IDENTITY_INSERT [dbo].[Books] ON 
GO
INSERT [dbo].[Books] ([book_id], [title], [quantity], [publication_date], [image], [created_at], [created_by], [category_id]) VALUES (1, N'Harry Potter and the Sorcerer''s Stone', 5, CAST(N'1997-06-26' AS Date), N'https://firebasestorage.googleapis.com/v0/b/fir-60e00.appspot.com/o/images%2Fanhthe34.jpg?alt=media&token=9ae32ee6-a1a0-4c83-845f-aeac0ece3cc4', CAST(N'2024-10-27T12:04:46.150' AS DateTime), N'Admin', 1)
GO
INSERT [dbo].[Books] ([book_id], [title], [quantity], [publication_date], [image], [created_at], [created_by], [category_id]) VALUES (2, N'A Game of Thrones', 0, CAST(N'1996-08-06' AS Date), NULL, CAST(N'2024-10-27T12:04:46.150' AS DateTime), N'Admin', 1)
GO
INSERT [dbo].[Books] ([book_id], [title], [quantity], [publication_date], [image], [created_at], [created_by], [category_id]) VALUES (3, N'Foundation', 5, CAST(N'1951-06-01' AS Date), NULL, CAST(N'2024-10-27T12:04:46.150' AS DateTime), N'Admin', 3)
GO
INSERT [dbo].[Books] ([book_id], [title], [quantity], [publication_date], [image], [created_at], [created_by], [category_id]) VALUES (4, N'Sapiens: A Brief History of Humankind', 6, CAST(N'2011-01-01' AS Date), NULL, CAST(N'2024-10-27T12:04:46.150' AS DateTime), N'Admin', 4)
GO
INSERT [dbo].[Books] ([book_id], [title], [quantity], [publication_date], [image], [created_at], [created_by], [category_id]) VALUES (5, N'I Am Malala', 0, CAST(N'2013-10-08' AS Date), NULL, CAST(N'2024-10-27T12:04:46.150' AS DateTime), N'Admin', 2)
GO
INSERT [dbo].[Books] ([book_id], [title], [quantity], [publication_date], [image], [created_at], [created_by], [category_id]) VALUES (6, N'Harry Potter and the Chamber of Secrets', 9, CAST(N'1998-07-02' AS Date), NULL, CAST(N'2024-10-27T12:04:46.150' AS DateTime), N'Admin', 1)
GO
INSERT [dbo].[Books] ([book_id], [title], [quantity], [publication_date], [image], [created_at], [created_by], [category_id]) VALUES (7, N'A Clash of Kings', 3, CAST(N'1998-11-16' AS Date), NULL, CAST(N'2024-10-27T12:04:46.150' AS DateTime), N'Admin', 1)
GO
INSERT [dbo].[Books] ([book_id], [title], [quantity], [publication_date], [image], [created_at], [created_by], [category_id]) VALUES (8, N'I, Robot', 6, CAST(N'1950-12-02' AS Date), NULL, CAST(N'2024-10-27T12:04:46.150' AS DateTime), N'Admin', 3)
GO
INSERT [dbo].[Books] ([book_id], [title], [quantity], [publication_date], [image], [created_at], [created_by], [category_id]) VALUES (9, N'Homo Deus: A Brief History of Tomorrow', 4, CAST(N'2015-01-01' AS Date), NULL, CAST(N'2024-10-27T12:04:46.150' AS DateTime), N'Admin', 4)
GO
INSERT [dbo].[Books] ([book_id], [title], [quantity], [publication_date], [image], [created_at], [created_by], [category_id]) VALUES (10, N'The Boy Who Ran Away', 2, CAST(N'2020-05-15' AS Date), NULL, CAST(N'2024-10-27T12:04:46.150' AS DateTime), N'Admin', 5)
GO
INSERT [dbo].[Books] ([book_id], [title], [quantity], [publication_date], [image], [created_at], [created_by], [category_id]) VALUES (11, N'Đắc Nhân Tâm', 123, CAST(N'2024-10-27' AS Date), N'https://firebasestorage.googleapis.com/v0/b/fir-60e00.appspot.com/o/images%2Fdacnhantam86.jpg?alt=media&token=2c960806-9888-436d-83ee-aab72197ba8d', CAST(N'2024-10-27T14:19:20.873' AS DateTime), NULL, 4)
GO
INSERT [dbo].[Books] ([book_id], [title], [quantity], [publication_date], [image], [created_at], [created_by], [category_id]) VALUES (12, N'test', 0, CAST(N'2024-11-03' AS Date), N'https://firebasestorage.googleapis.com/v0/b/fir-60e00.appspot.com/o/images%2Fanhthe34.jpg?alt=media&token=62287147-2586-460d-8e93-1711aedeea31', CAST(N'2024-11-05T13:11:59.620' AS DateTime), N'Quân', 2)
GO
INSERT [dbo].[Books] ([book_id], [title], [quantity], [publication_date], [image], [created_at], [created_by], [category_id]) VALUES (13, N'ten', 0, CAST(N'2024-10-31' AS Date), N'https://firebasestorage.googleapis.com/v0/b/fir-60e00.appspot.com/o/images%2Fstretched-1920-1080-1280310.jpg?alt=media&token=983a94ed-62dd-49e9-a9f5-b29bbf8f6cee', CAST(N'2024-11-05T13:14:57.450' AS DateTime), N'Quân', 2)
GO
INSERT [dbo].[Books] ([book_id], [title], [quantity], [publication_date], [image], [created_at], [created_by], [category_id]) VALUES (14, N't123', 0, CAST(N'2024-11-03' AS Date), N'https://firebasestorage.googleapis.com/v0/b/fir-60e00.appspot.com/o/images%2Fstretched-1920-1080-1280310.jpg?alt=media&token=932b622b-ca1a-4f22-bf39-c42be12c99d9', CAST(N'2024-11-05T13:18:06.080' AS DateTime), N'Quân', 1)
GO
INSERT [dbo].[Books] ([book_id], [title], [quantity], [publication_date], [image], [created_at], [created_by], [category_id]) VALUES (15, N't123', 3, CAST(N'2024-11-01' AS Date), N'https://firebasestorage.googleapis.com/v0/b/fir-60e00.appspot.com/o/images%2F394491315_1084449202930229_5235597799077725624_n.jpg?alt=media&token=8ef96a12-6109-42f2-81b7-89a228649a61', CAST(N'2024-11-05T13:20:44.667' AS DateTime), N'Quân', 5)
GO
INSERT [dbo].[Books] ([book_id], [title], [quantity], [publication_date], [image], [created_at], [created_by], [category_id]) VALUES (16, N'teset', 123, CAST(N'2024-11-04' AS Date), N'https://firebasestorage.googleapis.com/v0/b/fir-60e00.appspot.com/o/images%2F394491315_1084449202930229_5235597799077725624_n.jpg?alt=media&token=1b17bf99-ae17-42ce-a4b3-5d1f085d516b', CAST(N'2024-11-05T14:47:01.060' AS DateTime), N'Quân', 2)
GO
SET IDENTITY_INSERT [dbo].[Books] OFF
GO
INSERT [dbo].[Books_Authors] ([book_id], [author_id]) VALUES (1, 1)
GO
INSERT [dbo].[Books_Authors] ([book_id], [author_id]) VALUES (1, 3)
GO
INSERT [dbo].[Books_Authors] ([book_id], [author_id]) VALUES (1, 5)
GO
INSERT [dbo].[Books_Authors] ([book_id], [author_id]) VALUES (2, 2)
GO
INSERT [dbo].[Books_Authors] ([book_id], [author_id]) VALUES (3, 3)
GO
INSERT [dbo].[Books_Authors] ([book_id], [author_id]) VALUES (4, 4)
GO
INSERT [dbo].[Books_Authors] ([book_id], [author_id]) VALUES (5, 5)
GO
INSERT [dbo].[Books_Authors] ([book_id], [author_id]) VALUES (6, 1)
GO
INSERT [dbo].[Books_Authors] ([book_id], [author_id]) VALUES (7, 2)
GO
INSERT [dbo].[Books_Authors] ([book_id], [author_id]) VALUES (8, 3)
GO
INSERT [dbo].[Books_Authors] ([book_id], [author_id]) VALUES (9, 4)
GO
INSERT [dbo].[Books_Authors] ([book_id], [author_id]) VALUES (10, 5)
GO
INSERT [dbo].[Books_Authors] ([book_id], [author_id]) VALUES (11, 6)
GO
INSERT [dbo].[Books_Authors] ([book_id], [author_id]) VALUES (12, 8)
GO
INSERT [dbo].[Books_Authors] ([book_id], [author_id]) VALUES (12, 9)
GO
INSERT [dbo].[Books_Authors] ([book_id], [author_id]) VALUES (12, 10)
GO
INSERT [dbo].[Books_Authors] ([book_id], [author_id]) VALUES (13, 8)
GO
INSERT [dbo].[Books_Authors] ([book_id], [author_id]) VALUES (14, 8)
GO
INSERT [dbo].[Books_Authors] ([book_id], [author_id]) VALUES (15, 8)
GO
INSERT [dbo].[Books_Authors] ([book_id], [author_id]) VALUES (16, 11)
GO
SET IDENTITY_INSERT [dbo].[BorrowBooks] ON 
GO
INSERT [dbo].[BorrowBooks] ([borrow_id], [reservation_date], [borrow_date], [due_date], [return_date], [librarian_in_charge], [status], [book_id], [user_id]) VALUES (1, CAST(N'2024-10-26' AS Date), CAST(N'2024-10-28' AS Date), CAST(N'2024-11-12' AS Date), NULL, N'Librarian A', 1, 1, 1)
GO
INSERT [dbo].[BorrowBooks] ([borrow_id], [reservation_date], [borrow_date], [due_date], [return_date], [librarian_in_charge], [status], [book_id], [user_id]) VALUES (2, CAST(N'2024-10-27' AS Date), CAST(N'2024-10-28' AS Date), CAST(N'2024-11-12' AS Date), NULL, N'Librarian A', 1, 2, 2)
GO
INSERT [dbo].[BorrowBooks] ([borrow_id], [reservation_date], [borrow_date], [due_date], [return_date], [librarian_in_charge], [status], [book_id], [user_id]) VALUES (3, CAST(N'2024-10-28' AS Date), CAST(N'2024-10-28' AS Date), CAST(N'2024-11-12' AS Date), NULL, N'Librarian B', 4, 3, 3)
GO
INSERT [dbo].[BorrowBooks] ([borrow_id], [reservation_date], [borrow_date], [due_date], [return_date], [librarian_in_charge], [status], [book_id], [user_id]) VALUES (4, CAST(N'2024-10-29' AS Date), CAST(N'2024-10-28' AS Date), CAST(N'2024-11-12' AS Date), NULL, N'Librarian B', 4, 4, 4)
GO
INSERT [dbo].[BorrowBooks] ([borrow_id], [reservation_date], [borrow_date], [due_date], [return_date], [librarian_in_charge], [status], [book_id], [user_id]) VALUES (5, CAST(N'2024-10-30' AS Date), CAST(N'2024-10-28' AS Date), CAST(N'2024-11-12' AS Date), NULL, N'Librarian C', 4, 5, 5)
GO
INSERT [dbo].[BorrowBooks] ([borrow_id], [reservation_date], [borrow_date], [due_date], [return_date], [librarian_in_charge], [status], [book_id], [user_id]) VALUES (6, CAST(N'2024-10-10' AS Date), CAST(N'2024-10-11' AS Date), CAST(N'2024-10-26' AS Date), NULL, N'Librarian A', 3, 1, 1)
GO
INSERT [dbo].[BorrowBooks] ([borrow_id], [reservation_date], [borrow_date], [due_date], [return_date], [librarian_in_charge], [status], [book_id], [user_id]) VALUES (7, CAST(N'2024-10-11' AS Date), CAST(N'2024-10-12' AS Date), CAST(N'2024-10-27' AS Date), NULL, N'Librarian B', 3, 2, 2)
GO
INSERT [dbo].[BorrowBooks] ([borrow_id], [reservation_date], [borrow_date], [due_date], [return_date], [librarian_in_charge], [status], [book_id], [user_id]) VALUES (8, CAST(N'2024-10-12' AS Date), CAST(N'2024-10-13' AS Date), CAST(N'2024-10-28' AS Date), NULL, N'Librarian C', 3, 3, 3)
GO
INSERT [dbo].[BorrowBooks] ([borrow_id], [reservation_date], [borrow_date], [due_date], [return_date], [librarian_in_charge], [status], [book_id], [user_id]) VALUES (9, CAST(N'2024-10-13' AS Date), CAST(N'2024-10-14' AS Date), CAST(N'2024-10-29' AS Date), NULL, N'Librarian A', 3, 4, 4)
GO
INSERT [dbo].[BorrowBooks] ([borrow_id], [reservation_date], [borrow_date], [due_date], [return_date], [librarian_in_charge], [status], [book_id], [user_id]) VALUES (10, CAST(N'2024-10-14' AS Date), CAST(N'2024-10-15' AS Date), CAST(N'2024-10-30' AS Date), NULL, N'Librarian B', 4, 5, 5)
GO
INSERT [dbo].[BorrowBooks] ([borrow_id], [reservation_date], [borrow_date], [due_date], [return_date], [librarian_in_charge], [status], [book_id], [user_id]) VALUES (11, CAST(N'2024-10-05' AS Date), NULL, NULL, NULL, N'Librarian C', 2, 6, 1)
GO
INSERT [dbo].[BorrowBooks] ([borrow_id], [reservation_date], [borrow_date], [due_date], [return_date], [librarian_in_charge], [status], [book_id], [user_id]) VALUES (12, CAST(N'2024-10-06' AS Date), NULL, NULL, NULL, N'Librarian A', 2, 7, 2)
GO
INSERT [dbo].[BorrowBooks] ([borrow_id], [reservation_date], [borrow_date], [due_date], [return_date], [librarian_in_charge], [status], [book_id], [user_id]) VALUES (13, CAST(N'2024-10-07' AS Date), NULL, NULL, NULL, N'Librarian B', 2, 8, 3)
GO
INSERT [dbo].[BorrowBooks] ([borrow_id], [reservation_date], [borrow_date], [due_date], [return_date], [librarian_in_charge], [status], [book_id], [user_id]) VALUES (14, CAST(N'2024-10-08' AS Date), NULL, NULL, NULL, N'Librarian A', 2, 9, 4)
GO
INSERT [dbo].[BorrowBooks] ([borrow_id], [reservation_date], [borrow_date], [due_date], [return_date], [librarian_in_charge], [status], [book_id], [user_id]) VALUES (15, CAST(N'2024-10-09' AS Date), NULL, NULL, NULL, N'Librarian C', 2, 10, 5)
GO
INSERT [dbo].[BorrowBooks] ([borrow_id], [reservation_date], [borrow_date], [due_date], [return_date], [librarian_in_charge], [status], [book_id], [user_id]) VALUES (16, CAST(N'2024-09-15' AS Date), CAST(N'2024-09-16' AS Date), CAST(N'2024-10-01' AS Date), CAST(N'2024-10-05' AS Date), N'Librarian B', 3, 1, 1)
GO
INSERT [dbo].[BorrowBooks] ([borrow_id], [reservation_date], [borrow_date], [due_date], [return_date], [librarian_in_charge], [status], [book_id], [user_id]) VALUES (17, CAST(N'2024-09-17' AS Date), CAST(N'2024-09-18' AS Date), CAST(N'2024-10-03' AS Date), CAST(N'2024-10-06' AS Date), N'Librarian C', 3, 2, 2)
GO
INSERT [dbo].[BorrowBooks] ([borrow_id], [reservation_date], [borrow_date], [due_date], [return_date], [librarian_in_charge], [status], [book_id], [user_id]) VALUES (18, CAST(N'2024-09-19' AS Date), CAST(N'2024-09-20' AS Date), CAST(N'2024-10-05' AS Date), CAST(N'2024-10-07' AS Date), N'Librarian A', 3, 3, 3)
GO
INSERT [dbo].[BorrowBooks] ([borrow_id], [reservation_date], [borrow_date], [due_date], [return_date], [librarian_in_charge], [status], [book_id], [user_id]) VALUES (19, CAST(N'2024-09-21' AS Date), CAST(N'2024-09-22' AS Date), CAST(N'2024-10-07' AS Date), CAST(N'2024-10-08' AS Date), N'Librarian B', 2, 4, 4)
GO
INSERT [dbo].[BorrowBooks] ([borrow_id], [reservation_date], [borrow_date], [due_date], [return_date], [librarian_in_charge], [status], [book_id], [user_id]) VALUES (20, CAST(N'2024-09-23' AS Date), CAST(N'2024-09-24' AS Date), CAST(N'2024-10-09' AS Date), CAST(N'2024-10-10' AS Date), N'Librarian C', 3, 5, 5)
GO
INSERT [dbo].[BorrowBooks] ([borrow_id], [reservation_date], [borrow_date], [due_date], [return_date], [librarian_in_charge], [status], [book_id], [user_id]) VALUES (21, CAST(N'2024-08-20' AS Date), CAST(N'2024-08-21' AS Date), CAST(N'2024-09-05' AS Date), NULL, N'Librarian A', 4, 1, 1)
GO
INSERT [dbo].[BorrowBooks] ([borrow_id], [reservation_date], [borrow_date], [due_date], [return_date], [librarian_in_charge], [status], [book_id], [user_id]) VALUES (22, CAST(N'2024-08-22' AS Date), CAST(N'2024-08-23' AS Date), CAST(N'2024-09-07' AS Date), NULL, N'Librarian B', 4, 2, 2)
GO
INSERT [dbo].[BorrowBooks] ([borrow_id], [reservation_date], [borrow_date], [due_date], [return_date], [librarian_in_charge], [status], [book_id], [user_id]) VALUES (23, CAST(N'2024-08-24' AS Date), CAST(N'2024-08-25' AS Date), CAST(N'2024-09-09' AS Date), NULL, N'Librarian C', 4, 3, 3)
GO
INSERT [dbo].[BorrowBooks] ([borrow_id], [reservation_date], [borrow_date], [due_date], [return_date], [librarian_in_charge], [status], [book_id], [user_id]) VALUES (24, CAST(N'2024-08-26' AS Date), CAST(N'2024-08-27' AS Date), CAST(N'2024-09-11' AS Date), NULL, N'Librarian A', 4, 4, 4)
GO
INSERT [dbo].[BorrowBooks] ([borrow_id], [reservation_date], [borrow_date], [due_date], [return_date], [librarian_in_charge], [status], [book_id], [user_id]) VALUES (25, CAST(N'2024-08-28' AS Date), CAST(N'2024-08-29' AS Date), CAST(N'2024-09-13' AS Date), NULL, N'Librarian B', 4, 5, 5)
GO
INSERT [dbo].[BorrowBooks] ([borrow_id], [reservation_date], [borrow_date], [due_date], [return_date], [librarian_in_charge], [status], [book_id], [user_id]) VALUES (26, CAST(N'2024-09-01' AS Date), CAST(N'2024-09-02' AS Date), CAST(N'2024-09-17' AS Date), CAST(N'2024-09-15' AS Date), N'Librarian C', 5, 1, 1)
GO
INSERT [dbo].[BorrowBooks] ([borrow_id], [reservation_date], [borrow_date], [due_date], [return_date], [librarian_in_charge], [status], [book_id], [user_id]) VALUES (27, CAST(N'2024-09-03' AS Date), CAST(N'2024-09-04' AS Date), CAST(N'2024-09-19' AS Date), CAST(N'2024-09-17' AS Date), N'Librarian A', 5, 2, 2)
GO
INSERT [dbo].[BorrowBooks] ([borrow_id], [reservation_date], [borrow_date], [due_date], [return_date], [librarian_in_charge], [status], [book_id], [user_id]) VALUES (28, CAST(N'2024-09-05' AS Date), CAST(N'2024-09-06' AS Date), CAST(N'2024-09-21' AS Date), CAST(N'2024-09-20' AS Date), N'Librarian B', 5, 3, 3)
GO
INSERT [dbo].[BorrowBooks] ([borrow_id], [reservation_date], [borrow_date], [due_date], [return_date], [librarian_in_charge], [status], [book_id], [user_id]) VALUES (29, CAST(N'2024-09-07' AS Date), CAST(N'2024-09-08' AS Date), CAST(N'2024-09-23' AS Date), CAST(N'2024-09-21' AS Date), N'Librarian A', 5, 4, 4)
GO
INSERT [dbo].[BorrowBooks] ([borrow_id], [reservation_date], [borrow_date], [due_date], [return_date], [librarian_in_charge], [status], [book_id], [user_id]) VALUES (30, CAST(N'2024-09-09' AS Date), CAST(N'2024-09-10' AS Date), CAST(N'2024-09-25' AS Date), CAST(N'2024-09-24' AS Date), N'Librarian C', 5, 5, 5)
GO
INSERT [dbo].[BorrowBooks] ([borrow_id], [reservation_date], [borrow_date], [due_date], [return_date], [librarian_in_charge], [status], [book_id], [user_id]) VALUES (41, NULL, CAST(N'2024-10-29' AS Date), CAST(N'2024-11-13' AS Date), NULL, N'Quân', 4, 5, 3)
GO
INSERT [dbo].[BorrowBooks] ([borrow_id], [reservation_date], [borrow_date], [due_date], [return_date], [librarian_in_charge], [status], [book_id], [user_id]) VALUES (42, NULL, CAST(N'2024-11-05' AS Date), CAST(N'2024-11-20' AS Date), NULL, N'Quân', 1, 14, 2)
GO
INSERT [dbo].[BorrowBooks] ([borrow_id], [reservation_date], [borrow_date], [due_date], [return_date], [librarian_in_charge], [status], [book_id], [user_id]) VALUES (43, NULL, CAST(N'2024-11-05' AS Date), CAST(N'2024-11-20' AS Date), NULL, N'Quân', 1, 12, 3)
GO
INSERT [dbo].[BorrowBooks] ([borrow_id], [reservation_date], [borrow_date], [due_date], [return_date], [librarian_in_charge], [status], [book_id], [user_id]) VALUES (44, NULL, CAST(N'2024-11-05' AS Date), CAST(N'2024-11-20' AS Date), NULL, N'Quân', 1, 7, 1)
GO
INSERT [dbo].[BorrowBooks] ([borrow_id], [reservation_date], [borrow_date], [due_date], [return_date], [librarian_in_charge], [status], [book_id], [user_id]) VALUES (45, NULL, CAST(N'2024-11-05' AS Date), CAST(N'2024-11-20' AS Date), NULL, N'Quân', 1, 2, 3)
GO
INSERT [dbo].[BorrowBooks] ([borrow_id], [reservation_date], [borrow_date], [due_date], [return_date], [librarian_in_charge], [status], [book_id], [user_id]) VALUES (46, NULL, CAST(N'2024-11-05' AS Date), CAST(N'2024-11-20' AS Date), NULL, N'Quân', 4, 5, 1)
GO
INSERT [dbo].[BorrowBooks] ([borrow_id], [reservation_date], [borrow_date], [due_date], [return_date], [librarian_in_charge], [status], [book_id], [user_id]) VALUES (47, NULL, CAST(N'2024-11-05' AS Date), CAST(N'2024-11-20' AS Date), CAST(N'2024-11-05' AS Date), N'Quân', 5, 7, 3)
GO
INSERT [dbo].[BorrowBooks] ([borrow_id], [reservation_date], [borrow_date], [due_date], [return_date], [librarian_in_charge], [status], [book_id], [user_id]) VALUES (48, NULL, CAST(N'2024-11-05' AS Date), CAST(N'2024-11-20' AS Date), NULL, N'Quân', 1, 1, 3)
GO
INSERT [dbo].[BorrowBooks] ([borrow_id], [reservation_date], [borrow_date], [due_date], [return_date], [librarian_in_charge], [status], [book_id], [user_id]) VALUES (49, NULL, CAST(N'2024-11-05' AS Date), CAST(N'2024-11-20' AS Date), NULL, N'Quân', 1, 5, 3)
GO
INSERT [dbo].[BorrowBooks] ([borrow_id], [reservation_date], [borrow_date], [due_date], [return_date], [librarian_in_charge], [status], [book_id], [user_id]) VALUES (50, NULL, CAST(N'2024-11-05' AS Date), CAST(N'2024-11-20' AS Date), NULL, N'Quân', 1, 1, 3)
GO
INSERT [dbo].[BorrowBooks] ([borrow_id], [reservation_date], [borrow_date], [due_date], [return_date], [librarian_in_charge], [status], [book_id], [user_id]) VALUES (51, NULL, CAST(N'2024-11-05' AS Date), CAST(N'2024-11-20' AS Date), NULL, N'Quân', 1, 12, 3)
GO
INSERT [dbo].[BorrowBooks] ([borrow_id], [reservation_date], [borrow_date], [due_date], [return_date], [librarian_in_charge], [status], [book_id], [user_id]) VALUES (52, NULL, CAST(N'2024-11-05' AS Date), CAST(N'2024-11-20' AS Date), NULL, N'Quân', 1, 13, 3)
GO
INSERT [dbo].[BorrowBooks] ([borrow_id], [reservation_date], [borrow_date], [due_date], [return_date], [librarian_in_charge], [status], [book_id], [user_id]) VALUES (53, NULL, CAST(N'2024-11-05' AS Date), CAST(N'2024-11-20' AS Date), NULL, N'Quân', 1, 3, 3)
GO
INSERT [dbo].[BorrowBooks] ([borrow_id], [reservation_date], [borrow_date], [due_date], [return_date], [librarian_in_charge], [status], [book_id], [user_id]) VALUES (54, NULL, CAST(N'2024-11-05' AS Date), CAST(N'2024-11-20' AS Date), CAST(N'2024-11-07' AS Date), N'Quân', 5, 3, 3)
GO
INSERT [dbo].[BorrowBooks] ([borrow_id], [reservation_date], [borrow_date], [due_date], [return_date], [librarian_in_charge], [status], [book_id], [user_id]) VALUES (55, NULL, CAST(N'2024-11-05' AS Date), CAST(N'2024-11-20' AS Date), CAST(N'2024-11-06' AS Date), N'Quân', 5, 1, 3)
GO
INSERT [dbo].[BorrowBooks] ([borrow_id], [reservation_date], [borrow_date], [due_date], [return_date], [librarian_in_charge], [status], [book_id], [user_id]) VALUES (56, NULL, CAST(N'2024-11-05' AS Date), CAST(N'2024-11-20' AS Date), NULL, N'Quân', 4, 8, 3)
GO
INSERT [dbo].[BorrowBooks] ([borrow_id], [reservation_date], [borrow_date], [due_date], [return_date], [librarian_in_charge], [status], [book_id], [user_id]) VALUES (57, NULL, CAST(N'2024-11-05' AS Date), CAST(N'2024-11-20' AS Date), CAST(N'2024-11-05' AS Date), N'Quân', 5, 1, 3)
GO
INSERT [dbo].[BorrowBooks] ([borrow_id], [reservation_date], [borrow_date], [due_date], [return_date], [librarian_in_charge], [status], [book_id], [user_id]) VALUES (58, NULL, CAST(N'2024-11-05' AS Date), CAST(N'2024-11-20' AS Date), CAST(N'2024-11-05' AS Date), N'Quân', 5, 1, 3)
GO
INSERT [dbo].[BorrowBooks] ([borrow_id], [reservation_date], [borrow_date], [due_date], [return_date], [librarian_in_charge], [status], [book_id], [user_id]) VALUES (59, NULL, CAST(N'2024-11-05' AS Date), CAST(N'2024-11-20' AS Date), CAST(N'2024-11-05' AS Date), N'Quân', 5, 16, 3)
GO
SET IDENTITY_INSERT [dbo].[BorrowBooks] OFF
GO
SET IDENTITY_INSERT [dbo].[Categories] ON 
GO
INSERT [dbo].[Categories] ([category_id], [category_name], [created_by], [modified_by], [created_at]) VALUES (1, N'Fiction', N'Admin', N'Admin', NULL)
GO
INSERT [dbo].[Categories] ([category_id], [category_name], [created_by], [modified_by], [created_at]) VALUES (2, N'Non-Fiction', N'Admin', N'Admin', NULL)
GO
INSERT [dbo].[Categories] ([category_id], [category_name], [created_by], [modified_by], [created_at]) VALUES (3, N'Science', N'Admin', N'Admin', NULL)
GO
INSERT [dbo].[Categories] ([category_id], [category_name], [created_by], [modified_by], [created_at]) VALUES (4, N'History', N'Admin', N'Admin', NULL)
GO
INSERT [dbo].[Categories] ([category_id], [category_name], [created_by], [modified_by], [created_at]) VALUES (5, N'Children', N'Admin', N'Admin', NULL)
GO
INSERT [dbo].[Categories] ([category_id], [category_name], [created_by], [modified_by], [created_at]) VALUES (9, N'Romance', N'LIBRARIAN', NULL, CAST(N'2024-11-07T00:45:15.463' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[Categories] OFF
GO
SET IDENTITY_INSERT [dbo].[Fines] ON 
GO
INSERT [dbo].[Fines] ([fine_id], [fine_type], [fine_amount], [status], [paid_date], [borrow_id]) VALUES (1, N'Overdue', CAST(5000.00 AS Decimal(18, 2)), 0, NULL, 11)
GO
INSERT [dbo].[Fines] ([fine_id], [fine_type], [fine_amount], [status], [paid_date], [borrow_id]) VALUES (3, N'Overdue', CAST(5000.00 AS Decimal(18, 2)), 1, CAST(N'2024-11-02' AS Date), 13)
GO
INSERT [dbo].[Fines] ([fine_id], [fine_type], [fine_amount], [status], [paid_date], [borrow_id]) VALUES (4, N'Overdue', CAST(5000.00 AS Decimal(18, 2)), 0, NULL, 14)
GO
INSERT [dbo].[Fines] ([fine_id], [fine_type], [fine_amount], [status], [paid_date], [borrow_id]) VALUES (5, N'Overdue', CAST(5000.00 AS Decimal(18, 2)), 0, CAST(N'2024-11-03' AS Date), 15)
GO
INSERT [dbo].[Fines] ([fine_id], [fine_type], [fine_amount], [status], [paid_date], [borrow_id]) VALUES (6, N'Lost', CAST(20000.00 AS Decimal(18, 2)), 1, CAST(N'2024-10-20' AS Date), 16)
GO
INSERT [dbo].[Fines] ([fine_id], [fine_type], [fine_amount], [status], [paid_date], [borrow_id]) VALUES (7, N'Lost', CAST(20000.00 AS Decimal(18, 2)), 0, NULL, 17)
GO
INSERT [dbo].[Fines] ([fine_id], [fine_type], [fine_amount], [status], [paid_date], [borrow_id]) VALUES (8, N'Lost', CAST(20000.00 AS Decimal(18, 2)), 0, NULL, 18)
GO
INSERT [dbo].[Fines] ([fine_id], [fine_type], [fine_amount], [status], [paid_date], [borrow_id]) VALUES (9, N'Lost', CAST(20000.00 AS Decimal(18, 2)), 1, CAST(N'2024-10-22' AS Date), 19)
GO
INSERT [dbo].[Fines] ([fine_id], [fine_type], [fine_amount], [status], [paid_date], [borrow_id]) VALUES (10, N'Lost', CAST(20000.00 AS Decimal(18, 2)), 1, CAST(N'2024-10-23' AS Date), 20)
GO
INSERT [dbo].[Fines] ([fine_id], [fine_type], [fine_amount], [status], [paid_date], [borrow_id]) VALUES (11, N'Lost', CAST(100000.00 AS Decimal(18, 2)), 0, NULL, 56)
GO
SET IDENTITY_INSERT [dbo].[Fines] OFF
GO
SET IDENTITY_INSERT [dbo].[Post] ON 
GO
INSERT [dbo].[Post] ([id], [title], [content], [created_at]) VALUES (11, N'Nội quy Thư viện', N'\u003Cp>\u003Cstrong>Điều 1: Thẻ thư viện (Library card)\u003C/strong>\u003C/p>\u003Cp>Thẻ sinh viên / nhân viên đồng thời là thẻ thư viện để sử dụng các dịch vụ của thư viện.\u003C/p>\u003Cp>\u003Cstrong>Điều 2: Giờ mở cửa\u003C/strong>\u003C/p>\u003Cp>\u003Cstrong>Thứ Hai – Thứ Sáu:\u003C/strong>&nbsp; 8h15 – 21h00\u003C/p>\u003Cp>\u003Cstrong>&nbsp; &nbsp;Cuối tuần:\u003C/strong>&nbsp;8h00 – 12h00, 13h00 – 17h00&nbsp;(Buổi tối và cuối tuần thư viện chỉ phục vụ chỗ tự học)\u003C/p>\u003Cp>\u003Cstrong>Điều 3: Các dịch vụ của Thư viện\u003C/strong>\u003C/p>\u003Cp>3.1.&nbsp;Mượn, trả, gia hạn tài liệu;\u003C/p>\u003Cp>3.2.&nbsp;Hướng dẫn tìm tin và sử dụng thư viện;\u003C/p>\u003Cp>3.3.&nbsp;Tìm kiếm và tư vấn thông tin;\u003C/p>\u003Cp>3.4.&nbsp;Truy cập nguồn tài nguyên điện tử;\u003C/p>\u003Cp>3.5.&nbsp;Tiếp nhận đề nghị đặt mua tài liệu;\u003C/p>\u003Cp>3.6.&nbsp;Mượn liên thư viện;\u003C/p>\u003Cp>3.7.&nbsp;Phòng học nhóm.&nbsp;\u003C/p>\u003Cp>\u003Cstrong>Điều 4: Các quy định chung khi vào thư viện\u003C/strong>\u003C/p>\u003Cp>4.1.&nbsp;Xuất trình thẻ thư viện khi giao dịch với Thư viện. Không dùng thẻ của người khác và không cho người khác mượn thẻ của mình;\u003C/p>\u003Cp>4.2.&nbsp;Giữ gìn trật tự, đi nhẹ, nói khẽ;\u003C/p>\u003Cp>4.3.&nbsp;Giữ gìn vệ sinh chung: không hút thuốc lá, không viết, vẽ lên mặt bàn, không ngồi gác chân lên ghế, bỏ rác đúng nơi quy định;\u003C/p>\u003Cp>4.4.&nbsp;Không được mang vào thư viện đồ ăn, uống, chất độc hại, chất cháy nổ,...;\u003C/p>\u003Cp>4.5.&nbsp;Tắt chuông điện thoại, không nói chuyện điện thoại trong thư viện;\u003C/p>\u003Cp>4.6.&nbsp;Không viết bút chì, bút mực hoặc sử dụng bút đánh dấu lên sách;\u003C/p>\u003Cp>4.7.&nbsp;Không gập hoặc làm nhàu nát, rách sách;\u003C/p>\u003Cp>4.8.&nbsp;Không để sách bị ẩm ướt, mốc hoặc hư hỏng dưới bất kỳ hình thức nào.\u003C/p>\u003Cp>\u003Cstrong>Điều 5: Quy định khi mượn/trả tài liệu\u003C/strong>\u003C/p>\u003Cp>5.1.&nbsp;Sau khi đọc sách xong, bạn đọc đặt sách về khu vực đã được quy định, không tự ý xếp sách lên giá;\u003C/p>\u003Cp>5.2.&nbsp;Không mang bất cứ tài liệu nào ra khỏi thư viện khi chưa làm thủ tục mượn về;\u003C/p>\u003Cp>5.3.&nbsp;Đối với giáo trình, tài liệu học tập: Mỗi sinh viên được mượn 1 bộ giáo trình, tài liệu dùng cho kỳ học hiện tại theo danh sách lớp của phòng Đào tạo. Những tài liệu giáo trình này, bạn đọc được gia hạn tối đa 1 tuần khi có lý do hợp lý;\u003C/p>\u003Cp>5.4.&nbsp;Đối với sách giáo trình được mượn như tài liệu tham khảo, hạn trả áp dụng như đối với sách tham khảo thông thường;\u003C/p>\u003Cp>5.5.&nbsp;Đối với tài liệu tham khảo: Bạn đọc được mượn tối đa 10 tên sách, trong thời hạn 1 tuần/ tài liệu tiếng Việt, 2 tuần/ tài liệu ngoại văn và được gia hạn 4 lần;\u003C/p>\u003Cp>&nbsp; &nbsp; &nbsp; &nbsp;5.6.&nbsp;Người mượn có trách nhiệm:\u003C/p>\u003Cp>5.6.1&nbsp;Kiểm tra và xác nhận quá trình giao dịch với thủ thư ngay tại quầy;\u003C/p>\u003Cp>5.6.2&nbsp;Kiểm tra tình trạng thực tế của tài liệu đã được ghi mượn trước khi mang ra khỏi thư viện, đồng thời giữ gìn, bảo quản tài liệu trong thời gian mượn. Khi phát hiện hư hỏng, người mượn cuối cùng của tài liệu đó sẽ chịu trách nhiệm bồi thường theo quy định.\u003C/p>\u003Cp>\u003Cstrong>Điều 6: Xử lý vi phạm nội quy thư viện\u003C/strong>\u003C/p>\u003Cp>6.1&nbsp;Bạn đọc vi phạm các quy định tại Điều 4, 5 tùy theo mức độ và lần vi phạm có thể bị nhắc nhở, khiển trách và mời ra khỏi thư viện; lập biên bản cảnh cáo, tạm ngừng sử dụng các dịch vụ thư viện hoặc sẽ bị tước quyền sử dụng các dịch vụ thư viện vĩnh viễn, tạm thời đình chỉ học tập hoặc buộc thôi học;\u003C/p>\u003Cp>6.2&nbsp;Trường hợp làm hư hại (như long bìa, nhàu nát, bôi bẩn, viết, vẽ, mất trang...) hoặc mất tài liệu, bạn đọc phải bồi thường thiệt hại tương đương với giá trị của tài liệu;\u003C/p>\u003Cp>&nbsp; &nbsp; &nbsp; &nbsp;6.3&nbsp;Trường hợp mượn tài liệu quá hạn sẽ phải chịu tiền phạt là: 5.000 đồng/ngày/1 cuốn kể cả ngày nghỉ;\u003C/p>\u003Cp>&nbsp; &nbsp; &nbsp; &nbsp;6.4 Trong bất cứ trường hợp vi phạm nào, bạn đọc phải đền bù thiệt hại theo quy định.\u003C/p>\u003Cp>\u003Cstrong>PHÒNG THÔNG TIN - THƯ VIỆN\u003C/strong>\u003C/p>\u003Cp>\u003Cstrong>LIBRARY REGULATIONS\u003C/strong>\u003C/p>\u003Cp>&nbsp;\u003C/p>\u003Cp>\u003Cstrong>Article 1: How to get library card?\u003C/strong>\u003C/p>\u003Cp>Your student ID card is your Library card. Use your Student or Staff ID card to access Library services and resources.\u003C/p>\u003Cp>\u003Cstrong>Article 2: Opening hours\u003C/strong>\u003C/p>\u003Cp>Monday - Friday\u003Cstrong>:\u003C/strong>&nbsp;8:15 – 21:00\u003C/p>\u003Cp>Weekend\u003Cstrong>:\u003C/strong>&nbsp;8:00 – 12:00, 13:00 – 17:00&nbsp; (Evenings and weekends the library only serves self-study)\u003C/p>\u003Cp>\u003Cstrong>Article&nbsp;3: Library services:\u003C/strong>\u003C/p>\u003Cp>3.1&nbsp;Circulation;\u003C/p>\u003Cp>3.2&nbsp;Seeking information;\u003C/p>\u003Cp>3.3&nbsp;Information consulting;\u003C/p>\u003Cp>3.4&nbsp;“Log - on - to e-resource”;\u003C/p>\u003Cp>3.5&nbsp;Request materials;\u003C/p>\u003Cp>3.6&nbsp;Interlibrary loan;\u003C/p>\u003Cp>3.7&nbsp;Group work rooms.\u003C/p>\u003Cp>\u003Cstrong>Article&nbsp;4: General regulations\u003C/strong>\u003C/p>\u003Cp>4.1 Patrons must present a valid card to enter library. Cards are non-transferrable;\u003C/p>\u003Cp>4.2&nbsp;Loud conversation is forbidden throughout the Library;\u003C/p>\u003Cp>4.3&nbsp;Please consciously keep the library clean, no smoking, no graffiti, no littering ...;\u003C/p>\u003Cp>4.4&nbsp;Food, drink, toxic and explosive substances in the Library are forbidden;\u003C/p>\u003Cp>4.5&nbsp;Please keep quiet in the library and set the mobile phone or computer and other equipment in silent mode. Do not talk to other via phone;\u003C/p>\u003Cp>4.6&nbsp;Do not use pencils, pens or highlighters in the book;\u003C/p>\u003Cp>4.7&nbsp;Do not bend or tear the pages of the book;\u003C/p>\u003Cp>4.8&nbsp;Do not let it get wet or mouldy or damaged in any way.\u003C/p>\u003Cp>\u003Cstrong>Article&nbsp;5: Circulation policies\u003C/strong>\u003C/p>\u003Cp>5.1&nbsp;Place books on designed area after finishing. Do not arrange books on shelf freely;\u003C/p>\u003Cp>5.2&nbsp;Materials may not be taken out of the FPT university library without permission of the Librarian;\u003C/p>\u003Cp>5.3&nbsp;Textbook for semester/block: Textbook are delivered according to academic calendar. Textbooks can only be renewed up to 1 week when you having suitable reasons;\u003C/p>\u003Cp>5.4&nbsp;Textbook for reference purposes: 1 week after new block start, patrons can borrow textbooks as reference books;\u003C/p>\u003Cp>5.5&nbsp;Reference book: Patrons can borrow up to 10 titles, loan period is 1 week for mother tongue books and 2 weeks for foreign language books, you can renew up to 4 times;\u003C/p>\u003Cp>5.6&nbsp;Patrons should:\u003C/p>\u003Cp>5.6.1&nbsp;Check list of books borrowed and confirm with librarian;\u003C/p>\u003Cp>5.6.2&nbsp;Make sure book condition notes at the end of your borrowed books are updated. You are responsible for the condition of everything while it is on loan to you. Please take good care of books or you pay for the damage according to the regulation.\u003C/p>\u003Cp>\u003Cstrong>Article&nbsp;6: Fines and penalty\u003C/strong>\u003C/p>\u003Cp>6.1&nbsp;Patrons who violate regulations mentioned in article 4 and 5 may get reminded, punished or requested to leave the library based on the violation level. Librarian may take the initial to write violation report, stop providing library services to those who break the law. In the worst situation, those students may get expelled from the university;\u003C/p>\u003Cp>6.2&nbsp;Damaged items which can be used will occur an amount depend on how serious of damage. The book is lost or damaged beyond repair, you have to buy this book;\u003C/p>\u003Cp>6.3&nbsp;Overdue books will be fined 5000 VNĐ/1 item/1 day including day off;\u003C/p>\u003Cp>6.4&nbsp;In any situation, patrons have to compensate for damage as prescribed\u003C/p>\u003Cp>\u003Cstrong>INFORMATION AND LIBRARY CENTER\u003C/strong>\u003C/p>', CAST(N'2024-10-13T11:43:18.040' AS DateTime))
GO
INSERT [dbo].[Post] ([id], [title], [content], [created_at]) VALUES (12, N'Giới thiệu Thư viện trường Đại học FPT Hà Nội', N'\u003Ch2>Thư viện trường ĐH FPT cơ sở Hà Nội&nbsp;\u003C/h2>\u003Ch4>Chính thức thành lập theo quyết định số 1029/QĐ-ĐHFPT ngày 09/12/2015 của Hiệu trưởng trường ĐH FPT. Chức năng và nhiệm vụ của thư viện như sau:\u003C/h4>\u003Cp>\u003Ci>\u003Cstrong>Chức năng: \u003C/strong>\u003C/i>Là phòng học liệu tích hợp được xây dựng và phát triển nhằm hỗ trợ hiệu quả việc giảng dạy, nghiên cứu và các hoạt động thông tin học thuật tới đội ngũ giảng viên, nhân viên và sinh viên FPT.\u003C/p>\u003Cp>\u003Ci>\u003Cstrong>Nhiệm vụ:\u003C/strong>\u003C/i>\u003C/p>\u003Cp>- Thu thập, bổ sung, xử lý, thông báo, cung cấp tài liệu, thông tin về các lĩnh vực khoa học nhằm hỗ trợ hiệu quả cho việc giảng dạy, nghiên cứu, học tập và các hoạt động học thuật của đội ngũ giảng viên, nhân viên và sinh viên;\u003C/p>\u003Cp>- Đảm bảo cung cấp thông tin cho người dùng tin một cách đầy đủ, chính xác, đúng đối tượng, điều tra, đánh giá đúng nhu cầu tin của giảng viên, cán bộ nghiên cứu, học viên cao học, nghiên cứu sinh và sinh viên;\u003C/p>\u003Cp>- Tham mưu, lập kế hoạch dài hạn, ngắn hạn cho giám đốc cơ sở đào tạo về công tác thông tin tư liệu;\u003C/p>\u003Cp>- Tổ chức sắp xếp, lưu trữ, bảo quản kho tư liệu của cơ sở bao gồm tất cả các loại hình ấn phẩm và vật mang tin;\u003C/p>\u003Cp>- Xây dựng hệ thống tra cứu tin thích hợp nhằm phục vụ và phổ biến thông tin cho toàn thể người dùng tin;\u003C/p>\u003Cp>- Thu thập, lưu chiểu những ấn phẩm do Nhà trường xuất bản, các luận văn, đồ án tốt nghiệp;\u003C/p>\u003Cp>- Phát triển quan hệ trao đổi, hợp tác trực tiếp với các trung tâm thông tin – thư viện, các tổ chức khoa học, các trường đại học trong và ngoài nước.\u003C/p>\u003Cp>\u003Cstrong>Bộ sưu tập\u003C/strong>\u003C/p>\u003Cp>- Thư viện đang quản lý gần 48.000 tên sách với hơn 85.000 bản sách;\u003C/p>\u003Cp>- 4&nbsp;cơ sở dữ liệu (CSDL):\u003C/p>\u003Cp>\u003Ci>*\u003Cstrong>CSDL thư mục (opac)\u003C/strong>\u003C/i>: Tra cứu&nbsp;sách giáo trình và tham khảo (sách giấy) có trong thư viện; hướng dẫn xem tại link&nbsp;\u003Ca href=\"https://library.fpt.edu.vn/Pages/Index/10\">https://library.fpt.edu.vn/Pages/Index/10\u003C/a>&nbsp;\u003C/p>\u003Cp>\u003Ci>*\u003Cstrong>CSDL nội sinh (Dspace)\u003C/strong>\u003C/i>:&nbsp;Tra cứu đồ án&nbsp;tốt&nbsp;nghiệp&nbsp;của sinh viên&nbsp;các&nbsp;khóa; tài nguyên môn học, bài báo khoa học,...;\u003Cbr>Hướng dẫn xem tại link:&nbsp;\u003Ca href=\"https://library.fpt.edu.vn/Pages/Index/13\">https://library.fpt.edu.vn/Pages/Index/13\u003C/a>\u003C/p>\u003Cp>*\u003Ci>\u003Cstrong>CSDL sách trực tuyến Books24x7\u003C/strong>\u003C/i>:&nbsp;Đây là CSDL trường ĐH FPT mua quyền truy cập hàng năm. CSDL này gồm tài liệu thuộc các chủ đề như: Công nghệ thông tin, Kỹ thuật, Kinh tế, tài chính,...&nbsp;hướng dẫn xem tại link:&nbsp;\u003Ca href=\"https://library.fpt.edu.vn/Pages/Index/11\">https://library.fpt.edu.vn/Pages/Index/11\u003C/a>\u003C/p>\u003Cp>\u003Ci>*\u003Cstrong>Cổng truy cập nguồn tin điện tử&nbsp;VISTA\u003C/strong>\u003C/i>: Cổng thông tin này có thể giúp bạn đọc tiếp cận tài liệu ở một số cơ sở dữ liệu nổi tiếng. Các CSDL&nbsp;bao gồm:\u003C/p>\u003Cp>- Sciencedirect &nbsp;-&nbsp;\u003Ca href=\"https://www.sciencedirect.com/\">https://www.sciencedirect.com/\u003C/a>&nbsp;\u003C/p>\u003Cp>- Springer -&nbsp;\u003Ca href=\"https://link.springer.com/\">https://link.springer.com/\u003C/a>&nbsp;\u003C/p>\u003Cp>- Proquest - &nbsp;\u003Ca href=\"https://www.proquest.com/\">https://www.proquest.com/\u003C/a>\u003C/p>\u003Cp>- IEEE - \u003Ca href=\"https://ieeexplore.ieee.org/Xplore/home.jsp\">https://ieeexplore.ieee.org/Xplore/home.jsp\u003C/a>\u003C/p>\u003Cp>\u003Cstrong>Đối với nguồn tin này, bạn đọc cần khai thác thông qua thủ thư tại thư viện\u003C/strong>\u003C/p>\u003Cp>Ngoài ra còn rất nhiều loại báo, tạp chí cùng các tài liệu với nhiều loại hình như: CD-Rom; DVD, hộp linh kiện điện tử,...\u003C/p>\u003Cp>\u003Cstrong>Thư viện thành viên:\u003C/strong>\u003C/p>\u003Cp>\u003Cstrong>1. Thư viện&nbsp;FPTU Hà Nội (Tên viết tắt:&nbsp;FPTU HN)\u003C/strong>\u003C/p>\u003Cp>Địa chỉ: Phòng 107 tòa nhà Delta, cơ sở&nbsp;Hòa&nbsp;Lạc,&nbsp;Km29 Đại lộ Thăng Long, Thạch Thất, Hà Nội\u003C/p>\u003Cp>Email: thuvien_fu_hoalac@fpt.edu.vn\u003C/p>\u003Cp>ĐT: 02466.805.912\u003C/p>\u003Cp>Fanpage:&nbsp;\u003Ca href=\"https://www.facebook.com/thuvienfu\">\u003Cstrong>https://www.facebook.com/thuvienfu\u003C/strong>\u003C/a>\u003C/p>\u003Cp>Website:&nbsp;\u003Ca href=\"https://library.fpt.edu.vn/\">\u003Cstrong>https://library.fpt.edu.vn/\u003C/strong>\u003C/a>\u003C/p>\u003Cp>\u003Cstrong>2. Thư viện&nbsp;FPTU Đà Nẵng&nbsp;(Tên viết tắt:&nbsp;FPTU DN)\u003C/strong>\u003C/p>\u003Cp>Email:&nbsp;thuvien_dn@fpt.edu.vn\u003C/p>\u003Cp>Panpage:&nbsp;\u003Ca href=\"https://www.facebook.com/LIBRARY.FPTUDN\">\u003Cstrong>https://www.facebook.com/LIBRARY.FPTUDN\u003C/strong>\u003C/a>\u003C/p>\u003Cp>\u003Cstrong>3. Thư viện&nbsp;FPTU Quy Nhơn&nbsp;(Tên viết tắt:&nbsp;FPTU QN)\u003C/strong>\u003C/p>\u003Cp>Địa chỉ: phòng 103 tòa nhà Beta, cơ sở Quy Nhơn, khu đô thị mới An Phú Thịnh, phường Nhơn Bình,TP Quy Nhơn\u003C/p>\u003Cp>Email: LIB.FUBD@fe.edu.vn\u003C/p>\u003Cp>Fanpage: \u003Ca href=\"https://www.facebook.com/LIBRARY.FPTUQN/\">\u003Cstrong>https://www.facebook.com/LIBRARY.FPTUQN/\u003C/strong>\u003C/a>\u003C/p>\u003Cp>\u003Cstrong>4. Thư viện FPTU Hồ Chí Minh&nbsp;(Tên viết tắt:&nbsp;FPTU HCM)\u003C/strong>\u003C/p>\u003Cp>Email: Lib.fuhcm@fe.edu.vn\u003Cbr>Fanpage: \u003Ca href=\"https://www.facebook.com/library.fuhcm\">\u003Cstrong>https://www.facebook.com/library.fuhcm\u003C/strong>\u003C/a>\u003Cbr>website: \u003Ca href=\"http://hcmlib.fpt.edu.vn/\">\u003Cstrong>http://hcmlib.fpt.edu.vn/\u003C/strong>\u003C/a>\u003C/p>\u003Cp>\u003Cstrong>5. Thư viện FPTU Cần Thơ&nbsp;(Tên viết tắt:&nbsp;FPTU CT)\u003C/strong>\u003C/p>\u003Cp>Email: lib.ct@fe.edu.vn\u003Cbr>Fanpage: \u003Ca href=\"https://www.facebook.com/thuviendaihocfpt\">\u003Cstrong>https://www.facebook.com/thuviendaihocfpt\u003C/strong>\u003C/a>\u003C/p>\u003Cp>\u003Cstrong>6. Thư viện Greenwich Việt Nam - cơ sở Hà Nội (viết tắt:&nbsp;FGW-HN)\u003C/strong>\u003Cbr>Địa chỉ: Tòa Golden Park, số 2 Phạm Văn Bạch, Yên Hòa, Cầu Giấy, Hà Nội\u003Cbr>Email: Lib.fgw.hn@fe.edu.vn\u003Cbr>ĐT: 02473 066 788\u003Cbr>Fanpage: \u003Ca href=\"https://www.facebook.com/LibraryofGreenwichVietnam\">\u003Cstrong>https://www.facebook.com/LibraryofGreenwichVietnam\u003C/strong>\u003C/a>\u003C/p>\u003Cp>\u003Cstrong>7. Thư viện Greenwich Việt Nam - cơ sở Đà Nẵng (viết tắt: FGW-DN)\u003C/strong>\u003Cbr>Địa chỉ: 658 Ngô Quyền, An Hải Bắc, Sơn Trà, Đà Nẵng\u003Cbr>Email: Lib.fgw.dn@fe.edu.vn\u003Cbr>ĐT: 02367 305 767\u003Cbr>Fanpage: \u003Ca href=\"https://www.facebook.com/LibraryFGWDN\">\u003Cstrong>https://www.facebook.com/LibraryFGWDN\u003C/strong>\u003C/a>\u003C/p>\u003Cp>\u003Cstrong>8.&nbsp;Thư viện Greenwich Việt Nam - cơ sở Cần Thơ (viết tắt: FGW-CT)\u003C/strong>\u003Cbr>Địa chỉ: Số 160, đường 30/4, phường An Phú, quận Ninh Kiều, thành phố Cần Thơ\u003Cbr>Email: Lib.fgw.ct@fe.edu.vn\u003Cbr>ĐT: 02923 512 369\u003C/p>\u003Cp>\u003Cstrong>9. Thư viện Greenwich Việt Nam - cơ sở Hồ Chí Minh (viết tắt: FGW-HCM)\u003C/strong>\u003Cbr>Địa chỉ: Tòa nhà Cộng Hòa garden số 20, đường Cộng Hòa, phường 12, quận Tân Bình, TP HCM\u003Cbr>Email: Lib.fgw.hcm@fe.edu.vn\u003Cbr>ĐT: 0338 821 365\u003C/p>\u003Cp>\u003Cstrong>Video giới thiệu:\u003C/strong>\u003C/p>\u003Cfigure class=\"media\">\u003Cdiv data-oembed-url=\"https://youtu.be/GNp6tVgZLM4\">\u003Cdiv style=\"position: relative; padding-bottom: 100%; height: 0; padding-bottom: 56.2493%;\">\u003Ciframe src=\"https://www.youtube.com/embed/GNp6tVgZLM4\" style=\"position: absolute; width: 100%; height: 100%; top: 0; left: 0;\" frameborder=\"0\" allow=\"autoplay; encrypted-media\" allowfullscreen=\"\">\u003C/iframe>\u003C/div>\u003C/div>\u003C/figure>', CAST(N'2024-10-13T19:35:03.780' AS DateTime))
GO
INSERT [dbo].[Post] ([id], [title], [content], [created_at]) VALUES (13, N'Dịch vụ mượn, trả tài liệu', N'\u003Ch2>Dịch vụ này nhằm mục đích giúp bạn đọc có thể mượn sách về nhà&nbsp;\u003C/h2>\u003Ch4>Bạn đọc cần biết một số thông tin sau:\u003C/h4>\u003Cp>\u003Cstrong>1. MƯỢN SÁCH\u003C/strong>\u003C/p>\u003Cp>\u003Cstrong>1.1 Hạn ngạch\u003C/strong>:\u003C/p>\u003Cp>- Bạn đọc là Sinh viên, học sinh: Được mượn tối đa 10 tài liệu cùng lúc;\u003C/p>\u003Cp>- Bạn đọc là CBGV: Được mượn tối đa 20 tài liệu cùng lúc.\u003C/p>\u003Cp>\u003Cstrong>1.2 Điều kiện\u003C/strong>:\u003C/p>\u003Cp>- Bạn đọc cần xuất trình thẻ sinh viên hoặc căn cước công dân khi mượn (KHÔNG DÙNG ẢNH CHỤP);\u003C/p>\u003Cp>- Không sử dụng thẻ của bạn đọc khác khi mượn sách tại thư viện;\u003C/p>\u003Cp>- Bạn đọc không vi phạm nội quy thư viện.\u003C/p>\u003Cp>\u003Cstrong>1.3 Cách thức mượn đối với sách tham khảo:\u003C/strong>\u003C/p>\u003Cp>- Bạn đọc tự tìm sách trên giá, xem hướng dẫn tìm sách \u003Ca href=\"https://library.fpt.edu.vn/Pages/Index/10\">\u003Cstrong>TẠI ĐÂY\u003C/strong>\u003C/a>\u003C/p>\u003Cp>- Lựa chọn các tài liệu phù hợp\u003C/p>\u003Cp>- Làm thủ tục mượn tại quầy thủ thư.\u003C/p>\u003Cp>\u003Ci>\u003Cstrong>- Thời hạn mượn: Sách tiếng Việt&nbsp;7 ngày/tài liệu | sách ngoại văn, song ngữ:&nbsp;14 ngày/tài liệu\u003C/strong>\u003C/i>\u003C/p>\u003Cp>\u003Cstrong>1.4 Cách thức mượn đối với sách giáo trình:\u003C/strong>\u003C/p>\u003Cp>- Bạn đọc mượn, nhận giáo trình tại quầy thủ thư;\u003C/p>\u003Cp>- Trong vòng 3 ngày sau khi mượn, nếu phát hiện sách bị hư hại, bạn đọc có thể đổi một cuốn sách khác.\u003C/p>\u003Cp>\u003Ci>\u003Cstrong>- Thời hạn mượn: Theo kỳ học như lịch&nbsp;thông báo.\u003C/strong>\u003C/i>\u003C/p>\u003Cp>\u003Cstrong>1.5 Yêu cầu khi mượn sách:\u003C/strong>\u003C/p>\u003Cp>- Cần kiểm tra kỹ tình trạng sách trước khi rời khỏi quầy thủ thư;\u003C/p>\u003Cp>- Cần trả sách đúng hạn, không để quá hạn sách;\u003C/p>\u003Cp>- Phải giữ gìn sách cẩn thận, không viết, vẽ, làm ướt, tẩy, xóa lên nội dung sách.\u003C/p>\u003Cp>\u003Cstrong>1.6 Phí phạt nếu vi phạm nội quy\u003C/strong>:\u003C/p>\u003Cp>- Quá hạn sách: 5.000vnđ/tài liệu/ngày (bao gồm ngày nghỉ);\u003C/p>\u003Cp>- Đền sách: Bằng giá tiền thư viện nhập sách cộng tiền quá hạn sách&nbsp;(nếu có);\u003C/p>\u003Cp>- Hư hỏng sách: Tùy theo mức độ ảnh hưởng, thủ thư sẽ đưa ra mức phí phạt phù hợp, tối đa bằng giá đền sách.\u003C/p>\u003Cp>\u003Cstrong>2. TRẢ SÁCH\u003C/strong>\u003C/p>\u003Cp>Bạn đọc mang sách đã mượn tới quầy thủ thư (phòng 107 hoặc phòng 108 tòa nhà Delta) để thực hiện trả sách\u003C/p>\u003Cp>Yêu cầu:\u003C/p>\u003Cp>- Bạn đọc cần trả sách đúng hạn, không để quá hạn sách;\u003C/p>\u003Cp>- Có thể nhờ người khác mang sách đi trả.\u003C/p>\u003Cp>\u003Cstrong>Thông tin liên hệ:\u003C/strong>\u003C/p>\u003Cp>SĐT: 02466.805.912 (gọi trong giờ hành chính)\u003C/p>\u003Cp>Email: \u003Cstrong>thuvien_fu_hoalac@fpt.edu.vn\u003C/strong>\u003C/p>\u003Cp>Website: \u003Ca href=\"http://library.fpt.edu.vn/\">\u003Cstrong>http://library.fpt.edu.vn\u003C/strong>\u003C/a>\u003C/p>\u003Cp>Fanpage: \u003Ca href=\"https://www.facebook.com/thuvienfu\">\u003Cstrong>https://www.facebook.com/thuvienfu\u003C/strong>\u003C/a>\u003C/p>', CAST(N'2024-10-13T19:35:44.273' AS DateTime))
GO
INSERT [dbo].[Post] ([id], [title], [content], [created_at]) VALUES (14, N'Dịch vụ gia hạn sách', N'\u003Cp>Bạn đọc&nbsp;thân mến,\u003C/p>\u003Cp>Thư viện xin hướng dẫn các bạn các cách gia hạn sách như sau:\u003C/p>\u003Cp>\u003Cstrong>Cách\u003C/strong>&nbsp;𝟏: \u003Cstrong>Bản thân tự gia hạn, hướng dẫn \u003C/strong>\u003Ca href=\"https://library.fpt.edu.vn/Pages/Index/7\">\u003Cstrong>TẠI ĐÂY\u003C/strong>\u003C/a>\u003Cstrong>:\u003C/strong>\u003C/p>\u003Cp>\u003Cstrong>Cách\u003C/strong>&nbsp;𝟐: \u003Cstrong>Gửi email tới \"Thuvien_fu_hoalac@fpt.edu.vn\"\u003C/strong>\u003C/p>\u003Cp>- Chủ đề mail \"Xin gia hạn sách\"\u003C/p>\u003Cp>- Nội dung mail các bạn cần&nbsp;\u003Ci>\u003Cstrong>ghi rõ họ tên +&nbsp;mã sinh viên/nhân viên,\u003C/strong>\u003C/i>\u003C/p>\u003Cp>- Lý do xin gia hạn sách và thời hạn mong muốn nếu cần (để thư viện xem xét tùy từng trường hợp nha)\u003C/p>\u003Cp>- Ký tên (VD Họ tên, số điện thoại liên hệ, CLB em tham gia,...)\u003C/p>\u003Cp>LƯU Ý: Tuyệt đối 𝐊𝐇\u003Cstrong>Ô\u003C/strong>̂𝐍𝐆 𝐑𝐄𝐏𝐋𝐘 𝐀𝐔𝐓𝐎𝐌𝐀𝐈𝐋, các bạn nhớ soạn mail mới để đặt chủ đề mail đúng nha. \u003Ci>(Mail tự động ngoài việc thông báo hạn trả còn là hướng dẫn cách gia hạn nên các bạn đọc kỹ chữ đừng bỏ qua nhé.)\u003C/i>\u003C/p>\u003Cp>\u003Cstrong>Cách \u003C/strong>𝟑: Gọi điện tới&nbsp;Thư viện (gọi trong giờ hành chính) theo số: 024 6680 5912\u003C/p>\u003Cp>\u003Cstrong>Cách&nbsp;\u003C/strong>𝟒: Inbox page \u003Ca href=\"https://www.facebook.com/thuvienfu\">https://www.facebook.com/thuvienfu\u003C/a>, gửi mã số Sinh viên/ nhân viên để Admin hỗ trợ nhé.\u003C/p>\u003Cp>Chúc các bạn luôn có nhiều niềm vui và học tập tốt!\u003C/p>', CAST(N'2024-10-13T19:36:03.410' AS DateTime))
GO
INSERT [dbo].[Post] ([id], [title], [content], [created_at]) VALUES (16, N'Giờ mở cửa', N'\u003Ch2>\u003Cstrong>Giờ mở cửa, phục vụ tại Thư viện FPTU HN\u003C/strong>\u003C/h2>\u003Cfigure class=\"table\">\u003Ctable>\u003Ctbody>\u003Ctr>\u003Ctd>\u003Cstrong>TT\u003C/strong>\u003C/td>\u003Ctd>\u003Cstrong>NGÀY&nbsp;\u003C/strong>\u003C/td>\u003Ctd colspan=\"2\">\u003Cstrong>THỜI GIAN MỞ CỬA\u003C/strong>\u003C/td>\u003Ctd>\u003Cstrong>THỜI GIAN PHỤC VỤ MƯỢN – TRẢ SÁCH\u003C/strong>\u003C/td>\u003C/tr>\u003Ctr>\u003Ctd>01\u003C/td>\u003Ctd>Thứ Hai - Thứ Sáu\u003C/td>\u003Ctd colspan=\"2\">8:15&nbsp; – &nbsp; 21:00\u003C/td>\u003Ctd>\u003Cp>\u003Cstrong>Sáng: 08:30 - 12:00\u003C/strong>\u003C/p>\u003Cp>\u003Cstrong>Chiều: 13:00 -&nbsp;17:00\u003C/strong>\u003C/p>\u003C/td>\u003C/tr>\u003Ctr>\u003Ctd>02\u003C/td>\u003Ctd>Cuối tuần\u003C/td>\u003Ctd>8:00&nbsp; – &nbsp; 12:00\u003C/td>\u003Ctd>13:00&nbsp; – &nbsp; 17:00\u003C/td>\u003Ctd>\u003Cstrong>Không phục vụ\u003C/strong>\u003C/td>\u003C/tr>\u003C/tbody>\u003C/table>\u003C/figure>\u003Cp>&nbsp;\u003C/p>\u003Cp>\u003Cstrong>Lưu ý: Các buổi tối và cuối tuần&nbsp;thư viện chỉ phục vụ chỗ tự học, không phục vụ mượn, trả sách&nbsp;hay các dịch vụ khác.\u003C/strong>\u003C/p>\u003Cp>Mọi thắc mắc xin liên hệ:\u003C/p>\u003Cp>- Số ĐT: 024 6680 5912 (Gọi trong giờ hành chính)\u003C/p>\u003Cp>- Email:&nbsp;\u003Ca href=\"mailto:Thuvien_fu_hoalac@fpt.edu.vn\">\u003Cstrong>Thuvien_fu_hoalac@fpt.edu.vn\u003C/strong>\u003C/a>\u003C/p>\u003Cp>- Fanpage:&nbsp;\u003Ca href=\"https://www.facebook.com/thuvienfu\">\u003Cstrong>https://www.facebook.com/thuvienfu\u003C/strong>\u003C/a>\u003C/p>\u003Cp>- Website:&nbsp;\u003Ca href=\"https://library.fpt.edu.vn/\">\u003Cstrong>https://library.fpt.edu.vn/\u003C/strong>\u003C/a>\u003C/p>', CAST(N'2024-10-13T20:17:21.737' AS DateTime))
GO
INSERT [dbo].[Post] ([id], [title], [content], [created_at]) VALUES (18, N'Dịch vụ phòng học nhóm', N'\u003Ch2>Quy định sử dụng phòng học nhóm trong thư viện\u003C/h2>\u003Cp>Phòng học nhóm trong Thư viện được thiết kế nhằm phục vụ hoạt động học tập, nghiên cứu theo nhóm. Phòng được thiết kế cách âm, có sức chứa tối đa 8 người/phòng.\u003C/p>\u003Cp>\u003Cstrong>Bạn đọc truy cập link dưới bài viết để xem giờ trống và đăng ký sử dụng tại quầy Thủ thư\u003C/strong>.\u003C/p>\u003Cp>\u003Cstrong>1. Đối tượng sử dụng\u003C/strong>: Là bạn đọc của Thư viện FPTU Hà Nội.\u003C/p>\u003Cp>\u003Cstrong>2. Thời gian sử dụng\u003C/strong>: Tối đa là 2 giờ/1 lần đăng ký.\u003C/p>\u003Cp>\u003Cstrong>3. Điều kiện sử dụng:\u003C/strong>&nbsp;&nbsp;\u003C/p>\u003Cp>\u003Cstrong>- Bạn đọc chỉ được phép book trước 1 ngày;\u003C/strong>\u003C/p>\u003Cp>- \u003Cstrong>Mỗi nhóm chỉ book duy nhất 1 ca/ngày\u003C/strong>;\u003C/p>\u003Cp>-&nbsp;\u003Cstrong>MỖI NHÓM CHỈ MỘT THÀNH VIÊN DUY NHẤT BOOK\u003C/strong>;\u003C/p>\u003Cp>\u003Cstrong>- Sau 15 phút kể từ thời gian book, nếu nhóm không sử dụng phòng, đơn&nbsp;book sẽ bị hủy;\u003C/strong>\u003C/p>\u003Cp>- Số lượng thành viên trong nhóm tối thiểu là 4 người, tối đa 8 người (nếu không đủ số lượng thành viên sau 15 phút, ca book sẽ bị hủy).\u003C/p>\u003Cp>- Giữ gìn tài sản, không xê dịch, thay đổi, làm hư hỏng đồ dùng trong phòng;\u003C/p>\u003Cp>- Giữ gìn vệ sinh chung, không mang đồ ăn, thức uống, đồ dễ cháy nổ vào phòng;\u003C/p>\u003Cp>- Bạn đọc tự bảo quản tài sản cá nhân khi sử dụng phòng.\u003C/p>\u003Cp>- Chỉ book trong khoảng thời gian từ 08:30 - 17:00 từ thứ Hai tới thứ Sáu.\u003C/p>\u003Cp>\u003Cstrong>4. Xử lý vi phạm\u003C/strong>:&nbsp;\u003C/p>\u003Cp>- Bạn đọc vi phạm nội quy tại mục 3 sẽ bị hủy quyền book phòng lần sau;\u003C/p>\u003Cp>- Nếu làm hư hỏng đồ dùng, bạn đọc sẽ phải đền theo giá trị tài sản;\u003C/p>\u003Cp>- Bạn đọc sử dụng không đúng mục đích sẽ bị thu hồi.&nbsp;\u003C/p>\u003Cp>\u003Cstrong>HƯỚNG DẪN: Trước khi book phòng,&nbsp;Bạn đọc nên xem danh sách các nhóm đã book để không bị trùng khung giờ sử dụng.\u003C/strong>\u003C/p>\u003Cp>Link xem thời gian rảnh: \u003Ca href=\"https://fptuniversity-my.sharepoint.com/:x:/g/personal/thuvien_fu_hoalac_fpt_edu_vn/EfM935MYUh5JvXkWZEfPfm0BGbWVpEpaeFuloWjy5vIVsw?e=cpf4Ei\">\u003Cstrong>TẠI ĐÂY\u003C/strong>\u003C/a>\u003C/p>\u003Cp>\u003Cstrong>Đăng ký: Tại quầy Thủ thư\u003C/strong>\u003C/p>\u003Cp>\u003Cstrong>-------\u003C/strong>\u003C/p>\u003Cp>Trân trọng!&nbsp;\u003C/p>\u003Cp>Phòng Thông tin - Thư viện\u003C/p>\u003Cp>Office tel: 02466.805.912 (gọi trong giờ hành chính)\u003C/p>\u003Cp>Fanpage: \u003Ca href=\"https://www.facebook.com/thuvienfu\">\u003Cstrong>https://www.facebook.com/thuvienfu\u003C/strong>\u003C/a>\u003C/p>\u003Cp>Email: \u003Cstrong>thuvien_fu_hoalac@fpt.edu.vn\u003C/strong>\u003C/p>', CAST(N'2024-10-13T20:19:52.393' AS DateTime))
GO
INSERT [dbo].[Post] ([id], [title], [content], [created_at]) VALUES (21, N'anime', N'\u003Cp>anime 123123 \u003Cimg src=\"https://ckeditor.com/apps/ckfinder/userfiles/files/images/lemur.jpg\">\u003C/p>', CAST(N'2024-11-04T11:07:51.027' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[Post] OFF
GO
SET IDENTITY_INSERT [dbo].[Reports] ON 
GO
INSERT [dbo].[Reports] ([report_id], [report_reason], [user_id], [status], [borrow_id], [created_at], [fine_id]) VALUES (1, N'Library system error with checkout dates', 5, N'done', NULL, CAST(N'2024-11-04T22:19:23.210' AS DateTime), 5)
GO
SET IDENTITY_INSERT [dbo].[Reports] OFF
GO
SET IDENTITY_INSERT [dbo].[Roles] ON 
GO
INSERT [dbo].[Roles] ([id], [name]) VALUES (1, N'ADMIN')
GO
INSERT [dbo].[Roles] ([id], [name]) VALUES (2, N'STUDENT')
GO
INSERT [dbo].[Roles] ([id], [name]) VALUES (3, N'LIBRARIAN')
GO
SET IDENTITY_INSERT [dbo].[Roles] OFF
GO
SET IDENTITY_INSERT [dbo].[Thesis] ON 
GO
INSERT [dbo].[Thesis] ([id], [title], [author], [file_doc], [create_at]) VALUES (3, N'Đồ án Spring 2024', N'Chu Thiên Quân, Nguyễn Hiếu', N'https://firebasestorage.googleapis.com/v0/b/fir-60e00.appspot.com/o/documents%2FCQ1_Product%20End%20Users-1114931-264122.xlsx?alt=media&token=d6c435bc-4b6f-4d25-8798-83116dcaf2d6', CAST(N'2024-11-07' AS Date))
GO
INSERT [dbo].[Thesis] ([id], [title], [author], [file_doc], [create_at]) VALUES (4, N'Đồ án Summer 2024', N'Nguyễn Hà, Minh Bùi', N'https://firebasestorage.googleapis.com/v0/b/fir-60e00.appspot.com/o/documents%2FCQ1_Product%20End%20Users-1114931-264122.xlsx?alt=media&token=d8d312c9-5764-4bb0-aaf5-893a7979908a', CAST(N'2024-11-07' AS Date))
GO
SET IDENTITY_INSERT [dbo].[Thesis] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 
GO
INSERT [dbo].[Users] ([user_id], [email], [password], [student_code], [fullname], [phone], [role], [status], [create_at], [verify_code], [expiration_code], [image]) VALUES (1, N'student1@gmail.com', N'password1', N'HE171111', N'Tây', N'0123456781', N'STUDENT', 1, CAST(N'2024-10-08' AS Date), NULL, NULL, NULL)
GO
INSERT [dbo].[Users] ([user_id], [email], [password], [student_code], [fullname], [phone], [role], [status], [create_at], [verify_code], [expiration_code], [image]) VALUES (2, N'student2@gmail.com', N'password2', N'HE171222', N'Long', N'0123456782', N'STUDENT', 1, CAST(N'2024-10-08' AS Date), NULL, NULL, NULL)
GO
INSERT [dbo].[Users] ([user_id], [email], [password], [student_code], [fullname], [phone], [role], [status], [create_at], [verify_code], [expiration_code], [image]) VALUES (3, N'student3@gmail.com', N'password3', N'HE171333', N'Hiếu', N'0123456783', N'STUDENT', 1, CAST(N'2024-10-08' AS Date), NULL, NULL, NULL)
GO
INSERT [dbo].[Users] ([user_id], [email], [password], [student_code], [fullname], [phone], [role], [status], [create_at], [verify_code], [expiration_code], [image]) VALUES (4, N'admin@gmail.com', N'0192023a7bbd73250516f069df18b500', N'HE171444', N'Hà', N'0123456789', N'ADMIN', 1, CAST(N'2024-10-06' AS Date), NULL, NULL, NULL)
GO
INSERT [dbo].[Users] ([user_id], [email], [password], [student_code], [fullname], [phone], [role], [status], [create_at], [verify_code], [expiration_code], [image]) VALUES (5, N'student@gmail.com', N'ad6a280417a0f533d8b670c61667e1a0', N'HE171555', N'Hiếu', N'0987632111', N'STUDENT', 1, CAST(N'2024-10-07' AS Date), NULL, NULL, N'https://firebasestorage.googleapis.com/v0/b/fir-60e00.appspot.com/o/images%2Fm.jpg?alt=media&token=82c12ab4-501a-4617-a43f-cc559bb8d363')
GO
INSERT [dbo].[Users] ([user_id], [email], [password], [student_code], [fullname], [phone], [role], [status], [create_at], [verify_code], [expiration_code], [image]) VALUES (6, N'librarian@gmail.com', N'16e70200e2731e74d6c05bc0316cf293', N'HE171666', N'Quân', N'0654321566', N'LIBRARIAN', 1, CAST(N'2024-10-07' AS Date), NULL, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Users__AB6E616400D088D7]    Script Date: 11/9/2024 8:01:53 PM ******/
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [UQ__Users__AB6E616400D088D7] UNIQUE NONCLUSTERED 
(
	[email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Books] ADD  CONSTRAINT [DF__Books__created_a__4D94879B]  DEFAULT (getdate()) FOR [created_at]
GO
ALTER TABLE [dbo].[Post] ADD  CONSTRAINT [DF_Post_CreatedAt]  DEFAULT (getdate()) FOR [created_at]
GO
ALTER TABLE [dbo].[Books]  WITH CHECK ADD  CONSTRAINT [FK_Books_Categories] FOREIGN KEY([category_id])
REFERENCES [dbo].[Categories] ([category_id])
GO
ALTER TABLE [dbo].[Books] CHECK CONSTRAINT [FK_Books_Categories]
GO
ALTER TABLE [dbo].[Books_Authors]  WITH CHECK ADD  CONSTRAINT [FK_Books_Authors_Authors] FOREIGN KEY([author_id])
REFERENCES [dbo].[Authors] ([author_id])
GO
ALTER TABLE [dbo].[Books_Authors] CHECK CONSTRAINT [FK_Books_Authors_Authors]
GO
ALTER TABLE [dbo].[Books_Authors]  WITH CHECK ADD  CONSTRAINT [FK_Books_Authors_Books] FOREIGN KEY([book_id])
REFERENCES [dbo].[Books] ([book_id])
GO
ALTER TABLE [dbo].[Books_Authors] CHECK CONSTRAINT [FK_Books_Authors_Books]
GO
ALTER TABLE [dbo].[BorrowBooks]  WITH CHECK ADD  CONSTRAINT [FK_BorrowBooks_Books] FOREIGN KEY([book_id])
REFERENCES [dbo].[Books] ([book_id])
GO
ALTER TABLE [dbo].[BorrowBooks] CHECK CONSTRAINT [FK_BorrowBooks_Books]
GO
ALTER TABLE [dbo].[BorrowBooks]  WITH CHECK ADD  CONSTRAINT [FK_BorrowBooks_Users] FOREIGN KEY([user_id])
REFERENCES [dbo].[Users] ([user_id])
GO
ALTER TABLE [dbo].[BorrowBooks] CHECK CONSTRAINT [FK_BorrowBooks_Users]
GO
ALTER TABLE [dbo].[Fines]  WITH CHECK ADD  CONSTRAINT [FK_Fines_BorrowBooks] FOREIGN KEY([borrow_id])
REFERENCES [dbo].[BorrowBooks] ([borrow_id])
GO
ALTER TABLE [dbo].[Fines] CHECK CONSTRAINT [FK_Fines_BorrowBooks]
GO
ALTER TABLE [dbo].[Reports]  WITH CHECK ADD  CONSTRAINT [FK_Reports_BorrowedBooks] FOREIGN KEY([borrow_id])
REFERENCES [dbo].[BorrowBooks] ([borrow_id])
GO
ALTER TABLE [dbo].[Reports] CHECK CONSTRAINT [FK_Reports_BorrowedBooks]
GO
ALTER TABLE [dbo].[Reports]  WITH CHECK ADD  CONSTRAINT [FK_Reports_Fines] FOREIGN KEY([fine_id])
REFERENCES [dbo].[Fines] ([fine_id])
GO
ALTER TABLE [dbo].[Reports] CHECK CONSTRAINT [FK_Reports_Fines]
GO
ALTER TABLE [dbo].[Reports]  WITH CHECK ADD  CONSTRAINT [FK_Reports_Users] FOREIGN KEY([user_id])
REFERENCES [dbo].[Users] ([user_id])
GO
ALTER TABLE [dbo].[Reports] CHECK CONSTRAINT [FK_Reports_Users]
GO
USE [master]
GO
ALTER DATABASE [LibraryManageSystem] SET  READ_WRITE 
GO
