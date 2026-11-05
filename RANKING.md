# Ranking del juego

## Contrato

La API mantiene las rutas existentes y añade:

- `GET /api/laboratorio/ranking?limite=50`: público, ordenado solo por puntuación descendente; `limite` es opcional, entre 1 y 100. Si hay empate se usa el identificador para dar un orden estable, sin considerar el tiempo. Actualmente cada partida genera un registro: no agrupa jugadores ni elimina duplicados.
- `POST /api/administracion/ranking`: registra un resultado con el encabezado `X-API-Key` de administrador.
- `GET /api/administracion/ranking/{id}`: consulta pública del resultado por identificador.
- `DELETE /api/administracion/ranking/{id}`: elimina un resultado con la clave de administrador.

Ejemplo de cuerpo JSON para POST:

```json
{
  "playerName": "Jugador 1",
  "score": 1500,
  "time": 83.5,
  "characterId": 1,
  "date": "2026-11-05T12:00:00Z",
  "uploaded": false
}
```

Todos los campos son obligatorios. `playerName` acepta hasta 60 caracteres; `score` es un entero no negativo; `time` son segundos no negativos; `characterId` es un entero positivo, sin relación foránea obligatoria; `date` es texto de hasta 64 caracteres (se recomienda ISO 8601 en UTC); y `uploaded` es un booleano que se guarda tal como se recibe. El servidor no interpreta `date` ni `uploaded` como pruebas de autenticidad o confirmación de sincronización. El servidor asigna un `id` numérico.

Una creación correcta devuelve `201` y el objeto guardado. Si el juego mantiene una copia local, debe marcarla como sincronizada únicamente después de recibir la respuesta exitosa. El esquema actual no incluye un identificador de partida para detectar reintentos duplicados: si se repite el POST, puede crear dos registros. El campo `uploaded` no evita duplicados.

## Seguridad del juego

Solo el servidor de juego o un backend confiable debe conocer `Administracion__ApiKey` y enviar `X-API-Key` por HTTPS. No poner la clave en el ejecutable distribuido, la PWA, código fuente público ni en la app móvil. Una clave dentro de un cliente se puede extraer y permitiría modificar todos los recursos administrativos.

Si el juego es solo un ejecutable en el dispositivo del jugador y envía directamente su puntuación, no existe una manera de garantizar que la puntuación no fue manipulada. La protección real requiere un servidor confiable que autentique al jugador y calcule o valide el resultado a partir de datos verificables antes de enviarlo a la API. La implementación actual es una recepción para servidores confiables, no un sistema antitrampas.

## Límite de solicitudes

Cada instancia aplica una ventana global de 300 solicitudes por minuto entre todos los clientes y además hasta 30 escrituras de ranking por minuto entre todos. Al superar el límite devuelve `429 Too Many Requests`, sin cola. Son límites compartidos, no por jugador; usuarios simultáneos pueden afectarse entre sí. El contador está en memoria, se reinicia con la instancia y no se comparte entre instancias. Esto reduce la carga, pero no garantiza protección contra ataques distribuidos: para ello se necesita protección en el borde y, si crece el servicio, límites distribuidos con identidad de cliente verificada. El juego debe usar retroceso entre reintentos y no reintentar indiscriminadamente.

## Base de datos

Para bases existentes creadas con `EnsureCreated`, ejecutar `Laboratorio4.API/Database/001_ranking.sql` usando una conexión directa a PostgreSQL. El archivo crea la tabla y el índice de forma idempotente; `EnsureCreated` no agrega tablas nuevas a una base existente. Probar cambios futuros primero en una rama de Neon y usar migraciones versionadas. Esta actualización no modifica las tablas existentes.
