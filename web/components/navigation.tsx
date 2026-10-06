'use client';
import { useState } from 'react';
import { site } from '@/content/site';
const links = [{id:'inicio',label:'Inicio'},{id:'juego',label:site.game.title},{id:'arte',label:'Arte y desarrollo'},{id:'equipo',label:'Equipo'},{id:'contacto',label:'Contacto'}];
export function Navigation(){
 const [open,setOpen]=useState(false);
 return <header className="header"><a className="wordmark" href="#inicio" aria-label="LazySoft, inicio" onClick={()=>setOpen(false)}>lazysoft<span>STUDIO</span></a>
 <button className="menu-toggle" aria-expanded={open} aria-controls="main-navigation" onClick={()=>setOpen(!open)}>{open?'Cerrar':'Menú'}</button>
 <nav id="main-navigation" className={open?'nav-open':''} aria-label="Navegación principal" onKeyDown={e=>{if(e.key==='Escape'){setOpen(false);document.querySelector<HTMLButtonElement>('.menu-toggle')?.focus()}}}>{links.map(link=><a key={link.id} href={'#'+link.id} onClick={()=>setOpen(false)}>{link.label}</a>)}</nav></header>;
}
