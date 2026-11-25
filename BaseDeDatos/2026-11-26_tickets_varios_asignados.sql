/*
    SGEI - Tickets con varias personas asignadas.
    Crea ticket_asignado (un registro por ticket y persona), pasa ahí el asignado que tenía cada ticket y quita la
    columna ticket.asignado_id. Se puede correr más de una vez: solo hace lo que falta.

    Hay que correrlo ANTES de publicar la versión que lo usa.
*/

IF OBJECT_ID(N'dbo.ticket_asignado', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.ticket_asignado (
        id_ticket         int            NOT NULL CONSTRAINT FK_ticket_asignado_ticket REFERENCES dbo.ticket (id_ticket) ON DELETE CASCADE,
        usuario_id        nvarchar(128)  NOT NULL CONSTRAINT FK_ticket_asignado_usuario REFERENCES dbo.AspNetUsers (Id),
        fecha_asignacion  datetime       NOT NULL,
        CONSTRAINT PK_ticket_asignado PRIMARY KEY (id_ticket, usuario_id)
    );
    CREATE INDEX IX_ticket_asignado_usuario ON dbo.ticket_asignado (usuario_id);
END
GO

-- Pasa el asignado de cada ticket a la tabla nueva y quita la columna vieja.
IF COL_LENGTH(N'dbo.ticket', N'asignado_id') IS NOT NULL
BEGIN
    EXEC (N'
        INSERT INTO dbo.ticket_asignado (id_ticket, usuario_id, fecha_asignacion)
        SELECT t.id_ticket, t.asignado_id, t.fecha_alta
        FROM dbo.ticket t
        WHERE t.asignado_id IS NOT NULL
          AND NOT EXISTS (SELECT 1 FROM dbo.ticket_asignado a WHERE a.id_ticket = t.id_ticket AND a.usuario_id = t.asignado_id);');

    IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_ticket_asignado' AND object_id = OBJECT_ID(N'dbo.ticket'))
        DROP INDEX IX_ticket_asignado ON dbo.ticket;
    IF OBJECT_ID(N'dbo.FK_ticket_asignado', N'F') IS NOT NULL
        ALTER TABLE dbo.ticket DROP CONSTRAINT FK_ticket_asignado;
    ALTER TABLE dbo.ticket DROP COLUMN asignado_id;
END
GO
