'use client';

import { useEffect, useRef } from 'react';
import { site } from '@/content/site';

export function Hero() {
  const scene = useRef<HTMLElement>(null);
  useEffect(() => {
    const node = scene.current;
    if (!node) return;
    const motion = window.matchMedia('(prefers-reduced-motion: reduce)');
    let frame = 0;
    let pointerX = 0;
    let pointerY = 0;
    const paint = () => {
      frame = 0;
      const progress = motion.matches ? 0 : Math.min(1, Math.max(0, -node.getBoundingClientRect().top / node.offsetHeight));
      node.style.setProperty('--hero-scroll', String(progress));
      node.style.setProperty('--pointer-x', String(motion.matches ? 0 : pointerX));
      node.style.setProperty('--pointer-y', String(motion.matches ? 0 : pointerY));
    };
    const schedule = () => { if (!frame) frame = requestAnimationFrame(paint); };
    const move = (event: PointerEvent) => {
      if (event.pointerType !== 'mouse') return;
      const bounds = node.getBoundingClientRect();
      pointerX = (event.clientX - bounds.left) / bounds.width - .5;
      pointerY = (event.clientY - bounds.top) / bounds.height - .5;
      schedule();
    };
    const leave = () => { pointerX = 0; pointerY = 0; schedule(); };
    node.addEventListener('pointermove', move);
    node.addEventListener('pointerleave', leave);
    window.addEventListener('scroll', schedule, { passive: true });
    motion.addEventListener('change', schedule);
    paint();
    return () => {
      cancelAnimationFrame(frame);
      node.removeEventListener('pointermove', move);
      node.removeEventListener('pointerleave', leave);
      window.removeEventListener('scroll', schedule);
      motion.removeEventListener('change', schedule);
    };
  }, []);
  return <section ref={scene} className="hero-stage" id="inicio">
    <div className="hero-kicker"><span>ESTUDIO INDEPENDIENTE DE VIDEOJUEGOS</span><span>SEIS PERSONAS. UN MISMO MUNDO.</span></div>
    <h1 className="hero-title">LAZY<span>SOFT</span><span className="sr-only"> — Estudio de videojuegos</span></h1>
    <div className="hero-object"><img className="mascot-image" src="/assets/lazysoft-mascot-3d.png" alt="El perezoso de LazySoft abraza un mando de videojuegos" width="1254" height="1254" fetchPriority="high"/></div>
    <div className="hero-caption"><p>A nuestro ritmo.<br/><em>Con mucha historia.</em></p><div><p>Nuestro primer mundo<br/>está tomando forma.</p><a className="button light" href="#juego">Entra en {site.game.title}</a></div></div>
    <div className="hero-floor"><span>EXPLORA · ESCUCHA · DESCUBRE</span><a href="#juego">Desplázate para descubrir</a><span>PROYECTO EN DESARROLLO</span></div>
  </section>;
}
