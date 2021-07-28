using System.Collections;
using System.Collections.Generic;
using System.Numerics;

public static class MoneyFormatter 
{
	//500        = 500
	//5000       = 5k
	//200 000    = 200K
	//6000000    = 6M
	//1000000000 = 1B


	public static string FormatMoney(BigInteger value)
	{
		//default money format
		string moneyFormat = "{0}";

 
		//format the money for billions
		if (value >= 1000000000)
		{
			moneyFormat = "{0:#,0,,, B}"; 
			//format the money for millions 
		}
		else if(value >= 1000000)
		{
			moneyFormat = "{0:#,0,, M}"; // 1M
			//format the money for thousands 
		}
		else if (value >= 1000)
		{
			moneyFormat = "{0:#,0, K}"; //2000 -> 2K
		}

		//return the formatted money
		return string.Format(moneyFormat + "€", value);
	}
}
