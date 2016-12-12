package master;
import java.io.*;
import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;

public class Sonda {
	private String volumen;
	private String fechaUltimoCambio;
	private String led;
	private String id;
	private String ficheroAsociado = "sonda.txt";
	private String key;
	
	public Sonda(int id) {
		this.id = Integer.toString(id);
		leerSensor(ficheroAsociado);
		leerKey();
	}
	
	public String GetVolumen() {
		String mensaje = "";
		
		return mensaje;
	}
	
	public String getFechaActual() {
		DateTimeFormatter formato = DateTimeFormatter.ofPattern("dd/MM/yyyy HH:mm:ss");
		LocalDateTime now = LocalDateTime.now();
		String fechaActual = now.format(formato);
		return fechaActual;
	}
	
	public String GetLed() {
		return led;
	}
	
	public void SetLed( String valor) {
		String valorAnterior = led;
		led = valor;
		//Falta escribir en el fichero
	}
	
	private void leerKey() {
		//Lee el archivo key controlando excepciones
		File fichero = new File("key.txt");
		if(fichero.exists()) {
			try {
				FileReader fr = new FileReader(fichero);
				BufferedReader br = new BufferedReader(fr);
				String linea = br.readLine();
				key = linea;
				br.close();
				fr.close();
			}catch (Exception e){
				System.out.println("Error estableciendo la clave de cifrado: fichero corrupto (" + e.toString() + ")");
			}
		} else {
			System.out.println("Error: el fichero \"key.txt\" no se encuentra");
		}
	}
	
	private void leerSensor(String archivo) {
		File fichero = new File(archivo);
		if(fichero.exists()) {
			try {
				FileReader fr = new FileReader(fichero);
				BufferedReader br = new BufferedReader(fr);
				String linea;
				while((linea = br.readLine()) != null) {
					switch(linea.toLowerCase().split("=")[0]) {
					case "volumen":
						volumen = linea.split("=")[1];
						break;
					case "ultimafecha":
						fechaUltimoCambio = linea.split("=")[1];
						break;
					case "led":
						led = linea.split("=")[1];
						break;
					}
				}
				br.close();
				fr.close();
			} catch(Exception e) {
				System.out.println("Error al leer el fichero del sensor: " + e.toString());
			}
		}
		else {
			try {
				PrintWriter writer = new PrintWriter("sonda.txt", "UTF-8");
				writer.println("Volumen=30");
				writer.println("UltimaFecha=20/09/2016 15:30:26");
			    writer.println("Led=4500");
			    writer.close();
			    leerSensor(ficheroAsociado);
			} catch(Exception e) {
				System.out.println("Error creando el fichero por defecto del sensor " + id + ": " + e.toString());
			}
		}	
	}
}
