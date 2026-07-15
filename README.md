<div align="center" id="readme-top">

<img src="https://raw.githubusercontent.com/Xhope17/orbit-app/main/public/images/orbit_logo_v2.svg" alt="Orbit" width="70" height="70" style="border-radius: 50%"> 

# Orbit

### Red social con mensajería en tiempo real, publicaciones y comunidades

Frontend de **Orbit**, una red social con mensajería instantánea, comunidades y publicaciones en tiempo real. Conexión en tiempo real mediante SignalR.

[![Angular][angular-badge]][angular-url] [![TypeScript][typescript-badge]][typescript-url] [![Tailwind CSS][tailwind-badge]][tailwind-url] [![DaisyUI][daisyui-badge]][daisyui-url] [![SignalR][signalr-badge]][signalr-url] [![Vercel][vercel-badge]][vercel-url]

<br>

</div>

## Tabla de contenido

- [Características](#características)
- [Estructura del proyecto](#estructura-del-proyecto)
- [Requisitos previos](#requisitos-previos)
- [Instalación](#instalación)
- [Variables de entorno](#variables-de-entorno)
- [Ejecución](#ejecución)
- [Build de producción](#build-de-producción)
- [Pruebas](#pruebas)
- [Demo](#demo)
- [Despliegue](#despliegue)
- [WebSockets](#websockets)
- [Backend](#backend)

<br>

## Características

- Autenticación con JWT (login, registro, renovación de token).
- Feed principal con publicaciones en orden cronológico.
- Publicaciones: crear, editar, eliminar, citar y republicar posts.
- Comentarios en publicaciones con hilos de respuestas.
- Me gusta en posts y comentarios.
- Comunidades: crear, unirse, explorar y ver detalle con miembros.
- Chat en tiempo real mediante SignalR con mensajes directos.
- Perfiles de usuario con foto, biografía, seguidores y seguidos.
- Búsqueda de publicaciones y perfiles.
- Notificaciones en tiempo real.
- Bookmarks para guardar publicaciones favoritas.
- Tema oscuro con diseño responsive.
- Scroll infinito en feed y listas.

<p align="right">(<a href="#readme-top">volver arriba</a>)</p>

<br>

## Estructura del proyecto

```
orbit-app/
├── public/                    # Archivos estáticos (imágenes, iconos)
├── src/
│   ├── main.ts                # Punto de entrada
│   ├── index.html             # HTML principal
│   ├── styles.css             # Estilos globales
│   ├── app/
│   │   ├── app.config.ts      # Configuración de la app
│   │   ├── app.routes.ts      # Definición de rutas
│   │   ├── core/              # Guards, interceptors, layouts
│   │   ├── features/          # Páginas, servicios, interfaces
│   │   └── shared/            # Componentes reutilizables, pipes, servicios
│   └── environments/          # Variables de entorno
├── angular.json
├── package.json
├── tsconfig.json
└── tailwind.config.js
```

<p align="right">(<a href="#readme-top">volver arriba</a>)</p>

<br>

## Requisitos previos

Antes de comenzar, asegúrate de tener instalado:

- **Node.js** >= 20 (probado en 22)
- **npm** (incluido con Node.js)
- **Angular CLI** global:

  ```bash
  npm install -g @angular/cli
  ```

- El backend [**orbit-api**](#backend) corriendo (local o desplegado)

<br>

## Instalación

```bash
git clone https://github.com/Xhope17/orbit-app.git
cd orbit-app
npm install
```
<br>

## Variables de entorno

Las variables se definen en `src/environments/`:

| Variable | Descripción |
|----------|-------------|
| `apiUrl` | URL base de la API REST |
| `tokenKey` | Key para almacenar el JWT en localStorage |
| `refreshTokenKey` | Key para almacenar el refresh token |
| `signalRUrl` | URL del hub de SignalR |

<br>

## Ejecución

```bash
ng serve        # servidor de desarrollo en http://localhost:4200
```

<br>

## Build de producción

```bash
ng build                              # genera dist/
ng build --base-href=/orbit-app/      # para GitHub Pages
```

<br>

## Pruebas

```bash
ng test         # ejecuta pruebas con Vitest
```

<br>

## Demo

[https://xhope17.github.io/orbit-app](https://xhope17.github.io/orbit-app)

> La demo se despliega automáticamente desde la rama `develop` mediante GitHub Actions.

<br>

## Despliegue

| Entorno | Plataforma | Rama | URL |
|---------|-----------|------|-----|
| Demo / QA | GitHub Pages | `develop` | [xhope17.github.io/orbit-app](https://xhope17.github.io/orbit-app) |
| Producción | Vercel | `main` | — |

<p align="right">(<a href="#readme-top">volver arriba</a>)</p>

<br>

## WebSockets

La aplicación usa **SignalR** para funcionalidades en tiempo real.

> Ver [`websocket.md`](websocket.md) para documentación detallada de los hubs, eventos y autenticación.

<br>

## Backend

El servidor que consume esta aplicación está en el repositorio:

**[Xhope17/orbit-api](https://github.com/Xhope17/orbit-api)**

Backend en ASP.NET Core con SignalR, SQL Server y autenticación JWT.

<br>

<div align="center">
Desarrollado con Angular · Orbit
</div>

<!-- [contributors-shield]: https://img.shields.io/github/contributors/Xhope17/orbit-app.svg?style=for-the-badge -->
<!-- [contributors-url]: https://github.com/Xhope17/orbit-app/graphs/contributors -->
<!-- [stars-shield]: https://img.shields.io/github/stars/Xhope17/orbit-app.svg?style=for-the-badge -->
<!-- [stars-url]: https://github.com/Xhope17/orbit-app/stargazers -->
[angular-badge]: https://img.shields.io/badge/Angular-fff?style=for-the-badge&logo=angular&logoColor=DD0031&color=0F0F11
[angular-url]: https://angular.dev/
[typescript-badge]: https://img.shields.io/badge/TypeScript-3178C6?style=for-the-badge&logo=typescript&logoColor=white&color=3178C6
[typescript-url]: https://www.typescriptlang.org/
[tailwind-badge]: https://img.shields.io/badge/Tailwind-ffffff?style=for-the-badge&logo=tailwindcss&logoColor=06B6D4
[tailwind-url]: https://tailwindcss.com/
[daisyui-badge]: https://img.shields.io/badge/DaisyUI-fff?style=for-the-badge&logo=daisyui&logoColor=000000&color=E5A83A
[daisyui-url]: https://daisyui.com/
[signalr-badge]: https://img.shields.io/badge/SignalR-512BD4?style=for-the-badge&logo=dotnet&logoColor=white&color=512BD4
[signalr-url]: https://dotnet.microsoft.com/apps/aspnet/signalr
[vercel-badge]: https://img.shields.io/badge/Vercel-000000?style=for-the-badge&logo=vercel&logoColor=white&color=000000
[vercel-url]: https://vercel.com/