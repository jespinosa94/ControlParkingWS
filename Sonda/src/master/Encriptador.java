package master;

/****import java.io.UnsupportedEncodingException;
import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;
import java.util.Arrays;
import javax.crypto.Cipher;
import javax.crypto.spec.SecretKeySpec;
import org.apache.commons.codec.binary.Base64;
import java.util.Base64.*;****/

import java.io.UnsupportedEncodingException;
import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;
import java.util.Arrays;
import java.util.Base64;
import javax.crypto.Cipher;
import javax.crypto.spec.SecretKeySpec;

public class Encriptador {
	private static SecretKeySpec secretKey;
	private static byte[] key;
	
	public static void setKey(String p_key) {
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
	
	public static String encrypt(String cadena, String clave) {
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
	
	public static String decrypt(String cadena, String clave) {
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
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	/****private static SecretKeySpec secretKey;
	private static byte[] key;
	private static String decryptedString;
	private static String encryptedString;
	
	public static void setKey(String p_key) {
		MessageDigest sha = null;
		try {
            key = p_key.getBytes("UTF-8");
            System.out.println(key.length);
            sha = MessageDigest.getInstance("SHA-1");
            key = sha.digest(key);
            key = Arrays.copyOf(key, 16); // use only first 128 bit
            System.out.println(key.length);
            System.out.println(new String(key,"UTF-8"));
            secretKey = new SecretKeySpec(key, "AES");
		} catch (NoSuchAlgorithmException e) {
            e.printStackTrace();
        } catch (UnsupportedEncodingException e) {
            e.printStackTrace();
        }
	}
	
	public String getDecryptedString() {
		return decryptedString;
	}
	
	 public static void setDecryptedString(String p_decryptedString) {
		 decryptedString = p_decryptedString;
	 }
	 
	 public static void setEncryptedString(String p_encryptedString) {
		 encryptedString = p_encryptedString;
	 }
	 
	 public static String encrypt(String cadena) {
		 String resultado = "";
		 try {
			 Cipher cipher = Cipher.getInstance("AES/ECB/PKCS5Padding");
			 cipher.init(Cipher.ENCRYPT_MODE, secretKey);
			 resultado = Base64.encodeBase64(cipher.doFinal(cadena.getBytes("UTF-8"))).toString();
			 setEncryptedString(resultado);
		 } catch(Exception e) {
			 System.out.println("Error encriptando los datos: " + e.toString());
		 }
		 return resultado;
	 }
	 
	 public static String decrypt(String cadenaEncriptada) {
		 String resultado = "";
		 try {
			 Cipher cipher = Cipher.getInstance("AES/ECB/PKCS5PADDING");
			 cipher.init(Cipher.DECRYPT_MODE, secretKey);
			 resultado = new String(cipher.doFinal(Base64.decodeBase64(cadenaEncriptada.getBytes("UTF-8"))));
			 setDecryptedString(resultado);
		 } catch(Exception e) {
			 System.out.println("Error desencriptando los datos: " + e.toString());
		 }
		 return resultado;
	 }****/
}
