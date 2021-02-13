SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LinkedinCandidates](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Url] [nvarchar](200) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Location] [nvarchar](max) NULL,
	[AboutMe] [nvarchar](max) NULL,
	[Skills] [nvarchar](max) NULL,
	[Email] [nvarchar](100) NULL,
	[Phone] [nvarchar](50) NULL,
	[Photo] [nvarchar](max) NULL,
	[Address] [nvarchar](max) NULL,
	[Ims] [varchar](200) NULL,
	[Birthday] [varchar](200) NULL,
	[Connected] [varchar](200) NULL,
	[IsExistContactInfo] [bit] NOT NULL,
	[IsExistEducation] [bit] NOT NULL,
	[IsExistExperience] [bit] NOT NULL,
	[IsExistCertificates] [bit] NOT NULL,
	[IsFirstRound] [bit] NOT NULL,
	[IsNotStorage] [bit] NOT NULL,
	[UrlComparison] [nvarchar](500) NULL,
 CONSTRAINT [PK_LinkedinCandidates] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[LinkedinCandidates] ADD  CONSTRAINT [DF__LinkedinC__IsExi__0D44F85C]  DEFAULT ((0)) FOR [IsExistContactInfo]
GO

ALTER TABLE [dbo].[LinkedinCandidates] ADD  CONSTRAINT [DF__LinkedinC__IsExi__0E391C95]  DEFAULT ((0)) FOR [IsExistEducation]
GO

ALTER TABLE [dbo].[LinkedinCandidates] ADD  CONSTRAINT [DF__LinkedinC__IsExi__0F2D40CE]  DEFAULT ((0)) FOR [IsExistExperience]
GO

ALTER TABLE [dbo].[LinkedinCandidates] ADD  CONSTRAINT [DF__LinkedinC__IsExi__10216507]  DEFAULT ((0)) FOR [IsExistCertificates]
GO

ALTER TABLE [dbo].[LinkedinCandidates] ADD  CONSTRAINT [DF__LinkedinC__IsFir__11158940]  DEFAULT ((0)) FOR [IsFirstRound]
GO

ALTER TABLE [dbo].[LinkedinCandidates] ADD  CONSTRAINT [DF__LinkedinC__IsNot__1209AD79]  DEFAULT ((0)) FOR [IsNotStorage]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LinkedinCandidatesWebSites](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[CandidateId] [int] NOT NULL,
	[WebSiteName] [nvarchar](max) NULL,
	[WebSiteUrl] [nvarchar](max) NULL,
	[IsMessenger] [bit] NULL,
	[WebSiteUrlComparison] [nvarchar](500) NULL,
 CONSTRAINT [PK_LinkedinCandidatesWebSites] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[LinkedinCandidatesWebSites]  WITH CHECK ADD  CONSTRAINT [FK_LinkedinCandidatesWebSites_LinkedinCandidates] FOREIGN KEY([CandidateId])
REFERENCES [dbo].[LinkedinCandidates] ([Id])
GO

ALTER TABLE [dbo].[LinkedinCandidatesWebSites] CHECK CONSTRAINT [FK_LinkedinCandidatesWebSites_LinkedinCandidates]
GO

