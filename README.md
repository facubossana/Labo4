# Labo4

Éste es el proyecto final para la materia Laboratorio de computación 4 de la carrera Tecnicatura Universitaria en Programación.<br><br>
Para poder utilizarlo, lo primero que debemos hacer luego de clonarlo, es abrir la "consola del Administrador de paquetes", y luego escribir los comandos ADD-MIGRATION Inicio (o cualquier nombre aleatorio que desee)
y finalmente realizar un UPDATE-DATABASE, para que la base de datos se suba al explorador de objetos de SQL server.  <br>
Una vez realizado lo anterior, procedemos a ejecutar el proyecto y creamos un usuario para empezar a manipular el proyecto. Si desea crear un usuario con rol de administrador, le recomiendo que utilice el que está en 
"SpeedData.cs" con los datos que se encuentran en las lineas 48 y 51 respectivamente.
Cuando creamos los usuarios y queremos darle un rol de administrador, lo que necesitamos hacer es abrir el explorador de objetos de SQL Server.
Una vez allí dentro hacemos los siguietes pasos: <br>
  1-(localdb)\MSSQLLocalDB -> Bases de datos -> FinalLabo4 (o nombre de la tabla que le colocamos en el appsettings.json) -> Tablas. <br>
  2- Abrimos dbo.AspNetUsers con click derecho y "ver datos". Allí copiamos el Id del usuario que queremos convertir en administrador. <br>
  3- Al igual que antes, abrimos dbo.AspNetRoles para verificar que el rol de administrador corresponde al Id 1 (uno). <br>
  4- Del mismo modo que el paso anterior abrimos dbo.AspNetUserRoles y allí pegamos el Id copiado anteriormente en "UserId" y en "RoleId" colocamos 1 (uno) y presionamos la tecla enter.
Una vez realizado todos esos pasos, procedemos a ejecutar otra vez el proyecto e ingresamos con el usuario que declaramos administrador y veremos las diferencias entre un usuario común y 
el usuario con rol administrativo.
