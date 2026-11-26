/*
    SGEI - Observaciones e historial de incidentes, e incidentes generados desde tickets.
    Se puede correr más de una vez: solo crea lo que falta.

    Los incidentes de cada tipo de equipo están en tablas distintas (incidente, pc_incidentepc, incidente_tel_tel,
    camara_incidente_camara, switch_incidente_switch, escaner_incidente_escaner). Estas tablas los identifican con
    tipo_equipo + id_incidente:
        tipo_equipo  computadora · impresora · telefono · camara · switch · escaner
        id_incidente id de la tabla de incidentes de ese tipo

    incidente_comentario: observaciones de los usuarios (HTML saneado). Cada uno edita o elimina las suyas.
    incidente_historial:  quién cambió qué y cuándo (alta, equipo, incidente, fechas, acción). Los valores se guardan
                          como texto para mostrarlos tal como estaban. Empieza a registrarse desde esta versión.
    ticket_incidente:     incidente que se generó automáticamente desde un ticket con equipo.
*/

IF OBJECT_ID(N'dbo.incidente_comentario', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.incidente_comentario (
        id_comentario   int IDENTITY(1,1) NOT NULL CONSTRAINT PK_incidente_comentario PRIMARY KEY,
        tipo_equipo     varchar(20)    NOT NULL,
        id_incidente    int            NOT NULL,
        usuario_id      nvarchar(128)  NOT NULL CONSTRAINT FK_incidente_comentario_usuario REFERENCES dbo.AspNetUsers (Id),
        texto           nvarchar(max)  NOT NULL,
        fecha           datetime       NOT NULL,
        fecha_edicion   datetime       NULL
    );
    CREATE INDEX IX_incidente_comentario_incidente ON dbo.incidente_comentario (tipo_equipo, id_incidente);
END
GO

IF OBJECT_ID(N'dbo.incidente_historial', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.incidente_historial (
        id_historial    int IDENTITY(1,1) NOT NULL CONSTRAINT PK_incidente_historial PRIMARY KEY,
        tipo_equipo     varchar(20)    NOT NULL,
        id_incidente    int            NOT NULL,
        usuario_id      nvarchar(128)  NOT NULL CONSTRAINT FK_incidente_historial_usuario REFERENCES dbo.AspNetUsers (Id),
        fecha           datetime       NOT NULL,
        campo           varchar(20)    NOT NULL,
        valor_anterior  nvarchar(200)  NULL,
        valor_nuevo     nvarchar(200)  NULL
    );
    CREATE INDEX IX_incidente_historial_incidente ON dbo.incidente_historial (tipo_equipo, id_incidente);
END
GO

IF OBJECT_ID(N'dbo.ticket_incidente', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.ticket_incidente (
        id_ticket       int            NOT NULL CONSTRAINT FK_ticket_incidente_ticket REFERENCES dbo.ticket (id_ticket) ON DELETE CASCADE,
        tipo_equipo     varchar(20)    NOT NULL,
        id_incidente    int            NOT NULL,
        fecha           datetime       NOT NULL,
        CONSTRAINT PK_ticket_incidente PRIMARY KEY (id_ticket, tipo_equipo, id_incidente)
    );
    CREATE INDEX IX_ticket_incidente_incidente ON dbo.ticket_incidente (tipo_equipo, id_incidente);
END
GO
