# Operaciones CRUD de la API

Las rutas existentes `/api/laboratorio/*` siguen disponibles para las aplicaciones. Para administrar todos los registros, incluyendo sus identificadores, se añadieron estos recursos:

- `/api/administracion/historias`
- `/api/administracion/personajes`
- `/api/administracion/controles`
- `/api/administracion/creditos`
- `/api/administracion/creadores`

Cada recurso acepta `GET` para listar, `GET /{id}` para consultar uno, `POST` para crear, `PUT /{id}` para reemplazar sus campos y `DELETE /{id}` para eliminarlo. La ruta pública `/api/laboratorio/historia` devuelve una sola historia; `/api/laboratorio/creditos` combina el primer registro de créditos con los creadores. Las rutas de administración permiten consultar y gestionar cada registro por separado.

## Clave de administración

Las consultas GET son públicas. Para POST, PUT y DELETE configura una cadena secreta larga en `Administracion__ApiKey` como variable de entorno del servicio en Render. Nunca la agregues a GitHub ni la incluyas en la app móvil o la PWA, ya que cualquiera que extraiga la clave podría modificar la información.

En desarrollo se puede configurar temporalmente en PowerShell con `$env:Administracion__ApiKey = 'TU_CLAVE_SECRETA'` antes de iniciar la API. Si no existe la clave en el servidor, las operaciones de escritura responden 503. Si falta el encabezado `X-API-Key` o contiene una clave incorrecta, responden 401.

## Ejemplos en PowerShell

Sustituye la dirección y la clave por las tuyas. No publiques la clave ni compartas capturas que la muestren.

```powershell
$base = 'https://laboratorio4api.onrender.com/api/administracion'
$headers = @{ 'X-API-Key' = 'TU_CLAVE_SECRETA' }
$body = @{ tecla = 'Espacio'; accion = 'Saltar' } | ConvertTo-Json

# Crear. Guarda el id que devuelve para los siguientes pasos.
Invoke-RestMethod "$base/controles" -Method Post -Headers $headers -ContentType 'application/json' -Body $body

# Listar y consultar uno. Sustituye 1 por un id existente.
Invoke-RestMethod "$base/controles"
Invoke-RestMethod "$base/controles/1"

# Editar. Envía todos los campos requeridos.
$body = @{ tecla = 'E'; accion = 'Interactuar' } | ConvertTo-Json
Invoke-RestMethod "$base/controles/1" -Method Put -Headers $headers -ContentType 'application/json' -Body $body

# Eliminar. Esta acción borra el registro indicado.
Invoke-WebRequest "$base/controles/1" -Method Delete -Headers $headers
```

## Campos requeridos para POST y PUT

Los identificadores los genera la base de datos; no se incluyen al crear ni editar. Todos los campos de cada objeto son obligatorios y no pueden estar vacíos.

```json
// historias
{ "titulo": "Título", "subtitulo": "Subtítulo", "contenido": "Texto" }

// personajes
{ "nombre": "Nombre", "rol": "Protagonista", "genero": "Género", "vestimenta": "Vestimenta", "habilidad": "Habilidad", "biografia": "Biografía", "imagenUrl": "https://ejemplo.com/imagen.webp" }

// controles
{ "tecla": "Espacio", "accion": "Saltar" }

// creditos
{ "grupo": "Grupo", "descripcion": "Descripción" }

// creadores
{ "nombre": "Nombre" }
```

Los comentarios `//` solo identifican los ejemplos y no forman parte del JSON que se envía. Las imágenes se guardan externamente: `imagenUrl` es solo una dirección.

## Respuestas

- `200 OK`: consulta o edición exitosa.
- `201 Created`: registro creado; el cuerpo incluye su `id` y la cabecera `Location` apunta al registro.
- `204 No Content`: eliminación exitosa.
- `400 Bad Request`: faltan campos o el JSON es inválido.
- `401 Unauthorized`: la clave falta o es incorrecta.
- `404 Not Found`: el identificador no existe.
- `503 Service Unavailable`: no se ha configurado la clave de administración.

La clave compartida protege las operaciones básicas, pero no es un sistema completo de usuarios y permisos. Para una aplicación de producción que requiera varios administradores se necesitaría autenticación y autorización individual, además de considerar límites de solicitudes y auditoría.
