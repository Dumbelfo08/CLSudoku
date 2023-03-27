using System;
using Pastel;
using System.Drawing;

class Program
{
	static int remNum = 56;
	
	static int Xpos = 0;
	static int Ypos = 0;
	static int rPap = 0;
	static int gPap = 175;
	static int bPap = 255;
	static List<string> warns = new List<string>();
	static List<string> messages = new List<string>();
	static int[,] OgTablero = new int[9,9];
	static bool hasMadeChanges = false;
	
    static void Main()
    {
        string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
		if (!Directory.Exists(appdata + @"\ashproject")) {Directory.CreateDirectory(appdata + @"\ashproject");}
		if (!Directory.Exists(appdata + @"\ashproject\clsudoku")) {Directory.CreateDirectory(appdata + @"\ashproject\clsudoku");}
		if (!File.Exists(appdata + @"\ashproject\clsudoku\save.ash")) {File.WriteAllText(appdata + @"\ashproject\clsudoku\save.ash", "");}
		
		Console.WriteLine("CLSudoku made by Dumbelfo. Version 2.0".Pastel(Color.FromArgb(rPap, gPap, bPap)));
		int[,] tablero = GenerarSudoku();
		tablero = loadSudoku();

        while (!JuegoCompletado(tablero))
        {

			ImprimirSudoku(tablero); 
			// Capturar la entrada de teclado del usuario
    ConsoleKeyInfo keyInfo = Console.ReadKey(true);

    // Mover la selección en función de la tecla presionada
    switch (keyInfo.Key)
    {
        case ConsoleKey.UpArrow:
            if (Ypos > 0)
            {
                Ypos--;
            }
            break;
        case ConsoleKey.DownArrow:
            if (Ypos < 8)
			{
				Ypos++;
			}
			break;
		case ConsoleKey.LeftArrow:
            if (Xpos > 0)
            {
                Xpos--;
            }
            break;
        case ConsoleKey.RightArrow:
            if (Xpos < 8)
			{
				Xpos++;
			}
			break;
			case ConsoleKey.W:
            if (Ypos > 0)
            {
                Ypos--;
            }
            break;
        case ConsoleKey.S:
            if (Ypos < 8)
			{
				Ypos++;
			}
			break;
		case ConsoleKey.A:
            if (Xpos > 0)
            {
                Xpos--;
            }
            break;
        case ConsoleKey.D:
            if (Xpos < 8)
			{
				Xpos++;
			}
			break;
		case ConsoleKey.D1:
            tablero = setValue(tablero, Xpos, Ypos, 1);
			break;
		case ConsoleKey.D2:
            tablero = setValue(tablero, Xpos, Ypos, 2);
			break;
		case ConsoleKey.D3:
            tablero = setValue(tablero, Xpos, Ypos, 3);
			break;
		case ConsoleKey.D4:
            tablero = setValue(tablero, Xpos, Ypos, 4);
			break;
		case ConsoleKey.D5:
            tablero = setValue(tablero, Xpos, Ypos, 5);
			break;
		case ConsoleKey.D6:
            tablero = setValue(tablero, Xpos, Ypos, 6);
			break;
		case ConsoleKey.D7:
            tablero = setValue(tablero, Xpos, Ypos, 7);
			break;
		case ConsoleKey.D8:
            tablero = setValue(tablero, Xpos, Ypos, 8);
			break;
		case ConsoleKey.D9:
            tablero = setValue(tablero, Xpos, Ypos, 9);
			break;
		case ConsoleKey.R:
            tablero = delValue(tablero, Xpos, Ypos);
			break;
		case ConsoleKey.N:
			if (!hasMadeChanges){
				tablero = GenerarSudoku();
				saveSudoku(tablero);
				Xpos = 0;
				Ypos = 0;
			} else{
				Console.Write("  Are you sure you want to generate a new sudoku? All changes will be deleted(Y/N): ".Pastel(Color.FromArgb(rPap, gPap, bPap)));
				string y = Console.ReadLine();
				if (y=="Y" || y=="y"){
					tablero = GenerarSudoku();
					saveSudoku(tablero);
					Xpos = 0;
					Ypos = 0;
					hasMadeChanges = false;
				}
			}
			break;
		case ConsoleKey.Q:
			saveSudoku(tablero);
			break;
		case ConsoleKey.X:
            Environment.Exit(0);
			break;
		
	}
		Console.Clear();
        }

        Console.WriteLine("You completed the sudoku!".Pastel(Color.FromArgb(rPap, gPap, bPap)));
		Console.ReadKey();
    }

    static int[,] setValue(int[,] tablero, int columna, int fila, int input){

            if (!hasMadeChanges){
				hasMadeChanges = true;
			}
			
			if (tablero[fila, columna] != 0)
            {
				warns.Add("The cell is occupied. Try again");
                return tablero;
            }


            if (!EsValido(tablero, fila, columna, input))
            {
				warns.Add("The value is not correct. Try again");
                return tablero;
            }

            tablero[fila, columna] = input;
			return tablero;
	}
	
	static int[,] delValue(int[,] tablero, int columna, int fila){

            if (tablero[fila, columna] == 0)
            {
				warns.Add("The cell is not occupied");
                return tablero;
            }


            if (OgTablero[fila, columna]!=0)
            {
				warns.Add("The value can't be changed. Try again");
                return tablero;
            }

            tablero[fila, columna] = 0;
			return tablero;
	}
	
	static int[,] GenerarSudoku()
{
    int[,] tablero = new int[9, 9];

    // Generar el sudoku completo
    GenerarSudokuCompleto(tablero);

    // Remover algunos valores aleatorios
    Random rnd = new Random();
    for (int i = 0; i < remNum; i++)
    {
        int fila = rnd.Next(9);
        int columna = rnd.Next(9);
		if (tablero[fila, columna] != 0){
			tablero[fila, columna] = 0;
		} else {i--;}
        
    }
	OgTablero = tablero.Clone() as int[,];
    return tablero;
}

static bool GenerarSudokuCompleto(int[,] tablero)
{
    int fila = -1;
    int columna = -1;
    bool vacia = true;

    // Buscar la siguiente celda vacía
    for (int i = 0; i < 9; i++)
    {
        for (int j = 0; j < 9; j++)
        {
            if (tablero[i, j] == 0)
            {
                fila = i;
                columna = j;

                // Encontrar una celda vacía significa que el tablero aún no está completo
                vacia = false;
                break;
            }
        }

        if (!vacia)
        {
            break;
        }
    }

    // Si no se encontró ninguna celda vacía, el tablero está completo
    if (vacia)
    {
        return true;
    }

    // Crear una lista de valores del 1 al 9 y mezclarla aleatoriamente
    List<int> valores = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
    Random rnd = new Random();
    valores = valores.OrderBy(x => rnd.Next()).ToList();

    // Intentar colocar un valor aleatorio en la celda vacía
    foreach (int valor in valores)
    {
        if (EsValido(tablero, fila, columna, valor))
        {
            tablero[fila, columna] = valor;

            // Llamada recursiva para llenar las celdas vacías restantes
            if (GenerarSudokuCompleto(tablero))
            {
                return true;
            }

            // Si la llamada recursiva falló, se debe deshacer el cambio y probar otro valor
            tablero[fila, columna] = 0;
        }
    }

    // Si ningún valor encaja en la celda vacía, el tablero es inválido
    return false;
}

    static bool EsValido(int[,] tablero, int fila, int columna, int valor)
    {
        for (int i = 0; i < 9; i++)
        {
            if (tablero[fila, i] == valor || tablero[i, columna] == valor || tablero[(fila / 3) * 3 + i / 3, (columna / 3) * 3 + i % 3] == valor)
            {
                return false;
            }
        }

        return true;
    }

    static bool JuegoCompletado(int[,] tablero)
    {
        for (int fila = 0; fila < 9; fila++)
        {
            for (int columna = 0; columna < 9; columna++)
            {
                if (tablero[fila, columna] == 0)
                {
                    return false;
                }
            }
        }

        return true;
    }
	
	static void saveSudoku(int[,] tablero){
		string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
		string fin = "";
		for (int fila = 0; fila < 9; fila++){
			for (int columna = 0; columna < 9; columna++){
				fin = fin+OgTablero[fila,columna]+".";
			}
		}
		fin = fin+";";
		for (int fila = 0; fila < 9; fila++){
			for (int columna = 0; columna < 9; columna++){
				fin = fin+tablero[fila,columna]+".";
			}
		}
		File.WriteAllText(appdata + @"\ashproject\clsudoku\save.ash",fin);
		messages.Add("Game saved!");
	}
	static int[,] loadSudoku(){
		string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
		int[,] tablero = new int[9,9];
		List<string> file = File.ReadLines(appdata + @"\ashproject\clsudoku\save.ash").ToList();
		string[] bth = file[0].Split(";");
		string[] bto = bth[0].Split(".");
		string[] btn = bth[1].Split(".");
		for (int fila = 0; fila < 9; fila++){
			for (int columna = 0; columna < 9; columna++){
				OgTablero[columna,fila]= Int32.Parse(bto[(columna * 9)+fila]);
			}
		}
		for (int fila = 0; fila < 9; fila++){
			for (int columna = 0; columna < 9; columna++){
				tablero[columna,fila]= Int32.Parse(btn[(columna * 9)+fila]);
			}
		}
		saveSudoku(tablero);
		hasMadeChanges = true;
		return tablero;
	}

    static void ImprimirSudoku(int[,] tablero)
    {
		//Console.Clear();
		Console.WriteLine("");
		Console.WriteLine(" ╔═══════╦═══════╦═══════╗");
		for (int fila = 0; fila < 9; fila++)
        {
            if (fila % 3 == 0 && fila != 0 && fila==6)
            {
				Console.WriteLine(" ╠═══════╬═══════╬═══════╣            Q: Save");
				Console.Write(" ");
            }else if (fila % 3 == 0 && fila != 0 && fila==3)
            {
				Console.WriteLine(" ╠═══════╬═══════╬═══════╣            Arrows/WASD: Move selection");
				Console.Write(" ");
            } else {
				Console.Write(" ");
			}
			

            for (int columna = 0; columna < 9; columna++)
            {
                
				
				if (columna % 3 == 0)
                {
					Console.Write("║ ");
                }
				
				if (tablero[fila, columna] == 0 && fila == Ypos && columna == Xpos){
					Console.Write("· ".Pastel(Color.Yellow));
				} 
				else if (tablero[fila, columna] == 0){
					Console.Write("· ".Pastel(Color.FromArgb(rPap, gPap, bPap)));
				} 
				else if (fila == Ypos && columna == Xpos) {
					Console.Write((tablero[fila, columna]+" ").Pastel(Color.Yellow));
				} 
				else if(OgTablero[fila, columna]==0){
					Console.Write((tablero[fila, columna]+" ").Pastel(Color.FromArgb(186, 234, 255)));
				}
				else {
					Console.Write((tablero[fila, columna]+" "));
				}
				
				if (columna == 8){
					Console.Write("║");
				}
            }

             if (fila == 3){
				Console.WriteLine("            Numbers(1-9): Set if blank");
			}else if (fila == 4){
				Console.WriteLine("            R: Remove number");
			}else if (fila == 5){
				Console.WriteLine("            N: New sudoku");
			}else if (fila == 6){
				Console.WriteLine("            X: Exit");
			}else{
				Console.WriteLine("");
			}
        }
		Console.WriteLine(" ╚═══════╩═══════╩═══════╝");
        Console.WriteLine();
		foreach (string a in warns){
			Console.WriteLine("  "+a.Pastel(Color.DarkRed));
		}
		warns.Clear();
		Console.WriteLine("");
		foreach (string a in messages){
			Console.WriteLine("  "+a.Pastel(Color.FromArgb(rPap, gPap, bPap)));
		}
		messages.Clear();
    }
}