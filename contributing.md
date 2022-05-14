# Unity2dRogueLike
Repositorio juego unity2d Roguelike 


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

Inicio este proyecto personal de aprendizaje en Unity, con objetivo de trabajar todo el ciclo de desarrollo de principio a fin de un videojuego.
El genero a explorar es el roguelike, en la experiencia mas pequeña posible, pero poder practicar y diseñar mecanicas de este genero de forma práctica.


<!-- Diagrama de sistemas -->
## Diagrama de sistemas
Esquema de como se comunican los distintos sistemas entre si en el loop del juego.
![image](https://user-images.githubusercontent.com/4136363/128236435-6176c978-d328-41c2-ada0-4522c3532a47.png)


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

![image](https://user-images.githubusercontent.com/4136363/112748154-4b015680-8fba-11eb-9a43-322eb038e99b.png)
![image](https://user-images.githubusercontent.com/4136363/112748174-63717100-8fba-11eb-95d8-a97bc4f71a1b.png)



<!-- Weapon.cs -->
### Weapon.cs
Clase abstracta que implementa metodos para implementar armas especificas.
Controles de uso del arma, con ataque normal y especial.
Ataque direcional.

*Nueva herramienta para abrir puertas secretas

Armas implementadas: espadón, espada de caballero , escudo de caballero, martillo gigante.

![image](https://user-images.githubusercontent.com/4136363/112748203-90be1f00-8fba-11eb-8f0d-64bdd4e546d6.png)
![image](https://user-images.githubusercontent.com/4136363/112748211-9d427780-8fba-11eb-9600-c93a5e63c69c.png)
![image](https://user-images.githubusercontent.com/4136363/112748218-a59ab280-8fba-11eb-86ed-02dcf25ae237.png)


<!-- Projectile.cs -->
### Projectile.cs
Implementa el comportamiento de disparar un projectil al objetivo.
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

Mimico: un cofre que al acercarte se descubre como un enemigo, si el jugador se aleja demasiado, vuelve a la posicion de inicio y vuelve a ser un cofre. 

Goblin: pequeña y rapida criatura que persigue al jugador para atacarle en corta distancia, una vez impacta, se aleja de él un tiempo para volver. 

Ogro: enemigo grande y lento que golpea en area. 

Orco enmascarado: persigue al jugador haciendo fintas para engañarlo. 

Orco chaman: dispara projectiles cuando el jugador se acerca. 

Orco guerrero: carga en dirección al jugador. 

![image](https://user-images.githubusercontent.com/4136363/112749270-88b5ad80-8fc1-11eb-8d4c-1efb453d6c3a.png)
![image](https://user-images.githubusercontent.com/4136363/112749276-910de880-8fc1-11eb-925a-325c800de7ca.png)
![image](https://user-images.githubusercontent.com/4136363/112749282-a125c800-8fc1-11eb-983b-241f1f1ecf48.png)
![image](https://user-images.githubusercontent.com/4136363/112749287-b3076b00-8fc1-11eb-8478-e16d1cfafae5.png)
![image](https://user-images.githubusercontent.com/4136363/112749294-c0bcf080-8fc1-11eb-8487-02e4986d844d.png)
![image](https://user-images.githubusercontent.com/4136363/112749302-d6321a80-8fc1-11eb-98d5-cecc194f9954.png)
![image](https://user-images.githubusercontent.com/4136363/112749334-011c6e80-8fc2-11eb-8b39-4b815cb6c61f.png)




<!-- BoardManager.cs -->
### BoardManager.cs
Clase que gestiona la creación de cada habitación casilla por casilla.
Ademas de generar un modelo basico de habitacion ractangular rodeada de paredes, genera las puertas de entrada y salida(camino principal) que se le indica a la hora de crear la habitación.

*El camino secundario se actualiza después de esta primera instancia, por lo tanto las puertas que no se definan, quedan instanciadas como "NoDoor" y posteriormente se puede parametrizar para ser funcionales.
![image](https://user-images.githubusercontent.com/4136363/112749360-2315f100-8fc2-11eb-9159-53465696d409.png)


<!-- GameManager.cs -->
### GameManager.cs
Implementa distintas funciones de comunicación entre los sistemas del juego.
Carga y gestiona las habitaciones generadas:
Cambiar la habitacion actual(BoardRoom)
Generar los enemigos o puzzles en cada habitación(EventRoomController)
Cambio de habitación al cruzar puertas( cambia la camara , mueve al jugador , activa los enemigos o eventos de esa habitación).
Comprueba cuando han muerto todos los enemigos y abre las puertas de la habitación(BoardRoom y FRoomDoor).

<!-- LevelGeneration.cs -->
### LevelGeneration.cs
Genera las habitaciones del nivel de forma aleatoria.
Al ir generando cada habitación, se asigna en que dirección va a estar la siguiente, y se le pasa esa información al BoardManager para instanciar cada caso.
En cada paso, y decidido por unos parametros previos, comprueba si es posible crear una habitación como camino secundario, y guarda esto para instanciarlo al acabar de generar el camino principal.
![image](https://user-images.githubusercontent.com/4136363/112749390-62444200-8fc2-11eb-806b-81567c7d7ff3.png)


<!-- BoardRoom.cs -->
### BoardRoom.cs
Cuando se instancia una habitacion en el BoardManager, se genera una instancia de esta clase.
Gestiona y controla todo lo que incluye la habitación.
Pausar o empezar las rutinas de los enemigos(EventRoomController).
Controlar cuando se completa la habitación para abrir las puertas.
Asignacion de entrada y salida segun el sentido en el que este moviendose el jugador.
Invocar los enemigos(EventRoomController).
Dar la posicion donde aparece el jugador.


<!-- EventRoomController.cs -->
### EventRoomController.cs
Se asigna para cada habitación, que tipo de desafio se encontrará el jugador , bien enemigos , o algun tipo de desafio de puzzle.
En caso de enemigos , se asigna dinamicamente una cantidad de enemigos por dificultad .
En caso de puzzle, se asigna de forma dinamica algunos de los diseños pregenerados anteriormente.
Actualmente en desarrollo.

<!-- FfloorMechanic.cs -->
### FfloorMechanic.cs
Clase abstracta que implementa metodos de activar mecanismos genericos para los distintos objetos que se quieran implementar.
Actualmente estan integrados palancas, botones, enemigos ocultos, pociones, fuentes curativas, trampas y casillas de caida al vacio.

![image](https://user-images.githubusercontent.com/4136363/112749741-adf7eb00-8fc4-11eb-9dcc-922203c8a0a1.png)
![image](https://user-images.githubusercontent.com/4136363/112749608-ab48c600-8fc3-11eb-8829-72196f00b619.png)
![image](https://user-images.githubusercontent.com/4136363/112749614-c1568680-8fc3-11eb-9537-ff8a67b4695b.png)
![image](https://user-images.githubusercontent.com/4136363/112749638-e6e39000-8fc3-11eb-9175-81e52ecadcbc.png)
![image](https://user-images.githubusercontent.com/4136363/112749644-f06cf800-8fc3-11eb-8874-585121623a5a.png)
![image](https://user-images.githubusercontent.com/4136363/112749647-f8c53300-8fc3-11eb-8eb1-9042901d21db.png)

<!-- FRoomDoor.cs -->
### FRoomDoor.cs
Clase que gestiona los objetos que conforman el conjunto de cada puerta, para unificar y simplicar todas las funciones de puertas (abrir,cerrar,actualizar) como sus parametros (tipo de puerta, etc).

<!-- SoundManager.cs -->
### SoundManager.cs
Clase con los metodos necesarios para implementar sonidos y musica al juego.

<!-- Utilities.cs -->
### Utilities.cs
Libreria con varios metodos para facilitar el trabajo en el proyecto(busqueda por tag, busqueda de hijos,obtener enumerables de objetos de tipo generico,etc).

<!-- HealthManager.cs -->
### HealthManager.cs
Gestión de comunicar los distintos sistemas que provocan cambios en la vida del jugador y enemigos, y traducirlo en cambios en la UI.
![image](https://user-images.githubusercontent.com/4136363/112749654-04185e80-8fc4-11eb-841b-ea772c8032a9.png)
![image](https://user-images.githubusercontent.com/4136363/112749663-0c709980-8fc4-11eb-85c7-348f8dfd02a3.png)



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
