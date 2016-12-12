package master;

import java.io.UnsupportedEncodingException;
import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;
import java.util.Arrays;
import javax.crypto.Cipher;
import javax.crypto.spec.SecretKeySpec;
import org.apache.commons.codec.binary.Base64;

public class Encriptador {
	private SecretKeySpec secretKey;
	private byte[] key;
	private String decryptedString;
	private String encryptedString;
	
	public void setKey(String p_key) {
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
	
	 public void setDecryptedString(String decryptedString) {
		 this.decryptedString = decryptedString;
	 }
	 
	 public void setEncryptedString(String encryptedString) {
		 this.encryptedString = encryptedString;
	 }
	 
	 public String encrypt(String cadena) {
		 try {
			 Cipher cipher = Cipher.getInstance("AES/ECB/PKCS5Padding");
			 cipher.init(Cipher.ENCRYPT_MODE, secretKey);
			 setEncryptedString(Base64.);
		 }
	 }
}
