# ---

# 

# **GDD**

# El secreto de Monteviejo

LazySoft

# ---

# 

# 

# 

# 

# 

**REALIZADO POR**  
ÁNGEL BERMÚDEZ FARIÑAS  
ALBERTO CARO CANDÓN  
DANIEL CORBACHO ANES  
ALEJANDRO HERNÁNDEZ RUIZ  
PAULA ORTIZ FERNÁNDEZ  
IVÁN PALACIOS MARTÍN 

# Índice

[**INTRODUCCIÓN**](#introducción)

[**PLAN DE PRODUCCIÓN**](#plan-de-producción)

[PLAN DE DESARROLLO](#plan-de-desarrollo)

[PLAN DE MONETIZACIÓN](#plan-de-monetización)

[**MECÁNICAS**](#mecánicas)

[**HISTORIA**](#historia)

[Cinemática Inicial](#cinemática-inicial)

[**ARTE**](#arte)

[**ANIMACIÓN**](#animación)

[**ESCENARIOS**](#escenarios)

[**PERSONAJES**](#personajes)

[**MÚSICA Y SONIDOS**](#música-y-sonidos)

[**INTERFACES (UI/UX)**](#interfaces)

[**TECNOLOGÍAS Y HERRAMIENTAS**](#tecnologías-y-herramientas)

[**REQUISITOS DEL JUEGO**](#requisitos-del-juego)

[REQUISITOS FUNCIONALES](#requisitos-funcionales)

[REQUISITOS NO FUNCIONALES](#requisitos-no-funcionales)

[**RIESGOS Y SOLUCIONES**](#riesgos-y-soluciones)

[HITO 1](#hito-1)

[**ANEXO**](#anexo)

# **INTRODUCCIÓN**

A continuación se presenta el videojuego El Secreto de Monteviejo desarrollado por LazySoft. Es un juego 3D con vista en tercera persona donde se encarna a un joven que debe investigar en los secretos de un pueblo. El objetivo principal de esta aventura narrativa es descubrir la identidad de unos cuerpos que el jugador encuentra por casualidad. Es ideal para aquellos jugadores que desean descubrir las relaciones entre los personajes y explorar por un mundo abierto. A diferencia de muchos otros juegos, se da libertad de exploración y se incluyen subtramas que hacen de la investigación algo emocionante. Estará disponibles para PC y a partir de 7 años.

# PLAN DE PRODUCCIÓN

## PLAN DE DESARROLLO

El desarrollo del proyecto se organizará en varias fases de producción, cada una con objetivos diferentes asignados a diferentes miembros del equipo, con el fin de llevar a cabo el proyecto hacia adelante. Se establecerá un reparto de áreas en función de las habilidades y gustos de los miembros del grupo, para facilitar la coordinación y el seguimiento del desarrollo. A continuación se observa la matriz de multifuncionalidad del grupo:

Leyenda: 🟢Buena Habilidad | 🟡Habilidad Regular | 🔴 Mala Habilidad   
      \+ Estar Interesado | \- No Estar Interesado

|   | Game Design | Narrativa | Arte | Animación | Sonido | UX/UI | Accesibilidad | Programación |
| :---- | :---- | :---- | :---- | :---- | :---- | :---- | :---- | :---- |
| Dani | 🟢+ | 🟢+ | 🟢+ | 🟢+ | 🟢+ | 🟡 | 🟡 | 🟡- |
| Angel | 🟢+ | 🟡 | 🟢+ | 🟡+ | 🔴\- | 🟡 | 🟡 | 🟡- |
| Alex | 🟢+ | 🟡 | 🟡- | 🟢+ | 🟢 | 🟢 | 🟡+ | 🟡- |
| Paula | 🟢+ | 🟡 | 🟢+ | 🟢+ | 🟡- | 🟡 | 🟡+ | 🟢 |
| Ivan | 🟢+ | 🟡 | 🟡 | 🟡 | 🟡 | 🟡 | 🟡 | 🟢+ |
| Alberto | 🟢+ | 🟡 | 🟡 | 🟡 | 🟡 | 🟢 | 🟢 | 🟡 |

Durante todo el proyecto se realizarán reuniones periódicas para revisar el desarrollo del proyecto y avanzar en aspectos comunes del mismo, además de resolver incidencias y planificar el futuro del desarrollo, se realizarán reuniones a modo de review, para ver el resultado del proyecto desde la última reunión, y retrospectivas, para observar cómo ha trabajado el equipo y posibles mejoras.

## PLAN DE MONETIZACIÓN

El equipo ha decidido plantear un modelo de monetización basado principalmente en un sistema Buy to Play, complementado con contenido descargable (DLCs), y una campaña inicial de crowdfunding para ayudar a financiar parte del desarrollo del proyecto.

En la fase inicial del proyecto se lanzará una campaña de crowdfunding para cubrir parte de los gastos de producción. La campaña seguirá un modelo Pay What You Want (PWYW), permitiendo que cada persona done la cantidad que considere oportuna. A cambio, el público que haya donado se le compensará, en función de su aportación, con un pack sorpresa con posters, figuras de personajes, objetos de colección y otros artículos de merchandising exclusivos relacionados con el juego. Además, de esta forma se conseguirá feedback temprano. 

El modelo principal de monetización será Buy to Play, por lo que los jugadores deberán adquirir una copia del videojuego para poder acceder a la experiencia completa, con el objetivo de ofrecer el mismo producto de calidad a todos los usuarios que hayan adquirido el videojuego. 

Una vez lanzado el juego, se contempla la publicación de DLCs que amplíen la experiencia de los usuarios. Los DLCs pretenden incorporar dos nuevos cadáveres que el jugador deberá encontrar y resolver. Este contenido es completamente opcional y estará disponible para los jugadores que quieran seguir explorando *El Secreto de Monteviejo*. Estos ingresos adicionales ayudarán a financiar futuras actualizaciones y futuros proyectos de LazySoft.

Este modelo de monetización se escogió principalmente teniendo en cuenta al público objetivo del juego, que, teniendo en cuenta la taxonomía de Bartle, será principalmente jugadores “explorers”. Estos buscan explorar todo el contenido que ofrece el juego, por lo que los DLCs con nuevos cadáveres o incluso nuevos pueblos es algo que les encantará, ya que les otorga más contenido que explorar. Aunque este tipo de jugadores es el menos rentable, teniendo en cuenta el género del juego y que LazySoft es un estudio Indie pequeño, es el modelo más acertado y seguro para generar ingresos.

# MECÁNICAS
Las mecánicas es un aparte muy importante dentro del videojuego, indican lo que el jugador puede hacer en este. Las principales son el movimiento del jugador (movimiento tipo tanque) y la interacción con el entorno. Se puede interactuar con los NPCs que habrá repartidos por el escenario y además se podrá entrar en casa de alguno de ellos. El objetivo es que el jugador tenga que explorar para investigar sobre los cuerpos encontrados. Para ello los diálogos tendrán opciones y se ramifican dependiendo de las decisiones del jugador, pudiendo encontrar pistas materiales (objetos) o información. Todas estas pistas materiales se gestionan en un sistema de inventario. Este sistema permite guardar los objetos y nombres encontrados a través de la investigación. También habrá una forma de organización de pistas, donde se podrán arrastrar los objetos del inventario y los nombres, para que el jugador pueda organizar sus ideas.

Los controles serán los siguientes:  
**W** : moverse en la dirección del jugador hacia delante  
**S** : moverse en la dirección del jugador hacia atrás  
**A** : rotar la dirección del jugador hacia la izquierda  
**D** : rotar la dirección del jugador hacia la derecha  
**E** : interactuar con un objeto  
**Esc** : pausar el juego y mostrar la pantalla de pausa  
**M** : abrir inventario / pantalla de organización de pistas  
**Espacio**: avanzar diálogo

El juego será más difícil al inicio, ya que el jugador tiene que entender cómo funciona el sistema de pistas e información, una vez se familiarice con esta dinámica, la dificultad se mantiene durante la partida.

# HISTORIA

El juego se desarrollará en un pequeño pueblo de España, Monteviejo, al que un joven llamado Antonio se muda por trabajo sin saber nada sobre su historia. Pronto descubrirá que el pueblo esconde mucho más de lo que le habían contado, pues no solo hay una gran rivalidad entre los dos bandos del pueblo, sino que al poco de mudarse encontrará una fosa de cadáveres. Antonio decidirá indagar y descubrir todos los secretos del pueblo, intentando investigar las historias de aquellos cuerpos para darles sepultura y que por fin puedan descansar en paz. Así, el jugador deberá ir siguiendo las distintas pistas que encuentre por el pueblo para hallar las identidades de los cuerpos.

## Cinemática Inicial

El juego comenzará con la cinemática que se describe a continuación:

En una cálida mañana de septiembre, Antonio llega a la localidad de Monteviejo. Su superior, el director en la consejería de Medio Ambiente y Desarrollo Cultural, le había mandado unos meses a ese pueblo para que evalúe las necesidades del municipio relacionadas con el entorno natural. 

Para Antonio es otro trabajo aburrido y rutinario, que le dejará encerrado en un pueblo perdido en mitad de la nada durante dos largos meses, lejos de sus amigos y de la vida de la ciudad.

Una vez llegado a lo que iba a ser su casa los próximos meses, Antonio bajó del coche, observó la casa y se dispuso a descargar las cajas que llevaba en el coche. De repente siente que alguien lo observa.

De un momento a otro, lentamente se abre la puerta de la casa de al lado y sale una señora mayor muy despacio a darle la bienvenida. 

**Eugenia**: Buenos días, majo, por lo que veo eres nuevo (mirando las cajas y maletas), Me llamo Eugenia y soy tu vecina de al lado.

**Antonio**:

-  ¿Cómo ha dicho que se llamaba?	  
  **Eugenia**: Eugenia, hijooo  
- ¿Me ayudaría con una cosa?  
  **Antonio:** Tengo un calcetín con un tomate, ¿Me lo cosería?	  
  **Eugenia**: claro, tu pásamelo y yo  mañana te lo remiendo  
- Encantado, yo soy Antonio, he venido de la consejería del Medio Ambiente a hacer un trabajo a Monteviejo

**Eugenia**: Uuuuuuh, cerebro privilegiado. Mañana pasate por casa que te voy a preparar una tortillica que te vas a chupar los dedos, ya lo verás

**Antonio**:

- No soy muy fan de la tortilla, lo siento Eugenia  
  **Eugenia**: Ya verás como la mía si, la mia no es como las demás  
- ¿Lleva cebolla?  
  **Eugenia**: Obviamente, hijo  
- No hace falta Eugenia, traigo comida que me la tengo que comer rápido, otro día.

**Eugenia**: Bueeno, está bien majo. Le dejo ya tranquilito para bajar las maletas… Por cierto, una última cosa…Elige bien tu bando, hijo…

Sin dar explicaciones, la vecina se da la vuelta y vuelve lentamente hacia su casa, dejando a  Antonio desconcertado ante la bienvenida.

Pasadas unas horas, Antonio ya ha terminado de desempaquetar las cosas, y decide irse a dar una vuelta. (*Aquí se le da al jugador libertad para que explore un poco el mapa*) Paseando llega a la plaza, observa la iglesia, el ayuntamiento, etc. Antonio se da cuenta de que tiene sed y decide tomarse algo (*Aquí el jugador deberá decidir en qué bar tomarse algo*), una vez en el bar, el camarero le atiende.

**Camarero**: ¿Qué va a tomar el señorito?

**Antonio**:

- Un refresco, por favor  
  **Camarero**: Solo tenemos cerveza y agua con gas  
- Un café  
  **Camarero**: Solo tenemos cerveza y agua con gas  
- Una cerveza

**Camarero**: Buena elección, caballero. Haces bien en venir aquí y no al bar de enfrente de mi hermano gemelo…

Antonio aquí empieza a ver que hay algo raro en ese pueblo. Tras pagar, decide irse a casa dando una vuelta, para conocer un poco más el pueblo (Para que el jugador conozca el mapa)

Al día siguiente por la tarde, se oyen unos gritos provenientes de la calle. Cuando Antonio se asoma por la ventana, descubre al alcalde de Monteviejo y su mujer discutiendo con Eugenia. Una vez calmado el asunto, Antonio decide acercarse a saber cómo estaba. Antonio se encuentra a su vecina hablando con una amiga. 

**Eugenia**: Antonio, hijo, Buenas tardes nos de Dios

**Antonio**: 

- Buenos tardes a las dos

	**Amiga**: Buenas tardes, mozo

- Eugenia, te he visto discutiendo con el alcalde, estás bien


**Eugenia**: Si, si… Estoy bien, no te preocupes, querido.

**Amiga**: La familia del alcalde siempre ha estado algo… Loca  
**Eugenia**: Sole, baja la voz, que las paredes tienen oídos. Entrad los dos dentro

Los tres entran dentro de la casa y Antonio se sienta en el sofá mientras que las dos ancianas se sientan en dos sillones mirando a Antonio.

**Eugenia**: “Una noche de verano, hace ya unos años, los Toledo llegaron al pueblo y se asentaron por la Calleja del Cura, esa que está al lado del ayuntamiento, y ocuparon un terreno que según ellos les pertenecía a sus antepasados, esa gente de “bien” se piensa que todo es suyo. Parte de esas tierras pertenecían a mi marido y una noche fue a reclamar lo que era suyo, pero sin violencia, solamente derribó unos muros de la casa que se estaban construyendo, cosa que le sentó muy mal a los Toledo. Poco a poco, esa familia fue “comprando” seguidores para ponerse en contra de nosotros. Así el pueblo quedó dividido en dos bandos, los que defendían a mi marido y los que apoyaban a esa sucia familia.

Tiempo después, nos enteramos que el antiguo alcalde, Don Ignacio, muere en extrañas circunstancias y uno de los que se presenta para candidato a la alcaldía es Ramón Toledo. Mi marido intentó presentarse, pero debido a la gran influencia de los Toledo, perdió. Al poco de subir al poder, el alcalde mandó de madrugada un camión con soldados y se llevaron a unos cuantos que iban en su contra, entre ellos a nuestros maridos y nunca más les volvimos a ver.”

**Amiga**: mi marido ya se estaba oliendo esas cosas y planeo exiliarse a otro pueblo, pero saliendo por la puerta le pillaron. Antes de huir me dejó estos versos

“Un día, cuando ya no me persigan,  
que las estrellas salgan y bendigan,  
que bailen el mago y la doncella,  
te besaré a ti, mi estrella.”

**Eugenia**: Y así es como el pueblo lleva dividido años, sin poder hablar con antiguos y amigos, y peor aún, sin poder haber enterrado como es debido a mi Juanfran

Tras despedirse, Antonio decide dar un paseo para procesar la historia que acababa de oír. Los versos de Sole no paraban de resonar en su cabeza y recordó algo

**Antonio** (a sí mismo): El mago y la doncella… ¿No había una leyenda en este pueblo de eso? 

(*Aquí el jugador deberá preguntar a NPCs por la leyenda y conseguir llegar a un árbol con forma peculiar*)

Una vez en el árbol, Antonio se sienta en el suelo para descansar y cuando apoya la mano en el suelo, se encuentra con una bala antigua de fusil. Antonio abre los ojos como platos. Antonio acaba de descubrir el lugar del fusilamiento.

# ARTE

El juego se desarrollará en un ambiente rural en un entorno 3D. Los modelos serán de bajo poligonaje debido a las limitaciones de las plataformas en las que se quiere publicar el juego. Respecto a las texturas, se usarán texturas simples pero que representan bien los detalles del modelo del propio juego.

Como referencia del estilo visual, un buen ejemplo sería el videojuego Mario 64 (Véase imagen de referencia en los anexos). Con sus bajos polígonos y texturas simples, además de ser un juego de mundo abierto. 

Para la propuesta visual del entorno rural, se plantea un pueblo con viviendas antiguas y calles estrechas que desembocan en una plaza central. Las fachadas están hechas de piedra y los tejados cubiertos mayormente de ladrillo, con puertas de madera vieja y las típicas ventanas pequeñas desde donde puede cotillear el pueblo.  
La paleta de colores se basa en colores ocres y es la siguiente:

|   |   |   |   |   |
| ----- | ----- | ----- | ----- | ----- |
|  \#B8B01F |  \#C49421 |  \#AB7718 |  \#877E16 |  \#92B81F |

# ANIMACIÓN

El juego contará con dos tipos de animaciones, las animaciones de personajes (caminar, saludar, sentarse…) y las animaciones de escenarios. 

Se pretende que la cinemática inicial se parezca al siguiente storyboard:  

# ESCENARIOS

El escenario es el pueblo de Monteviejo, donde transcurre toda la historia. Dentro del pueblo, el jugador podrá moverse libremente por el pueblo, pudiendo entrar a algunas casas y edificios para obtener pistas que lo ayuden a completar la historia. 

Además, podrá hablar con todos los aldeanos que se encuentre (NPCs), algunos le darán información importante y otros no, por lo que debe interactuar con todo su entorno.

Los edificios principales del pueblo son: 

* Iglesia  
* Ayuntamiento (lugar de trabajo de Antonio)  
* Taberna 1  
* Taberna 2  
* Colegio  
* Biblioteca  
* Supermercado  
* Parque 1  
* Parque 2  
* Casa en ruinas  
* Médico  
* Hostal  
* Casa de Antonio  
* Casa de Eugenia  
* Casa del alcalde

Para la creación del escenario, primero se construye una versión a lápiz, a modo de boceto para comprender las diferentes secciones dentro del mapa, después, una vez definidas, se pasa a una herramienta de modelado 3D para poder crear el mapa que se incluirá en el proyecto de Unity.

# PERSONAJES

**ANTONIO:** Hombre de mediana edad, con barba, indumentaria informal, proveniente de San Fernando de Linares. Antonio ha llegado a Monteviejo para llevar a cabo un trabajo como encargado de la revisión de Medio Ambiente y Desarrollo Cultural del pueblo. Antonio es un hombre amable y educado, pero denota cansancio debido a su aburrido trabajo, sin embargo, siempre lleva una sonrisa en la cara cuando se dirige a la gente del pueblo.  

**EUGENIA:** Es la vecina de Antonio y es una persona de avanzada edad muy cotilla. Ha nacido y crecido en Monteviejo y ha visto cambiar su pueblo muchas veces. Tiene un carácter amable y trata como si fuera hijo suyo a todos los niños del pueblo (incluido Antonio). Suele vestir con ropa de color claro y nunca le falta una chaqueta para no pasar frio.

**ALCALDE:** El alcalde de Monteviejo, Arsenio, es un hombre de la alta sociedad y poco sociable que solo se relaciona con los suyos, y con trastornos de personalidad. Es arrogante y heredó el odio hacia la gente humilde del pueblo, especialmente a aquellos votantes del bando contrario. Por ello, les aplica una serie de impuestos extra a los paisanos como excusa de su crueldad haciéndoles pagar por tener ideologías contrarias a la máxima autoridad del pueblo. Además, vigila de cerca cualquier movimiento en el municipio y mantiene estrechos vínculos con los acontecimientos del pasado, convirtiéndose en uno de los principales obstáculos y sospechosos velados en la investigación de Antonio. 

**MUJER DEL ALCALDE:** Se llama Francisca, es una mujer bajita, de pelo castaño oscuro y con un sentido de la moda muy característico, proveniente de una familia que luchaba por subsistir, sin embargo, un día encontró a Arsenio y pensó que, al casarse con él, tendría la vida resuelta. Es una mujer amable y muy inteligente, apoya a Arsenio en sus decisiones y se mantiene al margen de la política, le gusta pasear por el pueblo, ir al supermercado, a la biblioteca, al colegio y conversar con los vecinos, estén o no de acuerdo con las ideas políticas de su marido. Es una mujer de casa que se encarga de mantener su hogar en pie, sin ella, Arsenio estaría perdido.

**MESEROS:** Gemelos provenientes de Ventas de la Reina, han llegado a Monteviejo a trabajar en el bar del pueblo para sacar algunos ahorros. Gustavo, uno de los hermanos, es sociable y extrovertido y le encanta atender a la clientela mientras que Basilio es introvertido y prefiere estar centrado en el trabajo tomando comandas y sirviendo comidas y bebidas. Aunque sean hermanos, no se llevan muy bien ya que tienen distintos genios y están en continuo enfrentamiento, por lo que decidieron separarse y montar cada uno su propia taberna. Aunque sean de otra población, los vecinos de Monteviejo les tratan como si fueran paisanos del propio pueblo haciendo que las horas que se pasan trabajando sean mucho más agradables.

Como se ha explicado, el juego consistirá en seguir distintas pistas por el pueblo para conseguir identificar los distintos cuerpos encontrados. A continuación, se describirán algunos de ellos:

**CUERPO 1:**

**Nombre**: Juan Francisco de Tomás Arribas  
**Edad**: 57 años  
**Estatura**: 1.65 cm  
**Aspecto**: Gordo, nariz grande, con calva (al estilo monje), lleva tirantes, uñas negras, ojos marrones y lleva reloj.  
**Personalidad**: Gruñón, trabajador, fumador de puros, cuñadismo nivel C2  
**Trasfondo**: Trabajador del campo, fusilado por el problema de la expropiación de tierras  
**Obj Representativo**: Reloj  
**Ruta de descubrimiento**:

* Descubres el reloj de Juanfran en la muñeca izquierda (te resulta raro, hay otro que lo tiene a la derecha) Recibes un puro de la marca típica que Juanfran solía fumar de un NPC  
* El camarero te ve con el puro y te empieza a hablar de que solo se lo había visto a una persona \-\> Juanfran  
* El camarero te da una foto y le ves a Juanfran pero no le reconoces porque no le ves el reloj (YA TIENES LA FOTO)  
* Investigando otro cadáver, descubres una foto de Juanfran con el reloj en la mano izquierda (YA ASOCIAS A JUANFRAN CON EL RELOJ)  
* Se lo entregas a la persona misteriosa

**CUERPO 2:**

**Nombre**: María Isabel López Abajos (Marisa)  
**Edad**: 51 años  
**Estatura**: 1.53 cm  
**Aspecto**: Pelo castaño-rojizo y rizado, ojos marrones, es flaca, suele llevar vestido antiguo y lleva gafas  
**Personalidad**: Agradable, alegre, un poco desconfiada, casada.  
**Trasfondo**: Profesora (igual que su marido), el hijo del alcalde le hacía la vida imposible  
**Obj representativo**:  Gafas  
**Ruta de descubrimiento**:

* Se encuentran las gafas en la fosa (YA TIENES EL OBJETO)  
* Escuchas a los niños del pueblo decir que están abarrotados en clase  
* Vas al colegio a preguntar (al final trabajas para el gobierno, aunque no seas del mismo departamento debes dar parte de ello)  
* El marido de Marisa te cuenta cosas (ASOCIAS A MARISA)  
* Coges la foto del despacho sin que nadie se entere (CONSIGUES LA FOTO)

**CUERPO 3:** 

**Nombre**: Federico Jimenez Machado  
**Edad**: 21 años  
**Estatura**: 1.72 cm  
**Aspecto**: Delgado, zurdo, pelo negro, bien vestido y le falta el brazo derecho.  
**Personalidad**: Alegre, romántico, entusiasta y defensor de los derechos y libertades.  
**Trasfondo**: Poeta, fusilado por seguir a su maestro.

**CUERPO 4:**  
— (Aún por definir) —

Además de estos personajes que se encuentran en la fosa, el jugador irá encontrando a distintos habitantes a medida que explora el pueblo. Algunos de ellos son: 

* Persona sentada en un banco con una espiga en la boca con la que se puede interactuar. Habrá que elegir el diálogo con el personaje y te dará información útil.  
* Personajes de ambos bandos que irán andando por el pueblo, y que al aproximarse a uno de su bando se saludan, y cuándo están cerca de alguien del otro bando, se pelean. Estos tendrán distintas necesidades básicas, como ir al baño o comer. Además, cada uno tendrá su función específica, como un repartidor.   
* Agricultores trabajando en el campo.  
* Bar: habrá distintos comportamientos de personajes en el bar, unos aldeanos que entran y salen, algunos bebiendo, otros pidiendo cosas, camareros repartiendo bebidas…  
* Niños jugando en el parque. Gritan, dan vueltas, se juntan unos con otros, se caen, lloran, se  montan en los columpios…

  *Se puede interactuar con todos los personajes, algunos te dan información útil y otros no, por ejemplo solo saludan y dicen hola.*

# 

# MÚSICA Y SONIDOS

\-(Aún por hacer)

# INTERFACES

Las interfaces para el videojuego serán las siguientes (aún son prototipos en desarrollo):

* **Interfaz de título:**  

* **Interfaz de nueva partida:**

*  **Interfaz carga de partida:**

* **Interfaz de ajustes:**

* **Interfaz de créditos:**

* **Interfaz de diálogos:**


Estas serían algunas de las propuestas diseñadas para el proyecto y apoyadas por el equipo de desarrollo.

# TECNOLOGÍAS Y HERRAMIENTAS

Las herramientas que se utilizarán en el desarrollo del videojuego serán: 

* **Unity**: motor de videojuego, se usará Visual Studio para programar en C\#  
* **3Ds Max**: modelado de objetos 3D y animaciones, tanto personajes como escenarios  
* **Photoshop**: diseño 2D de personajes  
* **GitHub**: control de versiones y herramienta para compartir el proyecto con los demás  
* **Miro**: organización de ideas  
* **Figma**: prototipado de interfaces

# REQUISITOS DEL JUEGO

## 	REQUISITOS FUNCIONALES
	— (Aún por hacer) —

## 	REQUISITOS NO FUNCIONALES
	— (Aún por hacer) —

# RIESGOS Y SOLUCIONES
—(Aún por hacer)---

# HOJA DE RUTA DE DESARROLLO
## HITO 1

Definir toda la historia del juego con la mayor cantidad de detalles posibles, así como el trasfondo tanto del escenario en el que se va a desarrollar, como el de los personajes que van a aparecer durante el transcurso del juego.

Empezar a crear diseños, tanto bocetos como dibujos digitales de los personajes, así como empezar a modelar tanto el escenario como a algunos personajes.

Crear el plan de desarrollo y de monetización, definiendo claramente las fortalezas y debilidades de cada trabajador y como se pretende obtener beneficios con el videojuego.

Por último, crear mecánicas de movimiento e interacción básicas, bocetos con detalle de las interfaces, y una primera vista de la página web.

# 

# 

# ANEXO

IMÁGENES DE REFERENCIA DE ESTILO: Crash Bandicoot, Mario 64 y otros juegos en 3D con poligonaje bajo.  

Para entregar a la persona misteriosa hay que llevar:

- foto de la persona antes de morir  
- algo característico suyo

Ideas pensadas para encontrar pistas:

- Periodico  
- Libro  
- Preguntando a NPC  
- Fotos  
- Gato  
- Detrás de cuadros  
- Archivos ayuntamiento  
- Archivos médicos  
- Caja enterrada  
- Leer diario  
- llave para abrir una casa  
- cartera

Al hombre misterioso le encuentras por:

- Pintadas que deja a lo largo de la ciudad  
- Carta a tu casa (Si eres muy tonto y no te enteras)

