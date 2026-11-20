/*
    SGEI - Sistema de tickets (menú Tickets).
    Crea las tablas ticket, ticket_comentario y ticket_historial. Se puede correr más de una vez: solo crea lo que falta.

    ticket.estado     1 Pendiente · 2 En curso · 3 Bloqueado · 4 Terminado
    ticket.prioridad  1 Baja · 2 Media · 3 Alta · 4 Urgente
    ticket.tipo_equipo / id_equipo: equipo opcional, con la misma clave que usan los incidentes de ese tipo
        (computadora: pc.id_pc · impresora: ubicacion_impresora.id · telefono: detalle_tel.id_detalle ·
         camara: camara.id_camara · switch: swicth.id_switch · escaner: escaner.id_escaner)
    ticket_historial: quién cambió qué y cuándo (alta, estado, asignado, prioridad, fecha prevista, título, equipo,
        descripción). Guarda los valores como texto para mostrarlos tal como estaban.
*/

IF OBJECT_ID(N'dbo.ticket', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.ticket (
        id_ticket       int IDENTITY(1,1) NOT NULL CONSTRAINT PK_ticket PRIMARY KEY,
        titulo          nvarchar(150)  NOT NULL,
        descripcion     nvarchar(max)  NULL,
        prioridad       tinyint        NOT NULL CONSTRAINT CK_ticket_prioridad CHECK (prioridad BETWEEN 1 AND 4),
        estado          tinyint        NOT NULL CONSTRAINT CK_ticket_estado CHECK (estado BETWEEN 1 AND 4),
        asignado_id     nvarchar(128)  NOT NULL CONSTRAINT FK_ticket_asignado REFERENCES dbo.AspNetUsers (Id),
        creado_por_id   nvarchar(128)  NOT NULL CONSTRAINT FK_ticket_creado_por REFERENCES dbo.AspNetUsers (Id),
        fecha_alta      datetime       NOT NULL,
        fecha_prevista  date           NOT NULL,
        fecha_estado    datetime       NOT NULL,
        fecha_fin       datetime       NULL,
        tipo_equipo     varchar(20)    NULL,
        id_equipo       int            NULL
    );
    CREATE INDEX IX_ticket_estado ON dbo.ticket (estado, fecha_fin);
    CREATE INDEX IX_ticket_asignado ON dbo.ticket (asignado_id);
END
GO

IF OBJECT_ID(N'dbo.ticket_comentario', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.ticket_comentario (
        id_comentario   int IDENTITY(1,1) NOT NULL CONSTRAINT PK_ticket_comentario PRIMARY KEY,
        id_ticket       int            NOT NULL CONSTRAINT FK_ticket_comentario_ticket REFERENCES dbo.ticket (id_ticket) ON DELETE CASCADE,
        usuario_id      nvarchar(128)  NOT NULL CONSTRAINT FK_ticket_comentario_usuario REFERENCES dbo.AspNetUsers (Id),
        texto           nvarchar(max)  NOT NULL,
        fecha           datetime       NOT NULL,
        fecha_edicion   datetime       NULL
    );
    CREATE INDEX IX_ticket_comentario_ticket ON dbo.ticket_comentario (id_ticket);
END
GO

IF OBJECT_ID(N'dbo.ticket_historial', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.ticket_historial (
        id_historial    int IDENTITY(1,1) NOT NULL CONSTRAINT PK_ticket_historial PRIMARY KEY,
        id_ticket       int            NOT NULL CONSTRAINT FK_ticket_historial_ticket REFERENCES dbo.ticket (id_ticket) ON DELETE CASCADE,
        usuario_id      nvarchar(128)  NOT NULL CONSTRAINT FK_ticket_historial_usuario REFERENCES dbo.AspNetUsers (Id),
        fecha           datetime       NOT NULL,
        campo           varchar(20)    NOT NULL,
        valor_anterior  nvarchar(200)  NULL,
        valor_nuevo     nvarchar(200)  NULL
    );
    CREATE INDEX IX_ticket_historial_ticket ON dbo.ticket_historial (id_ticket);
END
GO
