USE [master]
GO
/****** Object:  Database [AcmeCase]    Script Date: 21/10/2021 16:26:10 ******/
CREATE DATABASE [AcmeCase]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'AcmeCase', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS01\MSSQL\DATA\AcmeCase.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'AcmeCase_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS01\MSSQL\DATA\AcmeCase_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [AcmeCase] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [AcmeCase].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [AcmeCase] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [AcmeCase] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [AcmeCase] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [AcmeCase] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [AcmeCase] SET ARITHABORT OFF 
GO
ALTER DATABASE [AcmeCase] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [AcmeCase] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [AcmeCase] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [AcmeCase] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [AcmeCase] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [AcmeCase] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [AcmeCase] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [AcmeCase] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [AcmeCase] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [AcmeCase] SET  DISABLE_BROKER 
GO
ALTER DATABASE [AcmeCase] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [AcmeCase] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [AcmeCase] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [AcmeCase] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [AcmeCase] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [AcmeCase] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [AcmeCase] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [AcmeCase] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [AcmeCase] SET  MULTI_USER 
GO
ALTER DATABASE [AcmeCase] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [AcmeCase] SET DB_CHAINING OFF 
GO
ALTER DATABASE [AcmeCase] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [AcmeCase] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [AcmeCase] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [AcmeCase] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [AcmeCase] SET QUERY_STORE = OFF
GO
USE [AcmeCase]
GO
/****** Object:  Table [dbo].[tblAuditTrail]    Script Date: 21/10/2021 16:26:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblAuditTrail](
	[AuditID] [bigint] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[UserName] [nvarchar](100) NOT NULL,
	[IPAddress] [nvarchar](50) NOT NULL,
	[AuditDate] [datetime] NOT NULL,
	[RequestName] [nvarchar](100) NOT NULL,
	[Request] [nvarchar](max) NULL,
	[Response] [nvarchar](max) NULL,
 CONSTRAINT [PK_tblAuditTrail] PRIMARY KEY CLUSTERED 
(
	[AuditID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblCase]    Script Date: 21/10/2021 16:26:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblCase](
	[CaseID] [int] IDENTITY(1,1) NOT NULL,
	[CaseName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_tblCase] PRIMARY KEY CLUSTERED 
(
	[CaseID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblClient]    Script Date: 21/10/2021 16:26:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblClient](
	[ClientID] [int] IDENTITY(1,1) NOT NULL,
	[ClientIdentifier] [uniqueidentifier] NULL,
	[ClientName] [nvarchar](50) NULL,
 CONSTRAINT [PK_tblClient] PRIMARY KEY CLUSTERED 
(
	[ClientID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblClientCase]    Script Date: 21/10/2021 16:26:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblClientCase](
	[ClientCaseID] [int] IDENTITY(1,1) NOT NULL,
	[ClientID] [int] NOT NULL,
	[CaseID] [int] NOT NULL,
 CONSTRAINT [PK_tblClientCase] PRIMARY KEY CLUSTERED 
(
	[ClientCaseID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UC_ClientCase] UNIQUE NONCLUSTERED 
(
	[ClientID] ASC,
	[CaseID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblFile]    Script Date: 21/10/2021 16:26:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblFile](
	[FileID] [int] IDENTITY(1,1) NOT NULL,
	[CaseID] [int] NOT NULL,
	[FileTypeID] [int] NOT NULL,
	[FileData] [varbinary](max) NOT NULL,
	[FileDisplayName] [nvarchar](50) NOT NULL,
	[FileUploadedDate] [datetime] NOT NULL,
	[FileVisible] [bit] NOT NULL,
	[FileComment] [nvarchar](max) NULL,
 CONSTRAINT [PK_tblFile] PRIMARY KEY CLUSTERED 
(
	[FileID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblFileType]    Script Date: 21/10/2021 16:26:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblFileType](
	[FileTypeID] [int] IDENTITY(1,1) NOT NULL,
	[FileTypeName] [nvarchar](50) NOT NULL,
	[FileTypeExtension] [nvarchar](10) NOT NULL,
 CONSTRAINT [PK_tblFileType] PRIMARY KEY CLUSTERED 
(
	[FileTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Index [IX_tblClient]    Script Date: 21/10/2021 16:26:10 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_tblClient] ON [dbo].[tblClient]
(
	[ClientIdentifier] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[tblClientCase]  WITH CHECK ADD  CONSTRAINT [FK_tblClientCase_tblCase] FOREIGN KEY([CaseID])
REFERENCES [dbo].[tblCase] ([CaseID])
GO
ALTER TABLE [dbo].[tblClientCase] CHECK CONSTRAINT [FK_tblClientCase_tblCase]
GO
ALTER TABLE [dbo].[tblClientCase]  WITH CHECK ADD  CONSTRAINT [FK_tblClientCase_tblClient] FOREIGN KEY([ClientID])
REFERENCES [dbo].[tblClient] ([ClientID])
GO
ALTER TABLE [dbo].[tblClientCase] CHECK CONSTRAINT [FK_tblClientCase_tblClient]
GO
ALTER TABLE [dbo].[tblFile]  WITH CHECK ADD  CONSTRAINT [FK_tblFile_tblCase] FOREIGN KEY([CaseID])
REFERENCES [dbo].[tblCase] ([CaseID])
GO
ALTER TABLE [dbo].[tblFile] CHECK CONSTRAINT [FK_tblFile_tblCase]
GO
ALTER TABLE [dbo].[tblFile]  WITH CHECK ADD  CONSTRAINT [FK_tblFile_tblFileType] FOREIGN KEY([FileTypeID])
REFERENCES [dbo].[tblFileType] ([FileTypeID])
GO
ALTER TABLE [dbo].[tblFile] CHECK CONSTRAINT [FK_tblFile_tblFileType]
GO
/****** Object:  StoredProcedure [dbo].[Add_Audit_Trail]    Script Date: 21/10/2021 16:26:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Add_Audit_Trail]
@UserID int, 
@UserName nvarchar(100), 
@IPAddress nvarchar(50), 
@RequestName nvarchar(100), 
@Request nvarchar(max) = NULL, 
@Response nvarchar(max) = NULL
AS
BEGIN
	INSERT INTO tblAuditTrail(UserID, UserName, IPAddress, AuditDate, RequestName, Request, Response)
	VALUES (@UserID, @UserName, @IPAddress, GETDATE(), @RequestName, @Request, @Response)
END
GO
/****** Object:  StoredProcedure [dbo].[Get_All_Files_Info]    Script Date: 21/10/2021 16:26:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Get_All_Files_Info]
@ClientIdentifier uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT F.FileID, F.FileDisplayName, FT.FileTypeExtension, F.FileComment FROM tblClient C
	INNER JOIN tblClientCase CC ON C.ClientID=CC.ClientID
	INNER JOIN tblFile F ON CC.CaseID=F.CaseID
	INNER JOIN tblFileType FT ON F.FileTypeID=FT.FileTypeID
	WHERE C.ClientIdentifier=@ClientIdentifier

END
GO
/****** Object:  StoredProcedure [dbo].[Get_Case_Files_Info]    Script Date: 21/10/2021 16:26:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Get_Case_Files_Info]
@CaseID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT F.FileID, F.FileDisplayName, FT.FileTypeExtension, F.FileComment FROM tblFile F
	INNER JOIN tblFileType FT ON F.FileTypeID=FT.FileTypeID
	WHERE F.CaseID=@CaseID AND F.FileVisible=1
END
GO
/****** Object:  StoredProcedure [dbo].[Get_Client_Cases]    Script Date: 21/10/2021 16:26:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Get_Client_Cases]
@ClientIdentifier uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @ClientID int = (SELECT ClientID FROM tblClient WHERE ClientIdentifier = @ClientIdentifier)

    SELECT C.CaseID, C.CaseName FROM tblCase C
	INNER JOIN tblClientCase CC ON C.CaseID = CC.CaseID
	WHERE CC.ClientID=@ClientID
END
GO
/****** Object:  StoredProcedure [dbo].[Get_File_For_Download]    Script Date: 21/10/2021 16:26:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Get_File_For_Download]
@FileID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT F.FileDisplayName, FT.FileTypeExtension, F.FileData FROM tblFile F
	INNER JOIN tblFileType FT ON F.FileTypeID=FT.FileTypeID
	WHERE F.FileID=@FileID AND F.FileVisible=1
END
GO
/****** Object:  StoredProcedure [dbo].[Validate_Case_To_Client]    Script Date: 21/10/2021 16:26:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Validate_Case_To_Client]
@CaseID int,
@ClientIdentifier uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @IsValid bit = 0

	SELECT @IsValid=1 FROM tblClient C
	INNER JOIN tblClientCase CC ON C.ClientID=CC.ClientID
	WHERE C.ClientIdentifier=@ClientIdentifier AND CC.CaseID=@CaseID

	SELECT @IsValid

END
GO
/****** Object:  StoredProcedure [dbo].[Validate_File_To_Client]    Script Date: 21/10/2021 16:26:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Validate_File_To_Client]
@FileID int,
@ClientIdentifier uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @IsValid bit = 0

	SELECT @IsValid=1 FROM tblClient C
	INNER JOIN tblClientCase CC ON C.ClientID=CC.ClientID
	INNER JOIN tblFile F ON CC.CaseID=F.CaseID
	WHERE C.ClientIdentifier=@ClientIdentifier AND F.FileID=@FileID

	SELECT @IsValid

END
GO
USE [master]
GO
ALTER DATABASE [AcmeCase] SET  READ_WRITE 
GO
