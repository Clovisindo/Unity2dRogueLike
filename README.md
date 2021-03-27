# Unity2dRogueLike
Repositorio juego unity2d Roguelike project


<!--
*** Thanks for checking out the Best-README-Template. If you have a suggestion
*** that would make this better, please fork the repo and create a pull request
*** or simply open an issue with the tag "enhancement".
*** Thanks again! Now go create something AMAZING! :D
***
***
***
*** To avoid retyping too much info. Do a search and replace for the following:
*** github_username, repo_name, twitter_handle, email, project_title, project_description
-->



<!-- PROJECT SHIELDS -->
<!--
*** I'm using markdown "reference style" links for readability.
*** Reference links are enclosed in brackets [ ] instead of parentheses ( ).
*** See the bottom of this document for the declaration of the reference variables
*** for contributors-url, forks-url, etc. This is an optional, concise syntax you may use.
*** https://www.markdownguide.org/basic-syntax/#reference-style-links
-->


<!-- PROJECT LOGO -->
<br />
<p align="center">


  <h3 align="center">Unity2DRogueLike</h3>

  <p align="center">
    Proyecto autodidacta de aprendizaje en unity2D
    <br />
    <a href="https://github.com/github_username/repo_name"><strong>Explore the docs »</strong></a>
    <br />
    <br />
  </p>
</p>





<!-- Sobre el proyecto -->
## Sobre el proyecto

Como proyecto personal de aprendizaje en unity, inicio este proyecto con objetivo de trabajar todo el ciclo de desarrollo de principio a fin de un videojuego.
El genero a explorar es el roguelike, en la experiencia mas pequeña posible, pero poder practicar y diseñar mecanicas de este genero de forma practica.


<!-- Diagrama de clases -->
## Diagrama de clases
Resumen de las funciones realizadas en el proyecto


<!-- Player.cs -->
### Player.cs
Controla las funcionalidades basicas del jugador:
Controles
Cambios de armas( con cooldown)
Dependiente de la clase seleccionada( de momento solo implementado el guerrero)
Habilidades especiales de la clase(bloqueo con escudo)
Gestion de cuando el jugador es invulnerable tras un golpe
Gestión de colisiones con los enemigos y comunicar al resto de sistemas el daño recibido.
Gestion de colisiones con las salidas/entradas de las habitaciones, y comunicacion con los demas sistemas para cambiar de habitación.
Varios metodos para actualizar variables del jugador: vida, posicion de inicio en la habitacion, animación de caida, gestión de armas para cambiar.


<!-- FfloorMechanic.cs -->
### FfloorMechanic.cs
Clase abstracta que implementa metodos de activar mecanismos genericos para los distintos objetos que se quieran implementar.
Actualmente estan integrados palancas, botones, enemigos ocultos, pociones, fuentes curativas, trampas y casillas de caida al vacio.

<!-- Weapon.cs -->
### Weapon.cs
Clase abstracta que implementa metodos para implementar armas especificas.
Controles de uso del arma, con ataque normal y especial.
Ataque direcional.
Armas implementadas: espadón, espada de caballero , escudo de caballero, martillo gigante.

<!-- Projectile.cs -->
### Projectile.cs
Implementa el comportamiento de disparar un projectil al objeto.
Al contacto con el jugador, notifica a los sistemas necesarios para aplicar el daño y destruir el projectil.
Pendiente cambiarlo a una clase abstracta si se implementa en mas tipos de objetos especificos.

<!-- Enemy.cs -->
### Enemy.cs
Clase abstracta que implementa las funciones comunes a todos los enemigos.
Comportamiento de enemigo(implementado en cada clase especifica).
Comportamiento basico de perseguir al jugador.
Gestion de muerte de enemigos y comunicar el cambio a los demas sistemas.
Gestión de vida, recibir daño y inmunidad despues de un golpe durante un tiempo.
Enemigos implementados actualmente:
Mimico: un cofre que al acercarte se descubre como un enemigo.
Goblin: pequeña y rapida criatura que persigue al jugador para atacarle en corta distancia.
Ogro: enemigo grande y lento que golpea en area.
Orco enmascarado: persigue al jugador haciendo fintas para engañarlo.
Orco chaman: dispara projectiles cuando el jugador se acerca.
Orco guerrero: carga en dirección al jugador.

<!-- BoardManager.cs -->
### BoardManager.cs
Clase que gestiona la creación de cada habitación casilla por casilla.
Ademas de generar un modelo basico de habitacion ractangular rodeada de paredes, genera las puertas de entrada y salida que se le indica a la hora de crear la habitación.

<!-- GameManager.cs -->
### GameManager.cs
Implementa distintas funciones de comunicación entre los distintos sistemas del juego.
Carga y gestiona las habitaciones generadas:
cambiar la habitacion actual
generar los enemigos o puzzles en cada habitación
cambio de habitación al cruzar puertas( cambia la camara , mueve al jugador , activa los enemigos o eventos de esa habitación.
Comprueba cuando han muerto todos los enemigos y abre las puertas de la habitación.

<!-- LevelGeneration.cs -->
### LevelGeneration.cs
Genera las habitaciones del nivel de forma aleatoria.
Al ir generando cada habitación, se asigna en que dirección va a estar la siguiente, y se le pasa esa información al BoardManager para instanciar cada caso.


<!-- SoundManager.cs -->
### SoundManager.cs
Clase para con los metodos necesarios para implementar sonidos y musica al juego.


<!-- Utilities.cs -->
### Utilities.cs
Libreria con varios metodos para facilitar el trabajo en el proyecto(busqueda por tag, busqueda de hijos,obtener enumerables de objetos de tipo generico,etc).

<!-- BoardRoom.cs -->
### BoardRoom.cs
Cuando se instancia una habitacion en el BoardManager, se genera una instancia de esta clase.
Gestiona y controla todo lo que incluye la habitación.
Pausar o empezar las rutinas de los enemigos.
Controlar cuando se completa la habitación para abrir las puertas.
Asignacion de entrada y salida segun el sentido en el que este moviendose el jugador.
Invocar los enemigos.
Dar la posicion donde aparece el jugador.

<!-- EventRoomController.cs -->
### EventRoomController.cs
Se asigna para cada habitación, que tipo de desafio se encontrará el jugador , bien enemigos , o algun tipo de desafio de puzzle.
En caso de enemigos , se asigna dinamicamente una cantidad de enemigos por dificultad .
En caso de puzzle, se asigna de forma dinamica algunos de los diseños pregenerados anteriormente.
Actualmente en desarrollo.


<!-- HealthManager.cs -->
### HealthManager.cs
Gestión de comunicar los distintos sistemas que provocan cambios en la vida del jugador y enemigos, y traducirlo en cambios en la UI.



<!-- CONTACT -->
## Contact

Clovis - [@Clovisindo](https://twitter.com/clovisindo) 




<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->
[contributors-shield]: https://img.shields.io/github/contributors/github_username/repo.svg?style=for-the-badge
[contributors-url]: https://github.com/github_username/repo/graphs/contributors
[forks-shield]: https://img.shields.io/github/forks/github_username/repo.svg?style=for-the-badge
[forks-url]: https://github.com/github_username/repo/network/members
[stars-shield]: https://img.shields.io/github/stars/github_username/repo.svg?style=for-the-badge
[stars-url]: https://github.com/github_username/repo/stargazers
[issues-shield]: https://img.shields.io/github/issues/github_username/repo.svg?style=for-the-badge
[issues-url]: https://github.com/github_username/repo/issues
[license-shield]: https://img.shields.io/github/license/github_username/repo.svg?style=for-the-badge
[license-url]: https://github.com/github_username/repo/blob/master/LICENSE.txt
[linkedin-shield]: https://img.shields.io/badge/-LinkedIn-black.svg?style=for-the-badge&logo=linkedin&colorB=555
[linkedin-url]: https://linkedin.com/in/github_username
