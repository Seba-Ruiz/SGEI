SGEI

Manso sistema!

## Requisitos

- .NET Framework 4.8 (Developer Pack para compilar, runtime 4.8 en el servidor IIS)
- Visual Studio 2022 (o MSBuild 17+)

## Configuración local

1. Copiar `login v6/connectionStrings.example.config` como `login v6/connectionStrings.config`
   y completar servidor, base, usuario y contraseña.
   Copiar `login v6/appSettings.secret.example.config` como `login v6/appSettings.secret.config`
   y completar la cuenta SMTP (para Gmail, una contraseña de aplicación).
   Ambos archivos están en `.gitignore`: **nunca** se commitean.
2. Restaurar paquetes NuGet (Visual Studio lo hace solo al compilar, o por consola):

   ```
   msbuild "login v6.sln" -t:restore -p:RestorePackagesConfig=true
   ```

3. Compilar y ejecutar el proyecto `SGEI` (carpeta `login v6`).

## Publicación en IIS

El `Web.config` publicado toma las cadenas de conexión de `connectionStrings.config`
y los datos SMTP de `appSettings.secret.config`, que no se publican automáticamente.
En el servidor tienen que existir ambos junto al `Web.config` (se copian una sola vez;
la publicación no los borra porque el perfil usa `DeleteExistingFiles=False`).

## Seguridad y permisos

- Todas las pantallas requieren usuario logueado (filtro global `[Authorize]`).
- Roles (`AspNetRoles`): Id `1` = Administrador, Id `2` = Operario. Vale el rol más
  reciente de `AspNetUserRoles`.
- Solo Administrador: ABM de catálogos (`ABMController`), usuarios, roles y alta de usuarios
  nuevos. Usar `[SoloAdministrador]` en controladores/acciones y `User.EsAdministrador()` en vistas.
- Todo POST exige token antiforgery: en formularios `@Html.AntiForgeryToken()`; en AJAX el
  layout agrega el header automáticamente.
- Para borrar, usar `@Html.BotonEliminar(url, mensaje)` (form POST), nunca un link GET.

## Ramas

Cada funcionalidad nueva va en su rama `feature/claude-<nombreFuncionalidad>`.
