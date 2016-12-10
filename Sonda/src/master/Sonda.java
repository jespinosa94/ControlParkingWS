package master;
import java.io.*;
import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;

public class Sonda {
	private String volumen = "30";
	private String fechaUltimoCambio = "20/09/2016 15:30:26";
	private String led = "4500";
	private String id = "0";
	private String ficheroAsociado;
	
	public Sonda() {
		
	}
	
	public String GetVolumen() {
		return volumen;
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
}
