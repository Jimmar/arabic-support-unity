#region File Description
//-----------------------------------------------------------------------------
/// <summary>
/// This is an Open Source File Created by: Abdullah Konash (http://abdullahkonash.com/) Twitter: @konash
/// This File allow the users to use arabic text in XNA and Unity platform.
/// It flips the characters and replace them with the appropriate ones to connect the letters in the correct way.
/// </summary>
//-----------------------------------------------------------------------------
#endregion


#region Using Statements

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace ArabicSupport
{
	
	public class ArabicFixer
	{	
		/// <summary>
		/// Fix the specified string.
		/// </summary>
		/// <param name="str">String to be fixed.</param>
		/// <returns>Fixed string</returns>
		public static string Fix(string str)
		{
			return Fix(str, false, true);
		}
		
		public static string Fix(string str, bool rtl)
		{
			if (rtl)
				return Fix(str);
			string[] words = str.Split(' ');
			string result = "";
			string arabicToIgnore = "";
			foreach(string word in words)
			{
				if(char.IsLower(word.ToLower()[word.Length/2]))
				{
					result += Fix(arabicToIgnore) + word + " ";
					arabicToIgnore = "";
				}
				else
				{
					arabicToIgnore += word + " ";
						
				}
			}
			if(arabicToIgnore != "")
				result += Fix(arabicToIgnore);
				
			return result;
		}
		
		/// <summary>
		/// Fix the specified string with customization options.
		/// </summary>
		/// <param name='str'>
		/// String to be fixed.
		/// </param>
		/// <param name='showTashkeel'>
		/// Show tashkeel.
		/// </param>
		/// <param name='useHinduNumbers'>
		/// Use hindu numbers.
		/// </param>
		public static string Fix(string str, bool showTashkeel, bool useHinduNumbers)
		{
			ArabicFixerTool.showTashkeel = showTashkeel;
			ArabicFixerTool.useHinduNumbers =useHinduNumbers;
			
			str = str.Replace("\n", Environment.NewLine);

			if (!str.Contains(Environment.NewLine)) 
				return ArabicFixerTool.FixLine(str);
			
			string[] stringSeparators = new string[] {Environment.NewLine};
			string[] strSplit = str.Split(stringSeparators, StringSplitOptions.None);

			switch (strSplit.Length)
			{
				case 0:
				case 1:
					return ArabicFixerTool.FixLine(str);
				default:
					StringBuilder outputString = new StringBuilder(ArabicFixerTool.FixLine(strSplit[0]));
					for (int iteration = 1; iteration < strSplit.Length; iteration++)
						outputString.Append(Environment.NewLine + ArabicFixerTool.FixLine(strSplit[iteration]));

					return outputString.ToString();
			}
		}
	}
}


/// <summary>
/// Arabic Contextual forms General - Unicode
/// </summary>
internal enum IsolatedArabicLetters
{
	Hamza = 0xFE80,
	Alef = 0xFE8D,
	AlefHamza = 0xFE83,
	WawHamza = 0xFE85,
	AlefMaksoor = 0xFE87,
	AlefMaksora = 0xFBFC,
	HamzaNabera = 0xFE89,
	Ba = 0xFE8F,
	Ta = 0xFE95,
	Tha2 = 0xFE99,
	Jeem = 0xFE9D,
	H7aa = 0xFEA1,
	Khaa2 = 0xFEA5,
	Dal = 0xFEA9,
	Thal = 0xFEAB,
	Ra2 = 0xFEAD,
	Zeen = 0xFEAF,
	Seen = 0xFEB1,
	Sheen = 0xFEB5,
	S9a = 0xFEB9,
	Dha = 0xFEBD,
	T6a = 0xFEC1,
	T6ha = 0xFEC5,
	Ain = 0xFEC9,
	Gain = 0xFECD,
	Fa = 0xFED1,
	Gaf = 0xFED5,
	Kaf = 0xFED9,
	Lam = 0xFEDD,
	Meem = 0xFEE1,
	Noon = 0xFEE5,
	Ha = 0xFEE9,
	Waw = 0xFEED,
	Ya = 0xFEF1,
	AlefMad = 0xFE81,
	TaMarboota = 0xFE93,
	PersianPe = 0xFB56,  	// Persian Letters;
	PersianChe = 0xFB7A,
	PersianZe = 0xFB8A,
	PersianGaf = 0xFB92,
	PersianGaf2 = 0xFB8E
}

/// <summary>
/// Arabic Contextual forms - Isolated
/// </summary>
internal enum GeneralArabicLetters
{
	Hamza = 0x0621,
	Alef = 0x0627,
	AlefHamza = 0x0623,
	WawHamza = 0x0624,
	AlefMaksoor = 0x0625,
	AlefMagsora = 0x0649,
	HamzaNabera = 0x0626,
	Ba = 0x0628,
	Ta = 0x062A,
	Tha2 = 0x062B,
	Jeem = 0x062C,
	H7aa = 0x062D,
	Khaa2 = 0x062E,
	Dal = 0x062F,
	Thal = 0x0630,
	Ra2 = 0x0631,
	Zeen = 0x0632,
	Seen = 0x0633,
	Sheen = 0x0634,
	S9a = 0x0635,
	Dha = 0x0636,
	T6a = 0x0637,
	T6ha = 0x0638,
	Ain = 0x0639,
	Gain = 0x063A,
	Fa = 0x0641,
	Gaf = 0x0642,
	Kaf = 0x0643,
	Lam = 0x0644,
	Meem = 0x0645,
	Noon = 0x0646,
	Ha = 0x0647,
	Waw = 0x0648,
	Ya = 0x064A,
	AlefMad = 0x0622,
	TaMarboota = 0x0629,
	PersianPe = 0x067E,		// Persian Letters;
	PersianChe = 0x0686,
	PersianZe = 0x0698,
	PersianGaf = 0x06AF,
	PersianGaf2 = 0x06A9
	
}


/// <summary>
/// Sets up and creates the conversion table 
/// </summary>
internal class ArabicTable
{
	//convert this into a list of dictionary
	private static Dictionary<int, int> mapList;
	private static ArabicTable arabicMapper;
	
	/// <summary>
	/// Setting up the conversion table
	/// </summary>
	private ArabicTable()
	{
		mapList = new Dictionary<int,int>()
		{
			{(int) GeneralArabicLetters.Hamza, (int) IsolatedArabicLetters.Hamza},
			{(int) GeneralArabicLetters.Alef, (int) IsolatedArabicLetters.Alef},
			{(int) GeneralArabicLetters.AlefHamza, (int) IsolatedArabicLetters.AlefHamza},
			{(int) GeneralArabicLetters.WawHamza, (int) IsolatedArabicLetters.WawHamza},
			{(int) GeneralArabicLetters.AlefMaksoor, (int) IsolatedArabicLetters.AlefMaksoor},
			{(int) GeneralArabicLetters.AlefMagsora, (int) IsolatedArabicLetters.AlefMaksora},
			{(int) GeneralArabicLetters.HamzaNabera, (int) IsolatedArabicLetters.HamzaNabera},
			{(int) GeneralArabicLetters.Ba, (int) IsolatedArabicLetters.Ba},
			{(int) GeneralArabicLetters.Ta, (int) IsolatedArabicLetters.Ta},
			{(int) GeneralArabicLetters.Tha2, (int) IsolatedArabicLetters.Tha2},
			{(int) GeneralArabicLetters.Jeem, (int) IsolatedArabicLetters.Jeem},
			{(int) GeneralArabicLetters.H7aa, (int) IsolatedArabicLetters.H7aa},
			{(int) GeneralArabicLetters.Khaa2, (int) IsolatedArabicLetters.Khaa2},
			{(int) GeneralArabicLetters.Dal, (int) IsolatedArabicLetters.Dal},
			{(int) GeneralArabicLetters.Thal, (int) IsolatedArabicLetters.Thal},
			{(int) GeneralArabicLetters.Ra2, (int) IsolatedArabicLetters.Ra2},
			{(int) GeneralArabicLetters.Zeen, (int) IsolatedArabicLetters.Zeen},
			{(int) GeneralArabicLetters.Seen, (int) IsolatedArabicLetters.Seen},
			{(int) GeneralArabicLetters.Sheen, (int) IsolatedArabicLetters.Sheen},
			{(int) GeneralArabicLetters.S9a, (int) IsolatedArabicLetters.S9a},
			{(int) GeneralArabicLetters.Dha, (int) IsolatedArabicLetters.Dha},
			{(int) GeneralArabicLetters.T6a, (int) IsolatedArabicLetters.T6a},
			{(int) GeneralArabicLetters.T6ha, (int) IsolatedArabicLetters.T6ha},
			{(int) GeneralArabicLetters.Ain, (int) IsolatedArabicLetters.Ain},
			{(int) GeneralArabicLetters.Gain, (int) IsolatedArabicLetters.Gain},
			{(int) GeneralArabicLetters.Fa, (int) IsolatedArabicLetters.Fa},
			{(int) GeneralArabicLetters.Gaf, (int) IsolatedArabicLetters.Gaf},
			{(int) GeneralArabicLetters.Kaf, (int) IsolatedArabicLetters.Kaf},
			{(int) GeneralArabicLetters.Lam, (int) IsolatedArabicLetters.Lam},
			{(int) GeneralArabicLetters.Meem, (int) IsolatedArabicLetters.Meem},
			{(int) GeneralArabicLetters.Noon, (int) IsolatedArabicLetters.Noon},
			{(int) GeneralArabicLetters.Ha, (int) IsolatedArabicLetters.Ha},
			{(int) GeneralArabicLetters.Waw, (int) IsolatedArabicLetters.Waw},
			{(int) GeneralArabicLetters.Ya, (int) IsolatedArabicLetters.Ya},
			{(int) GeneralArabicLetters.AlefMad, (int) IsolatedArabicLetters.AlefMad},
			{(int) GeneralArabicLetters.TaMarboota, (int) IsolatedArabicLetters.TaMarboota},
			{(int) GeneralArabicLetters.PersianPe, (int) IsolatedArabicLetters.PersianPe},    // Persian Letters;
			{(int) GeneralArabicLetters.PersianChe, (int) IsolatedArabicLetters.PersianChe},
			{(int) GeneralArabicLetters.PersianZe, (int) IsolatedArabicLetters.PersianZe},
			{(int) GeneralArabicLetters.PersianGaf, (int) IsolatedArabicLetters.PersianGaf},
			{(int) GeneralArabicLetters.PersianGaf2, (int) IsolatedArabicLetters.PersianGaf2}
		};

	}
	
	/// <summary>
	/// Singleton design pattern, Get the mapper. If it was not created before, create it.
	/// </summary>
	internal static ArabicTable ArabicMapper
	{
		get { return arabicMapper ?? (arabicMapper = new ArabicTable()); }
	}

	internal int Convert(int toBeConverted)
	{
		int value;
		return mapList.TryGetValue(toBeConverted, out value) ? value : toBeConverted;
	}
}


internal class ArabicFixerTool
{
	internal static bool showTashkeel = true;
	internal static bool useHinduNumbers;
	
	/// <summary>
	/// Returns a string without tashkeels and out their locations
	/// </summary>
	/// <param name="str">the original string</param>
	/// <param name="tashkeelLocation">removed tashkeels and their locations</param>
	/// <returns>string without tashkeel</returns>
	internal static string RemoveTashkeel(string str, out Dictionary<int, char> tashkeelLocation)
	{
		tashkeelLocation = new Dictionary<int, char>();
		
		// a hashset of all the tashkeels
		HashSet<char> tashkeels = new HashSet<char>()
		{
			(char)0x064B, // Tanween Fatha
			(char)0x064C, // Tanween Thamma
			(char)0x064D, // Tanween Kasra
			(char)0x064E, // Fatha
			(char)0x064F, // Thamma
			(char)0x0650, // Kasra
			(char)0x0651, // Shaddah
			(char)0x0652, // Sukoon
			(char)0x0653, // Madd
		};
		
		// looks for all occurances of any tashkeel and saves their indexes
		for (int i = 0; i < str.Length; i++)
			if(tashkeels.Contains(str[i]))
				tashkeelLocation.Add(i, str[i]);
		
		// splits the string on tashkeels location
		string[] split = str.Split(tashkeels.ToArray());
		
		//joins the splitted string to end up with a string without the tashkeels
		string stringWithoutTashkeel = string.Join("", split).Trim();
		return stringWithoutTashkeel;
	}
	
	/// <summary>
	/// Returns the tashkeel back into the letters array
	/// </summary>
	/// <param name="letters">the character array that got stripped out from tashkeel</param>
	/// <param name="tashkeelLocation">dictionary with the tashkeel location and their values</param>
	/// <returns>a string with all the tashkeels added back</returns>
	internal static string ReturnTashkeel(char[] letters, Dictionary<int, char> tashkeelLocation)
	{
		StringBuilder lettersWithTashkeel = new StringBuilder(letters.Length + tashkeelLocation.Count);
		int letterWithTashkeelTracker = 0;
		foreach (char t in letters)
		{
			lettersWithTashkeel.Append(t);
			letterWithTashkeelTracker++;
			char value;
			while (tashkeelLocation.TryGetValue(letterWithTashkeelTracker, out value))
			{
				lettersWithTashkeel.Append(value);
				letterWithTashkeelTracker++;
			}
		}
		
		return lettersWithTashkeel.ToString();
	}
	
	/// <summary>
	/// Converts a string to a form in which the sting will be displayed correctly for arabic text.
	/// </summary>
	/// <param name="str">String to be converted. Example: "Aaa"</param>
	/// <returns>Converted string. Example: "aa aaa A" without the spaces.</returns>
	internal static string FixLine(string str)
	{
//		string test = "";
		
		Dictionary<int, char> tashkeelLocation;
		
		string originString = RemoveTashkeel(str, out tashkeelLocation);
		
		char[] lettersOrigin = originString.ToCharArray();
		char[] lettersFinal = originString.ToCharArray();

		lettersOrigin = lettersOrigin.Select(t => (char) ArabicTable.ArabicMapper.Convert(t)).ToArray();
		
		//dictionary to convert the alefs that come after lam into proper alef-lam
		Dictionary<char, char> alefs = new Dictionary<char, char>()
		{
			{(char) IsolatedArabicLetters.AlefMaksoor,	(char) 0xFEF7},
			{(char) IsolatedArabicLetters.Alef,			(char) 0xFEF9},
			{(char) IsolatedArabicLetters.AlefHamza, 	(char) 0xFEF5},
			{(char) IsolatedArabicLetters.AlefMad, 		(char) 0xFEF3},
		};
		
		for (int i = 0; i < lettersOrigin.Length; i++)
		{
			bool skip = false;

			// For special Lam Letter connections.
			if (lettersOrigin[i] == (char)IsolatedArabicLetters.Lam)
			{		
				if (i < lettersOrigin.Length - 1)
				{
					char value;
					if (alefs.TryGetValue(lettersOrigin[i + 1], out value))
					{
						lettersOrigin[i] = value;
						lettersFinal[i + 1] = (char) 0xFFFF;
						skip = true;
					}
				}
			}
			
			if (!IsIgnoredCharacter(lettersOrigin[i]))
			{
				if (IsMiddleLetter(lettersOrigin, i))
					lettersFinal[i] = (char)(lettersOrigin[i] + 3);
				else if (IsFinishingLetter(lettersOrigin, i))
					lettersFinal[i] = (char)(lettersOrigin[i] + 1);
				else if (IsLeadingLetter(lettersOrigin, i))
					lettersFinal[i] = (char)(lettersOrigin[i] + 2);
			}

            //string strOut = String.Format(@"\x{0:x4}", (ushort)lettersOrigin[i]);
            //UnityEngine.Debug.Log(strOut);

            //strOut = String.Format(@"\x{0:x4}", (ushort)lettersFinal[i]);
            //UnityEngine.Debug.Log(strOut);

//            test += Convert.ToString(lettersOrigin[i], 16) + " ";
			if (skip)
				i++;
			
			
			//changing numbers to hindu
			if (useHinduNumbers)
			{
				//checks if the character is in the range of the numbers
				if (lettersOrigin[i] >= (char) 0x0030 && lettersOrigin[i] <= (char) 0x0039)
				{
					// convert the character by offsetting
					lettersFinal[i] = (char) (lettersOrigin[i] + (0x0660 - 0x0030));
				}
			}
		}
		
		//Return the Tashkeel to their places.
		if(showTashkeel)
			lettersFinal = ReturnTashkeel(lettersFinal, tashkeelLocation).ToCharArray();
		
		
		List<char> list = new List<char>();
		
		List<char> numberList = new List<char>();
		
		for (int i = lettersFinal.Length - 1; i >= 0; i--)
		{
			
			
			//				if (lettersFinal[i] == '(')
			//						numberList.Add(')');
			//				else if (lettersFinal[i] == ')')
			//					numberList.Add('(');
			//				else if (lettersFinal[i] == '<')
			//					numberList.Add('>');
			//				else if (lettersFinal[i] == '>')
			//					numberList.Add('<');
			//				else 
			if (char.IsPunctuation(lettersFinal[i]) && i>0 && i < lettersFinal.Length-1 &&
			    (char.IsPunctuation(lettersFinal[i-1]) || char.IsPunctuation(lettersFinal[i+1])))
			{
				if (lettersFinal[i] == '(')
					list.Add(')');
				else if (lettersFinal[i] == ')')
					list.Add('(');
				else if (lettersFinal[i] == '<')
					list.Add('>');
				else if (lettersFinal[i] == '>')
					list.Add('<');
				else if (lettersFinal[i] == '[')
					list.Add(']');
				else if (lettersFinal[i] == ']')
					list.Add('[');
				else if (lettersFinal[i] != 0xFFFF)
					list.Add(lettersFinal[i]);
			}
			// For cases where english words and arabic are mixed. This allows for using arabic, english and numbers in one sentence.
			else if(lettersFinal[i] == ' ' && i > 0 && i < lettersFinal.Length-1 &&
			        (char.IsLower(lettersFinal[i-1]) || char.IsUpper(lettersFinal[i-1]) || char.IsNumber(lettersFinal[i-1])) &&
			        (char.IsLower(lettersFinal[i+1]) || char.IsUpper(lettersFinal[i+1]) ||char.IsNumber(lettersFinal[i+1])))
				
			{
				numberList.Add(lettersFinal[i]);
			}
			
			else if (char.IsNumber(lettersFinal[i]) || char.IsLower(lettersFinal[i]) ||
			         char.IsUpper(lettersFinal[i]) || char.IsSymbol(lettersFinal[i]) ||
			         char.IsPunctuation(lettersFinal[i]))// || lettersFinal[i] == '^') //)
			{
				
				if (lettersFinal[i] == '(')
					numberList.Add(')');
				else if (lettersFinal[i] == ')')
					numberList.Add('(');
				else if (lettersFinal[i] == '<')
					numberList.Add('>');
				else if (lettersFinal[i] == '>')
					numberList.Add('<');
				else if (lettersFinal[i] == '[')
					list.Add(']');
				else if (lettersFinal[i] == ']')
					list.Add('[');
				else
					numberList.Add(lettersFinal[i]);
			}
			else if( (lettersFinal[i] >= (char)0xD800 && lettersFinal[i] <= (char)0xDBFF) ||
			        (lettersFinal[i] >= (char)0xDC00 && lettersFinal[i] <= (char)0xDFFF))
			{
				numberList.Add(lettersFinal[i]);
			}
			else
			{
				if (numberList.Count > 0)
				{
					for (int j = 0; j < numberList.Count; j++)
						list.Add(numberList[numberList.Count - 1 - j]);
					numberList.Clear();
				}
				if (lettersFinal[i] != 0xFFFF)
					list.Add(lettersFinal[i]);
				
			}
		}
		if (numberList.Count > 0)
		{
			for (int j = 0; j < numberList.Count; j++)
				list.Add(numberList[numberList.Count - 1 - j]);
			numberList.Clear();
		}
		
		// Moving letters from a list to an array.
		lettersFinal = new char[list.Count];
		for (int i = 0; i < lettersFinal.Length; i++)
			lettersFinal[i] = list[i];
		
		
		str = new string(lettersFinal);
		return str;
	}
	
	/// <summary>
	/// English letters, numbers and punctuation characters are ignored. This checks if the ch is an ignored character.
	/// </summary>
	/// <param name="ch">The character to be checked for skipping</param>
	/// <returns>True if the character should be ignored, false if it should not be ignored.</returns>
	internal static bool IsIgnoredCharacter(char ch)
	{
		bool isPunctuation = char.IsPunctuation(ch);
		bool isNumber = char.IsNumber(ch);
		bool isLower = char.IsLower(ch);
		bool isUpper = char.IsUpper(ch);
		bool isSymbol = char.IsSymbol(ch);
		bool isPersianCharacter = ch == (char)0xFB56 || ch == (char)0xFB7A || ch == (char)0xFB8A || ch == (char)0xFB92 || ch == (char)0xFB8E;
        bool isPresentationFormB = (ch <= (char)0xFEFF && ch >= (char)0xFE70);
        bool isAcceptableCharacter = isPresentationFormB || isPersianCharacter || ch == (char)0xFBFC;



        return isPunctuation ||
            isNumber ||
                isLower ||
                isUpper ||
                isSymbol ||
                !isAcceptableCharacter ||
                ch == 'a' || ch == '>' || ch == '<' || ch == (char)0x061B;
		
		//            return char.IsPunctuation(ch) || char.IsNumber(ch) || ch == 'a' || ch == '>' || ch == '<' ||
		//                    char.IsLower(ch) || char.IsUpper(ch) || ch == (char)0x061B || char.IsSymbol(ch)
		//					|| !(ch <= (char)0xFEFF && ch >= (char)0xFE70) // Presentation Form B
		//					|| ch == (char)0xFB56 || ch == (char)0xFB7A || ch == (char)0xFB8A || ch == (char)0xFB92; // Persian Characters
		
		//					PersianPe = 0xFB56,
		//		PersianChe = 0xFB7A,
		//		PersianZe = 0xFB8A,
		//		PersianGaf = 0xFB92
		//lettersOrigin[i] <= (char)0xFEFF && lettersOrigin[i] >= (char)0xFE70
	}
	
	/// <summary>
	/// Checks if the letter at index value is a leading character in Arabic or not.
	/// </summary>
	/// <param name="letters">The whole word that contains the character to be checked</param>
	/// <param name="index">The index of the character to be checked</param>
	/// <returns>True if the character at index is a leading character, else, returns false</returns>
	internal static bool IsLeadingLetter(char[] letters, int index)
	{

		bool lettersThatCannotBeBeforeALeadingLetter = index == 0 
			|| letters[index - 1] == ' ' 
				|| letters[index - 1] == '*' // ??? Remove?
				|| letters[index - 1] == 'A' // ??? Remove?
				|| char.IsPunctuation(letters[index - 1])
				|| letters[index - 1] == '>' 
				|| letters[index - 1] == '<' 
				|| letters[index - 1] == (int)IsolatedArabicLetters.Alef
				|| letters[index - 1] == (int)IsolatedArabicLetters.Dal 
				|| letters[index - 1] == (int)IsolatedArabicLetters.Thal
				|| letters[index - 1] == (int)IsolatedArabicLetters.Ra2 
				|| letters[index - 1] == (int)IsolatedArabicLetters.Zeen 
				|| letters[index - 1] == (int)IsolatedArabicLetters.PersianZe
				//|| letters[index - 1] == (int)IsolatedArabicLetters.AlefMaksora 
				|| letters[index - 1] == (int)IsolatedArabicLetters.Waw
				|| letters[index - 1] == (int)IsolatedArabicLetters.AlefMad 
				|| letters[index - 1] == (int)IsolatedArabicLetters.AlefHamza
				|| letters[index - 1] == (int)IsolatedArabicLetters.AlefMaksoor 
				|| letters[index - 1] == (int)IsolatedArabicLetters.WawHamza;

		bool lettersThatCannotBeALeadingLetter = letters[index] != ' ' 
			&& letters[index] != (int)IsolatedArabicLetters.Dal
			&& letters[index] != (int)IsolatedArabicLetters.Thal
				&& letters[index] != (int)IsolatedArabicLetters.Ra2 
				&& letters[index] != (int)IsolatedArabicLetters.Zeen 
				&& letters[index] != (int)IsolatedArabicLetters.PersianZe
				&& letters[index] != (int)IsolatedArabicLetters.Alef 
				&& letters[index] != (int)IsolatedArabicLetters.AlefHamza
				&& letters[index] != (int)IsolatedArabicLetters.AlefMaksoor
				&& letters[index] != (int)IsolatedArabicLetters.AlefMad
				&& letters[index] != (int)IsolatedArabicLetters.WawHamza
				&& letters[index] != (int)IsolatedArabicLetters.Waw
				&& letters[index] != (int)IsolatedArabicLetters.Hamza;

		bool lettersThatCannotBeAfterLeadingLetter = index < letters.Length - 1 
			&& letters[index + 1] != ' '
				&& !char.IsPunctuation(letters[index + 1] )
				&& !char.IsNumber(letters[index + 1])
				&& !char.IsSymbol(letters[index + 1])
				&& !char.IsLower(letters[index + 1])
				&& !char.IsUpper(letters[index + 1])
				&& letters[index + 1] != (int)IsolatedArabicLetters.Hamza;

		if(lettersThatCannotBeBeforeALeadingLetter && lettersThatCannotBeALeadingLetter && lettersThatCannotBeAfterLeadingLetter)

//		if ((index == 0 || letters[index - 1] == ' ' || letters[index - 1] == '*' || letters[index - 1] == 'A' || char.IsPunctuation(letters[index - 1])
//		     || letters[index - 1] == '>' || letters[index - 1] == '<' 
//		     || letters[index - 1] == (int)IsolatedArabicLetters.Alef
//		     || letters[index - 1] == (int)IsolatedArabicLetters.Dal || letters[index - 1] == (int)IsolatedArabicLetters.Thal
//		     || letters[index - 1] == (int)IsolatedArabicLetters.Ra2 
//		     || letters[index - 1] == (int)IsolatedArabicLetters.Zeen || letters[index - 1] == (int)IsolatedArabicLetters.PersianZe
//		     || letters[index - 1] == (int)IsolatedArabicLetters.AlefMaksora || letters[index - 1] == (int)IsolatedArabicLetters.Waw
//		     || letters[index - 1] == (int)IsolatedArabicLetters.AlefMad || letters[index - 1] == (int)IsolatedArabicLetters.AlefHamza
//		     || letters[index - 1] == (int)IsolatedArabicLetters.AlefMaksoor || letters[index - 1] == (int)IsolatedArabicLetters.WawHamza) 
//		    && letters[index] != ' ' && letters[index] != (int)IsolatedArabicLetters.Dal
//		    && letters[index] != (int)IsolatedArabicLetters.Thal
//		    && letters[index] != (int)IsolatedArabicLetters.Ra2 
//		    && letters[index] != (int)IsolatedArabicLetters.Zeen && letters[index] != (int)IsolatedArabicLetters.PersianZe
//		    && letters[index] != (int)IsolatedArabicLetters.Alef && letters[index] != (int)IsolatedArabicLetters.AlefHamza
//		    && letters[index] != (int)IsolatedArabicLetters.AlefMaksoor
//		    && letters[index] != (int)IsolatedArabicLetters.AlefMad
//		    && letters[index] != (int)IsolatedArabicLetters.WawHamza
//		    && letters[index] != (int)IsolatedArabicLetters.Waw
//		    && letters[index] != (int)IsolatedArabicLetters.Hamza
//		    && index < letters.Length - 1 && letters[index + 1] != ' ' && !char.IsPunctuation(letters[index + 1] ) && !char.IsNumber(letters[index + 1])
//		    && letters[index + 1] != (int)IsolatedArabicLetters.Hamza )
		{
			return true;
		}
		else
			return false;
	}
	
	/// <summary>
	/// Checks if the letter at index value is a finishing character in Arabic or not.
	/// </summary>
	/// <param name="letters">The whole word that contains the character to be checked</param>
	/// <param name="index">The index of the character to be checked</param>
	/// <returns>True if the character at index is a finishing character, else, returns false</returns>
	internal static bool IsFinishingLetter(char[] letters, int index)
	{
		bool indexZero = index != 0;
		bool lettersThatCannotBeBeforeAFinishingLetter = (index == 0) ? false : 
				letters[index - 1] != ' '
//				&& char.IsDigit(letters[index-1])
//				&& char.IsLower(letters[index-1])
//				&& char.IsUpper(letters[index-1])
//				&& char.IsNumber(letters[index-1])
//				&& char.IsWhiteSpace(letters[index-1])
//				&& char.IsPunctuation(letters[index-1])
//				&& char.IsSymbol(letters[index-1])

				&& letters[index - 1] != (int)IsolatedArabicLetters.Dal 
				&& letters[index - 1] != (int)IsolatedArabicLetters.Thal
				&& letters[index - 1] != (int)IsolatedArabicLetters.Ra2 
				&& letters[index - 1] != (int)IsolatedArabicLetters.Zeen 
				&& letters[index - 1] != (int)IsolatedArabicLetters.PersianZe
				//&& letters[index - 1] != (int)IsolatedArabicLetters.AlefMaksora 
				&& letters[index - 1] != (int)IsolatedArabicLetters.Waw
				&& letters[index - 1] != (int)IsolatedArabicLetters.Alef 
				&& letters[index - 1] != (int)IsolatedArabicLetters.AlefMad
				&& letters[index - 1] != (int)IsolatedArabicLetters.AlefHamza 
				&& letters[index - 1] != (int)IsolatedArabicLetters.AlefMaksoor
				&& letters[index - 1] != (int)IsolatedArabicLetters.WawHamza 
				&& letters[index - 1] != (int)IsolatedArabicLetters.Hamza



				&& !char.IsPunctuation(letters[index - 1]) 
				&& letters[index - 1] != '>' 
				&& letters[index - 1] != '<';
				

		bool lettersThatCannotBeFinishingLetters = letters[index] != ' ' && letters[index] != (int)IsolatedArabicLetters.Hamza;

	


		if(lettersThatCannotBeBeforeAFinishingLetter && lettersThatCannotBeFinishingLetters)

//		if (index != 0 && letters[index - 1] != ' ' && letters[index - 1] != '*' && letters[index - 1] != 'A'
//		    && letters[index - 1] != (int)IsolatedArabicLetters.Dal && letters[index - 1] != (int)IsolatedArabicLetters.Thal
//		    && letters[index - 1] != (int)IsolatedArabicLetters.Ra2 
//		    && letters[index - 1] != (int)IsolatedArabicLetters.Zeen && letters[index - 1] != (int)IsolatedArabicLetters.PersianZe
//		    && letters[index - 1] != (int)IsolatedArabicLetters.AlefMaksora && letters[index - 1] != (int)IsolatedArabicLetters.Waw
//		    && letters[index - 1] != (int)IsolatedArabicLetters.Alef && letters[index - 1] != (int)IsolatedArabicLetters.AlefMad
//		    && letters[index - 1] != (int)IsolatedArabicLetters.AlefHamza && letters[index - 1] != (int)IsolatedArabicLetters.AlefMaksoor
//		    && letters[index - 1] != (int)IsolatedArabicLetters.WawHamza && letters[index - 1] != (int)IsolatedArabicLetters.Hamza 
//		    && !char.IsPunctuation(letters[index - 1]) && letters[index - 1] != '>' && letters[index - 1] != '<' 
//		    && letters[index] != ' ' && index < letters.Length
//		    && letters[index] != (int)IsolatedArabicLetters.Hamza)
		{
			//try
			//{
			//    if (char.IsPunctuation(letters[index + 1]))
			//        return true;
			//    else
			//        return false;
			//}
			//catch (Exception e)
			//{
			//    return false;
			//}
			
			return true;
		}
		//return true;
		else
			return false;
	}
	
	/// <summary>
	/// Checks if the letter at index value is a middle character in Arabic or not.
	/// </summary>
	/// <param name="letters">The whole word that contains the character to be checked</param>
	/// <param name="index">The index of the character to be checked</param>
	/// <returns>True if the character at index is a middle character, else, returns false</returns>
	internal static bool IsMiddleLetter(char[] letters, int index)
	{
		bool lettersThatCannotBeMiddleLetters = (index == 0) ? false : 
			letters[index] != (int)IsolatedArabicLetters.Alef 
				&& letters[index] != (int)IsolatedArabicLetters.Dal
				&& letters[index] != (int)IsolatedArabicLetters.Thal 
				&& letters[index] != (int)IsolatedArabicLetters.Ra2
				&& letters[index] != (int)IsolatedArabicLetters.Zeen 
				&& letters[index] != (int)IsolatedArabicLetters.PersianZe 
				//&& letters[index] != (int)IsolatedArabicLetters.AlefMaksora
				&& letters[index] != (int)IsolatedArabicLetters.Waw 
				&& letters[index] != (int)IsolatedArabicLetters.AlefMad
				&& letters[index] != (int)IsolatedArabicLetters.AlefHamza 
				&& letters[index] != (int)IsolatedArabicLetters.AlefMaksoor
				&& letters[index] != (int)IsolatedArabicLetters.WawHamza 
				&& letters[index] != (int)IsolatedArabicLetters.Hamza;

		bool lettersThatCannotBeBeforeMiddleCharacters = (index == 0) ? false :
				letters[index - 1] != (int)IsolatedArabicLetters.Alef 
				&& letters[index - 1] != (int)IsolatedArabicLetters.Dal
				&& letters[index - 1] != (int)IsolatedArabicLetters.Thal 
				&& letters[index - 1] != (int)IsolatedArabicLetters.Ra2
				&& letters[index - 1] != (int)IsolatedArabicLetters.Zeen 
				&& letters[index - 1] != (int)IsolatedArabicLetters.PersianZe 
				//&& letters[index - 1] != (int)IsolatedArabicLetters.AlefMaksora
				&& letters[index - 1] != (int)IsolatedArabicLetters.Waw 
				&& letters[index - 1] != (int)IsolatedArabicLetters.AlefMad
				&& letters[index - 1] != (int)IsolatedArabicLetters.AlefHamza 
				&& letters[index - 1] != (int)IsolatedArabicLetters.AlefMaksoor
				&& letters[index - 1] != (int)IsolatedArabicLetters.WawHamza 
				&& letters[index - 1] != (int)IsolatedArabicLetters.Hamza
				&& !char.IsPunctuation(letters[index - 1])
				&& letters[index - 1] != '>' 
				&& letters[index - 1] != '<' 
				&& letters[index - 1] != ' ' 
				&& letters[index - 1] != '*';

		bool lettersThatCannotBeAfterMiddleCharacters = (index >= letters.Length - 1) ? false :
			letters[index + 1] != ' ' 
				&& letters[index + 1] != '\r' 
				&& letters[index + 1] != (int)IsolatedArabicLetters.Hamza
				&& !char.IsNumber(letters[index + 1])
				&& !char.IsSymbol(letters[index + 1])
				&& !char.IsPunctuation(letters[index + 1]);
		if(lettersThatCannotBeAfterMiddleCharacters && lettersThatCannotBeBeforeMiddleCharacters && lettersThatCannotBeMiddleLetters)

//		if (index != 0 && letters[index] != ' '
//		    && letters[index] != (int)IsolatedArabicLetters.Alef && letters[index] != (int)IsolatedArabicLetters.Dal
//		    && letters[index] != (int)IsolatedArabicLetters.Thal && letters[index] != (int)IsolatedArabicLetters.Ra2
//		    && letters[index] != (int)IsolatedArabicLetters.Zeen && letters[index] != (int)IsolatedArabicLetters.PersianZe 
//		    && letters[index] != (int)IsolatedArabicLetters.AlefMaksora
//		    && letters[index] != (int)IsolatedArabicLetters.Waw && letters[index] != (int)IsolatedArabicLetters.AlefMad
//		    && letters[index] != (int)IsolatedArabicLetters.AlefHamza && letters[index] != (int)IsolatedArabicLetters.AlefMaksoor
//		    && letters[index] != (int)IsolatedArabicLetters.WawHamza && letters[index] != (int)IsolatedArabicLetters.Hamza
//		    && letters[index - 1] != (int)IsolatedArabicLetters.Alef && letters[index - 1] != (int)IsolatedArabicLetters.Dal
//		    && letters[index - 1] != (int)IsolatedArabicLetters.Thal && letters[index - 1] != (int)IsolatedArabicLetters.Ra2
//		    && letters[index - 1] != (int)IsolatedArabicLetters.Zeen && letters[index - 1] != (int)IsolatedArabicLetters.PersianZe 
//		    && letters[index - 1] != (int)IsolatedArabicLetters.AlefMaksora
//		    && letters[index - 1] != (int)IsolatedArabicLetters.Waw && letters[index - 1] != (int)IsolatedArabicLetters.AlefMad
//		    && letters[index - 1] != (int)IsolatedArabicLetters.AlefHamza && letters[index - 1] != (int)IsolatedArabicLetters.AlefMaksoor
//		    && letters[index - 1] != (int)IsolatedArabicLetters.WawHamza && letters[index - 1] != (int)IsolatedArabicLetters.Hamza 
//		    && letters[index - 1] != '>' && letters[index - 1] != '<' 
//		    && letters[index - 1] != ' ' && letters[index - 1] != '*' && !char.IsPunctuation(letters[index - 1])
//		    && index < letters.Length - 1 && letters[index + 1] != ' ' && letters[index + 1] != '\r' && letters[index + 1] != 'A' 
//		    && letters[index + 1] != '>' && letters[index + 1] != '>' && letters[index + 1] != (int)IsolatedArabicLetters.Hamza
//		    )
		{
			try
			{
				if (char.IsPunctuation(letters[index + 1]))
					return false;
				else
					return true;
			}
			catch
			{
				return false;
			}
			//return true;
		}
		else
			return false;
	}
	
	
}