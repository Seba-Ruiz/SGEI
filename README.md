SGEI

Manso sistema!

## Requisitos

- .NET Framework 4.8 (Developer Pack para compilar, runtime 4.8 en el servidor IIS)
- Visual Studio 2022 (o MSBuild 17+)

## Configuración local

1. Copiar `login v6/connectionStrings.example.config` como `login v6/connectionStrings.config`
   y completar servidor, base, usuario y contraseña.
   Ese archivo está en `.gitignore`: **nunca** se commitea.
2. Restaurar paquetes NuGet (Visual Studio lo hace solo al compilar, o por consola):

   ```
   msbuild "login v6.sln" -t:restore -p:RestorePackagesConfig=true
   ```

3. Compilar y ejecutar el proyecto `SGEI` (carpeta `login v6`).

## Publicación en IIS

El `Web.config` publicado toma las cadenas de conexión de `connectionStrings.config`,
que no se publica automáticamente. En el servidor tiene que existir
`connectionStrings.config` junto al `Web.config` (se copia una sola vez; la publicación
no lo borra porque el perfil usa `DeleteExistingFiles=False`).

## Ramas

Cada funcionalidad nueva va en su rama `feature/claude-<nombreFuncionalidad>`.
