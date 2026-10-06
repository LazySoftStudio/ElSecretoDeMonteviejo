# LazySoft — Portfolio web

Portfolio de LazySoft y de nuestro videojuego ambientado en Monteviejo. Incluye la presentación del estudio, el proyecto, una galería de arte, el equipo y el contacto.

## Desarrollo

Requiere Node.js 22.13 o posterior.

```sh
npm ci
npm run dev
```

El servidor muestra la dirección local en la consola.

## Comprobaciones y compilación

```sh
npm run typecheck
npm run lint
npm run build
npm run preview
```

La compilación genera `dist/`, lista para un alojamiento de archivos estáticos. No necesita servidor de aplicaciones, cuentas de servicios externos ni claves de API. La instalación de dependencias requiere conexión a Internet.

## Organización

- `app/page.tsx`: secciones del portfolio.
- `app/globals.css`: estilos, diseño adaptable y movimiento reducido.
- `components/`: navegación, portada, recorrido del juego, galería y créditos.
- `components/ui/`: componentes de interfaz reutilizables.
- `content/site.ts`: título, sinopsis, equipo, correo y colección de arte.
- `public/assets/`: logotipos, bocetos y estudios de volumen.
- `index.html` y `main.tsx`: entrada de la aplicación.

## Actualizar contenido

Para añadir una pieza a la galería, guarda su imagen en `public/assets/` y añade una entrada a `artworks` en `content/site.ts` con identificador, título, categoría, tipo, ruta, descripción y texto alternativo. Indica si es un boceto, un estudio de volumen o una captura jugable.

El nombre del juego sigue siendo provisional. Cuando se confirme, actualiza `site.game.title` y `site.game.provisionalTitle`. Los roles del equipo todavía no están repartidos.

El contacto utiliza un enlace de correo. La galería permite filtrar y ampliar imágenes; los diálogos se cierran con Escape. La portada responde al cursor y al desplazamiento, y se respeta la preferencia de movimiento reducido.

## Tecnologías

React, TypeScript, Vite y Tailwind CSS. Los componentes de interfaz utilizan Radix UI y shadcn/ui. Las versiones están fijadas en `package-lock.json`; las licencias incluidas se conservan en `vendor/`.
