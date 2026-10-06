# LazySoft web

Portfolio del equipo y del videojuego ambientado en Monteviejo. El título es provisional. Primera versión del 5 de octubre de 2026.

## Desarrollo local

Requiere Node.js 22.13 o posterior. Instalar con `npm ci` y abrir con `npm run dev`. La dirección aparece en la consola. Validar con `npx tsc --noEmit` y `npm run build`.

En esta máquina, los lanzadores npm de algunos helpers no resolvieron su ruta. Funcionó la CLI real: `node "C:/Program Files/nodejs/node_modules/npm/bin/npm-cli.js" ci --no-audit --no-fund`. También funcionan `node scripts/run-framework.mjs dev` y `node scripts/run-framework.mjs build`.

## Añadir material

- `content/site.ts`: título, sinopsis, correo, equipo y colección de arte. El título se comparte con el menú. Cambiar `provisionalTitle` cuando el equipo confirme el nombre.
- `public/assets`: imágenes usadas por la web. Añadir únicamente materiales que puedan mostrarse. No guardar aquí el GDD, entregas, documentos privados o fuentes con secretos.
- `app/page.tsx`: secciones, en el orden acordado.
- `app/globals.css`: colores, composición y adaptación a móvil.
- `components/art-gallery.tsx`: filtros y ampliación accesible de imágenes.
- `components/navigation.tsx`: navegación de escritorio y móvil.
- `components/credits.tsx`: créditos del equipo y materiales.

Para añadir una pieza: copiar la imagen a `public/assets` y añadir un objeto a `artworks` en `content/site.ts` con identificador único, título, categoría, tipo, ruta, texto alternativo y descripción. La galería la incorporará sin cambiar la estructura. Identificar si se trata de boceto, estudio de volumen o captura del juego.

La colección inicial contiene seis imágenes extraídas de la sección de primeros diseños de `GDD_ARTE_FINAL.docx`. Las referencias fotográficas de terceros y la historia completa del GDD no se han incorporado.

## Estado y límites

Menú, anclas, filtros, ampliación de imágenes, créditos y enlace de correo implementados. El contacto abre el cliente de correo del visitante; no envía nada automáticamente. No hay formulario, analítica, suscripciones ni base de datos. Los roles del equipo, el título definitivo, plataformas, fecha, demo y tráiler no están confirmados.

## Publicación

Se utiliza Sites con acceso privado para revisión. `.openai/hosting.json` conserva el identificador de este sitio: reutilizarlo en las siguientes actualizaciones, no crear otro. Las credenciales temporales no se guardan. El código permanece en esta carpeta para continuar con Codex o Claude Code.

Tecnologías: React, TypeScript estricto, Tailwind 4 y Vinext, con las versiones fijadas en `package-lock.json`. La ficha compartida está en `UNI/Proyectos/LazySoft/Ficha.md`.

## Rediseño del 6 de octubre de 2026

Referencia visual: vídeo aportado por el usuario (18,885 s), con ejemplos de objetos en primer plano, tipografía gigante, contraste claro/oscuro y transiciones al desplazarse. Se conservan las secciones y el contenido del portfolio.

- `components/hero.tsx`: imagen con volumen y movimiento por capas ligado al cursor y al desplazamiento. No es un objeto 3D giratorio ni utiliza WebGL.
- `components/game-journey.tsx`: tres capítulos con imágenes que cambian en un panel fijo en escritorio; recorrido apilado en móvil.
- `components/scroll-motion.tsx`: entrada de contenidos al aparecer en pantalla. El contenido se mantiene visible sin JavaScript y se respeta `prefers-reduced-motion`.
- `public/assets/lazysoft-mascot-3d.png`: reinterpretación decorativa del logotipo mediante image_gen integrado, con transparencia. No pertenece al material jugable. Logotipos originales conservados; uso aclarado en Créditos.

Prompt de la imagen: “Reinterpret the existing LazySoft sleeping sloth embracing a charcoal gamepad as a high-end stylized 3D studio product sculpture. Preserve face, sleeping pose, limb arrangement and cyan/violet LS monogram. Soft matte vinyl, satin plastic, delicate cyan rim, entire object centered on true transparent background. No environment, new text, accessories or extra limbs.”

La galería continúa mostrando exclusivamente las seis piezas reales del GDD; ninguna se ha sustituido por arte generado.

### Prompt exacto de la mascota (image_gen integrado)

```text
Use case: style-transfer.
Asset type: transparent website hero mascot, square approximately 1024×1024.
Input image 1 is the edit target and identity reference: the existing LazySoft logo depicting a sleeping brown sloth embracing a charcoal gamepad.
Primary request: Reinterpret this exact mascot as a high-end stylized 3D studio product sculpture. Preserve its recognizable cream round face, closed eyes, tiny peaceful smile, brown eye patches, hair tuft, sleeping pose and same limb arrangement embracing the controller. Preserve the controller's broad rounded two-grip silhouette and prominent interwoven LS monogram, cyan at the bottom and violet at the top with tiny light speckles.
Materials: soft smooth matte vinyl brown sloth with dimensional rounded forms; subtle satin charcoal plastic gamepad; tactile sculptural quality, carefully rounded claws.
Composition: entire object centered, generous transparent margin, slightly three-quarter view while keeping face and LS logo legible. No cropping.
Lighting: dramatic but soft studio lighting with a delicate cyan rim, designed for placement in front of huge typography on a midnight-dark website (do not render the typography or midnight background).
Scene/backdrop: true transparent alpha background. No environment, floor, platform or ground shadow.
Constraints: exactly one mascot and one controller. Keep face, pose, limb count/arrangement and logo identity faithful to reference. This is a decorative studio mascot, not game art. No extra limbs, accessories, new text, watermark or additional objects.
```
