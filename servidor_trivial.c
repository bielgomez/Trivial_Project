#include <stdio.h>
#include <string.h>
#include <stdlib.h> //Necesario para atof
#include <mysql.h>
#include <sys/types.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <unistd.h>
#include <ctype.h>

int main(int argc, char *argv[]) {
	
	int sock_conn, sock_listen, ret;
	struct sockaddr_in serv_adr;
	char buff[512];
	char buff2[512];
	// INICIALITZACIONES
	// Abrimos el socket
	if ((sock_listen = socket(AF_INET, SOCK_STREAM, 0)) < 0)
		printf("Error creant socket");
	// Hacemos el bind al puerto
	
	
	memset(&serv_adr, 0, sizeof(serv_adr));// inicializa a zero serv_addr
	serv_adr.sin_family = AF_INET;
	
	// asocia el socket a cualquiera de las IP de la maquina. 
	//htonl formatea el numero que recibe al formato necesario
	serv_adr.sin_addr.s_addr = htonl(INADDR_ANY);
	// escucharemos en el port 9020
	serv_adr.sin_port = htons(9020);
	if (bind(sock_listen, (struct sockaddr *) &serv_adr, sizeof(serv_adr)) < 0)
		printf ("Error al bind");
	//La cola de peticiones pendientes no podr? ser superior a 4
	if (listen(sock_listen, 2) < 0)
		printf("Error en el Listen");
	
	// Bucle infinito
	for(;;){
		printf ("Escuchando\n");
		
		sock_conn = accept(sock_listen, NULL, NULL);
		printf ("He recibido conexi?n\n");
		//sock_conn es el socket que usaremos para este cliente
		
		//Creamos la connexión a la BBDD
		MYSQL *conn;
		MYSQL_RES *resultado;
		MYSQL_ROW row;
		int err;
		conn = mysql_init(NULL);
		if (conn == NULL){
			printf("Error al crear la connexión: %u %s\n",mysql_errno(conn),mysql_error(conn));
			exit(1);
		}
		
		conn = mysql_real_connect(conn,"localhost","root","mysql","trivial_BBDD",0,NULL,0);
		if (conn==NULL){
			printf("Error al crear la connexión: %u %s\n",mysql_errno(conn),mysql_error(conn));
			exit(1);
		}
		
		//Variable para saber si se tiene que desconectar
		int terminar = 0;
		while (terminar==0)
		{
			// Informacion recibida almacenada en buff
			ret=read(sock_conn,buff, sizeof(buff));
			printf ("Recibido\n");
			
			// Tenemos que añadirle la marca de fin de string 
			// para que no escriba lo que hay despues en el buffer
			buff[ret]='\0';
			
			//Escribimos en consola lo que nos ha llegado (buff)
			printf("Se ha conectado: %s\n",buff);
			
			//Obtenemos el codigo que nos indica el tipo de petición.
			char *p = strtok(buff,"/");
			int codigo = atoi(p);
			printf("%d\n",codigo);
			
			
			//Codigo 0 --> Desconexión
			if (codigo == 0){
				//Mensaje en buff: 0/
				//Return en buff2: -
				terminar = 1;
			}
			
			//Codigo 1 --> Comprovación para el Login
			else {
				if (codigo == 1){
					//Mesnaje en buff: 1/username/contrasenya
					//Return en buff2: 0--> Todo OK ; 1 --> Usuario no existe ; 2 --> Contrasenya no coincide ; -1 --> Error de consulta
					
					char nombre[25];
					char contrasenya[20];
					
					p = strtok(NULL,"/");
					strcpy(nombre,p);
					p = strtok(NULL,"/");
					strcpy(contrasenya,p);
										
					//Consulta
					char consulta[80];
					strcpy(consulta,"SELECT contrasenya FROM jugadores WHERE nombre='");
					strcat(consulta,nombre);
					strcat(consulta,"'");
					err=mysql_query(conn,consulta);
					if (err != 0){
						printf("Error al consultar la BBDD %u %s",mysql_errno(conn),mysql_error(conn));
						strcpy(buff2, "-1");
					}
					else{
						//Recibimos el resultado de la consulta
						resultado = mysql_store_result(conn);
						row = mysql_fetch_row(resultado);
						if (row == NULL)
							strcpy(buff2,"1"); //error 1
						else
						{
							printf("%s\n",row[0]);
							if (strcmp(contrasenya,row[0])==0)
								strcpy(buff2,"0"); 
							else
								strcpy(buff2,"2"); //error 2
						}
					}
				}
				
				//Codigo 2 --> Insert de nuevos jugadores
				else if (codigo==2){ 
					//Mensaje en buff: 2/username/contrasenya/mail
					//Return en buff2: 0--> Todo OK ; 1 --> Usuario ya existente ; -1 --> Error de consulta
					
					char nombre[25];
					char contrasenya[20];
					char mail[100];
					
					p = strtok(NULL, "/");
					strcpy(nombre, p);
					p = strtok (NULL, "/");
					strcpy (contrasenya, p);
					p = strtok(NULL,"/");
					strcpy(mail, p);
					
					//Busca si ya hay un usuario con ese nombre
					char consulta[80];
					strcpy(consulta, "SELECT nombre FROM jugadores WHERE nombre='");
					strcat(consulta, nombre);
					strcat(consulta, "'");
					
					err = mysql_query(conn, consulta);
					if (err!=0)
						strcpy(buff2, "-1");
					
					else {
						resultado = mysql_store_result(conn);
						row =  mysql_fetch_row(resultado);
						
						//Se registra si no hay nadie con el mismo nombre
						if (row==NULL) {
							
							err=mysql_query(conn, "SELECT * FROM jugadores WHERE id=(SELECT max(id) FROM jugadores);");
							if (err!=0){
								strcpy(buff2, "-1");
								printf("NO fa SELECT\n");
							}
							resultado = mysql_store_result(conn);
							row =  mysql_fetch_row(resultado);
							printf("%s\n",row[0]);
							int id = atoi(row[0])+1;
							sprintf(consulta,"INSERT INTO jugadores VALUES ('%d','%s','%s','%s');", id, nombre, contrasenya, mail);
							printf("%s\n",consulta);
							err= mysql_query(conn, consulta);
							if (err!=0)
								strcpy(buff2, "-1");						
							else
								strcpy(buff2, "0"); 
						}
						else
							strcpy(buff2, "1");  
						
					}
				}
				
				
				//Codigo 3 --> Recuperar contraseña 
				else if (codigo == 3){
					//Mensaje en buff: 3/usuario
					//Return en buff2: contrasenya --> Todo OK ; 1--> No hay usuario ; -1 --> Error de consulta
					
					char nombre[20];
					p = strtok(NULL,"/");
					strcpy(nombre,p);
					
					char consulta[80];
					strcpy(consulta,"SELECT contrasenya FROM jugadores WHERE nombre='");
					strcat(consulta,nombre);
					strcat(consulta,"'");
					
					err=mysql_query(conn,consulta);
					if (err!=0)
						strcpy(buff2,"-1");
					else{
						resultado = mysql_store_result(conn);
						row = mysql_fetch_row(resultado);
						
						if (row == NULL)
							strcpy(buff2,"1"); //No hay ningun usuario con ese nombre
						else
							strcpy(buff2,row[0]); //No hay error, devolvemos la contraseña obtenida
					}
				}
				
				//Codigo 4 --> Obtener la partida mas larga 
				else if (codigo == 4){
					//Mensaje en buff: 4/
					//Return en buff2: idP partida mas larga --> Todo OK ; -1 --> Error de consulta ; -2 --> No hay partidas en BBDD
					
					//No tenemos ningun input por tanto procedemos a hacer la consulta directamente
					err=mysql_query (conn, "SELECT partidas.id FROM partidas WHERE partidas.duracion = (SELECT MAX(partidas.duracion) FROM partidas)");
					if (err!=0)
						strcpy(buff2,"-1");
					else{
						resultado = mysql_store_result(conn);
						row = mysql_fetch_row(resultado);
						
						if (row == NULL)
							strcpy(buff2,"-2"); 
						else
							strcpy(buff2,row[0]); 
					}
				}
				
				//Codigo 5 --> Obtener jugador con más puntos
				else {
					//Mensaje en buff: 5/
					//Return en buff 2: nombre jugador con mas puntos --> Todo OK ; -1 --> Error de consulta ; -2 --> No hay jugadores en BBDD
					
					//No tenemos ningun input por tanto procedemos a hacer la consulta directamente
					err=mysql_query (conn, "SELECT jugadores.nombre FROM (jugadores, registro) WHERE registro.puntos=(SELECT MAX(registro.puntos) FROM registro) AND registro.idJ=jugadores.id");
					if (err!=0)
						strcpy(buff2,"-1");
					else{
						resultado = mysql_store_result(conn);
						row = mysql_fetch_row(resultado);
						
						if (row == NULL)
							strcpy(buff2,"-2"); 
						else
							strcpy(buff2,row[0]); 
					}
				}
				// Y lo enviamos
				write (sock_conn,buff2, strlen(buff2));
				printf("Codigo: %d , Resultado: %s\n",codigo,buff2); //Vemos el resultado de la accion.
			}
		
		}
		
		//Cerramos conexion a la BBDD
		mysql_close(conn);
		// Se acabo el servicio para este cliente
		close(sock_conn);
		
	}
	

	exit(0);
}

