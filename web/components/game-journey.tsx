'use client';
import { useEffect, useRef, useState } from 'react';
import { site } from '@/content/site';
const scenes = [
 {src:'/assets/calle-volumen.png',alt:'Estudio de volumen de las calles y viviendas de Monteviejo',label:'El espacio / Estudio de volumen'},
 {src:'/assets/personaje-boceto.jpg',alt:'Boceto de un personaje, de frente y de perfil',label:'Sus habitantes / Estudio de personaje'},
 {src:'/assets/plano-pueblo.png',alt:'Plano dibujado a mano con los primeros recorridos del pueblo',label:'Las conexiones / Plano del pueblo'},
];
export function GameJourney(){
 const container=useRef<HTMLDivElement>(null);
 const [active,setActive]=useState(0);
 useEffect(()=>{
  const steps=container.current?.querySelectorAll<HTMLElement>('[data-journey-step]');
  if(!steps)return;
  const observer=new IntersectionObserver(entries=>{
   for(const entry of entries)if(entry.isIntersecting)setActive(Number((entry.target as HTMLElement).dataset.journeyStep));
  },{rootMargin:'-35% 0px -35% 0px',threshold:0});
  steps.forEach(step=>observer.observe(step));
  return()=>observer.disconnect();
 },[]);
 return <div className="journey" ref={container}>
  <div className="journey-visual" aria-hidden="true"><div className="journey-stage">{scenes.map((scene,index)=><img key={scene.src} className={index===active?'scene-active':''} src={scene.src} alt="" loading="lazy"/>)}</div><div className="journey-caption"><span>{scenes[active].label}</span><span>0{active+1} / 03</span></div><div className="journey-progress">{scenes.map((scene,index)=><span key={scene.src} className={index===active?'is-active':''}/>)}</div></div>
  <div className="journey-steps">{site.game.pillars.map((pillar,index)=><article className="journey-step" data-journey-step={index} key={pillar.title}><span className="chapter-number">0{index+1}</span><h3>{pillar.title}</h3><p>{pillar.text}</p><img className="journey-mobile-image" src={scenes[index].src} alt={scenes[index].alt} loading="lazy"/><p className="material-note">{scenes[index].label} · Material de desarrollo</p></article>)}</div>
 </div>;
}
