package master;

import master.Encriptador;
import master.Sonda;
import java.io.BufferedReader;
import java.io.File;
import java.io.FileReader;
import java.io.IOException;
import java.util.Scanner;


public class Main {

	private static String leerKey() throws IOException {
		//Lee el archivo key controlando excepciones
		String resultado = "";
		 String current = new java.io.File( "." ).getCanonicalPath();
		File fichero = new File(current + "\\src\\master\\key.txt");
		if(fichero.exists()) {
			try {
				FileReader fr = new FileReader(fichero);
				BufferedReader br = new BufferedReader(fr);
				String linea = br.readLine();
				resultado = linea;
				br.close();
				fr.close();
			}catch (Exception e){
				System.out.println("Error estableciendo la clave de cifrado: fichero corrupto (" + e.toString() + ")");
			}
		} else {
			System.out.println("Error: el fichero " + fichero + " no se encuentra");
		}
		return resultado;
	}
	
	public static void main(String[] args) throws IOException {
		/**Scanner reader = new Scanner(System.in);
		final String key = leerKey();
		System.out.println("Key: " + key);
		String cadena = "";
		String resultado = "";
		Encriptador crypt = new Encriptador();
		
		System.out.println("Escribe la cadena a encriptar: ");
		cadena = reader.nextLine();
		resultado = crypt.encrypt(cadena.trim(), key);
		System.out.println("Texto sin encriptar: " + cadena);
		System.out.println("Texto encriptado: " + resultado);
		System.out.println("Texto desencriptado: " + crypt.decrypt(resultado, key));**/
		
		Encriptador crypt = new Encriptador();
		final String key = leerKey();
		Sonda sonda3 = new Sonda(3);
		
		
		System.out.println(crypt.decrypt(sonda3.GetVolumen(), key));
		
	}
}