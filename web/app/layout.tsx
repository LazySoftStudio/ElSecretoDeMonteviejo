import type { Metadata } from 'next';
import './globals.css';
export const metadata: Metadata = {
 title: 'LazySoft — Un equipo, muchas historias',
 description: 'Conoce LazySoft y nuestro primer videojuego narrativo en 3D. Explora el proyecto de Monteviejo, su arte y el equipo que le da forma.',
 icons: {icon: '/favicon.svg',shortcut: '/favicon.svg'},
};
export default function RootLayout({children}: Readonly<{children: React.ReactNode}>) {
 return <html lang="es"><body>{children}</body></html>;
}
