// Contenido público: actualizar aquí conforme avance el proyecto.
export const site = {
 studio: 'LazySoft', email: 'lazysoftstudio@gmail.com',
 game: {title: 'Monteviejo',provisionalTitle: true,
 description: 'Antonio llega a Monteviejo para un trabajo que parece rutinario. Entre calles de piedra, conversaciones a media voz y recuerdos que no se han ido, descubre que conocer un lugar también significa escuchar su pasado.',
 pillars: [
 {title: 'Explora el pueblo',text: 'Calles, viviendas y una plaza donde se cruzan las historias. Un entorno rural pensado para descubrirlo a pie.'},
 {title: 'Escucha a sus habitantes',text: 'Cada vecino tiene su propia versión. El diálogo será la puerta de entrada a los vínculos y tensiones del pueblo.'},
 {title: 'Conecta las pistas',text: 'Observa, consulta documentos y contrasta testimonios para reconstruir lo que ocurrió.'},
 ]},
 team: ['Ángel Bermúdez','Alberto Caro','Daniel Corbacho','Alejandro Hernández','Paula Ortiz','Iván Palacios'],
};

export type Artwork = { id: string; title: string; category: 'Escenarios'|'Personajes'; type: string; src: string; alt: string; description: string };
export const artworks: Artwork[] = [
 {id:'01',title:'La plaza como punto de encuentro',category:'Escenarios',type:'Boceto de entorno',src:'/assets/plaza-boceto.jpg',alt:'Boceto a lápiz de una plaza rural con fachadas y espacio central',description:'Una primera aproximación a los espacios compartidos del pueblo y a la relación entre sus edificios.'},
 {id:'02',title:'Recorrer las calles',category:'Escenarios',type:'Boceto de entorno',src:'/assets/calle-boceto.jpg',alt:'Boceto a lápiz de una calle entre viviendas, con pavimento de piedra',description:'Calles estrechas, viviendas antiguas y una escala cercana para acompañar la exploración.'},
 {id:'03',title:'El pueblo sobre el papel',category:'Escenarios',type:'Estudio de distribución',src:'/assets/plano-pueblo.png',alt:'Plano a mano con la distribución inicial de edificios y calles',description:'El primer esquema espacial ayuda a pensar los recorridos y los lugares donde se encontrarán los habitantes.'},
 {id:'04',title:'Buscar la silueta',category:'Personajes',type:'Boceto de personaje',src:'/assets/personaje-boceto.jpg',alt:'Estudio a lápiz de un personaje visto de frente y de perfil',description:'Exploración inicial de proporciones y silueta para los personajes del proyecto.'},
 {id:'05',title:'Una referencia en la plaza',category:'Escenarios',type:'Estudio de volumen',src:'/assets/iglesia-volumen.png',alt:'Modelo de volumen de una iglesia con torre, sin materiales finales',description:'La iglesia como hito del entorno. Un estudio de formas y escala previo al acabado visual.'},
 {id:'06',title:'Primeras fachadas',category:'Escenarios',type:'Estudio de volumen',src:'/assets/calle-volumen.png',alt:'Estudio de edificios blancos sin materiales, alineados junto a una calle',description:'Exploración de fachadas, alturas y cubiertas para construir el conjunto de viviendas.'},
];
