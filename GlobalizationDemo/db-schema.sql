CREATE TABLE [cultures] (
  [code] varchar(8) NOT NULL
, [name] varchar(100) NOT NULL
, CONSTRAINT [sqlite_autoindex_cultures_1] PRIMARY KEY ([code])
);

CREATE TABLE [translations] (
  [id] bigint NOT NULL
, [culture_code] varchar(8) NOT NULL
, [key] varchar(100) NOT NULL
, [text] varchar(255) NOT NULL
, CONSTRAINT [sqlite_master_PK_translations] PRIMARY KEY ([id])
, CONSTRAINT [FK_translations_0_0] FOREIGN KEY ([culture_code]) REFERENCES [cultures] ([code]) ON DELETE NO ACTION ON UPDATE NO ACTION
);

INSERT INTO [cultures] ([code],[name])
VALUES ('en','English')
	,('en-US','English (United States)')
	,('es','Spanish')
	,('es-ES','Spanish (Spain)')
	,('pt','Portuguese')
	,('pt-BR','Portuguese (Brazil)');