USE [master]
GO
/****** Object:  Database [telefonie]    Script Date: 04/08/2014 11:23:54 ******/
CREATE DATABASE [telefonie] ON  PRIMARY 
( NAME = N'comunicatii', FILENAME = N'c:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\telefonie.mdf' , SIZE = 2048KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'comunicatii_log', FILENAME = N'c:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\telefonie_1.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [telefonie] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [telefonie].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [telefonie] SET ANSI_NULL_DEFAULT OFF
GO
ALTER DATABASE [telefonie] SET ANSI_NULLS OFF
GO
ALTER DATABASE [telefonie] SET ANSI_PADDING OFF
GO
ALTER DATABASE [telefonie] SET ANSI_WARNINGS OFF
GO
ALTER DATABASE [telefonie] SET ARITHABORT OFF
GO
ALTER DATABASE [telefonie] SET AUTO_CLOSE OFF
GO
ALTER DATABASE [telefonie] SET AUTO_CREATE_STATISTICS ON
GO
ALTER DATABASE [telefonie] SET AUTO_SHRINK OFF
GO
ALTER DATABASE [telefonie] SET AUTO_UPDATE_STATISTICS ON
GO
ALTER DATABASE [telefonie] SET CURSOR_CLOSE_ON_COMMIT OFF
GO
ALTER DATABASE [telefonie] SET CURSOR_DEFAULT  GLOBAL
GO
ALTER DATABASE [telefonie] SET CONCAT_NULL_YIELDS_NULL OFF
GO
ALTER DATABASE [telefonie] SET NUMERIC_ROUNDABORT OFF
GO
ALTER DATABASE [telefonie] SET QUOTED_IDENTIFIER OFF
GO
ALTER DATABASE [telefonie] SET RECURSIVE_TRIGGERS OFF
GO
ALTER DATABASE [telefonie] SET  DISABLE_BROKER
GO
ALTER DATABASE [telefonie] SET AUTO_UPDATE_STATISTICS_ASYNC OFF
GO
ALTER DATABASE [telefonie] SET DATE_CORRELATION_OPTIMIZATION OFF
GO
ALTER DATABASE [telefonie] SET TRUSTWORTHY OFF
GO
ALTER DATABASE [telefonie] SET ALLOW_SNAPSHOT_ISOLATION OFF
GO
ALTER DATABASE [telefonie] SET PARAMETERIZATION SIMPLE
GO
ALTER DATABASE [telefonie] SET READ_COMMITTED_SNAPSHOT OFF
GO
ALTER DATABASE [telefonie] SET HONOR_BROKER_PRIORITY OFF
GO
ALTER DATABASE [telefonie] SET  READ_WRITE
GO
ALTER DATABASE [telefonie] SET RECOVERY SIMPLE
GO
ALTER DATABASE [telefonie] SET  MULTI_USER
GO
ALTER DATABASE [telefonie] SET PAGE_VERIFY CHECKSUM
GO
ALTER DATABASE [telefonie] SET DB_CHAINING OFF
GO
USE [telefonie]
GO
/****** Object:  Table [dbo].[judete]    Script Date: 04/08/2014 11:23:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[judete](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[denumire] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_judete] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[judete] ON
INSERT [dbo].[judete] ([id], [denumire]) VALUES (1, N'Bucuresti')
INSERT [dbo].[judete] ([id], [denumire]) VALUES (2, N'Alba')
INSERT [dbo].[judete] ([id], [denumire]) VALUES (3, N'Bacau')
INSERT [dbo].[judete] ([id], [denumire]) VALUES (4, N'Neamt')
INSERT [dbo].[judete] ([id], [denumire]) VALUES (5, N'Iasi')
INSERT [dbo].[judete] ([id], [denumire]) VALUES (6, N'Bihor')
INSERT [dbo].[judete] ([id], [denumire]) VALUES (7, N'Cluj')
INSERT [dbo].[judete] ([id], [denumire]) VALUES (8, N'Salaj')
INSERT [dbo].[judete] ([id], [denumire]) VALUES (9, N'Suceava')
INSERT [dbo].[judete] ([id], [denumire]) VALUES (10, N'Maramures')
INSERT [dbo].[judete] ([id], [denumire]) VALUES (11, N'Satu Mare')
INSERT [dbo].[judete] ([id], [denumire]) VALUES (12, N'Botosani')
INSERT [dbo].[judete] ([id], [denumire]) VALUES (13, N'Vaslui')
INSERT [dbo].[judete] ([id], [denumire]) VALUES (14, N'Harghita')
INSERT [dbo].[judete] ([id], [denumire]) VALUES (15, N'Covasna')
INSERT [dbo].[judete] ([id], [denumire]) VALUES (16, N'Brasov')
INSERT [dbo].[judete] ([id], [denumire]) VALUES (17, N'Mures')
INSERT [dbo].[judete] ([id], [denumire]) VALUES (18, N'Salaj')
INSERT [dbo].[judete] ([id], [denumire]) VALUES (19, N'Bistrita-Nasaud')
INSERT [dbo].[judete] ([id], [denumire]) VALUES (20, N'Salaj')
INSERT [dbo].[judete] ([id], [denumire]) VALUES (21, N'Arad')
INSERT [dbo].[judete] ([id], [denumire]) VALUES (22, N'Hunedoara')
INSERT [dbo].[judete] ([id], [denumire]) VALUES (23, N'Galati')
INSERT [dbo].[judete] ([id], [denumire]) VALUES (24, N'Vrancea')
INSERT [dbo].[judete] ([id], [denumire]) VALUES (25, N'Timis')
INSERT [dbo].[judete] ([id], [denumire]) VALUES (26, N'Tulcea')
INSERT [dbo].[judete] ([id], [denumire]) VALUES (27, N'Braila')
INSERT [dbo].[judete] ([id], [denumire]) VALUES (28, N'Arges')
INSERT [dbo].[judete] ([id], [denumire]) VALUES (29, N'Buzau')
INSERT [dbo].[judete] ([id], [denumire]) VALUES (30, N'Prahova')
INSERT [dbo].[judete] ([id], [denumire]) VALUES (31, N'Dambovita')
INSERT [dbo].[judete] ([id], [denumire]) VALUES (32, N'Ilfov')
INSERT [dbo].[judete] ([id], [denumire]) VALUES (33, N'Constanta')
INSERT [dbo].[judete] ([id], [denumire]) VALUES (34, N'Giurgiu')
INSERT [dbo].[judete] ([id], [denumire]) VALUES (35, N'Olt')
INSERT [dbo].[judete] ([id], [denumire]) VALUES (36, N'Mehedinti')
INSERT [dbo].[judete] ([id], [denumire]) VALUES (37, N'Gorj')
INSERT [dbo].[judete] ([id], [denumire]) VALUES (38, N'Dolj')
INSERT [dbo].[judete] ([id], [denumire]) VALUES (39, N'Calarasi')
INSERT [dbo].[judete] ([id], [denumire]) VALUES (40, N'Ialomita')
SET IDENTITY_INSERT [dbo].[judete] OFF
/****** Object:  Table [dbo].[echipament_versiuni]    Script Date: 04/08/2014 11:23:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[echipament_versiuni](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[denumire] [nvarchar](50) NULL,
 CONSTRAINT [PK_echipament_versiuni] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[echipament_versiuni] ON
INSERT [dbo].[echipament_versiuni] ([id], [denumire]) VALUES (1, N'VER1')
INSERT [dbo].[echipament_versiuni] ([id], [denumire]) VALUES (2, N'VER2')
INSERT [dbo].[echipament_versiuni] ([id], [denumire]) VALUES (3, N'VER3')
INSERT [dbo].[echipament_versiuni] ([id], [denumire]) VALUES (4, N'VER4')
SET IDENTITY_INSERT [dbo].[echipament_versiuni] OFF
/****** Object:  Table [dbo].[echipament_tip]    Script Date: 04/08/2014 11:23:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[echipament_tip](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[denumire] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_echipament_tip] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[echipament_tip] ON
INSERT [dbo].[echipament_tip] ([id], [denumire]) VALUES (1, N'MX-ONE')
INSERT [dbo].[echipament_tip] ([id], [denumire]) VALUES (2, N'MD')
INSERT [dbo].[echipament_tip] ([id], [denumire]) VALUES (3, N'SIEMENS')
INSERT [dbo].[echipament_tip] ([id], [denumire]) VALUES (4, N'ALCATEL')
INSERT [dbo].[echipament_tip] ([id], [denumire]) VALUES (5, N'PANASONIC')
SET IDENTITY_INSERT [dbo].[echipament_tip] OFF
/****** Object:  Table [dbo].[tip_echipament]    Script Date: 04/08/2014 11:23:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tip_echipament](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[denumire] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_tip_echipament] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[tip_echipament] ON
INSERT [dbo].[tip_echipament] ([id], [denumire]) VALUES (1, N'PABX')
INSERT [dbo].[tip_echipament] ([id], [denumire]) VALUES (2, N'MC')
INSERT [dbo].[tip_echipament] ([id], [denumire]) VALUES (3, N'MSED')
INSERT [dbo].[tip_echipament] ([id], [denumire]) VALUES (4, N'Concentratoare')
INSERT [dbo].[tip_echipament] ([id], [denumire]) VALUES (5, N'Routere')
INSERT [dbo].[tip_echipament] ([id], [denumire]) VALUES (6, N'Switch-uri')
SET IDENTITY_INSERT [dbo].[tip_echipament] OFF
/****** Object:  Table [dbo].[tip_cartela]    Script Date: 04/08/2014 11:23:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tip_cartela](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[denumire] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_tip_cartela] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[tip_cartela] ON
INSERT [dbo].[tip_cartela] ([id], [denumire]) VALUES (1, N'ESU')
INSERT [dbo].[tip_cartela] ([id], [denumire]) VALUES (2, N'ASU')
INSERT [dbo].[tip_cartela] ([id], [denumire]) VALUES (3, N'NIU')
INSERT [dbo].[tip_cartela] ([id], [denumire]) VALUES (4, N'AAU2')
INSERT [dbo].[tip_cartela] ([id], [denumire]) VALUES (5, N'IPLU')
INSERT [dbo].[tip_cartela] ([id], [denumire]) VALUES (6, N'ELU28')
INSERT [dbo].[tip_cartela] ([id], [denumire]) VALUES (7, N'ELU29')
INSERT [dbo].[tip_cartela] ([id], [denumire]) VALUES (8, N'TLU75')
INSERT [dbo].[tip_cartela] ([id], [denumire]) VALUES (9, N'ELU33')
INSERT [dbo].[tip_cartela] ([id], [denumire]) VALUES (10, N'TLU34')
INSERT [dbo].[tip_cartela] ([id], [denumire]) VALUES (11, N'TLU83')
INSERT [dbo].[tip_cartela] ([id], [denumire]) VALUES (12, N'Management alimentare')
SET IDENTITY_INSERT [dbo].[tip_cartela] OFF
/****** Object:  Table [dbo].[tip_atribut]    Script Date: 04/08/2014 11:23:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tip_atribut](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[denumire] [nvarchar](max) NOT NULL,
	[tip_valoare] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_tip_atribut] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[tip_atribut] ON
INSERT [dbo].[tip_atribut] ([id], [denumire], [tip_valoare]) VALUES (2, N'Tip', N'I')
INSERT [dbo].[tip_atribut] ([id], [denumire], [tip_valoare]) VALUES (3, N'Versiune', N'I')
INSERT [dbo].[tip_atribut] ([id], [denumire], [tip_valoare]) VALUES (4, N'IP', N'S')
INSERT [dbo].[tip_atribut] ([id], [denumire], [tip_valoare]) VALUES (6, N'MASK', N'S')
INSERT [dbo].[tip_atribut] ([id], [denumire], [tip_valoare]) VALUES (9, N'GATEWAY', N'S')
INSERT [dbo].[tip_atribut] ([id], [denumire], [tip_valoare]) VALUES (10, N'BPOS', N'S')
INSERT [dbo].[tip_atribut] ([id], [denumire], [tip_valoare]) VALUES (11, N'Abonati analogici', N'CSV')
INSERT [dbo].[tip_atribut] ([id], [denumire], [tip_valoare]) VALUES (13, N'Abonati digitali', N'CSV')
INSERT [dbo].[tip_atribut] ([id], [denumire], [tip_valoare]) VALUES (15, N'Abonati IP', N'CSV')
INSERT [dbo].[tip_atribut] ([id], [denumire], [tip_valoare]) VALUES (17, N'Abonati DECT', N'CSV')
INSERT [dbo].[tip_atribut] ([id], [denumire], [tip_valoare]) VALUES (19, N'Abonati total', N'CSV')
INSERT [dbo].[tip_atribut] ([id], [denumire], [tip_valoare]) VALUES (21, N'IP CES', N'S')
INSERT [dbo].[tip_atribut] ([id], [denumire], [tip_valoare]) VALUES (24, N'IP CES Remote', N'S')
INSERT [dbo].[tip_atribut] ([id], [denumire], [tip_valoare]) VALUES (25, N'IP Management', N'S')
INSERT [dbo].[tip_atribut] ([id], [denumire], [tip_valoare]) VALUES (26, N'IP Management Remote', N'S')
INSERT [dbo].[tip_atribut] ([id], [denumire], [tip_valoare]) VALUES (27, N'Locatie remote', N'S')
INSERT [dbo].[tip_atribut] ([id], [denumire], [tip_valoare]) VALUES (28, N'Licenta', N'S')
INSERT [dbo].[tip_atribut] ([id], [denumire], [tip_valoare]) VALUES (29, N'Numar ruta', N'S')
INSERT [dbo].[tip_atribut] ([id], [denumire], [tip_valoare]) VALUES (30, N'Tip Conexiune', N'S')
INSERT [dbo].[tip_atribut] ([id], [denumire], [tip_valoare]) VALUES (31, N'Destinatie', N'S')
INSERT [dbo].[tip_atribut] ([id], [denumire], [tip_valoare]) VALUES (32, N'Port', N'S')
INSERT [dbo].[tip_atribut] ([id], [denumire], [tip_valoare]) VALUES (33, N'Contract', N'S')
INSERT [dbo].[tip_atribut] ([id], [denumire], [tip_valoare]) VALUES (34, N'LIM MAIN', N'S')
INSERT [dbo].[tip_atribut] ([id], [denumire], [tip_valoare]) VALUES (35, N'LIM DISTANT', N'S')
SET IDENTITY_INSERT [dbo].[tip_atribut] OFF
/****** Object:  Table [dbo].[atribut]    Script Date: 04/08/2014 11:23:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[atribut](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[tipID] [bigint] NOT NULL,
	[val_int] [int] NULL,
	[val_string] [nvarchar](max) NULL,
	[val_nr] [decimal](18, 8) NULL,
	[val_csv] [nvarchar](max) NULL,
 CONSTRAINT [PK_atribut] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[contact]    Script Date: 04/08/2014 11:23:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[contact](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[username] [varchar](50) NULL,
	[email] [varchar](50) NULL,
	[password] [varchar](50) NULL,
 CONSTRAINT [PK_contact] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[echipament]    Script Date: 04/08/2014 11:23:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[echipament](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[denumire] [nvarchar](max) NULL,
	[tipID] [int] NOT NULL,
	[siteID] [bigint] NULL,
 CONSTRAINT [PK_echipament] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[site]    Script Date: 04/08/2014 11:23:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[site](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[judetID] [int] NOT NULL,
	[denumire] [nvarchar](max) NOT NULL,
	[siteParinteID] [bigint] NOT NULL,
 CONSTRAINT [PK_site] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[site] ON
INSERT [dbo].[site] ([id], [judetID], [denumire], [siteParinteID]) VALUES (15, 2, N'ABB', 0)
INSERT [dbo].[site] ([id], [judetID], [denumire], [siteParinteID]) VALUES (16, 2, N'ABIP', 0)
INSERT [dbo].[site] ([id], [judetID], [denumire], [siteParinteID]) VALUES (17, 2, N'ABB1', 0)
INSERT [dbo].[site] ([id], [judetID], [denumire], [siteParinteID]) VALUES (18, 2, N'ABAN', 0)
INSERT [dbo].[site] ([id], [judetID], [denumire], [siteParinteID]) VALUES (19, 2, N'ABB2', 0)
INSERT [dbo].[site] ([id], [judetID], [denumire], [siteParinteID]) VALUES (20, 2, N'ABC', 0)
INSERT [dbo].[site] ([id], [judetID], [denumire], [siteParinteID]) VALUES (21, 2, N'ABE', 0)
INSERT [dbo].[site] ([id], [judetID], [denumire], [siteParinteID]) VALUES (22, 2, N'ABF', 0)
SET IDENTITY_INSERT [dbo].[site] OFF
/****** Object:  Table [dbo].[echipament_atribute]    Script Date: 04/08/2014 11:23:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[echipament_atribute](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[echipamentID] [bigint] NOT NULL,
	[atributID] [bigint] NOT NULL,
 CONSTRAINT [PK_echipament_atribute] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[cartela]    Script Date: 04/08/2014 11:23:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cartela](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[tipID] [bigint] NOT NULL,
	[echipamentID] [bigint] NOT NULL,
 CONSTRAINT [PK_cartela] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[cartela_atribute]    Script Date: 04/08/2014 11:23:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cartela_atribute](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[cartelaID] [bigint] NOT NULL,
	[atributID] [bigint] NOT NULL,
 CONSTRAINT [PK_cartela_atribute] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  ForeignKey [FK_echipament_tip_echipament]    Script Date: 04/08/2014 11:23:55 ******/
ALTER TABLE [dbo].[echipament]  WITH CHECK ADD  CONSTRAINT [FK_echipament_tip_echipament] FOREIGN KEY([tipID])
REFERENCES [dbo].[tip_echipament] ([id])
GO
ALTER TABLE [dbo].[echipament] CHECK CONSTRAINT [FK_echipament_tip_echipament]
GO
/****** Object:  ForeignKey [FK__site__judetID__09DE7BCC]    Script Date: 04/08/2014 11:23:55 ******/
ALTER TABLE [dbo].[site]  WITH CHECK ADD FOREIGN KEY([judetID])
REFERENCES [dbo].[judete] ([id])
GO
/****** Object:  ForeignKey [FK_echipament_atribute_atribut]    Script Date: 04/08/2014 11:23:55 ******/
ALTER TABLE [dbo].[echipament_atribute]  WITH CHECK ADD  CONSTRAINT [FK_echipament_atribute_atribut] FOREIGN KEY([atributID])
REFERENCES [dbo].[atribut] ([id])
GO
ALTER TABLE [dbo].[echipament_atribute] CHECK CONSTRAINT [FK_echipament_atribute_atribut]
GO
/****** Object:  ForeignKey [FK_echipament_atribute_echipament]    Script Date: 04/08/2014 11:23:55 ******/
ALTER TABLE [dbo].[echipament_atribute]  WITH CHECK ADD  CONSTRAINT [FK_echipament_atribute_echipament] FOREIGN KEY([echipamentID])
REFERENCES [dbo].[echipament] ([id])
GO
ALTER TABLE [dbo].[echipament_atribute] CHECK CONSTRAINT [FK_echipament_atribute_echipament]
GO
/****** Object:  ForeignKey [FK_cartela_echipament]    Script Date: 04/08/2014 11:23:55 ******/
ALTER TABLE [dbo].[cartela]  WITH CHECK ADD  CONSTRAINT [FK_cartela_echipament] FOREIGN KEY([echipamentID])
REFERENCES [dbo].[echipament] ([id])
GO
ALTER TABLE [dbo].[cartela] CHECK CONSTRAINT [FK_cartela_echipament]
GO
/****** Object:  ForeignKey [FK_cartela_tip_cartela]    Script Date: 04/08/2014 11:23:55 ******/
ALTER TABLE [dbo].[cartela]  WITH CHECK ADD  CONSTRAINT [FK_cartela_tip_cartela] FOREIGN KEY([tipID])
REFERENCES [dbo].[tip_cartela] ([id])
GO
ALTER TABLE [dbo].[cartela] CHECK CONSTRAINT [FK_cartela_tip_cartela]
GO
/****** Object:  ForeignKey [FK_cartela_atribute]    Script Date: 04/08/2014 11:23:55 ******/
ALTER TABLE [dbo].[cartela_atribute]  WITH CHECK ADD  CONSTRAINT [FK_cartela_atribute] FOREIGN KEY([cartelaID])
REFERENCES [dbo].[cartela] ([id])
GO
ALTER TABLE [dbo].[cartela_atribute] CHECK CONSTRAINT [FK_cartela_atribute]
GO
/****** Object:  ForeignKey [FK_cartela_atribute_atribut]    Script Date: 04/08/2014 11:23:55 ******/
ALTER TABLE [dbo].[cartela_atribute]  WITH CHECK ADD  CONSTRAINT [FK_cartela_atribute_atribut] FOREIGN KEY([atributID])
REFERENCES [dbo].[atribut] ([id])
GO
ALTER TABLE [dbo].[cartela_atribute] CHECK CONSTRAINT [FK_cartela_atribute_atribut]
GO

create procedure EmptyDb as
begin
delete from cartela_atribute where id > 0
delete from cartela where id > 0
delete from echipament_atribute where id > 0
delete from echipament where id > 0
delete from atribut where id > 0
end
go
