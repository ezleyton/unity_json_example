# Unity JSON Example

EJemplo de como usar Json en Unity con JsonUtils (built in)

## Funcionamiento
Disponemos de un personaje el cual se puede mover, podemos guardar su posicion y rotacion, moverlo nuevamente y volver a cargar la ultima posicion y rotacion registradas.
Existen dos modos de serializacion, Class y Structure, que se pueden indicar en el componente Player del jugador. Class utiliza una clase para serializar los datos, y Structure un struct.
Cada vez que se registra una posicion, se guarda la misma en un archivo (en la raiz del proyecto, con los nombres Class o Structure dependiendo del modo de serializacion elegido). Al iniciar el juego se verifica la existencia de estos archivos segun el modo elegido, y si se encuentra el mismo se actualiza la posicion del jugador.

## Teclas
* WSAD: Movimiento del personaje
* Barra: Guardar posicion y rotacion actual
* R: Cargar ultima posicion y rotacion guardadas
