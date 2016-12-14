package master;

import java.io.UnsupportedEncodingException;
import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;
import java.util.Arrays;
import java.util.Base64;
import javax.crypto.Cipher;
import javax.crypto.spec.SecretKeySpec;

public class Encriptador {
	private SecretKeySpec secretKey;
	private byte[] key;
	
	public void setKey(String p_key) {
		MessageDigest sha = null;
		
		try {
			key = p_key.getBytes("UTF-8");
			sha = MessageDigest.getInstance("SHA-1");
			key = sha.digest(key);
			key = Arrays.copyOf(key, 16); //Necesario para longitud de la clave AES
			secretKey = new SecretKeySpec(key, "AES");
		}
		catch (NoSuchAlgorithmException e) {
            e.printStackTrace();
        } 
        catch (UnsupportedEncodingException e) {
            e.printStackTrace();
        }
	}
	
	public String encrypt(String cadena, String clave) {
		try {
			setKey(clave);
			Cipher cipher = Cipher.getInstance("AES/ECB/PKCS5Padding");
			cipher.init(Cipher.ENCRYPT_MODE, secretKey);
			return Base64.getEncoder().encodeToString(cipher.doFinal(cadena.getBytes("UTF-8")));
		}
		catch (Exception e) {
			System.out.println("Error encriptando: " + e.toString());
		}
		return null;
	}
	
	public String decrypt(String cadena, String clave) {
		try {
			setKey(clave);
			Cipher cipher = Cipher.getInstance("AES/ECB/PKCS5Padding");
			cipher.init(Cipher.DECRYPT_MODE, secretKey);
			return new String(cipher.doFinal(Base64.getDecoder().decode(cadena)));
		}
		catch (Exception e) {
			System.out.println("Error en la desencriptación: " + e.toString());
		}
		return null;
	}
}
