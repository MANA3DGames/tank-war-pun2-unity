
using UnityEngine;

namespace MANA3DGames
{
	public static class StringUtil
	{
		// ***************************************************************
		// Adds decimal mark to integer value and returns string.
		// ***************************************************************
		public static string AddCommaToNumber( int val )
		{
			// Decimal mark counter.
			int counter = 3;
			
			// Convert val to string.
			string valStr = val + "";
			
			// Check if we have more than 3 digits.
			if ( valStr.Length > 3 )
			{
				// Keep adding comma as long as we have enough digits.
				while ( valStr.Length > counter )
				{
					// Insert a comma after the next 3 digists.
					valStr = valStr.Insert( valStr.Length - counter, "," );
					
					// Increment the counter with 4 (0,000).
					counter += 4;
				}
			}
			
			// Return the final string with decimal mark(s).
			return valStr;
		}

		public static string AddCommaToNumber<T>( ref T val )
		{
			// Decimal mark counter.
			int counter = 3;

			// Convert val to string.
			string valStr = System.Convert.ChangeType( val, typeof(int) ) + "";

			// Check if we have more than 3 digits.
			if ( valStr.Length > 3 )
			{
				// Keep adding comma as long as we have enough digits.
				while ( valStr.Length > counter )
				{
					// Insert a comma after the next 3 digists.
					valStr = valStr.Insert( valStr.Length - counter, "," );

					// Increment the counter with 4 (0,000).
					counter += 4;
				}
			}

			// Return the final string with decimal mark(s).
			return valStr;
		}


        public static string AddDecimalPoint_One( int val )
		{
            if ( val < 10 )
                return "0." + val;

			// Convert val to string.
			string valStr = val.ToString();
		    
			valStr = valStr.Insert( valStr.Length - 1, "." );

			// Return the final string with decimal mark(s).
			return valStr;
		}

        public static string GetTimeString( int seconds )
        {
            string str = string.Empty;

            int hours = seconds / 3600;
            if ( hours >= 1 )
                str = hours + ":";
            else
                str = "00:";

            int mintues = seconds % 3600;
            if ( mintues >= 1 )
            {
                if ( mintues >= 10 )
                    str += mintues;
                else
                    str += "0" + mintues;
            }
            else
                str = "00";

            return str;
        }



        public static void LogVector3( string str, Vector3 vec )
        { 
            Debug.Log( str + "[" + vec.x + "\t," + vec.y + "\t," + vec.z + "]" );
        }

	}
}























