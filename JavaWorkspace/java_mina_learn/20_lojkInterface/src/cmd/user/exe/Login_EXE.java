package cmd.user.exe;

import com.xxxxx.server.client.Client;
import cmd.user.Login;
import cmd.user.LoginResult;
import java.util.logging.Level;
import java.util.logging.Logger;

public class Login_EXE
{

	public static void execute(Login login, Client client)
	{
		String username = login.getUsername();
		String password = login.getPassword();
		System.out.println(username + " " + password);

		//¼ìË÷Êý¾Ý¿âÁË£¡
		if (username.equals(password))
		{
			client.getSession().write(
					LoginResult.newBuilder()
							.setResult(LoginResult.Result.SUCCESS).build());
		} else
		{
			client.getSession().write(
					LoginResult.newBuilder()
							.setResult(LoginResult.Result.FAILURE).build());
		}
	}
}
