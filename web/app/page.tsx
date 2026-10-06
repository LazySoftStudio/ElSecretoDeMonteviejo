import { site } from '@/content/site';
import { Hero } from '@/components/hero';
import { Navigation } from '@/components/navigation';
import { ArtGallery } from '@/components/art-gallery';
import { GameJourney } from '@/components/game-journey';
import { ScrollMotion } from '@/components/scroll-motion';
import { Credits } from '@/components/credits';
export default function Home(){
 return <><a className="skip-link" href="#contenido">Saltar al contenido</a><Navigation/>
 <main id="contenido"><Hero/>
 <section className="game-section" id="juego"><div className="wrap">
  <div className="section-heading"><p className="eyebrow">01 / NUESTRO PRIMER JUEGO</p><span className="outline-tag">En desarrollo</span></div>
  <div className="game-intro" data-reveal><h2>{site.game.title}{site.game.provisionalTitle && <span>Título provisional</span>}</h2><div><p className="display-copy">Un pueblo que guarda<br/>más de lo que cuenta.</p><p>{site.game.description}</p></div></div>
  <figure className="game-image" data-reveal><img loading="lazy" src="/assets/iglesia-volumen.png" alt="Estudio de volumen de la iglesia del pueblo, sin materiales finales"/><figcaption><span>MONTEVIEJO / PRIMERAS FORMAS</span><span>Estudio de volumen · Material de desarrollo</span></figcaption></figure>
  <GameJourney/>
  <div className="game-facts"><span>Aventura narrativa</span><span>3D estilizado</span><span>Tercera persona</span><span>Exploración e investigación</span></div><p className="development-note">Estos son los pilares de nuestra propuesta. El juego está en desarrollo y seguirá evolucionando.</p>
 </div></section>
 <section className="art-section" id="arte"><div className="wrap"><div className="section-heading"><p className="eyebrow">02 / DENTRO DEL PROCESO</p><span className="small-note">DEL PAPEL AL MUNDO DEL JUEGO</span></div><div className="section-intro" data-reveal><h2>Arte &<br/><em>desarrollo.</em></h2><p>Antes de que exista un mundo, hay unas cuantas líneas. Estos son los bocetos y estudios con los que estamos dando forma a Monteviejo.</p></div><ArtGallery/><div className="process-strip"><strong>Todavía queda mucho por descubrir.</strong><p>Compartiremos nuevas piezas conforme avance el desarrollo: escenarios, personajes y pruebas en el motor.</p></div></div></section>
 <section className="team-section" id="equipo"><div className="wrap"><div className="section-heading"><p className="eyebrow">03 / DETRÁS DEL MANDO</p><span className="outline-tag">6 integrantes</span></div><div className="team-layout"><div className="team-story" data-reveal><h2>Seis miradas.<br/><em>Un mismo<br/>juego.</em></h2><p>LazySoft nace de un proyecto universitario y de las ganas de crear videojuegos juntos. Compartimos ideas, aprendemos durante el proceso y construimos una historia a la que cada persona aporta su mirada.</p><div className="team-signature"><img loading="lazy" src="/assets/lazysoft-logo.jpeg" alt="Logotipo original de LazySoft" width="545" height="464"/><span>HECHO EN EQUIPO.<br/>A NUESTRO RITMO.</span></div></div><div className="team-list">{site.team.map((name,index)=><article key={name} data-reveal><span className="member-number">0{index+1}</span><div><h3>{name}</h3><p>Equipo LazySoft</p></div><span className="member-initials" aria-hidden="true">{name.split(' ').map(n=>n[0]).join('')}</span></article>)}</div></div></div></section>
 <section className="contact-section" id="contacto"><div className="wrap"><div className="section-heading"><p className="eyebrow">04 / CONTACTO</p><span className="small-note">LAS BUENAS IDEAS EMPIEZAN HABLANDO</span></div><h2 data-reveal>¿Hablamos?</h2><div className="contact-layout"><p>Para conocer más sobre el proyecto,<br/>compartir una idea o saludar al equipo.</p><a className="email-link" href={'mailto:'+site.email}>{site.email}</a></div></div></section>
 </main><footer className="footer"><div className="wrap"><div className="footer-top"><span>JUEGOS CON HISTORIA. HECHOS CON CALMA.</span><a href="#inicio">Volver al inicio</a></div><a className="footer-wordmark" href="#inicio" aria-label="LazySoft, volver al inicio">LAZYSOFT<span>STUDIO</span></a><div className="footer-bottom"><span>© 2026 LazySoft · Proyecto universitario</span><Credits/><span>EXPLORA. ESCUCHA. DESCUBRE.</span></div></div></footer><ScrollMotion/></>;
}
