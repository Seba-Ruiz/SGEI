/*
    SGEI - Notificaciones (campana de la barra superior y pantalla Notificaciones).
    Crea la tabla notificacion. Se puede correr más de una vez: solo crea lo que falta.

    Sirve para cualquier aviso del sistema: hoy la usan los tickets (asignación y cambio de estado).
    notificacion.tipo            código del aviso: TICKET_ASIGNADO, TICKET_ESTADO, ...
    notificacion.referencia_*    a qué se refiere (por ejemplo TICKET / 15), opcional.
    notificacion.url             a dónde lleva al abrirla (ruta dentro del sistema, por ejemplo /Tickets/Ver/15).
    notificacion.fecha           fecha y hora en que se generó.
    notificacion.fecha_lectura   fecha y hora en que la persona la marcó como leída (NULL = sin leer).
    notificacion.fecha_mail      fecha y hora en que se envió el mail (NULL = no se envió).
    notificacion.error_mail      motivo por el que no se pudo enviar el mail (la notificación queda igual).
*/

IF OBJECT_ID(N'dbo.notificacion', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.notificacion (
        id_notificacion   int IDENTITY(1,1) NOT NULL CONSTRAINT PK_notificacion PRIMARY KEY,
        usuario_id        nvarchar(128)  NOT NULL CONSTRAINT FK_notificacion_usuario REFERENCES dbo.AspNetUsers (Id),
        origen_usuario_id nvarchar(128)  NULL     CONSTRAINT FK_notificacion_origen REFERENCES dbo.AspNetUsers (Id),
        tipo              varchar(40)    NOT NULL,
        titulo            nvarchar(200)  NOT NULL,
        mensaje           nvarchar(1000) NULL,
        url               nvarchar(400)  NULL,
        referencia_tipo   varchar(20)    NULL,
        referencia_id     int            NULL,
        fecha             datetime       NOT NULL,
        fecha_lectura     datetime       NULL,
        fecha_mail        datetime       NULL,
        error_mail        nvarchar(500)  NULL
    );
    CREATE INDEX IX_notificacion_usuario ON dbo.notificacion (usuario_id, fecha_lectura, fecha DESC);
END
GO
