'use client';
import { useState } from 'react';
import { Dialog,DialogTrigger,DialogContent,DialogTitle,DialogDescription,DialogClose } from '@/components/ui/dialog';
import { artworks } from '@/content/site';
export function ArtGallery(){
 const [filter,setFilter]=useState('Todo');
 const shown=artworks.filter(art=>filter==='Todo'||art.category===filter);
 return <><div className="gallery-toolbar"><div className="filters" role="group" aria-label="Filtrar material por categoría">{['Todo','Escenarios','Personajes'].map(category=><button key={category} aria-pressed={filter===category} onClick={()=>setFilter(category)}>{category}</button>)}</div><p className="gallery-count" aria-live="polite">{shown.length} {shown.length===1?'pieza':'piezas'}</p></div>
 <div className="art-grid">{shown.map((art)=><Dialog key={art.id}><article className="art-card"><DialogTrigger asChild><button className="art-trigger" aria-label={'Ampliar: '+art.title}><div className="art-photo"><img loading="lazy" src={art.src} alt={art.alt}/><span className="zoom-label">Ampliar +</span></div><div className="art-caption"><div><p>{art.type}</p><h3>{art.title}</h3></div><span className="art-index">{art.id}</span></div></button></DialogTrigger><p className="art-summary">{art.description}</p></article><DialogContent className="art-dialog" showCloseButton={false}><div className="dialog-bar"><span>{art.category} / {art.type}</span><DialogClose className="close-dialog">Cerrar ×</DialogClose></div><img src={art.src} alt={art.alt}/><DialogTitle>{art.title}</DialogTitle><DialogDescription className="dialog-description">{art.description} Material incluido en el GDD de arte del proyecto. Diseño en desarrollo.</DialogDescription></DialogContent></Dialog>)}</div></>;
}
