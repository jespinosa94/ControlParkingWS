package master;
import java.io.*;
import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;

public class Sonda {
	private String volumen;
	private String fechaUltimoCambio;
	private String led;
	private String id;
	private String ficheroAsociado = "\\src\\master\\sonda.txt";
	private String key;
	
	public Sonda(int id) {
		this.id = Integer.toString(id);
		leerSensor(ficheroAsociado);
		leerKey();
	}
	
	public String GetVolumen() {
		String mensaje = "";
		Encriptador crypt = new Encriptador();
		
		try {
			mensaje = crypt.encrypt(volumen, key);
		} catch(Exception e) {
			System.out.println("Error al encriptar el volumen: " + e.toString());
		}
		return mensaje;
	}
	
	public String getFechaActual() {
		Encriptador crypt = new Encriptador();
		String mensaje = "";
		DateTimeFormatter formato = DateTimeFormatter.ofPattern("dd/MM/yyyy HH:mm:ss");
		LocalDateTime now = LocalDateTime.now();
		String fechaActual = now.format(formato);
		
		try {
			mensaje = crypt.encrypt(fechaActual, key);
		} catch (Exception e) {
			System.out.println("Error al encriptar la fecha actual: " + e.toString());
		}
		return mensaje;
	}
	
	public String GetLed() {
		Encriptador crypt = new Encriptador();
		String mensaje = "";
		
		try {
			mensaje = crypt.encrypt(led, key);
		} catch(Exception e) {
			System.out.println("Error al encriptar el led: " + e.toString());
		}
		return mensaje;
	}
	
	public void SetLed( String valor) {
		String valorAnterior = led;
		led = valor;
		//Falta escribir en el fichero
	}
	
	private void leerKey() {
		File fichero = new File(System.getProperty("user.dir") + "\\src\\master\\key.txt");
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
			System.out.println("Error: el fichero \"key.txt\" no se encuentra" + fichero);
		}
	}
	
	private void leerSensor(String archivo) {
		File fichero = new File(System.getProperty("user.dir") + archivo);
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
	
	public void escribeFichero(String valorAnterior, String nuevoValor, String nombreFichero) {
		File fichero = new File(System.getProperty("user.dir") + nombreFichero);
		String lectura = "";	
		
		try {
	        FileReader fr = new FileReader(fichero);
	        BufferedReader br = new BufferedReader(fr);

	        String linea;
	        while((linea = br.readLine()) != null) {
	          if(linea.contains(valorAnterior)) {
	            linea = linea.replace(valorAnterior, nuevoValor);
	          }
	          lectura += linea + ",";
	        }
	        br.close();
	        fr.close();
	        
	        String[] escritura = lectura.split(",");
	        FileWriter fw = new FileWriter(fichero, false);
	        for(String s: escritura) {
	          fw.write(s);
	          fw.write("\n");
	          fw.flush();
	        }

	        fw.close();
		} catch(Exception e) {
			System.out.println("Error haciendo Set de la luz del sensor: " + e.toString());
		}
}
}
