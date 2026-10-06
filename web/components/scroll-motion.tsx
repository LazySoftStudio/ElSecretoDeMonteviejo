'use client';
import { useEffect } from 'react';
export function ScrollMotion(){
 useEffect(()=>{
  const motion=window.matchMedia('(prefers-reduced-motion: reduce)');
  const elements=document.querySelectorAll<HTMLElement>('[data-reveal]');
  const observer=new IntersectionObserver(entries=>{entries.forEach(entry=>{if(!entry.isIntersecting)return;entry.target.classList.add('is-visible');observer.unobserve(entry.target);});},{threshold:.08});
  elements.forEach(element=>{element.classList.add('reveal-ready');observer.observe(element);});
  const showAll=()=>{if(motion.matches)elements.forEach(element=>element.classList.add('is-visible'));};
  motion.addEventListener('change',showAll);showAll();
  return()=>{observer.disconnect();motion.removeEventListener('change',showAll);elements.forEach(element=>element.classList.remove('reveal-ready'));};
 },[]);
 return null;
}
